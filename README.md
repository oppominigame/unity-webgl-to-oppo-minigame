# Unity WebGL OPPO 小游戏适配方案

欢迎使用 Unity WebGL OPPO 小游戏适配（转换）方案，本方案设计的目的是降低游戏转换的开发成本，即您无需更换 Unity 引擎，无需重写核心代码的情况下将原有的 Unity 游戏项目转换到 OPPO 小游戏

## 方案特点

- 对比 H5 小游戏方案，性能有明显提升
- 保持原引擎工具链与技术栈
- 无需重写核心代码，降低转换成本
- 转换工具可视化，方便开发者进行发布项配置，快速打包小游戏 RPK 包
- OPPO 小游戏平台能力以 C# SDK 方式提供给开发者，快速对接平台开放能力

## 转换案例

| 小小蚁国 | 地铁跑酷 | 大圣顶住 | 巨兽战场 | 
| --- | --- | --- | --- |
| <img src='doc/image/小小蚁国.png' width='240' alt="待补充"/> | <img src='doc/image/地铁跑酷.png' width='240' alt="待补充"/> |  <img src='doc/image/大圣顶住.png' width='240' alt="待补充"/>| <img src='doc/image/巨兽战场.png' width='240' alt="待补充"/> |

## 目录导览

### 快速开始

- [导出小游戏](doc/Transform.md)
  - [使用 SDK 导出](doc/TransformBySDK.md)
  - [使用命令行导出](doc/TransformByCLI.md)
- [运行小游戏](doc/RunQuickGame.md)
- [发布小游戏](doc/PublishQuickGame.md)

### 基本

- [Unity SDK API](doc/API.md)
- [更新日志](CHANGELOG.md)

### 方案与兼容性

- [技术原理](doc/Technique.md)
- [推荐引擎版本](doc/UnityVersion.md)
- [能力支持](doc/CapabilitySupport.md)
- [Shader 兼容性检测](doc/ShaderCompatibilityDetect.md)

### 版本管理

- [SDK 自检更新](doc/SDKUpdate.md)

### 性能优化

- [资源缓存](doc/AssetCache.md)

### 其他

- [示例工程](doc/Demo.md)
- [技术常见问题 QA](doc/DevelopmentQA.md)
- [问题反馈与联系我们](doc/IssueAndContact.md)
