using System.Globalization;
using System.Windows.Data;

namespace WinClock
{
    public class BoolInverseConverter : IValueConverter
    {
        // 將布林值反轉
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue) {
                return !boolValue; // 反轉布林值
            }
            return false; // 如果傳入的不是布林值，返回 false
        }

        // 如果需要支援從 UI 回到原始布林值的轉換（如 Command 的 Execute），也可實現 ConvertBack
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue) {
                return !boolValue; // 反轉布林值
            }
            return false; // 如果傳入的不是布林值，返回 false
        }
    }
}
