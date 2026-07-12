using System;
using System.Globalization;
using System.Windows.Data;

namespace GitLocalService.Converters
{
    public class StringEqualsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Equals(value?.ToString(), parameter?.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {
                return parameter?.ToString();
            }
            return Binding.DoNothing;
        }
    }
}