using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseApp.Shared.Extentions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            if (field != null)
            {
                var attr = (System.ComponentModel.DescriptionAttribute)Attribute.GetCustomAttribute(
                    field, typeof(System.ComponentModel.DescriptionAttribute));
                if (attr != null)
                {
                    return attr.Description;
                }
            }
            return value.ToString();
        }
    }
}
