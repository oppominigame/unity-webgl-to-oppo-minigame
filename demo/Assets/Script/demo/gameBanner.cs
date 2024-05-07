using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QGMiniGame;
using UnityEngine.SceneManagement;

public class gameBanner : MonoBehaviour
{
    public Button comebackbtn;

    public Button createGameBannerAdbtn;

    public Button showGameBannerAdbtn;

    public Button destroyGameBannerAdbtn;

    private QGGameBannerAd qGGameBannerAd;
    void Start()
    {
        comebackbtn.onClick.AddListener(comebackfunc);
        createGameBannerAdbtn.onClick.AddListener(createGameBannerAdfunc);
        showGameBannerAdbtn.onClick.AddListener(showGameBannerAdfunc);
        destroyGameBannerAdbtn.onClick.AddListener(destroyGameBannerAdfunc);
    }

    public void comebackfunc()
    {
        destroyGameBannerAdfunc();
        SceneManager.LoadScene("main");
    }

    public void createGameBannerAdfunc()
    {
        qGGameBannerAd =
            QG
                .CreateGameBannerAd(new QGCommonAdParam()
                { adUnitId = "201139" });
        Debug.Log("创建互推盒子横幅广告开始运行");
        qGGameBannerAd
            .OnLoad(() =>
            {
                Debug.Log("QG.gameBannerAd.OnLoad success = ");
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
        qGGameBannerAd.Show();
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
