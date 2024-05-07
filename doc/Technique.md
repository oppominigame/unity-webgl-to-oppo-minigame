# 技术原理

Unity 游戏是使用 C# 语言开发的游戏，而 OPPO 小游戏的运行环境却是使用 V8 引擎和 JS Framework 解析执行。从 C# 到 JS，我们采用的方案是使用 WebGL 作为桥梁进行跨端合并

- Unity 游戏提供了发布成 WebGL 项目的能力，详情见 [Unity官网介绍](https://docs.unity3d.com/cn/2020.3/Manual/webgl-technical-overview.html)
- OPPO 小游戏引擎采用了 WebGL1.0 规范，使用适配层模拟实现了 Unity 游戏发布 WebGL 项目后所依赖的 Web API
- 使用 OPPO 提供的 WebGL 配置方案，将 Unity 游戏原生 WebGL 项目打包成 OPPO 小游戏