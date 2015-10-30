using System;
using System.Collections;
using System.Text;
using System.Web;

namespace RockOut.HtmlHelpers
{
    public class RockOut
    {
        public static HtmlString Foreach(IEnumerable collection, Func<dynamic, string> template)
        {
            var sb = new StringBuilder();

            foreach (dynamic el in collection)
            {
                sb.AppendLine(template(el));
            }

            string htmlString = sb.ToString();

            return new HtmlString(htmlString);
        }
    }
}
