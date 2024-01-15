﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;

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

        // public void InstallShortcut(Action<QGCommonResponse<QGHasShortcutInstalled>> successCallback = null, Action<QGCommonResponse<QGHasShortcutInstalled>> failCallback = null)
        // {
        //     QGInstallShortcut(QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback));
        // }

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

        public void ShowModal()
        {
            QGShowModal();
        }
        
        #endregion

        #region 键盘
        public void ShowKeyboard(KeyboardParam param, Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null, Action<QGBaseResponse> completeCallback = null)
        {
            QGShowKeyboard(JsonUtility.ToJson(param), QGCallBackManager.Add(successCallback), 
            QGCallBackManager.Add(failCallback),
             QGCallBackManager.Add(completeCallback));
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

        #region 获取本地临时文件或本地用户文件的文件信息

        public void GetFileInfo()
        {
            QGGetFileInfo();
        }
        
        #endregion

        #region 播放远程音频

        public void PlayAudio()
        {
            QGPlayAudio();
        }
        
        #endregion

        #region 暂停音频

        public void PauseAudio()
        {
            QGPauseAudio();
        }

        #endregion

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
            QGCreateBannerAd(adId,param.adUnitId, JsonUtility.ToJson(param.style));
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

        public QGGameBannerAd CreateGameBannerAd (QGCommonAdParam param)
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

        public QGGameDrawerAd CreateGameDrawerAd (QGCreateGameDrawerAdParam param)
        {
            var adId = QGCallBackManager.getKey();
            QGGameDrawerAd ad = new QGGameDrawerAd(adId);
            QGCreateGameDrawerAd(adId, param.adUnitId,JsonUtility.ToJson(param.style));
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
        #endregion

        #region JS回调
        public void LoginResponseCallback(string msg)
        {
            QGCallBackManager.InvokeResponseCallback<QGCommonResponse<QGLoginBean>>(msg);
        }

        public void OnKeyboardInputResponseCallback(string msg)
        {
            QGCallBackManager.InvokeResponseCallback<QGResKeyBoardponse>(msg,false);
        }

        public void ShowKeyboardResponseCallback(string msg)
        {
            QGCallBackManager.InvokeResponseCallback<QGBaseResponse>(msg,false);
        }
        

        public void OnNetworkStatusChangeResponseCallback(string msg)
        {
            QGCallBackManager.InvokeResponseCallback<QGOnNetworkStatus>(msg,false);
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

        #endregion



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
        private static extern void QGShowModal();
        
        [DllImport("__Internal")]
        private static extern void QGShowKeyboard(string p, string s, string f, string o);

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
        private static extern void QGGetFileInfo();

        [DllImport("__Internal")]
        private static extern void QGPlayAudio();
        
        [DllImport("__Internal")]
        private static extern void QGPauseAudio();

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
    }
}