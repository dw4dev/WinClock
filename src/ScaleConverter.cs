using System.Globalization;
using System.Windows.Data;

namespace WinClock
{
    /// <summary>數值乘以 ConverterParameter（預設 1）。用來讓勾選框大小跟隨字型大小。</summary>
    public class ScaleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var factor = double.TryParse(parameter as string, NumberStyles.Float, CultureInfo.InvariantCulture, out var f) ? f : 1.0;
            return value is double d ? d * factor : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotSupportedException();
    }
}
