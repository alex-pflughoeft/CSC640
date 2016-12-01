using BLUE.ChocAn.Library.Communication;
using BLUE.ChocAn.Library.Database.Helper;
using BLUE.ChocAn.Library.Reports;
using BLUE.ChocAn.Library.Reports.Manager_Reports;
using BLUE.ChocAn.Library.Reports.Member_Reports;
using BLUE.ChocAn.Library.Reports.Provider_Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLUE.ChocAn.Library.Users.Managers
{
    public class Manager : User, IManager
    {
        #region Constructors

        public Manager()
        {
            // Default Constructor
            this.UserRole = (int)Users.UserRole.Manager;
        }

        public Manager(User user)
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

        public string GenerateMemberReport(DBHelper dbHelper, EmailSender emailSender = null, bool saveFile = false)
        {
            Report memberReport = new MemberReport(dbHelper.GetUsersByRole(Users.UserRole.Member));

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
                this.GenerateMemberReport(dbHelper, emailSender, saveFile),
                this.GenerateProviderReport(dbHelper, emailSender, saveFile),
                this.GenerateEFTRecord(dbHelper, emailSender, saveFile),
                this.GenerateManagersSummary(dbHelper, emailSender, saveFile)); 
        }

        #endregion
    }
}