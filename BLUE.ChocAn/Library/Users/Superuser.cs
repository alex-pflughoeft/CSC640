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
    public class Superuser : User, IProvider, IOperator, IManager
    {
        #region Private Variables

        private DBConnection _dbConnection;

        #endregion

        #region Public Properties

        public override string Username { get { return "superuser"; }}
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
            member.ActivateCard();
            return true;
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

        public bool AddMember(Member member)
        {
            try
            {
                this._dbConnection.AddUser(member);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteMember(Member member)
        {
            try
            {
                this._dbConnection.DeleteUser(member.UserNumber);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteMember(int memberNumber)
        {
            try
            {
                this._dbConnection.DeleteUser(memberNumber);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateMember(Member member)
        {
            try
            {
                this._dbConnection.UpdateUser(member);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateMember(int memberNumber)
        {
            throw new NotImplementedException();
        }

        public bool AddProvider(Provider provider)
        {
            try
            {
                this._dbConnection.AddUser(provider);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteProvider(Provider provider)
        {
            try
            {
                this._dbConnection.DeleteUser(provider.UserNumber);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteProvider(int providerNumber)
        {
            try
            {
                this._dbConnection.DeleteUser(providerNumber);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateProvider(Provider provider)
        {
            try
            {
                this._dbConnection.UpdateUser(provider);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateProvider(int providerNumber)
        {
            throw new NotImplementedException();
        }

        public void GenerateMemberReport(bool sendEmail = false)
        {
            throw new NotImplementedException();
        }

        public void GenerateProviderReport(bool sendEmail = false)
        {
            throw new NotImplementedException();
        }

        public void GenerateEFTRecord(bool sendEmail = false)
        {
            throw new NotImplementedException();
        }

        public void GenerateManagersSummary(bool sendEmail = false)
        {
            throw new NotImplementedException();
        }

        public void GenerateAllReports(bool sendEmail = false)
        {
            throw new NotImplementedException();
        }

        public void ConfigureDBConnection(string serverName, string databaseName)
        {
            this._dbConnection = new DBConnection(serverName, databaseName, this.Username, this._userPassword);
        }

        #endregion
    }
}
