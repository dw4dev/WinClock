# Build & Test Scripts — Universal Baseline

> 跨專案通用（隨 `00-*` 模板一起複用，換專案直接帶走）。
> 定位：**每個專案的基本配備**。建立第一個方案檔的那個 phase，就要一起建立。

---

## 1. 必備腳本

每個專案的 `tools\` 都有 2 組腳本。每組是一個 `.bat` 加一個 `.ps1`。**編譯和測試分開，不合併成一個腳本。**

| 腳本 | 作用 | 基本參數 |
|:--|:--|:--|
| `tools\build.bat` + `build.ps1` | 只編譯，不執行測試 | `-Configuration Debug\|Release`、`-Clean` |
| `tools\run-tests.bat` + `run-tests.ps1` | 只執行測試專案 | `-Filter <名稱>`、`-Configuration Debug\|Release` |

- **既有的等效腳本算數**：`build-debug.*`、`test.*` 這類已存在的腳本不必改名。新專案用上表的名稱。
- 發版、安裝、資料重建等其他腳本，放同一個 `tools\`，同樣用 `.bat` + `.ps1` 成對。

## 2. 腳本慣例

- **`.bat` 只是包裝**：用 `powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0xxx.ps1" %*` 呼叫同名 `.ps1`，並把參數原樣傳入。
- **`.bat` 內容只用 ASCII**（註解用英文），開頭執行 `chcp 65001 >nul`。存檔編碼依 `AI_Rules.md`（ANSI／CP950）。
- **`.ps1` 存成 UTF-8 with BOM**，相容 Windows PowerShell 5.1（不用 `&&`、`||`、三元運算子）。
- **可以點兩下執行**：`.bat` 偵測到由檔案總管啟動時才 `pause`；從命令列呼叫時不暫停。
- **回傳結束代碼**：失敗時 exit code 不是 0，`.bat` 用 `exit /b` 原樣傳回。
- **路徑從腳本位置推算**（`$MyInvocation.MyCommand.Path` 往上一層是 repo 根目錄），不依賴目前工作目錄。
- **輸出精簡**：`dotnet` 用 `--nologo` 與 `-v minimal`（或 `q`）；最後一行明確顯示成功或失敗。

## 3. 通用版腳本

專案沒有特殊需求時，直接複製通用版 4 個檔案（不需要修改）：

- `build.ps1`：自動尋找 `.sln`／`.slnx`（根目錄優先，再往下 2 層）。多個方案檔時用 `-Solution` 指定。
- `run-tests.ps1`：自動尋找檔名結尾是 `Tests.csproj` 的專案，逐一執行 `dotnet test`。沒有測試專案時顯示提示，exit 0。
- 來源：`DWAnnotation\tools\`、`WinClock\tools\`、`IAI\tools\` 內的 4 個檔案內容相同。

專案需要額外步驟時（輸出到 `dist`、先關閉執行中的程式、多個方案檔），在複製後的腳本內修改，並保留第 1 節的參數。

## 4. 和 AI 協作的關係

- `AI_Rules.md` 規定 AI 不自行編譯與執行。AI 交還工作時，**指明要執行哪個腳本**（例：「請執行 `tools\build.bat`，再執行 `tools\run-tests.bat`」），不貼一長串 `dotnet` 指令。
- 新增、改名、搬移專案或方案檔時，同一輪確認 2 組腳本還能用；不能用就一起修。
- README 的「編譯與測試」段落列出這 2 組腳本與參數。

## 5. 自審

- [ ] `tools\` 有編譯腳本與測試腳本，各自是 `.bat` + `.ps1`。
- [ ] 編譯腳本不執行測試；測試腳本可以單獨執行。
- [ ] `.bat` 可以點兩下執行，失敗時回傳非 0。
- [ ] 本次有新增或搬移專案時，已確認腳本仍然找得到方案檔與測試專案。
