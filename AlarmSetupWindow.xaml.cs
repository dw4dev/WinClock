using System.IO;
using System.Reflection;
using System.Windows;

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

        List<int> Hours = new() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };
        List<int> Minutes = new() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59 };

        public AlarmSetupWindow()
        {
            InitializeComponent();
            MyInit();
        }

        private void MyInit()
        {
            cmbHour.ItemsSource = Hours;
            cmbMinute.ItemsSource = Minutes;
            cmbSound.ItemsSource = SG.AlarmSounds.Keys;
        }

        #region 公開屬性

        #endregion

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (MyAlarmCfg!.GetMilliseconds() <= 2000) {
                //低於兩秒的鬧鐘時間不合理
                MessageBox.Show("鬧鐘時間不正確\n請重新設定", "警告", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (MyAlarmCfg!.IsPlaySound && string.IsNullOrEmpty(MyAlarmCfg.SoundName)) {
                MessageBox.Show("請選取要播放的鬧鈴", "警告", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (cmbSound.SelectedItem is not string soundName) return;

            IsPlaying = true;
            var fld = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var mp3 = Path.Combine(fld!, SG.AlarmSounds[MyAlarmCfg!.SoundName!]);
            mp3FileReader = new Mp3FileReader(mp3);
            waveOutEvent = new WaveOutEvent();

            waveOutEvent.PlaybackStopped += WaveOutEvent_PlaybackStopped;

            waveOutEvent.Init(mp3FileReader);
            waveOutEvent.Play();
        }

        private void WaveOutEvent_PlaybackStopped(object? sender, StoppedEventArgs e)
        {
            if (waveOutEvent != null) {
                IsPlaying = false;
                waveOutEvent.PlaybackStopped -= WaveOutEvent_PlaybackStopped;
                waveOutEvent.Stop();
                waveOutEvent.Dispose();
                mp3FileReader?.Dispose();
            }
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            MyAlarmCfg = null;
            DialogResult = true;
        }
    }
}
