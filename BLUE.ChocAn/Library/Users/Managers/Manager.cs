using BLUE.ChocAn.Library.Communication;
using BLUE.ChocAn.Library.Database.Helper;
using BLUE.ChocAn.Library.Reports;
using BLUE.ChocAn.Library.Reports.Manager_Reports;
using BLUE.ChocAn.Library.Reports.Member_Reports;
using BLUE.ChocAn.Library.Reports.Provider_Reports;
using System;
using System.Collections.Generic;
using System.Configuration;

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

        public string GenerateMemberReport(User member, List<UserServiceLinker> services)
        {
            Report memberReport = new MemberReport(member, services);

            return memberReport.ReportBody;
        }

        public string GenerateProviderReport(User provider, List<UserServiceLinker> services)
        {
            Report providerReport = new ProviderReport(provider, services);

            return providerReport.ReportBody;
        }

        public string GenerateEFTRecord(User provider, List<UserServiceLinker> services)
        {
            Report eftReport = new EFTReport(provider, services);

            return eftReport.ReportBody;
        }

        public string GenerateManagersSummary(List<UserServiceLinker> allServices)
        {
            Report managerSummaryReport = new ManagerSummaryReport(allServices);

            return managerSummaryReport.ToString();
        }

        public string GenerateAllReports(User provider, List<UserServiceLinker> providerServices, User member, List<UserServiceLinker> memberServices, List<UserServiceLinker> allServices)
        {
            return string.Format("{0}\n{1}\n{2}\n{3}",
                this.GenerateMemberReport(member, memberServices),
                this.GenerateProviderReport(provider, providerServices),
                this.GenerateEFTRecord(provider, providerServices),
                this.GenerateManagersSummary(allServices)); 
        }

        #endregion
    }
}