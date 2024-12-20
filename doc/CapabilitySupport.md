# 能力支持

| 能力 | 支持性 | 描述 |
| --- | --- | --- |
| Unity 基础模块 | 支持 | 支持动画、物理、渲染、音频、UI等基础模块 |
| Unity 渲染管线 | 部分支持 | 支持标准渲染管线，URP 未完全支持 |
| Unity 资源加载 | 支持 | 支持 AssetBundle、Addressables |
| Lua 脚本 | 不支持 | 小游戏暂不允许脚本更新能力 |
| PureTS | 不支持 | 小游戏暂不允许脚本更新能力 |
| 第三方插件 | 部分支持 | 支持大部分插件，C# 插件与非平台相关的 C 原生插件 |
| 分包 | 部分支持 | 不支持 wasm 分包，js 分包详见官方文档 [分包能力](https://ie-activity-cn.heytapimage.com/static/minigame/CN/docs/index.html#/develop/subpackage/subpackage) |
| 渲染接口 | 支持 | WebGL1.0 完全支持，WebGL2.0 部分支持[详情](WebGL2.md)  |
| 多线程 | 不支持 | 不支持 C# 多线程相关 API，使用异步等其他方式代替 |
| 文件系统 | 需调整 | 不支持 C# 文件相关 API，需要使用 [OPPO SDK API](API.md)，用户本地存储空间限制为 500M |
| 网络访问 | 支持 | 推荐使用 UnityWebRequest |