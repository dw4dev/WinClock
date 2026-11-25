using System.Windows;
using System.Windows.Controls;

namespace WinClock
{
    /// <summary>設定視窗「鬧鐘」類別（FR-006）。控制項改變時寫回 <see cref="AlarmSettings"/> 並觸發 <see cref="Changed"/>。</summary>
    public partial class AlarmSettingsPage : UserControl
    {
        private const string NoSound = "（不預選）";

        private readonly AlarmPlayer player = new();
        private AlarmSettings settings = new();
        private bool loading;

        public event EventHandler? Changed;

        public AlarmSettingsPage()
        {
            InitializeComponent();
            DefaultSoundComboBox.ItemsSource = new[] { NoSound }.Concat(SG.AlarmSounds.Keys).ToList();
            SnoozeComboBox.ItemsSource = Enumerable.Range(AlarmSettings.MinSnoozeMinutes,
                AlarmSettings.MaxSnoozeMinutes - AlarmSettings.MinSnoozeMinutes + 1).ToList();
            player.Stopped += (s, e) => PreviewButton.Content = "試聽";
            Unloaded += (s, e) => player.Stop();
        }

        /// <summary>顯示一份設定；之後的修改直接寫回這個物件。</summary>
        public void Load(AlarmSettings alarm)
        {
            settings = alarm;
            loading = true;
            DefaultSoundComboBox.SelectedItem = alarm.DefaultSound ?? NoSound;
            VolumeSlider.Value = alarm.Volume;
            VolumeText.Text = $"{alarm.Volume}";
            SnoozeComboBox.SelectedItem = alarm.SnoozeMinutes;
            loading = false;
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            VolumeText.Text = $"{(int)VolumeSlider.Value}";
            Control_Changed(sender, e);
        }

        private void Control_Changed(object sender, RoutedEventArgs e)
        {
            if (loading) return;
            settings.DefaultSound = DefaultSoundComboBox.SelectedItem is string s && s != NoSound ? s : null;
            settings.Volume = (int)VolumeSlider.Value;
            if (SnoozeComboBox.SelectedItem is int minutes) settings.SnoozeMinutes = minutes;
            Changed?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>用目前音量試聽預設鈴聲；沒有預選時試聽第一個鈴聲。再按一次停止。</summary>
        private void PreviewButton_Click(object sender, RoutedEventArgs e)
        {
            if (player.IsPlaying) {
                player.Stop();
                return;
            }
            player.Play(settings.DefaultSound ?? SG.AlarmSounds.Keys.First(), settings.Volume);
            if (player.IsPlaying) PreviewButton.Content = "停止";
        }
    }
}
