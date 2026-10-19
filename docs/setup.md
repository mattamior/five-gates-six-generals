# 环境与工具链

## 已安装

| 工具 | 版本 / 路径 | 状态 |
| --- | --- | --- |
| Unity Editor | 2022.3.62f3c1（China Edition，Apple Silicon） | 安装中，见下 |
| Blender | 5.2.1 LTS，`~/Applications/Blender.app` | 可用，CLI 已链接到 `~/.local/bin/blender` |
| Node.js | v26.10.0 | 可用 |
| ffmpeg | 已装 | 可用 |

原始 `/Applications/Blender.app` 被 Gatekeeper 隔离且属主非当前用户，无法原地修复，已复制到 `~/Applications/Blender.app` 并清除隔离属性。

## Unity 说明

当前网络访问 Unity 国际 CDN 会被重定向到中国镜像，而中国镜像没有 Unity 6 的编辑器包（只提供 2020/2021/2022 的 c1 版本）。因此先安装 **2022.3.62f3c1**：

- 功能上满足本项目：URP 14、Cinemachine 2.x、Timeline、Animation Rigging、Addressables、Input System 均可用。
- 后续拿到 Unity 6 安装包后，可直接升级工程（Unity 支持向前迁移）。

安装包：`/tmp/Unity-2022.3.62f3c1.pkg`（4.5 GB）。

手动安装（免 sudo）：

```bash
pkgutil --expand-full /tmp/Unity-2022.3.62f3c1.pkg /tmp/unity-expand
# 找到 Unity.app 后复制到：
mkdir -p /Applications/Unity/2022.3.62f3c1
cp -R /tmp/unity-expand/.../Unity.app /Applications/Unity/2022.3.62f3c1/
```

## 授权

Unity 需要登录授权后才能以 batchmode 运行。当前 Hub 3.3.7 为国内版本，CLI 参数不兼容。

首次打开编辑器时按提示登录 Unity ID 并激活 Personal 许可证；许可证写入 `~/Library/Application Support/Unity/`。

## 常用命令

```bash
# 生成灰盒场景（需要已授权）
/Applications/Unity/2022.3.62f3c1/Unity.app/Contents/MacOS/Unity \
  -batchmode -quit -projectPath "$PWD" \
  -executeMethod SanguoLegend.EditorTools.GrayboxBuilder.Build \
  -logFile -

# 构建 macOS 包
./tools/build.sh

# 多机位截图（视觉审查用）
./tools/shots.sh

# Blender 资产检查
blender -b -P tools/blender/inspect_glb.py -- path/to/model.glb
```

## 已知限制

- Unity 包注册表（packages.unity.com）可访问；URP 14 不在公共注册表，需从编辑器自带缓存或 URP 模板获取。
- 中国镜像不提供 Unity 6 安装包，升级需另找渠道。
- `~/.local/bin` 需在 PATH 中（`node` 已在该目录，通常已配置）。
