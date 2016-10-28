using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLUE.ChocAn.Library.Users.Managers
{
    interface IManager
    {
        void GenerateMemberReport();
        void GenerateProviderReport();
        void GenerateEFTRecord();
        void GenerateManagersSummary();
        void GenerateAllReports();
    }
}
