# Phase-11e — 外觀：主題與顏色

> 動工前必讀：`docs/kb/00-code-review.md`（通用 Code Smell Checklist）＋ `docs/kb/coding-rules.md`（本專案架構不變式；尚未建立時略過），兩者橫切全 phase。
> 另讀本 phase 相關的 Reference 素材與 ADR/KB；本 phase 改 UI，另讀 `docs/kb/00-ui-rules.md`。

狀態：✅ Done

規格：`docs/specs/v1.1.md` FR-005（主題、顏色部分）

## Goal

主題模式與明亮／暗色兩組顏色設定，套用到主視窗。

## Deliverables

- 主題：明亮／暗色／跟隨 Windows
- 明亮、暗色各一組：背景（單色或 2 色漸層＋角度 0–360°）、文字顏色、透明度
- `MainWindow.xaml` 的 `WindowBackground` 與文字顏色改由設定產生
- 與既有 `btnThemeToggle`（DWToolkit 擴充狀態）整合：只保留一份主題狀態

## Dependencies

Phase-11c。

## Acceptance Criteria

- [x] 漸層角度改變時，預覽立即更新
- [x] 關閉漸層時，背景用單色
- [ ] 「跟隨 Windows」時，系統切換主題後主視窗跟著切換（已實作；未實際切換系統主題測試）
- [x] 主視窗主題按鈕與設定同步；按按鈕後再開設定，顯示的模式正確
- [x] 未修改設定時，外觀和 v1.0 相同
- [x] 所有控制項設 AutomationId

## Outcome

- 新增 `src/ThemeBrushes.cs`：`ParseColor`／`FormatColor`（#RRGGBB）；`CreateBackground(ColorScheme)` 產生單色或 `LinearGradientBrush`（角度 0 為由左到右，順時針；不透明度套到 alpha，四捨五入，v1.0 的 0.85／0.882 得到 217／225）；`CreateText`。
- `AppSettings`：`ColorScheme.Normalize(defaults)` 把格式不對的顏色換成該主題的預設值；`AppearanceSettings.SchemeFor(isDark)`。
- `SettingsStore.Update(Action<AppSettings>)`：讀檔、改一欄、寫回。釘選按鈕與主題按鈕共用。
- `MainWindow`：`ApplyTheme()` 改為依 `Settings` 計算；`WindowBackground`、`TextPrimary` 來自設定，其他顏色（邊框、次要文字、圖示、節氣標籤）仍依明亮／暗色固定。`ApplySettings` 套用主題。訂閱 `SystemEvents.UserPreferenceChanged`：「跟隨 Windows」且系統明暗改變時重新套用（關閉時取消訂閱）。
- 主題狀態只存 `settings.json`：移除 DWToolkit 擴充狀態 `darkTheme`（`MainWindow` 不再實作 `IWindowExtraStateOwner`；舊值被忽略，同 11d 的 `topmost`）。工具列主題按鈕切到另一個固定模式（「跟隨 Windows」也改成明亮或暗色），立即套用並只保存 `Appearance.Theme`；設定視窗開啟時以 `SettingsWindow.SetTheme` 同步 `original` 與 `Working`（取消不還原）。
- 新增 `AppearanceSettingsPage`（主題三選一＋明亮／暗色兩個 GroupBox，外層 ScrollViewer）、`ColorSchemeEditor`（背景、使用漸層、第二色、角度 Slider 0–360 整數、文字顏色、不透明度 Slider 0–100%；未動過不透明度時保留原值）、`ColorField`（#RRGGBB 文字框＋色塊按鈕開 WinForms `ColorDialog`；不合法輸入離開時還原）。
- 新增 AutomationId：`Main_ThemeButton`（Name「切換主題」）、`Settings_AppearancePage`、`Settings_ThemeLightRadioButton`、`Settings_ThemeDarkRadioButton`、`Settings_ThemeFollowWindowsRadioButton`、`Settings_LightSchemeGroup`、`Settings_DarkSchemeGroup`；兩組編輯區以前綴 `Settings_Light`／`Settings_Dark` 加上 `BackgroundTextBox`、`BackgroundPickButton`、`GradientCheckBox`、`Background2TextBox`、`Background2PickButton`、`AngleSlider`、`AngleText`、`TextTextBox`、`TextPickButton`、`OpacitySlider`、`OpacityText`。
- 測試：UIA 腳本（`--dw-state-dir` 隔離）17 項通過：AutomationId 28 個、預設值、主題按鈕寫檔與同步已開啟的設定、預覽漸層角度 90°／0°（截圖確認）、預覽不寫檔、取消還原、確定寫入主題／漸層、不透明度保留 0.85、「跟隨 Windows」依目前系統主題顯示。系統主題即時切換、`ColorDialog` 未自動測試。
