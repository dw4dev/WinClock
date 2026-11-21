# current.md

> Source of truth。Resume 從 NEXT 開始。本檔僅存狀態（STATE）。

## STATE

- 版本：`version.json` 1.0（nbgv）；維護階段 2026-11-21 起，版號 1.0.x，紀錄 `specs/v1.0-maintenance.md`（規則見 AI_Rules〈Maintenance〉）；`net10.0-windows`（2026-11-14 從 net8 升級）；單一專案 `src/WinClock.csproj`。
- 腳本（`tools\`）：`build.bat` 只編譯，輸出到 `build\<組態>\`（預設 `build\Debug\WinClock.exe`）；`release.bat` 發佈單一檔案到 `build\publish`，保留該資料夾根層的 `*.json`（使用者狀態）；`run-tests.bat` 執行測試。
- 共用框架：引用 `..\DWToolkit\dist\DWToolkit.Wpf.props`（DWToolkit 與 WinClock 平行放置）。

## 進度

| 項目 | 內容 | 狀態 |
|---|---|---|
| DWToolkit 引用 | 主視窗位置／寬度持久化，主題與釘選走擴充狀態 | ✅ 2026-11-14 編譯 0 警告 0 錯誤、手動驗收通過 |
| 動態鬧鐘 | 「+」新增，上限 5，按鈕提示，保存到 `alarms.json`，貪睡 5 分鐘 | ✅ 2026-11-14 編譯 0 警告 0 錯誤、手動驗收通過 |
| 升級 .NET 10、腳本整理 | `net10.0-windows`；編譯輸出到 `build\<組態>`；`release.bat` 移到 `tools\` | ✅ 2026-11-14 |

設計與行為：`kb/alarms.md`。

## OPEN

- 沒有測試專案（`tools\run-tests.bat` 找不到測試時的行為未確認）。鬧鐘邏輯目前只能手動驗收。

## NEXT

1. 從 `specs/v1.0-maintenance.md` 挑 ⬜ 項目，一個項目一個 session（分支 `maint/M-NNN-<slug>`）。
2. 使用者從 `kb/alarms.md` §7 後續候選挑選的項目，先登記成 M 列再動工；符合次版號建議條件時，向使用者建議改排進 v1.1。

## REF

- `kb/00-ui-rules.md`（UI Automation、視窗持久化）
- `kb/alarms.md`（鬧鐘設計、AutomationId、驗收清單、後續候選）
- `..\DWToolkit\docs\kb\referencing.md`（DWToolkit 引用說明）
