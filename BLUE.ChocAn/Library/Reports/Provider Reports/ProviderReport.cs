using BLUE.ChocAn.Library.Database.Helper;
using BLUE.ChocAn.Library.Users;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLUE.ChocAn.Library.Reports.Provider_Reports
{
    public class ProviderReport : Report
    {
        #region Private Variables

        private User _provider;
        private List<UserServiceLinker> _listOfServices;

        #endregion

        #region Constructors

        public ProviderReport(User provider, List<UserServiceLinker> services)
        {
            this._provider = provider;
            this._listOfServices = services;
        }

        #endregion

        #region Public Properties

        public override ReportType TypeOfReport { get { return ReportType.Provider; } }

        public override string ReportBody
        {
            get { return this.ToString(); }
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            // TODO: Filter by the current week?

            DBHelper helper = new DBHelper();
            var result = string.Empty;
            double fee = 0;

            result += Environment.NewLine + "******************************************";
            result += string.Format(Environment.NewLine + "Provider Name: {0}", this._provider.UserName);
            result += string.Format(Environment.NewLine + "Provider Number: {0}", this._provider.UserNumber);
            result += string.Format(Environment.NewLine + "Provider Street Address: {0}", this._provider.UserAddress);
            result += string.Format(Environment.NewLine + "Provider City: {0}", this._provider.UserCity);
            result += string.Format(Environment.NewLine + "Provider State: {0}", this._provider.UserState);
            result += string.Format(Environment.NewLine + "Provider Zip: {0}", this._provider.UserZipCode);
            result += Environment.NewLine + "******************************************";

            foreach (UserServiceLinker service in this._listOfServices)
            {
                fee += helper.GetServiceByServiceCode(service.ServiceCode).ServiceFee;

                User thisMember = helper.GetUserByNumber(service.MemberNumber);
                Service thisService = helper.GetServiceByServiceCode(service.ServiceCode);

                result += Environment.NewLine;
                result += Environment.NewLine + "******************************************";
                result += string.Format(Environment.NewLine + "Member Name: {0}", thisMember.UserName);
                result += string.Format(Environment.NewLine + "Member Number: {0}", thisMember.UserNumber);
                result += string.Format(Environment.NewLine + "Service Code: {0}", thisService.ServiceCode);
                result += string.Format(Environment.NewLine + "Fee: {0}", thisService.ServiceFee);
                result += string.Format(Environment.NewLine + "Date of Service: {0}", service.DateOfService.ToString("MM-dd-yyyy"));
                result += string.Format(Environment.NewLine + "Date Created: {0}", service.DateCreated.ToString("MM-dd-yyyy"));
                result += Environment.NewLine + "******************************************";
            }

            result += Environment.NewLine;
            result += Environment.NewLine + "******************************************";
            result += string.Format(Environment.NewLine + "Total Number of Consultations: {0}", this._listOfServices.Count().ToString());
            result += string.Format(Environment.NewLine + "Total Fee: {0}", fee.ToString());
            result += Environment.NewLine + "******************************************";

            return result;
        }

        #endregion
    }
}