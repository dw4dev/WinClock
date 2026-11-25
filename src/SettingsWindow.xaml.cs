using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

using DWToolkit.Wpf.Windowing;

namespace WinClock
{
    /// <summary>
    /// 設定視窗（FR-003）。
    /// <para>
    /// 即時預覽：開啟時複製目前設定，修改副本後呼叫 <see cref="previewApply"/> 讓主視窗套用。
    /// 「確定」寫入 settings.json；「取消」或按 X 關閉時，用開啟前的設定還原。
    /// </para>
    /// <para>位置、尺寸與最後選取的類別由 DWToolkit 記住（Key="Settings"）。</para>
    /// </summary>
    public partial class SettingsWindow : Window, IWindowExtraStateOwner
    {
        private const string CategoryStateKey = "category";

        private readonly AppSettings original;
        private readonly GeneralSettingsPage generalPage = new();
        private readonly Action<AppSettings> previewApply;
        private bool committed;

        /// <summary>正在編輯的設定副本。各類別頁面（Phase-11d～11g）修改它後呼叫 <see cref="OnSettingsChanged"/>。</summary>
        public AppSettings Working { get; private set; }

        public SettingsWindow(AppSettings current, Action<AppSettings> previewApply)
        {
            InitializeComponent();
            original = current.Clone();
            Working = current.Clone();
            this.previewApply = previewApply;
            generalPage.Changed += (s, e) => OnSettingsChanged();
            SelectCategory(SettingsCategory.General);
        }

        private SettingsCategory CurrentCategory =>
            (CategoryList.SelectedItem as ListBoxItem)?.Tag is string tag && Enum.TryParse<SettingsCategory>(tag, out var c)
                ? c
                : SettingsCategory.General;

        private void SelectCategory(SettingsCategory category)
        {
            CategoryList.SelectedItem = CategoryList.Items.OfType<ListBoxItem>()
                .FirstOrDefault(i => (string)i.Tag == category.ToString()) ?? CategoryList.Items[0];
        }

        private void CategoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 至少保持一個類別被選取。
            if (CategoryList.SelectedItem is null) {
                SelectCategory(SettingsCategory.General);
                return;
            }
            ShowCategory(CurrentCategory);
        }

        /// <summary>顯示類別內容（外觀、鬧鐘由 Phase-11e～11g 填入）。</summary>
        private void ShowCategory(SettingsCategory category)
        {
            if (category == SettingsCategory.General) {
                generalPage.Load(Working.General);
                CategoryContent.Content = generalPage;
                return;
            }
            PlaceholderText.Text = category == SettingsCategory.Appearance ? "外觀設定（尚未提供）" : "鬧鐘設定（尚未提供）";
            CategoryContent.Content = PlaceholderText;
        }

        /// <summary>
        /// 工具列釘選按鈕已直接改變並保存最上層：同步到原始設定與副本，取消時不還原。
        /// </summary>
        public void SetTopmost(bool topmost)
        {
            original.General.Topmost = topmost;
            Working.General.Topmost = topmost;
            generalPage.SetTopmost(topmost);
        }

        /// <summary>設定副本改變後呼叫：通知主視窗預覽。</summary>
        public void OnSettingsChanged()
        {
            previewApply(Working.Clone());
        }

        private void ResetCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            Working.Reset(CurrentCategory);
            ShowCategory(CurrentCategory);
            OnSettingsChanged();
        }

        private void ResetAllButton_Click(object sender, RoutedEventArgs e)
        {
            Working = new AppSettings();
            ShowCategory(CurrentCategory);
            OnSettingsChanged();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsStore.Save(Working);
            AutoStart.Sync(Working.General.AutoStart);
            previewApply(Working.Clone());
            committed = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (!e.Cancel && !committed) previewApply(original.Clone());
        }

        #region 視窗擴充狀態（DWToolkit：最後選取的類別）

        public void CollectExtraState(WindowExtraState state)
        {
            state.Set(CategoryStateKey, CurrentCategory.ToString());
        }

        public void ApplyExtraState(WindowExtraState state)
        {
            if (Enum.TryParse<SettingsCategory>(state.GetString(CategoryStateKey), out var category)
                && Enum.IsDefined(category)) {
                SelectCategory(category);
            }
        }

        #endregion
    }
}
