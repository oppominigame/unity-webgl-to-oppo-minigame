using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace QGMiniGame
{
    public class QGLog
    {
        private static string TAG = "QGMiniGame : ";

        public static void Log(string msg)
        {
            Debug.Log(TAG + msg);
        }

        public static void LogWarning(string msg)
        {
            Debug.LogWarning(TAG + msg);
        }

        public static void LogError(string msg)
        {
            Debug.LogError(TAG + msg);
        }
    }
}

