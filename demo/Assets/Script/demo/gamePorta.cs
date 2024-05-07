using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QGMiniGame;
using UnityEngine.SceneManagement;

public class gamePorta : MonoBehaviour
{
    public Button comebackbtn;

    public Button createGamePortaAdbtn;

    public Button showGamePortaAdbtn;

    public Button destroyGamePortaAdbtn;

    private QGGamePortalAd qGGamePortalAd;
    void Start()
    {
        comebackbtn.onClick.AddListener(comebackfunc);
        createGamePortaAdbtn.onClick.AddListener(createGamePortaAdfunc);
        showGamePortaAdbtn.onClick.AddListener(showGamePortaAdfunc);
        destroyGamePortaAdbtn.onClick.AddListener(destroyGamePortaAdfunc);
    }

    public void comebackfunc()
    {
        destroyGamePortaAdfunc();
        SceneManager.LoadScene("main");
    }

    public void createGamePortaAdfunc()
    {
        qGGamePortalAd =
            QG
                .CreateGamePortalAd(new QGCommonAdParam()
                { adUnitId = "201138" });
        Debug.Log("创建互推盒子九宫格广告开始运行");
        qGGamePortalAd
            .OnLoad(() =>
            {
                qGGamePortalAd
                    .Show((msg) =>
                    {
                        Debug
                            .Log("互推盒子九宫格广告展示成功 = " +
                            JsonUtility.ToJson(msg));
                    },
                    (msg) =>
                    {
                        Debug.Log("互推盒子九宫格广告展示失败 = " + msg.errMsg);
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
                    .Log("QG.bannerAd.OnError success = " +
                    JsonUtility.ToJson(msg));
            });
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
