using BLUE.ChocAn.Library.Users;
using System.Collections.Generic;

namespace BLUE.ChocAn.Library.Reports.Manager_Reports
{
    public class ManagerSummaryReport : Report
    {
        #region Private Variables

        private List<User> _listOfProviders;
        private List<UserServiceLinker> _allServices;

        #endregion

        #region Constructors

        public ManagerSummaryReport(List<User> listOfProviders, List<UserServiceLinker> allServices)
        {
            this._listOfProviders = listOfProviders;
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
            // Every Provider to be paid that week
            // Total consultations
            // Total fee

            // Total overall consultations
            // Total overall fee
            // Total number of providers


            return "TODO: Finish me!";
        }

        #endregion
    }
}