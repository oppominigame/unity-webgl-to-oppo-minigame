using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QGMiniGame
{
    public class QGCallBackManager
    {
        public static readonly Hashtable responseCallBacks = new Hashtable();

        private static int id = 0;

        private static int GenarateId()
        {
            if (id > 1000000)
            {
                id = 0;
            }
            id++;

            return id;
        }

        public static string Add<T>(Action<T> callback)
            where T : QGBaseResponse
        {
            if (callback == null)
            {
                return "";
            }
            var key = getKey();
            responseCallBacks.Add (key, callback);
            return key;
        }

        public static string getKey()
        {
            int id = GenarateId();
            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            var time = Convert.ToInt64(ts.TotalSeconds);
            return (time.ToString() + '-' + id);
        }

        public static void InvokeResponseCallback<T>(string str, bool remove = true)
            where T : QGBaseResponse
        {
          QGLog.LogWarning("remove = " + remove);
            if (str != null)
            {
                T res = JsonUtility.FromJson<T>(str);
                var id = res.callbackId;
                if (responseCallBacks[id] != null)
                {
                    var callback = (Action<T>) responseCallBacks[id];
                    callback (res);
                    if (remove)
                    {
                        responseCallBacks.Remove (id);
                    }
                }
                else
                {
                    QGLog
                        .LogWarning("InvokeResponseCallback responseCallBacks get null id = " +
                        id);
                }
            }
            else
            {
                QGLog.LogWarning("InvokeResponseCallback str is null");
            }
        }
    }
}
