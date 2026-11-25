# Phase-11f — 外觀：字型

> 動工前必讀：`docs/kb/00-code-review.md`（通用 Code Smell Checklist）＋ `docs/kb/coding-rules.md`（本專案架構不變式；尚未建立時略過），兩者橫切全 phase。
> 另讀本 phase 相關的 Reference 素材與 ADR/KB；本 phase 改 UI，另讀 `docs/kb/00-ui-rules.md`。

狀態：✅ Done

規格：`docs/specs/v1.1.md` FR-005（字型部分）

## Goal

中文字型、數字字型、粗細與字型縮放比例可設定。

## Deliverables

- `MainWindow.xaml` 寫死的 `FontFamily="Segoe UI, Microsoft JhengHei UI"`（5 處）改成動態資源
- 中文字型、數字字型各自：字型清單（系統已安裝字型）＋粗細
- 數字一律等寬（`Typography.NumeralAlignment="Tabular"`；字型不支援時的處理寫進 Outcome）
- 字型縮放比例（使用者 2026-11-25 指定）：
  - `AppearanceSettings` 新增一個百分比欄位（建議 `FontScale`，預設 100；範圍待動工時確認，建議 50–200）
  - 乘在 `UpdateFontSizes()` 的寬度比例上，時間、日期、農曆、節氣一起縮放，彼此比例不變
  - 設定頁用 Slider＋數值文字；拖動時即時預覽
  - 預設 100% 時字級和 v1.0 相同

## Dependencies

Phase-11c。

## Acceptance Criteria

- [x] 選字型後預覽立即套用
- [ ] 設定的字型不存在時，退回預設字型，不當機（只做程式碼檢查）
- [x] 秒數跳動時，數字不左右晃動
- [x] 拖動字型縮放比例時，主視窗字級立即改變；取消時還原
- [ ] 未修改設定時，外觀和 v1.0 相同（待使用者目視確認）
- [x] 所有控制項設 AutomationId

## Outcome

2026-11-25 完成。

- 設定：`AppearanceSettings.FontScale`（int %，預設 100，範圍 50–200，`Normalize` 拉回範圍）；`FontSetting.Normalize` 把空白字型名稱、不認得的粗細換成預設值。沒安裝的字型名稱保留在設定裡。
- `FontCatalog`（新檔）：系統已安裝字型清單（`Fonts.SystemFontFamilies`，顯示當地語言名稱）、粗細清單（Thin–Black 9 級）、`ResolveFamily`（沒安裝時退回預設字型）、`ToWeight`。
- 主視窗：5 處寫死的 `FontFamily` 改成 `DynamicResource ClockFontFamily`，值為「數字字型, 中文字型」：數字字型沒有的字元由中文字型補上（同 v1.0 的組合方式）。數字字型本身含中文字時，中文也用數字字型顯示。
- 粗細：時間用 `DigitFontWeight`；日期、節氣用 `ChineseFontWeight`；農曆維持 Normal，版本文字維持預設。
- 等寬數字：時間、日期設 `Typography.NumeralAlignment="Tabular"`。字型不支援 tnum 時 WPF 忽略此設定，數字可能晃動；不另外處理。
- 縮放：`UpdateFontSizes()` 的比例乘上 `FontScale / 100`。`ApplyFonts()` 在啟動與 `ApplySettings`（預覽、確定、取消還原）時執行。
- 設定頁：外觀頁新增「字型」群組。AutomationId：`Settings_FontGroup`、`Settings_ChineseFontComboBox`、`Settings_ChineseFontWeightComboBox`、`Settings_DigitFontComboBox`、`Settings_DigitFontWeightComboBox`、`Settings_FontScaleSlider`（步進 5）、`Settings_FontScaleText`。設定的字型沒安裝時，下拉清單不選任何項目。
- UI Automation 驗證：預設值顯示 100%、兩種字型與粗體；拉到 150% 時時間高度 85→127；改粗細立即預覽；取消後還原；秒數跳動 6 秒時間寬度固定 256。字型不存在的退回路徑只做程式碼檢查，未實機測試（避免改動使用者的 settings.json）。
