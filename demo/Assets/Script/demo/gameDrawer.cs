using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QGMiniGame;
using UnityEngine.SceneManagement;

public class gameDrawer : MonoBehaviour
{
    public Button comebackbtn;

    public Button createGameDrawerAdbtn;

    public Button showGameDrawerAdbtn;

    public Button destroyGameDrawerAdbtn;

    public Button hideGameDrawerAdbtn;

    private QGGameDrawerAd qGGameDrawerAd;
    void Start()
    {
        comebackbtn.onClick.AddListener(comebackfunc);
        createGameDrawerAdbtn.onClick.AddListener(createGameDrawerAdfunc);
        showGameDrawerAdbtn.onClick.AddListener(showGameDrawerAdfunc);
        destroyGameDrawerAdbtn.onClick.AddListener(destroyGameDrawerAdfunc);
        hideGameDrawerAdbtn.onClick.AddListener(hideGameDrawerfunc);
    }

    public void comebackfunc()
    {
        destroyGameDrawerAdfunc();
        SceneManager.LoadScene("main");
    }

    public void createGameDrawerAdfunc()
    {
        qGGameDrawerAd =
            QG
                .CreateGameDrawerAd(new QGCreateGameDrawerAdParam()
                { adUnitId = "336614" });
        Debug.Log("创建互推盒子抽屉广告开始运行");
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
        if (qGGameDrawerAd == null)
        {
            return;
        }
        qGGameDrawerAd.Show();

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
        // qGGameDrawerAd.Hide();
        QG.ShowToast(new ShowToastParam()
        {
            title = "隐藏失败",
            iconType = "error",
            durationTime = 1500,
        });
    }
}
