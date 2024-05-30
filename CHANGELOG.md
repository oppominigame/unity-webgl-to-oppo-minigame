# 更新日志

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

