# Phase-11f — 外觀：字型

> 動工前必讀：`docs/kb/00-code-review.md`（通用 Code Smell Checklist）＋ `docs/kb/coding-rules.md`（本專案架構不變式；尚未建立時略過），兩者橫切全 phase。
> 另讀本 phase 相關的 Reference 素材與 ADR/KB；本 phase 改 UI，另讀 `docs/kb/00-ui-rules.md`。

狀態：⬜ 待開始

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

- [ ] 選字型後預覽立即套用
- [ ] 設定的字型不存在時，退回預設字型，不當機
- [ ] 秒數跳動時，數字不左右晃動
- [ ] 拖動字型縮放比例時，主視窗字級立即改變；取消時還原
- [ ] 未修改設定時，外觀和 v1.0 相同
- [ ] 所有控制項設 AutomationId

## Outcome

（完成時回填）
