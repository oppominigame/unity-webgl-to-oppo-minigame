using System.Collections;
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

        public void HasShortcutInstalled(Action<QGCommonResponse<QGShortcutBean>> successCallback = null, Action<QGCommonResponse<QGShortcutBean>> failCallback = null)
        {
            QGHasShortcutInstalled(QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback));
        }

        #endregion

        #region 创建桌面图标

        public void InstallShortcut(Action<QGBaseResponse> successCallback = null, Action<QGBaseResponse> failCallback = null)
        {
            QGInstallShortcut(QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback));
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
        public static void StorageGetItem(string keyName)
        {
            QGStorageGetItem(keyName);
        }
        // 清除数据 
        public static void StorageRemoveItem(string keyName)
        {
            QGStorageRemoveItem(keyName);
        }
        #endregion

        #region unity的PlayerPrefs

        public void StorageSetIntSync(string key, int value)
        {
            QGStorageSetIntSync(key, value);
        }

        public int StorageGetIntSync(string key, int defaultValue)
        {
            return QGStorageGetIntSync(key, defaultValue);
        }

        public void StorageSetStringSync(string key, string value)
        {
            QGStorageSetStringSync(key, value);
        }

        public string StorageGetStringSync(string key, string defaultValue)
        {
            return QGStorageGetStringSync(key, defaultValue);
        }

        public void StorageSetFloatSync(string key, float value)
        {
            QGStorageSetFloatSync(key, value);
        }

        public float StorageGetFloatSync(string key, float defaultValue)
        {
            return QGStorageGetFloatSync(key, defaultValue);
        }

        public void StorageDeleteAllSync()
        {
            QGStorageDeleteAllSync();
        }

        public void StorageDeleteKeySync(string key)
        {
            QGStorageDeleteKeySync(key);
        }

        public bool StorageHasKeySync(string key)
        {
            return QGStorageHasKeySync(key);
        }

        #endregion

        #region 支付
        public void Pay(PayParam param, Action<QGCommonResponse<QGPayBean>> successCallback = null, Action<QGCommonResponse<QGPayBean>> failCallback = null,Action<QGCommonResponse<QGPayBean>> completeCallback = null)
        {
            QGPay(JsonUtility.ToJson(param), QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback),QGCallBackManager.Add(completeCallback));
        }
        #endregion

        #region 判断文件是否存在
        public string AccessFile(QGAccessFileParam param)
        {
            return QGAccessFile(param.path);
        }
        #endregion

        #region 读取文件
        public void ReadFile(QGFileParam param, Action<QGFileResponse> successCallback = null, Action<QGFileResponse> failCallback = null)
        {
            QGReadFile(param.uri, param.encoding, param.position, param.length, QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback));
        }
        #endregion

        #region 写入文件
        public void WriteFile(QGFileParam param, Action<QGFileResponse> successCallback = null, Action<QGFileResponse> failCallback = null)
        {
            QGWriteFile(param.filePath, param.data, param.encoding, QGCallBackManager.Add(successCallback), QGCallBackManager.Add(failCallback));
        }
        #endregion

        #region JS回调
        public void LoginResponseCallback(string msg)
        {
            QGCallBackManager.InvokeResponseCallback<QGCommonResponse<QGLoginBean>>(msg);
        }

        // public void GetUserInfoResponseCallback(string msg)
        // {
        //     QGCallBackManager.InvokeResponseCallback<QGCommonResponse<QGUserInfoBean>>(msg);
        // }

        public void ShortcutResponseCallback(string msg)
        {
            QGCallBackManager.InvokeResponseCallback<QGCommonResponse<QGShortcutBean>>(msg);
        }

        public void DefaultResponseCallback(string msg)
        {
            QGCallBackManager.InvokeResponseCallback<QGBaseResponse>(msg);
        }

        public void PayResponseCallback(string msg)
        {
            QGCallBackManager.InvokeResponseCallback<QGCommonResponse<QGPayBean>>(msg);
        }

        public void ReadFileResponseCallback(string msg)
        {
            if (msg.Contains("utf8"))
            {
                QGCallBackManager.InvokeResponseCallback<QGFileResponse>(msg);
            }
            else
            {
                QGFileResponse response = JsonUtility.FromJson<QGFileResponse>(msg);
                var fileBuffer = new byte[response.byteLength];
                QGGetFileBuffer(fileBuffer, response.callbackId);
                response.textData = fileBuffer;
                var callback = (Action<QGFileResponse>)QGCallBackManager.responseCallBacks[response.callbackId];
                callback(response);
                QGCallBackManager.responseCallBacks.Remove(response.callbackId);
            }

        }

        public void WriteFileResponseCallback(string msg)
        {
            QGCallBackManager.InvokeResponseCallback<QGFileResponse>(msg);
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
        private static extern bool QGStorageSetItem(string k, string v);

        [DllImport("__Internal")]
        private static extern bool QGStorageGetItem(string k);

        [DllImport("__Internal")]
        private static extern bool QGStorageRemoveItem(string k);

        [DllImport("__Internal")]
        private static extern void QGStorageSetIntSync(string k, int v);

        [DllImport("__Internal")]
        private static extern int QGStorageGetIntSync(string k, int d);

        [DllImport("__Internal")]
        private static extern void QGStorageSetStringSync(string k, string v);

        [DllImport("__Internal")]
        private static extern string QGStorageGetStringSync(string k, string d);

        [DllImport("__Internal")]
        private static extern void QGStorageSetFloatSync(string k, float v);

        [DllImport("__Internal")]
        private static extern float QGStorageGetFloatSync(string k, float d);

        [DllImport("__Internal")]
        private static extern void QGStorageDeleteAllSync();

        [DllImport("__Internal")]
        private static extern void QGStorageDeleteKeySync(string k);

        [DllImport("__Internal")]
        private static extern bool QGStorageHasKeySync(string k);

        [DllImport("__Internal")]
        private static extern void QGPay(string p, string s, string f, string o);

        [DllImport("__Internal")]
        private static extern string QGAccessFile(string p);

        [DllImport("__Internal")]
        private static extern void QGReadFile(string u, string e, int p, int l, string s, string f);

        [DllImport("__Internal")]
        private static extern void QGGetFileBuffer(byte[] d, string c);

        [DllImport("__Internal")]
        private static extern void QGWriteFile(string fp, object d, string e, string c, string f);
    }
}
