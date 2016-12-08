using BLUE.ChocAn.Library.Utils;
using System.ComponentModel;

namespace BLUE.ChocAn.Library.Users
{
    public class Member : User
    {
        #region Constructors

        public Member()
        {
            this.UserRole = (int)Users.UserRole.Member;
        }

        public Member(User user)
        {
            this.UserId = user.UserId;
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

        public override string ToString()
        {
 	         return string.Format("Member Number: {0}, Member Name: {1}, Member Address: {2}, Member City: {3}, Member Province: {4}, Member Zip Code: {5}, Member Email Address: {6}",
                                  this.UserNumber,
                                  this.UserName,
                                  this.UserAddress,
                                  this.UserCity,
                                  this.UserState,
                                  this.UserZipCode,
                                  this.UserEmailAddress);
        }

        #endregion
    }
}