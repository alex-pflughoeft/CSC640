using BLUE.ChocAn.Library.Reports.Manager_Reports;
using BLUE.ChocAn.Library.Reports.Member_Reports;
using BLUE.ChocAn.Library.Reports.Provider_Reports;
using System.Collections.Generic;
using System.Net.Mail;

namespace BLUE.ChocAn.Library.Reports
{
    public enum ReportType
    {
        ManagerSummary,
        Member,
        Provider,
        ProviderDictionary,
        EFTRecord
    }

    public abstract class Report
    {
        public abstract string ReportTitle { get; }
        public abstract string ReportBody { get; }
        public abstract ReportType TypeOfReport { get; }
        public abstract string ReportString();
        public abstract string ReportHTML();
    }
}
