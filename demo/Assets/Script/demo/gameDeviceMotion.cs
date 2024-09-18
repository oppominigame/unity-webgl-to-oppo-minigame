using UnityEngine;
using UnityEngine.UI;
using QGMiniGame;
using UnityEngine.SceneManagement;
using System;
public class gameDeviceMotion : MonoBehaviour
{
    public Button comebackbtn;

    public Button startDeviceMotionListeningBtn;

    public Button stopDeviceMotionListeningBtn;

    public Button onDeviceMotionChangeBtn;

    public Button offDeviceMotionChangeBtn;
    public Button getDeviceMotionChangeBtn;

    public Text loginMessage;

    private bool isGetDeviceData = false;
    void Start()
    {
        comebackbtn.onClick.AddListener(comebackfunc);
        startDeviceMotionListeningBtn.onClick.AddListener(startDeviceMotionListeningFunc);
        stopDeviceMotionListeningBtn.onClick.AddListener(stopDeviceMotionListeningFunc);
        onDeviceMotionChangeBtn.onClick.AddListener(onDeviceMotionChangeFunc);
        offDeviceMotionChangeBtn.onClick.AddListener(offDeviceMotionChangeFunc);
        getDeviceMotionChangeBtn.onClick.AddListener(getDeviceMotionChangeFunc);
    }

    void Update()
    {
        if (isGetDeviceData)
        {
            DeviceMotionChangeParam deviceMotionChangeParam = QGArManager.GetDeviceMotionChange();
            // loginMessage.text = deviceMotionChangeParam == null ? "当前设备方向信息: \n数据为空" : loginMessage.text = "当前设备方向信息: \nalpha:" + deviceMotionChangeParam.alpha + "\nbeta:" + deviceMotionChangeParam.beta + "\ngamma:" + deviceMotionChangeParam.gamma;
            double radians = Math.PI / 2; // 90 degrees
            loginMessage.text = deviceMotionChangeParam == null ? "当前设备方向信息: \n数据为空" : loginMessage.text = "当前设备方向信息: \n弧度:\nalpha:" + deviceMotionChangeParam.alpha + "\nbeta:" + deviceMotionChangeParam.beta + "\ngamma:" + deviceMotionChangeParam.gamma + "\n角度:\nalpha:" + RadiansToDegrees(deviceMotionChangeParam.alpha) + "\nbeta:" + RadiansToDegrees(deviceMotionChangeParam.beta) + "\ngamma:" + RadiansToDegrees(deviceMotionChangeParam.gamma);
        }
    }

    public void comebackfunc()
    {
        SceneManager.LoadScene("main");
    }

    public void startDeviceMotionListeningFunc()
    {
        QGArManager.StartDeviceMotionListening(
            "game",
            (success) =>
            {
                Debug.Log("StartDeviceMotionListening = " + success);
            },
            (fail) =>
            {
                Debug.Log("StartDeviceMotionListening = " + fail);
            }
        );
    }
    public void stopDeviceMotionListeningFunc()
    {
        QGArManager.StopDeviceMotionListening(
          (success) =>
          {
              Debug.Log("StopDeviceMotionListening = " + success);
          },
          (fail) =>
          {
              Debug.Log("StopDeviceMotionListening = " + fail);
          }
      );
    }

    public void onDeviceMotionChangeFunc()
    {
        QGArManager.OnDeviceMotionChange();
    }

    public void offDeviceMotionChangeFunc()
    {
        QGArManager.OffDeviceMotionChange();
    }

    public void getDeviceMotionChangeFunc()
    {
        isGetDeviceData = true;
    }

    public double RadiansToDegrees(float radians)
    {
        return radians * (180 / Math.PI);
    }
}
