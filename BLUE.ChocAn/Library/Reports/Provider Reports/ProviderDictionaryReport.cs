using BLUE.ChocAn.Library.Users;
using BLUE.ChocAn.Library.Users.Providers;
using System;
using System.Collections.Generic;

namespace BLUE.ChocAn.Library.Reports.Provider_Reports
{
    public class ProviderDictionaryReport : Report
    {
        private List<Service> _listOfServices;

        #region Constructors

        public ProviderDictionaryReport(List<Service> services)
        {
            this._listOfServices = services;
        }

        #endregion

        #region Public Properties

        public override ReportType TypeOfReport { get { return ReportType.ProviderDictionary; } }

        public override string ReportBody
        {
            get { return this.ToString(); }
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            string result = string.Empty;

            foreach (Service service in this._listOfServices)
            {
                result += Environment.NewLine;
                result += Environment.NewLine + "******************************************";
                result += service.ToString() + Environment.NewLine;
                result += Environment.NewLine + "******************************************";
            }

            return result;
        }

        #endregion
    }
}