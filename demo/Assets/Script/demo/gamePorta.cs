using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QGMiniGame;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;

public class gamePorta : MonoBehaviour
{
    public Button comebackbtn;

    public Button createGamePortaAdbtn;
    public Button loadGamePortaAdbtn;
    public Button showGamePortaAdbtn;

    public Button destroyGamePortaAdbtn;

    private QGGamePortalAd qGGamePortalAd;

    public InputField inputField;

    private string inputAdUnitId;
    void Start()
    {
        comebackbtn.onClick.AddListener(comebackfunc);
        createGamePortaAdbtn.onClick.AddListener(createGamePortaAdfunc);
        loadGamePortaAdbtn.onClick.AddListener(loadGamePortaAdfunc);
        showGamePortaAdbtn.onClick.AddListener(showGamePortaAdfunc);
        destroyGamePortaAdbtn.onClick.AddListener(destroyGamePortaAdfunc);

        EventTrigger trigger = inputField.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((eventData) => { OnInputFieldClicked(); });
        trigger.triggers.Add(entry);
        inputAdUnitId = "201138";
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
        destroyGamePortaAdfunc();
        SceneManager.LoadScene("main");
    }

    public void createGamePortaAdfunc()
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

        qGGamePortalAd =
            QG
                .CreateGamePortalAd(new QGCommonAdParam()
                { adUnitId = inputAdUnitId });
        Debug.Log("创建互推盒子九宫格广告开始运行");
        QG.ShowToast(new ShowToastParam()
        {
            title = "创建互推盒子九宫格广告,adUnitId = " + inputAdUnitId,
            iconType = "none",
            durationTime = 1500,
        });
        qGGamePortalAd
            .OnLoad(() =>
            {
                QG.ShowToast(new ShowToastParam()
                {
                    title = "互推盒子九宫格广告加载成功",
                    iconType = "none",
                    durationTime = 1500,
                });
            });
        qGGamePortalAd
            .OnError((QGBaseResponse msg) =>
            {
                QG.ShowToast(new ShowToastParam()
                {
                    title = "互推盒子九宫格广告加载失败",
                    iconType = "none",
                    durationTime = 1500,
                });
                Debug
                    .Log("QG.qGGamePortalAd.OnError success = " +
                    JsonUtility.ToJson(msg));
            });
        qGGamePortalAd
.OnClose((QGBaseResponse msg) =>
{
    QG.ShowToast(new ShowToastParam()
    {
        title = "互推盒子九宫格广告关闭回调",
        iconType = "none",
        durationTime = 1500,
    });
    Debug
        .Log("QG.qGGamePortalAd.OnClose success = " +
        JsonUtility.ToJson(msg));
});
    }


    public void loadGamePortaAdfunc()
    {
        if (qGGamePortalAd == null)
        {
            return;
        }
        qGGamePortalAd.Load();
    }

    public void showGamePortaAdfunc()
    {
        if (qGGamePortalAd == null)
        {
            return;
        }
        qGGamePortalAd.Show();
    }

    public void destroyGamePortaAdfunc()
    {
        if (qGGamePortalAd != null)
        {
            QG.ShowToast(new ShowToastParam()
            {
                title = "销毁互推盒子九宫格广告",
                iconType = "success",
                durationTime = 1500,
            });
            qGGamePortalAd.Destroy();
        }
    }
}
