using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLUE.ChocAn.Library.Users.Providers
{
    public class Provider : User, IProvider
    {
        #region Constructors

        public Provider()
        {
            // Default Constructor
            this.UserRole = (int)Users.UserRole.Provider;
        }

        public Provider(User user)
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

        #region Public Methods

        public bool BillChocAn(int memberNumber, int serviceCode)
        {
            // TODO: Bill ChocAn - Maybe add some sort of table in the database which represents chocan invoices?
            return true;
        }

        #endregion
    }
}