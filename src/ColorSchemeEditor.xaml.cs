using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;

namespace WinClock
{
    /// <summary>
    /// 一組主題顏色（明亮或暗色）的編輯區。控制項改變時寫回 <see cref="ColorScheme"/> 並觸發 <see cref="Changed"/>。
    /// AutomationId 以 <see cref="SetAutomationPrefix"/> 指定的前綴命名，讓兩組編輯區不重複。
    /// </summary>
    public partial class ColorSchemeEditor : UserControl
    {
        private ColorScheme scheme = new();
        private bool loading;

        public event EventHandler? Changed;

        public ColorSchemeEditor()
        {
            InitializeComponent();
        }

        public void SetAutomationPrefix(string prefix)
        {
            BackgroundField.SetAutomation(prefix + "Background", "背景顏色");
            AutomationProperties.SetAutomationId(GradientCheckBox, prefix + "GradientCheckBox");
            Background2Field.SetAutomation(prefix + "Background2", "漸層第二色");
            AutomationProperties.SetAutomationId(AngleSlider, prefix + "AngleSlider");
            AutomationProperties.SetName(AngleSlider, "漸層角度");
            AutomationProperties.SetAutomationId(AngleText, prefix + "AngleText");
            TextField.SetAutomation(prefix + "Text", "文字顏色");
            AutomationProperties.SetAutomationId(LunarFollowCheckBox, prefix + "LunarFollowCheckBox");
            LunarField.SetAutomation(prefix + "Lunar", "農曆顏色");
            AutomationProperties.SetAutomationId(OpacitySlider, prefix + "OpacitySlider");
            AutomationProperties.SetName(OpacitySlider, "不透明度");
            AutomationProperties.SetAutomationId(OpacityText, prefix + "OpacityText");
        }

        /// <summary>顯示一組顏色；之後的修改直接寫回這個物件。</summary>
        public void Load(ColorScheme colors)
        {
            scheme = colors;
            loading = true;
            BackgroundField.Load(colors.Background);
            GradientCheckBox.IsChecked = colors.UseGradient;
            Background2Field.Load(colors.Background2);
            AngleSlider.Value = colors.GradientAngle;
            TextField.Load(colors.Text);
            LunarFollowCheckBox.IsChecked = colors.LunarFollowText;
            LunarField.Load(colors.LunarText);
            OpacitySlider.Value = colors.Opacity * 100;
            UpdateValueTexts();
            loading = false;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!IsInitialized) return;
            UpdateValueTexts();
            Control_Changed(sender, e);
        }

        private void Control_Changed(object? sender, EventArgs e)
        {
            if (loading || !IsInitialized) return;
            scheme.Background = BackgroundField.Value;
            scheme.UseGradient = GradientCheckBox.IsChecked == true;
            scheme.Background2 = Background2Field.Value;
            scheme.GradientAngle = AngleSlider.Value;
            scheme.Text = TextField.Value;
            scheme.LunarFollowText = LunarFollowCheckBox.IsChecked == true;
            scheme.LunarText = LunarField.Value;
            // 使用者沒動過滑桿時保留原值（例：v1.0 的 0.882），避免四捨五入改變外觀。
            if (Math.Abs(OpacitySlider.Value - scheme.Opacity * 100) > 1e-9) scheme.Opacity = OpacitySlider.Value / 100;
            Changed?.Invoke(this, EventArgs.Empty);
        }

        private void UpdateValueTexts()
        {
            AngleText.Text = $"{AngleSlider.Value:0}°";
            OpacityText.Text = $"{OpacitySlider.Value:0}%";
        }
    }
}
