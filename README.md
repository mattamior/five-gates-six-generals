# 单骑 · 过五关

《三国演义》第二十七回「美髯公千里走单骑 汉寿侯五关斩六将」的 3D 动作剧情 Demo。

- 引擎：Unity 2022.3 LTS（China Edition `2022.3.62f3c1`）+ 内置渲染管线灰盒，URP 待接入
- 平台：macOS（Apple Silicon），1080p / 60fps
- 形态：第三人称动作 + 护送，30 分钟一周目

## 目录

```
Assets/
  Scripts/Runtime/   运行时逻辑（移动、战斗、敌人、叙事）
  Scripts/Editor/    编辑器工具（灰盒场景、构建、截图）
  Data/              关卡与角色数据（ScriptableObject）
  Scenes/            场景
  Settings/          渲染与材质
docs/
  demo-plan.zh.md    完整方案
  setup.md           环境与工具链说明
tools/
  build.sh           命令行构建
  shots.sh           命令行多机位截图
```

## 打开

1. 用 Unity Hub / 编辑器打开本目录。
2. 菜单 `Sanguo → Setup Project`（一次性：色彩空间、产品名、目录）。
3. 菜单 `Sanguo → Build Graybox Scene` 生成东岭关灰盒。
4. 运行，或 `Sanguo → Build macOS` 出包。

## 操作

| 输入 | 行为 |
| --- | --- |
| WASD | 移动 |
| 鼠标 | 视角 |
| Shift | 疾跑 |
| 左键 | 轻击（横扫） |
| Q | 重击 |
| 右键按住 | 格挡 |
| F | 上/下马 |
| Esc | 释放鼠标 |

## 当前状态

当前阶段：东岭关灰盒已由 Unity 批处理生成；运行时脚本已编译，截图工具已验证。
尚未完成：编辑器内完整试玩、macOS 玩家包构建、URP、动画、角色模型、骑战、叙事 UI。

## Agnir Project Instructions

For Project activation, read [AGNIR.md](AGNIR.md). This section is a backward-compatible locator only.
