using System;

namespace BLUE.ChocAn.Library
{
    public abstract class BaseTable
    {
        public string GetTableName()
        {
            Attribute[] attrs = Attribute.GetCustomAttributes(this.GetType());

            foreach (Attribute attribute in attrs)
            {
                if (attribute is TableName)
                {
                    return ((TableName)attribute).GetTableName();
                }
            }

            return string.Empty;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class TableName : System.Attribute
    {
        /// <summary>
        /// Database table name (including and prefix)
        /// </summary>
        private string _name;

        /// <summary>
        /// Constructs attribute with name of database table
        /// </summary>
        /// <param name="name"></param>
        public TableName(string name)
        {
            this._name = name;
        }

        /// <summary>
        /// Get the database table name of the model class
        /// </summary>
        /// <returns>Database table name of the model class</returns>
        public string GetTableName()
        {
            return this._name;
        }
    }
}
