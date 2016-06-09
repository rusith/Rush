using System;
using System.Linq;
using System.Windows.Data;

namespace Rush.Converters
{
    public class OrganizeButtonConverter :IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return values.Length >= 1 && values.All(val => (bool) val);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
