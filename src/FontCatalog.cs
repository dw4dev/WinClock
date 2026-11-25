using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace WinClock
{
    /// <summary>字型清單中的一項：<see cref="Source"/> 寫進設定，<see cref="DisplayName"/> 顯示給使用者。</summary>
    public sealed record FontChoice(string Source, string DisplayName)
    {
        public override string ToString() => DisplayName;
    }

    /// <summary>粗細清單中的一項：<see cref="Name"/> 寫進設定。</summary>
    public sealed record WeightChoice(string Name, string DisplayName)
    {
        public override string ToString() => DisplayName;
    }

    /// <summary>系統已安裝字型、粗細名稱，以及把 <see cref="FontSetting"/> 換成 WPF 值（FR-005）。</summary>
    public static class FontCatalog
    {
        private static readonly Lazy<IReadOnlyList<FontChoice>> installed = new(LoadInstalled);

        public static IReadOnlyList<FontChoice> Installed => installed.Value;

        public static IReadOnlyList<WeightChoice> Weights { get; } = [
            new("Thin", "極細"),
            new("ExtraLight", "特細"),
            new("Light", "細"),
            new("Normal", "標準"),
            new("Medium", "中"),
            new("SemiBold", "半粗"),
            new("Bold", "粗"),
            new("ExtraBold", "特粗"),
            new("Black", "極粗"),
        ];

        public static bool IsInstalled(string? family) =>
            !string.IsNullOrWhiteSpace(family) && Installed.Any(f => string.Equals(f.Source, family, StringComparison.OrdinalIgnoreCase));

        public static bool IsValidWeight(string? weight) => Weights.Any(w => w.Name == weight);

        /// <summary>設定的字型沒有安裝時，回傳 <paramref name="fallback"/>。</summary>
        public static string ResolveFamily(string? family, string fallback) => IsInstalled(family) ? family! : fallback;

        public static FontWeight ToWeight(string? weight) =>
            IsValidWeight(weight) ? (FontWeight)new FontWeightConverter().ConvertFromInvariantString(weight!)! : FontWeights.Bold;

        private static IReadOnlyList<FontChoice> LoadInstalled()
        {
            var culture = XmlLanguage.GetLanguage(CultureInfo.CurrentUICulture.IetfLanguageTag);
            return Fonts.SystemFontFamilies
                .Where(f => !string.IsNullOrEmpty(f.Source))
                .Select(f => new FontChoice(f.Source,
                    f.FamilyNames.TryGetValue(culture, out var local) && local != f.Source ? $"{local}（{f.Source}）" : f.Source))
                .DistinctBy(f => f.Source, StringComparer.OrdinalIgnoreCase)
                .OrderBy(f => f.DisplayName, StringComparer.CurrentCulture)
                .ToList();
        }
    }
}
