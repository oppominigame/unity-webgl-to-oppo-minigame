using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QGMiniGame;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;

public class gameBanner : MonoBehaviour
{
    public Button comebackbtn;

    public Button createGameBannerAdbtn;

    public Button showGameBannerAdbtn;

    public Button destroyGameBannerAdbtn;

    private QGGameBannerAd qGGameBannerAd;

    public InputField inputField;

    private string inputAdUnitId;

    void Start()
    {
        comebackbtn.onClick.AddListener(comebackfunc);
        createGameBannerAdbtn.onClick.AddListener(createGameBannerAdfunc);
        showGameBannerAdbtn.onClick.AddListener(showGameBannerAdfunc);
        destroyGameBannerAdbtn.onClick.AddListener(destroyGameBannerAdfunc);

        EventTrigger trigger = inputField.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((eventData) => { OnInputFieldClicked(); });
        trigger.triggers.Add(entry);
        inputAdUnitId = "201139";
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
        destroyGameBannerAdfunc();
        SceneManager.LoadScene("main");
    }

    public void createGameBannerAdfunc()
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

        qGGameBannerAd =
            QG
                .CreateGameBannerAd(new QGCommonAdParam()
                { adUnitId = inputAdUnitId });
        Debug.Log("创建互推盒子横幅广告开始运行");
        QG.ShowToast(new ShowToastParam()
        {
            title = "创建互推盒子横幅广告,adUnitId = "+ inputAdUnitId,
            iconType = "none",
            durationTime = 1500,
        });

        qGGameBannerAd
           .OnLoad(() =>
           {
               Debug.Log("QG.gameBannerAd.OnLoad success = ");
               QG.ShowToast(new ShowToastParam()
               {
                   title = "互推盒子横幅广告加载成功",
                   iconType = "none",
                   durationTime = 1500,
               });
           });
        qGGameBannerAd
            .OnError((QGBaseResponse msg) =>
            {
                Debug
                    .Log("QG.gameBannerAd.OnError success = " +
                    JsonUtility.ToJson(msg));
            });
    }


    public void showGameBannerAdfunc()
    {
        if (qGGameBannerAd == null)
        {
            return;
        }
        qGGameBannerAd
    .Show((msg) =>
    {
        QG.ShowToast(new ShowToastParam()
        {
            title = "互推盒子横幅广告展示成功",
            iconType = "none",
            durationTime = 1500,
        });
        Debug
            .Log("互推盒子横幅广告展示成功 = " +
            JsonUtility.ToJson(msg));
    },
    (msg) =>
    {
        QG.ShowToast(new ShowToastParam()
        {
            title = "互推盒子横幅广告展示失败" + msg.errMsg,
            iconType = "none",
            durationTime = 1500,
        });
        Debug.Log("互推盒子横幅广告展示失败 = " + msg.errMsg);
    });
    }

    public void destroyGameBannerAdfunc()
    {
        if (qGGameBannerAd != null)
        {
            QG.ShowToast(new ShowToastParam()
            {
                title = "销毁互推盒子横幅广告",
                iconType = "success",
                durationTime = 1500,
            });
            qGGameBannerAd.Destroy();
        }
    }
}
