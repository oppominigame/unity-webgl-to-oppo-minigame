
using System.Collections.Generic;
using UnityEngine;
using System;

namespace QGMiniGame
{
    public class QGRecordManager
    {
        public static Dictionary<string, QGRecordManager> QGRecords = new Dictionary<string, QGRecordManager>();
        public string recordId; // 录音的唯一标识 
        public Action onStartAction;    //监听录音开始事件
        public Action onResumeAction;   //监听录音继续事件
        public Action onPauseAction;    //监听录音暂停事件
        public Action<QGBaseResponse> onStopAction;     //监听录音结束事件
        public Action onFrameRecordedAction;    //监听已录制完指定帧大小的文件事件。如果设置了 frameSize，则会回调此事件
        public Action onErrorAction;    //监听录音错误事件

        public QGRecordManager(string recordId)
        {
            this.recordId = recordId;
            QGRecords.Add(recordId, this);
        }

        public virtual void Start(RecordParam recordParam = null)
        {
            QGMiniGameManager.Instance.RecorderStart(recordId, recordParam);
        }

        public virtual void Pause()
        {
            QGMiniGameManager.Instance.RecorderPause(recordId);
        }
        public virtual void Resume()
        {
            QGMiniGameManager.Instance.RecorderResume(recordId);
        }

        public virtual void Stop()
        {
            QGMiniGameManager.Instance.RecorderStop(recordId);
        }

        public virtual void OnStart(Action onStart)
        {
            onStartAction += onStart;
        }

        public virtual void OffStart(Action offStart)
        {
            onStartAction -= offStart;
        }


        public virtual void OnResume(Action onResume)
        {
            onResumeAction += onResume;
        }

        public virtual void OffResume(Action offResume)
        {
            onResumeAction -= offResume;
        }

        public virtual void OnPause(Action onPause)
        {
            onPauseAction += onPause;
        }

        public virtual void OffPause(Action offPause)
        {
            onPauseAction -= offPause;
        }

        public virtual void OnStop(Action<QGBaseResponse> onStop)
        {
            onStopAction += onStop;
        }

        public virtual void OffStop(Action<QGBaseResponse> offStop)
        {
            onStopAction -= offStop;
        }


        public virtual void OnFrameRecorded(Action onFrameRecorded)
        {
            onFrameRecordedAction += onFrameRecorded;
        }

        public virtual void OffFrameRecorded(Action offFrameRecorded)
        {
            onFrameRecordedAction -= offFrameRecorded;
        }

        public virtual void OnError(Action onError)
        {
            onErrorAction += onError;
        }

        public virtual void OffError(Action offError)
        {
            onErrorAction -= offError;
        }
    }
}
