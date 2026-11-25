# ADR-0002 系統匣實作方式

狀態：Accepted（2026-11-25，Phase-11b，使用者確認）

## Context

v1.1 FR-002 要常駐系統匣圖示，選單有顯示、設定、結束。WPF 沒有內建系統匣元件。

## Decision

- 圖示用 WinForms `NotifyIcon`（csproj `UseWindowsForms=true`），不加第三方套件。與 DWAnnotation 做法一致。
- 選單用 WPF `ContextMenu`（右鍵放開時 `IsOpen=true`），不用 WinForms `ContextMenuStrip`：實測 `ToolStripMenuItem.Name` 不會輸出成 UIA AutomationId（讀到空值），WPF `MenuItem` 可以設。之後也能跟隨 WPF 主題。
- WinForms 的隱含 using（`System.Windows.Forms`、`System.Drawing`）在 csproj 移除，避免和 WPF 型別衝突；只在 `src/TrayIcon.cs` 使用。
- 圖示從 exe 取出（`Icon.ExtractAssociatedIcon(Environment.ProcessPath)`，來源是 csproj 的 `ApplicationIcon`），單一檔案發佈也適用。
- AutomationId：選單 `Tray_Menu`，項目 `Tray_ShowMenuItem`、`Tray_SettingsMenuItem`、`Tray_ExitMenuItem`。系統匣圖示本身由 Windows 殼層提供，UIA 名稱是 tooltip（「桌面時鐘」），預設在「顯示隱藏的圖示」溢出區。
- `TrayIcon` 由 `MainWindow` 建立，`OnClosed` 釋放（先 `Visible=false`），結束時不殘留圖示。
- 縮到系統匣 = `Hide()`；視窗隱藏時停止時鐘計時器，鬧鐘計時器照常運作。

## Consequences

- 多載入 WinForms 組件，發佈檔變大。
- 圖示用 WinForms、選單用 WPF，兩套 UI 框架並用；只限於 `src/TrayIcon.cs`。
- 程序被強制終止時，Windows 會殘留圖示，直到滑鼠移過。正常「結束」不殘留。
