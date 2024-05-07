# 更新日志

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