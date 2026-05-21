using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace WinClock
{
    /// <summary>
    /// MsgWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MsgWindow : Window
    {
        public string HeaderText
        {
            get { return (string)GetValue(HeaderTextProperty); }
            set { SetValue(HeaderTextProperty, value); }
        }

        public static readonly DependencyProperty HeaderTextProperty =
            DependencyProperty.Register("HeaderText", typeof(string), typeof(MsgWindow),
                new PropertyMetadata(""));

        public string MessageText
        {
            get { return (string)GetValue(MessageTextProperty); }
            set { SetValue(MessageTextProperty, value); }
        }

        public static readonly DependencyProperty MessageTextProperty =
            DependencyProperty.Register("MessageText", typeof(string), typeof(MsgWindow),
                new PropertyMetadata(""));

        public bool EnableFadeIn
        {
            get { return (bool)GetValue(EnableFadeInProperty); }
            set { SetValue(EnableFadeInProperty, value); }
        }

        public static readonly DependencyProperty EnableFadeInProperty =
            DependencyProperty.Register("EnableFadeIn", typeof(bool), typeof(MsgWindow),
                new PropertyMetadata(false));

        public bool IsDarkTheme
        {
            get { return (bool)GetValue(IsDarkThemeProperty); }
            set { SetValue(IsDarkThemeProperty, value); }
        }

        public static readonly DependencyProperty IsDarkThemeProperty =
            DependencyProperty.Register("IsDarkTheme", typeof(bool), typeof(MsgWindow),
                new PropertyMetadata(true, OnIsDarkThemeChanged));

        private static void OnIsDarkThemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MsgWindow window) {
                window.ApplyTheme((bool)e.NewValue);
            }
        }

        public MsgWindow()
        {
            InitializeComponent();
            ApplyTheme(IsDarkTheme);
        }

        private void ApplyTheme(bool isDark)
        {
            Color winBgColor = isDark ? Color.FromRgb(30, 30, 36) : Color.FromRgb(245, 245, 250);
            Color winBorderColor = isDark ? Color.FromArgb(51, 255, 255, 255) : Color.FromArgb(34, 0, 0, 0);
            Color textPrimaryColor = isDark ? Colors.White : Color.FromRgb(28, 25, 23);
            Color textSecondaryColor = isDark ? Color.FromRgb(160, 165, 181) : Color.FromRgb(87, 83, 78);
            Color iconColor = isDark ? Color.FromRgb(208, 211, 219) : Color.FromRgb(71, 85, 105);
            Color btnHoverColor = isDark ? Color.FromArgb(30, 255, 255, 255) : Color.FromArgb(20, 0, 0, 0);
            Color btnPressedColor = isDark ? Color.FromArgb(60, 255, 255, 255) : Color.FromArgb(40, 0, 0, 0);

            this.Resources["WindowBackground"] = new SolidColorBrush(Color.FromArgb((byte)(isDark ? 230 : 255), winBgColor.R, winBgColor.G, winBgColor.B));
            this.Resources["WindowBorder"] = new SolidColorBrush(winBorderColor);
            this.Resources["TextPrimary"] = new SolidColorBrush(textPrimaryColor);
            this.Resources["TextSecondary"] = new SolidColorBrush(textSecondaryColor);
            this.Resources["IconForeground"] = new SolidColorBrush(iconColor);
            this.Resources["ButtonHoverBackground"] = new SolidColorBrush(btnHoverColor);
            this.Resources["ButtonPressedBackground"] = new SolidColorBrush(btnPressedColor);
            this.Resources["AccentBrush"] = new SolidColorBrush(isDark ? Color.FromRgb(129, 191, 218) : Color.FromRgb(37, 99, 235));
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (EnableFadeIn) {
                var fadeInAnimation = new DoubleAnimation {
                    From = 0,                // 起始透明度
                    To = 1,                  // 結束透明度
                    Duration = TimeSpan.FromSeconds(1), // 動畫持續時間 (原為3秒，1秒較為自然俐落)
                    FillBehavior = FillBehavior.HoldEnd // 動畫結束後保持狀態
                };

                // 將動畫應用到視窗的 Opacity 屬性
                this.BeginAnimation(OpacityProperty, fadeInAnimation);
            }
        }
    }
}
