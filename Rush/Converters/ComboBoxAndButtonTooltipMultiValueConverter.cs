using System;
using System.Windows.Controls;
using System.Windows.Data;

namespace Rush.Converters
{
    public class ComboBoxAndButtonTooltipMultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Length < 2 || values[0]==null || values[1]==null) return "";
            if (!(values[0] is int)) return "";
            var index = (int) values[0];
            if (index <= -1) return "";
            var item = values[1] as ComboBoxItem;
            if (item != null)
                return string.Format("remove {0} from the source locations list",
                    item.Content);
            return "";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
