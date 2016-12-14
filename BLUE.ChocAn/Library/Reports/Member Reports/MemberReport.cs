using BLUE.ChocAn.Library.Users;
using System;
using System.Collections.Generic;

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
            string result = string.Empty;
            result += Environment.NewLine;
            result += Environment.NewLine + "******************************************";
            result += Environment.NewLine + "BEGIN MEMBER REPORT";
            result += Environment.NewLine + string.Format("Date: {0}", DateTime.Now.ToString());
            result += Environment.NewLine + "******************************************";

            result += Environment.NewLine + "------------------------------------------";
            result += string.Format(Environment.NewLine + "Member: {0} ({1})", this._member.UserName, this._member.UserNumber);
            result += Environment.NewLine + "------------------------------------------";

            foreach (UserServiceLinker serviceRendered in this._memberServices)
            {
                result += Environment.NewLine;
                result += Environment.NewLine + "------------------------------------------";
                result += string.Format(Environment.NewLine + "Service Provider: {0}", serviceRendered.ProviderNumber);
                result += string.Format(Environment.NewLine + "Service Code: {0}", serviceRendered.ServiceCode);
                result += Environment.NewLine + "------------------------------------------";
            }

            result += Environment.NewLine;
            result += Environment.NewLine + "******************************************";
            result += Environment.NewLine + "END MEMBER REPORT";
            result += Environment.NewLine + "******************************************";

            return result;
        }

        #endregion
    }
}