using System.Diagnostics;

using Microsoft.Win32;

namespace WinClock
{
    /// <summary>
    /// 開機自動啟動（FR-004，ADR-0003）：HKCU\...\Run 的 "WinClock" 值，指令為 "exe 路徑" --autostart。
    /// <para>登錄失敗不拋例外，只輸出到偵錯視窗。</para>
    /// </summary>
    public static class AutoStart
    {
        public const string Argument = "--autostart";
        private const string RunKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
        private const string ValueName = "WinClock";

        /// <summary>本次是否由開機啟動（命令列含 --autostart）。</summary>
        public static bool IsAutoStartLaunch =>
            Environment.GetCommandLineArgs().Skip(1).Any(a => string.Equals(a, Argument, StringComparison.OrdinalIgnoreCase));

        private static string Command => $"\"{Environment.ProcessPath}\" {Argument}";

        /// <summary>
        /// 讓登錄與設定一致：開啟時寫入（路徑不同才寫，exe 搬移後自動更新）；關閉時移除。
        /// </summary>
        public static void Sync(bool enabled)
        {
            try {
                using var key = Registry.CurrentUser.CreateSubKey(RunKeyPath, writable: true);
                if (enabled) {
                    if (!string.Equals(key.GetValue(ValueName) as string, Command, StringComparison.OrdinalIgnoreCase))
                        key.SetValue(ValueName, Command);
                }
                else if (key.GetValue(ValueName) is not null) {
                    key.DeleteValue(ValueName, throwOnMissingValue: false);
                }
            }
            catch (Exception ex) {
                Debug.WriteLine($"AutoStart.Sync({enabled}) 失敗：{ex.Message}");
            }
        }
    }
}
