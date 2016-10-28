using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLUE.ChocAn.Library.Reports.Member_Reports
{
    public class MemberReport : Report
    {
        public override string GenerateReport(List<System.Net.Mail.MailAddress> emailAddressList = null)
        {
            throw new NotImplementedException();
        }

        public override ReportType TypeOfReport { get { return ReportType.Member; } }
    }
}
