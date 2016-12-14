using BLUE.ChocAn.Library.Database.Helper;
using BLUE.ChocAn.Library.Users;
using BLUE.ChocAn.Library.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLUE.ChocAn.Library.Reports.Manager_Reports
{
    public class ManagerSummaryReport : Report
    {
        #region Private Variables

        private List<UserServiceLinker> _allServices;

        #endregion

        #region Constructors

        public ManagerSummaryReport(List<UserServiceLinker> allServices)
        {
            this._allServices = allServices;
        }

        #endregion

        #region Public Properties

        public override ReportType TypeOfReport { get { return ReportType.ManagerSummary; } }

        public override string ReportBody
        {
            get { return this.ToString(); }
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            // TODO: Filter by this week
            DBHelper helper = new DBHelper();
            var result = string.Empty;
            double totalFee = 0;

            // Total number of providers for this week (Distinct Providers)
            var providers = this._allServices.GroupBy(p => p.ProviderNumber).Select(g => g.First()).ToList();

            foreach (UserServiceLinker provider in providers)
            {
                User thisProvider = helper.GetUserByNumber(provider.ProviderNumber);
                var thisProvidersServices = this._allServices.Where(x => x.ProviderNumber == thisProvider.UserNumber).ToList();
                double feeProvider = 0;

                result += Environment.NewLine;
                result += Environment.NewLine + "******************************************";
                result += string.Format(Environment.NewLine + "Provider Name: {0}", thisProvider.UserName);
                result += string.Format(Environment.NewLine + "Total Consultations: {0}", thisProvidersServices.Count().ToString());

                foreach (UserServiceLinker thisService in thisProvidersServices)
                {
                    feeProvider += helper.GetServiceByServiceCode(thisService.ServiceCode).ServiceFee;
                }

                result += string.Format(Environment.NewLine + "Total Fee: {0}", feeProvider.ToString());
                result += Environment.NewLine + "******************************************";
            }
         
            foreach (UserServiceLinker service in this._allServices)
            {
                totalFee += helper.GetServiceByServiceCode(service.ServiceCode).ServiceFee;
            }

            result += Environment.NewLine;
            result += Environment.NewLine + "******************************************";
            result += string.Format(Environment.NewLine + "Total Overall Consultations: {0}", this._allServices.Count().ToString());
            result += string.Format(Environment.NewLine + "Total Overall Fee: {0}", totalFee.ToString());
            result += string.Format(Environment.NewLine + "Total Overall Number of Providers: {0}", providers.Count().ToString());
            result += Environment.NewLine + "******************************************";

            return result;
        }

        #endregion
    }
}