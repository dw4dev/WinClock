# Phase-11g — 鬧鐘設定

> 動工前必讀：`docs/kb/00-code-review.md`（通用 Code Smell Checklist）＋ `docs/kb/coding-rules.md`（本專案架構不變式；尚未建立時略過），兩者橫切全 phase。
> 另讀本 phase 相關的 Reference 素材與 ADR/KB；本 phase 改 UI，另讀 `docs/kb/00-ui-rules.md`、`docs/kb/alarms.md`。

狀態：⬜ 待開始

規格：`docs/specs/v1.1.md` FR-006

## Goal

鬧鐘的預設鈴聲、音量、貪睡分鐘數可設定。

## Deliverables

- 預設鈴聲（內建 4 個）、音量 0–100、貪睡分鐘數
- 新增鬧鐘時預先選好預設鈴聲
- 鈴聲播放套用音量；貪睡套用分鐘數（取代固定 5 分鐘）
- `alarms.json` 格式不變
- 更新 `kb/alarms.md` 相關章節

## Dependencies

Phase-11c。

## Acceptance Criteria

- [ ] 新增鬧鐘時，鈴聲預設為設定值
- [ ] 響鈴音量符合設定
- [ ] 貪睡時間符合設定
- [ ] 既有 `alarms.json` 正常讀取
- [ ] 所有控制項設 AutomationId

## Outcome

（完成時回填）
