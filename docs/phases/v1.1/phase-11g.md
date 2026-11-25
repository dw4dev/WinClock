# Phase-11g — 鬧鐘設定

> 動工前必讀：`docs/kb/00-code-review.md`（通用 Code Smell Checklist）＋ `docs/kb/coding-rules.md`（本專案架構不變式；尚未建立時略過），兩者橫切全 phase。
> 另讀本 phase 相關的 Reference 素材與 ADR/KB；本 phase 改 UI，另讀 `docs/kb/00-ui-rules.md`、`docs/kb/alarms.md`。

狀態：✅ Done

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

- [x] 新增鬧鐘時，鈴聲預設為設定值
- [x] 響鈴音量符合設定
- [x] 貪睡時間符合設定
- [x] 既有 `alarms.json` 正常讀取
- [x] 所有控制項設 AutomationId

## Outcome

- 新增 `AlarmSettingsPage`（設定視窗「鬧鐘」類別）：預設鈴聲（含「（不預選）」）＋試聽按鈕、音量滑桿 0–100、貪睡分鐘數 1–60 下拉選單。
- 新增 `AlarmPlayer`：共用鈴聲播放，用 NAudio `SampleChannel.Volume` 調整音量（不改系統音量）。`MsgWindow`、`AlarmSetupWindow` 改用它，兩者新增 `Volume` 屬性。
- `MainWindow`：移除常數 `SnoozeMinutes`，響鈴時讀 `Settings.Alarm.SnoozeMinutes`；「+」依 `DefaultSound` 預選鈴聲並勾選播放（假設：選了預設鈴聲即表示要播放）。
- `AlarmSettings.Normalize`：貪睡分鐘數上限 60（原本只有下限）。
- `alarms.json` 格式與讀寫程式未改。
- 驗證：`tools\build.bat` 成功；UI Automation 腳本（`--dw-test-mode`）確認設定頁初值、修改值、確定後新增鬧鐘預選「鬧鈴3」且勾選播放、試聽正常。響鈴音量大小與貪睡實際時間為程式碼檢查，未實際等待響鈴。
- AutomationId：見 `kb/alarms.md` §5。
