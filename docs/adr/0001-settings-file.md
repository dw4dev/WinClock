# ADR-0001 設定檔格式與位置

狀態：Accepted（2026-11-25，Phase-11a）

## Context

v1.1 新增設定功能（`docs/specs/v1.1.md` FR-001）。設定要在重新啟動後還原；檔案不存在、損毀或缺欄位時不能當機。v1.0 已有 `alarms.json`（`src/AlarmStore.cs`）與 DWToolkit 的 `window-state.json`。

## Decision

- 位置：exe 旁的 `settings.json`（`AppContext.BaseDirectory`）。測試參數同 `AlarmStore`：`--dw-test-mode` 不讀也不寫；`--dw-state-dir=資料夾` 改用該資料夾。
- 格式：UTF-8（無 BOM）、縮排 JSON，System.Text.Json。屬性名稱 PascalCase，讀取不分大小寫。列舉存成字串。顏色存成 `#RRGGBB`，透明度 0–1。
- 結構：`Version`（目前 1）＋三個類別 `General`、`Appearance`、`Alarm`。模型：`src/AppSettings.cs`；讀寫：`src/SettingsStore.cs`。
- 預設值：寫在模型屬性的初始值，等於 v1.0 行為。
- 容錯：
  - 檔案不存在 → 全部預設值。
  - JSON 語法錯誤或型別不符 → 全部預設值（只寫偵錯訊息）。
  - 缺欄位 → 只有該欄位用預設值；類別為 `null` → 該類別用預設值。
  - 超出範圍（角度、透明度、音量、貪睡分鐘數、不存在的鈴聲、未定義的列舉）→ `Normalize()` 拉回範圍或用預設值。
- 寫入：先寫 `settings.json.tmp` 再取代，避免留下半個檔案。整份覆寫。
- 複製：`AppSettings.Clone()` 用序列化來回做深層複製，供設定視窗「取消」還原。
- 發佈：`tools\release.ps1` 保留 `build\publish` 根層的 `*.json`，所以 `settings.json` 會保留。

## Consequences

- 與 `alarms.json` 寫法一致，維護一套模式。
- 單一欄位型別錯誤會讓整份退回預設值；接受此取捨，換取簡單的實作。
- v1.0 的主題與釘選仍由 DWToolkit 視窗擴充狀態保存，啟動時覆蓋 `settings.json` 的值。Phase-11d／11e 整合主題按鈕與最上層時再決定是否移除。
- 格式不相容的變更要提高 `Version`，並列入下一版。
