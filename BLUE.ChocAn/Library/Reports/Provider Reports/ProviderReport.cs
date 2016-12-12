using BLUE.ChocAn.Library.Users;
using BLUE.ChocAn.Library.Users.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            // TODO: Finish me
            /* Provider Name
             * Provider Number
             * Provider Street Address
             * Provider City
             * Provider State
             * Provider zip
             * 
             * For each service provided (for this week)
             * date of service
             * date/time data created
             * member name
             * member number
             * service code
             * fee to be paid
             * 
             * total number of consultations
             * total fee
             */

            return "TODO: Finish me!";
        }

        #endregion
    }
}