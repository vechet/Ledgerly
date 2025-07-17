using System.ComponentModel;

namespace Ledgerly.Helpers
{
    public static class EnumHelper
    {
        public static int Value<T>(this T source)
        {
            return Convert.ToInt32(source);
        }

        public static string Description<T>(this T source)
        {
            var field = source.GetType().GetField(source.ToString());
            if (field == null)
            {
                return source.ToString();
            }
            var attributes = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : source.ToString();
        }
    }
}