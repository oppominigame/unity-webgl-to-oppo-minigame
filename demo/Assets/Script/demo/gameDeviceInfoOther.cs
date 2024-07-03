using UnityEngine;
using UnityEngine.UI;
using QGMiniGame;
using UnityEngine.SceneManagement;

public class gameDeviceInfoOther : MonoBehaviour
{
    public Button comebackbtn;

    public Button onAccelerometerChangebtn;

    public Button startAccelerometerbtn;

    public Button stopAccelerometerbtn;

    public Button setClipboardDatabtn;
    public Button getClipboardDatabtn;

    public Button startCompassbtn;
    public Button stopCompassbtn;
    public Button onCompassChangebtn;

    public Text loginMessage;

    void Start()
    {
        comebackbtn.onClick.AddListener(comebackfunc);
        onAccelerometerChangebtn.onClick.AddListener(onAccelerometerChangeFunc);
        startAccelerometerbtn.onClick.AddListener(startAccelerometerFunc);
        stopAccelerometerbtn.onClick.AddListener(stopAccelerometerFunc);

        setClipboardDatabtn.onClick.AddListener(setClipboardDataFunc);
        getClipboardDatabtn.onClick.AddListener(getClipboardDataFunc);

        startCompassbtn.onClick.AddListener(startCompassFunc);
        stopCompassbtn.onClick.AddListener(stopCompassFunc);
        onCompassChangebtn.onClick.AddListener(onCompassChangeFunc);
    }

    public void comebackfunc()
    {
        SceneManager.LoadScene("main");
    }

    void onAccelerometerChangeFunc()
    {
        QG.OnAccelerometerChange(
        (success) =>
        {
            Debug.Log("QG.OnAccelerometerChange success = " + JsonUtility.ToJson(success));
            loginMessage.text = "监听加速度数据: \n" + JsonUtility.ToJson(success);
        });
    }

    void startAccelerometerFunc()
    {
        QG.StartAccelerometer(
          "game",
        (success) =>
        {
            Debug.Log("QG.StartAccelerometer success = " + JsonUtility.ToJson(success));
            loginMessage.text = "开始监听加速度数据: \n" + JsonUtility.ToJson(success);
        },
        (fail) =>
        {
            Debug.Log("QG.StartAccelerometer fail = " + JsonUtility.ToJson(fail));
            loginMessage.text = "开始监听加速度数据: 失败 \n" + JsonUtility.ToJson(fail);
        },
        (complete) =>
        {
            Debug.Log("开始监听加速度数据 complete");
        });
    }

    void stopAccelerometerFunc()
    {
        QG.StopAccelerometer(
        (success) =>
        {
            Debug.Log("QG.StopAccelerometer success = " + JsonUtility.ToJson(success));
            loginMessage.text = "停止监听加速度数据: \n" + JsonUtility.ToJson(success);
        },
        (fail) =>
        {
            Debug.Log("QG.StopAccelerometer fail = " + JsonUtility.ToJson(fail));
            loginMessage.text = "停止监听加速度数据: 失败 \n" + JsonUtility.ToJson(fail);
        },
        (complete) =>
        {
            Debug.Log("停止监听加速度数据 complete");
        });
    }


    void setClipboardDataFunc()
    {
        QG.SetClipboardData(
          "测试剪切板",
        (success) =>
        {
            Debug.Log("QG.SetClipboardData success = " + JsonUtility.ToJson(success));
            loginMessage.text = "设置系统剪贴板的内容: \n" + JsonUtility.ToJson(success);
        },
        (fail) =>
        {
            Debug.Log("QG.SetClipboardData fail = " + JsonUtility.ToJson(fail));
            loginMessage.text = "设置系统剪贴板的内容: 失败 \n" + JsonUtility.ToJson(fail);
        },
        (complete) =>
        {
            Debug.Log("设置系统剪贴板的内容 complete");
        });
    }


    void getClipboardDataFunc()
    {
        QG.GetClipboardData(
        (success) =>
        {
            Debug.Log("QG.GetClipboardData success = " + JsonUtility.ToJson(success));
            loginMessage.text = "获取系统剪贴板的内容: \n" + JsonUtility.ToJson(success);
        },
        (fail) =>
        {
            Debug.Log("QG.GetClipboardData fail = " + JsonUtility.ToJson(fail));
            loginMessage.text = "获取系统剪贴板的内容: 失败 \n" + JsonUtility.ToJson(fail);
        },
        (complete) =>
        {
            Debug.Log("获取系统剪贴板的内容 complete");
        });
    }


    void startCompassFunc()
    {
        QG.StartCompass(
        (success) =>
        {
            Debug.Log("QG.StartCompass success = " + JsonUtility.ToJson(success));
            loginMessage.text = "开始监听罗盘数据: \n" + JsonUtility.ToJson(success);
        },
        (fail) =>
        {
            Debug.Log("QG.StartCompass fail = " + JsonUtility.ToJson(fail));
            loginMessage.text = "开始监听罗盘数据: 失败 \n" + JsonUtility.ToJson(fail);
        },
        (complete) =>
        {
            Debug.Log("开始监听罗盘数据 complete");
        });
    }

    void stopCompassFunc()
    {
        QG.StopCompass(
        (success) =>
        {
            Debug.Log("QG.StopCompass success = " + JsonUtility.ToJson(success));
            loginMessage.text = "停止监听罗盘数据: \n" + JsonUtility.ToJson(success);
        },
        (fail) =>
        {
            Debug.Log("QG.StopCompass fail = " + JsonUtility.ToJson(fail));
            loginMessage.text = "停止监听罗盘数据: 失败 \n" + JsonUtility.ToJson(fail);
        },
        (complete) =>
        {
            Debug.Log("停止监听罗盘数据 complete");
        });
    }

    void onCompassChangeFunc()
    {
        QG.OnCompassChange(
        (success) =>
        {
            Debug.Log("QG.OnCompassChange success = " + JsonUtility.ToJson(success));
            loginMessage.text = "监听罗盘数据: \n" + JsonUtility.ToJson(success);
        });
    }
}
