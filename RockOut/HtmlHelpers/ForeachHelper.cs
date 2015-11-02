using System;
using System.Collections;
using System.Collections.Generic;
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
            var attrSb = new StringBuilder();
            var koSb = new StringBuilder();

            foreach (var attr in _attributes)
            {
                koSb.AppendFormat("{0} : {1}", attr.Key, attr.Value);
                attrSb.AppendFormat("{0}=\"{1}\"", attr.Key, attr.Value);
            }

            string htmlString = string.Format("<li data-bind=\"text : Label, attr : {{ {0} }}\" {1}>{2}</li>", koSb, attrSb, _text);

            return new HtmlString(htmlString);
        }

        public HtmlString AsElement()
        {
            var attrSb = new StringBuilder();

            foreach (var attr in _attributes)
            {
                attrSb.AppendFormat("{0}=\"{1}\"", attr.Key, attr.Value);
            }

            string htmlString = string.Format("<li {0}>{1}</li>", attrSb, _text);

            return new HtmlString(htmlString);
        }
    }
}
