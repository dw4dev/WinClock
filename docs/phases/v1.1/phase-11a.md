# Phase-11a — `settings.json` 設定檔模型與讀寫

> 動工前必讀：`docs/kb/00-code-review.md`（通用 Code Smell Checklist）＋ `docs/kb/coding-rules.md`（本專案架構不變式；尚未建立時略過），兩者橫切全 phase。
> 另讀本 phase 相關的 Reference 素材與 ADR/KB。

狀態：⬜ 待開始

規格：`docs/specs/v1.1.md` FR-001

## Goal

建立設定模型與 `settings.json` 的讀寫，供後續 phase 使用。本 phase 不做 UI。

## Deliverables

- 設定模型：一般（開機啟動、啟動狀態、最上層、12／24 小時制、顯示秒數）、外觀（主題模式、中文字型＋粗細、數字字型＋粗細、明亮／暗色各一組顏色：背景單色或 2 色漸層＋角度、文字顏色、透明度）、鬧鐘（預設鈴聲、音量、貪睡分鐘數）
- 每個欄位的預設值等於 v1.0 現行行為
- 設定存取類別（參考 `src/AlarmStore.cs` 的寫法）：讀取、寫入、複製（供設定視窗取消時還原）
- ADR：設定檔格式與位置

## Dependencies

無。

## Acceptance Criteria

- [ ] 檔案不存在時，回傳預設值
- [ ] 內容損毀或缺欄位時，回傳預設值（缺欄位只補該欄位），不當機
- [ ] 寫入後再讀取，值相同
- [ ] 主視窗啟動時讀取設定，行為和 v1.0 相同
- [ ] `release.bat` 發佈時保留 `settings.json`（根層 `*.json` 已保留，確認即可）

## Outcome

（完成時回填）
