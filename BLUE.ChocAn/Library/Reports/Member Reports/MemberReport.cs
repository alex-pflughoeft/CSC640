using BLUE.ChocAn.Library.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLUE.ChocAn.Library.Reports.Member_Reports
{
    public class MemberReport : Report
    {
        #region Private Variables

        private List<User> _memberList;

        #endregion

        #region Public Constructors

        public MemberReport(List<User> memberList)
        {
            this._memberList = memberList;
        }

        #endregion

        #region Public Properties

        public override ReportType TypeOfReport { get { return ReportType.Member; } }

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