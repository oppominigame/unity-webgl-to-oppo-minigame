# 导出小游戏

## 环境配置

1. 安装 Node.js 环境，建议安装 14.x 以上稳定版本，[点击](https://nodejs.org/en) 前往 Node.js 官方网站

2. 安装小游戏打包工具，在控制台执行 `npm install -g @oppo-minigame/cli` 进行安装。安装完成后执行 `quickgame -V` 能正确显示版本号表示安装成功。Unity 支持 **2.1.6 及以上版本**，推荐使用最新正式版本，beta 版本包含新特性但可能不稳定

    > <span style="font-size:0.8em">若提示 quickgame 不是内部或外部命令，可重新打开控制台或重启计算机后重新执行命令</span>

## 导出方式

当前支持 2 种方式将 Unity 游戏导出转换为 OPPO 小游戏

- [Unity SDK](TransformBySDK.md)**「强烈推荐」**：该方案会在 Unity Editor 内安装小游戏导出 SDK，通过界面的形式进行一键配置和导出，操作简单好用

- [命令行](TransformByCLI.md)「新手不推荐」：该方案需要根据指引先将 Unity 游戏导出到 WebGL 平台，再运行命令行完成小游戏导出，适合从 Unity 外部修改后进行重复导出