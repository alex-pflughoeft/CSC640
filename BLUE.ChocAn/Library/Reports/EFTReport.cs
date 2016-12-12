using BLUE.ChocAn.Library.Users;
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
            // Provider Name
            // Provider Number
            // Amount to be transferred
            return "TODO: Finish me!";
        }

        #endregion
    }
}
