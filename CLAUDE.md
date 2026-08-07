# Solomon_Midas — 開工起手式

本檔為專案根目錄 `CLAUDE.md`，每個 session 開始時會自動載入。目的：確保下列兩份文件
**不必每次手動 `@` 附加**，一開始就在 context 裡。

## 開工前必讀（依序）

1. **`docs/AI_Rules.md`** — 專案最高優先規則（SUPREME PROJECT LAW）。衝突時一律以此為準，
   優先權高於 MEMORY / 任何記憶 / current.md / 其他文件。
2. **`docs/current.md`** — 目前狀態（版本、進度表、鐵則、NEXT、OPEN、REF）。僅為索引，不含
   階段細節；細節在對應 `docs/phases/vX.Y/phase-0Xx.md` 的「Outcome」段。

任何實際動工（讀碼、改碼、規劃）前，先用 Read 工具讀取以上兩份檔案的**當前內容**——本檔只負責
「提醒讀」，不覆述、不快取兩者內容，避免與正本不同步。

## 提醒（摘自 AI_Rules.md，避免遺漏）

- 開發一律先開新分支，測試通過後 commit → merge main → 刪分支；不直接在 main 上 commit。
- 動工前若是 phase 0Xx，還要讀對應 `docs/phases/vX.Y/phase-0Xx.md`（不能只看 current.md）。
- 改完程式碼交還使用者自行編譯/執行，不自行 build/run（除非使用者當輪明確授權）。
