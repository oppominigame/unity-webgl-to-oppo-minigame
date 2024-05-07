using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using QGMiniGame;
using UnityEngine;
using UnityEngine.SceneManagement;

// using System.Runtime.InteropServices;
public class main_test : MonoBehaviour
{
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void playQGLogin()
    {
        SceneManager.LoadScene("login");
    }

    public void playQGHasShortcutInstalled()
    {
        QG
            .HasShortcutInstalled((msg) =>
            {
                Debug
                    .Log("QG.HasShortcutInstalled success = " +
                    JsonUtility.ToJson(msg));
            },
            (msg) =>
            {
                Debug.Log("QG.HasShortcutInstalled fail = " + msg.errMsg);
            });
    }

    public void playQGInstallShortcut()
    {
        QG
            .InstallShortcut((msg) =>
            {
                Debug
                    .Log("QG.InstallShortcut success = " +
                    JsonUtility.ToJson(msg));
            },
            (msg) =>
            {
                Debug.Log("QG.InstallShortcut fail = " + msg.errMsg);
            });
    }

    public void playQGCreateBannerAd()
    {
        SceneManager.LoadScene("banner");
        // var bannerAd =
        //     QG
        //         .CreateBannerAd(new QGCreateBannerAdParam()
        //         { adUnitId = "114131" });
        // Debug.Log("创建Banner广告开始运行");
        // bannerAd
        //     .OnLoad(() =>
        //     {
        //         Debug.Log("banner广告加载成功");
        //     });
        // bannerAd
        //     .OnError((QGBaseResponse msg) =>
        //     {
        //         Debug
        //             .Log("QG.bannerAd.OnError success = " +
        //             JsonUtility.ToJson(msg));
        //     });
    }

    public void playQGCreateRewardedVideoAd()
    {
        SceneManager.LoadScene("rewardedVideo");
        // var rewardedVideoAd =
        //     QG
        //         .CreateRewardedVideoAd(new QGCommonAdParam()
        //         { adUnitId = "114183" });
        // Debug.Log("创建激励视频开始运行");
        // rewardedVideoAd
        //     .OnLoad(() =>
        //     {
        //         Debug.Log("激励视频广告加载成功");
        //         rewardedVideoAd.Show();
        //     });
        // rewardedVideoAd
        //     .OnError((QGBaseResponse msg) =>
        //     {
        //         Debug
        //             .Log("QG.rewardedVideoAd.OnError success = " +
        //             JsonUtility.ToJson(msg));
        //     });
        // rewardedVideoAd
        //     .OnClose((QGRewardedVideoResponse msg) =>
        //     {
        //         if (msg.isEnded)
        //         {
        //             Debug.Log("激励视频广告完成，发放奖励");
        //         }
        //         else
        //         {
        //             Debug.Log("激励视频广告取消关闭，不发放奖励");
        //         }
        //     });
    }

    public void playQGCreateInterstitialAd()
    {
        SceneManager.LoadScene("Interstitial");
        // var interstitialAd =
        //     QG
        //         .CreateInterstitialAd(new QGCommonAdParam()
        //         { adUnitId = "114187" });
        // Debug.Log("创建插屏广告开始运行");
        // interstitialAd
        //     .OnLoad(() =>
        //     {
        //         Debug.Log("插屏广告加载成功");
        //         interstitialAd.Show();
        //     });
        // interstitialAd
        //     .OnError((QGBaseResponse msg) =>
        //     {
        //         Debug
        //             .Log("QG.interstitialAd.OnError success = " +
        //             JsonUtility.ToJson(msg));
        //     });
    }

    public void playQGCreateCustomAd()
    {
        SceneManager.LoadScene("custom");
        // var customAd =
        //     QG
        //         .CreateCustomAd(new QGCreateCustomAdParam()
        //         {
        //             adUnitId = "399676" //上文下图
        //         });
        // Debug.Log("创建原生模板广告开始运行");
        // customAd
        //     .OnLoad(() =>
        //     {
        //         Debug.Log("原生模板广告加载成功");
        //     });
        // customAd
        //     .Show((msg) =>
        //     {
        //         Debug.Log("原生模板广告展示成功 = " + JsonUtility.ToJson(msg));
        //     },
        //     (msg) =>
        //     {
        //         Debug.Log("原生模板广告展示失败 = " + msg.errMsg);
        //     });
        // customAd
        //     .OnError((QGBaseResponse msg) =>
        //     {
        //         Debug
        //             .Log("QG.customAd.OnError success = " +
        //             JsonUtility.ToJson(msg));
        //     });
        // customAd
        //     .OnHide(() =>
        //     {
        //         Debug.Log("QG.customAd.OnHide success ");
        //     });
    }

    public void playQGCreateGameBannerAd()
    {
        SceneManager.LoadScene("gameBanner");
        // var gameBannerAd =
        //     QG
        //         .CreateGameBannerAd(new QGCommonAdParam()
        //         { adUnitId = "201139" });
        // Debug.Log("创建互推盒子横幅广告开始运行");
        // gameBannerAd
        //     .OnLoad(() =>
        //     {
        //         Debug.Log("QG.gameBannerAd.OnLoad success = ");
        //         gameBannerAd
        //             .Show((msg) =>
        //             {
        //                 Debug
        //                     .Log("互推盒子横幅广告展示成功 = " +
        //                     JsonUtility.ToJson(msg));
        //             },
        //             (msg) =>
        //             {
        //                 Debug.Log("互推盒子横幅广告展示失败 = " + msg.errMsg);
        //             });
        //     });
        // gameBannerAd
        //     .OnError((QGBaseResponse msg) =>
        //     {
        //         Debug
        //             .Log("QG.gameBannerAd.OnError success = " +
        //             JsonUtility.ToJson(msg));
        //     });
    }

    public void playQGCreateGamePortalAd()
    {
        SceneManager.LoadScene("gamePortal");
        // var gamePortalAd =
        //     QG
        //         .CreateGamePortalAd(new QGCommonAdParam()
        //         { adUnitId = "201138" });
        // Debug.Log("创建互推盒子横幅广告开始运行");
        // gamePortalAd
        //     .OnLoad(() =>
        //     {
        //         gamePortalAd
        //             .Show((msg) =>
        //             {
        //                 Debug
        //                     .Log("互推盒子九宫格广告展示成功 = " +
        //                     JsonUtility.ToJson(msg));
        //             },
        //             (msg) =>
        //             {
        //                 Debug.Log("互推盒子九宫格广告展示失败 = " + msg.errMsg);
        //             });
        //     });
        // gamePortalAd
        //     .OnError((QGBaseResponse msg) =>
        //     {
        //         Debug
        //             .Log("QG.gamePortalAd.OnError success = " +
        //             JsonUtility.ToJson(msg));
        //     });
    }

    public void playQGCreateGameDrawerAd()
    {
        SceneManager.LoadScene("gameDrawer");
        // var GameDrawerAd =
        //     QG
        //         .CreateGameDrawerAd(new QGCreateGameDrawerAdParam()
        //         { adUnitId = "336614" });
        // Debug.Log("创建互推盒子抽屉广告开始运行");
        // GameDrawerAd
        //     .Show((msg) =>
        //     {
        //         Debug
        //             .Log("互推盒子抽屉广告展示成功 = " +
        //             JsonUtility.ToJson(msg));
        //     },
        //     (msg) =>
        //     {
        //         Debug.Log("互推盒子抽屉广告展示失败 = " + msg.errMsg);
        //     });
        // GameDrawerAd
        //     .OnError((QGBaseResponse msg) =>
        //     {
        //         Debug
        //             .Log("QG.gamePortalAd.OnError success = " +
        //             JsonUtility.ToJson(msg));
        //     });
    }

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
                             appId = 30173650,
                             token = parameterToken,
                             payUrl = "https://jits.open.oppomobile.com/jitsopen/api/pay/demo/preOrder",  //测试接口
                            //  payUrl =  "https://jits.open.oppomobile.com/jitsopen/api/pay/v1.0/preOrder", //正式接口
                             productName = "测试礼包",
                             productDesc = "测试支付",
                             count = 1, //商品数量（只能传1） 
                             price = 1, //商品价格，以分为单位
                             currency = "CNY", //币种，人民币如：CNY
                             callBackUrl = "", // 服务器接收平台返回数据的接口回调地址
                             cpOrderId = "1.0", //CP自己的订单号
                             appVersion = "1.0.0", //游戏版本
                             deviceInfo = "", //设备号 
                             //model = "", //机型 
                             ip = "", //终端IP 
                             attach = ""//附加信息 
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

    public void playQGStorageSetItem()
    {
        SceneManager.LoadScene("StorageItem");
        //QG.StorageSetItem("miniGame", "test");
        //Debug.Log("数据存储");
    }

    public void playQGStorageGetItem()
    {
        QG.StorageGetItem("miniGame");
        Debug.Log("数据读取");
    }

    public void playQGStorageRemoveItem()
    {
        QG.StorageRemoveItem("miniGame");
        Debug.Log("删除数据");
    }

    public void playAudio()
    {
        SceneManager.LoadScene("audio");
        Debug.Log("打开音频");
    }
}
