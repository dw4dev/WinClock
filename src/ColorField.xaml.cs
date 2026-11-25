using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Media;

namespace WinClock
{
    /// <summary>
    /// 顏色欄位：輸入 #RRGGBB，或按色塊開啟 Windows 選色對話框。
    /// 輸入合法顏色時更新 <see cref="Value"/> 並觸發 <see cref="ValueChanged"/>；離開欄位時，不合法的輸入還原成目前的值。
    /// </summary>
    public partial class ColorField : UserControl
    {
        private bool loading;

        public event EventHandler? ValueChanged;

        /// <summary>目前的顏色（#RRGGBB）。</summary>
        public string Value { get; private set; } = "#000000";

        public ColorField()
        {
            InitializeComponent();
        }

        /// <summary>設定 AutomationId 與名稱：文字框為 <c>{id}TextBox</c>，選色按鈕為 <c>{id}PickButton</c>。</summary>
        public void SetAutomation(string id, string name)
        {
            AutomationProperties.SetAutomationId(HexTextBox, id + "TextBox");
            AutomationProperties.SetName(HexTextBox, name);
            AutomationProperties.SetAutomationId(PickButton, id + "PickButton");
            AutomationProperties.SetName(PickButton, name + "選擇");
        }

        /// <summary>顯示一個值，不觸發 <see cref="ValueChanged"/>。</summary>
        public void Load(string value)
        {
            loading = true;
            Value = value;
            HexTextBox.Text = value;
            UpdateSwatch();
            loading = false;
        }

        private void HexTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (loading || ThemeBrushes.ParseColor(HexTextBox.Text) is not { } color) return;
            Commit(color, updateText: false);
        }

        private void HexTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ThemeBrushes.ParseColor(HexTextBox.Text) is null) Load(Value);
        }

        private void PickButton_Click(object sender, RoutedEventArgs e)
        {
            var current = ThemeBrushes.ParseColor(Value) ?? Colors.Black;
            using var dialog = new System.Windows.Forms.ColorDialog {
                FullOpen = true,
                Color = System.Drawing.Color.FromArgb(current.R, current.G, current.B)
            };
            if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
            Commit(Color.FromRgb(dialog.Color.R, dialog.Color.G, dialog.Color.B), updateText: true);
        }

        private void Commit(Color color, bool updateText)
        {
            var value = ThemeBrushes.FormatColor(color);
            if (updateText) Load(value);
            else {
                Value = value;
                UpdateSwatch();
            }
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }

        private void UpdateSwatch()
        {
            Swatch.Background = new SolidColorBrush(ThemeBrushes.ParseColor(Value) ?? Colors.Black);
        }
    }
}
