
using System.Collections.Generic;
using UnityEngine;
using System;

namespace QGMiniGame
{
    public class QGBaseAd
    {

        public static Dictionary<string, QGBaseAd> QGAds = new Dictionary<string, QGBaseAd>();

        public string adId; // 广告的唯一标识 
        public Action onLoadAction;
        public Action<QGBaseResponse> onErrorAction;
        public Action onCloseAction;
        public Action onHideAction;

        public QGBaseAd(string adId)
        {
            this.adId = adId;
            QGAds.Add(adId, this);
        }

        public virtual void Show(Action<QGBaseResponse> success = null, Action<QGBaseResponse> failed = null)
        {
            QGMiniGameManager.Instance.ShowAd(adId, success, failed);
        }

        public virtual void Load(Action<QGBaseResponse> success = null, Action<QGBaseResponse> failed = null)
        {
            QGMiniGameManager.Instance.LoadAd(adId, success, failed);
        }

        public virtual void OnLoad(Action onLoad)
        {
            onLoadAction += onLoad;
        }


        public virtual void OffLoad(Action offLoad)
        {
            onLoadAction -= offLoad;
        }

        public void OnError(Action<QGBaseResponse> onError)
        {
            onErrorAction += onError;
        }

        public void OffError(Action<QGBaseResponse> offError)
        {
            onErrorAction -= offError;
        }

        public virtual void OnClose(Action onClose)
        {
            onCloseAction += onClose;
        }

        public virtual void OffClose(Action offClose)
        {
            onCloseAction -= offClose;
        }

        public void OnHide(Action onHide)
        {
            onHideAction += onHide;
        }

        public void OffHide(Action offHide)
        {
            onHideAction -= offHide;
        }

        public virtual void Hide(Action<QGBaseResponse> success = null, Action<QGBaseResponse> failed = null)
        {
            QGMiniGameManager.Instance.HideAd(adId, success, failed);
        }

        public void Destroy()
        {
            QGMiniGameManager.Instance.DestroyAd(adId);
            QGAds.Remove(adId);
        }

    }
}
