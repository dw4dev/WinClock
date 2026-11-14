using System.Windows.Threading;

namespace WinClock
{
    /// <summary>
    /// 一個生效中的鬧鐘：設定、計時器，以及工具列按鈕要顯示的資料。
    /// 工具列的鬧鐘按鈕以這個物件為 DataContext。
    /// </summary>
    public class AlarmItem : ObservableObject
    {
        private int number;
        private AlarmCfg cfg;

        public AlarmItem(AlarmCfg cfg)
        {
            this.cfg = cfg;
        }

        /// <summary>
        /// 鬧鐘設定。
        /// </summary>
        public AlarmCfg Cfg
        {
            get => cfg;
            set
            {
                cfg = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ToolTipText));
            }
        }

        /// <summary>
        /// 檢查是否到時間的計時器。由 MainWindow 建立與停止。
        /// </summary>
        public DispatcherTimer? Timer { get; set; }

        /// <summary>
        /// 顯示編號（1 起算）。鬧鐘被移除後，後面的鬧鐘往前遞補。
        /// </summary>
        public int Number
        {
            get => number;
            set
            {
                number = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DisplayName));
                OnPropertyChanged(nameof(AutomationId));
                OnPropertyChanged(nameof(ToolTipText));
            }
        }

        public string DisplayName => $"鬧鐘{Number}";

        /// <summary>
        /// UI Automation 識別（對外契約）：Main_AlarmButton1 ~ Main_AlarmButton5。
        /// </summary>
        public string AutomationId => $"Main_AlarmButton{Number}";

        /// <summary>
        /// 按鈕提示：時間、剩餘時間、訊息、鈴聲。
        /// </summary>
        public string ToolTipText
        {
            get
            {
                var target = Cfg.GetTargetDateTime();
                var lines = new List<string> {
                    $"{DisplayName}　{target:yyyy/MM/dd HH:mm}",
                    $"剩餘 {FormatRemaining(target - DateTime.Now)}"
                };

                if (Cfg.IsShowMsg && !string.IsNullOrWhiteSpace(Cfg.MsgText))
                    lines.Add($"訊息：{Cfg.MsgText}");
                if (Cfg.IsPlaySound && !string.IsNullOrEmpty(Cfg.SoundName))
                    lines.Add($"鈴聲：{Cfg.SoundName}");

                lines.Add("（點擊可修改或移除）");
                return string.Join(Environment.NewLine, lines);
            }
        }

        /// <summary>
        /// 重新計算提示文字（剩餘時間會隨時間改變）。
        /// </summary>
        public void RefreshToolTip() => OnPropertyChanged(nameof(ToolTipText));

        internal static string FormatRemaining(TimeSpan remaining)
        {
            if (remaining <= TimeSpan.Zero) return "0 秒";
            if (remaining.TotalDays >= 1) return $"{(int)remaining.TotalDays} 天 {remaining.Hours} 小時 {remaining.Minutes} 分";
            if (remaining.TotalHours >= 1) return $"{remaining.Hours} 小時 {remaining.Minutes} 分";
            if (remaining.TotalMinutes >= 1) return $"{remaining.Minutes} 分 {remaining.Seconds} 秒";
            return $"{remaining.Seconds} 秒";
        }
    }
}
