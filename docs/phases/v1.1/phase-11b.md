# Phase-11b — 系統匣圖示與選單

> 動工前必讀：`docs/kb/00-code-review.md`（通用 Code Smell Checklist）＋ `docs/kb/coding-rules.md`（本專案架構不變式；尚未建立時略過），兩者橫切全 phase。
> 另讀本 phase 相關的 Reference 素材與 ADR/KB；本 phase 改 UI，另讀 `docs/kb/00-ui-rules.md`。

狀態：✅ Done（2026-11-25）

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

- [x] 程式啟動後系統匣出現圖示
- [x] 點兩下圖示或選「顯示」，主視窗顯示並取得焦點
- [x] 按主視窗最小化按鈕，視窗縮到系統匣，工作列不顯示
- [x] 選「結束」關閉程式，系統匣圖示消失（不殘留）
- [x] 鬧鐘在視窗縮到系統匣時照常響鈴
- [x] 選單項目設 AutomationId（`kb/00-ui-rules.md` §1）

## Outcome

- ADR：`docs/adr/0002-tray-icon.md`（使用者選 WinForms `NotifyIcon`）。圖示用 `NotifyIcon`；選單改用 WPF `ContextMenu`，因為實測 WinForms `ToolStripMenuItem.Name` 不輸出成 UIA AutomationId。
- 新檔 `src/TrayIcon.cs`：建立圖示（從 exe 取出 `ApplicationIcon`，tooltip＝視窗標題「桌面時鐘」）與選單；事件 `ShowRequested`、`ExitRequested`。左鍵點兩下＝顯示；右鍵放開開啟選單。`Dispose()` 先 `Visible=false` 再釋放。
- 選單：顯示、設定（`IsEnabled=false`，Phase-11c 接上）、分隔線、結束。
- `src/WinClock.csproj`：`UseWindowsForms=true`；移除隱含 using `System.Windows.Forms`、`System.Drawing`，避免和 WPF 型別衝突。
- `src/MainWindow.xaml.cs`：建構時建立 `TrayIcon`，`OnClosed` 釋放。最小化按鈕改呼叫 `HideToTray()`（`Hide()` 後把 `WindowState` 設回 Normal）。其他途徑最小化（`StateChanged`）同樣縮到系統匣。時鐘計時器改由 `IsVisibleChanged` 啟停；鬧鐘計時器不受影響。「結束」呼叫 `Application.Current.Shutdown()`。
- `src/MainWindow.xaml`：最小化按鈕加 AutomationId `Main_MinimizeButton`，Name 與 tooltip 改成「縮到系統匣」。
- AutomationId（新增）：`Main_MinimizeButton`、`Tray_Menu`、`Tray_ShowMenuItem`、`Tray_SettingsMenuItem`、`Tray_ExitMenuItem`。
- 驗收：UI Automation 腳本（`--dw-state-dir` 隔離）實測 6 項全部通過。系統匣圖示在 Win11 預設位於「顯示隱藏的圖示」溢出區。
- 已知限制：程序被強制終止時，Windows 殘留圖示直到滑鼠移過（Windows 行為）。
