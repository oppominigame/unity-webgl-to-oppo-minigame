using UnityEngine;
using UnityEngine.UI;
using QGMiniGame;
using UnityEngine.SceneManagement;

public class qgpay : MonoBehaviour
{
    public Button comebackbtn;

    public Button qgPaytestBtn;

    public Button qgPayBtn;

    void Start()
    {
        comebackbtn.onClick.AddListener(comebackfunc);
        qgPaytestBtn.onClick.AddListener(playQGPayTest);
        qgPayBtn.onClick.AddListener(playQGPay);
    }

    public void comebackfunc()
    {
        SceneManager.LoadScene("main");
    }


    public void playQGPayTest()
    {
        //支付先登录拉数据 
        QG.Login((msg) =>
        {
            Debug.Log("QG.Login success = " + JsonUtility.ToJson(msg));
            if (msg.data.token != string.Empty)
            {
                qgPaytestFunc(msg.data.token);
            }
            else
            {
                Debug.Log("The platform token fails to be obtained. Procedure");
            }
        },
          (msg) =>
          {
              Debug.Log("QG.Login fail = " + msg.errMsg);
          });
    }

    public void playQGPay()
    {
        //支付先登录拉数据 
        QG.Login((msg) =>
        {
            Debug.Log("QG.Login success = " + JsonUtility.ToJson(msg));
            if (msg.data.token != string.Empty)
            {
                qgPayFunc(msg.data.token);
            }
            else
            {
                Debug.Log("The platform token fails to be obtained. Procedure");
            }
        },
          (msg) =>
          {
              Debug.Log("QG.Login fail = " + msg.errMsg);
          });
    }

    public void qgPaytestFunc(string parameterToken)
    {
        PayTestParam param =
                  new PayTestParam()
                  {
                      appId = 30173650,
                      openId = parameterToken,
                      productName = "测试礼包",
                      productDesc = "测试支付",
                      count = 1, //商品数量（只能传1） 
                      price = 1, //商品价格，以分为单位
                      currency = "CNY", //币种，人民币如：CNY
                      callBackUrl = "", // 服务器接收平台返回数据的接口回调地址
                      cpOrderId = "1.0", //CP自己的订单号
                      appVersion = "1.0.0", //游戏版本
                      deviceInfo = "", //设备号 
                                       //model = "", //机型 
                      ip = "", //终端IP 
                      attach = ""//附加信息 
                  };
        QG
            .PayTest(param,
            (msg) =>
            {
                Debug.Log("QG.Pay success = " + JsonUtility.ToJson(msg));
            },
            (msg) =>
            {
                Debug.Log("QG.Pay fail = " + JsonUtility.ToJson(msg));
            });
    }

    public void qgPayFunc(string parameterToken)
    {
        PayParam param =
                         new PayParam()
                         {
                             appId = 0,
                             openId = "",
                             timestamp = 0,
                             orderNo = "",
                             paySign = ""
                         };
        QG
            .Pay(param,
            (msg) =>
            {
                Debug.Log("QG.Pay success = " + JsonUtility.ToJson(msg));
            },
                (msg) =>
                {
                    Debug.Log("QG.Pay fail = " + JsonUtility.ToJson(msg));
                });
    }

}
