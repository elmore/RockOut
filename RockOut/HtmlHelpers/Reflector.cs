using System;

namespace RockOut.HtmlHelpers
{
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