# 环境与工具链

## 已验证环境

| 工具 | 版本 / 路径 | 状态 |
| --- | --- | --- |
| Unity Editor | 2022.3.62f3c1（China Edition，Apple Silicon），`/Applications/Unity/Unity.app` | 可运行；批处理编译、灰盒生成、截图均通过 |
| Unity Hub | 3.3.7 | 可识别编辑器；启动后显示 Personal |
| Rosetta 2 | 系统安装 | 已安装；Unity Package Manager 随编辑器包为 x86_64，需要 Rosetta |
| Blender | 5.2.1 LTS，`~/Applications/Blender.app` | 可用，CLI 已链接到 `~/.local/bin/blender` |
| Node.js | v26.10.0 | 可用 |
| ffmpeg | 已安装 | 可用 |

原始 `/Applications/Blender.app` 被 Gatekeeper 隔离且属主非当前用户，无法原地修复；已复制到 `~/Applications/Blender.app` 并清除隔离属性，原件保留。

## Unity 安装与验证

Unity 2022.3.62f3c1 Apple Silicon 包曾被 Hub 下载校验失败。后由 macOS 官方安装器从 arm64 安装包安装，Hub 已识别 `/Applications/Unity/Unity.app`。主编辑器为 arm64；随包的 `UnityPackageManager` 是 x86_64，因此本机需要 Rosetta 2。已安装 Rosetta，Package Manager v6.1.5 可正常启动。

已在本机验证：

- `ProjectSetup.Setup` 批处理成功，0 个 C# 编译错误，并生成 `Assets/Data/Level_Dongling.asset`。
- `GrayboxBuilder.Build` 批处理成功，生成 `Assets/Scenes/Graybox_Dongling.unity` 并加入 Build Settings。
- `ScreenshotCapture.CaptureFromCommandLine` 成功生成 6 张 1920×1080 截图，输出在 `Screenshots/`（该目录被 Git 忽略）。
- 尚未验证：Unity 玩家包构建、完整交互试玩、URP 渲染。

授权诊断日志曾出现 Hub 常驻授权客户端 `Code 10` / IPC 协议版本不匹配提示；编辑器随后使用随编辑器提供的授权客户端继续运行，能创建工程并完成上述批处理。也出现 Unity 在线配置/遥测域名无法解析的网络日志。不要将这些日志误判为 C# 编译失败；在线服务是否全部可用未验证。

## 项目包与渲染

项目当前使用内置渲染管线生成灰盒。`Packages/manifest.json` 中的包已由 2022.3 编辑器解析并生成 `Packages/packages-lock.json`。URP 尚未加入项目；URP 14 在当前中国镜像的公共注册表中不可用，后续应先从可验证的官方来源获取，再切换材质和渲染管线。不要仅因为计划中写了 URP 就假定已配置。

## 常用命令

```bash
UNITY="/Applications/Unity/Unity.app/Contents/MacOS/Unity"

# 项目初始化与脚本编译验证
"$UNITY" -batchmode -quit -nographics -projectPath "$PWD" \
  -executeMethod SanguoLegend.EditorTools.ProjectSetup.Setup -logFile -

# 生成东岭关灰盒场景
"$UNITY" -batchmode -quit -nographics -projectPath "$PWD" \
  -executeMethod SanguoLegend.EditorTools.GrayboxBuilder.Build -logFile -

# 构建 macOS 包
./tools/build.sh

# 多机位截图（需要图形环境；不要加 -nographics）
./tools/shots.sh

# Blender 资产检查
blender -b -P tools/blender/inspect_glb.py -- path/to/model.glb
```

## 已知限制

- `UnityPackageManager` 是 Intel 辅助程序，Apple Silicon 主机需要 Rosetta 2。
- Unity Hub 3.3.7 与编辑器的常驻授权客户端出现协议/签名校验提示；编辑器内置客户端已成功执行当前批处理，但在线授权路径仍需后续观察。
- Unity 在线配置与遥测域名在检查时无法通过 DNS 解析；不影响已通过的本地编译和灰盒生成，但在线功能未验证。
- 当前没有配置 URP、动画、角色模型或成品资源；工程仍处于灰盒阶段。
- `~/.local/bin` 需在 PATH 中（`node` 与 Blender CLI 链接位于该目录）。
