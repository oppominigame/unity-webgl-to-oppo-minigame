using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using QGMiniGame;
using UnityEngine;
using UnityEngine.UI;

// using System.Runtime.InteropServices;
public class main_test : MonoBehaviour
{
    // public Text logText;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void playQGLogin()
    {
        QG
            .Login((msg) =>
            {
                Debug.Log("QG.Login success = " + JsonUtility.ToJson(msg));
            },
            (msg) =>
            {
                Debug.Log("QG.Login fail = " + msg.errMsg);
            });
    }

    public void playQGHasShortcutInstalled()
    {
        QG
            .HasShortcutInstalled((msg) =>
            {
                Debug
                    .Log("调起创建桌面图标弹窗成功" + JsonUtility.ToJson(msg));
            },
            (msg) =>
            {
                Debug.Log("调起创建桌面图标弹窗失败" + JsonUtility.ToJson(msg));
            });
    }

    // public void playQGInstallShortcut()
    // {
    //     QG
    //         .InstallShortcut((msg) =>
    //         {
    //             Debug
    //                 .Log("QG.InstallShortcut success" +
    //                 JsonUtility.ToJson(msg));
    //         },
    //         (msg) =>
    //         {
    //             Debug.Log("QG.InstallShortcut fail" + msg.errMsg);
    //         });
    // }
    public void playQGCreateBannerAd()
    {
        var bannerAd =
            QG
                .CreateBannerAd(new QGCreateBannerAdParam()
                { adUnitId = "114131" });
        Debug.Log("创建Banner广告开始运行");
        bannerAd
            .OnLoad(() =>
            {
                Debug.Log("banner广告加载成功");
            });
        bannerAd
            .OnError((QGBaseResponse msg) =>
            {
                Debug
                    .Log("QG.bannerAd.OnError success = " +
                    JsonUtility.ToJson(msg));
            });
    }

    public void playQGCreateRewardedVideoAd()
    {
        var rewardedVideoAd =
            QG
                .CreateRewardedVideoAd(new QGCommonAdParam()
                { adUnitId = "114183" });
        Debug.Log("创建激励视频开始运行");
        rewardedVideoAd
            .OnLoad(() =>
            {
                Debug.Log("激励视频广告加载成功");
                rewardedVideoAd.Show();
            });
        rewardedVideoAd
            .OnError((QGBaseResponse msg) =>
            {
                Debug
                    .Log("QG.rewardedVideoAd.OnError success = " +
                    JsonUtility.ToJson(msg));
            });
        rewardedVideoAd
            .OnClose((QGRewardedVideoResponse msg) =>
            {
                if (msg.isEnded)
                {
                    Debug.Log("激励视频广告完成，发放奖励");
                }
                else
                {
                    Debug.Log("激励视频广告取消关闭，不发放奖励");
                }
            });
    }

    public void playQGCreateInterstitialAd()
    {
        var interstitialAd =
            QG
                .CreateInterstitialAd(new QGCommonAdParam()
                { adUnitId = "114187" });
        Debug.Log("创建插屏广告开始运行");
        interstitialAd
            .OnLoad(() =>
            {
                Debug.Log("插屏广告加载成功");
                interstitialAd.Show();
            });
        interstitialAd
            .OnError((QGBaseResponse msg) =>
            {
                Debug
                    .Log("QG.interstitialAd.OnError success = " +
                    JsonUtility.ToJson(msg));
            });
    }

    public void playQGCreateCustomAd()
    {
        var customAd =
            QG
                .CreateCustomAd(new QGCreateCustomAdParam()
                {
                    adUnitId = "399676" //上文下图
                });
        Debug.Log("创建原生模板广告开始运行");
        customAd
            .OnLoad(() =>
            {
                Debug.Log("原生模板广告加载成功");
            });
        customAd
            .Show((msg) =>
            {
                Debug.Log("原生模板广告展示成功 = " + JsonUtility.ToJson(msg));
            },
            (msg) =>
            {
                Debug.Log("原生模板广告展示失败 = " + msg.errMsg);
            });
        customAd
            .OnError((QGBaseResponse msg) =>
            {
                Debug
                    .Log("QG.customAd.OnError success = " +
                    JsonUtility.ToJson(msg));
            });
        customAd
            .OnHide(() =>
            {
                Debug.Log("QG.customAd.OnHide success ");
            });
    }

    public void playQGCreateGameBannerAd()
    {
        var gameBannerAd =
            QG
                .CreateGameBannerAd(new QGCommonAdParam()
                { adUnitId = "201139" });
        Debug.Log("创建互推盒子横幅广告开始运行");
        gameBannerAd
            .OnLoad(() =>
            {
                Debug.Log("QG.gameBannerAd.OnLoad success = ");
                gameBannerAd
                    .Show((msg) =>
                    {
                        Debug
                            .Log("互推盒子横幅广告展示成功 = " +
                            JsonUtility.ToJson(msg));
                    },
                    (msg) =>
                    {
                        Debug.Log("互推盒子横幅广告展示失败 = " + msg.errMsg);
                    });
            });
        gameBannerAd
            .OnError((QGBaseResponse msg) =>
            {
                Debug
                    .Log("QG.gameBannerAd.OnError success = " +
                    JsonUtility.ToJson(msg));
            });
    }

    public void playQGCreateGamePortalAd()
    {
        var gamePortalAd =
            QG
                .CreateGamePortalAd(new QGCommonAdParam()
                { adUnitId = "201138" });
        Debug.Log("创建互推盒子横幅广告开始运行");
        gamePortalAd
            .OnLoad(() =>
            {
                gamePortalAd
                    .Show((msg) =>
                    {
                        Debug
                            .Log("互推盒子九宫格广告展示成功 = " +
                            JsonUtility.ToJson(msg));
                    },
                    (msg) =>
                    {
                        Debug.Log("互推盒子九宫格广告展示失败 = " + msg.errMsg);
                    });
            });
        gamePortalAd
            .OnError((QGBaseResponse msg) =>
            {
                Debug
                    .Log("QG.gamePortalAd.OnError success = " +
                    JsonUtility.ToJson(msg));
            });
    }

    public void playQGCreateGameDrawerAd()
    {
        var GameDrawerAd =
            QG
                .CreateGameDrawerAd(new QGCreateGameDrawerAdParam()
                { adUnitId = "336614" });
        Debug.Log("创建互推盒子抽屉广告开始运行");
        GameDrawerAd
            .Show((msg) =>
            {
                Debug
                    .Log("互推盒子抽屉广告展示成功 = " +
                    JsonUtility.ToJson(msg));
            },
            (msg) =>
            {
                Debug.Log("互推盒子抽屉广告展示失败 = " + msg.errMsg);
            });
        GameDrawerAd
            .OnError((QGBaseResponse msg) =>
            {
                Debug
                    .Log("QG.gamePortalAd.OnError success = " +
                    JsonUtility.ToJson(msg));
            });
    }

    public void playQGStorageSetItem()
    {
        QG.StorageSetItem("miniGame", "test");
        Debug.Log("数据存储");
    }

    public void playQGStorageGetItem()
    {
        Debug.Log("数据读取" + QG.StorageGetItem("miniGame"));
    }

    public void playQGStorageRemoveItem()
    {
        QG.StorageRemoveItem("miniGame");
        Debug.Log("删除数据");
    }

    // 网络状态  获取网络类型  监听网络状态
    public void playQGGetNetworkType()
    {
        QG
            .GetNetworkType((msg) =>
            {
                Debug.Log("获取网络状态：" + JsonUtility.ToJson(msg));
            },
            (msg) =>
            {
                Debug.Log("获取网络状态失败：" + JsonUtility.ToJson(msg));
            });
    }

    public void playQGOnNetworkStatusChange()
    {
      QG.OnNetworkStatusChange((msg) =>
            {
                Debug.Log("监听网络状态：" + JsonUtility.ToJson(msg));
            });
    }

    // 振动  长振动  短振动
    public void playQGVibrateShort()
    {
        QG.VibrateShort();
        Debug.Log("短振动调用");
    }

    public void playQGVibrateLong()
    {
        QG.VibrateLong();
        Debug.Log("长振动调用");
    }

    /**
    
    **/
    // 系统事件
    // 播放音频
    public void playQGAudio()
    {
        QG.PlayAudio();
        Debug.Log("播放音频");
    }

    public void playQGPauseAudio()
    {
        QG.PauseAudio();
        Debug.Log("暂停音频");
    }

    public void playQGOnAudioInterruptionBegin()
    {
        QG.OnAudioInterruptionBegin();
        Debug
            .Log("监听音频因为受到系统占用而被中断开始，以下场景会触发此事件：电话、音视频播放等。此事件触发后，OPPO 小游戏内所有音频会暂停。调用");
    }

    public void playQGOffAudioInterruptionBegin()
    {
        QG.OffAudioInterruptionBegin();
        Debug.Log("取消监听 qg.onAudioInterruptionBegin");
    }

    public void playQGOnAudioInterruptionEnd()
    {
        QG.OnAudioInterruptionEnd();
        Debug
            .Log("监听音频中断结束，在收到 onAudioInterruptionBegin 事件之后，OPPO 小游戏内所有音频会暂停，收到此事件之后才可再次播放成功。调用");
    }

    public void playQGOffAudioInterruptionEnd()
    {
        QG.OffAudioInterruptionEnd();
        Debug.Log("取消监听 qg.onAudioInterruptionEnd");
    }

    public void playQGOnError()
    {
        QG.OnError();
        Debug.Log("监听全局错误事件调用");
    }

    public void playQGOffError()
    {
        QG.OffError();
        Debug.Log("取消监听全局错误事件调用");
    }

    public void playQGDispatchError()
    {
        QG.DispatchError();
        Debug.Log("模拟触发Error调用");
    }

    // 系统信息
    // 异步
    public void playQGGetSystemInfo()
    {
        QG
            .GetSystemInfo((msg) =>
            {
                Debug.Log("异步获取系统信息成功：" + JsonUtility.ToJson(msg));
            },
            (msg) =>
            {
                Debug.Log("异步获取系统信息失败：" + JsonUtility.ToJson(msg));
            });
    }

    // 同步
    public void playQGGetSystemInfoSync()
    {
        Debug.Log("同步获取系统信息成功：" + QG.GetSystemInfoSync());
    }

    //  vConsole
    public void playQGSetEnableDebugTrue()
    {
        QG.SetEnableDebugTrue();
        Debug.Log("vConsole打开");
    }

    public void playQGSetEnableDebugFalse()
    {
        QG.SetEnableDebugFalse();
        Debug.Log("vConsole关闭");
    }

    // 文件系统
    public void playQGMkdir()
    {
        QG.Mkdir();
        Debug.Log("创建目录");
    }

    public void playQGRmdir()
    {
        QG.Rmdir();
        Debug.Log("删除目录");
    }

    public void playQGIsExist()
    {
        QG.IsExist();
        Debug.Log("是否是目录/文件");
    }

    public void playQGRename()
    {
        QG.Rename();
        Debug.Log("重命名目录");
    }

    public void playQGSaveFile()
    {
        QG.SaveFile();
        Debug.Log("保存临时文件到本地");
    }

    public void playQGReadDir()
    {
        QG.ReadDir();
        Debug.Log("读取目录内文件列表");
    }

    public void playQGWriteFile()
    {
        QG.WriteFile();
        Debug.Log("写入文件");
    }

    public void playQGReadFile()
    {
        QG.ReadFile();
        Debug.Log("读取文件");
    }

    public void playQGAppendFile()
    {
        QG.AppendFile();
        Debug.Log("追加文件");
    }

    public void playQGCopyFile()
    {
        QG.CopyFile();
        Debug.Log("复制文件");
    }

    public void playQGRemoveSavedFile()
    {
        QG.RemoveSavedFile();
        Debug.Log("删除文件");
    }

    public void playQGStat()
    {
        QG.Stat();
        Debug.Log("获取文件信息");
    }

    public void playQGUnzip()
    {
        QG.Unzip();
        Debug.Log("解压文件");
    }

    public void playQGGetFileInfo()
    {
        QG.GetFileInfo();
        Debug.Log("获取本地临时文件或本地用户文件的文件信息");
    }
}
