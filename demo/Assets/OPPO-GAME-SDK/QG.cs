using System;
using UnityEngine;


namespace QGMiniGame
{
    public class QG
    {
        public static int SDK_VERSION = 5;

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

        #region HasShortcutInstalled  获取创建桌面图标是否创建
        // https://ie-activity-cn.heytapimage.com/static/minigame/CN/docs/index.html#/develop/feature/install-shortcut
        //QG.HasShortcutInstalled(
        //(msg) => { Debug.Log("QG.HasShortcutInstalled success = " + JsonUtility.ToJson(msg)); },
        //(msg) => { Debug.Log("QG.HasShortcutInstalled fail = " + msg.errMsg); }
        //);
        public static void HasShortcutInstalled(Action<QGCommonResponse<QGShortcutBean>> succCallback = null, Action<QGCommonResponse<QGShortcutBean>> failCallback = null)
        {
            QGMiniGameManager.Instance.HasShortcutInstalled(succCallback, failCallback);
        }
        #endregion

        #region InstallShortcut  创建桌面图标
        // https://ie-activity-cn.heytapimage.com/static/minigame/CN/docs/index.html#/develop/feature/install-shortcut
        //QG.InstallShortcut(
        //"我来自Unity",
        //(msg) => { Debug.Log("QG.InstallShortcut success = " + JsonUtility.ToJson(msg)); },
        //(msg) => { Debug.Log("QG.InstallShortcut fail = " + msg.errMsg); }
        //);
        public static void InstallShortcut(Action<QGBaseResponse> succCallback = null, Action<QGBaseResponse> failCallback = null)
        {
            QGMiniGameManager.Instance.InstallShortcut(succCallback, failCallback);
        }
        #endregion

        #region CreateBannerAd  创建Banner广告
        // https://ie-activity-cn.heytapimage.com/static/minigame/CN/docs/index.html#/develop/ad/banner-ad
        // var bannerAd = QG.CreateBannerAd(new QGCreateBannerAdParam()
        // {
        // adUnitId = "xxxxxx"
        // });
        // Debug.Log("创建Banner广告开始运行");
        // bannerAd.OnLoad(() => {
        //     Debug.Log("banner广告加载成功");
        // });
        // bannerAd.OnError((QGBaseResponse msg) =>
        // {
        // Debug.Log("QG.bannerAd.OnError success = " + JsonUtility.ToJson(msg));
        // });
        public static QGBannerAd CreateBannerAd(QGCreateBannerAdParam param)
        {
            return QGMiniGameManager.Instance.CreateBannerAd(param);
        }
        #endregion

        #region CreateRewardedVideoAd  创建激励视频广告
        // https://ie-activity-cn.heytapimage.com/static/minigame/CN/docs/index.html#/develop/ad/video-ad
        // var rewardedVideoAd = QG.CreateRewardedVideoAd(new QGCommonAdParam()
        // {
        // adUnitId = "xxxxxx"
        // });
        // Debug.Log("创建激励视频开始运行");
        // rewardedVideoAd.OnLoad(() => {
        //   Debug.Log("激励视频广告加载成功");
        // rewardedVideoAd.Show();
        // });
        // rewardedVideoAd.OnError((QGBaseResponse msg) =>
        // {
        // Debug.Log("QG.rewardedVideoAd.OnError success = " + JsonUtility.ToJson(msg));
        // });
        // rewardedVideoAd.OnClose((QGRewardedVideoResponse msg) =>
        // {
        // if (msg.isEnded) {
        //       Debug.Log("激励视频广告完成，发放奖励");
        //     } else {
        //       Debug.Log("激励视频广告取消关闭，不发放奖励");
        //     }
        // });
        public static QGRewardedVideoAd CreateRewardedVideoAd(QGCommonAdParam param)
        {
            return QGMiniGameManager.Instance.CreateRewardedVideoAd(param);
        }
        #endregion

        #region CreateInterstitialAd  创建插屏广告
        // https://ie-activity-cn.heytapimage.com/static/minigame/CN/docs/index.html#/develop/ad/insert-ad
        // var interstitialAd = QG.CreateInterstitialAd(new QGCommonAdParam()
        // {
        // adUnitId = "xxxxxx"
        // });
        // Debug.Log("创建插屏广告开始运行");
        // interstitialAd.OnLoad(() => {
        //     Debug.Log("插屏广告加载成功");
        //     interstitialAd.Show();
        // });
        // interstitialAd.OnError((QGBaseResponse msg) =>
        // {
        // Debug.Log("QG.interstitialAd.OnError success = " + JsonUtility.ToJson(msg));
        // });
        public static QGInterstitialAd CreateInterstitialAd(QGCommonAdParam param)
        {
            return QGMiniGameManager.Instance.CreateInterstitialAd(param);
        }
        #endregion

        #region CreateCustomAd  创建原生模板广告
        // https://ie-activity-cn.heytapimage.com/static/minigame/CN/docs/index.html#/develop/ad/native-template-ad
        // var customAd = QG.CreateCustomAd(new QGCreateCustomAdParam()
        // {
        // adUnitId = "xxxxxx"  //上文下图
        // });
        // Debug.Log("创建原生模板广告开始运行");
        // customAd.OnLoad(() => {
        // Debug.Log("原生模板广告加载成功");
        // });
        // customAd.Show(
        // (msg) => { Debug.Log("原生模板广告展示成功 = " + JsonUtility.ToJson(msg)); },
        // (msg) => { Debug.Log("原生模板广告展示失败 = " + msg.errMsg); }
        // );
        // customAd.OnError((QGBaseResponse msg) =>
        // {
        // Debug.Log("QG.customAd.OnError success = " + JsonUtility.ToJson(msg));
        // });
        // customAd.OnHide(() =>
        // {
        // Debug.Log("QG.customAd.OnHide success ");
        // });
        public static QGCustomAd CreateCustomAd(QGCreateCustomAdParam param)
        {
            return QGMiniGameManager.Instance.CreateCustomAd(param);
        }
        #endregion

        #region CreateGameBannerAd    创建互推盒子横幅广告
        // https://ie-activity-cn.heytapimage.com/static/minigame/CN/docs/index.html#/develop/ad/mutual-push-box-ad?id=qgcreategamebanneradobject
        // var gameBannerAd = QG.CreateGameBannerAd(new QGCommonAdParam()
        // {
        // adUnitId = "xxxxxx"
        // });
        // Debug.Log("创建互推盒子横幅广告开始运行");
        // gameBannerAd.OnLoad(() => {
        // Debug.Log("QG.gameBannerAd.OnLoad success = ");
        // gameBannerAd.Show(
        // (msg) => { Debug.Log("互推盒子横幅广告展示成功 = " + JsonUtility.ToJson(msg)); },
        // (msg) => { Debug.Log("互推盒子横幅广告展示失败 = " + msg.errMsg); }
        // );
        // });
        // gameBannerAd.OnError((QGBaseResponse msg) =>
        // {
        // Debug.Log("QG.gameBannerAd.OnError success = " + JsonUtility.ToJson(msg));
        // });
        public static QGGameBannerAd CreateGameBannerAd(QGCommonAdParam param)
        {
            return QGMiniGameManager.Instance.CreateGameBannerAd(param);
        }
        #endregion

        #region CreateGamePortalAd  创建互推盒子九宫格广告
        // https://ie-activity-cn.heytapimage.com/static/minigame/CN/docs/index.html#/develop/ad/mutual-push-box-ad?id=qgcreategameportaladobject
        // var gamePortalAd = QG.CreateGamePortalAd(new QGCommonAdParam()
        // {
        // adUnitId = "xxxxxx"
        // });
        // Debug.Log("创建互推盒子横幅广告开始运行");
        // gamePortalAd.OnLoad(() => {
        // gamePortalAd.Show(
        // (msg) => { Debug.Log("互推盒子九宫格广告展示成功 = " + JsonUtility.ToJson(msg)); },
        // (msg) => { Debug.Log("互推盒子九宫格广告展示失败 = " + msg.errMsg); }
        // );
        // });
        // gamePortalAd.OnError((QGBaseResponse msg) =>
        // {
        // Debug.Log("QG.gamePortalAd.OnError success = " + JsonUtility.ToJson(msg));
        // });
        public static QGGamePortalAd  CreateGamePortalAd(QGCommonAdParam param)
        {
            return QGMiniGameManager.Instance.CreateGamePortalAd(param);
        }
        #endregion

        #region CreateGameDrawerAd  创建互推盒子抽屉广告
        // https://ie-activity-cn.heytapimage.com/static/minigame/CN/docs/index.html#/develop/ad/mutual-push-box-ad?id=qgcreategamedraweradobject
        // var GameDrawerAd  = QG.CreateGameDrawerAd(new QGCreateGameDrawerAdParam()
        // {
        // adUnitId = "xxxxxx"
        // });
        // Debug.Log("创建互推盒子抽屉广告开始运行");
        // GameDrawerAd.Show(
        // (msg) => { Debug.Log("互推盒子抽屉广告展示成功 = " + JsonUtility.ToJson(msg)); },
        // (msg) => { Debug.Log("互推盒子抽屉广告展示失败 = " + msg.errMsg); }
        // );
        // GameDrawerAd.OnError((QGBaseResponse msg) =>
        // {
        // Debug.Log("QG.gamePortalAd.OnError success = " + JsonUtility.ToJson(msg));
        // });
        public static QGGameDrawerAd  CreateGameDrawerAd(QGCreateGameDrawerAdParam param)
        {
            return QGMiniGameManager.Instance.CreateGameDrawerAd(param);
        }
        #endregion

        #region storage 
        // 数据存储  
        public static void StorageSetItem(string keyName, string keyValue)
        {
            QGMiniGameManager.StorageSetItem(keyName, keyValue);
        }
        // 数据读取  
        public static void StorageGetItem(string keyName)
        {
            QGMiniGameManager.StorageGetItem(keyName);
        }
        // 清除数据 
        public static void StorageRemoveItem(string keyName)
        {
            QGMiniGameManager.StorageRemoveItem(keyName);
        }
        #endregion



        #region 覆盖unity的PlayerPrefs

        public static void StorageSetIntSync(string key, int value)
        {
            QGMiniGameManager.Instance.StorageSetIntSync(key, value);
        }

        public static int StorageGetIntSync(string key, int defaultValue)
        {
            return QGMiniGameManager.Instance.StorageGetIntSync(key, defaultValue);
        }

        public static void StorageSetStringSync(string key, string value)
        {
            QGMiniGameManager.Instance.StorageSetStringSync(key, value);
        }

        public static string StorageGetStringSync(string key, string defaultValue)
        {
            return QGMiniGameManager.Instance.StorageGetStringSync(key, defaultValue);
        }

        public static void StorageSetFloatSync(string key, float value)
        {
            QGMiniGameManager.Instance.StorageSetFloatSync(key, value);
        }

        public static float StorageGetFloatSync(string key, float defaultValue)
        {
            return QGMiniGameManager.Instance.StorageGetFloatSync(key, defaultValue);
        }

        public static void StorageDeleteAllSync()
        {
            QGMiniGameManager.Instance.StorageDeleteAllSync();
        }

        public static void StorageDeleteKeySync(string key)
        {
            QGMiniGameManager.Instance.StorageDeleteKeySync(key);
        }

        public static bool StorageHasKeySync(string key)
        {
            return QGMiniGameManager.Instance.StorageHasKeySync(key);
        }

        #endregion

        #region Pay 支付
        // https://ie-activity-cn.heytapimage.com/static/minigame/CN/docs/index.html#/develop/pay/pay-introduction
        // https://ie-activity-cn.heytapimage.com/static/minigame/CN/docs/index.html#/develop/pay/pay
        // PayParam param = new PayParam()
        // {
        //     appId = "123", 
        //     token = "xxxxxxxxxxxxxxxxxxxx",
        //     timestamp = 1682244531643,  
        //     orderNo = "111111",
        //     paySign = "xxxxxxxxxxxxxxxxxxxx",
        //     // paySign 由 CP 服务端使用 appKey (不是 appId )、orderNo、timestamp 进行签名算法生成返回
        // };
        // QG.Pay(
        //     param,
        //     (msg) => { Debug.Log("QG.Pay success = " + JsonUtility.ToJson(msg)); },
        //     (msg) => { Debug.Log("QG.Pay fail = " + JsonUtility.ToJson(msg)); },
        //     (msg) => { Debug.Log("QG.Pay complete = " + JsonUtility.ToJson(msg)); }
        // );
        public static void Pay(PayParam param, Action<QGCommonResponse<QGPayBean>> successCallback = null, Action<QGCommonResponse<QGPayBean>> failCallback = null,Action<QGCommonResponse<QGPayBean>> completeCallback = null)
        {
            QGMiniGameManager.Instance.Pay(param, successCallback, failCallback, completeCallback);
        }
        #endregion

        #region AccessFile 判断文件是否存在 
        public static string AccessFile(QGAccessFileParam param)
        {
            return QGMiniGameManager.Instance.AccessFile(param);
        }
        #endregion

        #region ReadFile 读取文件 
        /*QGFileParam param = new QGFileParam()
        {
            uri = "internal://files/hehe.txt",
            encoding = "utf8"
        };
        QG.ReadFile(
            param,
           (msg) => { Debug.Log("QG.ReadFile success = " + JsonUtility.ToJson(msg)); },
               (msg) => { Debug.Log("QG.ReadFile fail = " + JsonUtility.ToJson(msg)); }
        );*/
        public static void ReadFile(QGFileParam param, Action<QGFileResponse> successCallback = null, Action<QGFileResponse> failCallback = null)
        {
            QGMiniGameManager.Instance.ReadFile(param, successCallback, failCallback);
        }
        #endregion

        #region WriteFile 写入文件
        /*QGFileParam param = new QGFileParam()
        {
            uri = "internal://files/hehe.txt",
            encoding = "utf8",
            textStr = "你是谁啊 "
        };
        QG.WriteFile(
            param,
           (msg) => { Debug.Log("QG.WriteFile success = " + JsonUtility.ToJson(msg)); },
               (msg) => { Debug.Log("QG.WriteFile fail = " + JsonUtility.ToJson(msg)); }
        );*/
        public static void WriteFile(QGFileParam param, Action<QGFileResponse> successCallback = null, Action<QGFileResponse> failCallback = null)
        {
            QGMiniGameManager.Instance.WriteFile(param, successCallback, failCallback);
        }
        #endregion
    }
}

