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

            ChineseFontCombo.ItemsSource = FontCatalog.Installed;
            DigitFontCombo.ItemsSource = FontCatalog.Installed;
            ChineseWeightCombo.ItemsSource = FontCatalog.Weights;
            DigitWeightCombo.ItemsSource = FontCatalog.Weights;
            loading = true;
            FontScaleSlider.Minimum = AppearanceSettings.MinFontScale;
            FontScaleSlider.Maximum = AppearanceSettings.MaxFontScale;
            loading = false;
        }

        /// <summary>顯示一份設定；之後的修改直接寫回這個物件。</summary>
        public void Load(AppearanceSettings appearance)
        {
            settings = appearance;
            SetTheme(appearance.Theme);
            LightEditor.Load(appearance.Light);
            DarkEditor.Load(appearance.Dark);

            loading = true;
            // 沒安裝的字型不選任何項目；設定值保留，主視窗退回預設字型。
            ChineseFontCombo.SelectedItem = FindFont(appearance.ChineseFont.Family);
            DigitFontCombo.SelectedItem = FindFont(appearance.DigitFont.Family);
            ChineseWeightCombo.SelectedItem = FindWeight(appearance.ChineseFont.Weight);
            DigitWeightCombo.SelectedItem = FindWeight(appearance.DigitFont.Weight);
            FontScaleSlider.Value = appearance.FontScale;
            FontScaleText.Text = $"{appearance.FontScale}%";
            loading = false;
        }

        private static FontChoice? FindFont(string family) =>
            FontCatalog.Installed.FirstOrDefault(f => string.Equals(f.Source, family, StringComparison.OrdinalIgnoreCase));

        private static WeightChoice? FindWeight(string weight) => FontCatalog.Weights.FirstOrDefault(w => w.Name == weight);

        private void Font_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (loading) return;
            if (ChineseFontCombo.SelectedItem is FontChoice chinese) settings.ChineseFont.Family = chinese.Source;
            if (DigitFontCombo.SelectedItem is FontChoice digit) settings.DigitFont.Family = digit.Source;
            if (ChineseWeightCombo.SelectedItem is WeightChoice chineseWeight) settings.ChineseFont.Weight = chineseWeight.Name;
            if (DigitWeightCombo.SelectedItem is WeightChoice digitWeight) settings.DigitFont.Weight = digitWeight.Name;
            Changed?.Invoke(this, EventArgs.Empty);
        }

        private void FontScaleSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (loading || FontScaleText is null) return;
            settings.FontScale = (int)Math.Round(FontScaleSlider.Value);
            FontScaleText.Text = $"{settings.FontScale}%";
            Changed?.Invoke(this, EventArgs.Empty);
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
