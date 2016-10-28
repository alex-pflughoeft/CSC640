using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLUE.ChocAn.Library.Users.Managers
{
    interface IManager
    {
        void GenerateMemberReport(bool sendEmail = false);
        void GenerateProviderReport(bool sendEmail = false);
        void GenerateEFTRecord(bool sendEmail = false);
        void GenerateManagersSummary(bool sendEmail = false);
        void GenerateAllReports(bool sendEmail = false);
    }
}
