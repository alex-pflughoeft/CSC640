﻿using BLUE.ChocAn.Library.Communication;
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
        #region Private Variables

        private EmailSender _emailSender;

        #endregion

        #region Constructors

        public Manager()
        {
            // Default Constructor
        }

        #endregion

        #region Public Properties

        public override string Username { get { return "Manager"; } }
        public override UserRole CurrentRole { get { return UserRole.Manager; } }

        #endregion

        #region Public Methods

        public void ConfigureEmailServer(string emailHost, int emailPort)
        {
            this._emailSender = new EmailSender(emailHost, emailPort);
        }

        public void GenerateMemberReport(bool sendEmail = false)
        {
            Report memberReport = new MemberReport();
            Console.WriteLine(memberReport.ReportString());

            if (sendEmail)
            {
                this._emailSender.SendEmail(this.UserEmailAddress, this.UserEmailAddress, memberReport.ReportTitle, memberReport.ReportHTML());
            }
        }

        public void GenerateProviderReport(bool sendEmail = false)
        {
            Report providerReport = new ProviderReport();
            Console.WriteLine(providerReport.ReportString());

            if (sendEmail)
            {
                this._emailSender.SendEmail(this.UserEmailAddress, this.UserEmailAddress, providerReport.ReportTitle, providerReport.ReportHTML());
            }
        }

        public void GenerateEFTRecord(bool sendEmail = false)
        {
            Report eftReport = new EFTReport();
            Console.WriteLine(eftReport.ReportString());

            if (sendEmail)
            {
                this._emailSender.SendEmail(this.UserEmailAddress, this.UserEmailAddress, eftReport.ReportTitle, eftReport.ReportHTML());
            }
        }

        public void GenerateManagersSummary(bool sendEmail = false)
        {
            Report managerSummaryReport = new ManagerSummaryReport();
            Console.WriteLine(managerSummaryReport.ReportString());

            if (sendEmail)
            {
                this._emailSender.SendEmail(this.UserEmailAddress, this.UserEmailAddress, managerSummaryReport.ReportTitle, managerSummaryReport.ReportHTML());
            }
        }

        public void GenerateAllReports(bool sendEmail = false)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
