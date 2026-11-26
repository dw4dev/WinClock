using System.Text.Json;
using System.Text.Json.Serialization;

namespace WinClock
{
    /// <summary>主題模式。</summary>
    [JsonConverter(typeof(JsonStringEnumConverter<AppThemeMode>))]
    public enum AppThemeMode
    {
        Light,
        Dark,
        FollowWindows
    }

    /// <summary>開機啟動時的視窗狀態。</summary>
    [JsonConverter(typeof(JsonStringEnumConverter<StartupState>))]
    public enum StartupState
    {
        Show,
        Tray
    }

    /// <summary>設定類別（設定視窗左側導覽）。</summary>
    public enum SettingsCategory
    {
        General,
        Appearance,
        Alarm
    }

    /// <summary>
    /// 全部設定（settings.json 的內容）。每個欄位的預設值等於 v1.0 的行為。
    /// 讀寫見 <see cref="SettingsStore"/>。
    /// </summary>
    public sealed class AppSettings
    {
        public int Version { get; set; } = SettingsStore.FileVersion;
        public GeneralSettings General { get; set; } = new();
        public AppearanceSettings Appearance { get; set; } = new();
        public AlarmSettings Alarm { get; set; } = new();

        /// <summary>
        /// 建立一份獨立的深層副本。設定視窗編輯副本，按「取消」時用原本的設定還原。
        /// </summary>
        public AppSettings Clone() =>
            JsonSerializer.Deserialize<AppSettings>(JsonSerializer.Serialize(this, SettingsStore.JsonOptions), SettingsStore.JsonOptions)!;

        /// <summary>把一個類別重設成預設值。</summary>
        public void Reset(SettingsCategory category)
        {
            switch (category) {
                case SettingsCategory.General: General = new(); break;
                case SettingsCategory.Appearance: Appearance = new(); break;
                case SettingsCategory.Alarm: Alarm = new(); break;
            }
        }

        /// <summary>
        /// 把 null 的類別換成預設值，把超出範圍的數值拉回範圍內。
        /// </summary>
        public void Normalize()
        {
            General ??= new();
            Appearance ??= new();
            Alarm ??= new();
            if (!Enum.IsDefined(General.StartupState)) General.StartupState = StartupState.Show;
            Appearance.Normalize();
            Alarm.Normalize();
        }
    }

    public sealed class GeneralSettings
    {
        /// <summary>開機自動啟動。v1.0 不登記開機啟動。</summary>
        public bool AutoStart { get; set; }

        /// <summary>開機啟動時的狀態（只套用在開機啟動）。</summary>
        public StartupState StartupState { get; set; } = StartupState.Show;

        /// <summary>永遠在最上層。預設啟用（同 v1.0）。</summary>
        public bool Topmost { get; set; } = true;

        /// <summary>true 為 24 小時制；false 為 12 小時制。</summary>
        public bool Use24Hour { get; set; } = true;

        /// <summary>顯示秒數。</summary>
        public bool ShowSeconds { get; set; } = true;
    }

    public sealed class AppearanceSettings
    {
        public AppThemeMode Theme { get; set; } = AppThemeMode.Dark;

        public FontSetting ChineseFont { get; set; } = new() { Family = "Microsoft JhengHei UI", Weight = "Bold" };
        public FontSetting DigitFont { get; set; } = new() { Family = "Segoe UI", Weight = "Bold" };

        public const int MinFontScale = 50;
        public const int MaxFontScale = 200;

        /// <summary>字型縮放比例（%），乘在主視窗隨寬度縮放的字級上。</summary>
        public int FontScale { get; set; } = 100;

        public ColorScheme Light { get; set; } = new() {
            Background = "#F0F0F5",
            Background2 = "#F0F0F5",
            Text = "#1C1917",
            LunarText = "#57534E",   // v1.0 農曆灰色
            Opacity = 0.882   // v1.0：alpha 225
        };

        public ColorScheme Dark { get; set; } = new() {
            Background = "#1E1E24",
            Background2 = "#1E1E24",
            Text = "#FFFFFF",
            LunarText = "#A0A5B5",   // v1.0 農曆灰色
            Opacity = 0.85    // v1.0：alpha 217
        };

        internal void Normalize()
        {
            if (!Enum.IsDefined(Theme)) Theme = AppThemeMode.Dark;
            var defaults = new AppearanceSettings();
            ChineseFont ??= defaults.ChineseFont;
            DigitFont ??= defaults.DigitFont;
            ChineseFont.Normalize(defaults.ChineseFont);
            DigitFont.Normalize(defaults.DigitFont);
            FontScale = Math.Clamp(FontScale, MinFontScale, MaxFontScale);
            Light ??= defaults.Light;
            Dark ??= defaults.Dark;
            Light.Normalize(defaults.Light);
            Dark.Normalize(defaults.Dark);
        }

        /// <summary>依明亮或暗色取得顏色設定。</summary>
        public ColorScheme SchemeFor(bool isDark) => isDark ? Dark : Light;
    }

    /// <summary>字型與粗細。粗細用 WPF FontWeight 名稱（Normal、Bold……）。</summary>
    public sealed class FontSetting
    {
        public string Family { get; set; } = "";
        public string Weight { get; set; } = "Bold";

        /// <summary>空白字型名稱或不認得的粗細換成 <paramref name="defaults"/> 的值。沒安裝的字型保留，套用時才退回（見 <see cref="FontCatalog"/>）。</summary>
        internal void Normalize(FontSetting defaults)
        {
            if (string.IsNullOrWhiteSpace(Family)) Family = defaults.Family;
            if (!FontCatalog.IsValidWeight(Weight)) Weight = defaults.Weight;
        }
    }

    /// <summary>一個主題的顏色。顏色用 #RRGGBB。</summary>
    public sealed class ColorScheme
    {
        public string Background { get; set; } = "#000000";

        /// <summary>true 時背景用 <see cref="Background"/> 到 <see cref="Background2"/> 的漸層。</summary>
        public bool UseGradient { get; set; }

        public string Background2 { get; set; } = "#000000";

        /// <summary>漸層角度 0–360。</summary>
        public double GradientAngle { get; set; }

        public string Text { get; set; } = "#FFFFFF";

        /// <summary>true 時農曆文字用 <see cref="Text"/>；false 時用 <see cref="LunarText"/>（M-001）。</summary>
        public bool LunarFollowText { get; set; } = true;

        public string LunarText { get; set; } = "#A0A5B5";

        /// <summary>背景不透明度 0.01–1；不允許 0，避免背景完全穿透滑鼠。</summary>
        public double Opacity { get; set; } = 1;

        internal const double MinOpacity = 0.01;

        /// <summary>數值拉回範圍內；格式不對的顏色換成 <paramref name="defaults"/> 的值。</summary>
        internal void Normalize(ColorScheme defaults)
        {
            Background = ValidColorOr(Background, defaults.Background);
            Background2 = ValidColorOr(Background2, defaults.Background2);
            Text = ValidColorOr(Text, defaults.Text);
            LunarText = ValidColorOr(LunarText, defaults.LunarText);
            GradientAngle = double.IsFinite(GradientAngle) ? Math.Clamp(GradientAngle, 0, 360) : 0;
            Opacity = double.IsFinite(Opacity) ? Math.Clamp(Opacity, MinOpacity, 1) : 1;
        }

        private static string ValidColorOr(string? value, string fallback) =>
            ThemeBrushes.ParseColor(value) is { } color ? ThemeBrushes.FormatColor(color) : fallback;
    }

    public sealed class AlarmSettings
    {
        /// <summary>新增鬧鐘時預先選的鈴聲（<see cref="SG.AlarmSounds"/> 的名稱）；null 為不預選。</summary>
        public string? DefaultSound { get; set; }

        /// <summary>音量 0–100。</summary>
        public int Volume { get; set; } = 100;

        public const int MinSnoozeMinutes = 1;
        public const int MaxSnoozeMinutes = 60;

        /// <summary>貪睡分鐘數 1–60。</summary>
        public int SnoozeMinutes { get; set; } = 5;

        internal void Normalize()
        {
            if (DefaultSound is not null && !SG.AlarmSounds.ContainsKey(DefaultSound)) DefaultSound = null;
            Volume = Math.Clamp(Volume, 0, 100);
            if (SnoozeMinutes < MinSnoozeMinutes) SnoozeMinutes = 5;
            SnoozeMinutes = Math.Min(SnoozeMinutes, MaxSnoozeMinutes);
        }
    }
}
