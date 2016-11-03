using BLUE.ChocAn.Library.Other;
using BLUE.ChocAn.Library.Users;
using BLUE.ChocAn.Library.Users.Managers;
using BLUE.ChocAn.Library.Users.Operators;
using BLUE.ChocAn.Library.Users.Providers;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Threading;
using WMPLib;

namespace BLUE.ChocAn.Library.Commands
{
    public class CommandRunner
    {
        #region Private Variables

        private Terminal _callbackTerminal;

        #endregion

        #region Constructors

        public CommandRunner(Terminal terminal)
        {
            this._callbackTerminal = terminal;
        }

        #endregion

        #region Public Methods

        public string RunMethodByName(string command, object[] parameters = null)
        {
            UserRole roleRequired = this.GetRoleRequired(command);

            if (roleRequired != UserRole.None)
            {
                if (this._callbackTerminal.CurrentUser.CurrentRole != UserRole.Super)
                {
                    if (roleRequired == UserRole.All || this._callbackTerminal.CurrentUser.CurrentRole == roleRequired)
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

        #endregion

        #region Public Console Commands

        [Display(Description = "Command Name: addmember\nDescription: Adds a member to the system.")]
        [RoleRequired(Role = UserRole.Operator)]
        public string addmember()
        {
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

                if (((IOperator)this._callbackTerminal.CurrentUser).AddMember(thisMember))
                {
                    return string.Format("Member \'{0}\' successfully added.\n", thisMember.UserName);
                }
            }

            return "The terminal must be in interactive mode to run this command.\n";
        }

        [Display(Description = "Command Name: addprovider\nDescription: Adds a provider to the system.")]
        [RoleRequired(Role = UserRole.Operator)]
        public string addprovider()
        {
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

                if (((IOperator)this._callbackTerminal.CurrentUser).AddMember(thisMember))
                {
                    return string.Format("Member \'{0}\' successfully added.\n", thisMember.UserName);
                }
            }

            return this.InsufficientPrivilegeMessage();
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
            ((IProvider)this._callbackTerminal.CurrentUser).BillChocAn();
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
            if (this._callbackTerminal.IsInteractiveMode)
            {
                if (memberNumber == -1)
                {
                    Console.WriteLine("Enter the member number:\n");
                    string memberNumberString = Console.ReadLine();
                    memberNumber = Convert.ToInt32(memberNumberString);
                }

                if (((IOperator)this._callbackTerminal.CurrentUser).DeleteMember(memberNumber))
                {
                    // TODO: Format return message
                    return string.Empty;
                }

                // TODO: Format return message
                return string.Empty;
            }

            // TODO: Format return message
            return string.Empty;
        }

        [Display(Description = "Command Name: deleteprovider\nParameters: deleteprovider <provider number>\nDescription: Deletes a provider from the system.")]
        [RoleRequired(Role = UserRole.Operator)]
        public string deleteprovider(int providerNumber = -1)
        {
            if (this._callbackTerminal.IsInteractiveMode)
            {
                if (providerNumber == -1)
                {
                    Console.WriteLine("Enter the member number:\n");
                    string providerNumberString = Console.ReadLine();
                    providerNumber = Convert.ToInt32(providerNumberString);
                }

                if (((IOperator)this._callbackTerminal.CurrentUser).DeleteProvider(providerNumber))
                {
                    // TODO: Format return message
                    return string.Empty;
                }

                // TODO: Format return message
                return string.Empty;
            }

            // TODO: Format return message
            return string.Empty;
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
                    ((IManager)this._callbackTerminal.CurrentUser).GenerateManagersSummary();
                    break;
                case "2":
                    ((IManager)this._callbackTerminal.CurrentUser).GenerateMemberReport();
                    break;
                case "3":
                    ((IManager)this._callbackTerminal.CurrentUser).GenerateProviderReport();
                    break;
                case "4":
                    ((IManager)this._callbackTerminal.CurrentUser).GenerateEFTRecord();
                    break;
                case "5":
                    ((IManager)this._callbackTerminal.CurrentUser).GenerateAllReports();
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
                    if (this._callbackTerminal.CurrentUser.CurrentRole != UserRole.Super)
                    {
                        if (roleRequired == UserRole.All || this._callbackTerminal.CurrentUser.CurrentRole == roleRequired)
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

                foreach (var key in this._callbackTerminal.CommandLibraries["CommandRunner"].Keys)
                {
                    UserRole roleRequired = this.GetRoleRequired(key.ToString());

                    if (roleRequired != UserRole.None)
                    {
                        if (this._callbackTerminal.CurrentUser.CurrentRole != UserRole.Super)
                        {
                            if (roleRequired == UserRole.All || this._callbackTerminal.CurrentUser.CurrentRole == roleRequired)
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
            User newUser;
            bool passRequired = true;

            switch (userName.ToLower())
            {
                case "manager":
                    newUser = new Manager();
                    break;
                case "operator":
                    newUser = new Operator();
                    break;
                case "provider":
                    passRequired = false;
                    Console.WriteLine("Please enter your provider number.");

                    // TODO: Validate number
                    if (Console.ReadLine() != string.Empty)
                    {
                        newUser = new Provider();
                    }
                    else
                    {
                        return string.Empty;
                    }

                    break;
                case "superuser":
                    newUser = new Superuser();
                    break;
                default:
                    return string.Format("User \'{0}\' does not exist. Please try again.\n", userName);
            }

            if (passRequired)
            {
                if (password == "password")
                {
                    this._callbackTerminal.CurrentUser = newUser;
                    this._callbackTerminal.UpdateTerminalPrompt();
                    return string.Format("Welcome \'{0}\'! Type 'help' to see the new available commands.\n", newUser.Username);
                }
            }
            else
            {
                this._callbackTerminal.CurrentUser = newUser;
                this._callbackTerminal.UpdateTerminalPrompt();
                return string.Format("Welcome \'{0}\'! Type 'help' to see the new available commands.\n", newUser.Username);
            }

            return string.Format("Failed login for user \'{0}\'. Wrong Password.\n", userName);
        }

        [Display(Description = "Command Name: logout\nDescription: Logs the user out of the application.")]
        [RoleRequired(Role = UserRole.All)]
        public string logout()
        {
            if (this._callbackTerminal.CurrentUser.CurrentRole == UserRole.Guest)
            {
                return "You are not currently logged in.\n";
            }

            Console.WriteLine("Are you sure you want to log out? (y/n)");

            if (Console.ReadLine().ToLower() == "y")
            {
                this._callbackTerminal.CurrentUser = new User();
                this._callbackTerminal.UpdateTerminalPrompt();
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
            this._callbackTerminal.CurrentUser = new User();
            this._callbackTerminal.UpdateTerminalPrompt();
            this.clear();
            Console.WriteLine(this.info());
            Console.WriteLine(string.Format("Welcome {0}! Here are your list of commands:\n", this._callbackTerminal.CurrentUser.Username));
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

                if (((IOperator)this._callbackTerminal.CurrentUser).AddMember(thisMember))
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
                if (this._callbackTerminal.CurrentUser is IOperator)
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

                    if (((IOperator)this._callbackTerminal.CurrentUser).AddMember(thisMember))
                    {
                        return string.Format("Member \'{0}\' successfully added.\n", thisMember.UserName);
                    }
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

            if (((IProvider)this._callbackTerminal.CurrentUser).ValidateMemberCard(Convert.ToInt32(cardId)))
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
            // TODO: Under construction
            ((IProvider)this._callbackTerminal.CurrentUser).ViewProviderDictionary();
            return string.Empty;
        }

        [Display(Description = "Command Name: whoami\nDescription: Displays the curent user.")]
        [RoleRequired(Role = UserRole.All)]
        public string whoami()
        {
            return this._callbackTerminal.CurrentUser.Username + "\n";
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

        #endregion
    }

    public class RoleRequired : Attribute
    {
        public UserRole Role { get; set; }
    }
}