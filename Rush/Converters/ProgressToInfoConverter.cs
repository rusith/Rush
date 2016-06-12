using System;
using System.Windows.Data;

namespace Rush.Converters
{
    public class ProgressToInfoConverter :IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Length>1 && values[0] is double && values[1] is double)
            {
                return (double) values[1] < 1 ? "" : string.Format("{0}|{1}", values[1], values[0]);
            }
            return "";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
