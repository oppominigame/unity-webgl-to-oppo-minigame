using System;
using UnityEngine;


namespace QGMiniGame
{
    public class QGAudioPlayer : QGBasePlayer
    {
        public QGAudioPlayer(string playerId) : base(playerId)
        {

        }


        //volume 0~1
        public virtual void SetVolume(float volume)
        {
            QGMiniGameManager.Instance.AudioPlayerVolume(playerId, volume);
        }

        public virtual void SetLoop(bool bl)
        {
            QGMiniGameManager.Instance.AudioPlayerLoop(playerId, bl);
        }
    }
}
