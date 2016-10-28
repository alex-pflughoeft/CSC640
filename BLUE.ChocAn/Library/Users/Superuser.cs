using BLUE.ChocAn.Library.Users.Managers;
using BLUE.ChocAn.Library.Users.Operators;
using BLUE.ChocAn.Library.Users.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLUE.ChocAn.Library.Users
{
    public class Superuser : User, IProvider, IOperator, IManager
    {
        public override string Username { get { return "Super User"; }}
        public override UserRole CurrentRole { get { return UserRole.Super; } }

        public bool ValidateMemberCard(Member member)
        {
            throw new NotImplementedException();
        }

        public bool ValidateMemberCard(int memberCardNumber)
        {
            throw new NotImplementedException();
        }

        public void BillChocAn()
        {
            throw new NotImplementedException();
        }

        public void ViewProviderDictionary()
        {
            throw new NotImplementedException();
        }

        public bool AddMember()
        {
            throw new NotImplementedException();
        }

        public bool DeleteMember(Member member)
        {
            throw new NotImplementedException();
        }

        public bool DeleteMember(int memberNumber)
        {
            throw new NotImplementedException();
        }

        public bool UpdateMember(Member member)
        {
            throw new NotImplementedException();
        }

        public bool UpdateMember(int memberNumber)
        {
            throw new NotImplementedException();
        }

        public bool AddProvider()
        {
            throw new NotImplementedException();
        }

        public bool DeleteProvider(Provider provider)
        {
            throw new NotImplementedException();
        }

        public bool DeleteProvider(int providerNumber)
        {
            throw new NotImplementedException();
        }

        public bool UpdateProvider(Member provider)
        {
            throw new NotImplementedException();
        }

        public bool UpdateProvider(int providerNumber)
        {
            throw new NotImplementedException();
        }

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
