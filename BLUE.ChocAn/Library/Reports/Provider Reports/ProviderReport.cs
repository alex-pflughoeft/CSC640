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

        private List<User> _listOfProviders;

        #endregion

        #region Constructors

        public ProviderReport(List<User> listOfProviders)
        {
            this._listOfProviders = listOfProviders;
        }

        #endregion

        #region Public Properties

        public override ReportType TypeOfReport { get { return ReportType.Provider; } }

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