using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;

namespace RockOut.HtmlHelpers
{
    public class RockOut
    {
        public static HtmlString Foreach(IEnumerable collection, Func<dynamic, object> template)
        {
            var sb = new StringBuilder();

            foreach (dynamic el in collection)
            {
                sb.AppendLine(template(el).ToString());
            }

            string htmlString = sb.ToString();

            return new HtmlString(htmlString);
        }

        public static HtmlString Li(string text, IDictionary<string, object> attributes) // 
        {

            //var li = new HtmlGenericControl();
            ////li.Attributes.Add("key", attributes.key);
            //li.TagName = "li";
            //li.InnerText = text;
            //string htmlString = li.RenderControl().ToString();


            foreach (var attr in attributes)
            {

            }



            string htmlString = string.Format("<li data-bind=\"text : Label, attr : {{ key : Value }}\" key=\"{0}\">{1}</li>", attributes["key"], text);





            return new HtmlString(htmlString);
        }
    }
}
