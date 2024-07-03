using UnityEngine;
using UnityEngine.UI;
using QGMiniGame;
using UnityEngine.SceneManagement;

public class gameDeviceInfo : MonoBehaviour
{
    public Button comebackbtn;

    public Button getBatteryInfobtn;

    public Button getBatteryInfoSyncbtn;

    public Button getDeviceIdbtn;

    public Button setScreenBrightnessbtn;
    public Button setKeepScreenOnbtn;
    public Button getScreenBrightnessbtn;

    public Button getLocationbtn;

    public Text loginMessage;

    public Scrollbar myScrollbar;

    private bool iskeepScreenOn = false;

    void Start()
    {
        comebackbtn.onClick.AddListener(comebackfunc);
        getBatteryInfobtn.onClick.AddListener(getBatteryInfoFunc);
        getBatteryInfoSyncbtn.onClick.AddListener(getBatteryInfoSyncFunc);
        getDeviceIdbtn.onClick.AddListener(getDeviceIdFunc);
        setScreenBrightnessbtn.onClick.AddListener(setScreenBrightnessFunc);
        setKeepScreenOnbtn.onClick.AddListener(setKeepScreenOnFunc);
        getScreenBrightnessbtn.onClick.AddListener(getScreenBrightnessFunc);
        getLocationbtn.onClick.AddListener(getLocationFunc);
    }

    public void comebackfunc()
    {
        SceneManager.LoadScene("main");
    }

    void getBatteryInfoFunc()
    {
        QG.GetBatteryInfo(
        (success) =>
        {
            Debug.Log("QG.GetBatteryInfo success = " + JsonUtility.ToJson(success));
            loginMessage.text = "异步电量信息: \n" + JsonUtility.ToJson(success);
        },
        (fail) =>
        {
            Debug.Log("QG.GetBatteryInfo fail = " + JsonUtility.ToJson(fail));
            loginMessage.text = "异步电量信息: 失败 \n" + JsonUtility.ToJson(fail);
        },
        (complete) =>
        {
            Debug.Log("异步电量信息 complete");
        });
    }

    void getBatteryInfoSyncFunc()
    {
        BatteryInfoParam batteryInfoParam = QG.GetBatteryInfoSync();
        loginMessage.text = "同步电量信息: \nlevel: " + batteryInfoParam.level + "\nisCharging:" + batteryInfoParam.isCharging;
        Debug.Log("同步电量信息: \nlevel: " + batteryInfoParam.level + "\nisCharging:" + batteryInfoParam.isCharging);
    }

    void getDeviceIdFunc()
    {
        QG.GetDeviceId(
        (success) =>
        {
            Debug.Log("QG.GetDeviceId success = " + JsonUtility.ToJson(success));
            loginMessage.text = "获取设备唯一标识: \n" + JsonUtility.ToJson(success);
        },
        (fail) =>
        {
            Debug.Log("QG.GetDeviceId fail = " + JsonUtility.ToJson(fail));
            loginMessage.text = "获取设备唯一标识: 失败 \n" + JsonUtility.ToJson(fail);
        },
        (complete) =>
        {
            Debug.Log("获取设备唯一标识 complete");
        });
    }

    void getScreenBrightnessFunc()
    {
        QG.GetScreenBrightness(
       (success) =>
       {
           Debug.Log("QG.GetScreenBrightness success = " + JsonUtility.ToJson(success));
           loginMessage.text = "获取设备亮度: \n" + JsonUtility.ToJson(success);
       },
       (fail) =>
       {
           Debug.Log("QG.GetScreenBrightness fail = " + JsonUtility.ToJson(fail));
           loginMessage.text = "获取设备亮度: 失败 \n" + JsonUtility.ToJson(fail);
       },
       (complete) =>
       {
           Debug.Log("获取设备亮度 complete");
       });
    }


    void setScreenBrightnessFunc()
    {
        float currentScrollbarValue = myScrollbar.value;
        QG.SetScreenBrightness(currentScrollbarValue,
       (success) =>
       {
           Debug.Log("QG.SetScreenBrightness success = " + JsonUtility.ToJson(success));
           loginMessage.text = "设置设备亮度: \n" + currentScrollbarValue + "\n" + JsonUtility.ToJson(success);
       },
       (fail) =>
       {
           Debug.Log("QG.SetScreenBrightness fail = " + JsonUtility.ToJson(fail));
           loginMessage.text = "设置设备亮度: 失败 \n" + currentScrollbarValue + "\n" + JsonUtility.ToJson(fail);
       },
       (complete) =>
       {
           Debug.Log("设置设备亮度 complete");
       });
    }

    void setKeepScreenOnFunc()
    {
        iskeepScreenOn = !iskeepScreenOn;
        QG.SetKeepScreenOn(iskeepScreenOn,
       (success) =>
       {
           Debug.Log("QG.SetScreenBrightness success = " + JsonUtility.ToJson(success));
           loginMessage.text = "设置是否保持常亮状态: \n" + iskeepScreenOn + "\n" + JsonUtility.ToJson(success);
       },
       (fail) =>
       {
           Debug.Log("QG.SetScreenBrightness fail = " + JsonUtility.ToJson(fail));
           loginMessage.text = "设置是否保持常亮状态: 失败 \n" + iskeepScreenOn + "\n" + JsonUtility.ToJson(fail);
       },
       (complete) =>
       {
           Debug.Log("设置是否保持常亮状态 complete");
       });
    }

    void getLocationFunc()
    {
        QG.GetLocation(
       (success) =>
       {
           Debug.Log("QG.SetScreenBrightness success = " + JsonUtility.ToJson(success));
           loginMessage.text = "获取当前的地理位置、速度: \n" + JsonUtility.ToJson(success);
       },
       (fail) =>
       {
           Debug.Log("QG.SetScreenBrightness fail = " + JsonUtility.ToJson(fail));
           loginMessage.text = "获取当前的地理位置、速度: 失败 \n" + JsonUtility.ToJson(fail);
       },
       (complete) =>
       {
           Debug.Log("获取当前的地理位置、速度 complete");
       });
    }
}
