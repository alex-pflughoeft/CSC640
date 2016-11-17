using BLUE.ChocAn.Library.Users;
using System.Collections.Generic;

namespace BLUE.ChocAn.Library.Database
{
    public class DBConnection
    {
        #region Private Variables

        private string _username;
        private string _password;

        #endregion

        #region Public Properties

        public string ServerName { get; private set; }
        public string DatabaseName { get; private set; }

        #endregion

        #region Constructors

        public DBConnection(string serverName, string databaseName, string userName, string password)
        {
            this._password = password;
            this._username = userName;
            this.ServerName = serverName;
            this.DatabaseName = databaseName;
        }

        #endregion

        #region Public Methods

        public User GetUser(int userNumber)
        {
            // TODO: Get a specific user from the database
            // my change
            return new User();
        }

        public bool AddUser(User user)
        {
            // TODO: Add a user to the database
            return true;
        }

        public bool UpdateUser(User user)
        {
            // TODO: Update a specific user from the database
            return true;
        }

        public bool DeleteUser(int userNumber)
        {
            // TODO: Delete a specific user from the database
            return true;
        }

        public List<User> GetAllUsers(UserRole role)
        {
            // TODO: Get all the users of that role
            return new List<User>();
        }

        public Service GetService(int serviceId)
        {
            // TODO: Get a service
            return new Service();
        }

        public bool AddService(Service service)
        {
            // TODO: Add a service to the database
            return true;
        }

        public bool UpdateService(Service service)
        {
            // TODO: Update a specifc service in the database
            return true;
        }

        public bool DeleteService(int serviceId)
        {
            // TODO: Delete a service the database
            return true;
        }

        public List<Service> GetAllServices(int userNumber)
        {
            // TODO: Get all the services for that specific user
            return new List<Service>();
        }

        #endregion
    }
}