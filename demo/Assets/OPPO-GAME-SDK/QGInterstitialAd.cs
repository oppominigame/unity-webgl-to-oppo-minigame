using System;
using UnityEngine;

namespace QGMiniGame
{
    public class QGInterstitialAd : QGBaseAd
    {


        public QGInterstitialAd(string adId) : base(adId)
        {

        }

        public override void Hide(Action<QGBaseResponse> success = null, Action<QGBaseResponse> failed = null)
        {
            QGLog.LogWarning("QGInterstitialAd no Hide Function");
        }
    }
}
