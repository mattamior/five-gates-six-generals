# Initialization Evidence — 2026-10-20

## Observations
- The user created and designated `https://github.com/mattamior/five-gates-six-generals` for the ChatGPT Project 过五关斩六将.
- GitHub repository inspection reported writable access, `main` as default branch, and an empty repository before setup.
- The actual published non-prerelease Agnir release `v1.0.2` was verified in `iorLab/agnir`, with source revision `b5626394ec40a5cb7a28c01892acde07cc0adc8e`.
- The Agnir 1.0 discovery profile requires a 1.0-compatible manifest, two required regular files (State and Next Actions), and correctly typed optional Decisions and Evidence targets.

## Initialization
- A repository README with a backward-compatible Agnir activation locator was created.
- The Agnir discovery manifest, Executor activation instructions, and selected-lineage continuity files are initialized as a coherent repository change.
- Execution-surface activation is explicitly pending, because the GitHub repository write cannot itself configure ChatGPT Project Instructions.

## Verification expectations
- Fresh-read `AGENTS.md` → `AGNIR.md` → `AGNIR.yaml`.
- Parse the manifest against `schemas/agnir-manifest-1.0.schema.json` from pinned v1.0.2.
- Resolve each declared memory locator inside the repository and confirm target shape.
- Read State and Next Actions without relying on the installing conversation.
- Confirm the README heading points to `AGNIR.md` only.
