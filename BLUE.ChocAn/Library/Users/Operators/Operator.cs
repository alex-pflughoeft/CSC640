using BLUE.ChocAn.Library.Database;
using BLUE.ChocAn.Library.Users.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLUE.ChocAn.Library.Users.Operators
{
    public class Operator : User, IOperator
    {
        #region Private Variables

        private DBConnection _dbConnection;

        #endregion

        #region Public Properties

        public override string Username { get { return "operator"; } }
        public override UserRole CurrentRole { get { return UserRole.Operator; } }

        #endregion

        #region Constructors

        public Operator()
        {
            // Default Constructor
        }

        #endregion

        #region Public Methods

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

        public bool AddProvider(Provider provider)
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

        public bool UpdateProvider(Provider provider)
        {
            throw new NotImplementedException();
        }

        public bool UpdateProvider(int providerNumber)
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
