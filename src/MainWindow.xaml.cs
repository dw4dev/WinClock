using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

using NAudio.Wave;

namespace WinClock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DispatcherTimer timer;
        private string nowDate = "", lunarDate = "";
        private const double BASE_WIDTH = 330;
        private const double TIME_FONT_SIZE = 64;
        private const double DATE_FONT_SIZE = 32;   // 預設 ~32 → 在 330px 寬時剛好比例正確
        private const double LUNAR_FONT_SIZE = 16;  // 農曆文字基準
        private const double TERM_FONT_SIZE = 14;   // 節氣標籤基準

        Dictionary<string, AlarmCfg> alarmCfgs = [];
        Dictionary<string, DispatcherTimer> alarms = [];

        public bool IsDarkTheme { get; private set; } = true;
        private Brush activePinBrush = Brushes.Transparent;
        private Brush activeAlarmBrush = Brushes.Transparent;

        // 季節標籤背景與前景顏色 (會根據主題在 ApplyTheme 中動態更新)
        private Brush bc一般 = Brushes.Transparent;
        private Brush fc一般 = Brushes.Transparent;
        private Brush bc立春 = Brushes.Transparent;
        private Brush fc立春 = Brushes.Transparent;
        private Brush bc立夏 = Brushes.Transparent;
        private Brush fc立夏 = Brushes.Transparent;
        private Brush bc立秋 = Brushes.Transparent;
        private Brush fc立秋 = Brushes.Transparent;
        private Brush bc立冬 = Brushes.Transparent;
        private Brush fc立冬 = Brushes.Transparent;

        public MainWindow()
        {
            InitializeComponent();

            // 顯示版本資訊 (由 Nerdbank.GitVersioning 自動產生)
            VersionText.Text = $"v{ThisAssembly.AssemblyInformationalVersion}";

            ApplyTheme(true); // 預設使用深色磨砂主題

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

        static readonly string[] weekdays = ["日", "一", "二", "三", "四", "五", "六"];
        private void UpdateTimeDisplay()
        {
            try {
                var now = DateTime.Now;
                var dow = (int)now.DayOfWeek;
                var currentDate = now.ToString($"yyyy/MM/dd");

                currentDate = $"{currentDate} ({weekdays[dow]})";

                lunarDate = GetChineseDate(now);
                var solarTerm = GetSolarTerm(now);

                if (currentDate != nowDate) {
                    nowDate = currentDate;
                }

                var hasSolarTerm = !string.IsNullOrEmpty(solarTerm);
                LunarTerm.Visibility = hasSolarTerm ? Visibility.Visible : Visibility.Collapsed;

                if (hasSolarTerm) {
                    LunarTermText.Text = solarTerm;
                    if (solarTerm.StartsWith("立")) {
                        switch (solarTerm) {
                            case "立春":
                                LunarTerm.Background = bc立春;
                                LunarTermText.Foreground = fc立春;
                                break;
                            case "立夏":
                                LunarTerm.Background = bc立夏;
                                LunarTermText.Foreground = fc立夏;
                                break;
                            case "立秋":
                                LunarTerm.Background = bc立秋;
                                LunarTermText.Foreground = fc立秋;
                                break;
                            case "立冬":
                                LunarTerm.Background = bc立冬;
                                LunarTermText.Foreground = fc立冬;
                                break;
                            default:
                                LunarTerm.Background = bc一般;
                                LunarTermText.Foreground = fc一般;
                                break;
                        }
                    }
                    else {
                        LunarTerm.Background = bc一般;
                        LunarTermText.Foreground = fc一般;
                    }
                }

                DateText.Text = currentDate;
                LunarDateText.Text = lunarDate;
                TimeText.Text = now.ToString("HH:mm:ss");
            }
            catch (Exception ex) {
                Debug.WriteLine(ex.Message);
            }
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
            LunarDateText.FontSize = LUNAR_FONT_SIZE * scale;
            LunarTermText.FontSize = TERM_FONT_SIZE * scale;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnPinTop_Click(object sender, RoutedEventArgs e)
        {
            this.Topmost = !this.Topmost;
            UpdateButtonVisuals();
        }

        private void btnThemeToggle_Click(object sender, RoutedEventArgs e)
        {
            ApplyTheme(!IsDarkTheme);
        }

        private void ApplyTheme(bool isDark)
        {
            IsDarkTheme = isDark;

            // 定義深淺色主題配色
            Color winBgColor = isDark ? Color.FromRgb(30, 30, 36) : Color.FromRgb(240, 240, 245);
            Color winBorderColor = isDark ? Color.FromArgb(51, 255, 255, 255) : Color.FromArgb(34, 0, 0, 0);
            Color textPrimaryColor = isDark ? Colors.White : Color.FromRgb(28, 25, 23);
            Color textSecondaryColor = isDark ? Color.FromRgb(160, 165, 181) : Color.FromRgb(87, 83, 78);
            Color iconColor = isDark ? Color.FromRgb(208, 211, 219) : Color.FromRgb(71, 85, 105);
            Color btnHoverColor = isDark ? Color.FromArgb(30, 255, 255, 255) : Color.FromArgb(20, 0, 0, 0);
            Color btnPressedColor = isDark ? Color.FromArgb(60, 255, 255, 255) : Color.FromArgb(40, 0, 0, 0);

            // 更新動態資源筆刷 (這將自動更新套用 DynamicResource 的 UI 元素)
            this.Resources["WindowBackground"] = new SolidColorBrush(Color.FromArgb((byte)(isDark ? 217 : 225), winBgColor.R, winBgColor.G, winBgColor.B));
            this.Resources["WindowBorder"] = new SolidColorBrush(winBorderColor);
            this.Resources["TextPrimary"] = new SolidColorBrush(textPrimaryColor);
            this.Resources["TextSecondary"] = new SolidColorBrush(textSecondaryColor);
            this.Resources["IconForeground"] = new SolidColorBrush(iconColor);
            this.Resources["ButtonHoverBackground"] = new SolidColorBrush(btnHoverColor);
            this.Resources["ButtonPressedBackground"] = new SolidColorBrush(btnPressedColor);

            // 設定釘選與鬧鐘啟用時的半透明效果色
            activePinBrush = new SolidColorBrush(isDark ? Color.FromArgb(64, 16, 185, 129) : Color.FromArgb(48, 16, 185, 129));
            activeAlarmBrush = new SolidColorBrush(isDark ? Color.FromArgb(64, 239, 68, 68) : Color.FromArgb(48, 239, 68, 68));

            // 更新節氣標籤調色盤 (莫蘭迪清新半透明配色)
            if (isDark) {
                bc一般 = new SolidColorBrush(Color.FromArgb(40, 255, 255, 255));
                fc一般 = new SolidColorBrush(Colors.White);

                bc立春 = new SolidColorBrush(Color.FromArgb(50, 16, 185, 129));
                fc立春 = new SolidColorBrush(Color.FromRgb(52, 211, 153));

                bc立夏 = new SolidColorBrush(Color.FromArgb(50, 239, 68, 68));
                fc立夏 = new SolidColorBrush(Color.FromRgb(252, 165, 165));

                bc立秋 = new SolidColorBrush(Color.FromArgb(50, 245, 158, 11));
                fc立秋 = new SolidColorBrush(Color.FromRgb(251, 191, 36));

                bc立冬 = new SolidColorBrush(Color.FromArgb(50, 59, 130, 246));
                fc立冬 = new SolidColorBrush(Color.FromRgb(147, 197, 253));
            }
            else {
                bc一般 = new SolidColorBrush(Color.FromArgb(20, 0, 0, 0));
                fc一般 = new SolidColorBrush(Color.FromRgb(30, 41, 59));

                bc立春 = new SolidColorBrush(Color.FromRgb(230, 253, 245));
                fc立春 = new SolidColorBrush(Color.FromRgb(5, 150, 105));

                bc立夏 = new SolidColorBrush(Color.FromRgb(254, 242, 242));
                fc立夏 = new SolidColorBrush(Color.FromRgb(220, 38, 38));

                bc立秋 = new SolidColorBrush(Color.FromRgb(254, 243, 199));
                fc立秋 = new SolidColorBrush(Color.FromRgb(217, 119, 6));

                bc立冬 = new SolidColorBrush(Color.FromRgb(239, 246, 255));
                fc立冬 = new SolidColorBrush(Color.FromRgb(37, 99, 235));
            }

            // 更新主題圖示
            var sunData = "M12,9A3,3 0 0,0 9,12A3,3 0 0,0 12,15A3,3 0 0,0 15,12A3,3 0 0,0 12,9M12,2A1,1 0 0,1 13,3V5A1,1 0 0,1 12,6A1,1 0 0,1 11,5V3A1,1 0 0,1 12,2M12,18A1,1 0 0,1 13,19V21A1,1 0 0,1 12,22A1,1 0 0,1 11,21V19A1,1 0 0,1 12,18M20,13H22A1,1 0 0,1 23,12A1,1 0 0,1 22,11H20A1,1 0 0,1 19,12A1,1 0 0,1 20,13M2,12A1,1 0 0,1 3,11H5A1,1 0 0,1 6,12A1,1 0 0,1 5,13H3A1,1 0 0,1 2,12M17.66,6.34A1,1 0 0,1 17.66,7.76L16.24,9.18A1,1 0 0,1 14.83,9.18A1,1 0 0,1 14.83,7.76L16.24,6.34A1,1 0 0,1 17.66,6.34M7.76,16.24A1,1 0 0,1 7.76,17.66L6.34,19.07A1,1 0 0,1 4.93,19.07A1,1 0 0,1 4.93,17.66L6.34,16.24A1,1 0 0,1 7.76,16.24M19.07,17.66A1,1 0 0,1 17.66,19.07L16.24,17.66A1,1 0 0,1 16.24,16.24A1,1 0 0,1 17.66,16.24L19.07,17.66M6.34,4.93A1,1 0 0,1 7.76,4.93L9.18,6.34A1,1 0 0,1 9.18,7.76A1,1 0 0,1 7.76,7.76L6.34,6.34A1,1 0 0,1 6.34,4.93Z";
            // 下弦月（左側亮弧 + 右側陰影遮蔽，明顯的月牙形狀）
            var moonData = "M12 2C9.2 2 6.6 3 4.7 4.9 2.8 6.8 2 9.2 2 12s.8 5.2 2.7 7.1C6.6 21 9.2 22 12 22c4.8 0 8.8-3 10.4-7.3-1 .4-2.1.6-3.2.6-5 0-9.2-4.2-9.2-9.2 0-1.7.5-3.3 1.3-4.7C10.6 2.5 11.3 2 12 2z";
            themeIcon.Data = Geometry.Parse(isDark ? sunData : moonData);

            // 更新按鈕視覺狀態
            UpdateButtonVisuals();
            UpdateTimeDisplay();
        }

        private void UpdateButtonVisuals()
        {
            // 釘選按鈕
            btnPinTop.Background = this.Topmost ? activePinBrush : Brushes.Transparent;

            // 鬧鐘按鈕 1 與 2
            UpdateAlarmButtonVisual("1", btnAlarm1);
            UpdateAlarmButtonVisual("2", btnAlarm2);
        }

        private void UpdateAlarmButtonVisual(string key, Button btn)
        {
            if (alarmCfgs.TryGetValue(key, out var cfg) && cfg.IsEnabled) {
                btn.Background = activeAlarmBrush;
            }
            else {
                btn.Background = Brushes.Transparent;
            }
        }

        private void btnAlarm_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn || btn.Tag == null) return;
            var key = btn.Tag.ToString() ?? "";
            if (!alarmCfgs.TryGetValue(key, out var alarmCfg))
                alarmCfg = new();

            if (!alarmCfg.IsEnabled)
                alarmCfg.AddMinutes(5);

            var alarmSetupWindow = new AlarmSetupWindow {
                MyAlarmCfg = alarmCfg,
                IsDarkTheme = this.IsDarkTheme
            };
            alarmSetupWindow.Topmost = this.Topmost;
            alarmSetupWindow.ShowDialog();
            if (alarmSetupWindow.DialogResult == true) {
                // 清除鬧鐘
                if (alarmSetupWindow.MyAlarmCfg == null) {
                    CleanAlarm(key);
                    UpdateButtonVisuals();
                    return;
                }

                // 設定鬧鐘
                alarmCfgs[key] = alarmSetupWindow.MyAlarmCfg;
                var cfg = alarmCfgs[key];
                if (cfg.IsEnabled) {
                    // 鬧鐘時間異動，重新設定
                    if (alarms.TryGetValue(key, out var timer)) {
                        timer.Stop();
                        timer.Interval = cfg.GetTimeSpan();
                        timer.Start();
                        UpdateButtonVisuals();
                        return;
                    }
                }

                alarms[key] = new() { Interval = cfg.GetTimeSpan() };
                alarms[key].Tick += (s, e) => {

                    MsgWindow msgWindow;
                    if (cfg.IsShowMsg) {
                        msgWindow = new MsgWindow {
                            EnableFadeIn = true,
                            HeaderText = $"鬧鐘{key}",
                            MessageText = cfg?.MsgText ?? $"鬧鐘{key}時間到",
                            IsDarkTheme = this.IsDarkTheme,
                            MyAlarmCfg = cfg
                        };
                    }
                    else {
                        msgWindow = new MsgWindow {
                            EnableFadeIn = true,
                            HeaderText = $"鬧鐘{key}",
                            MessageText = $"鬧鐘{key}時間到",
                            IsDarkTheme = this.IsDarkTheme,
                            MyAlarmCfg = cfg
                        };
                    }

                    alarms[key].Stop();
                    alarmCfgs[key].IsEnabled = false;
                    CleanAlarm(key);
                    UpdateButtonVisuals();

                    msgWindow.ShowDialog();
                };

                alarms[key].Start();
                cfg.IsEnabled = true;
                UpdateButtonVisuals();
            }
        }

        /// <summary>
        /// 表示清除指定鬧鐘
        /// </summary>
        void CleanAlarm(string key)
        {
            alarmCfgs.Remove(key);
            if (alarms.TryGetValue(key, out var timer)) {
                timer.Stop();
                alarms.Remove(key);
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

        // 農曆名稱
        static readonly string[] ChineseMonths = [
            "??",
            "正月", "二月", "三月", "四月", "五月", "六月",
            "七月", "八月", "九月", "十月", "冬月", "臘月"
        ];

        /// <summary>
        /// 取得農曆日期
        /// </summary>
        string GetChineseDate(DateTime wdt)
        {
            ChineseLunisolarCalendar lunar = new ChineseLunisolarCalendar();
            DateTime now = wdt;

            int year = lunar.GetYear(now);
            int month = lunar.GetMonth(now);
            int day = lunar.GetDayOfMonth(now);

            // 判斷是否為閏月
            bool isLeapMonth = lunar.IsLeapMonth(year, month > 12 ? month - 12 : month);
            // 真正的月份 (去掉閏月偏移)
            int displayMonth = month > 12 ? month - 12 : month;
            // 如果是閏月，前面加上「閏」
            string monthText = (isLeapMonth ? "閏" : "") + ChineseMonths[displayMonth];

            var dayText = GetChineseDayName(day);

            return $"農曆 {monthText}{dayText}";
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

        /// <summary>
        /// 取得指定日期時間的節氣
        /// </summary>
        string GetSolarTerm(DateTime dateTime)
        {
            if (DTHelper.SolarTerms.TryGetValue(dateTime.Year, out var yearTerms)) {
                var term = yearTerms.FirstOrDefault(t =>
                    t.Month == dateTime.Month &&
                    t.Day == dateTime.Day);

                if (term != null) {
                    var termTime = new DateTime(dateTime.Year, term.Month, term.Day, 0, 0, 0);
                    if (dateTime >= termTime) {
                        return $"{term.Name} @{term.Hour:D2}:{term.Minute:D2}";
                    }
                }
            }
            return string.Empty;
        }
        #endregion
    }
}