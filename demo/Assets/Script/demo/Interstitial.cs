using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QGMiniGame;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;

public class Interstitial : MonoBehaviour
{
    public Button comebackbtn;

    public Button createInterstitialAdbtn;

    public Button loadInterstitialAdbtn;

    public Button showInterstitialAdbtn;

    public Button destroyInterstitialAdbtn;

    private QGInterstitialAd qGInterstitialAd;

    public InputField inputField;

    private string inputAdUnitId;
    void Start()
    {
        comebackbtn.onClick.AddListener(comebackfunc);
        createInterstitialAdbtn.onClick.AddListener(createInterstitialAdfunc);
        loadInterstitialAdbtn.onClick.AddListener(loadInterstitialAdfunc);
        showInterstitialAdbtn.onClick.AddListener(showInterstitialAdfunc);
        destroyInterstitialAdbtn.onClick.AddListener(destroyInterstitialAdfunc);

        EventTrigger trigger = inputField.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((eventData) => { OnInputFieldClicked(); });
        trigger.triggers.Add(entry);
        inputAdUnitId = "114187";
        inputField.text = "adUnitId: " + inputAdUnitId;
    }
    private void OnInputFieldClicked()
    {
        // 在这里处理InputField被点击的逻辑
        QG.ShowKeyboard(new KeyboardParam()
        {
            defaultValue = "",
            maxLength = 100,
            multiple = true,
            confirmHold = true
        });
        QG.OnKeyboardInput((msg) =>
        {
            QGResKeyBoardponse data = JsonUtility.FromJson<QGResKeyBoardponse>(JsonUtility.ToJson(msg));
            inputField.text = "adUnitId: " + data.value;
            inputAdUnitId = data.value;
        });
    }

    public void comebackfunc()
    {
        destroyInterstitialAdfunc();
        SceneManager.LoadScene("main");
    }

    public void createInterstitialAdfunc()
    {
        bool isNumeric = Regex.IsMatch(inputAdUnitId, @"^\d+$");
        Debug.Log("inputAdUnitId：：：" + inputAdUnitId + isNumeric);
        if (!isNumeric)
        {
            QG.ShowToast(new ShowToastParam()
            {
                title = "adUnitId 必须是数字",
                iconType = "none",
                durationTime = 1500,
            });
            return;
        }

        qGInterstitialAd =
                QG
                    .CreateInterstitialAd(new QGCommonAdParam()
                    { adUnitId = inputAdUnitId });
        Debug.Log("创建插屏广告开始运行");
        QG.ShowToast(new ShowToastParam()
        {
            title = "创建插屏广告成功,adUnitId = " + inputAdUnitId,
            iconType = "none",
            durationTime = 1500,
        });
        qGInterstitialAd
            .OnLoad(() =>
            {
                QG.ShowToast(new ShowToastParam()
                {
                    title = "插屏广告加载成功",
                    iconType = "none",
                    durationTime = 1500,
                });
                Debug.Log("插屏广告加载成功");
            });
        qGInterstitialAd
            .OnError((QGBaseResponse msg) =>
            {
                QG.ShowToast(new ShowToastParam()
                {
                    title = "插屏广告加载失败",
                    iconType = "none",
                    durationTime = 1500,
                });
                Debug
                    .Log("QG.interstitialAd.OnError success = " +
                    JsonUtility.ToJson(msg));
            });
        qGInterstitialAd
    .OnClose((QGBaseResponse msg) =>
    {
        QG.ShowToast(new ShowToastParam()
        {
            title = "插屏广告关闭回调",
            iconType = "none",
            durationTime = 1500,
        });
        Debug
            .Log("QG.interstitialAd.OnClose success = " +
            JsonUtility.ToJson(msg));
    });
    }

    public void loadInterstitialAdfunc()
    {
        if (qGInterstitialAd == null)
        {
            return;
        }
        qGInterstitialAd.Load();
    }
    public void showInterstitialAdfunc()
    {
        if (qGInterstitialAd == null)
        {
            return;
        }
        qGInterstitialAd.Show();
    }

    public void destroyInterstitialAdfunc()
    {
        if (qGInterstitialAd != null)
        {
            QG.ShowToast(new ShowToastParam()
            {
                title = "销毁插屏广告",
                iconType = "success",
                durationTime = 1500,
            });
            qGInterstitialAd.Destroy();
        }
    }
}
