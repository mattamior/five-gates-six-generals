# Agnir Project Instructions

This file is the canonical Executor-facing activation and Project-operation surface for **过五关斩六将**. Project-owned continuity is declared in `AGNIR.yaml` and `.agnir/`; private conversation state is not the durable authority.

## Activation

1. Enter through the authorized root of `mattamior/five-gates-six-generals` on the selected ref. Do not guess another root, branch, repository, or lineage.
2. Read root `AGNIR.yaml`; validate exact Agnir Core/profile compatibility, `project.identity`, selected `continuity.lineage`, and any separate VCS selector/binding. Reject incompatible, missing, or conflicting discovery rather than silently repairing it.
3. Load the declared selected `memory.state` and `memory.next_actions`. Read Decisions and relevant Evidence as needed. Reconcile with newer user instructions or direct observation, but do not treat private chat history as authoritative Project continuity.
4. Perform the actual Project task. For Agnir distribution operations, use the pinned [Agnir v1.0.2 Skill](https://github.com/iorLab/agnir/blob/v1.0.2/SKILL.md) and applicable Core/profile contracts.

## Checkpoints

At intentional save, finish, or repository commit boundaries:

1. Evaluate material changes to current Project truth; checkpoint the **selected logical lineage** only.
2. Construct and validate the complete coherent checkpoint before publishing changed continuity.
3. If Project continuity did not materially change, checkpoint evaluation is a no-op: do not manufacture `.agnir/` mutations.
4. Reject stale-base publication as `AGNIR_CHECKPOINT_CONFLICT`; re-read and reconcile instead of overwriting newer truth.
5. Verify a fresh read of the same Project identity and selected lineage after publication.

## Repository operations

- Repository `commit`, `提交`, or `提交代码`: evaluate checkpoint → run Project-defined pre-commit policy if declared → commit.
- Repository `commit and push` or `提交推送`: evaluate checkpoint → run declared pre-commit policy if any → commit → push → verify actual destination ref.
- Prefer one coherent revision for real Project changes and related material continuity changes. A checkpoint no-op needs no `.agnir/` diff.
- Git refs/worktrees are selectors and commit SHAs are receipts; neither is the logical lineage identity.
- For branch/lineage integration, reconcile target truth from actual integrated Project state, relevant source continuity/Evidence, and the user's latest instructions. Source continuity is input, not automatically target truth. Publish the integrated Project and reconciled target checkpoint coherently.

## Compatibility locator

The root `README.md` section `Agnir Project Instructions` points here only for backward compatibility. This file remains the canonical Project-operation instruction surface.
