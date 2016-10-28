using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLUE.ChocAn.Library.Reports
{
    public class EFTReport : Report
    {
        public override string ReportString()
        {
            throw new NotImplementedException();
        }

        public override ReportType TypeOfReport { get { return ReportType.EFTRecord; } }

        public override string ReportTitle
        {
            get { throw new NotImplementedException(); }
        }

        public override string ReportBody
        {
            get { throw new NotImplementedException(); }
        }

        public override string ReportHTML()
        {
            throw new NotImplementedException();
        }
    }
}
