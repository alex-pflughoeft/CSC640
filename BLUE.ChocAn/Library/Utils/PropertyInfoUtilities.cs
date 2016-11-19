using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BLUE.ChocAn.Library.Utils
{
    public static class PropertyInfoUtilities
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
