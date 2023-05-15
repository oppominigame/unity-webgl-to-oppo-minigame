- [注意事项](#section0)
- [下载并安装插件](#section1)
- [OPPO小游戏C#SDK接口使用说明](#section2)
- [发布WebGL项目](#section3)
- [Addressable教程](#section4)
- [本地调试](#section5)

## 注意事项<a name="section0"></a>
**当前插件仅支持Windows用户使用！** Mac用户可使用[Unity-WebGL适配方案-命令行版](Unity适配方案-命令行版.md)

## 下载并安装插件<a name="section1"></a>
在当前Git项目的tools目录下有最新版本的插件包。

## OPPO小游戏C#SDK接口使用说明<a name="section2"></a>

开发Unity游戏项目使用C\#语言，开发OPPO小游戏使用JS语言，C\#不能直接调JS接口，需在jslib库中[OPPO小游戏C#SDK接口使用说明](第一步-OPPO小游戏C%23SDK接口使用说明.md)给C\#调用，就能使用OPPO小游戏丰富的能力，例如帐号、广告、支付等。

## 发布WebGL项目<a name="section3"></a>

OPPO小游戏引擎采用了WebGL1.0规范，您需将Unity游戏项目[发布WebGL项目](第二步-发布WebGL项目.md)。

## Addressable教程<a name="section4"></a>

1.  当游戏使用Addressable插件并导出WebGl时，在WebGl文件夹内除项目代码的Build文件夹及index.xml文件，会多出一个StreamingAssets文件夹。
2.  将StreamingAssets文件夹放到cdn即可。
3.  插件内勾选<color style = 'color:red'>使用Addressable</color>并输入cdn地址。例：http://localhost:8080/StreamingAssets


## 本地调试<a name="section5"></a>

Unity游戏成功转换成OPPO小游戏后，您可以先在本地调试小游戏的功能、性能。

1.  打开手机设备的“开发者模式”，使用USB连接测试设备与电脑。
2.  将小游戏RPK包体导入到测试设备中。
3.  在快应用调试器运行小游戏包，开始调试游戏。
详细可查看[技术文档- 真机调试](https://ie-activity-cn.heytapimage.com/static/minigame/CN/docs/index.html#/develop/games/debug)