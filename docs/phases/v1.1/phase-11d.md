# Phase-11d — 一般設定

> 動工前必讀：`docs/kb/00-code-review.md`（通用 Code Smell Checklist）＋ `docs/kb/coding-rules.md`（本專案架構不變式；尚未建立時略過），兩者橫切全 phase。
> 另讀本 phase 相關的 Reference 素材與 ADR/KB；本 phase 改 UI，另讀 `docs/kb/00-ui-rules.md`。

狀態：✅ Done（2026-11-25）

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

- [x] 開啟開機啟動後，登錄檔有正確的 exe 路徑；關閉後移除
- [x] exe 搬移後手動啟動一次，登錄的路徑自動更新（只在開機啟動已開啟時）
- [x] 開機啟動選「縮到系統匣」，登入後只出現系統匣圖示；手動開啟一律顯示（以 `--autostart` 模擬，未實際重新登入）
- [x] 最上層、時間格式在預覽時立即套用
- [x] 所有控制項設 AutomationId

## Outcome

- 新增 `src/AutoStart.cs`：`Sync(bool)` 寫入／移除 `HKCU\...\Run` 的 `WinClock` 值（`"exe" --autostart`，相同時不寫）；`IsAutoStartLaunch` 判斷命令列。決策見 `adr/0003-autostart.md`。
- `App.xaml` 改用 `Startup`：建立主視窗；`General.AutoStart` 為 true 時同步登錄路徑；`--autostart` 且 `StartupState=Tray` 時不 `Show()`。
- 新增 `src/GeneralSettingsPage.xaml(.cs)`（UserControl）：開機啟動、開機啟動狀態（顯示／縮到系統匣，開機啟動關閉時停用）、最上層、24／12 小時制、顯示秒數。事件用 `Checked`／`Unchecked`（UIA TogglePattern 不觸發 `Click`）。`Load(GeneralSettings)` 後直接改寫該物件並觸發 `Changed`。
- `SettingsWindow`：「一般」類別顯示此頁，其他類別仍為 `Settings_PlaceholderText`；「確定」時 `AutoStart.Sync`。`SetTopmost(bool)` 同步 `original` 與 `Working`（釘選按鈕的修改，取消時不還原）。字型大小 14（使用者指定）。
- 最上層：移除 `MainWindow.xaml` 寫死的 `Topmost="True"`，只以 `settings.json` 為準（預設 true，同 v1.0；使用者指定）；DWToolkit 擴充狀態不再存 `topmost`（舊的 `topmost` 值被忽略）。`ApplySettings` 套用最上層（含設定視窗）與時間格式。工具列釘選按鈕：立即套用，並讀取檔案、只改 `General.Topmost` 後寫回（不提交設定視窗中未確定的修改）。
- 新增 AutomationId：`Settings_GeneralPage`、`Settings_AutoStartCheckBox`、`Settings_StartupShowRadioButton`、`Settings_StartupTrayRadioButton`、`Settings_TopmostCheckBox`、`Settings_Use24HourRadioButton`、`Settings_Use12HourRadioButton`、`Settings_ShowSecondsCheckBox`、`Main_PinButton`（另設 Name「永遠在最上層」）、`Main_TimeText`。
- 測試：UIA 腳本（`--dw-state-dir` 隔離，結束時移除登錄值）28 項通過：AutomationId、預設值、預覽最上層／秒數／12 小時制、取消還原且不寫檔、確定寫入登錄與 json、釘選保存、`--autostart` 只留系統匣、手動開啟顯示、登錄路徑修復、關閉後移除。實際重新登入未測。
