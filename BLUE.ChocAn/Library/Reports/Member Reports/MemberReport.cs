using BLUE.ChocAn.Library.Database.Helper;
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
            DBHelper helper = new DBHelper();

            result += Environment.NewLine;
            result += Environment.NewLine + "******************************************";
            result += Environment.NewLine + "BEGIN MEMBER REPORT";
            result += Environment.NewLine + string.Format("Date: {0}", DateTime.Now.ToString());
            result += Environment.NewLine + "******************************************";

            result += Environment.NewLine + "------------------------------------------";
            result += string.Format(Environment.NewLine + "Member Name: {0}", this._member.UserName);
            result += string.Format(Environment.NewLine + "Member Number: {0}", this._member.UserNumber);
            result += string.Format(Environment.NewLine + "Member Street Address: {0}", this._member.UserAddress);
            result += string.Format(Environment.NewLine + "Member City: {0}", this._member.UserCity);
            result += string.Format(Environment.NewLine + "Member State: {0}", this._member.UserState);
            result += string.Format(Environment.NewLine + "Member Zip: {0}", this._member.UserZipCode);
            result += Environment.NewLine + "------------------------------------------";

            foreach (UserServiceLinker serviceRendered in this._memberServices)
            {
                result += Environment.NewLine;
                result += Environment.NewLine + "------------------------------------------";
                result += string.Format(Environment.NewLine + "Date of Service: {0}", serviceRendered.DateOfService.ToString("MM-dd-yyyy"));
                result += string.Format(Environment.NewLine + "Provider: {0}", helper.GetUserByNumber(serviceRendered.ProviderNumber).UserName);
                result += string.Format(Environment.NewLine + "Service Code: {0}", helper.GetServiceByServiceCode(serviceRendered.ServiceCode).ServiceName);
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