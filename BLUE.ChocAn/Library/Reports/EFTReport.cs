using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLUE.ChocAn.Library.Reports
{
    public class EFTReport : Report
    {
        #region Constructors

        public EFTReport()
        {
        }

        #endregion

        #region Public Properties

        public override string ReportTitle
        {
            // TODO: Finish me
            get { return "TODO: Finish me!"; }
        }

        public override string ReportBody
        {
            // TODO: Finish me
            get { return "TODO: Finish me!"; }
        }

        public override ReportType TypeOfReport { get { return ReportType.EFTRecord; } }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            // TODO: Finish me
            return "TODO: Finish me!";
        }

        public override string ReportHTML()
        {
            // TODO: Finish me
            return "TODO: Finish me!";
        }

        #endregion
    }
}
