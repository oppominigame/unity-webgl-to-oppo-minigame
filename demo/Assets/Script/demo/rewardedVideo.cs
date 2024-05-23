using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QGMiniGame;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;

public class rewardedVideo : MonoBehaviour
{
    public Button comebackbtn;
    public Button createRewardedVideoAdbtn;
    public Button loadRewardedVideoAdbtn;
    public Button showRewardedVideoAdbtn;

    public Button destroyRewardedVideoAdbtn;

    private QGRewardedVideoAd qGRewardedVideoAd;

    public InputField inputField;

    private string inputAdUnitId;
    void Start()
    {
        comebackbtn.onClick.AddListener(comebackfunc);
        createRewardedVideoAdbtn.onClick.AddListener(createRewardedVideoAdfunc);
        loadRewardedVideoAdbtn.onClick.AddListener(loadRewardedVideoAdfunc);
        showRewardedVideoAdbtn.onClick.AddListener(showRewardedVideoAdfunc);
        destroyRewardedVideoAdbtn.onClick.AddListener(destroyRewardedVideoAdfunc);

        EventTrigger trigger = inputField.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((eventData) => { OnInputFieldClicked(); });
        trigger.triggers.Add(entry);
        inputAdUnitId = "114183";
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
        destroyRewardedVideoAdfunc();
        SceneManager.LoadScene("main");
    }

    public void createRewardedVideoAdfunc()
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

        qGRewardedVideoAd =
             QG
                 .CreateRewardedVideoAd(new QGCommonAdParam()
                 { adUnitId = inputAdUnitId });
        Debug.Log("创建激励视频开始运行");
        QG.ShowToast(new ShowToastParam()
        {
            title = "创建激励视频,adUnitId = " + inputAdUnitId,
            iconType = "none",
            durationTime = 1500,
        });
        qGRewardedVideoAd
            .OnLoad(() =>
            {
                Debug.Log("激励视频广告加载成功");
                QG.ShowToast(new ShowToastParam()
                {
                    title = "视频加载成功",
                    iconType = "success",
                    durationTime = 1500,
                });
            });
        qGRewardedVideoAd
            .OnError((QGBaseResponse msg) =>
            {
                QG.ShowToast(new ShowToastParam()
                {
                    title = "视频加载失败",
                    iconType = "fail",
                    durationTime = 1500,
                });
                Debug
                    .Log("QG.rewardedVideoAd.OnError success = " +
                    JsonUtility.ToJson(msg));
            });
        qGRewardedVideoAd
            .OnClose((QGRewardedVideoResponse msg) =>
            {
                if (msg.isEnded)
                {
                    QG.ShowToast(new ShowToastParam()
                    {
                        title = "激励视频广告完成，发放奖励",
                        iconType = "none",
                        durationTime = 1500,
                    });
                    Debug.Log("激励视频广告完成，发放奖励");
                }
                else
                {
                    QG.ShowToast(new ShowToastParam()
                    {
                        title = "激励视频广告取消关闭，不发放奖励",
                        iconType = "none",
                        durationTime = 1500,
                    });
                    Debug.Log("激励视频广告取消关闭，不发放奖励");
                }
            });
    }

    public void loadRewardedVideoAdfunc()
    {
        if (qGRewardedVideoAd == null)
        {
            return;
        }
        qGRewardedVideoAd.Load();
    }

    public void showRewardedVideoAdfunc()
    {
        if (qGRewardedVideoAd == null)
        {
            return;
        }
        qGRewardedVideoAd.Show();
    }

    public void destroyRewardedVideoAdfunc()
    {
        if (qGRewardedVideoAd != null)
        {
            QG.ShowToast(new ShowToastParam()
            {
                title = "销毁激励视频",
                iconType = "success",
                durationTime = 1500,
            });
            qGRewardedVideoAd.Destroy();
        }
    }
}
