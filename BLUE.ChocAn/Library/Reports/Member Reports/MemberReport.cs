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

        private User _member;
        private List<UserServiceLinker> _memberServices;

        #endregion

        #region Public Constructors

        public MemberReport(User member, List<UserServiceLinker> memberServices)
        {
            this._member = member;
            this._memberServices = memberServices;
        }

        #endregion

        #region Public Properties

        public override ReportType TypeOfReport { get { return ReportType.Member; } }

        public override string ReportBody
        {
            get { return this.ToString(); }
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            string returnString = string.Empty;

            foreach (UserServiceLinker serviceRendered in this._memberServices)
            {
                returnString += string.Format("Service Provider: {0}\nService Code: {1}", serviceRendered.ProviderNumber, serviceRendered.ServiceCode) + "\n\n"; 
            }

            return returnString;
        }

        #endregion
    }
}