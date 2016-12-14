using BLUE.ChocAn.Library.Communication;
using BLUE.ChocAn.Library.Database.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLUE.ChocAn.Library.Users.Managers
{
    internal interface IManager
    {
        string GenerateMemberReport(User member, List<UserServiceLinker> services);
        string GenerateProviderReport(User provider, List<UserServiceLinker> services);
        string GenerateEFTRecord(User provider, List<UserServiceLinker> services);
        string GenerateManagersSummary(List<UserServiceLinker> allServices);
        string GenerateAllReports(User provider, List<UserServiceLinker> providerServices, User member, List<UserServiceLinker> memberServices, List<UserServiceLinker> allServices);
    }
}