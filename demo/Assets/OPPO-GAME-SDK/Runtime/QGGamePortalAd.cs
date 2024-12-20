using System;
using UnityEngine;


namespace QGMiniGame
{
    public class QGGamePortalAd : QGBaseAd
    {

        public QGGamePortalAd(string adId) : base(adId)
        {

        }

        public void Load(Action<QGBaseResponse> success = null, Action<QGBaseResponse> failed = null)
        {
            QGMiniGameManager.Instance.LoadAd(adId, success, failed);
        }
    }
}

