using System;
using UnityEngine;


namespace QGMiniGame
{
    public class QGCustomAd : QGBaseAd
    {
        public QGCustomAd(string adId) : base(adId)
        {

        }


        public bool IsShow()
        {
            return QGMiniGameManager.Instance.IsShow(adId);
        }
    }
}
