using System.Collections.Generic;

namespace SensorService.UI.Extensions
{
    public static class StringExtensions
    {
        public static string ToCommaSeparatedList(this IEnumerable<string> list,string wrapperChar = "")
        {
            var str = string.Empty;
            foreach (var item in list)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    str += ",";
                }

                str += wrapperChar +  item + wrapperChar;
            }

            return str;
        }
    }
}
