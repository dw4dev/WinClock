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
        private const double BASE_WIDTH = 310;
        private const double TIME_FONT_SIZE = 68;
        private const double DATE_FONT_SIZE = 30;

        Dictionary<string, AlarmCfg> alarmCfgs = new();
        Dictionary<string, DispatcherTimer> alarms = new();

        static Brush bc一般 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF7D1"));
        static Brush fc一般 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#40534C"));

        static Brush bc立春 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FEFDED"));
        static Brush fc立春 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FA7070"));

        static Brush bc立夏 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFB996"));
        static Brush fc立夏 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0000"));

        static Brush bc立秋 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#705C53"));
        static Brush fc立秋 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B17457"));

        static Brush bc立冬 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0A5EB0"));
        static Brush fc立冬 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F2F9FF"));

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

        private void btnAlarm_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn || btn.Tag == null) return;
            var key = btn.Tag.ToString() ?? "";
            if (!alarmCfgs.TryGetValue(key, out var alarmCfg))
                alarmCfg = new();

            if (!alarmCfg.IsEnabled)
                alarmCfg.AddMinutes(5);

            var alarmSetupWindow = new AlarmSetupWindow {
                MyAlarmCfg = alarmCfg
            };
            alarmSetupWindow.Topmost = this.Topmost;
            alarmSetupWindow.ShowDialog();
            if (alarmSetupWindow.DialogResult == true) {
                //清除鬧鐘
                if (alarmSetupWindow.MyAlarmCfg == null) {
                    CleanAlarm(key);
                    btn.Background = Brushes.Transparent;
                    return;
                }

                //設定鬧鐘
                alarmCfgs[key] = alarmSetupWindow.MyAlarmCfg;
                var cfg = alarmCfgs[key];
                if (cfg.IsEnabled) {
                    //鬧鐘時間異動，重新來
                    if (alarms.TryGetValue(key, out var timer)) {
                        timer.Stop();
                        timer.Interval = cfg.GetTimeSpan();
                        timer.Start();
                        return;
                    }
                }

                alarms[key] = new() { Interval = cfg.GetTimeSpan() };
                alarms[key].Tick += (s, e) => {
                    if (cfg.IsPlaySound) {
                        try {
                            var fld = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                            var mp3 = Path.Combine(fld!, SG.AlarmSounds[cfg.SoundName!]);
                            var mp3FileReader = new Mp3FileReader(mp3);
                            var waveOutEvent = new WaveOutEvent();
                            waveOutEvent.Init(mp3FileReader);
                            waveOutEvent.Play();
                            waveOutEvent.PlaybackStopped += (s, e) => {
                                waveOutEvent.Stop();
                                waveOutEvent.Dispose();
                                mp3FileReader.Dispose();
                            };
                        }
                        catch (Exception ex) {
                            //有可能檔案不存在，就不播放
                            Debug.WriteLine(ex.Message);
                        }
                    }

                    MsgWindow msgWindow;
                    if (cfg.IsShowMsg) {
                        msgWindow = new MsgWindow {
                            EnableFadeIn = true,
                            HeaderText = $"鬧鐘{key}",
                            MessageText = cfg?.MsgText ?? $"鬧鐘{key}時間到"
                        };
                    }
                    else {
                        msgWindow = new MsgWindow {
                            EnableFadeIn = true,
                            HeaderText = $"鬧鐘{key}",
                            MessageText = $"鬧鐘{key}時間到"
                        };
                    }

                    alarms[key].Stop();
                    alarmCfgs[key].IsEnabled = false;
                    CleanAlarm(key);
                    btn.Background = Brushes.Transparent;

                    msgWindow.ShowDialog();
                };

                alarms[key].Start();
                cfg.IsEnabled = true;
                btn.Background = Brushes.LightCoral;
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

            int year = lunar.GetYear(now);
            int month = lunar.GetMonth(now);
            int day = lunar.GetDayOfMonth(now);

            var dayText = GetChineseDayName(day);

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
                    //var termTime = new DateTime(dateTime.Year, term.Month, term.Day, term.Hour, term.Minute, 0);
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