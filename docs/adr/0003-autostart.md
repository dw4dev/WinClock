# ADR-0003 開機啟動登記方式

狀態：Accepted（2026-11-25，Phase-11d）

## Context

v1.1 FR-004 要開機自動啟動，並可選擇開機時縮到系統匣。程式要分辨開機啟動與手動開啟；exe 搬移後登錄要自動更新。

## Decision

- 登記在 `HKCU\Software\Microsoft\Windows\CurrentVersion\Run`，值名稱 `WinClock`，內容 `"<exe 完整路徑>" --autostart`（`Environment.ProcessPath`）。不用「啟動」資料夾捷徑、不用工作排程器，不需要系統管理員權限。
- 寫入時機：設定視窗按「確定」時，依 `General.AutoStart` 寫入或移除（預覽不碰登錄）。
- 路徑修復：每次啟動時，若 `General.AutoStart` 為 true，比對登錄內容，不同才覆寫。AutoStart 為 false 時不碰登錄。
- 命令列含 `--autostart` 且 `General.StartupState = Tray` 時，`App` 建立主視窗但不 `Show()`，只有系統匣圖示；其他情況一律顯示。`App.xaml` 改用 `Startup` 事件，不用 `StartupUri`。
- 登錄失敗不拋例外，只寫偵錯訊息。實作：`src/AutoStart.cs`。

## Consequences

- 測試參數（`--dw-state-dir`、`--dw-test-mode`）不隔離登錄；自動測試結束時要移除 `WinClock` 值。
- 同一使用者有多份 exe 時，最後一份以開機啟動開啟並啟動的會成為登錄路徑。
