# Unity SDK API

Unity SDK 提供了许多与 OPPO 小游戏相关的功能和接口，在 C# 脚本中可以使用多种 API 来实现包括登录、支付、各类广告、数据存储读取删除等能力

## <a id="登录"></a>登录

使用此接口可以让玩家使用 OPPO 账号登录游戏

```c#
QG.Login(
    (msg) => { Debug.Log("QG.Login success = " + JsonUtility.ToJson(msg)); },
    (msg) => { Debug.Log("QG.Login fail = " + msg.errMsg); }
);
```

## <a id="支付"></a>支付

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
                  PayOrder(msg.data.token); //正式接口，参数详见 https://ie-activity-cn.heytapimage.com/static/minigame/CN/docs/index.html#/develop/pay/order
                  //PayOrderTest(msg.data.token); OPPO示例，非正式接口，仅供参考
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

    //OPPO示例demo,非正式订单接口,仅供参考.
    public void PayOrderTest(string parameterToken)
    {
        PayTestParam param =
                          new PayTestParam()
                         {
                             appId = 30173650,  //CP游戏在开放平台申请的appId
                             openId = parameterToken,//QG.Login 传入
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
            .PayTest(param,
            (msg) =>
            {
                Debug.Log("QG.Pay success = " + JsonUtility.ToJson(msg));
            },
            (msg) =>
            {
                Debug.Log("QG.Pay fail = " + JsonUtility.ToJson(msg));
            });
    }

    //正式订单接口,该统一下单接口 https://jits.open.oppomobile.com/jitsopen/api/pay/v1.0/preOrder 返回 orderNo 预付订单号
    public void PayOrder(string parameterToken)
    {
        PayParam param =
                          new PayParam()
                         {
                             appId = 0,        //平台分配的游戏 appId
                             openId = "",      //qg.login 成功时获得的用户 token
                             timestamp = 0,    //时间戳，当前计算机时间和GMT时间(格林威治时间)1970年1月1号0时0分0秒所差的毫秒数
                             orderNo = "", //下单生成的预付订单号
                             paySign = ""  //支付签名，CP 服务端生成

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

## <a id="banner广告"></a>banner广告

使用此接口可以让玩家在游戏中打开banner广告

```c#
        qGBannerAd = QG.CreateBannerAd(new QGCreateBannerAdParam(){ adUnitId = inputAdUnitId });
        Debug.Log("创建Banner广告开始运行");
        qGBannerAd.OnLoad(() =>
            {
                Debug.Log("banner加载成功");
            });
        qGBannerAd.OnError((QGBaseResponse msg) =>
            {
                Debug.Log("QG.bannerAd.OnError success = " + JsonUtility.ToJson(msg));
            });
        qGBannerAd.OnHide(() =>
            {
                Debug.Log("隐藏成功");
            });
        qGBannerAd.OnClose((QGBaseResponse msg) =>
            {
                Debug.Log("banner关闭回调 = " + JsonUtility.ToJson(msg));
            });
```

## <a id="激励视频广告"></a>激励视频广告

使用此接口可以让玩家在游戏中打开激励视频广告

```c#
var rewardedVideoAd = QG.CreateRewardedVideoAd(new QGCommonAdParam()
          {
          adUnitId = "114183"
          });
          Debug.Log("创建激励视频开始运行");
          rewardedVideoAd.Load();
          rewardedVideoAd.OnLoad(() => {
            Debug.Log("激励视频广告加载成功");
          rewardedVideoAd.Show();
          });
          rewardedVideoAd.OnError((QGBaseResponse msg) =>
          {
          Debug.Log("rewardedVideoAd.OnError = " + JsonUtility.ToJson(msg));
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

## <a id="插屏广告"></a>插屏广告

使用此接口可以让玩家在游戏中打开插屏广告

```c#
var qGInterstitialAd = QG.CreateInterstitialAd(new QGCommonAdParam()
          {
          adUnitId = "114187"
          });
          Debug.Log("创建插屏广告开始运行");
          qGInterstitialAd.OnLoad(() => {
          Debug.Log("插屏广告加载成功");
          qGInterstitialAd.Show();
          });
          qGInterstitialAd.OnError((QGBaseResponse msg) =>
          {
          Debug.Log("qGInterstitialAd.OnError = " + JsonUtility.ToJson(msg));
          });
          qGInterstitialAd.OnClose((QGBaseResponse msg) =>
          {
          Debug.Log("插屏广告关闭回调 = " + JsonUtility.ToJson(msg));
          });
         qGInterstitialAd.Load();
```

## <a id="原生模板广告"></a>原生模板广告

使用此接口可以让玩家在游戏中打开原生模板广告

```c#
var qGCustomAd = QG.CreateCustomAd(new QGCreateCustomAdParam()
          {
          adUnitId = "1193999"
          });
        Debug.Log("创建原生模板广告开始运行");
        qGCustomAd.OnLoad(() =>{Debug.Log("原生模板广告加载成功");});
        qGCustomAd.OnError((QGBaseResponse msg) => {Debug.Log("qGCustomAd.OnError = " +JsonUtility.ToJson(msg));});
        qGCustomAd.OnHide(() => {Debug.Log("隐藏原生广告");});
        qGCustomAd.OnShow(() => {Debug.Log("原生模板广告展示回调成功");});
        qGCustomAd.Show((msg) =>
          {
            Debug.Log("原生模板广告展示成功 = " + JsonUtility.ToJson(msg));
          },
          (msg) =>
          {
            Debug.Log("原生模板广告展示失败 = " + msg.errMsg);
          });
        qGCustomAd.OnClose((QGBaseResponse msg) =>
        {
            Debug.Log("原生模板广告关闭回调 = " + JsonUtility.ToJson(msg));
        });
```

## <a id="互推盒子横幅广告"></a>互推盒子横幅广告

使用此接口可以让玩家在游戏中打开互推盒子横幅广告

```c#
var qGGameBannerAd = QG.CreateGameBannerAd(new QGCommonAdParam()
            { 
                adUnitId = "201139" 
            });
            Debug.Log("创建互推盒子横幅广告开始运行");
            qGGameBannerAd.OnLoad(() => { Debug.Log("互推盒子横幅广告加载成功");});
            qGGameBannerAd.OnError((QGBaseResponse msg) => { Debug.Log("qGGameBannerAd.OnError = " + JsonUtility.ToJson(msg));});
            qGGameBannerAd.OnShow(() => { Debug.Log("互推盒子横幅广告展示回调成功");});    
            qGGameBannerAd.Show((msg) =>
            {
             Debug.Log("互推盒子横幅广告展示成功 = " + JsonUtility.ToJson(msg));
            },
            (msg) =>
            {
            Debug.Log("互推盒子横幅广告展示失败 = " + msg.errMsg);
            });
            qGGameBannerAd.OnClose((QGBaseResponse msg) =>
            {
            Debug.Log("互推盒子横幅广告关闭回调 = " + JsonUtility.ToJson(msg));
            });
```

## <a id="互推盒子九宫格广告"></a>互推盒子九宫格广告

使用此接口可以让玩家在游戏中打开互推盒子九宫格广告

```c#
var = qGGamePortalAd = QG.CreateGamePortalAd(new QGCommonAdParam()
                         { adUnitId = "201138" });
            Debug.Log("创建互推盒子九宫格广告开始运行");
            qGGamePortalAd.OnLoad(() =>
                {
                    Debug.Log("互推盒子九宫格广告加载成功");
                    qGGamePortalAd.Show();
                });
            qGGamePortalAd.OnError((QGBaseResponse msg) =>
                {
                    Debug.Log("qGGamePortalAd.OnError = " + JsonUtility.ToJson(msg));
                });
            qGGamePortalAd.Load();
            qGGamePortalAd.OnClose((QGBaseResponse msg) =>
                {
                    Debug.Log("互推盒子九宫格广告关闭回调 = " + JsonUtility.ToJson(msg));
                });
```

## <a id="互推盒子抽屉广告"></a>互推盒子抽屉广告

使用此接口可以让玩家在游戏中打开互推盒子抽屉广告

```c#
var = qGGameDrawerAd =QG.CreateGameDrawerAd(new QGCreateGameDrawerAdParam()
                    { adUnitId = "336614" });
            Debug.Log("创建互推盒子抽屉广告开始运行");
          qGGameDrawerAd.OnLoad(() =>
           {
               Debug.Log("互推盒子抽屉广告加载成功");
               qGGameDrawerAd
              .Show((msg) =>
              {
                  Debug.Log("互推盒子抽屉广告展示成功 = " + JsonUtility.ToJson(msg));
              },
              (msg) =>
              {
                  Debug.Log("互推盒子抽屉广告展示失败 = " + msg.errMsg);
              });
           });
            qGGameDrawerAd.OnError((QGBaseResponse msg) =>
                {
                    Debug.Log("qGGameDrawerAd.OnError = " + JsonUtility.ToJson(msg));
                });
            qGGameDrawerAd.OnShow(() =>
            {
                Debug.Log("互推盒子抽屉广告展示回调成功");
            });    
```

## <a id="文件下载"></a>文件下载

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

## <a id="文件上传"></a>文件上传

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

## <a id="视频播放"></a>视频播放

使用此接口可以让玩家在游戏中播放远程视频

```c#
    QGVideoPlayer obj = QG.PlayVideo(new VideoParam()
    {
        //必要参数 具体参数参考: https://ie-activity-cn.heytapimage.com/static/minigame/CN/docs/index.html#/develop/media/video
        url = "http://vjs.zencdn.net/v/oceans.mp4",  //视频的资源地址
        //选填参数
        ParamX = 0,                 //视频的左上角横坐标 默认值0
        ParamY = 500,               //视频的左上角纵坐标 默认值0
        ParamWidth = 1000,          //视频的宽度 默认值300
        ParamHeight = 800,          //视频的高度 默认值150
        poster = "/poster.jpg",     //视频封面 qgfile://usr/poster
        initialTime = 0.1f,         //视频的初始播放位置，单位为 s 秒  默认值0
        playbackRate = 0.5f,        //视频的播放速率，有效值有 0.5、0.8、1.0、1.25、1.5 默认值1.0
        live = false,               //视频是否为直播 默认值false    
        objectFit = "contain",      //视频的缩放模式 fill:填充  contain:包含，保持原有长宽比例  cover: 覆盖，保持原有长宽比例 默认值'contain'    
        autoplay = false,           //视频是否自动播放 默认值false
        loop = true,                //视频是否循环播放 默认值false
        muted = false,              //视频是否静音 默认值false
        controls = true,            //视频是否显示控件 true
        showCenterPlayBtn = true,   //是否显示视频中央的播放按钮 默认值false
        enableProgressGesture = false   //是否启用手势控制播放进度 默认值false
    });
    // 新增视频监听:OnPlay,OnPause,OnEnded,OnTimeUpdate,OnError,OnWaiting,OnSeeking,OnSeeked.
    // 新增视频方法:Play(),Pause(),Stop(),Seek(Time),Destroy().
```

## <a id="音频播放"></a>音频播放

使用此接口可以让玩家在游戏中播放远程音频

```c#
  qGAudioPlayer = QG.PlayAudio(new AudioParam()
        {
            url = "https://activity-cdo.heytapimage.com/cdo-activity/static/minigame/test/demo/music/huxia-4M.mp3", //播放链接
            // url = "/huxia-4M.mp3", //如果已使用 QG.DownLoadFile 下载音频文件到本地，可播放本地音频
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

// 新增音频监听:OnPlay,OnCanPlay,OnPause,OnStop,OnEnded,OnTimeUpdate,OnError,OnWaiting,OnSeeking,OnSeeked.
// 新增音频方法:Play(),Pause(),Stop(),Seek(Time),Destroy,SetVolume,SetLoop.
```

## <a id="录音能力"></a>录音能力

使用此接口可以让玩家在游戏中使用录音能力

```c#
    //录音音频源
    private string audioUrl; 
    //创建录音
    QGRecordManager qGRecordManager = QG.GetRecorderManager(); 
    qGRecordManager.OnStop((QGBaseResponse res) =>
    {
        audioUrl = res.errMsg;  //录音音频源需要停止录音后在录音回调获取。
    });
    //开始录音 默认可调用qGRecordManager.Start(new RecordParam());
    qGRecordManager.Start(new RecordParam()                    
            {
                duration = 80000,       //录音的时长，单位 ms，最大值 600000（10 分钟） 默认值 60000
                sampleRate = 8000,      //采样率 默认值 8000
                numberOfChannels = 2,   //录音通道数  默认值 2
                encodeBitRate = 48000,  //编码码率 默认值 48000
                format = "aac",         //音频格式  默认值 aac
                frameSize = 1000,       //指定帧大小，单位 KB
                audioSource = "auto"    //指定录音的音频输入源 默认值 auto
            });  
    //暂停录音
    qGRecordManager.Pause();
    //继续录音
    qGRecordManager.Resume();
    //停止录音
    qGRecordManager.Stop();
    //播放录音
    QGAudioPlayer qGAudioPlayer = QG.PlayAudio(new AudioParam()
                {
                    url = audioUrl,
                    startTime = 0f,
                    loop = true,
                    volume = 1f
                });
    // 新增录音监听:OnStart,OnResume,OnPause,OnStop,OnError.
    // 新增录音方法:Start(),Pause(),Resume(),Stop().
```

## <a id="消息框"></a>消息框

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

## <a id="数据储存"></a>数据储存

```c#
     QG.StorageSetItem(Key, Value); //存储
     QG.StorageGetItem(Key); //读取
     QG.StorageRemoveItem(Key); //删除
```

## <a id="创建桌面图标"></a>创建桌面图标

```c#
    QG.HasShortcutInstalled((msg) =>
    {
    Debug.Log("QG.HasShortcutInstalled success = " +JsonUtility.ToJson(msg));},
    (msg) =>
    {
    Debug.Log("QG.HasShortcutInstalled fail = " + msg.errMsg);
    });
```

## <a id="退出当前 OPPO 小游戏"></a>退出当前 OPPO 小游戏

使用此接口可以让玩家在游戏中退出当前 OPPO 小游戏

```c#
        QG.ExitApplication();
        QG.ExitApplication(null);
        QG.ExitApplication(new ExitApplicationParam(){data = null});
        QG.ExitApplication(new ExitApplicationParam(){data = "发送到客户端"});
        //以上退出传参皆可
```

## <a id="键盘"></a>键盘

使用此接口可以让玩家在游戏中使用键盘

```c#
        //显示键盘
        string keyboardId = QG.ShowKeyboard(new KeyboardParam() //keyboardId 创建键盘返回唯一标识 用于判断多键盘场景
        {
            defaultValue = "", //键盘输入框显示的默认值 默认为空字符串
            maxLength = 100,   //键盘中文本的最大长度 默认值为 100
            multiple = true,  //是否为多行输入 默认值为 false
            confirmHold = true //当点击完成时键盘是否收起 默认值为 true
        });
        //隐藏键盘
        QG.HideKeyboard();
        //监听键盘输入事件
        QG.OnKeyboardInput((msg) =>
        {
            QGResKeyBoardponse data = JsonUtility.FromJson<QGResKeyBoardponse>(JsonUtility.ToJson(msg));
                if (data.keyboardId == keyboardId)
                {
                    InputObject.text = data.value;
                }
        });
        //取消监听键盘输入事件
        QG.OffKeyboardInput();
        //监听用户点击键盘 Confirm 按钮时的事件
        QG.OnKeyboardConfirm((msg) =>
        {
            QGResKeyBoardponse data = JsonUtility.FromJson<QGResKeyBoardponse>(JsonUtility.ToJson(msg));
               if (data.keyboardId == keyboardId)
               {
                    InputObject.text = data.value;
               }
        });
        //取消监听用户点击键盘 Confirm 按钮时的事件
        QG.OffKeyboardConfirm();
        //监听监听键盘收起的事件
        QG.OnKeyboardComplete((msg) =>
        {
            QGResKeyBoardponse data = JsonUtility.FromJson<QGResKeyBoardponse>(JsonUtility.ToJson(msg));
               if (data.keyboardId == keyboardId)
               {
                    InputObject.text = data.value;
               }
        });
        //取消监听监听键盘收起的事件
        QG.OffKeyboardComplete();
```

## <a id="对话框"></a>对话框

使用此接口可以让玩家在游戏中使用对话框

```c#
     QG.ShowModal(new ShowModalParam()
        {
            title = "对话标题",
            content = "对话内容",
            showCancel = true  //是否显示取消按钮，默认为 true
        },
        (success) =>
        {
            if (success.data.confirm)
            {
                Debug.Log("确认");
            }
            if (success.data.cancel)
            {
                Debug.Log("取消");
            }
        },
        (fail) =>
        {
            Debug.Log("失败返回");
        },
        (complete) =>
        {
            Debug.Log("跳过返回");
        });
```

## <a id="进度条"></a>进度条

使用此接口可以让玩家在游戏中使用进度条

```c#
     QG.ShowLoading("进度提示文本");
        QG.SetTimeout(2000, (msg) =>
        {
            QG.HideLoading((msg) =>
            {
                Debug.Log("关闭进度成功");
            });
        });
```

## <a id="定时器"></a>定时器

使用此接口可以让玩家在游戏中使用定时器

```c#
        QG.SetTimeout(2000, (msg) =>
        {
            Debug.Log("2000毫秒后执行");
        });
```

## <a id="云存储"></a>云存储

使用此接口可以让玩家在游戏中使用云存储

```c#
       //云存储key value
       QG.SetUserCloudStorage("key", "value",
       (success) =>
       {
           Debug.Log("云存储成功" + success.data);
       },
       (fail) =>
       {
           Debug.Log("云存储失败" + fail);
       },
       (complete) =>
       {
           Debug.Log("云存储跳过");
       });

        //获取value
        QG.GetUserCloudStorage("key",
        (success) =>
        {
            Debug.Log("云数据读取,Key: " + success.data.key + ",Value: " + success.data.value);
        },
        (fail) =>
        {
            Debug.Log("获取miniKey fail");
        },
        (complete) =>
        {
            Debug.Log("获取获取miniKey complete");
        });

        //通过key删除
        QG.RemoveUserCloudStorage("key");
```

## <a id="系统信息"></a>系统信息

使用此接口可以让玩家在游戏中使用系统信息

```c#
    //系统信息(异步)
     QG.GetSystemInfo((success) =>
        {
        var msg = success.data;
        string brand = msg.brand; // 手机品牌
        string language = msg.language; // 系统语言
        string model = msg.model; // 手机型号
        float notchHeight = msg.notchHeight; //刘海高度
        float statusBarHeight = msg.statusBarHeight; // 状态栏/异形缺口高度
        float pixelRatio = msg.pixelRatio; // 设备像素比
        string platform = msg.platform; // 客户端平台
        float platformVersion = msg.platform; // 平台版本号
        string platformVersionName = msg.platformVersionName; // 客户端平台
        float platformVersionCode = msg.platformVersionCode; // 网络类型
        float screenHeight = msg.screenHeight; // 屏幕高度
        float screenWidth = msg.screenWidth; // 屏幕宽度
        string system = msg.system; // 系统版本
        float windowHeight = msg.windowHeight; // 可使用窗口高度
        float windowWidth = msg.windowWidth; // 可使用窗口宽度
        string theme = msg.theme; // 系统当前主题
        string deviceOrientation = msg.deviceOrientation; // 设备方向
        float benchmarkLevel = msg.benchmarkLevel; // 设备性能等级（仅 Android）。取值为：（性能未知）-1，0<（低端机）<44，44<=（中端机）<58，>=58（高端机）
        string COREVersion = msg.COREVersion; //版本号(兼容旧版本)
        string coreVersion = msg.coreVersion; //版本号(客户端基础库版本)
        },
         (err) =>
         {
             Debug.Log("QG.GetSystemInfo fail = " + JsonUtility.ToJson(err));
         });

    //系统信息(同步)
    string systemStr = QG.GetSystemInfoSync();
    QGSystemInfo qgSystemInfo = JsonUtility.FromJson<QGSystemInfo>(systemStr);
    Debug.Log("QG.GetSystemInfoSyncFunc = " + systemStr);
```

## <a id="获取渠道的名称"></a>获取渠道的名称

使用此接口可以让玩家在游戏中获取渠道的名称

```c#
        string provider = QG.GetProvider();
        Debug.Log("渠道信息: \n" + provider);
```

## <a id="获取配置文件Manifest"></a>获取配置文件 Manifest

使用此接口可以让玩家在游戏中获取配置文件 Manifest

```c#
        QG.GetManifestInfo((msg) =>
        {
        string package = msg.data.package;  //游戏包名
        string name = msg.data.name;     //游戏名
        string versionName = msg.data.versionName; //游戏版本名
        string versionCode = msg.data.versionCode; //游戏版本号
        string minPlatformVersion = msg.data.minPlatformVersion; //最小平台版本号
        string icon = msg.data.icon; //桌面图标
        string orientation = msg.data.orientation; //设备方向
        string type = msg.data.type; //不填或者默认值为 app，取值为 app 或 game
        object config = msg.data.config; //logLevel 取值
        object subpackages = msg.data.subpackages; //分包功能，有分包时才需要，可选字段
        },
         (err) =>
         {
             Debug.Log("QG.GetManifestInfo fail = " + JsonUtility.ToJson(err));
         }
        );
```

## <a id="修改渲染帧率"></a>修改渲染帧率

使用此接口可以让玩家在游戏中修改渲染帧率

```c#
   int fps = 30; //默认渲染帧率为 60 帧每秒
   QG.SetPreferredFramesPerSecond(fps);
```

## <a id="电量"></a>电量

使用此接口可以让玩家在游戏中获取电量

```c#
        //电量(异步)
        QG.GetBatteryInfo(
        (success) =>
        {
        float level = success.data.level; //设备电量，范围 1 - 100
        bool isCharging = success.data.isCharging; //是否正在充电中
        },
        (fail) =>
        {
            Debug.Log("QG.GetBatteryInfo fail = " + JsonUtility.ToJson(fail));
        },
        (complete) =>
        {
        });

        //电量(同步)
        BatteryInfoParam batteryInfoParam = QG.GetBatteryInfoSync();
        Debug.Log("同步电量信息: level: " + batteryInfoParam.level + "isCharging:" + batteryInfoParam.isCharging);
```

## <a id="获取设备唯一标识"></a>获取设备唯一标识

使用此接口可以让玩家在游戏中获取设备唯一标识

```c#
        QG.GetDeviceId(
        (success) =>
        {
            string deviceId = success.data.deviceId; //设备唯一标识
        },
        (fail) =>
        {
            Debug.Log("QG.GetDeviceId fail = " + JsonUtility.ToJson(fail));
        },
        (complete) =>
        {
        });
```

## <a id="获取设备亮度"></a>获取设备亮度

使用此接口可以让玩家在游戏中获取设备亮度

```c#
        //亮度(获取)
        QG.GetScreenBrightness(
       (success) =>
       {
            float value = success.data.value; //亮度
       },
       (fail) =>
       {
           Debug.Log("QG.GetScreenBrightness fail = " + JsonUtility.ToJson(fail));
       },
       (complete) =>
       {
       });

       //亮度(设置)
       float num = 0.5f; //亮度取值范围 0~1
       QG.SetScreenBrightness(num,
       (success) =>
       {
           Debug.Log("QG.SetScreenBrightness success = " + JsonUtility.ToJson(success));
       },
       (fail) =>
       {
           Debug.Log("QG.SetScreenBrightness fail = " + JsonUtility.ToJson(fail));
       },
       (complete) =>
       {
       });

       //亮度(设置常亮)
       bool bl = true; //常亮true
       QG.SetKeepScreenOn(bl,
       (success) =>
       {
           Debug.Log("QG.SetKeepScreenOn success = " + JsonUtility.ToJson(success));
       },
       (fail) =>
       {
           Debug.Log("QG.SetKeepScreenOn fail = " + JsonUtility.ToJson(fail));
       },
       (complete) =>
       {
       });
```

## <a id="获取当前的地理位置、速度"></a>获取当前的地理位置、速度

使用此接口可以让玩家在游戏中获取当前的地理位置、速度

```c#
       QG.GetLocation(
       (success) =>
       {
           float latitude = success.data.latitude; //纬度，范围为 -90~90，负数表示南纬
           float longitude = success.data.longitude; //经度，范围为 -180~180，负数表示西经
           float speed = success.data.speed; //速度，单位 m/s
           float accuracy = success.data.accuracy; //位置的精确度
           float altitude = success.data.altitude; //高度，单位 m
           float verticalAccuracy = success.data.verticalAccuracy; //垂直精度，单位 m（Android 无法获取，返回 0）
           float horizontalAccuracy = success.data.horizontalAccuracy; //水平精度，单位 m
           },
       (fail) =>
       {
           Debug.Log("QG.GetLocation fail = " + JsonUtility.ToJson(fail));
       },
       (complete) =>
       {
       });
```

## <a id="监听加速度数据"></a>监听加速度数据

使用此接口可以让玩家在游戏中监听加速度数据

```c#
        //开始监听加速度数据
        //game   适用于更新游戏的回调频率，在 20ms/次 左右
        //ui     适用于更新 UI 的回调频率，在 60ms/次 左右
        //normal 普通的回调频率，在 200ms/次 左右
        string type = "game";
        QG.StartAccelerometer(
        type,
        (success) =>
        {
            Debug.Log("QG.StartAccelerometer success = " + JsonUtility.ToJson(success));
        },
        (fail) =>
        {
            Debug.Log("QG.StartAccelerometer fail = " + JsonUtility.ToJson(fail));
        },
        (complete) =>
        {
        });

        //停止监听加速度数据
        QG.StopAccelerometer(
        (success) =>
        {
            Debug.Log("QG.StopAccelerometer success = " + JsonUtility.ToJson(success));
        },
        (fail) =>
        {
            Debug.Log("QG.StopAccelerometer fail = " + JsonUtility.ToJson(fail));
        },
        (complete) =>
        {
        });

        //监听加速度数据
        QG.OnAccelerometerChange(
        (success) =>
        {
            float x = success.data.QgParamX;
            float y = success.data.QgParamY;
            float z = success.data.QgParamZ;
        });
```

## <a id="获取系统剪贴板的内容"></a>获取系统剪贴板的内容

使用此接口可以让玩家在游戏中获取系统剪贴板的内容

```c#
        //设置系统剪贴板的内容
        string context = "测试剪切板"; //设置内容
        QG.SetClipboardData(
        context,
        (success) =>
        {
            Debug.Log("QG.SetClipboardData success = " + JsonUtility.ToJson(success));
        },
        (fail) =>
        {
            Debug.Log("QG.SetClipboardData fail = " + JsonUtility.ToJson(fail));
        },
        (complete) =>
        {
        });

        //获取系统剪贴板的内容
        QG.GetClipboardData(
        (success) =>
        {
            string context = success.data; //内容
            Debug.Log("QG.GetClipboardData success = " + context);
        },
        (fail) =>
        {
            Debug.Log("QG.GetClipboardData fail = " + JsonUtility.ToJson(fail));
        },
        (complete) =>
        {
        });
```

## <a id="监听罗盘数据"></a>监听罗盘数据

使用此接口可以让玩家在游戏中监听罗盘数据

```c#
        //开始监听罗盘数据
        QG.StartCompass(
        (success) =>
        {
            Debug.Log("QG.StartCompass success = " + JsonUtility.ToJson(success));
        },
        (fail) =>
        {
            Debug.Log("QG.StartCompass fail = " + JsonUtility.ToJson(fail));
        },
        (complete) =>
        {
        });

        //停止监听罗盘数据
        QG.StopCompass(
        (success) =>
        {
            Debug.Log("QG.StopCompass success = " + JsonUtility.ToJson(success));
        },
        (fail) =>
        {
            Debug.Log("QG.StopCompass fail = " + JsonUtility.ToJson(fail));
        },
        (complete) =>
        {
        });

        //监听罗盘数据
        QG.OnCompassChange(
        (success) =>
        {
            float direction = success.data;
            Debug.Log("面对的方向度数 = " + direction);
        });
```

## <a id="获取本地临时文件或本地用户文件的文件信息"></a>获取本地临时文件或本地用户文件的文件信息

使用此接口可以让玩家在游戏中获取本地临时文件或本地用户文件的文件信息

```c#
      //filename 说明: "/"+本地文件名.后缀
      string filename = "/BeAttack.ogg";
      QG.GetFileInfo(filename, (success) =>
      {
        Debug.Log("本地文件已存在：" + JsonUtility.ToJson(success));
      },
      (fail) =>
      {
        Debug.Log("本地文件不存在：" + JsonUtility.ToJson(fail));
      }
     );
```

## <a id="判断文件/目录是否存在"></a>判断文件/目录是否存在

使用此接口可以让玩家在游戏中判断文件/目录是否存在

```c#
     //异步
      string filename = "/test.zip";    //要判断是否存在的文件/目录路径
      QG.Access(filename, (success) =>
      {
          Debug.Log("QG.Access success = " + JsonUtility.ToJson(success));
      },
      (fail) =>
      {
          Debug.Log("QG.Access fail = " + JsonUtility.ToJson(fail));
      },
       (complete) =>
      {
          Debug.Log("QG.Access complete = " + JsonUtility.ToJson(complete));
      }
     );
     //同步
    //bool isAccessSync = QG.AccessSync(filename); //所有File类Sync同步方法 success fail 回调可不传
    bool isAccessSync = QG.AccessSync(filename, (success) =>
        {
            Debug.Log("QG.AccessSync success = " + JsonUtility.ToJson(success));
        },
        (fail) =>
        {
            Debug.Log("QG.AccessSync fail = " + JsonUtility.ToJson(fail));
        }
    );
    loginMessage.text = "QG.AccessSync 同步判断文件/目录是否存在 = " + isAccessSync;
```

## <a id="在文件结尾追加内容"></a>在文件结尾追加内容

使用此接口可以让玩家在游戏中在文件结尾追加内容

```c#
     //AppendFile AppendFileSync 第二个参数 param 支持传入string 或 byte数组
     //异步
        string filename = "/myfile.txt";                //要追加内容的文件路径
        string append_data = "appendFile data";         //要追加的文本或二进制数据
        byte[] append_byteArray = new byte[] { 1, 2, 3, 4 };    //要追加的文本或二进制数据
        QG.AppendFile(filename, append_byteArray, (success) =>
      {
          Debug.Log("QG.AppendFile success = " + JsonUtility.ToJson(success));
      },
      (fail) =>
      {
          Debug.Log("QG.AppendFile fail = " + JsonUtility.ToJson(fail));
      },
       (complete) =>
      {
          Debug.Log("QG.AppendFile complete = " + JsonUtility.ToJson(complete));
      }
     );
     //同步
        bool isAppendFileSync = QG.AppendFileSync(filename, append_byteArray, (success) =>
         {
             Debug.Log("QG.AppendFileSync success = " + JsonUtility.ToJson(success));
         },
         (fail) =>
         {
             Debug.Log("QG.AppendFileSync fail = " + JsonUtility.ToJson(fail));
         }
        );
        Debug.Log("QG.AppendFileSync 是否同步在文件結尾追加內容 = " + isAppendFileSync);
```

## <a id="复制文件"></a>复制文件

使用此接口可以让玩家在游戏中复制文件

```c#
     //异步
        string srcPath = "/myfile.txt";         //源文件路径，只可以是普通文件
        string destPath = "/myfileCopy.txt";    //目标文件路径
        QG.CopyFile(srcPath, destPath, (success) =>
      {
          Debug.Log("QG.CopyFile success = " + JsonUtility.ToJson(success));
      },
      (fail) =>
      {
          Debug.Log("QG.CopyFile fail = " + JsonUtility.ToJson(fail));
      },
       (complete) =>
      {
          Debug.Log("QG.CopyFile complete = " + JsonUtility.ToJson(complete));
      }
     );
     //同步
        bool isCopyFileSync = QG.CopyFileSync(srcPath, destPath, (success) =>
          {
              Debug.Log("QG.CopyFileSync success = " + JsonUtility.ToJson(success));
          },
          (fail) =>
          {
              Debug.Log("QG.CopyFileSync fail = " + JsonUtility.ToJson(fail));
          }
         );
        Debug.Log("QG.CopyFileSync 是否同步拷貝目录 = " + isCopyFileSync);
```

## <a id="创建目录"></a>创建目录

使用此接口可以让玩家在游戏中创建目录

```c#
     //异步
        string dirPath = "/oppo";       //创建的目录路径
        QG.Mkdir(dirPath, (success) =>
        {
            Debug.Log("QG.Mkdir success = " + JsonUtility.ToJson(success));
        },
        (fail) =>
        {
            Debug.Log("QG.Mkdir fail = " + JsonUtility.ToJson(fail));
        },
         (complete) =>
        {
            Debug.Log("QG.Mkdir complete = " + JsonUtility.ToJson(complete));
        }
       );
     //同步
        bool recursive = true; //是否在递归创建该目录的上级目录后再创建该目录
        bool isMkdirSync = QG.MkdirSync(dirPath, recursive, (success) =>
       {
           Debug.Log("QG.MkdirSync success = " + JsonUtility.ToJson(success));
       },
       (fail) =>
       {
           Debug.Log("QG.MkdirSync fail = " + JsonUtility.ToJson(fail));
       }
      );
        Debug.Log("QG.MkdirSync 是否同步创建目录 = " + isMkdirSync);
```

## <a id="读取本地文件内容"></a>读取本地文件内容

使用此接口可以让玩家在游戏中读取本地文件内容

```c#
     //异步
        string filename = "/myfile.txt";    //要读取的文件的路径
        string encoding = "binary"; //指定读取文件的字符编码，默认为 binary, 支持 utf8 binary 类型
        QG.ReadFile(filename, encoding, (success) =>
      {
          ReadFileResponse res = JsonUtility.FromJson<ReadFileResponse>(JsonUtility.ToJson(success));
          Debug.Log("QG.ReadFile success = " + JsonUtility.ToJson(success));
          if (res.encoding == "utf8")
          {
              Debug.Log("QG.ReadFile success = " + JsonUtility.ToJson(success) + "\n encoding:" + res.encoding + "\n dataStr:" + res.dataStr + "\n dataUtf8:" + res.dataUtf8);
          }
          else if (res.encoding == "binary")
          {
              Debug.Log("QG.ReadFile success = " + JsonUtility.ToJson(success) + "\n encoding:" + res.encoding + "\n dataStr:" + res.dataStr + "\n dataUtf8:" + res.dataUtf8 + "\n dataBytes[0]:" + res.dataBytes[0] + "\n dataBytes[end]:" + res.dataBytes[res.dataBytes.Length - 1]);
          }
      },
      (fail) =>
      {
          Debug.Log("QG.ReadFile fail = " + JsonUtility.ToJson(fail));
      },
       (complete) =>
      {
          Debug.Log("QG.ReadFile complete = " + JsonUtility.ToJson(complete));
      }
     );
     //同步
        ReadFileResponse res = QG.ReadFileSync(filename, encoding, (success) =>
       {
           ReadFileResponse readFileResponse = JsonUtility.FromJson<ReadFileResponse>(JsonUtility.ToJson(success));
           if (readFileResponse.encoding == "utf8")
           {
               Debug.Log("QG.ReadFileSync utf8 success = " + JsonUtility.ToJson(success));
           }
           else if (readFileResponse.encoding == "binary")
           {
               Debug.Log("QG.ReadFileSync binary success = " + JsonUtility.ToJson(success));
           }
       },
       (fail) =>
       {
           Debug.Log("QG.ReadFileSync fail = " + JsonUtility.ToJson(fail));
       }
      );
        if (res == null)
        {
            Debug.Log("QG.ReadFileSync fail");
            return;
        }
        if (res.encoding == "utf8")
        {
            Debug.Log("QG.ReadFileSync success =\n encoding:" + res.encoding + "\n dataStr:" + res.dataStr + "\n dataUtf8:" + res.dataUtf8);
        }
        else if (res.encoding == "binary")
        {
            Debug.Log("QG.ReadFileSync success =\n encoding:" + res.encoding + "\n dataStr:" + res.dataStr + "\n dataUtf8:" + res.dataUtf8 + "\n dataBytes[0]:" + res.dataBytes[0] + "\n dataBytes[end]:" + res.dataBytes[res.dataBytes.Length - 1]);
        }
    //返回对象
    public class ReadFileResponse
    {
        public string encoding;     //读取文件编码
        public string dataStr;      //读取文件字符串
        public string dataUtf8;     //读取文件UTF8
        public byte[] dataBytes;    //读取文件字节
    }
```

## <a id="重命名文件，可以把文件从 oldPath 移动到 newPath"></a>重命名文件，可以把文件从 oldPath 移动到 newPath

使用此接口可以让玩家在游戏中重命名文件，可以把文件从 oldPath 移动到 newPath

```c#
     //异步
        string oldPath = "/myfile.txt";         //源文件路径，可以是普通文件或目录
        string newPath = "/new1/rename.txt";    //新文件路径
        QG.Rename(oldPath, newPath, (success) =>
      {
          Debug.Log("QG.Rename success = " + JsonUtility.ToJson(success));
      },
      (fail) =>
      {
          Debug.Log("QG.Rename fail = " + JsonUtility.ToJson(fail));
      },
       (complete) =>
      {
          Debug.Log("QG.Rename complete = " + JsonUtility.ToJson(complete));
      }
     );
     //同步
        bool isRenameSync = QG.RenameSync(oldPath, newPath, (success) =>
          {
              Debug.Log("QG.RenameSync success = " + JsonUtility.ToJson(success));
          },
          (fail) =>
          {
              Debug.Log("QG.RenameSync fail = " + JsonUtility.ToJson(fail));
          }
         );
        Debug.Log("QG.RenameSync 是否同步重命名文件 = " + isRenameSync);
```

## <a id="删除目录"></a>删除目录

使用此接口可以让玩家在游戏中删除目录

```c#
     //异步
        string dirPath = "/oppo";   //要删除的目录路径
        bool recursive = true;      //是否递归删除目录。如果为 true，则删除该目录和该目录下的所有子目录以及文件。
        QG.Rmdir(dirPath, recursive, (success) =>
        {
            Debug.Log("QG.Rmdir success = " + JsonUtility.ToJson(success));
        },
        (fail) =>
        {
            Debug.Log("QG.Rmdir fail = " + JsonUtility.ToJson(fail));
        },
         (complete) =>
        {
            Debug.Log("QG.Rmdir complete = " + JsonUtility.ToJson(complete));
        }
       );
     //同步
        bool isRmdirSync = QG.RmdirSync(dirPath, recursive, (success) =>
        {
            Debug.Log("QG.RmdirSync success = " + JsonUtility.ToJson(success));
        },
        (fail) =>
        {
            Debug.Log("QG.RmdirSync fail = " + JsonUtility.ToJson(fail));
        }
       );
        Debug.Log("QG.RmdirSync 是否同步删除目录 = " + isRmdirSync);
```

## <a id="读取目录内文件列表"></a>读取目录内文件列表

使用此接口可以让玩家在游戏中读取目录内文件列表

```c#
     //异步
        string dirPath = "/";   //要读取的目录路径
        QG.ReadDir(dirPath, (success) =>
      {
          ReadDirResponse res = JsonUtility.FromJson<ReadDirResponse>(JsonUtility.ToJson(success));
          Debug.Log("QG.ReadDir success = " + JsonUtility.ToJson(success));
      },
      (fail) =>
      {
          Debug.Log("QG.ReadDir fail = " + JsonUtility.ToJson(fail));
      },
       (complete) =>
      {
          Debug.Log("QG.ReadDir complete = " + JsonUtility.ToJson(complete));
      }
     );
     //同步
        ReadDirResponse res = QG.ReadDirSync(dirPath, (success) =>
          {
              ReadDirResponse readDirResponse = JsonUtility.FromJson<ReadDirResponse>(JsonUtility.ToJson(success));
              Debug.Log("QG.ReadDirSync success = " + JsonUtility.ToJson(success));
          },
          (fail) =>
          {
              Debug.Log("QG.ReadDirSync fail = " + JsonUtility.ToJson(fail));
          }
         );
        if (res == null)
        {
            Debug.Log("QG.ReadDirSync fail");
            return;
        }
        Debug.Log("QG.ReadDirSync success\n files[0]:" + res.files[0] + "\n files[end]:" + res.files[res.files.Length - 1]);
     //返回对象
    public class ReadDirResponse
    {
        public string filesStr; //文件列表字符串
        public string[] files;  //文件列表字符串数组
    }
```

## <a id="删除文件"></a>删除文件

使用此接口可以让玩家在游戏中删除文件

```c#
     //异步
        string dirPath = "/Unlink.txt";     //要删除的文件路径
        QG.Unlink(dirPath, (success) =>
      {
          Debug.Log("QG.Unlink success = " + JsonUtility.ToJson(success));
      },
      (fail) =>
      {
          Debug.Log("QG.Unlink fail = " + JsonUtility.ToJson(fail));
      },
       (complete) =>
      {
          Debug.Log("QG.Unlink complete = " + JsonUtility.ToJson(complete));
      }
     );
     //同步
        bool isUnlinkSync = QG.UnlinkSync(dirPath, (success) =>
       {
           Debug.Log("QG.UnlinkSync success = " + JsonUtility.ToJson(success));
       },
       (fail) =>
       {
           Debug.Log("QG.UnlinkSync fail = " + JsonUtility.ToJson(fail));
       }
      );
        Debug.Log("QG.UnlinkSync 是否同步刪除文件 = " + isUnlinkSync);
```

## <a id="解压文件"></a>解压文件

使用此接口可以让玩家在游戏中解压文件

```c#
     //异步
        string zipFilePath = "/test.zip"; //源文件路径，只可以是 zip 压缩文件
        string targetPath = "/";          //目标目录路径
        QG.Unzip(zipFilePath, targetPath, (success) =>
      {
          Debug.Log("QG.Unzip success = " + JsonUtility.ToJson(success));
      },
      (fail) =>
      {
          Debug.Log("QG.Unzip fail = " + JsonUtility.ToJson(fail));
      },
       (complete) =>
      {
          Debug.Log("QG.Unzip complete = " + JsonUtility.ToJson(complete));
      }
     );
```

## <a id="写文件"></a>写文件

使用此接口可以让玩家在游戏中写文件

```c#
     //WriteFile WriteFileSync 第二个参数 param 支持传入string 或 byte数组
     //异步
        string filename = "/myfile.txt";    //要写入的文件路径
        string append_data = "writeFile data"; //要写入的文本或二进制数据
        byte[] append_byteArray = new byte[] { 1, 2, 3, 4 }; //要写入的文本或二进制数据
        bool append = false;            //默认为 false，覆盖旧文件
        QG.WriteFile(filename, append_data, append, (success) =>
      {
          Debug.Log("QG.WriteFile success = " + JsonUtility.ToJson(success));
      },
      (fail) =>
      {
          Debug.Log("QG.WriteFile fail = " + JsonUtility.ToJson(fail));
      },
       (complete) =>
      {
          Debug.Log("QG.WriteFile complete = " + JsonUtility.ToJson(complete));
      }
     );
     //同步
        bool isWriteFileSync = QG.WriteFileSync(filename, append_byteArray, append, (success) =>
          {
              Debug.Log("QG.WriteFileSync success = " + JsonUtility.ToJson(success));
          },
          (fail) =>
          {
              Debug.Log("QG.WriteFileSync fail = " + JsonUtility.ToJson(fail));
          }
         );
        Debug.Log("QG.WriteFileSync 是否同步写入文件 = " + isWriteFileSync);
```

## <a id="保存临时文件到本地"></a>保存临时文件到本地

使用此接口可以让玩家在游戏中保存临时文件到本地

```c#
     //异步
        string filePath = "/myfile.txt";          //要存储的文件路径
        string tempFilePath = "/myfileSave.txt";  //临时存储文件路径
        QG.SaveFile(tempFilePath, filePath, (success) =>
      {
          Debug.Log("QG.SaveFile success = " + JsonUtility.ToJson(success));
      },
      (fail) =>
      {
          Debug.Log("QG.SaveFile fail = " + JsonUtility.ToJson(fail));
      },
       (complete) =>
      {
          Debug.Log("QG.SaveFile complete = " + JsonUtility.ToJson(complete));
      }
     );
     //同步
        string SaveFileSyncStr = QG.SaveFileSync(tempFilePath, filePath, (success) =>
          {
              Debug.Log("QG.SaveFileSync success = " + JsonUtility.ToJson(success));
          },
          (fail) =>
          {
              Debug.Log("QG.SaveFileSync fail = " + JsonUtility.ToJson(fail));
          }
         );
        Debug.Log("QG.SaveFileSync 同步保存临时文件路径 = " + SaveFileSyncStr);
```

## <a id="删除该 OPPO 小游戏下已保存的本地缓存文件"></a>删除该 OPPO 小游戏下已保存的本地缓存文件

使用此接口可以让玩家在游戏中删除该 OPPO 小游戏下已保存的本地缓存文件

```c#
     //异步
        string filePath = "/RemoveSavedFile.txt";       //文件/目录路径
        QG.RemoveSavedFile(filePath, (success) =>
      {
          Debug.Log("QG.RemoveSavedFile success = " + JsonUtility.ToJson(success));
      },
      (fail) =>
      {
          Debug.Log("QG.RemoveSavedFile fail = " + JsonUtility.ToJson(fail));
      },
       (complete) =>
      {
          Debug.Log("QG.RemoveSavedFile complete = " + JsonUtility.ToJson(complete));
      }
     );
```

## <a id="获取文件 Stats 对象"></a>获取文件 Stats 对象

使用此接口可以让玩家在游戏中获取文件 Stats 对象

```c#
     //异步
        string path = "/test.zip";  //文件/目录路径
        bool recursive = false;     //是否递归获取目录中所有文件的信息
        QG.Stat(path, (success) =>
      {
          StatResponse res = JsonUtility.FromJson<StatResponse>(JsonUtility.ToJson(success));
          Debug.Log("QG.Stat success = " + JsonUtility.ToJson(success));
      },
      (fail) =>
      {
          Debug.Log("QG.Stat fail = " + JsonUtility.ToJson(fail));
      },
       (complete) =>
      {
          Debug.Log("QG.Stat complete = " + JsonUtility.ToJson(complete));
      }
     );
     //同步
        StatResponse res = QG.StatSync(path, recursive, (success) =>
        {
            StatResponse statResponse = JsonUtility.FromJson<StatResponse>(JsonUtility.ToJson(success));
            Debug.Log("QG.StatSync success = " + JsonUtility.ToJson(success));
        },
        (fail) =>
        {
            Debug.Log("QG.StatSync fail = " + JsonUtility.ToJson(fail));
        }
       );
        if (res == null)
        {
            Debug.Log("QG.StatSync fail");
            return;
        }
        Debug.Log("QG.StatSync success\n mode:" + res.mode + "\n size:" + res.size + "\n lastAccessedTime:" + res.lastAccessedTime + "\n lastModifiedTime:" + res.lastModifiedTime + "\n isDirectory:" + res.isDirectory + "\n isFile:" + res.isFile);
        //返回对象
        public class StatResponse
        {
            public int mode; //文件 mode
            public int size; //文件大小
            public int lastAccessedTime; //最后一次读取的时间
            public int lastModifiedTime; //最后一次修改时间
            public bool isDirectory; //判断当前文件是否一个目录
            public bool isFile; //判断当前文件是否一个普通文件
        }
```

## <a id="判断是否已经创建桌面图标"></a>判断是否已经创建桌面图标

使用此接口可以让玩家在游戏中判断是否已经创建桌面图标

```c#
        QG.HasShortcutInstalled((msg) =>
           {
               Debug.Log("是否已经创建桌面图标 = " + msg.hasShortcutInstalled);
               if (msg.hasShortcutInstalled)
               {
                   Debug.Log("桌面图标已创建");
                   return;
               }
                    QG.InstallShortcut((success) =>
                    {Debug.Log("创建桌面图标成功 = " + JsonUtility.ToJson(success));},
                    (fail) =>
                    {Debug.Log("取消创建桌面图标 = " + JsonUtility.ToJson(fail));},
                    (complete) =>
                    {Debug.Log("QG.InstallShortcut complete = " + JsonUtility.ToJson(complete));}
                    );
           },
           (msg) =>
           {
               Debug.Log("接口调用失败的回调函数 = " + JsonUtility.ToJson(msg));
           },
           (msg) =>
           {
               Debug.Log("接口调用结束的回调函数 = " + JsonUtility.ToJson(msg));
           });
```

## <a id="创建桌面图标，每次创建都需要用户授权"></a>创建桌面图标，每次创建都需要用户授权

使用此接口可以让玩家在游戏中创建桌面图标，每次创建都需要用户授权

```c#
     QG.InstallShortcut((success) =>
                        {Debug.Log("创建桌面图标成功 = " + JsonUtility.ToJson(success));},
                        (fail) =>
                        {Debug.Log("取消创建桌面图标 = " + JsonUtility.ToJson(fail));},
                        (complete) =>
                        {Debug.Log("QG.InstallShortcut complete = " + JsonUtility.ToJson(complete));}
                        );
```

## <a id="判断是否是桌面启动"></a>判断是否是桌面启动

使用此接口可以让玩家在游戏中判断是否是桌面启动

```c#
        QG.IsStartupByShortcut((success) =>
        {
            string titleNote = success.isStartupByShortcut ? "桌面" : "其他"; //isStartupByShortcut 桌面 true 其他 false
            Debug.Log($"启动方式: {titleNote}");
        },
            (fail) =>
            {
                Debug.Log("调用接口失败");
            });
```

## <a id="UnityEngine.PlayerPrefs"></a>Unity持久化

使用此接口可以让玩家在游戏中使用UnityEngine.PlayerPrefs

```c#
using UnityEngine;
using QGMiniGame; //需要引入OPPO小游戏库
    PlayerPrefs.SetInt("OPPO-Key-Int", 1);  //储存 int 类型
    PlayerPrefs.SetString("OPPO-Key-String", "OPPO-Value-String"); //储存 string 类型键值
    PlayerPrefs.SetFloat("OPPO-Key-Float", 2.333); //储存 float 类型

    int ValueInt = PlayerPrefs.GetInt("OPPO-Key-Int");  //获取 int 类型
    string ValueStr = PlayerPrefs.GetString("OPPO-Key-String"); //获取 string 类型键值
    float ValueFloat = PlayerPrefs.GetFloat("OPPO-Key-Float");  //获取 float 类型
    bool isHasKey = PlayerPrefs.HasKey("OPPO-Key-Int"); //是否存在Key
    PlayerPrefs.DeleteKey("OPPO-Key-Int");              //删除单个键值存储
    PlayerPrefs.DeleteAll();                            //清空存储中的所有键名
```

## <a id="监听OPPO小游戏切换前台后台事件"></a>监听OPPO小游戏切换前台后台事件

使用此接口可以让玩家在游戏中监听OPPO小游戏切换前台后台事件

```c#
        //监听 OPPO 小游戏回到前台的事件
        QG.OnShow((msg) =>
         {
             Debug.Log("QG.OnShow = " + JsonUtility.ToJson(msg));
             Query query = msg.query; //启动小游戏的 query 参数
             ReferrerInfo referrerInfo = msg.referrerInfo; //其他信息,包含跳转到该小游戏的应用带来的额外数据
             string bdsTmp = msg.query.bdsTmp; 
             string _ORNT_ = msg.query._ORNT_;
             ExtraData extraData = msg.referrerInfo;
         });

        //取消监听 OPPO 小游戏回到前台的事件 
        QG.OffShow((msg) =>
         {
             Debug.Log("QG.OffShow = " + JsonUtility.ToJson(msg));
         });   

        //监听 OPPO 小游戏隐藏到后台事件。锁屏、按 HOME 键退到桌面等操作会触发此事件。 
        QG.OnHide((msg) =>
         {
             Debug.Log("QG.OnHide = " + JsonUtility.ToJson(msg));
         });   

        //取消监听 OPPO 小游戏隐藏到后台事件。锁屏、按 HOME 键退到桌面、显示在聊天顶部等操作会触发此事件。 
        QG.OffHide((msg) =>
         {
             Debug.Log("QG.OffHide = " + JsonUtility.ToJson(msg));
         });                            
```

## <a id="监听账号登录及实名认证"></a>监听账号登录及实名认证

使用此接口可以让玩家在游戏中监听账号登录及实名认证

```c#
        //监控游戏中登录账号和实名制验证弹窗的显示
        QG.OnAuthDialogShow((msg) =>
         {
             Debug.Log("QG.OnAuthDialogShow = " + JsonUtility.ToJson(msg));
             int authType = msg.authType; // 弹窗类型：1-登录账号；2-实名认证
         });

        //取消监听游戏中登录账号和实名制验证弹窗的显示 
        QG.OffAuthDialogShow((msg) =>
         {
             Debug.Log("QG.OffAuthDialogShow = " + JsonUtility.ToJson(msg));
         });   

        //监听游戏中登录账号和实名验证弹窗的关闭
        QG.OnAuthDialogClose((msg) =>
         {
             Debug.Log("QG.OnAuthDialogClose = " + JsonUtility.ToJson(msg));
             int authType = msg.authType; // 弹窗类型：1-登录账号；2-实名认证
         });   
         
        //取消监听游戏中登录账号和实名验证弹窗的关闭 
        QG.OffAuthDialogClose((msg) =>
         {
             Debug.Log("QG.OffAuthDialogClose = " + JsonUtility.ToJson(msg));
         });                            
```

## <a id="自定义拓展"></a>自定义拓展

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
