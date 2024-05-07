using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QGMiniGame;
using UnityEngine.SceneManagement;

public class customAd : MonoBehaviour
{
    public Button comebackbtn;

    public Button createcustomAdbtn;

    public Button showcustomAdbtn;

    public Button destroycustomAdbtn;

    public Button hidecustomAdbtn;

    private QGCustomAd qGCustomAd;
    void Start()
    {
        comebackbtn.onClick.AddListener(comebackfunc);
        createcustomAdbtn.onClick.AddListener(createcustomAdfunc);
        showcustomAdbtn.onClick.AddListener(showcustomAdfunc);
        hidecustomAdbtn.onClick.AddListener(hidecustomAdfunc);
        destroycustomAdbtn.onClick.AddListener(destroycustomAdfunc);
    }

    public void comebackfunc()
    {
        destroycustomAdfunc();
        SceneManager.LoadScene("main");
    }

    public void createcustomAdfunc()
    {
        qGCustomAd =
         QG
             .CreateCustomAd(new QGCreateCustomAdParam()
             {
                 adUnitId = "1193999" //上文下图
             });
        Debug.Log("创建原生模板广告开始运行");
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
            qGCustomAd.Show();
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
