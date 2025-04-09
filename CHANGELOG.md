# 更新日志

## 2025-4-9 【普通更新】
### Feature

- 普通：更新基础能力版本 Unity SDK，位于 `tools/unity_webgl_rpk_oppo_v8.0.7_open_ability.unitypackage`
- 普通：新增基础能力版本 Unity SDK 创建桌面接口 QG.InstallShortcut.
- 
## 2025-3-19 【普通更新】
### Feature

- 重要：Unity微信小游戏一键转OPPO小游戏文档说明. [详情](https://ie-activity-cn.heytapimage.com/static/minigame/CN/docs/index.html#/develop/games/wx-transfer)
- 重要：上线新版本 Unity SDK，版本号 V8.1.0，位于 `tools/unity_webgl_rpk_oppo_v8.1.0.unitypackage`
- 重要：新增 QG.OnShow,QG.OffShow,QG.OnHide,QG.OffHide 监听OPPO小游戏切换前台后台事件. 详情参考 [OPPO SDK API](doc/API.md#监听OPPO小游戏切换前台后台事件)
- 重要：新增 QG.OnAuthDialogShow,QG.OffAuthDialogShow,QG.OnAuthDialogClose,QG.OffAuthDialogClose 账号登录及实名认证监听. 详情参考 [OPPO SDK API](doc/API.md#监听账号登录及实名认证)
- 重要：适配 UnityEngine.PlayerPrefs. API: SetInt,GetInt,SetString,GetString,SetFloat,GetFloat,DeleteAll,DeleteKey,HasKey. 详情参考 [PlayerPrefs](doc/API.md#Unity持久化)

## 2025-1-9 【普通更新】
### Feature

- 普通：更新基础能力版本 Unity SDK，位于 `tools/unity_webgl_rpk_oppo_v8.0.7_open_ability.unitypackage`
- 普通：新增基础能力版本 Unity SDK 获取渠道接口.

## 2025-1-6 【重要更新】
### Feature

- 重要：上线新版本 Unity SDK，版本号 V8.0.9，位于 `tools/unity_webgl_rpk_oppo_v8.0.9.unitypackage`
- 重要：上线基础能力版本 Unity SDK，位于 `tools/unity_webgl_rpk_oppo_v8.0.7_open_ability.unitypackage`
- 普通：修改构建默认关闭nameFilesAsHashes.
- 普通：修改构建完成提示渲染版本和发布类型.

## 2024-12-18 【重要更新】
### Feature

- 重要：上线新版本 Unity SDK，版本号 V8.0.8，位于 `tools/unity_webgl_rpk_oppo_v8.0.8.unitypackage`
- 重要：新增 [使用WebGL2.0说明文档](doc/WebGL2.md) .
- 重要：新增 QG.IsStartupByShortcut 判断是否是桌面启动.详情参考 [OPPO SDK API](doc/API.md#判断是否是桌面启动)
- 重要：新增 录音能力.详情参考 [OPPO SDK API](doc/API.md#录音能力)
- 重要：新增 原生模板广告 互推盒子横幅广告 互推盒子抽屉广告 OnShow OffShow 回调.详情参考 [OPPO SDK API](doc/API.md#原生模板广告)
- 重要：修改 [资源缓存系统](doc/AssetCache.md) 使用指导，新增 缓存CDN路径, 修改 缓存路径标识.
- 重要：修改 视频能力参数补充.详情参考 [OPPO SDK API](doc/API.md#视频播放)

## 2024-11-14 【重要更新】
### Feature

- 重要：上线新版本 Unity SDK，版本号 V8.0.7，位于 `tools/unity_webgl_rpk_oppo_v8.0.7.unitypackage`

- 重要：修改 QG.ReadFile, QG.ReadFileSync 可读取6mb以上二进制文件.
- 重要：修改 QG.HasShortcutInstalled, QG.InstallShortcut 获取/创建桌面异常.详情参考 [OPPO SDK API](doc/API.md#判断是否已经创建桌面图标)


## 2024-11-07 【重要更新】
### Feature

- 重要：上线新版本 Unity SDK，版本号 V8.0.6，位于 `tools/unity_webgl_rpk_oppo_v8.0.6.unitypackage`

- 重要：修改 BuildEditorWindow.cs 编译异常.
- 重要：修改 开放WEBGL2.0设置.
- 重要：新增 画面旋转设置 "landscapeLeft" or "landscapeRight", OPPO 小游戏调试器 V9.2.0 有效.
- 重要：新增 FileSystemManager 适配.详情参考 [OPPO SDK API](doc/API.md#获取本地临时文件或本地用户文件的文件信息) 

## 2024-9-18 【重要更新】
### Feature

- 重要：上线新版本 Unity SDK，版本号 V8.0.5，位于 `tools/unity_webgl_rpk_oppo_v8.0.5.unitypackage`

- 重要：新增 [创建证书](doc/OpensslPlugin.md).
- 重要：新增 AR相机 Yuv格式接口.
- 重要：修改 解决EditorWindow.HasOpenInstances unity2018打包编译异常.

- 普通：新增 设备方向接口.

## 2024-8-6 【普通更新】
### Feature

- 重要：上线新版本 Unity SDK，版本号 V8.0.4，位于 `tools/unity_webgl_rpk_oppo_v8.0.4.unitypackage`

- 普通：修改unity2018,unity2019 js语法适配.
- 普通：修改激励视频广告示例,新增.load调用.

## 2024-7-23 【重要更新】
### Feature

- 重要：上线新版本 Unity SDK，版本号 V8.0.3，位于 `tools/unity_webgl_rpk_oppo_v8.0.3.unitypackage`

- 重要：新增 GetFileInfo 获取本地临时文件或本地用户文件的文件信息接口.
- 重要：修改 QG.Pay,由开发者创建订单.
- 普通：修改 创建Ar相机支持回调.



## 2024-7-3 【重要更新】
### Feature

- 重要：上线新版本 Unity SDK，版本号 V8.0.2，位于 `tools/unity_webgl_rpk_oppo_v8.0.2.unitypackage`

- 重要：修改 QG.Pay 正式订单链接, 新增 QG.PayTest 示例接口.
- 重要：新增 QG.PlayAudio 音频播放 SetVolume(音量),SetLoop(设置循环) 接口.
- 重要：修复shader真机检测材质路径判断异常.
- 重要：新增 消息框,对话框,进度条,定时器,云储存,系统信息,渠道信息,配置文件信息,修改渲染帧率,电量,亮度,获取设备ID,获取地理和速度,加速计,剪切板,罗盘 接口. 详情参考 [OPPO SDK API](doc/API.md)
- 重要：新增 示例 支付,云储存,系统信息,设备信息,提示框 场景.

## 2024-5-30 【普通更新】

### Fix
- 普通：更新打包工具版本至 `2.1.8-beta.1`，修复配置项 `bundleHashLength`，`defaultReleaseSize` 类型错误导致的缓存系统失效

## 2024-5-20 【重要更新】

### Feature
- 重要：上线新版本 Unity SDK，版本号 V8.0.1，位于 `tools/unity_webgl_rpk_oppo_v8.0.1.unitypackage`
- 重要：新增 `qg.exitApplication` 游戏退出接口

### Fix
- 普通：修改 `qg.pay` 默认支付订单地址payUrl, 开发者无需传参
- 普通：修复 `qg.showKeyboard` 等键盘接口报错
- 普通：修复广告创建后自动展示的问题，详情参考 [OPPO SDK API 激励视频广告](doc/API.md#激励视频广告)

## 2024-5-7 【重要更新】
### Feature

- 重要：上线新版本 Unity SDK，版本号 V8.0.0，位于 `tools/unity_webgl_rpk_oppo_v8.0.0.unitypackage`

    - 重要：新增 Shader 兼容性检测工具
    - 重要：新增 SDK 版本自检更新
    - 普通：优化界面展示，重新规划配置分区
    - 普通：C# 接口对齐，`QG.PlayVideo` 更改为 `QG.CreateVideo`

- 重要：文档重构，新增使用指导

    - 重要：新增 [资源缓存系统](doc/AssetCache.md) 使用指导
    - 重要：新增 [Shader 兼容性检测](doc/ShaderCompatibilityDetect.md) 工具使用指导
    - 重要：新增转换案例

### Fix

- 普通：修复 `qg.pay` 无支付回调的问题

