﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLUE.ChocAn.Library.Reports.Provider_Reports
{
    public class ProviderReport : Report
    {
        public override string ReportString()
        {
            // TODO: Finish me
            throw new NotImplementedException();
        }

        public override ReportType TypeOfReport { get { return ReportType.Provider; } }

        public override string ReportTitle
        {
            // TODO: Finish me
            get { throw new NotImplementedException(); }
        }

        public override string ReportBody
        {
            // TODO: Finish me
            get { throw new NotImplementedException(); }
        }

        public override string ReportHTML()
        {
            // TODO: Finish me
            throw new NotImplementedException();
        }
    }
}