namespace BLUE.ChocAn.Library.Reports
{
    #region Enums

    public enum ReportType
    {
        ManagerSummary,
        Member,
        Provider,
        ProviderDictionary,
        EFTRecord
    }

    #endregion

    public abstract class Report
    {
        #region Public Methods

        public abstract string ReportBody { get; }
        public abstract ReportType TypeOfReport { get; }

        #endregion

        #region Public Methods

        public abstract override string ToString();

        #endregion
    }
}
