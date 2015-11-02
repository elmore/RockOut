using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace RockOut.HtmlHelpers
{
    public class RockOut
    {
        public static HtmlString Foreach(IEnumerable collection, Func<dynamic, RepeatedHtmlElement> template)
        {
            var sb = new StringBuilder();

            bool first = true;

            foreach (dynamic el in collection)
            {
                RepeatedHtmlElement repeated = template(el);

                HtmlString s = first ? repeated.AsFirstElement(el) : repeated.AsElement(el);

                sb.AppendLine(s.ToString());

                first = false;
            }

            string htmlString = sb.ToString();

            return new HtmlString(htmlString);
        }

        public static RepeatedHtmlElement Li(string text, IDictionary<string, string> attributes) 
        {
            return new RepeatedHtmlElement(text, attributes);
        }
    }

    public class RepeatedHtmlElement
    {
        private readonly string _text;
        private readonly IDictionary<string, string> _attributes;

        public RepeatedHtmlElement(string text, IDictionary<string, string> attributes)
        {
            _text = text;
            _attributes = attributes;
        }

        public HtmlString AsFirstElement(object model)
        {
            var allAtts = GetPropertyValues(model);

            allAtts.Add("data-bind", TemplateKoString());

            string htmlString = Template(allAtts);

            return new HtmlString(htmlString);
        }

        public HtmlString AsElement(object model)
        {
            var allAtts = GetPropertyValues(model);

            string htmlString = Template(allAtts);

            return new HtmlString(htmlString);
        }

        private string Template(IDictionary<string, string> allAtts)
        {
            var atts = allAtts.Select(attr => string.Format("{0}=\"{1}\"", attr.Key, attr.Value)).ToList();

            var attrSb = string.Join(" ", atts);

            return string.Format("<li {0}>{1}</li>", attrSb, _text);
        }

        private string TemplateKoString()
        {
            var koSb = _attributes.Select(attr => string.Format("{0} : {1}", attr.Key, attr.Value)).ToList();

            return string.Format("text : Label, attr : {{ {0} }}", string.Join(",", koSb));
        }

        private string GetValue(object model, string field)
        {
            Type objtype = model.GetType();

            var info = objtype.GetProperty(field);

            return info.GetValue(model, null).ToString();
        }

        private Dictionary<string, string> GetPropertyValues(object model)
        {
            return _attributes.ToDictionary(a => a.Key, a => GetValue(model, a.Value));
        }
    }
}
