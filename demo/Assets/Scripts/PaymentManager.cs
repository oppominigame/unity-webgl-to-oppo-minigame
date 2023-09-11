using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using QGMiniGame;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using LitJson;

public class PaymentManager : MonoBehaviour
{
    // 统一下单接口URL
    // 正式的请求使用：https://jits.open.oppomobile.com/jitsopen/api/pay/v1.0/preOrder
    private string url = "https://jits.open.oppomobile.com/jitsopen/api/pay/demo/preOrder";
    
    // 处理JSON数据生成实体类
    public class Data
    {
        public long timestamp { get; set; }
        public string orderNo { get; set; }
        public string paySign { get; set; }
    }

    public class Result
    {
        public string code { get; set; }
        public string msg { get; set; }
        public Data data { get; set; }
    }

    private void Start()
    {
    }

    public void playPayOrder()
    {
        QG
            .Login((msg) =>
            {
                Debug.Log("QG.Login success = " + JsonUtility.ToJson(msg));
                Debug.Log("msg = " + msg);
                Debug.Log("msg.data.token = " + msg.data.token);
                string token = msg.data.token;
                Debug.Log("token = " + token);

                // 发起支付请求
                RequestPayment(token);
            },
            (msg) =>
            {
                Debug.Log("QG.Login fail = " + msg.errMsg);
            });
    }

    private void RequestPayment(string token)
    {
        // 构建支付参数  统一下单必填的数据（除了sign）
        Dictionary<string, object> payParams = new Dictionary<string, object>();
        // payParams.Add("appId", "30173650");
        payParams.Add("openId", token);
        // payParams.Add("timestamp", timestamp);
        payParams.Add("deviceInfo", "");
        payParams.Add("model", "PAAM00");
        payParams.Add("ip", "10.102.217.239");
        payParams.Add("productName", "测试");
        payParams.Add("productDesc", "testpay");
        payParams.Add("count", "1");
        payParams.Add("price", "1");
        payParams.Add("currency", "CNY");
        payParams.Add("attach", "");
        payParams.Add("appVersion", "1.0.0");
        payParams.Add("engineVersion", "1045");
        // payParams.Add("callbackUrl", "http://127.0.0.1:3000/payResult"); // 服务器接收平台返回数据的接口回调地址

        // JsonConvert解析JSON   JSON序列化  把对象转换成json字符串
        string payParamsJson = JsonConvert.SerializeObject(payParams);
        Debug.Log("payParamsJson = " + payParamsJson);

        // 发起支付请求
        StartCoroutine(SendPaymentRequest(url, payParamsJson, token));
    }

    private IEnumerator SendPaymentRequest(string url, string payParamsJson, string token)
    {
        // 发起网络请求
        using (
            UnityWebRequest request = new UnityWebRequest(url,"POST")
            // UnityWebRequest request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST)
        )
        {
            byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(payParamsJson);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(postBytes);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json;charset=utf-8");

            yield return request.SendWebRequest();
            // if (request.result == UnityWebRequest.Result.Success)     // 2020.1及以上版本
            if ( request.isNetworkError || request.isHttpError)   // 2019.4及以下版本
            {
              // 网络请求失败
              Debug.Log("Payment request failed11111111: " + request.error);
            }
            else
            {
                string response = request.downloadHandler.text;
                Debug.Log("Payment request response success: " + response);

                // 解析支付结果
                Result result = JsonMapper.ToObject<Result>(request.downloadHandler.text);
                string code = result.code;
                string message = result.msg; 
                long timestamp = result.data.timestamp;
                string orderNo = result.data.orderNo;
                string paySign = result.data.paySign;

                Debug.Log("result: code = " + code);
                Debug.Log("result: message = " + message);
                Debug.Log("result: timestamp = " + timestamp);
                Debug.Log("result: orderNo = " + orderNo);
                Debug.Log("result: paySign = " + paySign);

                if (code == "200")
                {
                    // 调用OPPO小游戏SDK的支付接口
                    PayParam param =
                        new PayParam()
                        {
                            appId = 30173650,
                            token = token,
                            timestamp = timestamp, //可填写修改自己的时间戳
                            orderNo = orderNo,
                            paySign = paySign
                            // paySign 由 CP 服务端使用 appKey (不是 appId )、orderNo、timestamp 进行签名算法生成返回
                        };
                    Debug.Log("param = " + JsonConvert.SerializeObject(param));
                    QG
                        .Pay(param,
                        (msg) =>
                        {
                            Debug
                                .Log("QG.Pay success = " +
                                JsonUtility.ToJson(msg));
                        },
                        (msg) =>
                        {
                            Debug
                                .Log("QG.Pay fail = " +
                                JsonUtility.ToJson(msg));
                        });
                }
                else
                {
                    // 支付请求失败
                    Debug.Log("Payment request failed2222222: " + message);
                }
            }
        }
    }
}
