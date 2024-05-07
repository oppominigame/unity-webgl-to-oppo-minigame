using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QGMiniGame;
using UnityEngine.SceneManagement;

public class Interstitial : MonoBehaviour
{
    public Button comebackbtn;

    public Button createInterstitialAdbtn;

    public Button showInterstitialAdbtn;

    public Button destroyInterstitialAdbtn;

    private QGInterstitialAd qGInterstitialAd;
    void Start()
    {
        comebackbtn.onClick.AddListener(comebackfunc);
        createInterstitialAdbtn.onClick.AddListener(createInterstitialAdfunc);
        showInterstitialAdbtn.onClick.AddListener(showInterstitialAdfunc);
        destroyInterstitialAdbtn.onClick.AddListener(destroyInterstitialAdfunc);
    }

    public void comebackfunc()
    {
        destroyInterstitialAdfunc();
        SceneManager.LoadScene("main");
    }

    public void createInterstitialAdfunc()
    {
        qGInterstitialAd =
                QG
                    .CreateInterstitialAd(new QGCommonAdParam()
                    { adUnitId = "114187" });
        Debug.Log("创建插屏广告开始运行");
        qGInterstitialAd
            .OnLoad(() =>
            {
                QG.ShowToast(new ShowToastParam()
                {
                    title = "插屏广告加载成功",
                    iconType = "none",
                    durationTime = 1500,
                });
                Debug.Log("插屏广告加载成功");
                qGInterstitialAd.Show();
            });
        qGInterstitialAd
            .OnError((QGBaseResponse msg) =>
            {
                QG.ShowToast(new ShowToastParam()
                {
                    title = "插屏广告加载失败",
                    iconType = "none",
                    durationTime = 1500,
                });
                Debug
                    .Log("QG.interstitialAd.OnError success = " +
                    JsonUtility.ToJson(msg));
            });
    }


    public void showInterstitialAdfunc()
    {
        if (qGInterstitialAd == null)
        {
            return;
        }
        qGInterstitialAd.Load();
        qGInterstitialAd.Show();
    }

    public void destroyInterstitialAdfunc()
    {
        if (qGInterstitialAd != null)
        {
            QG.ShowToast(new ShowToastParam()
            {
                title = "销毁插屏广告",
                iconType = "success",
                durationTime = 1500,
            });
            qGInterstitialAd.Destroy();
        }
    }
}
