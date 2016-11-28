using BLUE.ChocAn.Library.Database.Hydrator;
using BLUE.ChocAn.Library.Database.Persister;
using BLUE.ChocAn.Library.Users;
using BLUE.ChocAn.Library.Users.Managers;
using BLUE.ChocAn.Library.Users.Operators;
using BLUE.ChocAn.Library.Users.Providers;
using BLUE.ChocAn.Library.Utils;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Configuration;

namespace BLUE.ChocAn.Library.Database.Helper
{
    public class DBHelper
    {
        public bool Create(BaseTable table)
        {
            try
            {
                DBPersistor.Create(table);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool Update(BaseTable table)
        {
            try
            {
                DBPersistor.Update(table);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public bool DeleteUser(int userNumber)
        {
            DBHydrator hydrator = new DBHydrator();

            using (DBConnection connection = new DBConnection(ConfigurationManager.AppSettings["DbServer"], ConfigurationManager.AppSettings["DbName"], ConfigurationManager.AppSettings["DbUserName"], ConfigurationManager.AppSettings["DbPassword"]))
            {
                string sql = string.Format("DELETE FROM {0} WHERE user_number = {1}", new User().GetTableName(), userNumber);

                using (MySqlDataReader reader = connection.Read(sql))
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        return true;
                    }
                }
            }

            return false;
        }

        public bool DeleteService(string serviceCode)
        {
            DBHydrator hydrator = new DBHydrator();

            using (DBConnection connection = new DBConnection(ConfigurationManager.AppSettings["DbServer"], ConfigurationManager.AppSettings["DbName"], ConfigurationManager.AppSettings["DbUserName"], ConfigurationManager.AppSettings["DbPassword"]))
            {
                string sql = string.Format("DELETE FROM {0} WHERE service_code = {1}", new Service().GetTableName(), serviceCode);

                using (MySqlDataReader reader = connection.Read(sql))
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        return true;
                    }
                }
            }

            return false;
        }
      
        public User GetUserByNumber(int userNumber)
        {
            DBHydrator hydrator = new DBHydrator();

            using (DBConnection connection = new DBConnection(ConfigurationManager.AppSettings["DbServer"], ConfigurationManager.AppSettings["DbName"], ConfigurationManager.AppSettings["DbUserName"], ConfigurationManager.AppSettings["DbPassword"]))
            {
                string sql = string.Format("SELECT * FROM {0} WHERE user_number = {1}", new User().GetTableName(), userNumber);

                using (MySqlDataReader reader = connection.Read(sql))
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        return (User)hydrator.Hydrate(new User(), reader);
                    }
                }
            }

            return null;
        }

        public User GetUserByLoginName(string loginName)
        {
            DBHydrator hydrator = new DBHydrator();

            using (DBConnection connection = new DBConnection(ConfigurationManager.AppSettings["DbServer"], ConfigurationManager.AppSettings["DbName"], ConfigurationManager.AppSettings["DbUserName"], ConfigurationManager.AppSettings["DbPassword"]))
            {
                string sql = string.Format("SELECT * FROM {0} WHERE login_name = '{1}'", new User().GetTableName(), loginName);

                using (MySqlDataReader reader = connection.Read(sql))
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        User thisUser = (User)hydrator.Hydrate(new User(), reader);

                        switch (thisUser.GetUserRole())
                        {
                            case UserRole.Member:
                                return new Member(thisUser);
                            case UserRole.Manager:
                                return new Manager(thisUser);
                            case UserRole.Operator:
                                return new Operator(thisUser);
                            case UserRole.Provider:
                                return new Provider(thisUser);
                            case UserRole.Super:
                                return new Superuser(thisUser);
                        }
                    }
                }
            }

            return null;
        }

        public List<User> GetUsersByRole(UserRole userRole)
        {
            List<User> users = new List<User>();
            DBHydrator hydrator = new DBHydrator();

            using (DBConnection connection = new DBConnection(ConfigurationManager.AppSettings["DbServer"], ConfigurationManager.AppSettings["DbName"], ConfigurationManager.AppSettings["DbUserName"], ConfigurationManager.AppSettings["DbPassword"]))
            {
                string sql = string.Empty;

                if (userRole != UserRole.All)
                {
                    sql = string.Format("SELECT * FROM {0} WHERE user_role = {1}", new User().GetTableName(), (int)userRole);
                }
                else
                {
                    sql = string.Format("SELECT * FROM {0}", new User().GetTableName());
                }

                using (MySqlDataReader reader = connection.Read(sql))
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            users.Add((User)hydrator.Hydrate((BaseTable)(new User()), reader));
                        }
                    }
                }
            }

            return users;
        }

        public User GetMemberByCardNumber(int cardNumber)
        {
            DBHydrator hydrator = new DBHydrator();

            using (DBConnection connection = new DBConnection(ConfigurationManager.AppSettings["DbServer"], ConfigurationManager.AppSettings["DbName"], ConfigurationManager.AppSettings["DbUserName"], ConfigurationManager.AppSettings["DbPassword"]))
            {
                string sql = string.Format("SELECT * FROM {0} WHERE card_number = {1}", new User().GetTableName(), cardNumber);

                using (MySqlDataReader reader = connection.Read(sql))
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        return (User)hydrator.Hydrate(new User(), reader);
                    }
                }
            }

            return null;
        }
    }
}
