using MySql.Data.MySqlClient;

namespace BLUE.ChocAn.Library.Utils
{
    public static class MySQLUtilities
    {
        public static int? GetNullableInt(this MySqlDataReader r, int ord)
        {
            if (r.IsDBNull(ord))
                return null;
            else
                return r.GetInt32(ord);
        }

        public static int? GetNullableInt(this MySqlDataReader r, string name)
        {
            return GetNullableInt(r, r.GetOrdinal(name));
        }

        public static string GetNullableString(this MySqlDataReader r, string name)
        {
            string value = r.GetValue<string>(name);

            if (value == null)
            {
                return string.Empty;
            }

            return value;
        }

        public static T GetValue<T>(this MySqlDataReader r, string name) where T : class
        {
            return GetValue<T>(r, r.GetOrdinal(name));
        }

        public static T GetValue<T>(this MySqlDataReader r, int ord) where T : class
        {
            return r[ord] as T;
        }
    }
}
