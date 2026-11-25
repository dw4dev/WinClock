using System.Windows;
using System.Windows.Controls;

namespace WinClock
{
    /// <summary>設定視窗「一般」類別（FR-004）。控制項改變時寫回 <see cref="GeneralSettings"/> 並觸發 <see cref="Changed"/>。</summary>
    public partial class GeneralSettingsPage : UserControl
    {
        private GeneralSettings settings = new();
        private bool loading;

        public event EventHandler? Changed;

        public GeneralSettingsPage()
        {
            InitializeComponent();
        }

        /// <summary>顯示一份設定；之後的修改直接寫回這個物件。</summary>
        public void Load(GeneralSettings general)
        {
            settings = general;
            loading = true;
            AutoStartCheckBox.IsChecked = general.AutoStart;
            StartupShowRadio.IsChecked = general.StartupState == StartupState.Show;
            StartupTrayRadio.IsChecked = general.StartupState == StartupState.Tray;
            TopmostCheckBox.IsChecked = general.Topmost;
            Use24HourRadio.IsChecked = general.Use24Hour;
            Use12HourRadio.IsChecked = !general.Use24Hour;
            ShowSecondsCheckBox.IsChecked = general.ShowSeconds;
            loading = false;
        }

        /// <summary>只更新最上層（工具列釘選按鈕在設定視窗開啟時被按下）。</summary>
        public void SetTopmost(bool topmost)
        {
            settings.Topmost = topmost;
            TopmostCheckBox.IsChecked = topmost;
        }

        private void Control_Changed(object sender, RoutedEventArgs e)
        {
            if (loading) return;
            settings.AutoStart = AutoStartCheckBox.IsChecked == true;
            settings.StartupState = StartupTrayRadio.IsChecked == true ? StartupState.Tray : StartupState.Show;
            settings.Topmost = TopmostCheckBox.IsChecked == true;
            settings.Use24Hour = Use24HourRadio.IsChecked == true;
            settings.ShowSeconds = ShowSecondsCheckBox.IsChecked == true;
            Changed?.Invoke(this, EventArgs.Empty);
        }
    }
}
