using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

using NAudio.Wave;

namespace WinClock
{
    /// <summary>
    /// AlarmSetupWindow.xaml 的互動邏輯
    /// </summary>
    public partial class AlarmSetupWindow : Window
    {
        private WaveOutEvent? waveOutEvent;
        private Mp3FileReader? mp3FileReader;

        public bool IsPlaying
        {
            get { return (bool)GetValue(IsPlayingProperty); }
            set { SetValue(IsPlayingProperty, value); }
        }

        public static readonly DependencyProperty IsPlayingProperty =
            DependencyProperty.Register("IsPlaying", typeof(bool), typeof(AlarmSetupWindow), new PropertyMetadata(false));

        public AlarmCfg? MyAlarmCfg
        {
            get { return (AlarmCfg)GetValue(MyAlarmCfgProperty); }
            set { SetValue(MyAlarmCfgProperty, value); }
        }

        public static readonly DependencyProperty MyAlarmCfgProperty =
            DependencyProperty.Register("MyAlarmCfg", typeof(AlarmCfg), typeof(AlarmSetupWindow), new PropertyMetadata(null));

        public bool IsDarkTheme
        {
            get { return (bool)GetValue(IsDarkThemeProperty); }
            set { SetValue(IsDarkThemeProperty, value); }
        }

        public static readonly DependencyProperty IsDarkThemeProperty =
            DependencyProperty.Register("IsDarkTheme", typeof(bool), typeof(AlarmSetupWindow), new PropertyMetadata(true, OnIsDarkThemeChanged));

        private static void OnIsDarkThemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AlarmSetupWindow window) {
                window.ApplyTheme((bool)e.NewValue);
            }
        }

        List<int> Hours = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23];
        List<int> Minutes = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59];

        public AlarmSetupWindow()
        {
            InitializeComponent();
            MyInit();
            ApplyTheme(IsDarkTheme);
        }

        private void MyInit()
        {
            cmbHour.ItemsSource = Hours;
            cmbMinute.ItemsSource = Minutes;
            cmbSound.ItemsSource = SG.AlarmSounds.Keys;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) {
                this.DragMove();
            }
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

            Color controlBgColor = isDark ? Color.FromRgb(42, 42, 50) : Color.FromRgb(255, 255, 255);
            Color controlBorderColor = isDark ? Color.FromArgb(68, 255, 255, 255) : Color.FromRgb(209, 213, 219);
            Color controlFgColor = isDark ? Colors.White : Color.FromRgb(30, 41, 59);

            this.Resources["WindowBackground"] = new SolidColorBrush(Color.FromArgb((byte)(isDark ? 230 : 255), winBgColor.R, winBgColor.G, winBgColor.B));
            this.Resources["WindowBorder"] = new SolidColorBrush(winBorderColor);
            this.Resources["TextPrimary"] = new SolidColorBrush(textPrimaryColor);
            this.Resources["TextSecondary"] = new SolidColorBrush(textSecondaryColor);
            this.Resources["IconForeground"] = new SolidColorBrush(iconColor);
            this.Resources["ButtonHoverBackground"] = new SolidColorBrush(btnHoverColor);
            this.Resources["ButtonPressedBackground"] = new SolidColorBrush(btnPressedColor);

            this.Resources["ControlBackground"] = new SolidColorBrush(controlBgColor);
            this.Resources["ControlBorder"] = new SolidColorBrush(controlBorderColor);
            this.Resources["ControlForeground"] = new SolidColorBrush(controlFgColor);
            this.Resources["AccentBrush"] = new SolidColorBrush(isDark ? Color.FromRgb(129, 191, 218) : Color.FromRgb(37, 99, 235));
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (MyAlarmCfg!.GetMilliseconds() <= 2000) {
                MessageBox.Show("鬧鐘時間不正確\n請重新設定", "警告", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (MyAlarmCfg!.IsPlaySound && string.IsNullOrEmpty(MyAlarmCfg.SoundName)) {
                MessageBox.Show("請選取要播放的鬧鈴", "警告", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            StopPlayback();
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            StopPlayback();
            DialogResult = false;
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (cmbSound.SelectedItem is not string soundName) return;

            IsPlaying = true;
            var fld = AppContext.BaseDirectory;
            var mp3 = Path.Combine(fld, SG.AlarmSounds[MyAlarmCfg!.SoundName!]);
            mp3FileReader = new Mp3FileReader(mp3);
            waveOutEvent = new WaveOutEvent();

            waveOutEvent.PlaybackStopped += WaveOutEvent_PlaybackStopped;

            waveOutEvent.Init(mp3FileReader);
            waveOutEvent.Play();
        }

        private void WaveOutEvent_PlaybackStopped(object? sender, StoppedEventArgs e)
        {
            StopPlayback();
        }

        private void StopPlayback()
        {
            if (waveOutEvent != null) {
                IsPlaying = false;
                waveOutEvent.PlaybackStopped -= WaveOutEvent_PlaybackStopped;
                waveOutEvent.Stop();
                waveOutEvent.Dispose();
                waveOutEvent = null;
                mp3FileReader?.Dispose();
                mp3FileReader = null;
            }
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            StopPlayback();
            MyAlarmCfg = null;
            DialogResult = true;
        }
    }
}
