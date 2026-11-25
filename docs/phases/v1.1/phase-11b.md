# Phase-11b — 系統匣圖示與選單

> 動工前必讀：`docs/kb/00-code-review.md`（通用 Code Smell Checklist）＋ `docs/kb/coding-rules.md`（本專案架構不變式；尚未建立時略過），兩者橫切全 phase。
> 另讀本 phase 相關的 Reference 素材與 ADR/KB；本 phase 改 UI，另讀 `docs/kb/00-ui-rules.md`。

狀態：⬜ 待開始

規格：`docs/specs/v1.1.md` FR-002

## Goal

常駐系統匣圖示；主視窗最小化時縮到系統匣。

## Deliverables

- ADR：系統匣實作方式（WinForms `NotifyIcon` 或第三方套件）——動工前先和使用者確認
- 系統匣圖示（`Images/WinClock.ico`）與選單：顯示、設定、結束
- 「設定」本 phase 先保留項目，開啟設定視窗由 Phase-11c 接上
- 主視窗最小化按鈕改成縮到系統匣，工作列不顯示

## Dependencies

無。

## Acceptance Criteria

- [ ] 程式啟動後系統匣出現圖示
- [ ] 點兩下圖示或選「顯示」，主視窗顯示並取得焦點
- [ ] 按主視窗最小化按鈕，視窗縮到系統匣，工作列不顯示
- [ ] 選「結束」關閉程式，系統匣圖示消失（不殘留）
- [ ] 鬧鐘在視窗縮到系統匣時照常響鈴
- [ ] 選單項目設 AutomationId（`kb/00-ui-rules.md` §1）

## Outcome

（完成時回填）
