using System.Globalization;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace WinClock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DispatcherTimer timer;
        private string nowDate = "", lunarDate = "";
        private const double BASE_WIDTH = 320;
        private const double TIME_FONT_SIZE = 68;
        private const double DATE_FONT_SIZE = 30;

        public MainWindow()
        {
            InitializeComponent();

            timer = new DispatcherTimer {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += Timer_Tick;

            StateChanged += MainWindow_StateChanged;

            timer.Start();
            UpdateTimeDisplay();
        }

        private void MainWindow_StateChanged(object? sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized) {
                timer.Stop();
            }
            else {
                timer.Start();
                UpdateTimeDisplay();
            }
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            UpdateTimeDisplay();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) {
                this.DragMove();
            }
        }

        static string[] weekdays = { "日", "一", "二", "三", "四", "五", "六" };
        private void UpdateTimeDisplay()
        {
            var now = DateTime.Now;
            var dow = (int)now.DayOfWeek;
            var currentDate = now.ToString($"yyyy/MM/dd");

            currentDate = $"{currentDate} ({weekdays[dow]})";

            lunarDate = GetChineseDate(now);
            if (currentDate != nowDate) {
                nowDate = currentDate;
                DateText.Text = currentDate;
                LunarDateText.Text = lunarDate;
            }

            TimeText.Text = now.ToString("HH:mm:ss");
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateFontSizes();
        }

        private void UpdateFontSizes()
        {
            double scale = this.Width / BASE_WIDTH;
            TimeText.FontSize = TIME_FONT_SIZE * scale;
            DateText.FontSize = DATE_FONT_SIZE * scale;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnPinTop_Click(object sender, RoutedEventArgs e)
        {
            if (this.Topmost) {
                this.Topmost = false;
                btnPinTop.Background = System.Windows.Media.Brushes.Transparent;
            }
            else {
                this.Topmost = true;
                btnPinTop.Background = System.Windows.Media.Brushes.DarkSeaGreen;
            }
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ResizeThumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            double newWidth = Math.Max(MinWidth, Width + e.HorizontalChange);
            Width = newWidth;
        }

        #region 處理農曆

        static readonly string[] ChineseMonths = {
            "正月", "二月", "三月", "四月", "五月", "六月",
            "七月", "八月", "九月", "十月", "冬月", "臘月"
        };
        /// <summary>
        /// 取得農曆日期
        /// </summary>
        string GetChineseDate(DateTime wdt)
        {
            ChineseLunisolarCalendar lunar = new ChineseLunisolarCalendar();
            DateTime now = wdt;

            //int year = lunar.GetYear(now);
            int month = lunar.GetMonth(now);
            int day = lunar.GetDayOfMonth(now);

            var dayText = GetChineseDayName(day);

            //return $"農曆 {year}年{month}月{day}日";
            return $"農曆 {ChineseMonths[month]}{dayText}";
        }

        static string GetChineseDayName(int day)
        {
            string[] numbers = { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九", "十" };
            if (day < 11)
                return "初" + numbers[day];
            if (day < 20)
                return "十" + numbers[day % 10];
            if (day == 20)
                return "二十";
            if (day < 30)
                return "廿" + numbers[day % 10];
            return day == 30 ? "三十" : "三十" + numbers[day % 10];
        }

        #endregion
    }
}