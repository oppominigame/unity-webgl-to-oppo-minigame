using System;
using UnityEngine;


namespace QGMiniGame
{
    public class QGGamePortalAd : QGBaseAd
    {

        // public Action onShowAction;

        public QGGamePortalAd(string adId) : base(adId)
        {

        }

        // public void OnShow(Action onShow)
        // {
        //     onShowAction += onShow;
        // }


        // public void OffShow(Action offShow)
        // {
        //     onShowAction -= offShow;
        // }

        public void Load(Action<QGBaseResponse> success = null, Action<QGBaseResponse> failed = null)
        {
            QGMiniGameManager.Instance.LoadAd(adId, success, failed);
        }
    }
}

