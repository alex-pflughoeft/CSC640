using BLUE.ChocAn.Library.Reports.Manager_Reports;
using BLUE.ChocAn.Library.Reports.Member_Reports;
using BLUE.ChocAn.Library.Reports.Provider_Reports;
using System.Collections.Generic;
using System.Net.Mail;

namespace BLUE.ChocAn.Library.Reports
{
    public abstract class Report
    {
        public abstract string GenerateReport(List<MailAddress> emailAddressList = null);
        public abstract ReportType TypeOfReport { get; }

        public static Dictionary<ReportType, Report> ReportList = new Dictionary<ReportType, Report>()
        {
            { ReportType.ManagerSummary, new ManagerSummaryReport() },
            { ReportType.EFTRecord, new EFTReport() },
            { ReportType.Provider, new ProviderReport() },
            { ReportType.Member, new MemberReport() }
        };
    }

    public enum ReportType
    {
        ManagerSummary,
        Member,
        Provider,
        ProviderDictionary,
        EFTRecord
    }
}
