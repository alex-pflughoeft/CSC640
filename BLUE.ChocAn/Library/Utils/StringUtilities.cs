using System.Linq;

namespace BLUE.ChocAn.Library.Utils
{
    public static class StringUtilities
    {
        public static string ToUnderscoreCase(this string str)
        {
            return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
        }
    }
}
