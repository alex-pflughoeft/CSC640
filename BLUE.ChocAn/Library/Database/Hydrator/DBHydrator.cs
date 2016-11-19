using MySql.Data.MySqlClient;
using System;
using System.Reflection;
using BLUE.ChocAn.Library.Utils;

namespace BLUE.ChocAn.Library.Database.Hydrator
{
    public class DBHydrator
    {
        public object Hydrate(object _service, MySqlDataReader reader)
        {
            // Loop through all properties in the provided model.
            foreach (PropertyInfo property in _service.GetType().GetProperties())
            {
                // Convert the property name from camelcase to underscore spaced.
                string columnName = property.Name.ToUnderscoreCase();

                // Check the property type and make the correct assignment
                if (property.PropertyType == typeof(int?))
                {
                    _service.GetType().GetProperty(property.Name).SetValue(_service, reader.GetNullableInt(columnName));
                }
                else if (property.PropertyType == typeof(int))
                {
                    _service.GetType().GetProperty(property.Name).SetValue(_service, reader.GetInt32(columnName));
                }
                else if (property.PropertyType == typeof(string))
                {
                    _service.GetType().GetProperty(property.Name).SetValue(_service, reader.GetNullableString(columnName));
                }
                else if (property.PropertyType == typeof(DateTime))
                {
                    _service.GetType().GetProperty(property.Name).SetValue(_service, reader.GetDateTime(columnName));
                }
                else if (property.PropertyType == typeof(double))
                {
                    _service.GetType().GetProperty(property.Name).SetValue(_service, reader.GetDouble(columnName));
                }
            }

            // Object is hydrated and ready to go.
            return _service;
        }
    }
}
