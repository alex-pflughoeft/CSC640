using BLUE.ChocAn.Library.Commands;
using BLUE.ChocAn.Library.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BLUE.ChocAn.Library
{
    public class Terminal
    {
        #region Private Variables

        private string _readPrompt;
        private string _commandNamespace;
        private Dictionary<string, Dictionary<string, IEnumerable<ParameterInfo>>> _commandLibraries;
        private CommandRunner _commandRunner;

        #endregion

        #region Constructors

        public Terminal(string readPrompt, string commandNameSpace)
        {
            this._readPrompt = readPrompt;
            this._commandNamespace = commandNameSpace;
            this.HistoricalQueue = new Queue<string>();
            this.CurrentUser = new User();

            this._commandLibraries = new Dictionary<string, Dictionary<string, IEnumerable<ParameterInfo>>>();

            var q = from t in Assembly.GetExecutingAssembly().GetTypes()
                    where t.IsClass && t.Namespace == this._commandNamespace
                    select t;
            var commandClasses = q.ToList();

            foreach (var commandClass in commandClasses)
            {
                // Load the method info from each class into a dictionary:
                var methods = commandClass.GetMethods();
                var methodDictionary = new Dictionary<string, IEnumerable<ParameterInfo>>();

                foreach (var method in methods)
                {
                    string commandName = method.Name;
                    methodDictionary.Add(commandName, method.GetParameters());
                }

                // Add the dictionary of methods for the current class into a dictionary of command classes:
                this._commandLibraries.Add(commandClass.Name, methodDictionary);
            }

            this._commandRunner = new CommandRunner(this);
        }

        #endregion

        #region Public Properties

        public Dictionary<string, Dictionary<string, IEnumerable<ParameterInfo>>> CommandLibraries { get { return this._commandLibraries; } }
        public User CurrentUser { get; set; }
        public Queue<string> HistoricalQueue { get; private set; }
        public string TerminalName { get { return "ChocAn Terminal v1.0.0"; } }

        #endregion

        #region Public Methods

        public void Run()
        {
            this._commandRunner.reboot();

            while (true)
            {
                var consoleInput = this.ReadTermInput();
                if (string.IsNullOrWhiteSpace(consoleInput)) continue;

                try
                {
                    this.HistoricalQueue.Enqueue(consoleInput);
                    var command = new ConsoleCommand(consoleInput);
                    string result = Execute(command);
                    this.WriteConsoleOutput(result);
                }
                catch (Exception e)
                {
                    this.WriteConsoleOutput(e.Message);
                }
            }
        }

        public void PrintHistory()
        {
            int initialCount = this.HistoricalQueue.Count;

            for (int i = 0; i < initialCount; i++)
            {
                string command = this.HistoricalQueue.Dequeue();

                Console.WriteLine(command.ToString());

                this.HistoricalQueue.Enqueue(command);
            }
        }

        public void WriteConsoleOutput(string message = "")
        {
            if (message.Length > 0)
            {
                Console.WriteLine(message);
            }
        }

        public void UpdateTerminalPrompt()
        {
            if (this.CurrentUser.CurrentRole == UserRole.Guest)
            {
                this._readPrompt = "ChocAnon> ";
            }
            else
            {
                this._readPrompt = string.Format("ChocAnon.{0}> ", this.CurrentUser.Username);
            }
        }

        #endregion

        #region Private Methods

        private string Execute(ConsoleCommand command)
        {
            #region Validate the command name

            if (!this._commandLibraries.ContainsKey(command.LibraryClassName))
            {
                return string.Format("Command \'{0}\' does not exist\n", command.Name);
            }
            var methodDictionary = _commandLibraries[command.LibraryClassName];
            if (!methodDictionary.ContainsKey(command.Name))
            {
                return string.Format("Command \'{0}\' does not exist\n", command.Name);
            }

            #endregion

            // Make sure the corret number of required arguments are provided:
            var methodParameterValueList = new List<object>();
            IEnumerable<ParameterInfo> paramInfoList = methodDictionary[command.Name].ToList();

            // Validate proper # of required arguments provided. Some may be optional:
            var requiredParams = paramInfoList.Where(p => p.IsOptional == false);
            var optionalParams = paramInfoList.Where(p => p.IsOptional == true);
            int requiredCount = requiredParams.Count();
            int optionalCount = optionalParams.Count();
            int providedCount = command.Arguments.Count();

            if (requiredCount > providedCount)
            {
                return string.Format("Missing required argument. {0} required, {1} optional, {2} provided\n", requiredCount, optionalCount, providedCount);
            }

            // Make sure all arguments are coerced to the proper type, and that there is a 
            // value for every emthod parameter. The InvokeMember method fails if the number 
            // of arguments provided does not match the number of parameters in the 
            // method signature, even if some are optional:
            // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

            if (paramInfoList.Count() > 0)
            {
                // Populate the list with default values:
                foreach (var param in paramInfoList)
                {
                    // This will either add a null object reference if the param is required 
                    // by the method, or will set a default value for optional parameters. in 
                    // any case, there will be a value or null for each method argument 
                    // in the method signature:
                    methodParameterValueList.Add(param.DefaultValue);
                }

                // Now walk through all the arguments passed from the console and assign 
                // accordingly. Any optional arguments not provided have already been set to 
                // the default specified by the method signature:
                for (int i = 0; i < command.Arguments.Count(); i++)
                {
                    ParameterInfo methodParam = null;

                    try
                    {
                        methodParam = paramInfoList.ElementAt(i);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("The command contains too many arguments. Type help <command> for argument information.\n");
                        return string.Empty;
                    }

                    var typeRequired = methodParam.ParameterType;
                    object value = null;

                    try
                    {
                        // Coming from the Console, all of our arguments are passed in as 
                        // strings. Coerce to the type to match the method paramter:
                        value = ParseArgument(typeRequired, command.Arguments.ElementAt(i));
                        methodParameterValueList.RemoveAt(i);
                        methodParameterValueList.Insert(i, value);
                    }
                    catch (ArgumentException ex)
                    {
                        string argumentName = methodParam.Name;
                        string argumentTypeName = typeRequired.Name;
                        string message = string.Format("" + "The value passed for argument '{0}' cannot be parsed to type '{1}'", argumentName, argumentTypeName);
                        throw new ArgumentException(message);
                    }
                }
            }

            #region Invoke Method Using Reflection

            Assembly current = typeof(Program).Assembly;

            // Need the full Namespace for this:
            Type commandLibaryClass =
                current.GetType(this._commandNamespace + "." + command.LibraryClassName);

            object[] inputArgs = null;
            if (methodParameterValueList.Count > 0)
            {
                inputArgs = methodParameterValueList.ToArray();
            }

            var typeInfo = commandLibaryClass;

            // This will throw if the number of arguments provided does not match the number 
            // required by the method signature, even if some are optional:
            try
            {
                var result = this._commandRunner.RunMethodByName(command.Name, inputArgs);
                return result.ToString();
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }

            #endregion
        }

        private string ReadTermInput(string promptMessage = "")
        {
            Console.Write(this._readPrompt + promptMessage);
            return Console.ReadLine();
        }

        private object ParseArgument(Type requiredType, string inputValue)
        {
            var requiredTypeCode = Type.GetTypeCode(requiredType);
            string exceptionMessage = string.Format("Cannnot parse the input argument {0} to required type {1}", inputValue, requiredType.Name);
            object result = null;

            switch (requiredTypeCode)
            {
                case TypeCode.String:
                    result = inputValue;
                    break;
                case TypeCode.Int16:
                    short number16;
                    if (Int16.TryParse(inputValue, out number16))
                        result = number16;
                    else
                        throw new ArgumentException(exceptionMessage);
                    break;
                case TypeCode.Int32:
                    int number32;
                    if (Int32.TryParse(inputValue, out number32))
                        result = number32;
                    else
                        throw new ArgumentException(exceptionMessage);
                    break;
                case TypeCode.Int64:
                    long number64;
                    if (Int64.TryParse(inputValue, out number64))
                        result = number64;
                    else
                        throw new ArgumentException(exceptionMessage);
                    break;
                case TypeCode.Boolean:
                    bool trueFalse;
                    if (bool.TryParse(inputValue, out trueFalse))
                        result = trueFalse;
                    else
                        throw new ArgumentException(exceptionMessage);
                    break;
                case TypeCode.Byte:
                    byte byteValue;
                    if (byte.TryParse(inputValue, out byteValue))
                        result = byteValue;
                    else
                        throw new ArgumentException(exceptionMessage);
                    break;
                case TypeCode.Char:
                    char charValue;
                    if (char.TryParse(inputValue, out charValue))
                        result = charValue;
                    else
                        throw new ArgumentException(exceptionMessage);
                    break;
                case TypeCode.DateTime:
                    DateTime dateValue;
                    if (DateTime.TryParse(inputValue, out dateValue))
                        result = dateValue;
                    else
                        throw new ArgumentException(exceptionMessage);
                    break;
                case TypeCode.Decimal:
                    Decimal decimalValue;
                    if (Decimal.TryParse(inputValue, out decimalValue))
                        result = decimalValue;
                    else
                        throw new ArgumentException(exceptionMessage);
                    break;
                case TypeCode.Double:
                    Double doubleValue;
                    if (Double.TryParse(inputValue, out doubleValue))
                        result = doubleValue;
                    else
                        throw new ArgumentException(exceptionMessage);
                    break;
                case TypeCode.Single:
                    Single singleValue;
                    if (Single.TryParse(inputValue, out singleValue))
                        result = singleValue;
                    else
                        throw new ArgumentException(exceptionMessage);
                    break;
                case TypeCode.UInt16:
                    UInt16 uInt16Value;
                    if (UInt16.TryParse(inputValue, out uInt16Value))
                        result = uInt16Value;
                    else
                        throw new ArgumentException(exceptionMessage);
                    break;
                case TypeCode.UInt32:
                    UInt32 uInt32Value;
                    if (UInt32.TryParse(inputValue, out uInt32Value))
                        result = uInt32Value;
                    else
                        throw new ArgumentException(exceptionMessage);
                    break;
                case TypeCode.UInt64:
                    UInt64 uInt64Value;
                    if (UInt64.TryParse(inputValue, out uInt64Value))
                        result = uInt64Value;
                    else
                        throw new ArgumentException(exceptionMessage);
                    break;
                default:
                    throw new ArgumentException(exceptionMessage);
            }

            return result;
        }

        #endregion
    }
}