using BLUE.ChocAn.Library.Utils;
using System.ComponentModel;

namespace BLUE.ChocAn.Library.Users
{
    public enum MemberStatusEnum
    {
        [Description("Inactive")]
        INACTIVE,

        [Description("Active")]
        ACTIVE,

        [Description("Suspended")]
        SUSPENDED
    }

    public class Member : User
    {
        #region Public Properties

        public override UserRole CurrentRole { get { return UserRole.Member; } }
        public MemberStatusEnum MemberStatus { get; set; }
        public int CardNumber { get; set; }
        public bool CardValidated { get; private set; }
        public virtual string Username { get { return "member"; } }

        #endregion

        #region Constructors

        public Member()
        {
            this.MemberStatus = MemberStatusEnum.INACTIVE;
            this.CardValidated = false;
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
 	         return string.Format("Member Number: {0}, Member Name: {1}, Member Address: {2}, Member City: {3}, Member Province: {4}, Member Zip Code: {5}, Member Email Address: {6}, Member Status: {7}",
                                  this.UserNumber,
                                  this.UserName,
                                  this.UsertAddress,
                                  this.UserCity,
                                  this.UserState,
                                  this.UserZipCode,
                                  this.UserEmailAddress,
                                  EnumUtilities.GetEnumDescription(this.MemberStatus));
        }

        public void ViewServices()
        {
            // TODO: Finish me!
        }

        #endregion
    }
}