# Phase-11e — 外觀：主題與顏色

> 動工前必讀：`docs/kb/00-code-review.md`（通用 Code Smell Checklist）＋ `docs/kb/coding-rules.md`（本專案架構不變式；尚未建立時略過），兩者橫切全 phase。
> 另讀本 phase 相關的 Reference 素材與 ADR/KB；本 phase 改 UI，另讀 `docs/kb/00-ui-rules.md`。

狀態：⬜ 待開始

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

- [ ] 漸層角度改變時，預覽立即更新
- [ ] 關閉漸層時，背景用單色
- [ ] 「跟隨 Windows」時，系統切換主題後主視窗跟著切換
- [ ] 主視窗主題按鈕與設定同步；按按鈕後再開設定，顯示的模式正確
- [ ] 未修改設定時，外觀和 v1.0 相同
- [ ] 所有控制項設 AutomationId

## Outcome

（完成時回填）
