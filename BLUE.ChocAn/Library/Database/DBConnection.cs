using BLUE.ChocAn.Library.Users;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace BLUE.ChocAn.Library.Database
{
    public class DBConnection : IDisposable
    {
        internal MySqlConnection connection;

        #region Constructors

        public DBConnection(string serverName, string databaseName, string userName, string password)
        {
            try
            {
                string connectionString = "SERVER=" + serverName + ";DATABASE=" + databaseName + ";UID=" + userName + ";PASSWORD=" + password + ";" + "Convert Zero Datetime=True;";
                this.connection = new MySqlConnection(connectionString);
                this.connection.Open();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        #endregion

        #region Public Methods

        public void Write(string sql)
        {
            try
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, this.connection))
                    cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public MySqlDataReader Read(string sql)
        {
            try
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, this.connection))
                    return cmd.ExecuteReader();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public void Dispose()
        {
            try
            {
                this.connection.Close();
            }
            catch (Exception e)
            {
                throw;
            }

            this.connection.Dispose();
        }

        #endregion
    }
}