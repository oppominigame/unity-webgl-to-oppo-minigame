using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QGMiniGame;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;

public class customAd : MonoBehaviour
{
    public Button comebackbtn;

    public Button createcustomAdbtn;

    public Button showcustomAdbtn;

    public Button destroycustomAdbtn;

    public Button hidecustomAdbtn;

    private QGCustomAd qGCustomAd;

    public InputField inputField;

    private string inputAdUnitId;
    void Start()
    {
        comebackbtn.onClick.AddListener(comebackfunc);
        createcustomAdbtn.onClick.AddListener(createcustomAdfunc);
        showcustomAdbtn.onClick.AddListener(showcustomAdfunc);
        hidecustomAdbtn.onClick.AddListener(hidecustomAdfunc);
        destroycustomAdbtn.onClick.AddListener(destroycustomAdfunc);

        EventTrigger trigger = inputField.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((eventData) => { OnInputFieldClicked(); });
        trigger.triggers.Add(entry);
        inputAdUnitId = "1193999";
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
        destroycustomAdfunc();
        SceneManager.LoadScene("main");
    }

    public void createcustomAdfunc()
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

        qGCustomAd =
         QG
             .CreateCustomAd(new QGCreateCustomAdParam()
             {
                 adUnitId = inputAdUnitId //上文下图
             });
        Debug.Log("创建原生模板广告开始运行");
        QG.ShowToast(new ShowToastParam()
        {
            title = "创建原生模板广告,adUnitId = "+ inputAdUnitId,
            iconType = "none",
            durationTime = 1500,
        });

        // qGCustomAd.Load(); 不支持 报错
        qGCustomAd
            .OnLoad(() =>
            {
                QG.ShowToast(new ShowToastParam()
                {
                    title = "原生模板广告加载成功",
                    iconType = "none",
                    durationTime = 1500,
                });
            });
        qGCustomAd
            .OnError((QGBaseResponse msg) =>
            {
                QG.ShowToast(new ShowToastParam()
                {
                    title = "原生模板广告加载失败",
                    iconType = "none",
                    durationTime = 1500,
                });
                Debug
                    .Log("QG.bannerAd.OnError success = " +
                    JsonUtility.ToJson(msg));
            });
        qGCustomAd
            .OnHide(() =>
            {
                QG.ShowToast(new ShowToastParam()
                {
                    title = "隐藏原生广告",
                    iconType = "success",
                    durationTime = 1500,
                });
            });
    }


    public void showcustomAdfunc()
    {
        if (qGCustomAd == null)
        {
            createcustomAdfunc();
        }
        else
        {
            qGCustomAd
            .Show((msg) =>
            {
                QG.ShowToast(new ShowToastParam()
                {
                    title = "原生模板广告展示成功",
                    iconType = "none",
                    durationTime = 1500,
                });
                Debug.Log("原生模板广告展示成功 = " + JsonUtility.ToJson(msg));
            },
            (msg) =>
            {
                QG.ShowToast(new ShowToastParam()
                {
                    title = "原生模板广告展示失败",
                    iconType = "none",
                    durationTime = 1500,
                });
                Debug.Log("原生模板广告展示失败 = " + msg.errMsg);
            });
        }
    }

    public void destroycustomAdfunc()
    {
        if (qGCustomAd == null)
        {
            return;
        }
        QG.ShowToast(new ShowToastParam()
        {
            title = "销毁原生广告",
            iconType = "success",
            durationTime = 1500,
        });
        qGCustomAd.Destroy();
    }

    public void hidecustomAdfunc()
    {
        if (qGCustomAd != null)
        {
            qGCustomAd.Hide();
        }
    }
}
