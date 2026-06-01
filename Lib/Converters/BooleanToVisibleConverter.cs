using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SoundEffect.Lib.Converters
{
    internal class BooleanToVisibleConverter : IValueConverter
    {
        public bool IsReversed { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue = value is bool b && b;
            if (IsReversed)
                boolValue = !boolValue;
            return boolValue ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
