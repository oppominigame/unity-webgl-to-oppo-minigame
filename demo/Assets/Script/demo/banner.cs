using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QGMiniGame;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;

public class banner : MonoBehaviour
{
    public Button comebackbtn;

    public Button createBannerAdbtn;

    public Button stylebtn;

    public Button showBannerAdbtn;

    public Button hideBannerAdbtn;

    public Button destroyBannerAdbtn;

    public QGBannerAd qGBannerAd;

    public InputField inputField;

    private string inputAdUnitId;
    void Start()
    {
        comebackbtn.onClick.AddListener(comebackfunc);
        createBannerAdbtn.onClick.AddListener(createBannerAdfunc);
        stylebtn.onClick.AddListener(stylefunc);
        showBannerAdbtn.onClick.AddListener(showBannerAdfunc);
        hideBannerAdbtn.onClick.AddListener(hideBannerAdfunc);
        destroyBannerAdbtn.onClick.AddListener(destroyBannerAdfunc);

        EventTrigger trigger = inputField.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((eventData) => { OnInputFieldClicked(); });
        trigger.triggers.Add(entry);
        inputAdUnitId = "114131";
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
        destroyBannerAdfunc();
        SceneManager.LoadScene("main");
    }

    public void createBannerAdfunc()
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

        qGBannerAd =
            QG
                .CreateBannerAd(new QGCreateBannerAdParam()
                { adUnitId = inputAdUnitId });
        Debug.Log("创建Banner广告开始运行");
        QG.ShowToast(new ShowToastParam()
        {
            title = "创建Banner广告,adUnitId = " + inputAdUnitId,
            iconType = "none",
            durationTime = 1500,
        });
        qGBannerAd
            .OnLoad(() =>
            {
                QG.ShowToast(new ShowToastParam()
                {
                    title = "banner加载成功",
                    iconType = "success",
                    durationTime = 1500,
                });
            });
        qGBannerAd
            .OnError((QGBaseResponse msg) =>
            {
                QG.ShowToast(new ShowToastParam()
                {
                    title = "banner加载失败",
                    iconType = "error",
                    durationTime = 1500,
                });
                Debug
                    .Log("QG.bannerAd.OnError success = " +
                    JsonUtility.ToJson(msg));
            });
        qGBannerAd
      .OnHide(() =>
      {
          QG.ShowToast(new ShowToastParam()
          {
              title = "隐藏成功",
              iconType = "success",
              durationTime = 1500,
          });
      });
        qGBannerAd
         .OnClose((QGBaseResponse msg) =>
         {
             QG.ShowToast(new ShowToastParam()
             {
                 title = "banner广告关闭回调",
                 iconType = "none",
                 durationTime = 1500,
             });
             Debug
                 .Log("QG.qGBannerAd.OnClose success = " +
                 JsonUtility.ToJson(msg));
         });
    }

    public void stylefunc()
    {
        QG.ShowToast(new ShowToastParam()
        {
            title = "设置banner",
            iconType = "success",
            durationTime = 1500,
        });
    }

    public void showBannerAdfunc()
    {
        if (qGBannerAd == null)
        {
            createBannerAdfunc();
        }
        else
        {
            qGBannerAd.Show();
        }
    }

    public void hideBannerAdfunc()
    {
        if (qGBannerAd == null)
        {
            return;
        }
        qGBannerAd.Hide();
    }


    public void destroyBannerAdfunc()
    {
        if (qGBannerAd != null)
        {
            QG.ShowToast(new ShowToastParam()
            {
                title = "销毁banner",
                iconType = "success",
                durationTime = 1500,
            });
            qGBannerAd.Destroy();
        }
    }
}
