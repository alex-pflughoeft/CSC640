using BLUE.ChocAn.Library.Communication;
using BLUE.ChocAn.Library.Database.Helper;
using BLUE.ChocAn.Library.Reports;
using BLUE.ChocAn.Library.Reports.Manager_Reports;
using BLUE.ChocAn.Library.Reports.Member_Reports;
using BLUE.ChocAn.Library.Reports.Provider_Reports;
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

        public bool ValidateMemberCard(Member member)
        {
            //member.MemberStatus = (int)MemberStatusEnum.ACTIVE;
            return true;
        }

        public bool BillChocAn(int memberNumber, int serviceCode)
        {
            // TODO: Finish me
            return true;
        }

        public string GenerateMemberReport(int memberNumber, DBHelper dbHelper, EmailSender emailSender = null, bool saveFile = false)
        {
            Report memberReport = new MemberReport(dbHelper.GetRenderedServicesByMember(memberNumber));

            if (emailSender != null)
            {
                emailSender.SendEmail(this.UserEmailAddress, this.UserEmailAddress, memberReport.ReportTitle, memberReport.ReportHTML());
            }

            if (saveFile)
            {
                // TODO: Save the file
            }

            return memberReport.ToString();
        }

        public string GenerateProviderReport(DBHelper dbHelper, EmailSender emailSender = null, bool saveFile = false)
        {
            Report providerReport = new ProviderReport(dbHelper.GetUsersByRole(Users.UserRole.Provider));

            if (emailSender != null)
            {
                emailSender.SendEmail(this.UserEmailAddress, this.UserEmailAddress, providerReport.ReportTitle, providerReport.ReportHTML());
            }

            if (saveFile)
            {
                // TODO: Save the file
            }

            return providerReport.ToString();
        }

        public string GenerateEFTRecord(DBHelper dbHelper, EmailSender emailSender = null, bool saveFile = false)
        {
            Report eftReport = new EFTReport();

            if (emailSender != null)
            {
                emailSender.SendEmail(this.UserEmailAddress, this.UserEmailAddress, eftReport.ReportTitle, eftReport.ReportHTML());
            }

            if (saveFile)
            {
                // TODO: Save the file
            }

            return eftReport.ToString();
        }

        public string GenerateManagersSummary(DBHelper dbHelper, EmailSender emailSender = null, bool saveFile = false)
        {
            Report managerSummaryReport = new ManagerSummaryReport(dbHelper.GetUsersByRole(Users.UserRole.All));

            if (emailSender != null)
            {
                emailSender.SendEmail(this.UserEmailAddress, this.UserEmailAddress, managerSummaryReport.ReportTitle, managerSummaryReport.ReportHTML());
            }

            if (saveFile)
            {
                // TODO: Save the file
            }

            return managerSummaryReport.ToString();
        }

        public string GenerateAllReports(DBHelper dbHelper, EmailSender emailSender = null, bool saveFile = false)
        {
            return string.Format("{0}\n{1}\n{2}\n{3}",
                //this.GenerateMemberReport(dbHelper, emailSender, saveFile),
                this.GenerateProviderReport(dbHelper, emailSender, saveFile),
                this.GenerateEFTRecord(dbHelper, emailSender, saveFile),
                this.GenerateManagersSummary(dbHelper, emailSender, saveFile));
        }

        #endregion
    }
}