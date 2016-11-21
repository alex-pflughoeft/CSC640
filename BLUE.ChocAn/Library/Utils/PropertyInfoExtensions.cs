using System.Reflection;

namespace BLUE.ChocAn.Library.Utils
{
    public static class PropertyInfoExtensions
    {
        public static bool IsPrimaryKey(this PropertyInfo propertyInfo)
        {
            foreach (var attribute in propertyInfo.CustomAttributes)
            {
                if (attribute.AttributeType == typeof(BLUE.ChocAn.Library.PrimaryKeyAttribute))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
