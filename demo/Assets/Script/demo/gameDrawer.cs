using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QGMiniGame;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;

public class gameDrawer : MonoBehaviour
{
    public Button comebackbtn;

    public Button createGameDrawerAdbtn;

    public Button showGameDrawerAdbtn;

    public Button destroyGameDrawerAdbtn;

    public Button hideGameDrawerAdbtn;

    private QGGameDrawerAd qGGameDrawerAd;

    public InputField inputField;

    private string inputAdUnitId;
    void Start()
    {
        comebackbtn.onClick.AddListener(comebackfunc);
        createGameDrawerAdbtn.onClick.AddListener(createGameDrawerAdfunc);
        showGameDrawerAdbtn.onClick.AddListener(showGameDrawerAdfunc);
        destroyGameDrawerAdbtn.onClick.AddListener(destroyGameDrawerAdfunc);
        hideGameDrawerAdbtn.onClick.AddListener(hideGameDrawerfunc);

        EventTrigger trigger = inputField.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((eventData) => { OnInputFieldClicked(); });
        trigger.triggers.Add(entry);
        inputAdUnitId = "336614";
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
        destroyGameDrawerAdfunc();
        SceneManager.LoadScene("main");
    }

    public void createGameDrawerAdfunc()
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

        qGGameDrawerAd =
            QG
                .CreateGameDrawerAd(new QGCreateGameDrawerAdParam()
                { adUnitId = inputAdUnitId });
        Debug.Log("创建互推盒子抽屉广告开始运行");
        QG.ShowToast(new ShowToastParam()
        {
            title = "创建互推盒子抽屉广告,adUnitId = " + inputAdUnitId,
            iconType = "none",
            durationTime = 1500,
        });
        qGGameDrawerAd
            .OnShow(() =>
            {
                QG.ShowToast(new ShowToastParam()
                {
                    title = "互推盒子抽屉广告展示回调成功",
                    iconType = "none",
                    durationTime = 1500,
                });
                Debug.Log("互推盒子抽屉广告展示回调成功");
            });
        qGGameDrawerAd
       .OnLoad(() =>
       {
           QG.ShowToast(new ShowToastParam()
           {
               title = "互推盒子抽屉广告加载成功",
               iconType = "none",
               durationTime = 1500,
           });
       });
        qGGameDrawerAd
            .OnError((QGBaseResponse msg) =>
            {
                QG.ShowToast(new ShowToastParam()
                {
                    title = "互推盒子抽屉广告加载失败",
                    iconType = "none",
                    durationTime = 1500,
                });
                Debug
                    .Log("QG.bannerAd.OnError success = " +
                    JsonUtility.ToJson(msg));
            });
    }


    public void showGameDrawerAdfunc()
    {
        Debug.Log("qGGameDrawerAd:::" + qGGameDrawerAd);
        if (qGGameDrawerAd == null)
        {
            return;
        }
        qGGameDrawerAd
          .Show((msg) =>
          {
              QG.ShowToast(new ShowToastParam()
              {
                  title = "互推盒子抽屉广告展示成功",
                  iconType = "none",
                  durationTime = 1500,
              });
              Debug
                  .Log("互推盒子抽屉广告展示成功 = " +
                  JsonUtility.ToJson(msg));
          },
          (msg) =>
          {
              QG.ShowToast(new ShowToastParam()
              {
                  title = "互推盒子抽屉广告展示失败",
                  iconType = "none",
                  durationTime = 1500,
              });
              Debug.Log("互推盒子抽屉广告展示失败 = " + msg.errMsg);
          });
    }

    public void destroyGameDrawerAdfunc()
    {
        if (qGGameDrawerAd != null)
        {
            QG.ShowToast(new ShowToastParam()
            {
                title = "销毁互推盒子抽屉广告",
                iconType = "success",
                durationTime = 1500,
            });
            qGGameDrawerAd.Destroy();
        }
    }

    public void hideGameDrawerfunc()
    {
        if (qGGameDrawerAd == null)
        {
            return;
        }
        qGGameDrawerAd.Hide();
    }
}
