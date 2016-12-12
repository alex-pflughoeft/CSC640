using System.ComponentModel.DataAnnotations;

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
        None,
        AllLoggedIn,
    }

    [TableName("chocan_user")]
    public class User : BaseTable
    {
        #region Constructors

        public User()
        {
            this.UserRole = (int)Users.UserRole.Guest;
            this.LoginName = "guest";
        }

        public override string ToString()
        {
            return string.Format("User Number:\t{0}\nRole:\t\t{1}\nName:\t\t{2}\nLogin Name:\t{3}\nEmail:\t\t{4}\n", this.UserNumber, ((UserRole)this.UserRole).ToString() ,this.UserName, this.LoginName, this.UserEmailAddress);
        }

        #endregion

        #region Public Properties

        [PrimaryKey]
        public int UserId { get; set; }

        public int UserNumber { get; set; }

        public string LoginName { get; set; }

        public string UserName { get; set; }

        public string UserAddress { get; set; }

        public string UserCity { get; set; }

        public string UserState { get; set; }

        public string UserZipCode { get; set; }

        public string UserEmailAddress { get; set; }

        public string UserPassword { get; set; }

        public int UserRole { get; set; }

        public int? CardNumber { get; set; }

        #endregion
    }
}