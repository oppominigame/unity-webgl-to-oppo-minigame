# OPPO 小游戏 C#SDK 接口使用说明

## 1. 注册并下载插件

在[OPPO 开发者平台](https://open.oppomobile.com/new/introduction?page_name=h5game)注册并创建您的小游戏应用后，您需要下载 oppo 小游戏 unity 插件。

## 2. 使用 SDK 接口

OPPO 小游戏 C# SDK，它提供了许多与 OPPO 小游戏相关的功能和接口，

在您的 C#脚本中，您可以使用 OPPO 小游戏 C# SDK 提供的接口来实现包括登录、支付、各类广告、数据存储读取删除等能力。使用此插件可以帮助您更快地开发 OPPO 小游戏，并且提高游戏的用户体验。以下是一些常用的接口：

### 2.1 登录接口

使用此接口可以让玩家使用 OPPO 账号登录游戏。以下是示例代码：

```c#
QG.Login(
    (msg) => { Debug.Log("QG.Login success = " + JsonUtility.ToJson(msg)); },
    (msg) => { Debug.Log("QG.Login fail = " + msg.errMsg); }
);
```

### 2.2 支付接口

使用此接口可以让玩家在游戏中购买虚拟物品道具。以下是示例代码：

```c#
PayParam param = new PayParam()
        {
            appId = "123",
            token = "xxxxxxxxxxxxxxxxxxxx",
            timestamp = 1682244531643,
            orderNo = "1",
            paySign = "xxxxxxxxxxxxxxxxxxxx",
            // paySign 由 CP 服务端使用 appKey (不是 appId )、orderNo、timestamp 进行签名算法生成返回
        };
        QG.Pay(
            param,
            (msg) => { Debug.Log("QG.Pay success = " + JsonUtility.ToJson(msg)); },
            (msg) => { Debug.Log("QG.Pay fail = " + JsonUtility.ToJson(msg)); },
            (msg) => { Debug.Log("QG.Pay complete = " + JsonUtility.ToJson(msg)); }
        );
```

### 2.3 激励视频广告接口

使用此接口可以让玩家在游戏中打开激励视频广告。以下是示例代码：

```c#
var rewardedVideoAd = QG.CreateRewardedVideoAd(new QGCommonAdParam()
          {
          adUnitId = "114183"
          });
          Debug.Log("创建激励视频开始运行");
          rewardedVideoAd.OnLoad(() => {
            Debug.Log("激励视频广告加载成功");
          rewardedVideoAd.Show();
          });
          rewardedVideoAd.OnError((QGBaseResponse msg) =>
          {
          Debug.Log("QG.rewardedVideoAd.OnError success = " + JsonUtility.ToJson(msg));
          });
          rewardedVideoAd.OnClose((QGRewardedVideoResponse msg) =>
          {
          if (msg.isEnded) {
                Debug.Log("激励视频广告完成，发放奖励");
              } else {
                Debug.Log("激励视频广告取消关闭，不发放奖励");
              }
         });
```

### 2.4 文件下载接口

使用此接口可以让玩家在游戏中下载文件。以下是示例代码：

```c#
 QG.DownLoadFile(new DownLoadFileParam()
        {
            path = "/test.png",         //path:下载替换路径 qgfile://usr/path,  path 可为空
            url = "https://openfs.oppomobile.com/open/res/201907/31/5f27f86a3cc84b02a8baaf0a4a3066ab.png",  //url:下载路径
        },
        (msg) =>
        {
            QG.ShowToast(new ShowToastParam()
            {
                title = "下载成功",
                iconType = "success",
                durationTime = 2000,
            });
        },
        (msg) =>
        {
            QG.ShowToast(new ShowToastParam()
            {
                title = "下载失败" + msg.errMsg,
                iconType = "error",
                durationTime = 2000,
            });
 });
```

### 2.5 文件上传接口

使用此接口可以让玩家在游戏中上传文件。以下是示例代码：

```c#
 QG.UploadFile(new UploadFileParam()
        {
            path = "/test.png",         //path:上传路径 qgfile://usr/path,  path 不可为空
            url = "http://example.com/resource",    //url:上传路径
            name = "test",                           //name:上传名
        },
        (msg) =>
        {
            QG.ShowToast(new ShowToastParam()
            {
                title = "上传成功",
                iconType = "success",
                durationTime = 2000,
            });
        },
        (msg) =>
        {
            QG.ShowToast(new ShowToastParam()
            {
                title = "上传失败" + msg.errMsg,
                iconType = "error",
                durationTime = 2000,
            });
  });

```

### 2.6 视频播放接口

使用此接口可以让玩家在游戏中播放远程视频。以下是示例代码：

```c#
    QGVideoPlayer obj = QG.PlayVideo(new VideoParam()
    {
        ParamX = 0,                 //屏幕 X 轴位置
        ParamY = 500,               //屏幕 Y 轴位置
        ParamWidth = 1000,          //视频宽
        ParamHeight = 800,          //视频长
        url = "http://vjs.zencdn.net/v/oceans.mp4",  //视频源
        poster = "/poster.jpg",                //视频封面 qgfile://usr/poster
    });
    obj.Destroy(); //销毁
```

### 2.7 音频播放接口

使用此接口可以让玩家在游戏中播放远程音频。以下是示例代码：

```c#
  qGAudioPlayer = QG.PlayAudio(new AudioParam()
        {
            url = "https://activity-cdo.heytapimage.com/cdo-activity/static/minigame/test/demo/music/huxia-4M.mp3", //播放链接
            startTime = 1.5f,  //起始播放时间
            loop = true,  //设置循环
            volume = 0.75f  //设置音量
        });

        qGAudioPlayer
            .OnPlay(() =>
            {
                Debug
                    .Log("监听音频播放成功");
  });
// 新增视频音频通用监听:OnPlay,OnCanPlay,OnPause,OnStop,OnEnded,OnTimeUpdate,OnError,OnWaiting,OnSeeking,OnSeeked.
// 新增视频音频通用方法:Play(),Pause(),Stop(),Seek(Time),Destroy.
```

### 2.8 消息框接口

使用此接口可以让玩家在游戏中弹出提示框。以下是示例代码：

```c#
    // success  显示成功图标，此时 title 文本最多显示 7 个汉字长度
    // error    显示失败图标，此时 title 文本最多显示 7 个汉字长度
    // loading  显示加载图标，此时 title 文本最多显示 7 个汉字长度
    // none     不显示图标，此时 title 文本最多可显示两行
    QG.ShowToast(new ShowToastParam()
      {
      title = "下载成功",           //title：标题
      iconType = "success",
      durationTime = 2000,         //延迟时间
      });
```

## 3. 自定义拓展

可参考 Unity 提供的 C#调用 JS 方法的代码示例 https://docs.unity.cn/cn/2019.4/Manual/webgl-interactingwithbrowserscripting.html

主要在 Plugins 中的（qg.minigame.jslib）插件编写 JS 代码及在 QGMiniGameManager 封装调用入口，可依据[OPPO 小游戏文档](https://ie-activity-cn.heytapimage.com/static/minigame/CN/docs/index.html#/develop/feature/account)自行添加能力

以登录功能举例：

（1）先在 qg.minigame.jslib 文件中写入 oppo 小游戏登录 API（qg.login）

```js
QGLogin: function (success, fail) {
    if (typeof qg == "undefined") {
      console.log("qg.minigame.jslib  qg is undefined");
      return;
    }

    var successID = UTF8ToString(success);
    var failID = UTF8ToString(fail);

    qg.login({
      success: function (res) {
        var json = JSON.stringify({
          callbackId: successID,
          data: res.data,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "LoginResponseCallback",
          json
        );
      },
      fail: function (res) {
        var json = JSON.stringify({
          callbackId: failID,
          errMsg: res.errMsg,
          errCode: res.errCode,
        });
        unityInstance.SendMessage(
          CONSTANT.ACTION_CALL_BACK_CLASS_NAME_DEFAULT,
          "LoginResponseCallback",
          json
        );
      },
    });
  },
```

（2）在 QGMiniGameManager.cs 中注册

```c#
#region 登录

        public void Login(Action<QGCommonResponse<QGLoginBean>> successCallback = null, Action<QGCommonResponse<QGLoginBean>> failCallback = null)
        {
            QGLogin(QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback));
        }

        #endregion
```

（3）在 QGModel.cs 写入 API 中的实体类（请求参和返回参）

```c#
[Serializable]
    public class QGLoginBean
    {
        public string avatar;
        public string sex;
        public string age;
        public string token; //调用接口获取登录凭证（token）。通过凭证进而换取用户登录态信息，包括用户的唯一标识（openid）
        public string nickName;
        public string uid;
        public string time;
        public string code;
        public string phoneNum;
    }
```

（4）在 QG.cs 文件中写入调用

```js
#region Login  登录
        // https://ie-activity-cn.heytapimage.com/static/minigame/CN/docs/index.html#/develop/feature/account
        //QG.Login(
        //(msg) => { Debug.Log("QG.Login success = " + JsonUtility.ToJson(msg)); },
        //(msg) => { Debug.Log("QG.Login fail = " + msg.errMsg); }
        ///);
        public static void Login(Action<QGCommonResponse<QGLoginBean>> successCallback = null, Action<QGCommonResponse<QGLoginBean>> failCallback = null)
        {
            QGMiniGameManager.Instance.Login(successCallback, failCallback);
        }
        #endregion
```

（5）如何调用 API 能力

新建一个 c#文件,引入 using QGMiniGame;

```c#
 // 1、登录
        QG
            .Login((msg) =>
            {
                Debug.Log("QG.Login success = " + JsonUtility.ToJson(msg));
            },
            (msg) =>
            {
                Debug.Log("QG.Login fail = " + msg.errMsg);
            });
```

## 4. 插件目录详细介绍

<img src="./imgs/image5.png" />

```
OPPO-GAME-SDK/                                          SDK源码文件夹
├── Plugins/
│   ├── qg.minigame.jslib                               JS插件，用于处理C#调用的方法以及callBack通讯
├── QG.cs                                               调用代码入口
├── QGModel.cs                                          API中的实体类（请求参和返回参）
├── QGMiniGameManager.cs                                C#与JS通讯的桥接代码
├── QGCallBackManager.cs                                用于处理JS相关的回调，JS对象映射到C#对象
└── CHANGELOG                                           版本日志查看
```
