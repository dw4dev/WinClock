using System.Windows;
using System.Windows.Controls;

namespace WinClock
{
    /// <summary>設定視窗「外觀」類別的主題與顏色（FR-005）。修改直接寫回 <see cref="AppearanceSettings"/> 並觸發 <see cref="Changed"/>。</summary>
    public partial class AppearanceSettingsPage : UserControl
    {
        private AppearanceSettings settings = new();
        private bool loading;

        public event EventHandler? Changed;

        public AppearanceSettingsPage()
        {
            InitializeComponent();
            LightEditor.SetAutomationPrefix("Settings_Light");
            DarkEditor.SetAutomationPrefix("Settings_Dark");
            LightEditor.Changed += (s, e) => Changed?.Invoke(this, EventArgs.Empty);
            DarkEditor.Changed += (s, e) => Changed?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>顯示一份設定；之後的修改直接寫回這個物件。</summary>
        public void Load(AppearanceSettings appearance)
        {
            settings = appearance;
            SetTheme(appearance.Theme);
            LightEditor.Load(appearance.Light);
            DarkEditor.Load(appearance.Dark);
        }

        /// <summary>只更新主題模式，不觸發 <see cref="Changed"/>（主視窗主題按鈕在設定視窗開啟時被按下）。</summary>
        public void SetTheme(AppThemeMode mode)
        {
            loading = true;
            settings.Theme = mode;
            ThemeLightRadio.IsChecked = mode == AppThemeMode.Light;
            ThemeDarkRadio.IsChecked = mode == AppThemeMode.Dark;
            ThemeFollowRadio.IsChecked = mode == AppThemeMode.FollowWindows;
            loading = false;
        }

        private void ThemeRadio_Checked(object sender, RoutedEventArgs e)
        {
            if (loading) return;
            settings.Theme = ThemeLightRadio.IsChecked == true ? AppThemeMode.Light
                : ThemeDarkRadio.IsChecked == true ? AppThemeMode.Dark
                : AppThemeMode.FollowWindows;
            Changed?.Invoke(this, EventArgs.Empty);
        }
    }
}
