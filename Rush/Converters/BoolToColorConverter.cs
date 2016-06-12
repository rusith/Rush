﻿using System;
using System.Windows.Data;
using System.Windows.Media;

namespace Rush.Converters
{
    class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return Brushes.Tomato;
            var s = value as string;
            if (s != null)
                return s.ToUpper() == "TRUE" ? Brushes.CornflowerBlue : Brushes.Tomato;
            return ((bool)value) ? Brushes.CornflowerBlue : Brushes.Tomato;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
