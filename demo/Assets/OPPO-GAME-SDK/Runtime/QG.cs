using System;
using UnityEngine;


namespace QGMiniGame
{
    public class QG
    {
        public static int SDK_VERSION = 5;
        public static UserCloudStorageParam userCloudStorageParam = new UserCloudStorageParam();
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
        public static void HasShortcutInstalled(Action<QGHasShortcutInstalledBean> succCallback = null, Action<QGBaseResponse> failCallback = null, Action<QGBaseResponse> completeCallback = null)
        {
            QGMiniGameManager.Instance.HasShortcutInstalled(succCallback, failCallback, completeCallback);
        }
        #endregion

        // #region InstallShortcut  创建桌面图标
        // // https://ie-activity-cn.heytapimage.com/static/minigame/CN/docs/index.html#/develop/feature/install-shortcut
        // //QG.InstallShortcut(
        // //"我来自Unity",
        // //(msg) => { Debug.Log("QG.InstallShortcut success = " + JsonUtility.ToJson(msg)); },
        // //(msg) => { Debug.Log("QG.InstallShortcut fail = " + msg.errMsg); }
        // //);
        public static void InstallShortcut(Action<QGInstallShortcutBean> succCallback = null, Action<QGInstallShortcutBean> failCallback = null, Action<QGBaseResponse> completeCallback = null)
        {
            QGMiniGameManager.Instance.InstallShortcut(succCallback, failCallback, completeCallback);
        }
        // #endregion

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
        public static QGGamePortalAd CreateGamePortalAd(QGCommonAdParam param)
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
        public static QGGameDrawerAd CreateGameDrawerAd(QGCreateGameDrawerAdParam param)
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
        public static string StorageGetItem(string keyName)
        {
            return QGMiniGameManager.Instance.StorageGetItem(keyName);
        }
        // 清除数据 
        public static void StorageRemoveItem(string keyName)
        {
            QGMiniGameManager.StorageRemoveItem(keyName);
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
        public static void Pay(PayParam param, Action<QGCommonResponse<QGPayBean>> successCallback = null, Action<QGCommonResponse<QGPayBeanFail>> failCallback = null)
        {
            QGMiniGameManager.Instance.Pay(param, successCallback, failCallback);
        }

        public static void PayTest(PayTestParam param, Action<QGCommonResponse<QGPayBean>> successCallback = null, Action<QGCommonResponse<QGPayBeanFail>> failCallback = null)
        {
            QGMiniGameManager.Instance.PayTest(param, successCallback, failCallback);
        }
        #endregion

        #region GetNetworkType 获取网络类型

        public static void GetNetworkType(Action<QGCommonResponse<QGGetNetworkType>> succCallback = null, Action<QGCommonResponse<QGGetNetworkType>> failCallback = null)
        {
            QGMiniGameManager.Instance.GetNetworkType(succCallback, failCallback);
        }

        #endregion

        #region OnNetworkStatusChange 监听网络状态变化事件

        public static void OnNetworkStatusChange(Action<QGOnNetworkStatus> callback)
        {
            QGMiniGameManager.Instance.OnNetworkStatusChange(callback);
        }

        #endregion


        #region VibrateShort 短振动

        public static void VibrateShort()
        {
            QGMiniGameManager.Instance.VibrateShort();
        }

        #endregion

        #region VibrateLong 长振动

        public static void VibrateLong()
        {
            QGMiniGameManager.Instance.VibrateLong();
        }

        #endregion

        #region GetSystemInfo 异步获取系统信息

        public static void GetSystemInfo(Action<QGCommonResponse<QGSystemInfo>> successCallback = null, Action<QGCommonResponse<QGSystemInfo>> failCallback = null)
        {
            QGMiniGameManager.Instance.GetSystemInfo(successCallback, failCallback);
        }

        #endregion

        #region GetSystemInfoSync 同步获取系统信息

        public static string GetSystemInfoSync()
        {
            return QGMiniGameManager.Instance.GetSystemInfoSync();
        }

        #endregion

        #region SetEnableDebugTrue 打开vConsole

        public static void SetEnableDebugTrue()
        {
            QGMiniGameManager.Instance.SetEnableDebugTrue();
        }

        #endregion

        #region SetEnableDebugFalse 关闭vConsole

        public static void SetEnableDebugFalse()
        {
            QGMiniGameManager.Instance.SetEnableDebugFalse();
        }

        #endregion

        #region ShowModal 显示对话框

        public static void ShowModal(ShowModalParam showModalParam, Action<QGCommonResponse<ShowModalResponse>> successCallback = null, Action<QGCommonResponse<ShowModalResponse>> failCallback = null, Action<QGCommonResponse<ShowModalResponse>> completeCallback = null)
        {
            QGMiniGameManager.Instance.ShowModal(showModalParam, successCallback, failCallback, completeCallback);
        }

        #endregion

        #region 键盘
        public static string ShowKeyboard(KeyboardParam param, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null, Action<QGBaseResponse> completeCallback = null)
        {
            return QGMiniGameManager.Instance.ShowKeyboard(param, successCallback, failCallback, completeCallback);
        }

        public static void OnKeyboardInput(Action<QGResKeyBoardponse> callback)
        {
            QGMiniGameManager.Instance.OnKeyboardInput(callback);
        }

        public static void OffKeyboardInput()
        {
            QGMiniGameManager.Instance.OffKeyboardInput();
        }

        public static void OnKeyboardConfirm(Action<QGResKeyBoardponse> callback)
        {
            QGMiniGameManager.Instance.OnKeyboardConfirm(callback);
        }

        public static void OffKeyboardConfirm()
        {
            QGMiniGameManager.Instance.OffKeyboardConfirm();
        }

        public static void OnKeyboardComplete(Action<QGResKeyBoardponse> callback)
        {
            QGMiniGameManager.Instance.OnKeyboardComplete(callback);
        }

        public static void OffKeyboardComplete()
        {
            QGMiniGameManager.Instance.OffKeyboardComplete();
        }

        public static void HideKeyboard()
        {
            QGMiniGameManager.Instance.HideKeyboard();
        }

        #endregion


        // 文件系统
        #region Mkdir 创建目录

        public static void Mkdir(string dirPath, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null, Action<QGBaseResponse> completeCallback = null)
        {
            QGMiniGameManager.Instance.Mkdir(dirPath, successCallback, failCallback, completeCallback);
        }

        public static bool MkdirSync(string dirPath, bool recursive, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null)
        {
            return QGMiniGameManager.Instance.MkdirSync(dirPath, recursive, successCallback, failCallback);
        }

        #endregion

        #region Rmdir 删除目录
        public static void Rmdir(string dirPath, bool recursive, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null, Action<QGBaseResponse> completeCallback = null)
        {
            QGMiniGameManager.Instance.Rmdir(dirPath, recursive, successCallback, failCallback, completeCallback);
        }

        public static bool RmdirSync(string dirPath, bool recursive, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null)
        {
            return QGMiniGameManager.Instance.RmdirSync(dirPath, recursive, successCallback, failCallback);
        }
        #endregion

        #region Rmdir 删除文件
        public static void Unlink(string dirPath, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null, Action<QGBaseResponse> completeCallback = null)
        {
            QGMiniGameManager.Instance.Unlink(dirPath, successCallback, failCallback, completeCallback);
        }

        public static bool UnlinkSync(string dirPath, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null)
        {
            return QGMiniGameManager.Instance.UnlinkSync(dirPath, successCallback, failCallback);
        }
        #endregion

        #region IsExist 是否是目录/文件

        public static void IsExist()
        {
            QGMiniGameManager.Instance.IsExist();
        }

        #endregion

        #region Rename 重命名目录

        public static void Rename(string oldPath, string newPath, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null, Action<QGBaseResponse> completeCallback = null)
        {
            QGMiniGameManager.Instance.Rename(oldPath, newPath, successCallback, failCallback, completeCallback);
        }

        public static bool RenameSync(string oldPath, string newPath, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null)
        {
            return QGMiniGameManager.Instance.RenameSync(oldPath, newPath, successCallback, failCallback);
        }

        #endregion

        #region SaveFile 保存临时文件到本地
        public static void SaveFile(string tempFilePath, string filePath, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null, Action<QGBaseResponse> completeCallback = null)
        {
            QGMiniGameManager.Instance.SaveFile(tempFilePath, filePath, successCallback, failCallback, completeCallback);
        }

        public static string SaveFileSync(string tempFilePath, string filePath, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null)
        {
            return QGMiniGameManager.Instance.SaveFileSync(tempFilePath, filePath, successCallback, failCallback);
        }

        #endregion

        #region ReadDir 读取目录内文件列表

        public static void ReadDir(string dirPath, Action<ReadDirResponse> successCallback = null, Action<QGBaseResponse> failCallback = null, Action<QGBaseResponse> completeCallback = null)
        {
            QGMiniGameManager.Instance.ReadDir(dirPath, successCallback, failCallback, completeCallback);
        }

        public static ReadDirResponse ReadDirSync(string dirPath, Action<ReadDirResponse> successCallback = null, Action<QGBaseResponse> failCallback = null)
        {
            return QGMiniGameManager.Instance.ReadDirSync(dirPath, successCallback, failCallback);
        }

        #endregion

        #region WriteFile 写入文件
        public static void WriteFile(string filePath, object param, bool append, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null, Action<QGBaseResponse> completeCallback = null)
        {
            QGMiniGameManager.Instance.WriteFile(filePath, param, append, successCallback, failCallback, completeCallback);
        }

        public static bool WriteFileSync(string filePath, object param, bool append, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null)
        {
            return QGMiniGameManager.Instance.WriteFileSync(filePath, param, append, successCallback, failCallback);
        }

        #endregion

        #region ReadFile 读取文件

        public static void ReadFile(string filePath, string encoding, Action<ReadFileResponse> successCallback = null, Action<QGBaseResponse> failCallback = null, Action<QGBaseResponse> completeCallback = null)
        {
            QGMiniGameManager.Instance.ReadFile(filePath, encoding, successCallback, failCallback, completeCallback);
        }

        public static ReadFileResponse ReadFileSync(string filePath, string encoding, Action<ReadFileResponse> successCallback = null, Action<QGBaseResponse> failCallback = null)
        {
            return QGMiniGameManager.Instance.ReadFileSync(filePath, encoding, successCallback, failCallback);
        }

        #endregion

        #region AppendFile 追加文件

        public static void AppendFile(string filePath, object param, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null, Action<QGBaseResponse> completeCallback = null)
        {
            QGMiniGameManager.Instance.AppendFile(filePath, param, successCallback, failCallback, completeCallback);
        }

        public static bool AppendFileSync(string filePath, object param, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null)
        {
            return QGMiniGameManager.Instance.AppendFileSync(filePath, param, successCallback, failCallback);
        }

        #endregion

        #region CopyFile 复制文件

        public static void CopyFile(string srcPath, string destPath, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null, Action<QGBaseResponse> completeCallback = null)
        {
            QGMiniGameManager.Instance.CopyFile(srcPath, destPath, successCallback, failCallback, completeCallback);
        }

        public static bool CopyFileSync(string srcPath, string destPath, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null)
        {
            return QGMiniGameManager.Instance.CopyFileSync(srcPath, destPath, successCallback, failCallback);
        }

        #endregion

        #region RemoveSavedFile 删除文件

        public static void RemoveSavedFile(string filePath, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null, Action<QGBaseResponse> completeCallback = null)
        {
            QGMiniGameManager.Instance.RemoveSavedFile(filePath, successCallback, failCallback, completeCallback);
        }

        #endregion

        #region Stat 获取文件信息

        public static void Stat(string path, Action<StatResponse> successCallback = null, Action<QGBaseResponse> failCallback = null, Action<QGBaseResponse> completeCallback = null)
        {
            QGMiniGameManager.Instance.Stat(path, successCallback, failCallback, completeCallback);
        }

        public static StatResponse StatSync(string path, bool recursive, Action<StatResponse> successCallback = null, Action<QGBaseResponse> failCallback = null)
        {
            return QGMiniGameManager.Instance.StatSync(path, recursive, successCallback, failCallback);
        }

        #endregion

        #region Unzip 解压文件

        public static void Unzip(string zipFilePath, string targetPath, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null, Action<QGBaseResponse> completeCallback = null)
        {
            QGMiniGameManager.Instance.Unzip(zipFilePath, targetPath, successCallback, failCallback, completeCallback);
        }

        #endregion

        #region GetFileInfo 获取本地临时文件或本地用户文件的文件信息

        public static void GetFileInfo(string filename, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null)
        {
            QGMiniGameManager.Instance.GetFileInfo(filename, successCallback, failCallback);
        }

        #endregion

        #region DownLoadIcon 下载图片

        public static void DownLoadFile(DownLoadFileParam param, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null)
        {
            QGMiniGameManager.Instance.DownLoadFile(param, successCallback, failCallback);
        }

        #endregion

        #region UploadFile 上传图片

        public static void UploadFile(UpLoadFileParam param, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null)
        {
            QGMiniGameManager.Instance.UploadFile(param, successCallback, failCallback);
        }

        #endregion

        #region PlayVideo 播放远程视频

        public static QGVideoPlayer CreateVideo(VideoParam param)
        {
            return QGMiniGameManager.Instance.CreateVideo(param);
        }

        #endregion

        #region PlayAudio 播放远程音频

        public static QGAudioPlayer PlayAudio(AudioParam param)
        {
            return QGMiniGameManager.Instance.PlayAudio(param);
        }

        #endregion



        // #region PauseAudio 暂停音频

        // public static void PauseAudio()
        // {
        //     QGMiniGameManager.Instance.PauseAudio();
        // }

        // #endregion 

        #region OnAudioInterruptionBegin 监听qg.onAudioInterruptionBegin

        public static void OnAudioInterruptionBegin()
        {
            QGMiniGameManager.Instance.OnAudioInterruptionBegin();
        }

        #endregion 

        #region OffAudioInterruptionBegin 取消监听qg.onAudioInterruptionBegin

        public static void OffAudioInterruptionBegin()
        {
            QGMiniGameManager.Instance.OffAudioInterruptionBegin();
        }

        #endregion 

        #region OnAudioInterruptionEnd 监听qg.onAudioInterruptionEnd

        public static void OnAudioInterruptionEnd()
        {
            QGMiniGameManager.Instance.OnAudioInterruptionEnd();
        }

        #endregion 

        #region OffAudioInterruptionEnd 取消监听qg.onAudioInterruptionEnd

        public static void OffAudioInterruptionEnd()
        {
            QGMiniGameManager.Instance.OffAudioInterruptionEnd();
        }

        #endregion 

        #region OnError 监听全局错误事件

        public static void OnError()
        {
            QGMiniGameManager.Instance.OnError();
        }

        #endregion 

        #region OffError 取消监听全局错误事件

        public static void OffError()
        {
            QGMiniGameManager.Instance.OffError();
        }

        #endregion 

        #region DispatchError 模拟触发Error

        public static void DispatchError()
        {
            QGMiniGameManager.Instance.DispatchError();
        }

        #endregion 

        #region ShowToast 提示框

        public static void ShowToast(ShowToastParam param)
        {
            QGMiniGameManager.Instance.ShowToast(param);
        }

        #endregion 
        public static void Log()
        {
            QGMiniGameManager.Instance.Log();
        }

        public static void LogClose()
        {
            QGMiniGameManager.Instance.LogClose();
        }

        #region ExitApplication 退出游戏
        public static void ExitApplication(ExitApplicationParam param, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null, Action<QGBaseResponse> completeCallback = null)
        {
            QGMiniGameManager.Instance.ExitApplication(param, successCallback, failCallback, completeCallback);
        }

        public static void ExitApplication()
        {
            QGMiniGameManager.Instance.ExitApplication(null, null, null, null);
        }
        #endregion
        #region 系统信息
        public static void GetManifestInfo(Action<QGCommonResponse<QGManifestInfoRponse>> successCallback = null, Action<QGCommonResponse<QGManifestInfoRponse>> failCallback = null)
        {
            QGMiniGameManager.Instance.GetManifestInfo(successCallback, failCallback);
        }

        public static void GetProvider(Action<QGCommonResponse<QGProviderRponse>> callback = null)
        {
            QGMiniGameManager.Instance.GetProvider(callback);
        }

        public static void SetPreferredFramesPerSecond(int fps)
        {
            QGMiniGameManager.Instance.SetPreferredFramesPerSecond(fps);
        }
        #endregion

        #region ShowLoading 进度条

        public static void ShowLoading(string title)
        {
            QGMiniGameManager.Instance.ShowLoading(title);
        }

        public static void HideLoading(Action<QGBaseResponse> success = null)
        {
            QGMiniGameManager.Instance.HideLoading(success);
        }

        public static void SetTimeout(int times, Action<QGBaseResponse> action = null)
        {
            QGMiniGameManager.Instance.SetTimeout(times, action);
        }
        #endregion 

        #region UserCloudStorage 云存储

        public static void SetUserCloudStorage(string key, string value, Action<QGCommonResponse<string>> successCallback = null, Action<QGCommonResponse<string>> failCallback = null, Action<QGCommonResponse<string>> completeCallback = null)
        {
            userCloudStorageParam.key = key;
            userCloudStorageParam.value = value;
            QGMiniGameManager.Instance.SetUserCloudStorage(userCloudStorageParam, successCallback, failCallback, completeCallback);
        }

        public static void GetUserCloudStorage(string key, Action<QGCommonResponse<UserCloudStorageParam>> successCallback = null, Action<QGCommonResponse<UserCloudStorageParam>> failCallback = null, Action<QGCommonResponse<UserCloudStorageParam>> completeCallback = null)
        {
            QGMiniGameManager.Instance.GetUserCloudStorage(key, successCallback, failCallback, completeCallback);
        }

        public static void RemoveUserCloudStorage(string key)
        {
            QGMiniGameManager.Instance.RemoveUserCloudStorage(key);
        }

        #endregion 

        #region 设备信息 电量 设备号

        public static void GetBatteryInfo(Action<QGCommonResponse<BatteryInfoParam>> successCallback = null, Action<QGCommonResponse<BatteryInfoParam>> failCallback = null, Action<QGCommonResponse<BatteryInfoParam>> completeCallback = null)
        {
            QGMiniGameManager.Instance.GetBatteryInfo(successCallback, failCallback, completeCallback);
        }

        public static BatteryInfoParam GetBatteryInfoSync()
        {
            string batteryJsonStr = QGMiniGameManager.Instance.GetBatteryInfoSync();
            BatteryInfoParam batteryInfoParam = JsonUtility.FromJson<BatteryInfoParam>(batteryJsonStr);
            return batteryInfoParam;
        }

        public static void GetDeviceId(Action<QGCommonResponse<DeviceIdParam>> successCallback = null, Action<QGCommonResponse<DeviceIdParam>> failCallback = null, Action<QGCommonResponse<DeviceIdParam>> completeCallback = null)
        {
            QGMiniGameManager.Instance.GetDeviceId(successCallback, failCallback, completeCallback);
        }

        public static void GetScreenBrightness(Action<QGCommonResponse<ScreenBrightnessParam>> successCallback = null, Action<QGCommonResponse<ScreenBrightnessParam>> failCallback = null, Action<QGCommonResponse<ScreenBrightnessParam>> completeCallback = null)
        {
            QGMiniGameManager.Instance.GetScreenBrightness(successCallback, failCallback, completeCallback);
        }

        public static void SetScreenBrightness(float param, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null, Action<QGBaseResponse> completeCallback = null)
        {
            QGMiniGameManager.Instance.SetScreenBrightness(param, successCallback, failCallback, completeCallback);
        }

        public static void SetKeepScreenOn(bool param, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null, Action<QGBaseResponse> completeCallback = null)
        {
            QGMiniGameManager.Instance.SetKeepScreenOn(param, successCallback, failCallback, completeCallback);
        }

        public static void GetLocation(Action<QGCommonResponse<GetLocationParam>> successCallback = null, Action<QGCommonResponse<GetLocationParam>> failCallback = null, Action<QGCommonResponse<GetLocationParam>> completeCallback = null)
        {
            QGMiniGameManager.Instance.GetLocation(successCallback, failCallback, completeCallback);
        }

        public static void OnAccelerometerChange(Action<QGCommonResponse<onAccelerometerChangeParam>> successCallback = null)
        {
            QGMiniGameManager.Instance.OnAccelerometerChange(successCallback);
        }

        public static void StartAccelerometer(string param, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null, Action<QGBaseResponse> completeCallback = null)
        {
            QGMiniGameManager.Instance.StartAccelerometer(param, successCallback, failCallback, completeCallback);
        }

        public static void StopAccelerometer(Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null, Action<QGBaseResponse> completeCallback = null)
        {
            QGMiniGameManager.Instance.StopAccelerometer(successCallback, failCallback, completeCallback);
        }

        public static void SetClipboardData(string param, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null, Action<QGBaseResponse> completeCallback = null)
        {
            QGMiniGameManager.Instance.SetClipboardData(param, successCallback, failCallback, completeCallback);
        }
        public static void GetClipboardData(Action<QGCommonResponse<string>> successCallback = null, Action<QGCommonResponse<string>> failCallback = null, Action<QGCommonResponse<string>> completeCallback = null)
        {
            QGMiniGameManager.Instance.GetClipboardData(successCallback, failCallback, completeCallback);
        }

        public static void OnCompassChange(Action<QGCommonResponse<float>> successCallback = null)
        {
            QGMiniGameManager.Instance.OnCompassChange(successCallback);
        }

        public static void StartCompass(Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null, Action<QGBaseResponse> completeCallback = null)
        {
            QGMiniGameManager.Instance.StartCompass(successCallback, failCallback, completeCallback);
        }

        public static void StopCompass(Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null, Action<QGBaseResponse> completeCallback = null)
        {
            QGMiniGameManager.Instance.StopCompass(successCallback, failCallback, completeCallback);
        }
        #endregion

        #region FileSystemManager  文件类
        public static void Access(string filename, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null, Action<QGBaseResponse> completeCallback = null)
        {
            QGMiniGameManager.Instance.Access(filename, successCallback, failCallback, completeCallback);
        }

        public static bool AccessSync(string filename, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null)
        {
            return QGMiniGameManager.Instance.AccessSync(filename, successCallback, failCallback);
        }
        #endregion
    }
}

