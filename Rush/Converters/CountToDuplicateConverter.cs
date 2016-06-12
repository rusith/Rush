using System;
using System.Windows.Data;

namespace Rush.Converters
{
    public class CountToDuplicateConverter :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is int && (int) value > 0)
                return string.Format("{0} duplicate{1}",value,(int)value>1?"s":"");
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
