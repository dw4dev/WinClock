using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace WinClock
{
    /// <summary>把 <see cref="ColorScheme"/> 的文字值換成 WPF 顏色與筆刷。</summary>
    public static class ThemeBrushes
    {
        /// <summary>解析 #RRGGBB；格式不對時回傳 null。</summary>
        public static Color? ParseColor(string? text)
        {
            if (text is null) return null;
            var s = text.Trim().TrimStart('#');
            if (s.Length != 6 || !int.TryParse(s, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var rgb)) return null;
            return Color.FromRgb((byte)(rgb >> 16), (byte)(rgb >> 8), (byte)rgb);
        }

        public static string FormatColor(Color color) => $"#{color.R:X2}{color.G:X2}{color.B:X2}";

        /// <summary>背景筆刷：單色或 2 色漸層，套用不透明度。</summary>
        public static Brush CreateBackground(ColorScheme scheme)
        {
            var alpha = (byte)Math.Round(scheme.Opacity * 255);
            var first = WithAlpha(ParseColor(scheme.Background) ?? Colors.Black, alpha);
            if (!scheme.UseGradient) return Frozen(new SolidColorBrush(first));

            var second = WithAlpha(ParseColor(scheme.Background2) ?? Colors.Black, alpha);
            // 角度 0 為由左到右，順時針增加（螢幕座標 y 向下）。
            var radians = scheme.GradientAngle * Math.PI / 180;
            var dx = Math.Cos(radians) / 2;
            var dy = Math.Sin(radians) / 2;
            return Frozen(new LinearGradientBrush(first, second, new Point(0.5 - dx, 0.5 - dy), new Point(0.5 + dx, 0.5 + dy)));
        }

        public static Brush CreateText(ColorScheme scheme) =>
            Frozen(new SolidColorBrush(ParseColor(scheme.Text) ?? Colors.White));

        private static Color WithAlpha(Color color, byte alpha) => Color.FromArgb(alpha, color.R, color.G, color.B);

        private static Brush Frozen(Brush brush)
        {
            brush.Freeze();
            return brush;
        }
    }
}
