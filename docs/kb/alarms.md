# 鬧鐘與視窗持久化

> 2026-11-14 定案（使用者）。程式碼：`src/MainWindow.xaml(.cs)`、`AlarmItem.cs`、`AlarmStore.cs`、`AlarmCfg.cs`、`MsgWindow.xaml(.cs)`。

## 1. 已定案的決策

- 工具列只顯示「生效中」的鬧鐘。一開始只有「+」。
- 按「+」開啟設定視窗，按「確定」後出現一個鬧鐘按鈕。
- 鬧鐘響完或被移除後，按鈕消失，後面的鬧鐘往前遞補編號。
- 上限 5 個（`MainWindow.MaxAlarms`）。滿 5 個時「+」隱藏。之後視情況開放。
- 鬧鐘保存在 exe 旁的 `alarms.json`，與視窗狀態（`window-state.json`）分開。
- 主題與釘選保存在 DWToolkit 的視窗擴充狀態。
- 加入：鬧鐘按鈕提示、貪睡。

## 2. 行為

| 動作 | 結果 |
|:--|:--|
| 按「+」 | 預設時間是現在加 5 分鐘。按「確定」新增；按「取消」不新增 |
| 按鬧鐘按鈕 | 編輯副本。「取消」不影響原本的鬧鐘；「確定」更新；「清除鬧鐘」移除 |
| 滑鼠移到鬧鐘按鈕 | 提示：日期時間、剩餘時間、訊息、鈴聲 |
| 時間到 | 移除該鬧鐘，顯示訊息視窗（有設定鈴聲時播放） |
| 訊息視窗按「5 分鐘後提醒」 | 用同一份設定新增一個鬧鐘，時間是現在加 5 分鐘（精確到秒）。鬧鐘已滿 5 個時不新增 |
| 關閉程式再開啟 | 還原尚未到時間的鬧鐘。關閉期間已過時間的鬧鐘直接移除，不補響 |

計時方式：每個鬧鐘一個 `DispatcherTimer`，最長每 30 秒重新比對一次時間（`AlarmCheckInterval`）。
原因：`DispatcherTimer` 的間隔上限約 24.8 天，而且電腦睡眠後間隔會不準。

## 3. 保存格式

`alarms.json`（UTF-8 無 BOM）：

```json
{
  "Version": 1,
  "Alarms": [
    { "Target": "2026-11-14T18:30:00", "IsPlaySound": true, "SoundName": "鬧鈴1", "IsShowMsg": true, "MsgText": "開會" }
  ]
}
```

- 讀取失敗、格式錯誤：當成沒有鬧鐘，不拋例外。下次儲存時覆寫。
- 鈴聲名稱不在 `SG.AlarmSounds` 內：改成不播放。
- 超過上限的項目：依時間保留最早的 5 個。

`window-state.json` 的 `Main` 視窗擴充狀態：`darkTheme`、`topmost`。

測試模式（DWToolkit 參數，兩個檔案都適用）：

- `--dw-test-mode`：不讀也不寫。
- `--dw-state-dir=<資料夾>`：改用隔離資料夾。

## 4. 視窗持久化

| 視窗 | 方式 | 說明 |
|:--|:--|:--|
| `MainWindow` | `dw:WindowPersistence.Key="Main"` + `IWindowExtraStateOwner` | `SizeToContent="Height"`：還原位置與寬度，高度由內容決定 |
| `AlarmSetupWindow` | 退出 | 固定大小對話框，每次置中 |
| `MsgWindow` | 退出 | 固定大小提醒對話框，每次置中 |

## 5. AutomationId（對外契約）

- 主視窗：`Main_Window`、`Main_AddAlarmButton`、`Main_AlarmButton1` ~ `Main_AlarmButton5`（編號隨遞補改變）。
- 設定視窗：`AlarmSetup_Window`、`AlarmSetup_DatePicker`、`AlarmSetup_HourComboBox`、`AlarmSetup_MinuteComboBox`、
  `AlarmSetup_PlaySoundCheckBox`、`AlarmSetup_SoundComboBox`、`AlarmSetup_PlayButton`、`AlarmSetup_StopButton`、
  `AlarmSetup_ShowMsgCheckBox`、`AlarmSetup_MessageTextBox`、`AlarmSetup_RemoveButton`、`AlarmSetup_CancelButton`、
  `AlarmSetup_OkButton`、`AlarmSetup_HeaderCloseButton`。
- 訊息視窗：`Msg_Window`、`Msg_HeaderText`、`Msg_MessageText`、`Msg_SnoozeButton`、`Msg_CloseButton`、`Msg_HeaderCloseButton`。
- 尚未加：主視窗的釘選、主題、最小化、關閉按鈕，以及時間、日期文字（這一輪沒有修改）。

## 6. 手動驗收清單

1. 首次開啟：工具列只有釘選與「+」。
2. 新增 5 個鬧鐘：「+」在第 5 個之後隱藏；最小寬度 330 時按鈕不重疊。
3. 修改鬧鐘後按「取消」：時間不變。
4. 移除中間的鬧鐘：後面的編號往前遞補，「+」重新出現。
5. 提示顯示時間與剩餘時間，再次移入時剩餘時間更新。
6. 時間到：訊息視窗出現，按「5 分鐘後提醒」後工具列多一個鬧鐘。
7. 關閉再開啟：鬧鐘、位置、寬度、主題、釘選都還原。
8. 設定 30 天後的鬧鐘：可以按「確定」（舊版會誤判成時間不正確）。
9. `WinClock.exe --dw-test-mode`：不還原也不寫入任何狀態。
10. 執行 `tools\release.bat` 後，`build\publish` 內的 `window-state.json`、`alarms.json` 仍然存在。

## 7. 後續候選（尚未排入）

- 重複鬧鐘（每天、平日、每週指定日）。
- 倒數計時器（例：25 分鐘番茄鐘），與鬧鐘共用提醒視窗。
- 貪睡分鐘數可設定。
- 鬧鐘清單視窗（上限開放後，工具列放不下時使用）。
- 多時區時鐘列。
- 開機自動啟動選項。
- 主視窗其餘控制項補 AutomationId。
- 加入測試專案，涵蓋 `AlarmStore` 讀寫容錯與 `AlarmItem.FormatRemaining`。
