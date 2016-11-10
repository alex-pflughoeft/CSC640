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
        //brad
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
            // TODO: Finish me
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
            // TODO: Finish me
            throw new NotImplementedException();
        }

        public void ConfigureDBConnection(string serverName, string databaseName)
        {
            this._dbConnection = new DBConnection(serverName, databaseName, this.Username, this._userPassword);
        }

        #endregion
    }
}