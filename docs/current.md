# current.md

> Source of truth。Resume 從 NEXT 開始。本檔僅存狀態（STATE）。

## STATE

- 版本：`version.json` 1.1（nbgv）；2026-11-25 開啟 v1.1，規格 `specs/v1.1.md`（持續）；v1.0 維護檔 `specs/v1.0-maintenance.md` 已關閉。`net10.0-windows`；單一專案 `src/WinClock.csproj`。
- 腳本（`tools\`）：`build.bat` 只編譯，輸出到 `build\<組態>\`（預設 `build\Debug\WinClock.exe`）；`release.bat` 發佈單一檔案到 `build\publish`，保留該資料夾根層的 `*.json`（使用者狀態）；`run-tests.bat` 執行測試。
- 共用框架：引用 `..\DWToolkit\dist\DWToolkit.Wpf.props`（DWToolkit 與 WinClock 平行放置）。

## 進度（v1.1 設定功能）

| Phase | 內容 | 依賴 | 狀態 |
|---|---|---|---|
| 11a | `settings.json` 模型與讀寫 | — | ✅ |
| 11b | 系統匣圖示與選單 | — | ✅ |
| 11c | 設定視窗骨架、即時預覽、齒輪按鈕 | 11a、11b | ✅ |
| 11d | 一般設定（開機啟動、最上層、時間格式） | 11b、11c | ✅ |
| 11e | 外觀：主題與顏色 | 11c | ✅ |
| 11f | 外觀：字型 | 11c | ✅ |
| 11g | 鬧鐘設定 | 11c | ✅ |

Phase 文件：`phases/v1.1/phase-11x.md`。v1.0 設計：`kb/alarms.md`。

## OPEN

- 沒有測試專案（`tools\run-tests.bat` 找不到測試時的行為未確認）。

## NEXT

1. v1.1 所有 phase 完成。依 AI_Rules「Release Tag」詢問使用者是否建立 release tag；依「Maintenance」建立 `specs/v1.1-maintenance.md`。

## REF

- `adr/0001-settings-file.md`（settings.json 格式與位置）
- `adr/0002-tray-icon.md`（系統匣：NotifyIcon＋WPF 選單）
- `adr/0003-autostart.md`（開機啟動：HKCU Run＋`--autostart`）
- `kb/00-ui-rules.md`（UI Automation、視窗持久化）
- `kb/alarms.md`（鬧鐘設計、AutomationId、驗收清單、後續候選）
- `..\DWToolkit\docs\kb\referencing.md`（DWToolkit 引用說明）
