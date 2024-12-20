using System;
using UnityEngine;


namespace QGMiniGame
{
    public class QGGameDrawerAd : QGBaseAd
    {
        public Action onShowAction;

        public QGGameDrawerAd(string adId) : base(adId)
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
