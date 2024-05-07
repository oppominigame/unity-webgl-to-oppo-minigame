
using System.Collections.Generic;
using UnityEngine;
using System;

namespace QGMiniGame
{
    public class QGBasePlayer
    {

        public static Dictionary<string, QGBasePlayer> QGPlayers = new Dictionary<string, QGBasePlayer>();

        public string playerId; // 播放器的唯一标识 
        public Action onPlayAction;
        public Action onCanPlayAction;
        public Action onPauseAction;
        public Action onStopAction;
        public Action onEndedAction;
        public Action onTimeUpdateAction;
        public Action onErrorAction;
        public Action onWaitingAction;
        public Action onSeekingAction;
        public Action onSeekedAction;

        public QGBasePlayer(string playerId)
        {
            this.playerId = playerId;
            QGPlayers.Add(playerId, this);
        }

        public virtual void Play()
        {
            QGMiniGameManager.Instance.PlayMedia(playerId);
        }

        public virtual void Pause()
        {
            QGMiniGameManager.Instance.PauseMedia(playerId);
        }

        public virtual void Stop()
        {
            QGMiniGameManager.Instance.StopMedia(playerId);
        }

        public virtual void Seek(float time)
        {
            QGMiniGameManager.Instance.SeekMedia(playerId,time);
        }

        public void Destroy()
        {
            QGMiniGameManager.Instance.DestroyMedia(playerId);
            QGPlayers.Remove(playerId);
        }

        public virtual void OnPlay(Action onPlay)
        {
            onPlayAction += onPlay;
        }

        public virtual void OffPlay(Action offPlay)
        {
            onPlayAction -= offPlay;
        }

        public virtual void OnCanPlay(Action onCanPlay)
        {
            onCanPlayAction += onCanPlay;
        }

        public virtual void OffCanPlay(Action offCanPlay)
        {
            onCanPlayAction -= offCanPlay;
        }

        public virtual void OnPause(Action onPause)
        {
            onPauseAction += onPause;
        }

        public virtual void OffPause(Action offPause)
        {
            onPauseAction -= offPause;
        }

        public virtual void OnStop(Action onStop)
        {
            onStopAction += onStop;
        }

        public virtual void OffStop(Action offStop)
        {
            onStopAction -= offStop;
        }

        public virtual void OnEnded(Action onEnded)
        {
            onEndedAction += onEnded;
        }

        public virtual void OffEnded(Action offEnded)
        {
            onEndedAction -= offEnded;
        }
        public virtual void OnTimeUpdate(Action onTimeUpdate)
        {
            onTimeUpdateAction += onTimeUpdate;
        }

        public virtual void OffTimeUpdate(Action offTimeUpdate)
        {
            onTimeUpdateAction -= offTimeUpdate;
        }
        public virtual void OnError(Action onError)
        {
            onErrorAction += onError;
        }

        public virtual void OffError(Action offError)
        {
            onErrorAction -= offError;
        }
        public virtual void OnWaiting(Action onWaiting)
        {
            onWaitingAction += onWaiting;
        }

        public virtual void OffWaiting(Action offWaiting)
        {
            onWaitingAction -= offWaiting;
        }
        public virtual void OnSeeking(Action onSeeking)
        {
            onSeekingAction += onSeeking;
        }

        public virtual void OffSeeking(Action offSeeking)
        {
            onSeekingAction -= offSeeking;
        }
        public virtual void OnSeeked(Action onSeeked)
        {
            onSeekedAction += onSeeked;
        }

        public virtual void OffSeeked(Action offSeeked)
        {
            onSeekedAction -= offSeeked;
        }
    }
}
