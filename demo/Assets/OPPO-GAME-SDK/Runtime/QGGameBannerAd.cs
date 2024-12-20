using System;
using UnityEngine;


namespace QGMiniGame
{
    public class QGGameBannerAd : QGBaseAd
    {
        public Action onShowAction;
        public QGGameBannerAd(string adId) : base(adId)
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
    }
}
