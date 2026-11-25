using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

using DWToolkit.Wpf.Windowing;

namespace WinClock
{
    /// <summary>
    /// 設定的保存：exe 旁的 settings.json（格式見 docs/adr/0001-settings-file.md）。
    /// <para>讀寫失敗不拋例外：讀取失敗回傳預設值，寫入失敗只輸出到偵錯視窗。</para>
    /// <para>測試模式參數同 <see cref="AlarmStore"/>：--dw-test-mode 不讀也不寫；--dw-state-dir=資料夾 改用隔離資料夾。</para>
    /// </summary>
    public static class SettingsStore
    {
        public const string FileName = "settings.json";
        internal const int FileVersion = 1;

        internal static readonly JsonSerializerOptions JsonOptions = new() {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        private static readonly WindowPersistenceEnvironment Env = WindowPersistenceEnvironment.FromCurrentProcess();

        public static bool IsEnabled => !Env.IsDisabled;

        public static string FilePath =>
            Path.Combine(string.IsNullOrWhiteSpace(Env.StateDirectory) ? AppContext.BaseDirectory : Env.StateDirectory, FileName);

        /// <summary>
        /// 讀取設定。檔案不存在或損毀時回傳預設值；缺少的欄位用該欄位的預設值。
        /// </summary>
        public static AppSettings Load()
        {
            if (!IsEnabled) return new AppSettings();

            try {
                var path = FilePath;
                if (!File.Exists(path)) return new AppSettings();

                var settings = JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(path, Encoding.UTF8), JsonOptions) ?? new AppSettings();
                settings.Normalize();
                return settings;
            }
            catch (Exception ex) {
                Debug.WriteLine($"讀取 {FileName} 失敗：{ex.Message}");
                return new AppSettings();
            }
        }

        /// <summary>
        /// 寫入設定（整份覆寫）。
        /// </summary>
        public static void Save(AppSettings settings)
        {
            if (!IsEnabled) return;

            try {
                settings.Version = FileVersion;

                var path = FilePath;
                var directory = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(directory)) Directory.CreateDirectory(directory);

                // 先寫暫存檔再取代，避免寫到一半當機時留下半個檔案。
                var tempPath = path + ".tmp";
                File.WriteAllText(tempPath, JsonSerializer.Serialize(settings, JsonOptions), new UTF8Encoding(false));
                File.Move(tempPath, path, overwrite: true);
            }
            catch (Exception ex) {
                Debug.WriteLine($"寫入 {FileName} 失敗：{ex.Message}");
            }
        }
    }
}
