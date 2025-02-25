using System.ComponentModel;
using System.Globalization;

namespace Chatty.Agents.Personalities
{
    public class PersonalityTypeConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
        {
            if (destinationType == typeof(string))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }

        public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type? destinationType)
        {
            if (destinationType == typeof(string) && value is Personality personality)
            {
                return personality.ToString();
            }

            return base.ConvertTo(context, culture, value, destinationType!);
        }
    }
}
