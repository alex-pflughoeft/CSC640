using System;
using MySql.Data.MySqlClient;
using System.Reflection;
using BLUE.ChocAn.Library.Utils;
using System.Collections.Generic;
using System.Configuration;

namespace BLUE.ChocAn.Library.Database.Persister
{
    public class DBPersistor
    {
        public static void Create(BaseTable _table)
        {
            // Query template
            string sql = "INSERT INTO {0} ({1}) VALUES ({2}); SELECT LAST_INSERT_ID() as `object_id`";

            List<string> columns = new List<string>();
            List<string> values = new List<string>();

            // Get table name based on TableName attribute on model class
            string tableName = _table.GetTableName();

            if (tableName.Equals(string.Empty))
            {
                throw new Exception(string.Format("Type {0} does not have TableName attribute applied to class.", _table.GetType().Name));
            }

            // Loop through all properties in model to build query
            foreach (PropertyInfo property in _table.GetType().GetProperties())
            {
                // Skip if property is primary
                if (!property.IsPrimaryKey())
                {
                    string columnName = property.Name.ToUnderscoreCase();

                    columns.Add(columnName);

                    var value = property.GetValue(_table);

                    if (value == null)
                    {
                        values.Add("NULL");
                    }
                    else
                    {
                        if (property.PropertyType == typeof(int?) || property.PropertyType == typeof(int))
                        {
                            values.Add(value.ToString());
                        }
                        else if (property.PropertyType == typeof(string))
                        {
                            values.Add("'" + value.ToString() + "'");
                        }
                        else if (property.PropertyType == typeof(DateTime))
                        {
                            values.Add("'" + ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss") + "'");
                        }
                        else if (property.PropertyType == typeof(bool))
                        {
                            if(Convert.ToBoolean(value))
                            {
                                values.Add("1");
                            }
                            else
                            {
                                values.Add("0");
                            }
                        }
                    }
                }
            }

            // Build query from template with column names and values, respectively
            sql = string.Format(sql, tableName, string.Join(",", columns), string.Join(",", values));

            using (DBConnection connection = new DBConnection(ConfigurationManager.AppSettings["DbServer"], ConfigurationManager.AppSettings["DbName"], ConfigurationManager.AppSettings["DbUserName"], ConfigurationManager.AppSettings["DbPassword"]))
            {
                // Execute query and get back LAST_INSERT_ID()              
                var reader = connection.Read(sql);

                reader.Read();
                int id = reader.GetInt32("object_id");

                if (id == 0)
                {
                    throw new Exception("Persisted object did not return LAST_INSERT_ID()");
                }

                // Set primary key on object
                foreach (PropertyInfo property in _table.GetType().GetProperties())
                {
                    foreach (var attribute in property.CustomAttributes)
                    {
                        if (attribute.AttributeType == typeof(BLUE.ChocAn.Library.PrimaryKeyAttribute))
                        {
                            property.SetValue(_table, id);
                        }
                    }
                }
            }
        }

        public static string Update(BaseTable _table, bool returnQuery = false)
        {
            string sql = "UPDATE {0} SET {1} WHERE {2}";
            List<string> columnUpdates = new List<string>();

            string tableName = _table.GetTableName();

            if (tableName.Equals(string.Empty))
            {
                throw new Exception(string.Format("Type {0} does not have TableName attribute applied to class.", _table.GetType().Name));
            }

            // If the object does not have a primary ID set, we can't update it
            PropertyInfo primaryProperty = null;

            // Loop through properties to build update query
            foreach (PropertyInfo property in _table.GetType().GetProperties())
            {
                foreach (var attribute in property.CustomAttributes)
                {
                    if (attribute.AttributeType == typeof(BLUE.ChocAn.Library.PrimaryKeyAttribute))
                    {
                        primaryProperty = property;
                    }
                }

                string columnName = property.Name.ToUnderscoreCase();

                var value = property.GetValue(_table);
                string valueString = "NULL";

                if (value != null)
                {
                    if (property.PropertyType == typeof(int?) || property.PropertyType == typeof(int))
                    {
                        valueString = value.ToString();
                    }
                    else if (property.PropertyType == typeof(string))
                    {
                        valueString = "'" + value.ToString() + "'";
                    }
                    else if (property.PropertyType == typeof(DateTime))
                    {
                        valueString = "'" + ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss") + "'";
                    }

                    // TODO: This may break everything
                    columnUpdates.Add(string.Format("{0} = {1}", columnName, valueString));
                }

                ////columnUpdates.Add(string.Format("{0} = {1}", columnName, valueString));
            }

            if (primaryProperty != null)
            {
                // Primary key where condition
                string whereCondition = string.Format("{0} = {1}", primaryProperty.Name.ToUnderscoreCase(), primaryProperty.GetValue(_table).ToString());

                sql = string.Format(sql, tableName, string.Join(",", columnUpdates), whereCondition);

                if (returnQuery)
                {
                    return sql;
                }

                using (DBConnection connection = new DBConnection(ConfigurationManager.AppSettings["DbServer"], ConfigurationManager.AppSettings["DbName"], ConfigurationManager.AppSettings["DbUserName"], ConfigurationManager.AppSettings["DbPassword"]))
                {
                    connection.Write(sql);
                }
            }
            else
            {
                throw new Exception("Model must have a primary key set");
            }

            return string.Empty;
        }
    }
}
