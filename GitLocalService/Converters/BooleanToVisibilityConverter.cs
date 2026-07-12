using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace GitLocalService.Converters
{
    /// <summary>
    /// 将布尔值转换为 Visibility 枚举值的转换器
    /// <para>true → Visibility.Visible</para>
    /// <para>false → Visibility.Collapsed</para>
    /// </summary>
    public class BooleanToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// 将布尔值转换为 Visibility
        /// </summary>
        /// <param name="value">布尔值</param>
        /// <param name="targetType">目标类型（Visibility）</param>
        /// <param name="parameter">参数（可选）</param>
        /// <param name="culture">文化信息</param>
        /// <returns>Visibility.Visible 或 Visibility.Collapsed</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool boolValue && boolValue ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// 将 Visibility 转换为布尔值（反向转换）
        /// </summary>
        /// <param name="value">Visibility 值</param>
        /// <param name="targetType">目标类型（bool）</param>
        /// <param name="parameter">参数（可选）</param>
        /// <param name="culture">文化信息</param>
        /// <returns>true（Visible）或 false（其他）</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Visibility visibility && visibility == Visibility.Visible;
        }
    }
}