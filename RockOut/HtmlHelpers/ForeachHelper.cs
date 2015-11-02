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

                HtmlString s = first ? repeated.AsFirstElement() : repeated.AsElement();

                sb.AppendLine(s.ToString());

                first = false;
            }

            string htmlString = sb.ToString();

            return new HtmlString(htmlString);
        }

        public static RepeatedHtmlElement Li(string text, IDictionary<string, object> attributes) 
        {

            return new RepeatedHtmlElement(text, attributes);
        }
    }

    public class RepeatedHtmlElement
    {
        private readonly string _text;
        private readonly IDictionary<string, object> _attributes;

        public RepeatedHtmlElement(string text, IDictionary<string, object> attributes)
        {
            _text = text;
            _attributes = attributes;
        }

        public HtmlString AsFirstElement()
        {
            var allAtts = _attributes;

            allAtts.Add("data-bind", TemplateKoString());

            string htmlString = Template(allAtts);

            return new HtmlString(htmlString);
        }

        public HtmlString AsElement()
        {
            string htmlString = Template(_attributes);

            return new HtmlString(htmlString);
        }

        private string Template(IDictionary<string, object>  allAtts)
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
    }
}
