#!/usr/bin/env bash
set -euo pipefail

ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
UNITY="${UNITY:-/Applications/Unity/2022.3.62f3c1/Unity.app/Contents/MacOS/Unity}"

if [[ ! -x "$UNITY" ]]; then
  echo "Unity not found at: $UNITY" >&2
  echo "Set UNITY=/path/to/Unity or install the editor first." >&2
  exit 1
fi

mkdir -p "$ROOT/Builds"
"$UNITY" -batchmode -quit \
  -projectPath "$ROOT" \
  -executeMethod SanguoLegend.EditorTools.BuildScript.BuildMac \
  -logFile -
