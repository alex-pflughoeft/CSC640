using BLUE.ChocAn.Library.Database.Helper;
using BLUE.ChocAn.Library.Users;
using System;
using System.Collections.Generic;

namespace BLUE.ChocAn.Library.Reports
{
    public class EFTReport : Report
    {
        private User _provider;
        private List<UserServiceLinker> _listOfServices;


        #region Constructors

        public EFTReport(User provider, List<UserServiceLinker> services)
        {
            this._provider = provider;
            this._listOfServices = services;
        }

        #endregion

        #region Public Properties

        public override string ReportBody
        {
            get { return this.ToString(); }
        }

        public override ReportType TypeOfReport { get { return ReportType.EFTRecord; } }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            DBHelper helper = new DBHelper();
            var result = string.Empty;
            double fee = 0;

            result += Environment.NewLine;
            result += Environment.NewLine + "******************************************";
            result += Environment.NewLine + "BEGIN EFT RECORD";
            result += Environment.NewLine + string.Format("Date: {0}", DateTime.Now.ToString());
            result += Environment.NewLine + "******************************************";

            result += Environment.NewLine;
            result += Environment.NewLine + "------------------------------------------";
            result += string.Format(Environment.NewLine + "Provider Name: {0}", this._provider.UserName);
            result += string.Format(Environment.NewLine + "Provider Number: {0}", this._provider.UserNumber);

            foreach (UserServiceLinker service in this._listOfServices)
            {
                fee += helper.GetServiceByServiceCode(service.ServiceCode).ServiceFee;
            }

            result += string.Format(Environment.NewLine + "Total Transfer Amount: {0}", fee.ToString("C0"));
            result += Environment.NewLine + "------------------------------------------";

            result += Environment.NewLine;
            result += Environment.NewLine + "******************************************";
            result += Environment.NewLine + "END EFT RECORD";
            result += Environment.NewLine + "******************************************";

            return result;
        }

        #endregion
    }
}
