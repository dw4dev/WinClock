using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace WinClock
{
    /// <summary>
    /// 系統匣圖示與選單（顯示、設定、結束），見 ADR-0002。
    /// 圖示用 WinForms NotifyIcon；選單用 WPF ContextMenu，選單項目才能設 AutomationId
    /// （WinForms ToolStripMenuItem 不會把 Name 輸出成 AutomationId）。
    /// </summary>
    public sealed class TrayIcon : IDisposable
    {
        private readonly System.Windows.Forms.NotifyIcon notifyIcon;
        private readonly ContextMenu menu;

        public event EventHandler? ShowRequested;
        public event EventHandler? SettingsRequested;
        public event EventHandler? ExitRequested;

        public TrayIcon(string toolTip)
        {
            menu = new ContextMenu { Placement = PlacementMode.MousePoint };
            AutomationProperties.SetAutomationId(menu, "Tray_Menu");
            menu.Items.Add(CreateItem("Tray_ShowMenuItem", "顯示", () => ShowRequested?.Invoke(this, EventArgs.Empty)));
            menu.Items.Add(CreateItem("Tray_SettingsMenuItem", "設定", () => SettingsRequested?.Invoke(this, EventArgs.Empty)));
            menu.Items.Add(new Separator());
            menu.Items.Add(CreateItem("Tray_ExitMenuItem", "結束", () => ExitRequested?.Invoke(this, EventArgs.Empty)));

            notifyIcon = new System.Windows.Forms.NotifyIcon {
                Icon = LoadAppIcon(),
                Text = toolTip,
                Visible = true
            };
            notifyIcon.MouseDoubleClick += (s, e) => {
                if (e.Button == System.Windows.Forms.MouseButtons.Left) ShowRequested?.Invoke(this, EventArgs.Empty);
            };
            notifyIcon.MouseUp += (s, e) => {
                if (e.Button == System.Windows.Forms.MouseButtons.Right) menu.IsOpen = true;
            };
        }

        private static MenuItem CreateItem(string automationId, string header, Action onClick)
        {
            var item = new MenuItem { Header = header };
            AutomationProperties.SetAutomationId(item, automationId);
            item.Click += (s, e) => onClick();
            return item;
        }

        /// <summary>從 exe 取出應用程式圖示（csproj 的 ApplicationIcon）；失敗時用系統預設圖示。</summary>
        private static System.Drawing.Icon LoadAppIcon()
        {
            try {
                var path = Environment.ProcessPath;
                if (path is not null && System.Drawing.Icon.ExtractAssociatedIcon(path) is { } icon) return icon;
            }
            catch (Exception ex) {
                Debug.WriteLine($"讀取程式圖示失敗：{ex.Message}");
            }
            return System.Drawing.SystemIcons.Application;
        }

        public void Dispose()
        {
            menu.IsOpen = false;
            // 先隱藏再釋放，避免系統匣殘留圖示。
            notifyIcon.Visible = false;
            notifyIcon.Dispose();
        }
    }
}
