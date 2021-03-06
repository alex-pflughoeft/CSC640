﻿using BLUE.ChocAn.Library.Communication;
using BLUE.ChocAn.Library.Database.Helper;
using BLUE.ChocAn.Library.Other;
using BLUE.ChocAn.Library.Reports.Provider_Reports;
using BLUE.ChocAn.Library.Users;
using BLUE.ChocAn.Library.Users.Managers;
using BLUE.ChocAn.Library.Users.Operators;
using BLUE.ChocAn.Library.Users.Providers;
using BLUE.ChocAn.Library.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.IO;
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
        private EmailSender _emailSender;
        private User _currentUser;
        private User _validatedMember;

        #endregion

        #region Constructors

        public CommandPresenter(TerminalView terminal)
        {
            this._callbackTerminal = terminal;
            this._currentUser = new User();
            this.CommandNamespace = "BLUE.ChocAn.Library.Commands";
            this.InitializeCommandList();
            this._dbHelper = new DBHelper();
            this._emailSender = new EmailSender(ConfigurationManager.AppSettings["EmailHost"],
                Convert.ToInt32(ConfigurationManager.AppSettings["EmailPort"]),
                ConfigurationManager.AppSettings["FromAddress"],
                ConfigurationManager.AppSettings["FromPassword"]);
            this._validatedMember = null;
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

        [Display(Description = "Command Name: addmanager\nDescription: Adds a manager to the system.")]
        [RoleRequired(Role = UserRole.Super)]
        public string addmanager()
        {
            return this.AddUser(new Manager());
        }

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
            if (this._callbackTerminal.IsInteractiveMode)
            {
                string serviceCode = string.Empty;
                string serviceName = string.Empty;
                string fee = string.Empty;

                // Validate code
                while (!System.Text.RegularExpressions.Regex.IsMatch(serviceCode, "^[0-9]{1,10}$"))
                {
                    Console.WriteLine("Enter the Service Code (<= 9 digits):");
                    serviceCode = Console.ReadLine();
                }

                // Validate Name
                while (serviceName.Length == 0 || serviceName.Length > 25)
                {
                    Console.WriteLine("Enter the Service Name (max 25 characters):");
                    serviceName = Console.ReadLine();
                }

                // TODO: Validate Fee
                while (!System.Text.RegularExpressions.Regex.IsMatch(fee, "^[0-9]{1,10}$"))
                {
                    Console.WriteLine("Enter the cost of the service ($xxx.xx):");
                    serviceName = Console.ReadLine();
                }

                // Confirm add service
                Console.WriteLine("\nYou have entered the following for a new Service:");
                Console.WriteLine(serviceCode);
                Console.WriteLine(serviceName);
                Console.WriteLine(fee);

                string confirmationResponse = string.Empty;

                // Confirm creating new service
                while (confirmationResponse.Length == 0)
                {
                    Console.WriteLine("\nDo you want to add this new Service (Y/N)?");
                    confirmationResponse = Console.ReadLine();
                }

                if (confirmationResponse.ToUpper().Equals("Y") || confirmationResponse.ToUpper().Equals("YES"))
                {
                    Service service = new Service();

                    // Create the new service
                    service.ServiceCode = serviceCode;
                    service.ServiceName = serviceName;
                    service.ServiceFee = Convert.ToDouble(fee);

                    if (this._dbHelper.Create(service))
                    {
                        return string.Format("\n{0} \'{1}\' successfully added.\n", "Service", service.ServiceName);
                    }
                    else
                    {
                        return string.Format("\n{0} \'{1}\' could not be added!\n", "Service", service.ServiceName);
                    }
                }
                else
                {
                    return "\nAdd service aborted.";
                }
            }

            return "The terminal must be in interactive mode to run this command.\n";
        }

        [Display(Description = "Command Name: billchoc\nParameters: billchoc\nDescription: Bills Chocoholics Anonymous with the charges that are outstanding.")]
        [RoleRequired(Role = UserRole.Provider)]
        public string billchoc()
        {
            if (_validatedMember != null)
            {
                string providerNumber = string.Empty;
                string dateOfService = string.Empty;
                string serviceComments = string.Empty;
                DateTime serviceDate;

                if (this._currentUser.UserRole == (int)UserRole.Super)
                {
                    while (!System.Text.RegularExpressions.Regex.IsMatch(providerNumber, "^[0-9]{1,9}$"))
                    {
                        Console.WriteLine("Enter the Provider Number (<= 9 digits):");
                        providerNumber = Console.ReadLine();
                    }
                }
                else
                {
                    providerNumber = this._currentUser.UserNumber.ToString();
                }

                string serviceCode;
                Service service = null;

                while (true)
                {
                    Console.WriteLine("Which service would you like to charge? (Press 'd' for directory)");
                    serviceCode = Console.ReadLine();

                    if (serviceCode.ToLower() == "d")
                    {
                        Console.WriteLine(this.viewpd());
                    }

                    service = this._dbHelper.GetServiceByServiceCode(serviceCode);

                    if (service != null)
                    {
                        break;
                    }
                }

                UserServiceLinker linker = new UserServiceLinker();

                // Date of service
                while (dateOfService.Length == 0 || !DateTime.TryParse(dateOfService, out serviceDate))
                {
                    Console.WriteLine("Enter the date of service (MM-DD-YYYY):");
                    dateOfService = Console.ReadLine();
                }
                // Service Comments
                do
                {
                    Console.WriteLine("Enter the service comments (optional, max 100 characters):");
                    serviceComments = Console.ReadLine();
                } while (serviceComments.Length > 100);

                linker.ProviderNumber = Convert.ToInt32(providerNumber);
                linker.DateOfService = serviceDate;
                linker.MemberNumber = _validatedMember.UserNumber;
                linker.ServiceCode = service.ServiceCode;
                linker.DateCreated = DateTime.Now;
                linker.ServiceComments = serviceComments;

                // TODO: Make this bool work correctly
                this._dbHelper.Create(linker);
                return string.Format("Service '{0}' has been successfully charged to member '{1}'.\n", linker.ServiceCode, linker.MemberNumber);                
            }

            return "Please validate the member card before performing this action.\n";
        }

        [Display(Description = "Command Name: changepassword\nParameters: changepassword <old password> <new password>\nDescription: Changes your password from the old password to the new password.")]
        [RoleRequired(Role = UserRole.AllLoggedIn)]
        public string changepassword(string oldPassword, string newPassword = "")
        {
            if (oldPassword == this._currentUser.UserPassword)
            {
                if (newPassword == "")
                {
                    Console.WriteLine("Please enter your new password:\n");
                    newPassword = Console.ReadLine();
                }

                this._currentUser.UserPassword = newPassword;

                if (this._dbHelper.Update(this._currentUser))
                {
                    return string.Format("Password was successfully updated!.\n", this._currentUser.LoginName);
                }

                return string.Format("Failed to change password. Database failure.\n");
            }

            return string.Format("Failed to change password. Password Incorrect.\n");
        }

        [Display(Description = "Command Name: clear\nDescription: Clears the screen.")]
        [RoleRequired(Role = UserRole.All)]
        public string clear()
        {
            Console.Clear();
            return string.Empty;
        }

        [Display(Description = "Command Name: credits\nDescription: Displays the credits for the project.")]
        [RoleRequired(Role = UserRole.All)]
        public string credits()
        {
            var result = string.Empty;

            result += "\n**********************************************************************";
            result += "\n*               Chocoholics Anonymous Terminal Program               *";
            result += "\n*                        CSC640 - Final Project                      *";
            result += "\n*                                                                    *";
            result += "\n*   The ChocAn Terminal is a program used to manage services and     *";
            result += "\n*        users relating to a chocoholics anonymous program.          *";
            result += "\n*                                                                    *";
            result += "\n*   This program was created using the following technology stack:   *";
            result += "\n*                        C# with .NET 4.5, MySQL                     *";
            result += "\n*                                                                    *";
            result += "\n*                                                                    *";
            result += "\n*                        The developing team:                        *";
            result += "\n*                Alex Pflughoeft - Software Engineer, PM             *";
            result += "\n*                   Brad Roberts - Software Engineer                 *";
            result += "\n*                    Sonia Akter - Software Engineer                 *";
            result += "\n**********************************************************************";

            return result + "\n";
        }

        [Display(Description = "Command Name: deletemanager\nParameters: deletemanager <manager number>\nDescription: Deletes a manager from the system.")]
        [RoleRequired(Role = UserRole.Operator)]
        public string deletemanager(int managerNumber = -1)
        {
            return this.DeleteUser(UserRole.Manager, managerNumber);
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

                return "The terminal has been set to interactive mode.\n";
            }

            // Already in interactive mode
            return "The terminal is already in interactive mode.\n";
        }

        [Display(Description = "Command Name: exitim\nParameters: exitim <seconds until exit>\nDescription: Exits interactive mode with an optional time period until exit.")]
        [RoleRequired(Role = UserRole.Manager)]
        public string exitim(int secondsUntilExit = -1)
        {
            if (this._callbackTerminal.IsInteractiveMode)
            {
                this._callbackTerminal.DisableInteractiveMode(secondsUntilExit);

                return "The terminal has exited interactive mode.\n";
            }

            // Not in interactive mode
            return "The terminal is not in interactive mode.\n";
        }

        [Display(Description = "Command Name: genreport\nParameters: genreport <report name> <email=y/n>\nDescription: Generates the specified report on screen and optionally emails it to you.")]
        [RoleRequired(Role = UserRole.Manager)]
        public string genreport(string reportName = "")
        {
            EmailSender emailSender = null;
            bool saveCopy = false;
            string returnMessage = string.Empty;

            if (reportName == string.Empty)
            {
                Console.WriteLine("Which report would you like to run?\n");
                Console.WriteLine("{0}\n{1}\n{2}\n{3}\n{4}", "1 - Managers Summary", "2 - Member Report", "3 - Provider Report", "4 - EFT Record", "5 - All Reports");
                reportName = Console.ReadLine();
            }

            Console.WriteLine("Would you like an email of the report? (y/n)");
            string email = Console.ReadLine();

            if (email.ToLower() == "y")
            {
                emailSender = this._emailSender;
            }

            Console.WriteLine("Would you like to save a local copy of the report? (y/n)");
            string save = Console.ReadLine();

            if (save.ToLower() == "y")
            {
                saveCopy = true;
            }

            string providerNumber = "";
            string memberNumber = "";

            switch (reportName)
            {
                case "1": // Managers Summary
                    returnMessage = ((IManager)this._currentUser).GenerateManagersSummary(this._dbHelper.GetRenderedServices(0));
                    break;
                case "2": // Member Report
                    memberNumber = this.GetUserNumber("Member");
                    returnMessage = ((IManager)this._currentUser).GenerateMemberReport(this._dbHelper.GetUserByNumber(Convert.ToInt32(memberNumber)), this._dbHelper.GetRenderedServicesByMember(Convert.ToInt32(memberNumber)));
                    break;
                case "3": // Provider Report
                    providerNumber = this.GetUserNumber("Provider");
                    returnMessage = ((IManager)this._currentUser).GenerateProviderReport(this._dbHelper.GetUserByNumber(Convert.ToInt32(providerNumber)), this._dbHelper.GetRenderedServicesByProvider(Convert.ToInt32(providerNumber)));
                    break;
                case "4": // EFT Record
                    providerNumber = this.GetUserNumber("Provider");
                    returnMessage = ((IManager)this._currentUser).GenerateEFTRecord(this._dbHelper.GetUserByNumber(Convert.ToInt32(providerNumber)), this._dbHelper.GetRenderedServicesByProvider(Convert.ToInt32(providerNumber)));
                    break;
                case "5": // All reports
                    providerNumber = this.GetUserNumber("Provider");
                    memberNumber = this.GetUserNumber("Member");
                    returnMessage = ((IManager)this._currentUser).GenerateAllReports(this._dbHelper.GetUserByNumber(Convert.ToInt32(providerNumber)), this._dbHelper.GetRenderedServicesByProvider(Convert.ToInt32(providerNumber)), this._dbHelper.GetUserByNumber(Convert.ToInt32(memberNumber)), this._dbHelper.GetRenderedServicesByMember(Convert.ToInt32(memberNumber)), this._dbHelper.GetRenderedServices(0));
                    break;
            }

            if (emailSender != null)
            {
                emailSender.SendEmail(this._currentUser.UserEmailAddress, this._currentUser.UserEmailAddress, string.Format("ChocAn Report - ", DateTime.Now.ToShortDateString()), returnMessage);
            }

            if (saveCopy)
            {
                string path = string.Format("{0}\\{1}{2}{3}", ConfigurationManager.AppSettings["DefaultSaveLocation"], this._currentUser.UserName, string.Format("{0:yyyy-MM-dd}", DateTime.Now), ".txt");

                if(!File.Exists(path))
                {
                    File.Create(path).Dispose();
                    using (TextWriter tw = new StreamWriter(path))
                    {
                        tw.Write(returnMessage);
                        tw.Close();
                    }
                }
                else if (File.Exists(path))
                {
                    using (TextWriter tw = new StreamWriter(path))
                    {
                        tw.Write(returnMessage);
                        tw.Close();
                    }
                }
            }

            return returnMessage;
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

            // Enter this block if you want to view the specifics of a certain command
            if (command != "No Data")
            {
                UserRole roleRequired = this.GetRoleRequired(command);

                if (roleRequired != UserRole.None)
                {
                    // Check to see if it's an open command or if this is a super user
                    if (roleRequired == UserRole.All || this._currentUser.GetUserRole() == UserRole.Super)
                    {
                        return this.GetCommandDescription(command) + "\n";
                    }

                    if (this._currentUser.GetUserRole() != UserRole.Guest)
                    {
                        // Check to see if the user has the correct role
                        if (this._currentUser.GetUserRole() == roleRequired)
                        {
                            return this.GetCommandDescription(command) + "\n";
                        }
                        else if (roleRequired == UserRole.AllLoggedIn)
                        {
                            return this.GetCommandDescription(command) + "\n";
                        }
                    }
                    else
                    {
                        if (this._currentUser.GetUserRole() == roleRequired)
                        {
                            return this.GetCommandDescription(command) + "\n";
                        }
                    }
                }
            }
            // Enter this block to get a list of commands
            else
            {
                result += "(Type help <command> to view the information about the command.)\n";

                foreach (var key in this.CommandLibraries["CommandPresenter"].Keys)
                {
                    UserRole roleRequired = this.GetRoleRequired(key.ToString());

                    if (roleRequired != UserRole.None)
                    {
                        // Check to see if it's an open command or if this is a super user
                        if (roleRequired == UserRole.All || this._currentUser.GetUserRole() == UserRole.Super)
                        {
                            result += "\n" + key.ToString();
                            continue;
                        }

                        if (this._currentUser.GetUserRole() != UserRole.Guest)
                        {
                            // Check to see if the user has the correct role
                            if (this._currentUser.GetUserRole() == roleRequired)
                            {
                                result += "\n" + key.ToString();
                                continue;
                            }
                            else if (roleRequired == UserRole.AllLoggedIn)
                            {
                                result += "\n" + key.ToString();
                                continue;
                            }
                        }
                        else
                        {
                            if (this._currentUser.GetUserRole() == roleRequired)
                            {
                                result += "\n" + key.ToString();
                                continue;
                            }
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
            if (this._currentUser.GetUserRole() == UserRole.Guest)
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

            return "You are already logged in. Please logout first to login as a different user.\n";
        }

        [Display(Description = "Command Name: logout\nDescription: Logs the user out of the application.")]
        [RoleRequired(Role = UserRole.AllLoggedIn)]
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

        [Display(Description = "Command Name: payservice\nDescription: Pays for a service that has been rendered.")]
        [RoleRequired(Role = UserRole.Member)]
        public string payservice()
        {
            string userNumber = string.Empty;

            if (this._currentUser.UserRole == (int)UserRole.Super)
            {
                while (!System.Text.RegularExpressions.Regex.IsMatch(userNumber, "^[0-9]{1,10}$"))
                {
                    Console.WriteLine("Enter the Member Number (<= 9 digits):");
                    userNumber = Console.ReadLine();
                }
            }
            else
            {
                userNumber = this._currentUser.UserNumber.ToString();
            }

            List<UserServiceLinker> services = this._dbHelper.GetRenderedServicesByMember(Convert.ToInt32(userNumber), 0);

            if (services.Count > 0)
            {
                string serviceList = string.Empty;
                string serviceCode = string.Empty;

                foreach (UserServiceLinker renderedService in services)
                {
                    serviceList += Environment.NewLine;
                    serviceList += Environment.NewLine + "Service Name: \t\t" + this._dbHelper.GetServiceByServiceCode(renderedService.ServiceCode).ServiceName;
                    serviceList += Environment.NewLine + renderedService.ToString();
                    serviceList += Environment.NewLine;
                }

                Console.WriteLine(serviceList);
                Console.WriteLine("Enter the service code you would like to pay for:");
                serviceCode = Console.ReadLine();

                foreach (UserServiceLinker renderedService in services)
                {
                    if (renderedService.ServiceCode == serviceCode)
                    {
                        renderedService.IsPaid = true;
                        this._dbHelper.Update(renderedService);
                        return string.Format("Successfully paid for service '{0}'!", this._dbHelper.GetServiceByServiceCode(renderedService.ServiceCode).ServiceName);
                    }
                }

                return "Service code not found!";
            }

            return "There are no outstanding services that need to be paid!";
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

        [Display(Description = "Command Name: resetpassword\nParameters: resetpassword <user> <new password>\nDescription: Resets the password of the user to the new password assigned.")]
        [RoleRequired(Role = UserRole.Super)]
        public string resetpassword(string username, string newPassword)
        {
            User thisUser = this._dbHelper.GetUserByLoginName(username);

            if (thisUser == null)
            {
                return string.Format("User '{0}' not found, please try again.", username);
            }

            thisUser.UserPassword = newPassword;

            if (this._dbHelper.Update(thisUser))
            {
                return string.Format("Password was successfully changed for user '{0}'.", thisUser.UserName);
            }
            else
            {
                return string.Format("Failed to update the password for user '{0}'.", thisUser.UserName);
            }
        }

        [Display(Description = "Command Name: shutdown\nDescription: Shuts down the terminal.")]
        [RoleRequired(Role = UserRole.Guest)]
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
            return UpdateUser("Member");
        }

        [Display(Description = "Command Name: updateprovider\nDescription: Updates a provider to the system.")]
        [RoleRequired(Role = UserRole.Operator)]
        public string updateprovider()
        {
            return UpdateUser("Provider");
        }

        [Display(Description = "Command Name: userlist\nDescription: View the list of all the users in the system.")]
        [RoleRequired(Role = UserRole.Super)]
        public string userlist()
        {
            List<User> listOfUsers = this._dbHelper.GetUsersByRole(UserRole.All);
            string returnMessage = string.Empty;

            if (listOfUsers.Count > 0)
            {
                foreach (User user in listOfUsers)
                {
                    returnMessage += user.ToString() + "\n";
                }

                return returnMessage;
            }

            return "No users found!";
        }

        [Display(Description = "Command Name: validatecard\nParameters: validatecard <member number>\nDescription: Validate a members card.")]
        [RoleRequired(Role = UserRole.Provider)]
        public string validatecard(string memberNumber = "")
        {
            if (memberNumber == string.Empty)
            {
                Console.WriteLine("Please slide card, or type in the member number.");
                memberNumber = Console.ReadLine();
            }

            // Get the member corresponding with the number
            User thisMember = this._dbHelper.GetUserByNumber(Convert.ToInt32(memberNumber));

            if (thisMember != null)
            {
                if (thisMember.GetUserRole() == UserRole.Member)
                {
                    List<UserServiceLinker> servicesOutstanding = this._dbHelper.GetRenderedServicesByMember(thisMember.UserNumber, 0);

                    foreach (UserServiceLinker service in servicesOutstanding)
                    {
                        if (service.ProviderNumber == this._currentUser.UserNumber)
                        {
                            if (!service.IsPaid)
                            {
                                return string.Format("Member '{0}' is suspended. Payment for service '{0}' is past due!", memberNumber, service.ServiceCode);                   
                            }
                        }
                    }

                    _validatedMember = thisMember;
                    return string.Format("Member '{0}' has been validated!", thisMember.UserName);
                }
            }

            return string.Format("Member '{0}' not found!", memberNumber);
        }

        [Display(Description = "Command Name: viewpend\nDescription: View the pending service charges.")]
        [RoleRequired(Role = UserRole.Provider)]
        public string viewpend()
        {
            string userNumber = string.Empty;

            if (this._currentUser.UserRole == (int)UserRole.Super)
            {
                while (!System.Text.RegularExpressions.Regex.IsMatch(userNumber, "^[0-9]{1,10}$"))
                {
                    Console.WriteLine("Enter the Provider Number (<= 9 digits):");
                    userNumber = Console.ReadLine();
                }
            }
            else
            {
                userNumber = this._currentUser.UserNumber.ToString();
            }

            List<UserServiceLinker> listOfRenderedServices = this._dbHelper.GetRenderedServicesByProvider(Convert.ToInt32(userNumber), 0);
            string returnMessage = string.Empty;

            if (listOfRenderedServices.Count > 0)
            {
                foreach (UserServiceLinker renderedService in listOfRenderedServices)
                {
                    returnMessage += renderedService.ToString() + "\n";
                }

                return returnMessage;
            }

            return "No pending charges found!";
        }

        [Display(Description = "Command Name: viewservices\nDescription: View the service charges.")]
        [RoleRequired(Role = UserRole.Member)]
        public string viewservices()
        {
            string userNumber = string.Empty;

            if (this._currentUser.UserRole == (int)UserRole.Super)
            {
                while (!System.Text.RegularExpressions.Regex.IsMatch(userNumber, "^[0-9]{1,10}$"))
                {
                    Console.WriteLine("Enter the Member Number (<= 9 digits):");
                    userNumber = Console.ReadLine();
                }
            }
            else
            {
                userNumber = this._currentUser.UserNumber.ToString();
            }

            List<UserServiceLinker> listOfRenderedServices = this._dbHelper.GetRenderedServicesByMember(Convert.ToInt32(userNumber));
            string returnMessage = string.Empty;

            if (listOfRenderedServices.Count > 0)
            {
                foreach (UserServiceLinker renderedService in listOfRenderedServices)
                {
                    returnMessage += Environment.NewLine;
                    returnMessage += Environment.NewLine + "Service Name: \t\t" + this._dbHelper.GetServiceByServiceCode(renderedService.ServiceCode).ServiceName;
                    returnMessage += Environment.NewLine + renderedService.ToString();
                    returnMessage += Environment.NewLine;
                }

                return returnMessage;
            }

            return "No service charges found!";
        }

        [Display(Description = "Command Name: viewpd\nDescription: View the provider dictionary.")]
        [RoleRequired(Role = UserRole.Provider)]
        public string viewpd()
        {
            try
            {
                List<Service> listOfServices = this._dbHelper.GetAllServices();

                if (listOfServices.Count > 0)
                {
                    ProviderDictionaryReport pd = new ProviderDictionaryReport(listOfServices);

                    return pd.ReportBody;
                }

                return "No services found!";
            }
            catch
            {
                return "Failed to create the Provider Dictionary report!";
            }
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
                string loginName = string.Empty;
                string userName = string.Empty;
                string userNumber = string.Empty;
                string userStreetAddress = string.Empty;
                string userCity = string.Empty;
                string userState = string.Empty;
                string userZip = string.Empty;
                string userEmail = string.Empty;
                string confirmationResponse = string.Empty;

                // Login Name
                while (loginName.Length == 0 || loginName.Length > 10)
                {
                    Console.WriteLine("Enter the {0} Login Name (max 10 characters):", user.GetUserRole().ToString());
                    loginName = Console.ReadLine();
                }
                // Validate name
                while (userName.Length == 0 || userName.Length > 25)
                {
                    Console.WriteLine("Enter the {0} Name (max 25 characters):", user.GetUserRole().ToString());
                    userName = Console.ReadLine();
                }
                // Validate number
                while (!System.Text.RegularExpressions.Regex.IsMatch(userNumber, "^[0-9]{1,9}$"))
                {
                    Console.WriteLine("Enter the {0} Number (<= 9 digits):", user.GetUserRole().ToString());
                    userNumber = Console.ReadLine();
                }
                // Validate address
                while (userStreetAddress.Length == 0 || userStreetAddress.Length > 25)
                {
                    Console.WriteLine("Enter the {0} Street Address (max 25 characters):", user.GetUserRole().ToString());
                    userStreetAddress = Console.ReadLine();
                }
                // Validate City
                while (userCity.Length == 0 || userCity.Length > 14)
                {
                    Console.WriteLine("Enter the {0} City (max 14 characters):", user.GetUserRole().ToString());
                    userCity = Console.ReadLine();
                }
                // Validate State (2 characters, alpha only)
                while (!System.Text.RegularExpressions.Regex.IsMatch(userState, "^[a-zA-Z]{2}$"))
                {
                    Console.WriteLine("Enter the {0} State (ex: 'WI'):", user.GetUserRole().ToString());
                    userState = Console.ReadLine();
                }
                // Validate Zip Code
                while (!System.Text.RegularExpressions.Regex.IsMatch(userZip, "^[0-9]{5}$"))
                {
                    Console.WriteLine("Enter the {0} Zip:", user.GetUserRole().ToString());
                    userZip = Console.ReadLine();
                }

                // Get Email address
                Console.WriteLine("Enter the {0} Email Address:", user.GetUserRole().ToString());
                userEmail = Console.ReadLine();
                // Confirm add user
                Console.WriteLine("\nYou have entered the following for a new {0}:", user.GetUserRole().ToString());
                Console.WriteLine(userName);
                Console.WriteLine(userNumber);
                Console.WriteLine(userStreetAddress);
                Console.WriteLine(userCity);
                Console.WriteLine(userState);
                Console.WriteLine(userZip);
                Console.WriteLine(userEmail);

                // Confirm creating new user
                while (confirmationResponse.Length == 0)
                {
                    Console.WriteLine("\nDo you want to add this new {0} (Y/N)?:", user.GetUserRole().ToString());
                    confirmationResponse = Console.ReadLine();
                }

                if (confirmationResponse.ToUpper().Equals("Y") || confirmationResponse.ToUpper().Equals("YES"))
                {
                    // Create the new user
                    user.LoginName = loginName;
                    user.UserName = userName;
                    user.UserNumber = Convert.ToInt32(userNumber);
                    user.UserState = userState;
                    user.UserCity = userCity;
                    user.UserAddress = userStreetAddress;
                    user.UserZipCode = userZip;
                    user.UserEmailAddress = userEmail;
                    user.UserPassword = "password";

                    if (this._dbHelper.Create(user))
                    {
                        return string.Format("\n{0} \'{1}\' successfully added.\n", user.GetUserRole().ToString(), user.UserName);
                    }
                    else
                    {
                        return string.Format("\n{0} \'{1}\' could not be added!\n", user.GetUserRole().ToString(), user.UserName);
                    }
                }
                else
                {
                    return "Add user aborted.";
                }
            }

            return "The terminal must be in interactive mode to run this command.\n";
        }

        private string UpdateUser(string userType)
        {
            if (this._callbackTerminal.IsInteractiveMode)
            {
                string userName = null;
                string userNumber = null;
                string userStreetAddress = null;
                string userCity = null;
                string userState = null;
                string userZip = null;
                string userEmail = null;
                User tempUser = null;

                Console.WriteLine("Enter {0} Number ('q' to quit):", userType);
                userNumber = Console.ReadLine();
                if (userNumber.Equals("q") || userNumber.Equals("Q")) return string.Format("{0} update command aborted.", userType);
                if (System.Text.RegularExpressions.Regex.IsMatch(userNumber, "^[0-9]{1,10}$")) tempUser = this._dbHelper.GetUserByNumber(Convert.ToInt32(userNumber));

                while (tempUser == null)
                {
                    Console.WriteLine("Invalid Entry.\nEnter {0} Number ('q' to quit):", userType);
                    userNumber = Console.ReadLine();
                    if (userNumber.Equals("q") || userNumber.Equals("Q")) return string.Format("{0} update command aborted.", userType);
                    if (System.Text.RegularExpressions.Regex.IsMatch(userNumber, "^[0-9]{1,10}$")) tempUser = this._dbHelper.GetUserByNumber(Convert.ToInt32(userNumber));
                }

                Console.WriteLine("\n{0} # {1} selected: {2}.", userType, tempUser.UserNumber, tempUser.UserName);
                Console.WriteLine("For each item press 'Enter' to keep current data, or enter new data.");

                // Validate name (max 25 characters)
                while (((userName == null)) || (userName.Length > 25 && (!(userName.Length == 0))))
                {
                    Console.WriteLine("Name ({0}), max 25 characters:", tempUser.UserName);
                    userName = Console.ReadLine();
                }

                // Validate address
                while (userStreetAddress == null || (userStreetAddress.Length > 25 && (!(userStreetAddress.Length == 0))))
                {
                    Console.WriteLine("Street Address ({0}), max 25 characters:", tempUser.UserAddress);
                    userStreetAddress = Console.ReadLine();
                }

                // Validate City
                while (userCity == null || (userCity.Length > 14 && (!(userCity.Length == 0))))
                {
                    Console.WriteLine("City ({0}), max 14 characters:", tempUser.UserCity);
                    userCity = Console.ReadLine();
                }

                // Validate State (2 characters, alpha only)
                while (userState == null || (!System.Text.RegularExpressions.Regex.IsMatch(userState, "^[a-zA-Z]{2}$") && (!(userState.Length == 0))))
                {
                    Console.WriteLine("State ({0}), ex: 'WI':", tempUser.UserState);
                    userState = Console.ReadLine();
                }

                // Validate Zip Code
                while (userZip == null || (!System.Text.RegularExpressions.Regex.IsMatch(userZip, "^[0-9]{5}$") && (!(userZip.Length == 0))))
                {
                    Console.WriteLine("Zip Code ({0}), ex: '12345':", tempUser.UserZipCode);
                    userZip = Console.ReadLine();
                }

                // Get Email address
                Console.WriteLine("Email Address ({0}):", tempUser.UserEmailAddress);
                userEmail = Console.ReadLine();

                // Update Any Changes
                if (userName.Length != 0) this._dbHelper.updateUser(Convert.ToInt32(userNumber), "user_name", userName);
                if (userStreetAddress.Length != 0) this._dbHelper.updateUser(Convert.ToInt32(userNumber), "user_address", userStreetAddress);
                if (userCity.Length != 0) this._dbHelper.updateUser(Convert.ToInt32(userNumber), "user_city", userCity);
                if (userState.Length != 0) this._dbHelper.updateUser(Convert.ToInt32(userNumber), "user_state", userState);
                if (userZip.Length != 0) this._dbHelper.updateUser(Convert.ToInt32(userNumber), "user_zip_code", userZip);
                if (userEmail.Length != 0) this._dbHelper.updateUser(Convert.ToInt32(userNumber), "user_email_address", userEmail);

                // Format return message
                if (userName.Length == 0 && userStreetAddress.Length == 0 && userCity.Length == 0 && userState.Length == 0 && userZip.Length == 0 && userEmail.Length == 0)
                {
                    return string.Format("No changes made to {0} # {1}, {2}", userType, userNumber, tempUser.UserName);
                }
                else
                {
                    return string.Format("{0} # {1}, has been updated", userType, userNumber);
                }
            }

            return string.Format("Must be in interactive mode to update {0}.", userType);
        }

        private string DeleteUser(UserRole userType, int userNumber = -1)
        {
            if (this._callbackTerminal.IsInteractiveMode)
            {
                if (userNumber == -1)
                {
                    Console.WriteLine("Enter the {0} number:\n", userType.ToString());
                    string providerNumberString = Console.ReadLine();
                    userNumber = Convert.ToInt32(providerNumberString);
                }

                if (this._dbHelper.DeleteUser(userNumber))
                {
                    return string.Format("\n{0} \'{1}\' successfully deleted.\n", userType.ToString(), userNumber.ToString());
                }

                return string.Format("\n{0} \'{1}\' could not be deleted.\n", userType.ToString(), userNumber.ToString());
            }

            return "The terminal must be in interactive mode to run this command.\n";
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
                    decimal decimalValue;
                    if (decimal.TryParse(inputValue, out decimalValue))
                        result = decimalValue;
                    else
                        throw new ArgumentException(exceptionMessage);
                    break;
                case TypeCode.Double:
                    double doubleValue;
                    if (double.TryParse(inputValue, out doubleValue))
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

            // Make sure that is is a command we can run
            if (roleRequired != UserRole.None)
            {
                // Check to see if it's an open command or if this is a super user
                if (roleRequired == UserRole.All || this._currentUser.GetUserRole() == UserRole.Super)
                {
                    return this.RunMethod(command, parameters);
                }

                if (this._currentUser.GetUserRole() != UserRole.Guest)
                {
                    // Check to see if the user has the correct role
                    if (this._currentUser.GetUserRole() == roleRequired)
                    {
                        return this.RunMethod(command, parameters);
                    }
                    else if (roleRequired == UserRole.AllLoggedIn)
                    {
                        return this.RunMethod(command, parameters);
                    }
                }
                else
                {
                    if (this._currentUser.GetUserRole() == roleRequired)
                    {
                        return this.RunMethod(command, parameters);
                    }
                }

                return this.InsufficientPrivilegeMessage();
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

        private string GetUserNumber(string role)
        {
            string userNumber = "";

            while (!System.Text.RegularExpressions.Regex.IsMatch(userNumber, "^[0-9]{1,10}$"))
            {
                Console.WriteLine("Enter the {0} Number (<= 9 digits):", role);
                userNumber = Console.ReadLine();
            }

            return userNumber.Trim();
        }

        #endregion
    }

    public class RoleRequired : Attribute
    {
        public UserRole Role { get; set; }
    }
}