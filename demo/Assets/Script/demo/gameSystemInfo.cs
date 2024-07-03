using UnityEngine;
using UnityEngine.UI;
using QGMiniGame;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Windows;

public class gameSystemInfo : MonoBehaviour
{
    public Button comebackbtn;

    public Button getSystemInfobtn;

    public Button getSystemInfoSyncbtn;

    public Button getProviderbtn;

    public Button getManifestInfobtn;

    public Button setPreferredFramesPerSecondbtn;

    public InputField setPreferredFramesPerSecondInput;

    public Text loginMessage;

    private int PreferredFramesPerSecond = 60;
    void Start()
    {
        comebackbtn.onClick.AddListener(comebackfunc);
        getSystemInfobtn.onClick.AddListener(getSystemInfoFunc);
        getSystemInfoSyncbtn.onClick.AddListener(getSystemInfoSyncFunc);
        getProviderbtn.onClick.AddListener(getProviderFunc);
        getManifestInfobtn.onClick.AddListener(getManifestInfoFunc);
        setPreferredFramesPerSecondbtn.onClick.AddListener(setPreferredFramesPerSecondFunc);

        EventTrigger trigger = setPreferredFramesPerSecondInput.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((eventData) => { OnInputFieldClicked(); });
        trigger.triggers.Add(entry);
    }

    private void OnInputFieldClicked()
    {
        // 在这里处理InputField被点击的逻辑
        string keyboardId = QG.ShowKeyboard(new KeyboardParam()
        {
            defaultValue = ""+ PreferredFramesPerSecond,
            maxLength = 100,
            multiple = true,
            confirmHold = true
        });

        QG.OnKeyboardInput((msg) =>
        {
            QGResKeyBoardponse data = JsonUtility.FromJson<QGResKeyBoardponse>(JsonUtility.ToJson(msg));
            if (data.keyboardId == keyboardId)
            {
                try
                {
                    int result = int.Parse(data.value);
                    if (result > 0 && result <= 60)
                    {
                        PreferredFramesPerSecond = result;
                        setPreferredFramesPerSecondInput.text = data.value;
                    }
                    else
                    {
                        Debug.Log("帧率1-60有效");
                    }
                }
                catch (FormatException)
                {
                    Debug.Log("Conversion failed.");
                }
            }
        });
    }

    public void comebackfunc()
    {
        SceneManager.LoadScene("main");
    }

    void getSystemInfoFunc()
    {
        QG.GetSystemInfo((msg) =>
        {
            Debug.Log("QG.GetSystemInfo success = " + JsonUtility.ToJson(msg));
            loginMessage.text = "同步系统信息: \n" + JsonUtility.ToJson(msg);
        },
         (err) =>
         {
             Debug.Log("QG.GetSystemInfo fail = " + JsonUtility.ToJson(err));
             loginMessage.text = "同步系统信息: 失败 \n" + JsonUtility.ToJson(err);
         });
    }

    void getSystemInfoSyncFunc()
    {
        string systemStr = QG.GetSystemInfoSync();
        loginMessage.text = "异步系统信息: \n"+ systemStr;
        Debug.Log("QG.GetSystemInfoSyncFunc = " + systemStr);
    }

    void getProviderFunc()
    {
        QG.GetProvider((msg) =>
        {
            Debug.Log("QG.GetProviderFunc success = " + JsonUtility.ToJson(msg));
            loginMessage.text = "渠道信息: \n" + JsonUtility.ToJson(msg);
        });
    }

    void getManifestInfoFunc()
    {
        QG.GetManifestInfo((msg) =>
        {
            Debug.Log("QG.GetManifestInfo success = " + JsonUtility.ToJson(msg));
            loginMessage.text = "配置文件Manifest: \n" + JsonUtility.ToJson(msg);
        },
         (err) =>
         {
             Debug.Log("QG.GetManifestInfo fail = " + JsonUtility.ToJson(err));
             loginMessage.text = "配置文件Manifest: 失败 \n" + JsonUtility.ToJson(err);
         }
        //   (comp) =>
        //   {
        //       Debug.Log("QG.GetManifestInfo complete = " + JsonUtility.ToJson(comp));
        //   }
        );
    }

    void setPreferredFramesPerSecondFunc()
    {
        QG.SetPreferredFramesPerSecond(PreferredFramesPerSecond);
        QG.ShowToast(new ShowToastParam()
        {
            title = "已设置帧率" + PreferredFramesPerSecond ,
            iconType = "success",
            durationTime = 1500,
        });
    }
}
