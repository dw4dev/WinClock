################################################################
# AI_Rules.md — SUPREME PROJECT LAW (read first, obey always)
# These instructions OVERRIDE all default behavior, all memory,
# and any recalled note. This is NOT background context.
# If anything conflicts with this file, THIS FILE WINS.
################################################################

# Authority

- This file is the **single highest-priority instruction in the project**. Treat every line as a hard directive, not advisory context.
- Precedence on conflict (strict): **AI_Rules.md > MEMORY.md / recalled memory > current.md > ADR/KB > phase docs > other docs**.
- MEMORY / recalled notes may *inform* but **never override** a rule here. On conflict: follow AI_Rules, then tell the user the memory is stale.
- When this file is provided in a turn, **re-anchor to it**: its rules take effect immediately and persist for the session, even if buried under other context.
- If you are about to act in a way that contradicts any rule here, STOP and say so first.

# Decision Discipline (clarify first, don't guess)

- **Use explicit sources.** Base every decision on user input, specifications, or provided resources to maximize first-pass accuracy.
- **Clarify before coding.** Confirm requirements whenever goals, behavior, interactions, or UI are not fully defined.
- **Review all references first.** Read every referenced resource (`docs/specs/resources/vX.Y/`) before implementation. Request any missing references before proceeding.
- **Act independently on mechanical details.** Apply standard conventions for product-neutral details (naming, formatting, imports). If the user delegates a decision for this session, state the assumption and continue.

## Rule of Three (user directive)

- **Before building any UI pattern, helper, or logic, search for similar code.** If it already exists twice, STOP: extract a shared component / method first, then use it (including replacing the existing copies, or log them for gradual replacement).
- Applies to XAML snippets too (icon + tooltip, label + value rows, etc.), not only C#.
- Say so in the turn it is found: name the 2 existing places and the proposed shared form, then proceed per the Interrogation Gate.

# Runtime

## Output

- code/answer first
- no filler, minimal prose
- Dev: full code, minimal comments
- Debug: fix/diff only, root cause in 1 line

## Build Control

- No self-build / self-run after changes.
- Always hand off to user for compilation/execution.
- Only process user-provided error snippets.
- Prevent log inflation in context.

### Build & test scripts (2026-11-14, user directive)

- Every project keeps one-click scripts in `tools\`, each a `.bat` wrapper + `.ps1` pair: `build.bat` / `build.ps1` (compile only) and `run-tests.bat` / `run-tests.ps1` (tests only). Build and test stay separate scripts.
- Existing equivalents under other names (`build-debug.*`, `test.*`) count; do not rename them. New projects use the names above.
- Create the scripts in the same phase that creates the first solution. When projects or solutions are added, renamed, or moved, keep the scripts working in the same turn.
- On hand-off, name the script to run (e.g. "run `tools\build.bat`, then `tools\run-tests.bat`"), not raw `dotnet` commands.
- Conventions and the generic copy-ready scripts: `docs/kb/00-build-scripts.md`.

## Context

Load (general): current.md > ADR > KB > requested files
Skip: history, completed phases, old specs, release notes, logs

### Retrieval Efficiency (token budget)

- **Retrieve only what the task requires.** Start with targeted search, then read only the relevant ranges. Read the whole file only when it is small (≲400 lines) or multiple edits are expected.
- **Read the full implementation before refactoring or rewriting.** Understand the architecture, flow, dependencies, and edge cases before making changes.
- **Use authoritative runtime data.** For databases, logs, configs, or live state, inspect them with read-only queries or scripts.
- **Validate design changes with real data.** Verify thresholds, calibration, algorithms, or behavioral changes against historical or production data whenever practical.
- **Reuse existing context.** Avoid retrieving the same unchanged information more than once.

### Before coding a phase (mandatory)

- current.md is **STATE/index only — NOT a handoff doc**. Never treat it as the source of phase detail or as enough to start coding.
- Before implementing phase 0Xx you MUST read, in order:
  1. `docs/phases/vX.Y/phase-0Xx.md` (the phase dev doc; include adjacent phase docs if the work spans them)
  2. any `Reference:` resources it points to under `docs/specs/resources/vX.Y/`
  3. `docs/kb/00-code-review.md` — ALWAYS (universal code-smell checklist; cross-project)
  4. `docs/kb/00-ui-rules.md` — ALWAYS when the work adds or changes UI (universal UI checklist: UI Automation + window persistence; cross-project)
  5. `docs/kb/coding-rules.md` — ALWAYS once it exists (WinClock architectural invariants; cross-cutting across every phase, never left to keyword search)
  6. other relevant ADR / KB (search by the phase's topic)
- If the phase doc or a needed reference resource is missing/empty, **stop and ask** — do not infer the spec from current.md.

## Memory

Store: state, decisions, blockers, next, refs
Drop: reasoning, discussion, attempts, obsolete items
Rule: State > History

## Budget

Trigger handover when ANY:

- system message compression detected
- single task exceeds 15 turns without completion
- OPEN items grow instead of shrinking for 3 consecutive turns

On trigger:

1. stop current expansion immediately
2. finish only the in-progress atomic unit (e.g. current file edit)
3. update current.md
4. handover, continue in new session

## Handover

STATE / DONE / OPEN / NEXT / RISK / REF

## current.md

current.md holds STATE only: version, progress table, hard-rules, NEXT, OPEN, REF.

On phase completion (at wrap-up, NOT deferred to a later slim pass):

1. Write the COMPLETE result of the phase to its `phase-0Xx.md` "## Outcome" section, and set its status to ✅ Done. That doc is the permanent home — write it in full so it can be searched later without this chat.
2. In current.md: flip the progress-table row to ✅ and update NEXT only. Never copy phase detail into current.md.
3. Remove obsolete items.

Before handover: sync all sections, compress state.

Limit: <2500 tokens (the progress table is the irreducible floor; any other section exceeding it triggers relocation).

### Slim rule (slimming = relocate, never delete)

Completed-phase detail must NOT accumulate in current.md. Relocate first, then keep only a pointer:

- "what actually shipped" → `phase-0Xx.md` "## Outcome" section (+ status ✅ Done)
- design trade-offs / locked definitions → ADR
- known limitations / blind spots → `docs/kb/known-limitations.md`
- pure process / attempts / commit detail → not relocated, git log already has it

Never delete a detail from current.md unless its durable content already has a permanent home above. Slimming without relocating = data loss (the root cause of past forgetting).

## Spec Status (2026-10-10, user directive; added to WinClock 2026-11-21)

Each version spec under `docs/specs/` carries one `Status`, moving forward only: **Draft → Approved → 持續 → 關閉**.

| Status | Meaning | Trigger |
|:--|:--|:--|
| Draft | Interview in progress | Spec created |
| Approved | User confirmed; phases may be decomposed; version not opened | User confirms consensus |
| 持續 | Version open; keeps receiving changes (main line in the spec; from the maintenance start point, changes go to `vX.Y-maintenance.md`, see "Maintenance") | Version opened |
| 關閉 | Frozen: no new FR, sub-case, or addition (the maintenance file closes with it) | Next version opened |

- Opening a new version = ONE first commit: bump the minor version in `version.json` (NBGV), set the new spec to 持續, set the previous spec to 關閉. Requires the user's explicit instruction.
- Open work in a spec about to close, including open maintenance rows (⬜／🔄), moves to the new spec before or in that commit. Only typo/link fixes go into a 關閉 spec.
- Only one spec may be 持續 at a time.
- v1.0 has no spec. Its work units are the current.md progress rows; its design lives in `docs/kb/` (e.g. `kb/alarms.md`).

## Maintenance (2026-11-21, user directive)

**Start point**: every phase (for v1.0: every progress row) of version vX.Y is done, its tests passed, and it is merged into `main`. Before the start point this section does not apply. At the start point, create `docs/specs/vX.Y-maintenance.md` and point current.md to it. v1.0 entered maintenance on 2026-11-21 (user decision).

- **Version**: stays vX.Y.x; never bump the minor number in maintenance. The patch number comes from Nerdbank.GitVersioning (NBGV); never edit `version.json` by hand.
- **Record**: every maintenance change goes to `docs/specs/vX.Y-maintenance.md` only, one row per item: `M-NNN` (from M-001, never reused) | date | type (錯誤／調整／功能異動) | content | affected spec section (v1.0: the KB doc section, e.g. `kb/alarms.md` §7) | status (⬜ 待處理／🔄 進行中／✅ 完成／⏭️ 移到下一版／❌ 不做) | commit.
- **Spec and phase docs**: the spec stays 持續 but gets no new content (typo/link fixes only). Completed phase docs are history; never edit them. On conflict, the newer maintenance row wins over the spec and the KB design doc.
- **Small functional changes go straight into vX.Y.x.** No minor bump needed.
- **Minor-version suggestion**: when an item adds a new FR or a new window/tool, changes a file format (`alarms.json`, window state, other settings) incompatibly, needs a new ADR, or is too big for one session, propose moving it to vX.(Y+1) (a Draft spec). The user decides; until then, keep it ⬜.
- **Workflow**: one item = one branch `maint/M-NNN-<slug>` = one session: implement → hand off `tools\build.bat` (and `tools\run-tests.bat` once a test project exists) plus the manual acceptance items per "Build Control" → set the row ✅ with the commit → merge into `main` → delete the branch (see "Git / Commit").
- **current.md** in maintenance: NEXT points to the open rows of the maintenance file; never copy the rows into current.md.
- **Next version**: the opening commit moves every open row (⬜／🔄) to the new spec and closes the maintenance file (see "Spec Status").

## Release Tag (2026-11-21, user directive)

- At the wrap-up of a version's last phase test step (all phases done, tests passed, merged into `main`), ask the user whether to create a release tag. Never create one without a yes.
- Tag the merge commit on `main` with `nbgv tag` (name `vX.Y.<height>`, matching the build version). Push the tag only when the user says so.
- In maintenance, ask again only when the user plans a release build (`tools\release.bat`) of the maintenance line.

## Resume

Load: current.md > REF
Source of truth: current.md
Ignore: old chats, reasoning history
Resume from NEXT

## Priority

task > state > tokens
compression > context growth

## UI Baseline (2026-11-14, user directive)

Applies to every window, dialog, and control this project adds or changes. Detail and self-review checklist: `docs/kb/00-ui-rules.md`.

- **Design UI for UI Automation.** Give every interactive control, every value a test must read, and every Window a stable `AutomationProperties.AutomationId`. Use standard controls (or provide an `AutomationPeer`), and expose busy/done/error state through readable elements. Treat an AutomationId as a public contract.
- **Persist window position and size by default.** Every movable or resizable window restores its position, size, and maximized state through the shared window base — never hand-rolled per window. Opt-out must be explicit, with the reason. Window-specific layout state (column widths, splitter ratios, selected tab) goes through the same mechanism.

## Git / Commit

- Keep messages concise: one-line summary; bullet body only if necessary.
- STRICTLY PROHIBIT `Co-Authored-By` in commit messages (do not tag AI as co-author).
- PROHIBIT `--no-verify` / `--no-gpg-sign` (unless user explicitly requests).
- Branching: always develop on a new branch, never commit directly to the default branch. After tests pass: commit, merge into `main`, delete the feature branch immediately.
- Stop after sub-phases (2026-07-30 user directive): when a task is split into phases/sub-phases, after each sub-phase is completed and committed, just stop — do NOT ask whether to proceed. A new session will pick up the next sub-phase.

## Windows Scripting & Encoding

- PowerShell scripts (*.ps1) MUST be saved as `UTF-8 with BOM`.
- Batch files (*.bat) MUST be saved as `ANSI` (Windows-950 / Big5).
