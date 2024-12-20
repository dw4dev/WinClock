using System.Windows;
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

        public MsgWindow()
        {
            InitializeComponent();
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
                    Duration = TimeSpan.FromSeconds(3), // 動畫持續時間
                    FillBehavior = FillBehavior.HoldEnd // 動畫結束後保持狀態
                };

                // 將動畫應用到視窗的 Opacity 屬性
                this.BeginAnimation(OpacityProperty, fadeInAnimation);
            }
        }
    }
}
