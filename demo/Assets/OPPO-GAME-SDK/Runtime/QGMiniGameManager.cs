﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using System.Text;

namespace QGMiniGame
{
    public class QGMiniGameManager : MonoBehaviour
    {
        #region Instance

        private static QGMiniGameManager instance = null;


        public static QGMiniGameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject(typeof(QGMiniGameManager).Name).AddComponent<QGMiniGameManager>();
                    DontDestroyOnLoad(instance.gameObject);
                }
                return instance;
            }
        }

        #endregion

        #region 登录

        public void Login(Action<QGCommonResponse<QGLoginBean>> successCallback = null, Action<QGCommonResponse<QGLoginBean>> failCallback = null)
        {
            QGLogin(QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback));
        }

        #endregion

        // #region 用户信息

        // public void GetUserInfo(Action<QGCommonResponse<QGUserInfoBean>> successCallback = null, Action<QGCommonResponse<QGUserInfoBean>> failCallback = null)
        // {
        //     QGGetUserInfo(QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback));
        // }

        // #endregion

        #region 获取桌面图标是否创建

        public void HasShortcutInstalled(Action<QGCommonResponse<QGHasShortcutInstalled>> successCallback = null, Action<QGCommonResponse<QGHasShortcutInstalled>> failCallback = null)
        {
            QGHasShortcutInstalled(QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback));
        }

        #endregion

        // #region 创建桌面图标

        public void InstallShortcut(Action<QGCommonResponse<QGHasShortcutInstalled>> successCallback = null, Action<QGCommonResponse<QGHasShortcutInstalled>> failCallback = null)
        {
            QGInstallShortcut(QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback));
        }

        // #endregion

        #region 获取网络类型

        public void GetNetworkType(Action<QGCommonResponse<QGGetNetworkType>> successCallback = null, Action<QGCommonResponse<QGGetNetworkType>> failCallback = null)
        {
            QGGetNetworkType(QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback));
        }

        #endregion

        #region 监听网络状态变化事件

        public void OnNetworkStatusChange(Action<QGOnNetworkStatus> successCallback = null)
        {
            QGOnNetworkStatusChange(QGCallBackManager.Add(successCallback));
        }

        #endregion


        #region 短振动

        public void VibrateShort()
        {
            QGVibrateShort();
        }

        #endregion

        #region 长振动

        public void VibrateLong()
        {
            QGVibrateLong();
        }

        #endregion

        #region 异步获取系统信息

        public void GetSystemInfo(Action<QGCommonResponse<QGSystemInfo>> successCallback = null, Action<QGCommonResponse<QGSystemInfo>> failCallback = null)
        {
            QGGetSystemInfo(QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback));
        }

        #endregion

        #region 同步获取系统信息

        public string GetSystemInfoSync()
        {
            return QGGetSystemInfoSync();
        }

        #endregion

        #region 打开vConsole

        public void SetEnableDebugTrue()
        {
            QGSetEnableDebugTrue();
        }

        #endregion

        #region 关闭vConsole

        public void SetEnableDebugFalse()
        {
            QGSetEnableDebugFalse();
        }

        #endregion

        #region 显示对话框

        public void ShowModal(ShowModalParam showModalParam, Action<QGCommonResponse<ShowModalResponse>> successCallback = null, Action<QGCommonResponse<ShowModalResponse>> failCallback = null, Action<QGCommonResponse<ShowModalResponse>> completeCallback = null)
        {
            QGShowModal(JsonUtility.ToJson(showModalParam), QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback), QGCallBackManager.Add(completeCallback));
        }

        #endregion

        #region 键盘
        public string ShowKeyboard(KeyboardParam param, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null, Action<QGBaseResponse> completeCallback = null)
        {
            var keyboardId = QGCallBackManager.getKey();
            QGShowKeyboard(keyboardId, JsonUtility.ToJson(param), QGCallBackManager.Add(successCallback),
            QGCallBackManager.Add(failCallback),
            QGCallBackManager.Add(completeCallback));
            return keyboardId;
        }

        public void OnKeyboardInput(Action<QGResKeyBoardponse> successCallback = null)
        {
            QGOnKeyboardInput(QGCallBackManager.Add(successCallback));
        }

        public void OffKeyboardInput()
        {
            QGOffKeyboardInput();
        }

        public void OnKeyboardConfirm(Action<QGResKeyBoardponse> successCallback = null)
        {
            QGOnKeyboardConfirm(QGCallBackManager.Add(successCallback));
        }

        public void OffKeyboardConfirm()
        {
            QGOffKeyboardConfirm();
        }

        public void OnKeyboardComplete(Action<QGResKeyBoardponse> successCallback = null)
        {
            QGOnKeyboardComplete(QGCallBackManager.Add(successCallback));
        }

        public void OffKeyboardComplete()
        {
            QGOffKeyboardComplete();
        }

        public void HideKeyboard()
        {
            QGHideKeyboard();
        }
        #endregion

        #region 创建目录

        public void Mkdir()
        {
            QGMkdir();
        }

        #endregion

        #region 删除目录

        public void Rmdir()
        {
            QGRmdir();
        }

        #endregion

        #region 是否是目录/文件

        public void IsExist()
        {
            QGIsExist();
        }

        #endregion

        #region 重命名目录

        public void Rename()
        {
            QGRename();
        }

        #endregion

        #region 保存临时文件到本地

        public void SaveFile()
        {
            QGSaveFile();
        }

        #endregion

        #region 读取目录内文件列表

        public void ReadDir()
        {
            QGReadDir();
        }

        #endregion

        #region 写入文件

        public void WriteFile()
        {
            QGWriteFile();
        }

        #endregion

        #region 读取文件

        public void ReadFile()
        {
            QGReadFile();
        }

        #endregion

        #region 追加文件

        public void AppendFile()
        {
            QGAppendFile();
        }

        #endregion

        #region 复制文件

        public void CopyFile()
        {
            QGCopyFile();
        }

        #endregion

        #region 删除文件

        public void RemoveSavedFile()
        {
            QGRemoveSavedFile();
        }

        #endregion

        #region 获取文件信息

        public void Stat()
        {
            QGStat();
        }

        #endregion

        #region 解压文件

        public void Unzip()
        {
            QGUnzip();
        }

        #endregion

        #region 获取本地临时文件或本地用户文件的文件信息 filename 例: "/abc/file.txt"

        public void GetFileInfo(string filename,Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null)
        {
            QGGetFileInfo(filename,QGCallBackManager.Add(successCallback),
            QGCallBackManager.Add(failCallback));
        }

        #endregion

        #region 播放远程音频

        public QGAudioPlayer PlayAudio(AudioParam param)
        {
            var playerId = QGCallBackManager.getKey();
            Debug.Log("playerId: " + playerId);
            QGAudioPlayer ap = new QGAudioPlayer(playerId);
            QGPlayAudio(playerId, JsonUtility.ToJson(param));
            return ap;
        }

        #endregion

        #region 播放远程视频

        public QGVideoPlayer CreateVideo(VideoParam param)
        {
            var adId = QGCallBackManager.getKey();
            Debug.Log("adId: " + adId);
            QGVideoPlayer ad = new QGVideoPlayer(adId);
            QGCreateVideo(adId, JsonUtility.ToJson(param));
            return ad;
        }

        #endregion

        #region 下载图片

        public void DownLoadFile(DownLoadFileParam param, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null)
        {
            QGDownLoadFile(JsonUtility.ToJson(param), QGCallBackManager.Add(successCallback),
            QGCallBackManager.Add(failCallback));
        }

        #endregion

        #region 上传图片

        public void UploadFile(UpLoadFileParam param, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null)
        {
            QGUploadFile(JsonUtility.ToJson(param), QGCallBackManager.Add(successCallback),
            QGCallBackManager.Add(failCallback));
        }
        #endregion


        // #region 暂停音频

        // public void PauseAudio()
        // {
        //     QGPauseAudio();
        // }

        // #endregion

        #region 监听qg.onAudioInterruptionBegin

        public void OnAudioInterruptionBegin()
        {
            QGOnAudioInterruptionBegin();
        }

        #endregion

        #region 取消监听qg.onAudioInterruptionBegin

        public void OffAudioInterruptionBegin()
        {
            QGOffAudioInterruptionBegin();
        }

        #endregion

        #region 监听qg.onAudioInterruptionEnd

        public void OnAudioInterruptionEnd()
        {
            QGOnAudioInterruptionEnd();
        }

        #endregion

        #region 取消监听qg.onAudioInterruptionEnd

        public void OffAudioInterruptionEnd()
        {
            QGOffAudioInterruptionEnd();
        }

        #endregion

        #region 监听全局错误事件

        public void OnError()
        {
            QGOnError();
        }

        #endregion

        #region 取消监听全局错误事件

        public void OffError()
        {
            QGOffError();
        }

        #endregion

        #region 模拟触发Error

        public void DispatchError()
        {
            QGDispatchError();
        }

        #endregion


        #region 创建Banner广告

        public QGBannerAd CreateBannerAd(QGCreateBannerAdParam param)
        {
            var adId = QGCallBackManager.getKey();
            QGBannerAd ad = new QGBannerAd(adId);
            // QGCreateBannerAd(adId, param.posId, JsonUtility.ToJson(param.style), param.adIntervals);
            QGCreateBannerAd(adId, param.adUnitId, JsonUtility.ToJson(param.style));
            return ad;
        }

        #endregion

        #region 创建激励视频广告

        public QGRewardedVideoAd CreateRewardedVideoAd(QGCommonAdParam param)
        {
            var adId = QGCallBackManager.getKey();
            QGRewardedVideoAd ad = new QGRewardedVideoAd(adId);
            QGCreateRewardedVideoAd(adId, param.adUnitId);
            return ad;
        }

        #endregion

        #region 创建插屏广告

        public QGInterstitialAd CreateInterstitialAd(QGCommonAdParam param)
        {
            var adId = QGCallBackManager.getKey();
            QGInterstitialAd ad = new QGInterstitialAd(adId);
            QGCreateInterstitialAd(adId, param.adUnitId);
            return ad;
        }

        #endregion

        #region 创建模板广告

        public QGCustomAd CreateCustomAd(QGCreateCustomAdParam param)
        {
            var adId = QGCallBackManager.getKey();
            QGCustomAd ad = new QGCustomAd(adId);
            QGCreateCustomAd(adId, param.adUnitId, JsonUtility.ToJson(param.style));
            return ad;
        }

        public bool IsShow(string adId)
        {
            return QGIsShow(adId);
        }

        #endregion

        #region 创建互推盒子横幅广告

        public QGGameBannerAd CreateGameBannerAd(QGCommonAdParam param)
        {
            var adId = QGCallBackManager.getKey();
            QGGameBannerAd ad = new QGGameBannerAd(adId);
            QGCreateGameBannerAd(adId, param.adUnitId);
            return ad;
        }

        #endregion

        #region 创建互推盒子九宫格广告

        public QGGamePortalAd CreateGamePortalAd(QGCommonAdParam param)
        {
            var adId = QGCallBackManager.getKey();
            QGGamePortalAd ad = new QGGamePortalAd(adId);
            QGCreateGamePortalAd(adId, param.adUnitId);
            return ad;
        }

        #endregion

        #region 创建互推盒子抽屉广告

        public QGGameDrawerAd CreateGameDrawerAd(QGCreateGameDrawerAdParam param)
        {
            var adId = QGCallBackManager.getKey();
            QGGameDrawerAd ad = new QGGameDrawerAd(adId);
            QGCreateGameDrawerAd(adId, param.adUnitId, JsonUtility.ToJson(param.style));
            return ad;
        }

        #endregion

        #region 广告通用逻辑
        public void ShowAd(string adId, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null)
        {
            QGShowAd(adId, QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback));
        }

        public void HideAd(string adId, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null)
        {
            QGHideAd(adId, QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback));
        }

        public void LoadAd(string adId, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null)
        {
            QGLoadAd(adId, QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback));
        }

        public void DestroyAd(string adId)
        {
            QGDestroyAd(adId);
        }

        #endregion

        #region storage 
        // 数据存储  
        public static void StorageSetItem(string keyName, string keyValue)
        {
            QGStorageSetItem(keyName, keyValue);
        }
        // 数据读取  
        public string StorageGetItem(string keyName)
        {
            return QGStorageGetItem(keyName);
        }
        // 清除数据 
        public static void StorageRemoveItem(string keyName)
        {
            QGStorageRemoveItem(keyName);
        }
        #endregion

        #region 支付
        public void Pay(PayParam param, Action<QGCommonResponse<QGPayBean>> successCallback = null, Action<QGCommonResponse<QGPayBeanFail>> failCallback = null)
        {
            QGPay(JsonUtility.ToJson(param), QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback));
        }

        public void PayTest(PayTestParam param, Action<QGCommonResponse<QGPayBean>> successCallback = null, Action<QGCommonResponse<QGPayBeanFail>> failCallback = null)
        {
            QGPayTest(JsonUtility.ToJson(param), QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback));
        }
        #endregion

        #region JS回调
        public void LoginResponseCallback(string msg)
        {
            QGCallBackManager.InvokeResponseCallback<QGCommonResponse<QGLoginBean>>(msg);
        }

        public void OnKeyboardInputResponseCallback(string msg)
        {
            QGCallBackManager.InvokeResponseCallback<QGResKeyBoardponse>(msg, false);
        }

        public void ShowKeyboardResponseCallback(string msg)
        {
            QGCallBackManager.InvokeResponseCallback<QGBaseResponse>(msg, false);
        }


        public void OnNetworkStatusChangeResponseCallback(string msg)
        {
            QGCallBackManager.InvokeResponseCallback<QGOnNetworkStatus>(msg, false);
        }

        // public void GetUserInfoResponseCallback(string msg)
        // {
        //     QGCallBackManager.InvokeResponseCallback<QGCommonResponse<QGUserInfoBean>>(msg);
        // }
        public void ShortcutResponseCallback(string msg)
        {
            QGCallBackManager.InvokeResponseCallback<QGCommonResponse<QGShortcutBean>>(msg);
        }

        public void HasShortcutInstalledResponseCallback(string msg)
        {
            QGCallBackManager.InvokeResponseCallback<QGCommonResponse<QGHasShortcutInstalled>>(msg);
        }

        public void DefaultResponseCallback(string msg)
        {
            QGCallBackManager.InvokeResponseCallback<QGBaseResponse>(msg);
        }

        // 支付回调成功
        public void PayResponseCallback(string msg)
        {
            QGCallBackManager.InvokeResponseCallback<QGCommonResponse<QGPayBean>>(msg);
        }

        // 支付回调失败
        public void PayResponseFailCallback(string msg)
        {
            QGCallBackManager.InvokeResponseCallback<QGCommonResponse<QGPayBeanFail>>(msg);
        }

        public void GetNetworkTypeCallback(string msg)
        {
            QGCallBackManager.InvokeResponseCallback<QGCommonResponse<QGGetNetworkType>>(msg);
        }

        public void SystemInfo(string msg)
        {
            QGCallBackManager.InvokeResponseCallback<QGCommonResponse<QGSystemInfo>>(msg);
        }

        public void ManifestInfo(string msg)
        {
            QGCallBackManager.InvokeResponseCallback<QGCommonResponse<QGManifestInfoRponse>>(msg);
        }

        public void ProviderInfo(string msg)
        {
            QGCallBackManager.InvokeResponseCallback<QGCommonResponse<QGProviderRponse>>(msg);
        }

        // 广告通用回调 
        public void AdOnErrorCallBack(string msg)
        {
            var res = JsonUtility.FromJson<QGBaseResponse>(msg);
            var ad = QGBaseAd.QGAds[res.callbackId];
            if (ad != null)
            {
                ad.onErrorAction?.Invoke(res);
            }
        }

        public void AdOnLoadCallBack(string msg)
        {
            var res = JsonUtility.FromJson<QGBaseResponse>(msg);
            var ad = QGBaseAd.QGAds[res.callbackId];
            if (ad != null)
            {
                ad.onLoadAction?.Invoke();
            }
        }

        public void AdOnCloseCallBack(string msg)
        {
            var res = JsonUtility.FromJson<QGBaseResponse>(msg);
            var ad = QGBaseAd.QGAds[res.callbackId];
            if (ad != null)
            {
                ad.onCloseAction?.Invoke();
            }
        }

        public void AdOnHideCallBack(string msg)
        {
            var res = JsonUtility.FromJson<QGBaseResponse>(msg);
            var ad = QGBaseAd.QGAds[res.callbackId];
            if (ad != null)
            {
                ad.onHideAction?.Invoke();
            }
        }

        // public void AdOnShowCallBack(string msg)
        // {
        //     var res = JsonUtility.FromJson<QGBaseResponse>(msg);
        //     var ad = QGBaseAd.QGAds[res.callbackId];
        //     if (ad != null && ad is QGGamePortalAd)
        //     {
        //         ((QGGamePortalAd)ad).onShowAction?.Invoke();
        //     }
        // }

        public void RewardedVideoAdOnCloseCallBack(string msg)
        {
            var res = JsonUtility.FromJson<QGRewardedVideoResponse>(msg);
            var ad = QGBaseAd.QGAds[res.callbackId];
            if (ad != null && ad is QGRewardedVideoAd)
            {
                ((QGRewardedVideoAd)ad).onCloseRewardedVideoAction?.Invoke(res);
            }
        }

        public void ShowToast(ShowToastParam param)
        {
            QGShowToast(JsonUtility.ToJson(param));
        }

        // 播放器通用回调 
        public void pdOnPlayCallBack(string msg)
        {
            var res = JsonUtility.FromJson<QGBaseResponse>(msg);
            var pd = QGBasePlayer.QGPlayers[res.callbackId];
            if (pd != null)
            {
                pd.onPlayAction?.Invoke();
            }
        }

        public void pdOnCanPlayCallBack(string msg)
        {
            var res = JsonUtility.FromJson<QGBaseResponse>(msg);
            var pd = QGBasePlayer.QGPlayers[res.callbackId];
            if (pd != null)
            {
                pd.onCanPlayAction?.Invoke();
            }
        }

        public void pdOnPauseCallBack(string msg)
        {
            var res = JsonUtility.FromJson<QGBaseResponse>(msg);
            var pd = QGBasePlayer.QGPlayers[res.callbackId];
            if (pd != null)
            {
                pd.onPauseAction?.Invoke();
            }
        }

        public void pdOnStopCallBack(string msg)
        {
            var res = JsonUtility.FromJson<QGBaseResponse>(msg);
            var pd = QGBasePlayer.QGPlayers[res.callbackId];
            if (pd != null)
            {
                pd.onStopAction?.Invoke();
            }
        }

        public void pdOnEndedCallBack(string msg)
        {
            var res = JsonUtility.FromJson<QGBaseResponse>(msg);
            var pd = QGBasePlayer.QGPlayers[res.callbackId];
            if (pd != null)
            {
                pd.onEndedAction?.Invoke();
            }
        }

        public void pdOnTimeUpdateCallBack(string msg)
        {
            var res = JsonUtility.FromJson<QGBaseResponse>(msg);
            var pd = QGBasePlayer.QGPlayers[res.callbackId];
            if (pd != null)
            {
                pd.onTimeUpdateAction?.Invoke();
            }
        }

        public void pdOnErrorCallBack(string msg)
        {
            var res = JsonUtility.FromJson<QGBaseResponse>(msg);
            var pd = QGBasePlayer.QGPlayers[res.callbackId];
            if (pd != null)
            {
                pd.onErrorAction?.Invoke();
            }
        }

        public void pdOnWaitingCallBack(string msg)
        {
            var res = JsonUtility.FromJson<QGBaseResponse>(msg);
            var pd = QGBasePlayer.QGPlayers[res.callbackId];
            if (pd != null)
            {
                pd.onWaitingAction?.Invoke();
            }
        }

        public void pdOnSeekingCallBack(string msg)
        {
            var res = JsonUtility.FromJson<QGBaseResponse>(msg);
            var pd = QGBasePlayer.QGPlayers[res.callbackId];
            if (pd != null)
            {
                pd.onSeekingAction?.Invoke();
            }
        }

        public void pdOnSeekedCallBack(string msg)
        {
            var res = JsonUtility.FromJson<QGBaseResponse>(msg);
            var pd = QGBasePlayer.QGPlayers[res.callbackId];
            if (pd != null)
            {
                pd.onSeekedAction?.Invoke();
            }
        }
        #endregion

        #region 播放器通用逻辑
        public void PlayMedia(string playerId)
        {
            QGPlayMedia(playerId);
        }

        public void PauseMedia(string playerId)
        {
            QGPauseMedia(playerId);
        }

        public void StopMedia(string playerId)
        {
            QGStopMedia(playerId);
        }

        public void DestroyMedia(string playerId)
        {
            QGDestroyMedia(playerId);
        }

        public void SeekMedia(string playerId, float time)
        {
            QGSeekMedia(playerId, time);
        }

        public void AudioPlayerVolume(string playerId, float volume)
        {
            QGAudioPlayerVolume(playerId, volume);
        }

        public void AudioPlayerLoop(string playerId, bool bl)
        {
            QGAudioPlayerLoop(playerId, bl);
        }

        #endregion
        public List<string> LogMessage = new List<string>();
        public void HandleLogMessage(string message)
        {
            LogMessage.Add(message);
        }
        public void Log()
        {
            QGLog();
        }
        public void LogClose()
        {
            QGLogClose();
        }
        public void ExitApplication(ExitApplicationParam param, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null, Action<QGBaseResponse> completeCallback = null)
        {
            string exitData = param == null ? null : param.data;
            QGExitApplication(exitData, QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback), QGCallBackManager.Add(completeCallback));
        }

        public void ExitApplicationSuCallback(string msg)
        {
            QGCallBackManager.InvokeResponseCallback<QGCommonResponse<QGGetNetworkType>>(msg);
        }
        public void GetManifestInfo(Action<QGCommonResponse<QGManifestInfoRponse>> successCallback = null, Action<QGCommonResponse<QGManifestInfoRponse>> failCallback = null)
        {
            QGGetManifestInfo(QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback));
        }
        public void GetProvider(Action<QGCommonResponse<QGProviderRponse>> callback = null)
        {
            QGGetProvider(QGCallBackManager.Add(callback));
        }
        public void SetPreferredFramesPerSecond(int fps)
        {
            QGSetPreferredFramesPerSecond(fps);
        }
        public void ShowLoading(string title)
        {
            QGShowLoading(title);
        }
        public void HideLoading(Action<QGBaseResponse> success = null)
        {
            QGHideLoading(QGCallBackManager.Add(success));
        }
        public void SetTimeout(int times, Action<QGBaseResponse> action = null)
        {
            QGSetTimeout(times, QGCallBackManager.Add(action));
        }

        public void ShowModalCallback(string msg)
        {
            QGCallBackManager.InvokeResponseCallback<QGCommonResponse<ShowModalResponse>>(msg);
        }

        #region 数据云储存

        public void SetUserCloudStorage(UserCloudStorageParam userCloudStorageParam, Action<QGCommonResponse<string>> successCallback = null, Action<QGCommonResponse<string>> failCallback = null, Action<QGCommonResponse<string>> completeCallback = null)
        {
            QGSetUserCloudStorage(JsonUtility.ToJson(userCloudStorageParam), QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback), QGCallBackManager.Add(completeCallback));
        }

        public void SetUserCloudStorageCallBack(string msg)
        {
            QGCallBackManager.InvokeResponseCallback<QGCommonResponse<string>>(msg);
        }

        public void GetUserCloudStorage(string storageKey, Action<QGCommonResponse<UserCloudStorageParam>> successCallback = null, Action<QGCommonResponse<UserCloudStorageParam>> failCallback = null, Action<QGCommonResponse<UserCloudStorageParam>> completeCallback = null)
        {
            QGGetUserCloudStorage(storageKey, QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback), QGCallBackManager.Add(completeCallback));
        }

        public void GetUserCloudStorageCallBack(string msg)
        {
            QGCallBackManager.InvokeResponseCallback<QGCommonResponse<UserCloudStorageParam>>(msg);
        }

        public void RemoveUserCloudStorage(string storageKey)
        {
            QGRemoveUserCloudStorage(storageKey);
        }
        #endregion

        #region 设备信息 电量 

        public void GetBatteryInfo(Action<QGCommonResponse<BatteryInfoParam>> successCallback = null, Action<QGCommonResponse<BatteryInfoParam>> failCallback = null, Action<QGCommonResponse<BatteryInfoParam>> completeCallback = null)
        {
            QGGetBatteryInfo(QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback), QGCallBackManager.Add(completeCallback));
        }

        public void GetBatteryInfoCallBack(string msg)
        {
            QGCallBackManager.InvokeResponseCallback<QGCommonResponse<BatteryInfoParam>>(msg);
        }

        public string GetBatteryInfoSync()
        {
            string str = QGGetBatteryInfoSync();
            return QGGetBatteryInfoSync();
        }
        #endregion

        #region 设备信息 设备号 
        public void GetDeviceId(Action<QGCommonResponse<DeviceIdParam>> successCallback = null, Action<QGCommonResponse<DeviceIdParam>> failCallback = null, Action<QGCommonResponse<DeviceIdParam>> completeCallback = null)
        {
            QGGetDeviceId(QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback), QGCallBackManager.Add(completeCallback));
        }

        public void GetDeviceIdCallBack(string msg)
        {
            QGCallBackManager.InvokeResponseCallback<QGCommonResponse<DeviceIdParam>>(msg);
        }
        #endregion

        #region 设备信息 屏幕亮度 
        public void GetScreenBrightness(Action<QGCommonResponse<ScreenBrightnessParam>> successCallback = null, Action<QGCommonResponse<ScreenBrightnessParam>> failCallback = null, Action<QGCommonResponse<ScreenBrightnessParam>> completeCallback = null)
        {
            QGGetScreenBrightness(QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback), QGCallBackManager.Add(completeCallback));
        }

        public void GetScreenBrightnessCallBack(string msg)
        {
            QGCallBackManager.InvokeResponseCallback<QGCommonResponse<ScreenBrightnessParam>>(msg);
        }

        public void SetScreenBrightness(float param, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null, Action<QGBaseResponse> completeCallback = null)
        {
            QGSetScreenBrightness(param, QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback), QGCallBackManager.Add(completeCallback));
        }

        public void SetKeepScreenOn(bool param, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null, Action<QGBaseResponse> completeCallback = null)
        {
            QGSetKeepScreenOn(param, QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback), QGCallBackManager.Add(completeCallback));
        }
        #endregion

        #region 设备信息 地理位置 
        public void GetLocation(Action<QGCommonResponse<GetLocationParam>> successCallback = null, Action<QGCommonResponse<GetLocationParam>> failCallback = null, Action<QGCommonResponse<GetLocationParam>> completeCallback = null)
        {
            QGGetLocation(QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback), QGCallBackManager.Add(completeCallback));
        }

        public void GetLocationCallBack(string msg)
        {
            QGCallBackManager.InvokeResponseCallback<QGCommonResponse<GetLocationParam>>(msg);
        }
        #endregion

        #region 设备信息 加速计 
        public void OnAccelerometerChange(Action<QGCommonResponse<onAccelerometerChangeParam>> successCallback = null)
        {
            QGOnAccelerometerChange(QGCallBackManager.Add(successCallback));
        }

        public void OnAccelerometerChangeCallBack(string msg)
        {
            QGCallBackManager.InvokeResponseCallback<QGCommonResponse<onAccelerometerChangeParam>>(msg);
        }

        public void StartAccelerometer(string param, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null, Action<QGBaseResponse> completeCallback = null)
        {
            QGStartAccelerometer(param, QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback), QGCallBackManager.Add(completeCallback));
        }

        public void StopAccelerometer(Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null, Action<QGBaseResponse> completeCallback = null)
        {
            QGStopAccelerometer(QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback), QGCallBackManager.Add(completeCallback));
        }
        #endregion

        #region 设备信息 剪切板 
        public void SetClipboardData(string param, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null, Action<QGBaseResponse> completeCallback = null)
        {
            QGSetClipboardData(param, QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback), QGCallBackManager.Add(completeCallback));
        }

        public void GetClipboardData(Action<QGCommonResponse<string>> successCallback = null, Action<QGCommonResponse<string>> failCallback = null, Action<QGCommonResponse<string>> completeCallback = null)
        {
            QGGetClipboardData(QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback), QGCallBackManager.Add(completeCallback));
        }

        public void GetClipboardDataCallBack(string msg)
        {
            QGCallBackManager.InvokeResponseCallback<QGCommonResponse<string>>(msg);
        }
        #endregion

        #region 设备信息 罗盘 
        public void OnCompassChange(Action<QGCommonResponse<float>> successCallback = null)
        {
            QGOnCompassChange(QGCallBackManager.Add(successCallback));
        }
        public void OnCompassChangeCallBack(string msg)
        {
            QGCallBackManager.InvokeResponseCallback<QGCommonResponse<float>>(msg);
        }

        public void StartCompass(Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null, Action<QGBaseResponse> completeCallback = null)
        {
            QGStartCompass(QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback), QGCallBackManager.Add(completeCallback));
        }

        public void StopCompass(Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null, Action<QGBaseResponse> completeCallback = null)
        {
            QGStopCompass(QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback), QGCallBackManager.Add(completeCallback));
        }
        #endregion

        private Action<string> onARCameraSuccess;
        private Action<string> onARCameraFail;
        public void AddARCameraCallBack(Action<string> successCallback = null, Action<string> failCallback = null)
        {
            onARCameraSuccess = successCallback;
            onARCameraFail = failCallback;
        }

        public void OnARCameraSuccess(string msg)
        {
            onARCameraSuccess(msg);
        }

        public void OnARCameraFail(string msg)
        {
            onARCameraFail(msg);
        }

        [DllImport("__Internal")]
        private static extern void QGLogin(string s, string f);

        [DllImport("__Internal")]
        private static extern void QGHasShortcutInstalled(string s, string f);

        [DllImport("__Internal")]
        private static extern void QGInstallShortcut(string s, string f);

        [DllImport("__Internal")]
        // private static extern void QGCreateBannerAd(string a, string p, string s, int i);
        private static extern void QGCreateBannerAd(string a, string p, string s);

        [DllImport("__Internal")]
        private static extern void QGCreateRewardedVideoAd(string a, string p);

        [DllImport("__Internal")]
        private static extern void QGCreateInterstitialAd(string a, string p);

        [DllImport("__Internal")]
        private static extern void QGCreateCustomAd(string a, string p, string s);

        [DllImport("__Internal")]
        private static extern void QGCreateGameBannerAd(string a, string p);

        [DllImport("__Internal")]
        private static extern void QGCreateGamePortalAd(string a, string p);

        [DllImport("__Internal")]
        private static extern void QGCreateGameDrawerAd(string a, string p, string s);

        [DllImport("__Internal")]
        private static extern void QGShowAd(string a, string s, string f);

        [DllImport("__Internal")]
        private static extern void QGHideAd(string a, string s, string f);

        [DllImport("__Internal")]
        private static extern void QGLoadAd(string a, string s, string f);

        [DllImport("__Internal")]
        private static extern void QGDestroyAd(string a);

        [DllImport("__Internal")]
        private static extern bool QGIsShow(string a);

        // 数据存储
        [DllImport("__Internal")]
        private static extern void QGStorageSetItem(string k, string v);

        [DllImport("__Internal")]
        private static extern string QGStorageGetItem(string k);

        [DllImport("__Internal")]
        private static extern void QGStorageRemoveItem(string k);

        [DllImport("__Internal")]
        private static extern void QGPay(string p, string s, string f);

        [DllImport("__Internal")]
        private static extern void QGPayTest(string p, string s, string f);

        [DllImport("__Internal")]
        private static extern void QGGetNetworkType(string s, string f);

        [DllImport("__Internal")]
        private static extern void QGOnNetworkStatusChange(string p);

        [DllImport("__Internal")]
        private static extern void QGVibrateShort();

        [DllImport("__Internal")]
        private static extern void QGVibrateLong();

        [DllImport("__Internal")]
        private static extern void QGGetSystemInfo(string s, string f);

        [DllImport("__Internal")]
        private static extern string QGGetSystemInfoSync();

        [DllImport("__Internal")]
        private static extern void QGSetEnableDebugTrue();

        [DllImport("__Internal")]
        private static extern void QGSetEnableDebugFalse();

        [DllImport("__Internal")]
        private static extern void QGShowModal(string a, string b, string c, string d);

        [DllImport("__Internal")]
        private static extern void QGShowKeyboard(string a, string p, string s, string f, string o);

        [DllImport("__Internal")]
        private static extern void QGOnKeyboardInput(string p);

        [DllImport("__Internal")]
        private static extern void QGOffKeyboardInput();

        [DllImport("__Internal")]
        private static extern void QGOnKeyboardConfirm(string p);

        [DllImport("__Internal")]
        private static extern void QGOffKeyboardConfirm();

        [DllImport("__Internal")]
        private static extern void QGOnKeyboardComplete(string p);

        [DllImport("__Internal")]
        private static extern void QGOffKeyboardComplete();

        [DllImport("__Internal")]
        private static extern void QGHideKeyboard();

        [DllImport("__Internal")]
        private static extern void QGMkdir();

        [DllImport("__Internal")]
        private static extern void QGRmdir();

        [DllImport("__Internal")]
        private static extern void QGIsExist();

        [DllImport("__Internal")]
        private static extern void QGRename();

        [DllImport("__Internal")]
        private static extern void QGSaveFile();

        [DllImport("__Internal")]
        private static extern void QGReadDir();

        [DllImport("__Internal")]
        private static extern void QGWriteFile();

        [DllImport("__Internal")]
        private static extern void QGReadFile();

        [DllImport("__Internal")]
        private static extern void QGAppendFile();

        [DllImport("__Internal")]
        private static extern void QGCopyFile();

        [DllImport("__Internal")]
        private static extern void QGRemoveSavedFile();

        [DllImport("__Internal")]
        private static extern void QGStat();

        [DllImport("__Internal")]
        private static extern void QGUnzip();

        [DllImport("__Internal")]
        private static extern void QGGetFileInfo(string a,string b, string c);

        [DllImport("__Internal")]
        private static extern void QGPlayAudio(string a, string b);

        // [DllImport("__Internal")]
        // private static extern void QGPauseAudio();

        [DllImport("__Internal")]
        private static extern void QGOnAudioInterruptionBegin();

        [DllImport("__Internal")]
        private static extern void QGOffAudioInterruptionBegin();

        [DllImport("__Internal")]
        private static extern void QGOnAudioInterruptionEnd();

        [DllImport("__Internal")]
        private static extern void QGOffAudioInterruptionEnd();

        [DllImport("__Internal")]
        private static extern void QGOnError();

        [DllImport("__Internal")]
        private static extern void QGOffError();

        [DllImport("__Internal")]
        private static extern void QGDispatchError();

        [DllImport("__Internal")]
        private static extern void QGCreateVideo(string a, string b);
        [DllImport("__Internal")]
        private static extern void QGDownLoadFile(string a, string b, string c);
        [DllImport("__Internal")]
        private static extern void QGUploadFile(string a, string b, string c);
        [DllImport("__Internal")]
        private static extern void QGShowToast(string a);
        [DllImport("__Internal")]
        private static extern void QGPlayMedia(string a);
        [DllImport("__Internal")]
        private static extern void QGPauseMedia(string a);
        [DllImport("__Internal")]
        private static extern void QGStopMedia(string a);
        [DllImport("__Internal")]
        private static extern void QGDestroyMedia(string a);
        [DllImport("__Internal")]
        private static extern void QGSeekMedia(string a, float time);
        [DllImport("__Internal")]
        private static extern void QGLog();
        [DllImport("__Internal")]
        private static extern void QGLogClose();
        [DllImport("__Internal")]
        private static extern void QGExitApplication(string a, string b, string c, string d);
        [DllImport("__Internal")]
        private static extern void QGGetManifestInfo(string a, string b);
        [DllImport("__Internal")]
        private static extern void QGGetProvider(string a);
        [DllImport("__Internal")]
        private static extern void QGSetPreferredFramesPerSecond(int a);
        [DllImport("__Internal")]
        private static extern void QGShowLoading(string a);
        [DllImport("__Internal")]
        private static extern void QGHideLoading(string a);
        [DllImport("__Internal")]
        private static extern void QGSetTimeout(int a, string b);
        [DllImport("__Internal")]
        private static extern void QGAudioPlayerVolume(string a, float b);
        [DllImport("__Internal")]
        private static extern void QGAudioPlayerLoop(string a, bool b);
        [DllImport("__Internal")]
        private static extern void QGSetUserCloudStorage(string a, string b, string c, string d);
        [DllImport("__Internal")]
        private static extern void QGGetUserCloudStorage(string k, string b, string c, string d);
        [DllImport("__Internal")]
        private static extern void QGRemoveUserCloudStorage(string k);
        [DllImport("__Internal")]
        private static extern void QGGetBatteryInfo(string a, string b, string c);
        [DllImport("__Internal")]
        private static extern string QGGetBatteryInfoSync();
        [DllImport("__Internal")]
        private static extern void QGGetDeviceId(string a, string b, string c);
        [DllImport("__Internal")]
        private static extern void QGGetScreenBrightness(string a, string b, string c);
        [DllImport("__Internal")]
        private static extern void QGSetScreenBrightness(float a, string b, string c, string d);
        [DllImport("__Internal")]
        private static extern void QGSetKeepScreenOn(bool a, string b, string c, string d);
        [DllImport("__Internal")]
        private static extern void QGGetLocation(string a, string b, string c);
        [DllImport("__Internal")]
        private static extern void QGOnAccelerometerChange(string a);
        [DllImport("__Internal")]
        private static extern void QGStartAccelerometer(string a, string b, string c, string d);
        [DllImport("__Internal")]
        private static extern void QGStopAccelerometer(string a, string b, string c);
        [DllImport("__Internal")]
        private static extern void QGSetClipboardData(string a, string b, string c, string d);
        [DllImport("__Internal")]
        private static extern void QGGetClipboardData(string a, string b, string c);
        [DllImport("__Internal")]
        private static extern void QGOnCompassChange(string a);
        [DllImport("__Internal")]
        private static extern void QGStartCompass(string a, string b, string c);
        [DllImport("__Internal")]
        private static extern void QGStopCompass(string a, string b, string c);
    }
}
