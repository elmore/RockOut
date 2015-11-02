using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace RockOut.HtmlHelpers
{
    public class RockOut
    {
        public static HtmlString Foreach(object model, string collectionFieldName, Func<dynamic, RepeatedHtmlElement> template)
        {
            var sb = new StringBuilder();

            bool first = true;

            var collection = Reflector.GetValue(model, collectionFieldName) as IEnumerable<object>;

            if(collection == null)            
            {
                throw new ArgumentException(string.Format("The field '{0}' is not IEnumerable<object>", collectionFieldName));
            }

            foreach (dynamic el in collection)
            {
                RepeatedHtmlElement repeated = template(el);

                ComposableHtmlString s = first ? repeated.AsFirstElement(el) : repeated.AsElement(el);

                sb.AppendLine(s.ToString());

                first = false;
            }

            string htmlString = sb.ToString();

            return new ComposableHtmlString(htmlString);
        }

        public static RepeatedHtmlElement Li(string text, IDictionary<string, string> attributes) 
        {
            return new RepeatedHtmlElement(text, attributes);
        }
    }

    public class ComposableHtmlString : HtmlString
    {
        public ComposableHtmlString(string value) : base(value) { }
    }
}
