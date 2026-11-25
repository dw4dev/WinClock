# Phase-11a — `settings.json` 設定檔模型與讀寫

> 動工前必讀：`docs/kb/00-code-review.md`（通用 Code Smell Checklist）＋ `docs/kb/coding-rules.md`（本專案架構不變式；尚未建立時略過），兩者橫切全 phase。
> 另讀本 phase 相關的 Reference 素材與 ADR/KB。

狀態：✅ Done（2026-11-25）

規格：`docs/specs/v1.1.md` FR-001

## Goal

建立設定模型與 `settings.json` 的讀寫，供後續 phase 使用。本 phase 不做 UI。

## Deliverables

- 設定模型：一般（開機啟動、啟動狀態、最上層、12／24 小時制、顯示秒數）、外觀（主題模式、中文字型＋粗細、數字字型＋粗細、明亮／暗色各一組顏色：背景單色或 2 色漸層＋角度、文字顏色、透明度）、鬧鐘（預設鈴聲、音量、貪睡分鐘數）
- 每個欄位的預設值等於 v1.0 現行行為
- 設定存取類別（參考 `src/AlarmStore.cs` 的寫法）：讀取、寫入、複製（供設定視窗取消時還原）
- ADR：設定檔格式與位置

## Dependencies

無。

## Acceptance Criteria

- [x] 檔案不存在時，回傳預設值
- [x] 內容損毀或缺欄位時，回傳預設值（缺欄位只補該欄位），不當機
- [x] 寫入後再讀取，值相同（程式碼審查；尚無呼叫 `Save` 的 UI，11c 實測）
- [x] 主視窗啟動時讀取設定，行為和 v1.0 相同
- [x] `release.bat` 發佈時保留 `settings.json`（`tools/release.ps1` 保留根層 `*.json`）

## Outcome

使用者 2026-11-25 驗收全部通過。

- `src/AppSettings.cs`：模型 `AppSettings`（`Version`、`General`、`Appearance`、`Alarm`）。
  - `GeneralSettings`：`AutoStart`=false、`StartupState`=Show（`Show`／`Tray`）、`Topmost`=false、`Use24Hour`=true、`ShowSeconds`=true。
  - `AppearanceSettings`：`Theme`=Dark（`AppThemeMode`：Light／Dark／FollowWindows；避開 WPF 的 `System.Windows.ThemeMode`）；`ChineseFont`=Microsoft JhengHei UI／Bold；`DigitFont`=Segoe UI／Bold；`Light`、`Dark` 各一個 `ColorScheme`（`Background`、`UseGradient`=false、`Background2`、`GradientAngle`、`Text`、`Opacity`）。預設值取自 v1.0 `ApplyTheme`：明亮 `#F0F0F5`／文字 `#1C1917`／0.882（alpha 225），暗色 `#1E1E24`／`#FFFFFF`／0.85（alpha 217）。
  - `AlarmSettings`：`DefaultSound`=null（不預選，同 v1.0）、`Volume`=100、`SnoozeMinutes`=5。
  - `Normalize()`：null 類別補預設；未定義列舉、角度 0–360、透明度 0–1、音量 0–100、貪睡 <1、不存在的鈴聲修正。
  - `Clone()`：序列化來回深層複製。
- `src/SettingsStore.cs`：`Load()`／`Save()`，寫法同 `AlarmStore`（測試參數、暫存檔取代、失敗不拋例外）。
- `MainWindow`：建構時 `Settings = SettingsStore.Load()`，套用 `Topmost`、主題（FollowWindows 讀 `AppsUseLightTheme` 一次）、時間格式（12／24、秒數）。DWToolkit 視窗擴充狀態（darkTheme、topmost）之後仍覆蓋，故行為同 v1.0。
- 尚未套用：字型、顏色、透明度、音量、貪睡分鐘數、開機啟動（11d–11g）。
- ADR：`docs/adr/0001-settings-file.md`。
