using UnityEngine;
using System;



namespace QGMiniGame
{
    public class QGRewardedVideoAd : QGBaseAd
    {

        public Action<QGRewardedVideoResponse> onCloseRewardedVideoAction;

        public QGRewardedVideoAd(string adId) : base(adId)
        {

        }

        public override void Hide(Action<QGBaseResponse> success = null, Action<QGBaseResponse> failed = null)
        {
            QGLog.LogWarning("QGRewardedVideoAd no Hide Function");
        }

        public void Load(Action<QGBaseResponse> success = null, Action<QGBaseResponse> failed = null)
        {
            QGMiniGameManager.Instance.LoadAd(adId, success, failed);
        }


        public void OnClose(Action<QGRewardedVideoResponse> onClose)
        {
            onCloseRewardedVideoAction += onClose;
        }


        public void OffClose(Action<QGRewardedVideoResponse> offClose)
        {
            onCloseRewardedVideoAction -= offClose;
        }
    }
}
