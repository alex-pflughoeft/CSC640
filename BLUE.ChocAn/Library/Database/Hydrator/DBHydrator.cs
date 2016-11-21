using MySql.Data.MySqlClient;
using System;
using System.Reflection;
using BLUE.ChocAn.Library.Utils;

namespace BLUE.ChocAn.Library.Database.Hydrator
{
    public class DBHydrator
    {
        public object Hydrate(object ret, MySqlDataReader reader)
        {
            // Loop through all properties in the provided model.
            foreach (PropertyInfo property in ret.GetType().GetProperties())
            {
                // Convert the property name from camelcase to underscore spaced.
                string columnName = property.Name.ToUnderscoreCase();

                // Check the property type and make the correct assignment
                if (property.PropertyType == typeof(int?))
                {
                    ret.GetType().GetProperty(property.Name).SetValue(ret, reader.GetNullableInt(columnName));
                }
                else if (property.PropertyType == typeof(int))
                {
                    ret.GetType().GetProperty(property.Name).SetValue(ret, reader.GetInt32(columnName));
                }
                else if (property.PropertyType == typeof(string))
                {
                    ret.GetType().GetProperty(property.Name).SetValue(ret, reader.GetNullableString(columnName));
                }
                else if (property.PropertyType == typeof(DateTime))
                {
                    ret.GetType().GetProperty(property.Name).SetValue(ret, reader.GetDateTime(columnName));
                }
                else if (property.PropertyType == typeof(double))
                {
                    ret.GetType().GetProperty(property.Name).SetValue(ret, reader.GetDouble(columnName));
                }
            }

            // Object is hydrated and ready to go.
            return ret;
        }
    }
}
