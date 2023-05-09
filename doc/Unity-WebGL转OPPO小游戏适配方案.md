# Unity WebGL OPPO小游戏适配方案

-   [方案特点](#section2023_0506_001_001)
-   [工作原理](#section2023_0506_001_002)
-   [准备工作](#section2023_0506_001_004)
-   [方案选择](#section2023_0506_001_005)
-   [技术支持](#section2023_0506_001_006)

欢迎使用 Unity WebGL OPPO小游戏适配(转换)方案，本方案设计的目的是降低游戏转换的开发成本，即您无需更换Unity引擎、无需重写核心代码的情况下将原有的Unity游戏项目转换到OPPO小游戏。

## 方案特点<a name="section2023_0506_001_001"></a>

-   对比H5小游戏方案，性能有明显提升。
-   保持原引擎工具链与技术栈
-   无需重写核心代码，降低转换成本。
-   由转换工具可视化，方便开发者进行发布项配置，快速打包小游戏RPK包
-   OPPO小游戏平台能力以C# SDK方式提供给开发者，快速对接平台开放能力

## 工作原理<a name="section2023_0506_001_002"></a>

Unity游戏是使用C\#语言开发的游戏，而OPPO小游戏的运行环境却是使用V8引擎和JS Framework解析执行。从c\#到js，我们采用的方案是使用WebGL作为桥梁进行跨端合并。

-   Unity游戏提供了发布成WebGL项目的能力，详情见[Unity官网介绍](https://docs.unity3d.com/cn/2020.3/Manual/webgl-technical-overview.html)。
-   OPPO小游戏引擎采用了WebGL1.0规范，使用adapter层模拟实现了Unity游戏发布WebGL项目后所依赖的WEB API。
-   使用OPPO提供的WebGL配置方案，将Unity游戏原生WebGL项目打包成OPPO小游戏。


## 准备工作<a name="section2023_0506_001_004"></a>

### 1. 环境准备
* 安装了node环境，建议安装14.x以上稳定版本 [node官⽹网：https://nodejs.org/en/]


* 游戏包体 rpk 限制在**30M**以内，并且unity游戏不支持小游戏分包能力。用户本地文件大小限制在 **500M**以内


-   您已[下载并安装Unity](https://unity.cn/releases/lts/2021)，请按实际情况选择Unity版本号：
* 当前支持版本如下：

    | **Unity版本** | **推荐版本**      | **下载链接**      |
    | :------      | :-----           | :-----           |
    | 2018.4.x     | 2018.4.30f1      |https://unity.com/releases/editor/whats-new/2018.4.30|
    | 2019.4.x     | 2019.4.35f1      |https://unity.com/releases/editor/whats-new/2019.4.35|
    | 2020.3.x     | 2020.3.47f1      |https://unity.com/releases/editor/whats-new/2020.3.47|
    | 2019.3.x     | 2021.3.14f1      |https://unity.com/releases/editor/whats-new/2021.3.14|



### 2. 安装打包工具

1. 安装 OPPO 小游戏打包工具 [@oppo-minigame/cli](https://www.npmjs.com/package/@oppo-minigame/cli)

```
npm i -g @oppo-minigame/cli@2.1.6-beta.10
```

2. 运行 `quickgame -V` 能够正确显示版本号表示安装成功

**注：若提示 quickgame 不是内部或外部命令，可重新打开命令提示符窗口或者重启计算机后再运行 quickgame -V**

> Unity ⽀持的打包工具是 2.1.6-beta.10 版本

### 3. 安装调试器

1. 当前unity适配能力在内测阶段，所以安装压缩包中的 v_6.8.0_beta.apk 包到 OPPO 手机上才能力进行调试（调试包可向oppo小游戏的商务人员索取）

2. 如果安装成功，手机桌面有一个“快应用”图标出现

### 4. 素材与签名准备
-   您已提前准备如下素材内容。

    <a name="table17287102162315"></a>
    <table><thead align="left"><tr id="row728814212234"><th class="cellrowborder" valign="top" width="25.94%" id="mcps1.1.3.1.1"><p id="p328819213234"><a name="p328819213234"></a><a name="p328819213234"></a>准备项</p>
    </th>
    <th class="cellrowborder" valign="top" width="74.06%" id="mcps1.1.3.1.2"><p id="p1128815222320"><a name="p1128815222320"></a><a name="p1128815222320"></a>说明</p>
    </th>
    </tr>
    </thead>
    <tbody><tr id="row132883252311"><td class="cellrowborder" valign="top" width="25.94%" headers="mcps1.1.3.1.1 "><p id="p610175162913"><a name="p610175162913"></a><a name="p610175162913"></a>原始代码包</p>
    </td>
    <td class="cellrowborder" valign="top" width="74.06%" headers="mcps1.1.3.1.2 "><p id="p2037810019214"><a name="p2037810019214"></a><a name="p2037810019214"></a>将在Unity中导出成WebGL项目。建议较大的Unity游戏项目提前进行<a href="https://docs.unity3d.com/cn/2020.3/Manual/AssetBundles-Workflow.html" target="_blank" rel="noopener noreferrer">分包处理</a>。</p>
    </td>
    </tr>
    <tr id="row32886232317"><td class="cellrowborder" valign="top" width="25.94%" headers="mcps1.1.3.1.1 "><p id="p92880222310"><a name="p92880222310"></a><a name="p92880222310"></a>签名文件</p>
    </td>
    <td class="cellrowborder" valign="top" width="74.06%" headers="mcps1.1.3.1.2 "><p id="p10288824237"><a name="p10288824237"></a><a name="p10288824237"></a>操作详情可参见<a href="https://ie-activity-cn.heytapimage.com/static/minigame/CN/docs/index.html#/develop/games/quickgame" target="_blank" rel="noopener noreferrer">生成签名文件</a>。</p>
    </td>
    </tr>
    <tr id="row32884212313"><td class="cellrowborder" valign="top" width="25.94%" headers="mcps1.1.3.1.1 "><p id="p0288162112312"><a name="p0288162112312"></a><a name="p0288162112312"></a>小游戏图标</p>
    </td>
    <td class="cellrowborder" valign="top" width="74.06%" headers="mcps1.1.3.1.2 "><a name="ul1445873435215"></a><a name="ul1445873435215"></a>要求与平台上传图标一致。尺寸：256*256 px，图片格式：PNG，大小500kb以内。
    </td>
    </tr>
    </tbody>
    </table>

## 方案选择<a name="section2023_0506_001_005"></a>
#### 当前两种适配方案
1. [Unity-WebGL适配方案-Unity插件版](Unity适配方案-Unity插件版.md) **（强烈推荐）**

>该方案会在Unity编辑器内安装OPPO小游戏发布插件和C# SDK,发布和适配流程完全在Unity中完成，操作简单好用，强烈推荐。

2. [Unity-WebGL适配方案-命令行版](Unity适配方案-命令行版.md)

>该方案适合将发布流程工程化的项目，操作略复杂，新手不推荐


## 技术支持<a name="section2023_0506_001_006"></a>

1.  查看[常见问题以及解决思路](常见问题以及解决思路.md)。
2.  通过[OPPO小游戏官方技术QQ群](https://ie-activity-cn.heytapimage.com/static/minigame/CN/docs/index.html#/introduce/know/linkman)查找问题解决方案。
3.  登录[OPPO小游戏开发者论坛](https://open.oppomobile.com/bbs/geek.php?mod=develop&order=lastpost&page=1&limit=10&fid=21&page=1)进行留言。
4. 通过[官方技术文档](https://ie-activity-cn.heytapimage.com/static/minigame/CN/docs/index.html#/)进行技术问题反馈
