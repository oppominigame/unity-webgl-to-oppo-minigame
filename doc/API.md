# Unity SDK API

Unity SDK 提供了许多与 OPPO 小游戏相关的功能和接口，在 C# 脚本中可以使用多种 API 来实现包括登录、支付、各类广告、数据存储读取删除等能力

## 登录

使用此接口可以让玩家使用 OPPO 账号登录游戏

```c#
QG.Login(
    (msg) => { Debug.Log("QG.Login success = " + JsonUtility.ToJson(msg)); },
    (msg) => { Debug.Log("QG.Login fail = " + msg.errMsg); }
);
```

## 支付

使用此接口可以让玩家在游戏中购买虚拟物品道具

```c#
 public void playQGPay()
    {
        //支付先登录拉数据
        QG.Login((msg) =>
          {
              Debug.Log("QG.Login success = " + JsonUtility.ToJson(msg));
              if (msg.data.token != string.Empty)
              {
                  PayOrder(msg.data.token);
              }
              else
              {
                  Debug.Log("The platform token fails to be obtained. Procedure");
              }
          },
          (msg) =>
          {
              Debug.Log("QG.Login fail = " + msg.errMsg);
          });
    }

    public void PayOrder(string parameterToken)
    {
        PayParam param =
                          new PayParam()
                         {
                             appId = 30173650,  //CP游戏在开放平台申请的appId
                             token = parameterToken,//QG.Login 传入
                             payUrl = "https://jits.open.oppomobile.com/jitsopen/api/pay/demo/preOrder",  //测试接口(必填)
                             //payUrl =  "https://jits.open.oppomobile.com/jitsopen/api/pay/v1.0/preOrder", //正式接口(必填)
                             productName = "测试礼包",//商品名称(必填)
                             productDesc = "测试支付",//商品说明(必填)
                             count = 1, //商品数量（只能传1）(必填)
                             price = 1, //商品价格，以分为单位(必填)
                             currency = "CNY", //币种，人民币如：CNY(必填)
                             callBackUrl = "", // 服务器接收平台返回数据的接口回调地址(可不填)
                             cpOrderId = "1.0", //CP自己的订单号(可不填)
                             appVersion = "1.0.0", //游戏版本(必填)
                             deviceInfo = "", //设备号(可不填)
                             ip = "", //终端IP(可不填)
                             attach = ""//附加信息(可不填)
                         };
        QG
            .Pay(param,
            (msg) =>
            {
                Debug.Log("QG.Pay success = " + JsonUtility.ToJson(msg));
            },
            (msg) =>
            {
                Debug.Log("QG.Pay fail = " + JsonUtility.ToJson(msg));
            });
    }

```

## 激励视频广告

使用此接口可以让玩家在游戏中打开激励视频广告

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

## 文件下载

使用此接口可以让玩家在游戏中下载文件

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

## 文件上传

使用此接口可以让玩家在游戏中上传文件

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

## 视频播放

使用此接口可以让玩家在游戏中播放远程视频

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

## 音频播放

使用此接口可以让玩家在游戏中播放远程音频

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

## 消息框

使用此接口可以让玩家在游戏中弹出提示框

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

## 数据储存

```c#
     QG.StorageSetItem(Key, Value); //存储
     QG.StorageGetItem(Key); //读取
     QG.StorageRemoveItem(Key); //删除
```

## 创建桌面图标

```c#
    QG.HasShortcutInstalled((msg) =>
    {
    Debug.Log("QG.HasShortcutInstalled success = " +JsonUtility.ToJson(msg));},
    (msg) =>
    {
    Debug.Log("QG.HasShortcutInstalled fail = " + msg.errMsg);
    });
```

## 自定义拓展

- 可参考 Unity 提供的 C# 调用 JS 方法的 [代码示例](https://docs.unity.cn/cn/2019.4/Manual/webgl-interactingwithbrowserscripting.html)
- 主要在 `Plugins/qg.minigame.jslib` 中编写 JS 代码以及在 QGMiniGameManager 中封装调用入口，可依据 [OPPO 小游戏文档](https://ie-activity-cn.heytapimage.com/static/minigame/CN/docs/index.html#/develop/feature/account) 自行添加能力
- 以登录功能举例

1. 先在 qg.minigame.jslib 文件中写入 oppo 小游戏登录 API（qg.login）

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

2. 在 QGMiniGameManager.cs 中注册

    ```c#
    #region 登录

            public void Login(Action<QGCommonResponse<QGLoginBean>> successCallback = null, Action<QGCommonResponse<QGLoginBean>> failCallback = null)
            {
                QGLogin(QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback));
            }

            #endregion
    ```

3. 在 QGModel.cs 写入 API 中的实体类（请求参和返回参）

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

4. 在 QG.cs 文件中写入调用

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

5. 引用 QGMiniGame 名空间并调用方法

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
