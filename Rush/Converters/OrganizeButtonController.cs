using System;
using System.Windows.Data;

namespace Rush.Converters
{
    public class OrganizeButtonController :IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Length < 1)
                return false;
            var res = false;
            foreach (var val in values)
            {
                if (val is bool)
                    res = (bool)val;
                else
                {
                    res = false;
                }
            }

            return res;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
