using BLUE.ChocAn.Library.Database;
using BLUE.ChocAn.Library.Users.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLUE.ChocAn.Library.Users.Operators
{
    public class Operator : User
    {
        #region Constructors

        public Operator()
        {
            // Default Constructor
            this.UserRole = (int)Users.UserRole.Provider;
        }

        public Operator(User user)
        {
            this.UserAddress = user.UserAddress;
            this.UserCity = user.UserCity;
            this.UserEmailAddress = user.UserEmailAddress;
            this.UserName = user.UserName;
            this.UserNumber = user.UserNumber;
            this.UserRole = user.UserRole;
            this.UserState = user.UserState;
            this.UserZipCode = user.UserZipCode;
            this.LoginName = user.LoginName;
            this.UserPassword = user.UserPassword;
        }

        #endregion
    }
}