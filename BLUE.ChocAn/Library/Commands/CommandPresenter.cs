using BLUE.ChocAn.Library.Database;
using BLUE.ChocAn.Library.Database.Helper;
using BLUE.ChocAn.Library.Other;
using BLUE.ChocAn.Library.Users;
using BLUE.ChocAn.Library.Users.Managers;
using BLUE.ChocAn.Library.Users.Operators;
using BLUE.ChocAn.Library.Users.Providers;
using BLUE.ChocAn.Library.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading;
using WMPLib;

namespace BLUE.ChocAn.Library.Commands
{
    public class CommandPresenter
    {
        #region Private Variables

        private TerminalView _callbackTerminal;
        private DBHelper _dbHelper;
        private User _currentUser;

        #endregion

        #region Constructors

        public CommandPresenter(TerminalView terminal)
        {
            this._callbackTerminal = terminal;
            this._currentUser = new User();
            this.CommandNamespace = "BLUE.ChocAn.Library.Commands";
            this.InitializeCommandList();
            this._dbHelper = new DBHelper();
        }

        #endregion

        #region Public Properties

        public Dictionary<string, Dictionary<string, IEnumerable<ParameterInfo>>> CommandLibraries { get; private set; }

        public string CommandNamespace { get; private set; }

        #endregion

        #region Public Methods

        public string Execute(ConsoleCommand command)
        {
            #region Validate the command name

            if (!this.CommandLibraries.ContainsKey(command.LibraryClassName))
            {
                return string.Format("Command \'{0}\' does not exist\n", command.Name);
            }
            var methodDictionary = this.CommandLibraries[command.LibraryClassName];
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
                        value = this.ParseArgument(typeRequired, command.Arguments.ElementAt(i));
                        methodParameterValueList.RemoveAt(i);
                        methodParameterValueList.Insert(i, value);
                    }
                    catch (ArgumentException ex)
                    {
                        string argumentName = methodParam.Name;
                        string argumentTypeName = typeRequired.Name;
                        string message = string.Format("The value passed for argument '{0}' cannot be parsed to type '{1}'", argumentName, argumentTypeName);
                        throw new ArgumentException(message);
                    }
                }
            }

            #region Invoke Method Using Reflection

            Assembly current = typeof(Program).Assembly;

            // Need the full Namespace for this:
            Type commandLibaryClass =
                current.GetType(this.CommandNamespace + "." + command.LibraryClassName);

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
                var result = this.RunMethodByName(command.Name, inputArgs);
                return result.ToString();
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }

            #endregion
        }

        #endregion

        #region Public Console Commands

        [Display(Description = "Command Name: addmember\nDescription: Adds a member to the system.")]
        [RoleRequired(Role = UserRole.Operator)]
        public string addmember()
        {
            return this.AddUser(new Member());
        }

        [Display(Description = "Command Name: addprovider\nDescription: Adds a provider to the system.")]
        [RoleRequired(Role = UserRole.Operator)]
        public string addprovider()
        {
            return this.AddUser(new Provider());
        }

        [Display(Description = "Command Name: addoperator\nDescription: Adds an operator to the system.")]
        [RoleRequired(Role = UserRole.Super)]
        public string addoperator()
        {
            return this.AddUser(new Operator());
        }

        [Display(Description = "Command Name: addservice\nDescription: Adds a service to the system.")]
        [RoleRequired(Role = UserRole.Provider)]
        public string addservice()
        {
            // TODO: Finish me!
            return string.Empty;
        }

        [Display(Description = "Command Name: billchoc\nParameters: billchoc <date of service (dd/mm/yyyy)> <service code>\nDescription: Bills Chocoholics Anonymous with the service.")]
        [RoleRequired(Role = UserRole.Provider)]
        public string billchoc(string dateOfService, string serviceCode)
        {
            ((IProvider)this._currentUser).BillChocAn();
            // TODO: Format return message
            return string.Empty;
        }

        [Display(Description = "Command Name: clear\nDescription: Clears the screen.")]
        [RoleRequired(Role = UserRole.All)]
        public string clear()
        {
            Console.Clear();
            return string.Empty;
        }

        [Display(Description = "Command Name: credits\nDescription: Prints the credits for the project.")]
        [RoleRequired(Role = UserRole.All)]
        public string credits()
        {
            // TODO: Print the credits of the program and copyright information
            return string.Format("TODO: List Credits\n");
        }

        [Display(Description = "Command Name: deletemember\nParameters: deletemember <member number>\nDescription: Deletes a member from the system.")]
        [RoleRequired(Role = UserRole.Operator)]
        public string deletemember(int memberNumber = -1)
        {
            return this.DeleteUser(UserRole.Member, memberNumber);
        }

        [Display(Description = "Command Name: deleteprovider\nParameters: deleteprovider <provider number>\nDescription: Deletes a provider from the system.")]
        [RoleRequired(Role = UserRole.Operator)]
        public string deleteprovider(int providerNumber = -1)
        {
            return this.DeleteUser(UserRole.Provider, providerNumber);
        }

        [Display(Description = "Command Name: deleteoperator\nParameters: deleteoperator <operator number>\nDescription: Deletes an operator from the system.")]
        [RoleRequired(Role = UserRole.Super)]
        public string deleteoperator(int operatorNumber = -1)
        {
            return this.DeleteUser(UserRole.Operator, operatorNumber);
        }

        [Display(Description = "Command Name: echo\nParameters: echo <message>\nDescription: Prints the message.")]
        [RoleRequired(Role = UserRole.All)]
        public string echo(string message)
        {
            return message + "\n";
        }

        [Display(Description = "Command Name: enterim\nParameters: enterim <seconds until enter>\nDescription: Enters interactive mode with an optional time period until exit.")]
        [RoleRequired(Role = UserRole.Manager)]
        public string enterim(int secondsUntilExit = -1)
        {
            if (!this._callbackTerminal.IsInteractiveMode)
            {
                if (secondsUntilExit == -1)
                {
                    Console.WriteLine("Would you like to set up a time to exit? (y/n)\n");
                    string exit = Console.ReadLine();

                    if (exit.ToLower() == "y")
                    {
                        Console.WriteLine("Enter the time until exit (s):\n");
                        string memberNumberString = Console.ReadLine();
                        secondsUntilExit = Convert.ToInt32(memberNumberString);
                    }
                }

                this._callbackTerminal.EnableInteractiveMode(secondsUntilExit);

                // TODO: Format return message
                return string.Empty;
            }

            // Already in interactive mode
            return string.Empty;
        }

        [Display(Description = "Command Name: exitim\nParameters: exitim <seconds until exit>\nDescription: Exits interactive mode with an optional time period until exit.")]
        [RoleRequired(Role = UserRole.Manager)]
        public string exitim(int secondsUntilExit = -1)
        {
            if (this._callbackTerminal.IsInteractiveMode)
            {
                this._callbackTerminal.DisableInteractiveMode(secondsUntilExit);
                // TODO: Validate the member card to be used.
                return string.Format("TODO: Exit Interactive Mode\n");
            }

            // Not in interactive mode
            return string.Empty;
        }

        [Display(Description = "Command Name: genreport\nParameters: genreport <report name> <email=y/n>\nDescription: Generates the specified report on screen and optionally emails it to you.")]
        [RoleRequired(Role = UserRole.Manager)]
        public string genreport(string reportName = "")
        {
            if (reportName == string.Empty)
            {
                Console.WriteLine("Which report would you like to run?\n");
                reportName = Console.ReadLine();
            }

            switch (reportName)
            {
                case "1":
                    ((IManager)this._currentUser).GenerateManagersSummary();
                    break;
                case "2":
                    ((IManager)this._currentUser).GenerateMemberReport();
                    break;
                case "3":
                    ((IManager)this._currentUser).GenerateProviderReport();
                    break;
                case "4":
                    ((IManager)this._currentUser).GenerateEFTRecord();
                    break;
                case "5":
                    ((IManager)this._currentUser).GenerateAllReports();
                    break;
            }

            // TODO: Format return message
            return string.Empty;
        }

        [Display(Description = "Command Name: history\nDescription: Show the historical list of commands.")]
        [RoleRequired(Role = UserRole.All)]
        public string history()
        {
            this._callbackTerminal.PrintHistory();

            return string.Empty;
        }

        [Display(Description = "Command Name: help\nParameters: help <command name>\nDescription: Show the information for the command.")]
        [RoleRequired(Role = UserRole.All)]
        public string help(string command = "No Data")
        {
            var result = string.Empty;

            if (command != "No Data")
            {
                UserRole roleRequired = this.GetRoleRequired(command);

                if (roleRequired != UserRole.None)
                {
                    if (this._currentUser.GetUserRole() != UserRole.Super)
                    {
                        if (roleRequired == UserRole.All || this._currentUser.GetUserRole() == roleRequired)
                        {
                            result += this.GetCommandDescription(command);
                        }
                    }
                    else
                    {
                        result += this.GetCommandDescription(command);
                    }
                }
            }
            else
            {
                result += "(Type help <command> to view the information about the command.)\n";

                foreach (var key in this.CommandLibraries["CommandPresenter"].Keys)
                {
                    UserRole roleRequired = this.GetRoleRequired(key.ToString());

                    if (roleRequired != UserRole.None)
                    {
                        if (this._currentUser.GetUserRole() != UserRole.Super)
                        {
                            if (roleRequired == UserRole.All || this._currentUser.GetUserRole() == roleRequired)
                            {
                                result += "\n" + key.ToString();
                                continue;
                            }
                        }
                        else
                        {
                            result += "\n" + key.ToString();
                            continue;
                        }
                    }
                }
            }

            return result + "\n";
        }

        [Display(Description = "Command Name: info\nDescription: Displays the information about the software.")]
        [RoleRequired(Role = UserRole.All)]
        public string info()
        {
            return this._callbackTerminal.TerminalInfo();
        }

        [Display(Description = "Command Name: login\nParameters: login <username> <password>\nDescription: Logs the user into the application.")]
        [RoleRequired(Role = UserRole.Guest)]
        public string login(string userName, string password = "")
        {
            var user = this._dbHelper.GetUserByLoginName(userName);

            if (user != null)
            {
                if (password == user.UserPassword)
                {
                    switch ((UserRole)user.UserRole)
                    {
                        case UserRole.Manager:
                            this._currentUser = (Manager)user;
                            break;
                        case UserRole.Member:
                            this._currentUser = (Member)user;
                            break;
                        case UserRole.Operator:
                            this._currentUser = (Operator)user;
                            break;
                        case UserRole.Provider:
                            this._currentUser = (Provider)user;
                            break;
                        case UserRole.Super:
                            this._currentUser = (Superuser)user;
                            break;
                        default:
                            break;
                    }

                    if (this._currentUser.GetUserRole() == UserRole.Guest)
                    {
                        this._callbackTerminal.UpdateTerminalPrompt("ChocAnon> ");
                    }
                    else
                    {
                        this._callbackTerminal.UpdateTerminalPrompt(string.Format("ChocAnon.{0}> ", this._currentUser.LoginName));
                    }

                    return string.Format("Welcome \'{0}\'! Type 'help' to see the new available commands.\n", this._currentUser.LoginName);
                }

                return string.Format("Failed login for user \'{0}\'. Wrong Password.\n", userName);
            }

            return string.Format("Failed login for user \'{0}\'. User Not Found.\n", userName);
        }

        [Display(Description = "Command Name: logout\nDescription: Logs the user out of the application.")]
        [RoleRequired(Role = UserRole.All)]
        public string logout()
        {
            if (this._currentUser.GetUserRole() == UserRole.Guest)
            {
                return "You are not currently logged in.\n";
            }

            Console.WriteLine("Are you sure you want to log out? (y/n)");

            if (Console.ReadLine().ToLower() == "y")
            {
                this._currentUser = new User();
                this._callbackTerminal.UpdateTerminalPrompt("ChocAnon> ");
                this._callbackTerminal.ClearHistoricalQueue();
                this.clear();
                return string.Format("Logged Out.\n");
            }

            // TODO: Format return message
            return string.Empty;
        }

        [Display(Description = "Command Name: matrix\nParameters: matrix <password>\nDescription: See how deep the rabbit hole goes.")]
        [RoleRequired(Role = UserRole.Super)]
        public string matrix(string password)
        {
            if (!string.IsNullOrEmpty(password))
            {
                if (password == "redpill")
                {
                    Console.WriteLine("Remember that all I am offering is the truth. Nothing more...");
                    Thread.Sleep(3000);
                    Console.WriteLine("Continue? (y/n)");

                    if (Console.ReadLine().ToLower() == "y")
                    {
                        Console.WriteLine("Follow me.");
                        Thread.Sleep(2000);
                        Console.WriteLine("Time is always against us.");
                        Thread.Sleep(3000);
                        Console.WriteLine("I imagine you know something about virtual reality?");
                        Thread.Sleep(5000);
                        Console.WriteLine("If the virtual reality apparatus was wired to all of your senses and controlled them completely, would you be able to tell the difference between the virtual world and the real world?");
                        Thread.Sleep(8000);
                        Console.WriteLine("The pill you took is part of a trace program.  It's going to make things feel a bit strange.");
                        Thread.Sleep(5000);
                        Console.WriteLine("Just relax...");
                        Thread.Sleep(3000);
                        Console.WriteLine("Now, Tank now!");

                        WindowsMediaPlayer myplayer = new WindowsMediaPlayer();
                        myplayer.URL = ".\\Library\\Sounds\\matrixwakeup.mp3";
                        new Thread(() => myplayer.controls.play()).Start();
                        Thread.Sleep(1000);

                        int width, height;
                        int[] y;
                        int[] l;
                        Matrix.Initialize(out width, out height, out y, out l);
                        int ms;

                        while (true)
                        {
                            DateTime t1 = DateTime.Now;
                            Matrix.MatrixStep(width, height, y, l);
                            ms = 10 - (int)((TimeSpan)(DateTime.Now - t1)).TotalMilliseconds;

                            if (ms > 0)
                                System.Threading.Thread.Sleep(ms);

                            if (Console.KeyAvailable)
                                if (Console.ReadKey().Key == ConsoleKey.F5)
                                {
                                    Matrix.Initialize(out width, out height, out y, out l);
                                }
                                else if (Console.ReadKey().Key == ConsoleKey.Escape)
                                {
                                    clear();
                                    break;
                                }
                        }

                        this.clear();
                    }

                    return string.Empty;
                }
                else
                {
                    return "You are not the one.\n";
                }
            }

            return "Required parameter '<password>' missing.\n";
        }

        [Display(Description = "Command Name: reboot\nDescription: Reboots the terminal.")]
        [RoleRequired(Role = UserRole.All)]
        public string reboot()
        {
            this._currentUser = new User();
            this._callbackTerminal.UpdateTerminalPrompt("ChocAnon> ");
            this.clear();
            Console.WriteLine(this.info());
            Console.WriteLine(string.Format("Welcome {0}! Here are your list of commands:\n", this._currentUser.LoginName));
            Console.WriteLine(this.help());
            Console.WriteLine("Please Enter a Command. (type 'help' for a list of commands)\n");

            return string.Empty;
        }

        [Display(Description = "Command Name: shutdown\nDescription: Shuts down the terminal.")]
        [RoleRequired(Role = UserRole.All)]
        public string shutdown()
        {
            Console.WriteLine("Are you sure you want to shutdown the terminal? (y/n)");

            var input = Console.ReadLine();

            if (input.ToLower().Equals("y"))
            {
                Environment.Exit(0);
            }

            return string.Empty;
        }

        [Display(Description = "Command Name: updatemember\nDescription: Updates a member in the system.")]
        [RoleRequired(Role = UserRole.Operator)]
        public string updatemember()
        {
            // TODO: Under Construction
            if (this._callbackTerminal.IsInteractiveMode)
            {
                string memberName;
                string memberNumber;
                string memberStreetAddress;
                string memberCity;
                string memberState;
                string memberZip;
                string memberEmail;

                Console.WriteLine("Enter the Member Name:");
                memberName = Console.ReadLine();
                // TODO: Validate name (max 25 characters)

                Console.WriteLine("Enter the Member Number:");
                memberNumber = Console.ReadLine();
                // TODO: Validate number

                Console.WriteLine("Enter the Member Street Address:");
                memberStreetAddress = Console.ReadLine();
                // TODO: Validate

                Console.WriteLine("Enter the Member City:");
                memberCity = Console.ReadLine();
                // TODO: Validate

                Console.WriteLine("Enter the Member State:");
                memberState = Console.ReadLine();
                // TODO: Validate

                Console.WriteLine("Enter the Member Zip:");
                memberZip = Console.ReadLine();
                // TODO: Validate

                Console.WriteLine("Enter the Email Address:");
                memberEmail = Console.ReadLine();
                // TODO: Validate

                // Create the new member
                Member thisMember = new Member();

                thisMember.UserName = memberName;
                thisMember.UserNumber = Convert.ToInt32(memberNumber);
                thisMember.UserState = memberState;
                thisMember.UserCity = memberCity;
                thisMember.UserZipCode = memberZip;
                thisMember.UserEmailAddress = memberEmail;

                if (this._dbHelper.Create(thisMember))
                {
                    return string.Format("Member \'{0}\' successfully added.\n", thisMember.UserName);
                }

                // TODO: Format return message
                return string.Empty;
            }

            // TODO: Format return message
            return string.Empty;
        }

        [Display(Description = "Command Name: updateprovider\nDescription: Updates a provider to the system.")]
        [RoleRequired(Role = UserRole.Operator)]
        public string updateprovider()
        {
            // TODO: Under Construction
            if (this._callbackTerminal.IsInteractiveMode)
            {
                string memberName;
                string memberNumber;
                string memberStreetAddress;
                string memberCity;
                string memberState;
                string memberZip;
                string memberEmail;

                Console.WriteLine("Enter the Member Name:");
                memberName = Console.ReadLine();
                // TODO: Validate name

                Console.WriteLine("Enter the Member Number:");
                memberNumber = Console.ReadLine();
                // TODO: Validate number

                Console.WriteLine("Enter the Member Street Address:");
                memberStreetAddress = Console.ReadLine();
                // TODO: Validate

                Console.WriteLine("Enter the Member City:");
                memberCity = Console.ReadLine();
                // TODO: Validate

                Console.WriteLine("Enter the Member State:");
                memberState = Console.ReadLine();
                // TODO: Validate

                Console.WriteLine("Enter the Member Zip:");
                memberZip = Console.ReadLine();
                // TODO: Validate

                Console.WriteLine("Enter the Email Address:");
                memberEmail = Console.ReadLine();
                // TODO: Validate

                // Create the new member
                Member thisMember = new Member();

                thisMember.UserName = memberName;
                thisMember.UserNumber = Convert.ToInt32(memberNumber);
                thisMember.UserState = memberState;
                thisMember.UserCity = memberCity;
                thisMember.UserZipCode = memberZip;
                thisMember.UserEmailAddress = memberEmail;

                if (this._dbHelper.Create(thisMember))
                {
                    return string.Format("Member \'{0}\' successfully added.\n", thisMember.UserName);
                }
                

                return this.InsufficientPrivilegeMessage();
            }

            // TODO: Format return message
            return string.Empty;
        }

        [Display(Description = "Command Name: validatecard\nParameters: validatecard <member card id>\nDescription: Validate a members card.")]
        [RoleRequired(Role = UserRole.Provider)]
        public string validatecard(string cardId = "")
        {
            // TODO: Under construction

            if (cardId == string.Empty)
            {
                Console.WriteLine("Please slide card, or type in the member number.");
                cardId = Console.ReadLine();
            }

            // Get the member corresponding with the number
            Member thisMember = (Member)this._dbHelper.GetMemberByCardNumber(Convert.ToInt32(cardId));

            if (((IProvider)this._currentUser).ValidateMemberCard(thisMember))
            {
                // TODO: Format return message
                return string.Empty;
            }

            // TODO: Format return message
            return string.Empty;
        }

        [Display(Description = "Command Name: viewpend\nDescription: View the pending service charges.")]
        [RoleRequired(Role = UserRole.Provider)]
        public string viewpend()
        {
            // TODO: Validate the member card to be used.
            return string.Format("TODO: View the pending charges\n");
        }

        [Display(Description = "Command Name: viewpd\nDescription: View the provider dictionary.")]
        [RoleRequired(Role = UserRole.Provider)]
        public string viewpd()
        {
            var listOfProviders = this._dbHelper.GetUsersByRole(UserRole.Provider);

            // TODO: Format the list of providers

            return listOfProviders.ToString();
        }

        [Display(Description = "Command Name: whoami\nDescription: Displays the curent user.")]
        [RoleRequired(Role = UserRole.All)]
        public string whoami()
        {
            return this._currentUser.LoginName + "\n";
        }

        #endregion

        #region Private Methods

        private string RunMethod(string command, object[] parameters = null)
        {
            Type thisType = this.GetType();
            MethodInfo theMethod = thisType.GetMethod(command);
            var result = theMethod.Invoke(this, parameters);
            return result.ToString();
        }

        private string GetCommandDescription(string command)
        {
            try
            {
                Type thisType = this.GetType();
                MethodInfo theMethod = thisType.GetMethod(command);
                var desc = theMethod.GetCustomAttribute<DisplayAttribute>();
                return desc.Description;
            }
            catch
            {
                return string.Format("Command \'{0}\' was not found.", command);
            }
        }

        private UserRole GetRoleRequired(string command)
        {
            try
            {
                Type thisType = this.GetType();
                MethodInfo theMethod = thisType.GetMethod(command);
                var desc = theMethod.GetCustomAttribute<RoleRequired>();
                return desc.Role;
            }
            catch
            {
                return UserRole.None;
            }
        }

        private string InsufficientPrivilegeMessage()
        {
            return "You do not have sufficient privileges to perform this command!\n";
        }

        private string AddUser(User user)
        {
            if (this._callbackTerminal.IsInteractiveMode)
            {
                string userName = "";
                string userNumber = "";
                string userStreetAddress = "";
                string userCity = "";
                string userState = "";
                string userZip = "";
                string userEmail = "";
                string confirmationResponse = "";

                // Validate name
                while (userName.Length == 0 || userName.Length > 25)
                {
                    Console.WriteLine("Enter the {0} Name (max 25 characters):", user.GetUserRole().ToString());
                    userName = Console.ReadLine();
                }
                // Validate number
                while (!System.Text.RegularExpressions.Regex.IsMatch(userNumber, "^[0-9]{9}$"))
                {
                    Console.WriteLine("Enter the {0} Number (9 digits):", user.GetUserRole().ToString());
                    userNumber = Console.ReadLine();
                }
                // Validate address
                while (userStreetAddress.Length == 0 || userStreetAddress.Length > 25)
                {
                    Console.WriteLine("Enter the {0} Street Address (max 25 characters):", user.GetUserRole().ToString());
                    userStreetAddress = Console.ReadLine();
                }
                //Validate City
                while (userCity.Length == 0 || userCity.Length > 14)
                {
                    Console.WriteLine("Enter the {0} City (max 14 characters):", user.GetUserRole().ToString());
                    userCity = Console.ReadLine();
                }
                //Validate State (2 characters, alpha only)
                while (!System.Text.RegularExpressions.Regex.IsMatch(userState, "^[a-zA-Z]{2}$"))
                {
                    Console.WriteLine("Enter the {0} State (ex: 'WI'):", user.GetUserRole().ToString());
                    userState = Console.ReadLine();
                }
                //Validate Zip Code
                while (!System.Text.RegularExpressions.Regex.IsMatch(userZip, "^[0-9]{5}$"))
                {
                    Console.WriteLine("Enter the {0} Zip:", user.GetUserRole().ToString());
                    userZip = Console.ReadLine();
                }
                //Get Email address
                Console.WriteLine("Enter the {0} Email Address:", user.GetUserRole().ToString());
                userEmail = Console.ReadLine();
                //Confirm add user
                Console.WriteLine("You have entered the following for a new {0}:", user.GetUserRole().ToString());
                Console.WriteLine(userName);
                Console.WriteLine(userNumber);
                Console.WriteLine(userStreetAddress);
                Console.WriteLine(userCity);
                Console.WriteLine(userState);
                Console.WriteLine(userZip);
                Console.WriteLine(userEmail);
                //Confirm creating new user
                while (confirmationResponse.Length == 0)
                {
                    Console.WriteLine("Do you want to add this new {0} (Y/N)?:", user.GetUserRole().ToString());
                    confirmationResponse = Console.ReadLine();
                }
                confirmationResponse = confirmationResponse.ToUpper();
                if (confirmationResponse.Equals("Y") || confirmationResponse.Equals("YES"))
                {
                    // Create the new user
                    user.UserName = userName;
                    user.UserNumber = Convert.ToInt32(userNumber);
                    user.UserState = userState;
                    user.UserCity = userCity;
                    user.UserZipCode = userZip;
                    user.UserEmailAddress = userEmail;
                    user.UserPassword = "password";

                    if (this._dbHelper.Create(user))
                    {
                        return string.Format("{0} \'{1}\' successfully added.\n", user.GetUserRole().ToString(), user.UserName);
                    }
                }else
                {
                    return "Add user aborted.";
                }
            }
            return "The terminal must be in interactive mode to run this command.\n";
        }

        private string DeleteUser(UserRole userType, int userNumber = -1)
        {
            if (this._callbackTerminal.IsInteractiveMode)
            {
                //if (userNumber == -1)
                //{
                //    Console.WriteLine("Enter the {0} number:\n", userType.ToString());
                //    string providerNumberString = Console.ReadLine();
                //    userNumber = Convert.ToInt32(providerNumberString);
                //}

                //if (this._dbHelper.de(userNumber))
                //{
                //    // TODO: Format return message
                //    return string.Empty;
                //}

                // TODO: Format return message
                return string.Empty;
            }

            // TODO: Format return message
            return string.Empty;
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

        private string RunMethodByName(string command, object[] parameters = null)
        {
            UserRole roleRequired = this.GetRoleRequired(command);

            if (roleRequired != UserRole.None)
            {
                if (this._currentUser.GetUserRole() != UserRole.Super)
                {
                    if (roleRequired == UserRole.All || this._currentUser.GetUserRole() == roleRequired)
                    {
                        return this.RunMethod(command, parameters);
                    }
                    else
                    {
                        return this.InsufficientPrivilegeMessage();
                    }
                }

                return this.RunMethod(command, parameters);
            }

            return string.Format("Command \'{0}\' was not found.", command);
        }

        private void InitializeCommandList()
        {
            this.CommandLibraries = new Dictionary<string, Dictionary<string, IEnumerable<ParameterInfo>>>();

            var q = from t in Assembly.GetExecutingAssembly().GetTypes()
                    where t.IsClass && t.Namespace == this.CommandNamespace
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
                this.CommandLibraries.Add(commandClass.Name, methodDictionary);
            }
        }

        #endregion
    }

    public class RoleRequired : Attribute
    {
        public UserRole Role { get; set; }
    }
}