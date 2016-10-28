using BLUE.ChocAn.Library.Utils;
using System.ComponentModel;

namespace BLUE.ChocAn.Library.Users
{
    public class Member : User
    {
        public int MemberNumber { get; set; }
        public string MemberName { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string EmailAddress { get; set; }
        public MemberStatusEnum MemberStatus { get; set; }

        /// <summary>
        /// Initializes a new instance of Member.
        /// </summary>
        public Member()
        {
            // Default constructor
        }

        public override string ToString()
        {
 	         return string.Format("Member Number: {0}, Member Name: {1}, Member Address: {2}, Member City: {3}, Member Province: {4}, Member Zip Code: {5}, Member Email Address: {6}, Member Status: {7}",
                                  this.MemberNumber,
                                  this.MemberName,
                                  this.StreetAddress,
                                  this.City,
                                  this.State,
                                  this.ZipCode,
                                  this.EmailAddress,
                                  EnumUtilities.GetEnumDescription(this.MemberStatus));
        }
    }

    public enum MemberStatusEnum
    {
        [Description("Active")]
        ACTIVE,

        [Description("Suspended")]
        SUSPENDED
    }
}