# Phase-11c — 設定視窗骨架

> 動工前必讀：`docs/kb/00-code-review.md`（通用 Code Smell Checklist）＋ `docs/kb/coding-rules.md`（本專案架構不變式；尚未建立時略過），兩者橫切全 phase。
> 另讀本 phase 相關的 Reference 素材與 ADR/KB；本 phase 改 UI，另讀 `docs/kb/00-ui-rules.md`。

狀態：⬜ 待開始

規格：`docs/specs/v1.1.md` FR-003

## Goal

建立設定視窗的版面與共用機制。各類別的內容由 11d～11g 填入。

## Deliverables

- 設定視窗：左側導覽（一般／外觀／鬧鐘），右側內容（本 phase 放空白頁）
- 底部按鈕：確定、取消、重設此類別、全部重設
- 即時預覽機制：開啟時複製目前設定；修改時通知主視窗套用；取消或關閉視窗時還原
- 入口：主視窗工具列齒輪按鈕；系統匣選單「設定」
- 單一實例：已開啟時切到前景
- 視窗位置與最後選取的類別，走 DWToolkit `WindowPersistence`

## Dependencies

Phase-11a、Phase-11b。

## Acceptance Criteria

- [ ] 齒輪按鈕、系統匣「設定」都能開啟設定視窗；重複開啟不產生第二個
- [ ] 「確定」寫入 `settings.json` 並關閉
- [ ] 「取消」或按 X 關閉時，主視窗還原成開啟前的設定
- [ ] 「重設此類別」只重設目前類別；「全部重設」重設全部；都要按「確定」才寫入
- [ ] 重新開啟時，視窗位置與類別還原
- [ ] 所有控制項設 AutomationId

## Outcome

（完成時回填）
