using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLUE.ChocAn.Library.Users
{
    public enum UserRole
    {
        All,
        Guest, // Essentially a role for public commands
        Member,
        Provider,
        Operator,
        Manager,
        Super, // The override role
        None
    }

    public class User
    {
        protected string _userPassword;

        public virtual string Username { get { return "guest"; } }
        public int UserNumber { get; set; }
        public string UserName { get; set; }
        public string UsertAddress { get; set; }
        public string UserCity { get; set; }
        public string UserState { get; set; }
        public string UserZipCode { get; set; }
        public string UserEmailAddress { get; set; }
        public virtual UserRole CurrentRole { get { return UserRole.Guest; } }

        public override string ToString()
        {
            return "No User Specified";
        }

        public void ChangePassword()
        {
            if (this.CurrentRole == UserRole.Guest)
            {
                Console.WriteLine("Guests do not have passwords!\n");
                return;
            }

            Console.WriteLine("Please enter your current password:\n");
            string oldPassword = Console.ReadLine();

            if (oldPassword == this._userPassword)
            {
                Console.WriteLine("Please enter your new password:\n");
                string newPassword = Console.ReadLine();
                this._userPassword = newPassword;

                Console.WriteLine("Your new password has been set successfully!\n");
            }
        }
    }
}