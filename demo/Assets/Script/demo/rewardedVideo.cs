using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QGMiniGame;
using UnityEngine.SceneManagement;

public class rewardedVideo : MonoBehaviour
{
    public Button comebackbtn;

    public Button createRewardedVideoAdbtn;

    public Button showRewardedVideoAdbtn;

    public Button destroyRewardedVideoAdbtn;

    private QGRewardedVideoAd qGRewardedVideoAd;
    void Start()
    {
        comebackbtn.onClick.AddListener(comebackfunc);
        createRewardedVideoAdbtn.onClick.AddListener(createRewardedVideoAdfunc);
        showRewardedVideoAdbtn.onClick.AddListener(showRewardedVideoAdfunc);
        destroyRewardedVideoAdbtn.onClick.AddListener(destroyRewardedVideoAdfunc);
    }

    public void comebackfunc()
    {
        destroyRewardedVideoAdfunc();
        SceneManager.LoadScene("main");
    }

    public void createRewardedVideoAdfunc()
    {
        qGRewardedVideoAd =
             QG
                 .CreateRewardedVideoAd(new QGCommonAdParam()
                 { adUnitId = "114183" });
        Debug.Log("创建激励视频开始运行");
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
                qGRewardedVideoAd.Show();
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


    public void showRewardedVideoAdfunc()
    {
        if (qGRewardedVideoAd == null)
        {
            return;
        }
        qGRewardedVideoAd.Load();
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
