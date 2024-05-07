# 发布小游戏

## 打正式包

正式发布前，需要使用正式签名文件进行打包，按照以下步骤依次进行

1. **生成签名**：通过 [OPPO 小游戏开发者工具](https://ie-activity-cn.heytapimage.com/static/minigame/CN/docs/index.html#/develop/games/ide?id=_5%e3%80%81%e7%94%9f%e6%88%90%e6%b8%b8%e6%88%8f%e7%ad%be%e5%90%8d)或 [OpenSSL](https://ie-activity-cn.heytapimage.com/static/minigame/CN/docs/index.html#/develop/games/quickgame?id=_53-%e5%85%b6%e4%bb%96%e7%94%9f%e6%88%90-release-%e7%ad%be%e5%90%8d%e6%96%b9%e5%bc%8f) 生成正式签名，执行完成后将生成**私钥文件** `private.pem` 和**证书文件** `certificate.pem`

2. **正式打包**：将 `private.pem` 和 `certificate.pem` 放到 `小游戏根目录/sign/release` 目录下，再于小游戏根目录执行 `quickgame pack release` 命令完成正式打包

> RPK 包体积限制在 30M 以内，若超过此值将无法通过上架审核

## 上架审核

详细步骤参照 [官方文档](https://ie-activity-cn.heytapimage.com/static/minigame/CN/docs/index.html#/introduce/know/know) 进行