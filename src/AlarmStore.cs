using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

using DWToolkit.Wpf.Windowing;

namespace WinClock
{
    /// <summary>
    /// 鬧鐘的保存：exe 旁的 alarms.json。
    /// <para>讀寫失敗不拋例外：讀取失敗回傳空清單，寫入失敗只輸出到偵錯視窗。</para>
    /// <para>
    /// 測試模式沿用 DWToolkit 的參數（docs/kb/00-ui-rules.md §1.4）：
    /// --dw-test-mode 不讀也不寫；--dw-state-dir=資料夾 改用隔離資料夾。
    /// </para>
    /// </summary>
    public static class AlarmStore
    {
        public const string FileName = "alarms.json";
        private const int FileVersion = 1;

        private static readonly JsonSerializerOptions JsonOptions = new() {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        private static readonly WindowPersistenceEnvironment Env = WindowPersistenceEnvironment.FromCurrentProcess();

        /// <summary>
        /// 是否啟用保存。測試模式（--dw-test-mode）時為 false。
        /// </summary>
        public static bool IsEnabled => !Env.IsDisabled;

        public static string FilePath =>
            Path.Combine(string.IsNullOrWhiteSpace(Env.StateDirectory) ? AppContext.BaseDirectory : Env.StateDirectory, FileName);

        /// <summary>
        /// 讀取尚未到時間的鬧鐘，依時間排序，最多 <paramref name="maxCount"/> 個。
        /// 已過時間的鬧鐘、格式錯誤的項目會被略過。
        /// </summary>
        public static List<AlarmCfg> Load(int maxCount)
        {
            var result = new List<AlarmCfg>();
            if (!IsEnabled) return result;

            try {
                var path = FilePath;
                if (!File.Exists(path)) return result;

                var file = JsonSerializer.Deserialize<AlarmFile>(File.ReadAllText(path, Encoding.UTF8), JsonOptions);
                if (file?.Alarms is null) return result;

                var now = DateTime.Now;
                foreach (var dto in file.Alarms.Where(a => a is not null && a.Target > now).OrderBy(a => a.Target)) {
                    if (result.Count >= maxCount) break;

                    var cfg = new AlarmCfg {
                        IsShowMsg = dto.IsShowMsg,
                        MsgText = dto.MsgText,
                        IsEnabled = true
                    };
                    cfg.SetTarget(dto.Target);

                    // 鈴聲名稱不在清單內（檔案被手動修改、鈴聲被移除）時，改成不播放。
                    var soundOk = !string.IsNullOrEmpty(dto.SoundName) && SG.AlarmSounds.ContainsKey(dto.SoundName);
                    cfg.SoundName = soundOk ? dto.SoundName : null;
                    cfg.IsPlaySound = dto.IsPlaySound && soundOk;

                    result.Add(cfg);
                }
            }
            catch (Exception ex) {
                Debug.WriteLine($"讀取 {FileName} 失敗：{ex.Message}");
                result.Clear();
            }

            return result;
        }

        /// <summary>
        /// 儲存目前全部的鬧鐘（整份覆寫）。
        /// </summary>
        public static void Save(IEnumerable<AlarmCfg> alarms)
        {
            if (!IsEnabled) return;

            try {
                var file = new AlarmFile {
                    Version = FileVersion,
                    Alarms = alarms.Select(a => new AlarmDto {
                        Target = a.GetTargetDateTime(),
                        IsPlaySound = a.IsPlaySound,
                        SoundName = a.SoundName,
                        IsShowMsg = a.IsShowMsg,
                        MsgText = a.MsgText
                    }).ToList()
                };

                var path = FilePath;
                var directory = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(directory)) Directory.CreateDirectory(directory);

                // 先寫暫存檔再取代，避免寫到一半當機時留下半個檔案。
                var tempPath = path + ".tmp";
                File.WriteAllText(tempPath, JsonSerializer.Serialize(file, JsonOptions), new UTF8Encoding(false));
                File.Move(tempPath, path, overwrite: true);
            }
            catch (Exception ex) {
                Debug.WriteLine($"寫入 {FileName} 失敗：{ex.Message}");
            }
        }

        private sealed class AlarmFile
        {
            public int Version { get; set; } = FileVersion;
            public List<AlarmDto>? Alarms { get; set; }
        }

        private sealed class AlarmDto
        {
            public DateTime Target { get; set; }
            public bool IsPlaySound { get; set; }
            public string? SoundName { get; set; }
            public bool IsShowMsg { get; set; } = true;
            public string? MsgText { get; set; }
        }
    }
}
