using System.Windows;

namespace WinClock
{
    public partial class App : Application
    {
        /// <summary>
        /// 開機啟動（--autostart）且設定為「縮到系統匣」時不顯示主視窗；手動開啟一律顯示。
        /// 已開啟開機啟動時，同步登錄的 exe 路徑（exe 搬移後自動更新）。
        /// </summary>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var window = new MainWindow();
            MainWindow = window;

            var general = window.Settings.General;
            if (general.AutoStart) AutoStart.Sync(true);

            if (!(AutoStart.IsAutoStartLaunch && general.StartupState == StartupState.Tray)) window.Show();
        }
    }
}
