using System;
using System.Collections;
using System.Collections.Generic;
using QGMiniGame;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
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

        // 2、获取桌面图标是否创建
        QG.HasShortcutInstalled(
          (msg) => { Debug.Log("QG.HasShortcutInstalled success = " + JsonUtility.ToJson(msg)); },
          (msg) => { Debug.Log("QG.HasShortcutInstalled fail = " + msg.errMsg); }
        );

        // 3、创建桌面图标
        // QG.InstallShortcut(
        //   (msg) => { Debug.Log("QG.InstallShortcut success = " + JsonUtility.ToJson(msg)); },
        //   (msg) => { Debug.Log("QG.InstallShortcut fail = " + msg.errMsg); }
        // );

        // 4、创建Banner广告
        var bannerAd = QG.CreateBannerAd(new QGCreateBannerAdParam()
          {
          adUnitId = "114131"
          });
          Debug.Log("创建Banner广告开始运行");
          bannerAd.OnLoad(() => {
              Debug.Log("banner广告加载成功");
          });
          bannerAd.OnError((QGBaseResponse msg) =>
          {
          Debug.Log("QG.bannerAd.OnError success = " + JsonUtility.ToJson(msg));
          });

        // 5、创建激励视频广告
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

        // 6、创建插屏广告
        var interstitialAd = QG.CreateInterstitialAd(new QGCommonAdParam()
          {
          adUnitId = "114187"
          });
          Debug.Log("创建插屏广告开始运行");
          interstitialAd.OnLoad(() => {
              Debug.Log("插屏广告加载成功");
              interstitialAd.Show();
          });
          interstitialAd.OnError((QGBaseResponse msg) =>
          {
          Debug.Log("QG.interstitialAd.OnError success = " + JsonUtility.ToJson(msg));
          });

        // 7、创建原生模板广告
        var customAd = QG.CreateCustomAd(new QGCreateCustomAdParam()
          {
          adUnitId = "399676"  //上文下图
          });
          Debug.Log("创建原生模板广告开始运行");
          customAd.OnLoad(() => {
          Debug.Log("原生模板广告加载成功");
          });
          customAd.Show(
          (msg) => { Debug.Log("原生模板广告展示成功 = " + JsonUtility.ToJson(msg)); },
          (msg) => { Debug.Log("原生模板广告展示失败 = " + msg.errMsg); }
          );
          customAd.OnError((QGBaseResponse msg) =>
          {
          Debug.Log("QG.customAd.OnError success = " + JsonUtility.ToJson(msg));
          });
          customAd.OnHide(() =>
          {
          Debug.Log("QG.customAd.OnHide success ");
          });
        // 8、创建互推盒子横幅广告
        var gameBannerAd = QG.CreateGameBannerAd(new QGCommonAdParam()
          {
          adUnitId = "201139"
          });
          Debug.Log("创建互推盒子横幅广告开始运行");
          gameBannerAd.OnLoad(() => {
          Debug.Log("QG.gameBannerAd.OnLoad success = ");
          gameBannerAd.Show(
          (msg) => { Debug.Log("互推盒子横幅广告展示成功 = " + JsonUtility.ToJson(msg)); },
          (msg) => { Debug.Log("互推盒子横幅广告展示失败 = " + msg.errMsg); }
          );
          });
          gameBannerAd.OnError((QGBaseResponse msg) =>
          {
          Debug.Log("QG.gameBannerAd.OnError success = " + JsonUtility.ToJson(msg));
          });

        // 9、创建互推盒子九宫格广告
        var gamePortalAd = QG.CreateGamePortalAd(new QGCommonAdParam()
          {
          adUnitId = "201138"
          });
          Debug.Log("创建互推盒子横幅广告开始运行");
          gamePortalAd.OnLoad(() => {
          gamePortalAd.Show(
          (msg) => { Debug.Log("互推盒子九宫格广告展示成功 = " + JsonUtility.ToJson(msg)); },
          (msg) => { Debug.Log("互推盒子九宫格广告展示失败 = " + msg.errMsg); }
          );
          });
          gamePortalAd.OnError((QGBaseResponse msg) =>
          {
          Debug.Log("QG.gamePortalAd.OnError success = " + JsonUtility.ToJson(msg));
          });
        // 10、创建互推盒子抽屉广告
        var GameDrawerAd  = QG.CreateGameDrawerAd(new QGCreateGameDrawerAdParam()
          {
          adUnitId = "336614"
          });
          Debug.Log("创建互推盒子抽屉广告开始运行");
          GameDrawerAd.Show(
          (msg) => { Debug.Log("互推盒子抽屉广告展示成功 = " + JsonUtility.ToJson(msg)); },
          (msg) => { Debug.Log("互推盒子抽屉广告展示失败 = " + msg.errMsg); }
          );
          GameDrawerAd.OnError((QGBaseResponse msg) =>
          {
          Debug.Log("QG.gamePortalAd.OnError success = " + JsonUtility.ToJson(msg));
          });

        // 13、Pay 支付
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

        QG.StorageSetItem("miniGame", "test");
        Debug.Log("数据存储");

        QG.StorageGetItem("miniGame");
        Debug.Log("数据读取");
        
         QG.StorageRemoveItem("miniGame");
        Debug.Log("删除数据");
    }

    // Update is called once per frame
    void Update()
    {
    }
}
