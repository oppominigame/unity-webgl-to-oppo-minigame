using System;
using UnityEngine;


namespace QGMiniGame
{
    public class QGCustomAd : QGBaseAd
    {
        public Action onShowAction;

        public QGCustomAd(string adId) : base(adId)
        {

        }

        public void OnShow(Action onShow)
        {
            onShowAction += onShow;
        }


        public void OffShow(Action offShow)
        {
            onShowAction -= offShow;
        }

        public bool IsShow()
        {
            return QGMiniGameManager.Instance.IsShow(adId);
        }
    }
}
