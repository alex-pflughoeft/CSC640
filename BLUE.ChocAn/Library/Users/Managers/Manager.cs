using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLUE.ChocAn.Library.Users.Managers
{
    public class Manager : User, IManager
    {
        public Manager()
        {
            // Default Constructor
        }

        public override string Username { get { return "Manager"; } }
        public override UserRole CurrentRole { get { return UserRole.Manager; } }

        public void GenerateMemberReport()
        {
            throw new NotImplementedException();
        }

        public void GenerateProviderReport()
        {
            throw new NotImplementedException();
        }

        public void GenerateEFTRecord()
        {
            throw new NotImplementedException();
        }

        public void GenerateManagersSummary()
        {
            throw new NotImplementedException();
        }

        public void GenerateAllReports()
        {
            throw new NotImplementedException();
        }
    }
}
