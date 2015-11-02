using System.Collections.Generic;
using System.Linq;

namespace RockOut.HtmlHelpers
{
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
}