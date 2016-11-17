using BLUE.ChocAn.Library.Database;
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
    public class Superuser : User, IProvider, IManager
    {
        #region Public Properties

        public override string Username { get { return "superuser"; } }
        public override UserRole CurrentRole { get { return UserRole.Super; } }

        #endregion

        #region Constructors

        public Superuser()
        {
            // Default constructor
        }

        #endregion

        #region Public Methods

        public bool ValidateMemberCard(Member member)
        {
            member.MemberStatus = MemberStatusEnum.ACTIVE;
            return true;
        }

        public void BillChocAn()
        {
            // TODO: Finish me
            throw new NotImplementedException();
        }

        public void GenerateMemberReport(bool sendEmail = false)
        {
            // TODO: Finish me
            throw new NotImplementedException();
        }

        public void GenerateProviderReport(bool sendEmail = false)
        {
            // TODO: Finish me
            throw new NotImplementedException();
        }

        public void GenerateEFTRecord(bool sendEmail = false)
        {
            // TODO: Finish me
            throw new NotImplementedException();
        }

        public void GenerateManagersSummary(bool sendEmail = false)
        {
            // TODO: Finish me
            throw new NotImplementedException();
        }

        public void GenerateAllReports(bool sendEmail = false)
        {
            // TODO: Finish me
            throw new NotImplementedException();
        }

        #endregion
    }
}