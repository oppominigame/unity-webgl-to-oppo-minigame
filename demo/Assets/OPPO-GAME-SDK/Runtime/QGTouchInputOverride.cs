using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace QGMiniGame
{
    internal class TouchData
    {
        public Touch touch;
        public long timeStamp;
    }

    [RequireComponent(typeof(StandaloneInputModule))]
    public class QGTouchInputOverride : BaseInput
    {
        private readonly List<TouchData> mTouches = new List<TouchData>();
        private StandaloneInputModule mStandaloneInputModule = null;
        private string mTouchStartCallbackKey = null;
        private string mTouchMoveCallbackKey = null;
        private string mTouchEndCallbackKey = null;
        private string mTouchCancelCallbackKey = null;

        protected override void Awake()
        {
            base.Awake();
            mStandaloneInputModule = GetComponent<StandaloneInputModule>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            RegisterTouchEvents();
            if (mStandaloneInputModule)
            {
                mStandaloneInputModule.inputOverride = this;
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            UnregisterTouchEvents();
            if (mStandaloneInputModule)
            {
                mStandaloneInputModule.inputOverride = null;
            }
        }

        public override bool touchSupported
        {
            get
            {
                return true;
            }
        }

        public override bool mousePresent
        {
            get
            {
                return false;
            }
        }

        public override int touchCount
        {
            get
            {
                return mTouches.Count;
            }
        }

        public override Touch GetTouch(int index)
        {
            return mTouches[index].touch;
        }

        private void LateUpdate()
        {
            foreach (var touchData in mTouches)
            {
                if (touchData.touch.phase == TouchPhase.Began)
                {
                    touchData.touch.phase = TouchPhase.Stationary;
                }
            }
            RemoveEndedTouches();
        }

        private void RegisterTouchEvents()
        {
            mTouchStartCallbackKey = QGMiniGameManager.Instance.OnTouchStart(OnTouchStart);
            mTouchMoveCallbackKey = QGMiniGameManager.Instance.OnTouchMove(OnTouchMove);
            mTouchEndCallbackKey = QGMiniGameManager.Instance.OnTouchEnd(OnTouchEnd);
            mTouchCancelCallbackKey = QGMiniGameManager.Instance.OnTouchCancel(OnTouchCancel);
        }

        private void UnregisterTouchEvents()
        {
            if (!string.IsNullOrEmpty(mTouchStartCallbackKey))
            {
                QGMiniGameManager.Instance.OffTouchStart(mTouchStartCallbackKey);
                mTouchStartCallbackKey = null;
            }
            if (!string.IsNullOrEmpty(mTouchMoveCallbackKey))
            {
                QGMiniGameManager.Instance.OffTouchMove(mTouchMoveCallbackKey);
                mTouchMoveCallbackKey = null;
            }
            if (!string.IsNullOrEmpty(mTouchEndCallbackKey))
            {
                QGMiniGameManager.Instance.OffTouchEnd(mTouchEndCallbackKey);
                mTouchEndCallbackKey = null;
            }
            if (!string.IsNullOrEmpty(mTouchCancelCallbackKey))
            {
                QGMiniGameManager.Instance.OffTouchCancel(mTouchCancelCallbackKey);
                mTouchCancelCallbackKey = null;
            }
        }

        private void OnTouchStart(QGTouchData touchData)
        {
            foreach (var touch in touchData.changedTouches)
            {
                var data = FindOrCreateTouchData(touch.identifier);
                data.touch.phase = TouchPhase.Began;
                data.touch.position = new Vector2(touch.clientX, touch.clientY);
                data.touch.rawPosition = data.touch.position;
                data.timeStamp = touchData.timeStamp;
            }
        }

        private void OnTouchMove(QGTouchData touchData)
        {
            foreach (var touch in touchData.changedTouches)
            {
                var data = FindOrCreateTouchData(touch.identifier);
                UpdateTouchData(data, new Vector2(touch.clientX, touch.clientY), touchData.timeStamp, TouchPhase.Moved);
            }
        }

        private void OnTouchEnd(QGTouchData touchData)
        {
            foreach (var touch in touchData.changedTouches)
            {
                TouchData data = FindTouchData(touch.identifier);
                if (data == null)
                {
                    Debug.LogError($"OnTouchEnd, error identifier: {touch.identifier}");
                    return;
                }
                if (data.touch.phase == TouchPhase.Canceled || data.touch.phase == TouchPhase.Ended)
                {
                    Debug.LogWarning($"OnTouchEnd, error phase: {touch.identifier}, phase:{data.touch.phase}");
                }
                UpdateTouchData(data, new Vector2(touch.clientX, touch.clientY), touchData.timeStamp, TouchPhase.Ended);
            }
        }

        private void OnTouchCancel(QGTouchData touchData)
        {
            foreach (var touch in touchData.changedTouches)
            {
                TouchData data = FindTouchData(touch.identifier);
                if (data == null)
                {
                    Debug.LogError($"OnTouchCancel, error identifier: {touch.identifier}");
                    return;
                }
                if (data.touch.phase == TouchPhase.Canceled || data.touch.phase == TouchPhase.Ended)
                {
                    Debug.LogWarning($"OnTouchCancel, error phase: {touch.identifier}, phase:{data.touch.phase}");
                }
                UpdateTouchData(data, new Vector2(touch.clientX, touch.clientY), touchData.timeStamp, TouchPhase.Canceled);
            }
        }

        private TouchData FindOrCreateTouchData(int identifier)
        {
            var touchData = FindTouchData(identifier);
            if (touchData != null)
            {
                return touchData;
            }
            var data = new TouchData();
            data.touch.pressure = 1.0f;
            data.touch.maximumPossiblePressure = 1.0f;
            data.touch.type = TouchType.Direct;
            data.touch.tapCount = 1;
            data.touch.fingerId = identifier;
            data.touch.radius = 0;
            data.touch.radiusVariance = 0;
            data.touch.altitudeAngle = 0;
            data.touch.azimuthAngle = 0;
            data.touch.deltaTime = 0;
            mTouches.Add(data);
            return data;
        }

        private TouchData FindTouchData(int identifier)
        {
            foreach (var touchData in mTouches)
            {
                var touch = touchData.touch;
                if (touch.fingerId == identifier)
                {
                    return touchData;
                }
            }
            return null;
        }

        private static void UpdateTouchData(TouchData data, Vector2 pos, long timeStamp, TouchPhase phase)
        {
            data.touch.phase = phase;
            data.touch.deltaPosition = pos - data.touch.position;
            data.touch.position = pos;
            data.touch.deltaTime = (timeStamp - data.timeStamp) / 1000.0f;
        }

        private void RemoveEndedTouches()
        {
            if (mTouches.Count > 0)
            {
                mTouches.RemoveAll(touchData =>
                {
                    var touch = touchData.touch;
                    return touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled;
                });
            }
        }
    }
}