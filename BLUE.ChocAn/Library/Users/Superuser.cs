using BLUE.ChocAn.Library.Users.Managers;
using BLUE.ChocAn.Library.Users.Providers;
using System;

namespace BLUE.ChocAn.Library.Users
{
    public class Superuser : User, IProvider, IManager
    {
        #region Constructors

        public Superuser()
        {
            // Default constructor
            this.UserRole = (int)Users.UserRole.Super;
        }

        public Superuser(User user)
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

        public bool ValidateMemberCard(Member member)
        {
            member.MemberStatus = (int)MemberStatusEnum.ACTIVE;
            return true;
        }

        public void BillChocAn()
        {
            // TODO: Finish me
            throw new NotImplementedException();
        }

        public void GenerateMemberReport(bool sendEmail = false)
        {
            // TODO: Finish me
            throw new NotImplementedException();
        }

        public void GenerateProviderReport(bool sendEmail = false)
        {
            // TODO: Finish me
            throw new NotImplementedException();
        }

        public void GenerateEFTRecord(bool sendEmail = false)
        {
            // TODO: Finish me
            throw new NotImplementedException();
        }

        public void GenerateManagersSummary(bool sendEmail = false)
        {
            // TODO: Finish me
            throw new NotImplementedException();
        }

        public void GenerateAllReports(bool sendEmail = false)
        {
            // TODO: Finish me
            throw new NotImplementedException();
        }

        #endregion
    }
}