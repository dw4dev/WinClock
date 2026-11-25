# Phase-11c — 設定視窗骨架

> 動工前必讀：`docs/kb/00-code-review.md`（通用 Code Smell Checklist）＋ `docs/kb/coding-rules.md`（本專案架構不變式；尚未建立時略過），兩者橫切全 phase。
> 另讀本 phase 相關的 Reference 素材與 ADR/KB；本 phase 改 UI，另讀 `docs/kb/00-ui-rules.md`。

狀態：✅ Done（2026-11-25）

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

- [x] 齒輪按鈕、系統匣「設定」都能開啟設定視窗；重複開啟不產生第二個
- [x] 「確定」寫入 `settings.json` 並關閉
- [x] 「取消」或按 X 關閉時，主視窗還原成開啟前的設定
- [x] 「重設此類別」只重設目前類別；「全部重設」重設全部；都要按「確定」才寫入
- [x] 重新開啟時，視窗位置與類別還原
- [x] 所有控制項設 AutomationId

## Outcome

- 新增 `src/SettingsWindow.xaml(.cs)`：標準視窗框，560×420，可調整大小；`dw:WindowPersistence.Key="Settings"`，擴充狀態 `category` 記住最後選取的類別。
- 左側 `ListBox`（一般／外觀／鬧鐘，`Tag` = `SettingsCategory` 名稱）；右側 `ContentControl` 目前只放 `Settings_PlaceholderText`。11d～11g 依 `SettingsCategory` 換成各類別頁面，修改 `Working` 後呼叫 `OnSettingsChanged()`。
- 即時預覽：建構時 `original` 與 `Working` 各複製一份目前設定；`OnSettingsChanged()` 把 `Working` 的副本交給主視窗 `ApplySettings`。「確定」：`SettingsStore.Save(Working)` 後套用並關閉。未確定就關閉（取消、Esc、X）：`OnClosing` 套用 `original`。
- `AppSettings.Reset(SettingsCategory)` 重設單一類別；「全部重設」換成 `new AppSettings()`。兩者都不寫檔。
- `MainWindow.ApplySettings` 目前只換 `Settings` 並更新時間文字。最上層與主題刻意不套用，以免覆蓋工具列釘選／主題按鈕的狀態；由 11d／11e 接上。
- 入口：工具列齒輪按鈕 `Main_SettingsButton`（主題按鈕左側）；系統匣「設定」改為可用（`TrayIcon.SettingsRequested`）。單一實例：`MainWindow.settingsWindow` 欄位，已開啟時還原最小化並 `Activate()`。不設 Owner（主視窗會隱藏到系統匣）；`Topmost` 跟隨主視窗。
- AutomationId：`Settings_Window`、`Settings_CategoryList`、`Settings_{General|Appearance|Alarm}CategoryItem`、`Settings_CategoryContent`、`Settings_PlaceholderText`、`Settings_OkButton`、`Settings_CancelButton`、`Settings_ResetCategoryButton`、`Settings_ResetAllButton`、`Main_SettingsButton`。
- 測試：UIA 腳本（`--dw-state-dir` 隔離）19 項通過：齒輪開啟、單一實例、AutomationId、重設與取消不寫檔、確定寫檔、類別還原。系統匣「設定」沒有自動測試（NotifyIcon 不在 UIA 樹），待手動確認。
