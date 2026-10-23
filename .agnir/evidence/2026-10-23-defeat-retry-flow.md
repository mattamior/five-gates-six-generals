# Defeat and Retry Flow — 2026-10-23

## User playtest report
- User confirmed the screenshot showing `关隘已破` was captured after defeating the enemies, not immediately on scene start.
- User reported the tested graybox behavior as normal except that HP reaching zero did not trigger a failure state.

## Implementation
- `LevelDirector` now subscribes to the Player `Damageable.Died` event, disables player movement/combat, releases the cursor, freezes time, shows `战败 / 按 R 重新挑战`, and reloads the active scene on R.
- `GameBootstrap` restores `Time.timeScale` to 1 on scene start and destruction so retries/editor exit do not leave time frozen.

## Verification boundary
- The already-open Unity Editor recompiled after the script edits; its `Editor.log` contains no C# compiler error diagnostics and shows assembly reload.
- No batchmode run was started because the user had the same project open in the GUI Editor.
- HP-zero freeze, defeat UI, and R retry have not yet been manually exercised; they remain the next acceptance check.
- Right-mouse input still does not reduce or negate incoming damage; blocking implementation is pending.
