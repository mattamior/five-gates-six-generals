# Decisions

## D-001 — Canonical repository
- Date: 2026-10-20
- Decision: Use `mattamior/five-gates-six-generals` as the canonical GitHub Project repository for 过五关斩六将.
- Evidence: User supplied this newly created repository after selecting the name.

## D-002 — Agnir compatibility and distribution
- Date: 2026-10-20
- Decision: Fresh initialization uses published Agnir v1.0.2, Core `1.0`, and `repository-filesystem/1.0`.
- Distribution source: `iorLab/agnir` release `v1.0.2` at `b5626394ec40a5cb7a28c01892acde07cc0adc8e`.

## D-003 — Selected initial continuity
- Date: 2026-10-20
- Decision: Use a separate durable Project identity and logical Continuity Lineage, initially bound to `refs/heads/main` as a selector.
- Project identity: `urn:agnir:project:mattamior:five-gates-six-generals`
- Logical lineage: `urn:agnir:lineage:five-gates-six-generals:primary`
- The lineage is not derived from a branch name or commit SHA.

## D-004 — Initial game scope and engine
- Date: 2026-10-22
- Decision: Build a Three Kingdoms 3D action-story demo, with Dongling Pass as the first graybox slice for the Five Gates / Six Generals story.
- Engine choice: Unity, as selected by the user. The locally available editor is Unity 2022.3.62f3c1 China Edition (Apple Silicon); Unity 6 remains a later upgrade option when a verified installer is available.
- Evidence: User's project requests and verified installed editor/project artifacts.

## D-005 — Graybox defeat response
- Date: 2026-10-23
- Decision: At player HP zero, freeze the graybox encounter, show a defeat prompt, and allow R to reload the current scene for retry.
- Evidence: User reported the missing failure check as an odd behavior after the initial playtest; implementation is in `LevelDirector` and `GameBootstrap`, with manual verification pending.
