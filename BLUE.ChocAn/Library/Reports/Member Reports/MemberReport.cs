﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLUE.ChocAn.Library.Reports.Member_Reports
{
    public class MemberReport : Report
    {
        public override string ReportString()
        {
            throw new NotImplementedException();
        }

        public override ReportType TypeOfReport { get { return ReportType.Member; } }

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