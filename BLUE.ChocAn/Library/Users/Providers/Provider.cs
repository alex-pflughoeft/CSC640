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

        public bool BillChocAn(int memberNumber, int serviceCode)
        {
            // Theoretically this is where the system would actually bill the ChocAn system. We are just going to assume it works.
            return true;
        }

        #endregion
    }
}