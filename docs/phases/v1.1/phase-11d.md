# Phase-11d — 一般設定

> 動工前必讀：`docs/kb/00-code-review.md`（通用 Code Smell Checklist）＋ `docs/kb/coding-rules.md`（本專案架構不變式；尚未建立時略過），兩者橫切全 phase。
> 另讀本 phase 相關的 Reference 素材與 ADR/KB；本 phase 改 UI，另讀 `docs/kb/00-ui-rules.md`。

狀態：⬜ 待開始

規格：`docs/specs/v1.1.md` FR-004

## Goal

填入「一般」類別並套用到主視窗與系統。

## Deliverables

- 開機自動啟動：`HKCU\Software\Microsoft\Windows\CurrentVersion\Run`，指令加 `--autostart`
- 開機啟動狀態：預設顯示／縮到系統匣（只在 `--autostart` 時套用）
- 永遠在最上層（取代寫死的 `Topmost="True"`）
- 時間格式：12／24 小時制、顯示秒數
- ADR：開機啟動登記方式

## Dependencies

Phase-11b、Phase-11c。

## Acceptance Criteria

- [ ] 開啟開機啟動後，登錄檔有正確的 exe 路徑；關閉後移除
- [ ] exe 搬移後手動啟動一次，登錄的路徑自動更新（只在開機啟動已開啟時）
- [ ] 開機啟動選「縮到系統匣」，登入後只出現系統匣圖示；手動開啟一律顯示
- [ ] 最上層、時間格式在預覽時立即套用
- [ ] 所有控制項設 AutomationId

## Outcome

（完成時回填）
