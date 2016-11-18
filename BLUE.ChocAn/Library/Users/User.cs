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
        None
    }

    [TableName("chocan_user")]
    public class User : BaseTable
    {
        [PrimaryKey]
        public int UserNumber { get; set; }

        [StringLength(15, ErrorMessage = "Login Name must not be greater than 15 characters long.")]
        public string LoginName { get; set; }

        [StringLength(120, ErrorMessage = "User Name must not be greater than 120 characters long.")]
        public string UserName { get; set; }

        [StringLength(255, ErrorMessage = "User Address must not be greater than 255 characters long.")]
        public string UserAddress { get; set; }

        [StringLength(45, ErrorMessage = "User City must not be greater than 45 characters long.")]
        public string UserCity { get; set; }

        [StringLength(2, ErrorMessage = "User State must not be greater than 2 characters long.")]
        public string UserState { get; set; }

        [StringLength(5, ErrorMessage = "User Zip Code must not be greater than 5 characters long.")]
        public string UserZipCode { get; set; }

        [StringLength(45, ErrorMessage = "User Email Address must not be greater than 45 characters long.")]
        public string UserEmailAddress { get; set; }

        [StringLength(15, ErrorMessage = "User Password must not be greater than 15 characters long.")]
        public string UserPassword { get; set; }

        public int UserRole { get; set; }

        public User()
        {
            this.UserRole = (int)Users.UserRole.Guest;
            this.LoginName = "guest";
        }

        public override string ToString()
        {
            return "No User Specified";
        }

        //public void ChangePassword()
        //{
        //    if (this.UserRole == UserRole.Guest)
        //    {
        //        Console.WriteLine("Guests do not have passwords!\n");
        //        return;
        //    }

        //    Console.WriteLine("Please enter your current password:\n");
        //    string oldPassword = Console.ReadLine();

        //    if (oldPassword == this._userPassword)
        //    {
        //        Console.WriteLine("Please enter your new password:\n");
        //        string newPassword = Console.ReadLine();
        //        this._userPassword = newPassword;

        //        Console.WriteLine("Your new password has been set successfully!\n");
        //    }
        //}
    }
}