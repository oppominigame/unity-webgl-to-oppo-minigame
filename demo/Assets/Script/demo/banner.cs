using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QGMiniGame;
using UnityEngine.SceneManagement;
public class banner : MonoBehaviour
{
    public Button comebackbtn;

    public Button createBannerAdbtn;

    public Button stylebtn;

    public Button showBannerAdbtn;

    public Button hideBannerAdbtn;

    public Button destroyBannerAdbtn;

    public QGBannerAd qGBannerAd;
    void Start()
    {
        comebackbtn.onClick.AddListener(comebackfunc);
        createBannerAdbtn.onClick.AddListener(createBannerAdfunc);
        stylebtn.onClick.AddListener(stylefunc);
        showBannerAdbtn.onClick.AddListener(showBannerAdfunc);
        hideBannerAdbtn.onClick.AddListener(hideBannerAdfunc);
        destroyBannerAdbtn.onClick.AddListener(destroyBannerAdfunc);
    }

    public void comebackfunc()
    {
        destroyBannerAdfunc();
        SceneManager.LoadScene("main");
    }

    public void createBannerAdfunc()
    {
        qGBannerAd =
            QG
                .CreateBannerAd(new QGCreateBannerAdParam()
                { adUnitId = "114131" });
        Debug.Log("创建Banner广告开始运行");
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
