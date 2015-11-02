using System;
using System.Collections.Generic;
using System.Linq;
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

    public class RepeatedHtmlElement
    {
        private readonly string _textFieldName;
        private readonly IDictionary<string, string> _attributes;

        public RepeatedHtmlElement(string textFieldName, IDictionary<string, string> attributes)
        {
            _textFieldName = textFieldName;
            _attributes = attributes;
        }

        public ComposableHtmlString AsFirstElement(object model)
        {
            var allAtts = GetPropertyValues(model);

            allAtts.Add("data-bind", TemplateKoString());

            string htmlString = Template(model, allAtts);

            return new ComposableHtmlString(htmlString);
        }

        public ComposableHtmlString AsElement(object model)
        {
            var allAtts = GetPropertyValues(model);

            string htmlString = Template(model, allAtts);

            return new ComposableHtmlString(htmlString);
        }

        private string Template(object model, IDictionary<string, string> allAtts)
        {
            var atts = allAtts.Select(attr => string.Format("{0}=\"{1}\"", attr.Key, attr.Value)).ToList();

            var attrSb = string.Join(" ", atts);

            return string.Format("<li {0}>{1}</li>", attrSb, Reflector.GetValue(model, _textFieldName));
        }

        private string TemplateKoString()
        {
            var koSb = _attributes.Select(attr => string.Format("{0} : {1}", attr.Key, attr.Value)).ToList();

            return string.Format("text : {0}, attr : {{ {1} }}", _textFieldName, string.Join(",", koSb));
        }

        private Dictionary<string, string> GetPropertyValues(object model)
        {
            return _attributes.ToDictionary(a => a.Key, a => Reflector.GetValue(model, a.Value).ToString());
        }
    }

    static class Reflector
    {
        public static object GetValue(object model, string field)
        {
            Type objtype = model.GetType();

            var info = objtype.GetProperty(field);

            if (info == null)
            {
                throw new ArgumentException(string.Format("Field '{0}' does not exist on type '{1}'",
                    field,
                    objtype.Name));
            }

            return info.GetValue(model, null);
        }
    }
}
