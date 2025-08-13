using UnityEngine;
using UnityEngine.UI;
using QGMiniGame;
using UnityEngine.SceneManagement;

public class gameVideo : MonoBehaviour
{
    public Button createVideobtn; //创建视频
    public Button playVideobtn;   //播放视频
    public Button pauseVideobtn;  //暂停视频
    public Button stopVideobtn;   //停止视频
    public Button seekVideobtn;   //跳转视频
    public Button destroyVideobtn;//销毁视频
    public Button comebackbtn;
    QGVideoPlayer qgVideoPlayer;  //视频对象
    void Start()
    {
        comebackbtn.onClick.AddListener(comebackfunc);
        createVideobtn.onClick.AddListener(createVideofunc);
        playVideobtn.onClick.AddListener(playVideofunc);
        pauseVideobtn.onClick.AddListener(pauseVideofunc);
        stopVideobtn.onClick.AddListener(stopVideofunc);
        seekVideobtn.onClick.AddListener(seekVideofunc);
        destroyVideobtn.onClick.AddListener(destroyVideofunc);
    }

    public void comebackfunc()
    {
        if (qgVideoPlayer != null)
        {
            qgVideoPlayer.Destroy();
            qgVideoPlayer = null;
        }
        SceneManager.LoadScene("main");
    }

    public void createVideofunc()
    {
        qgVideoPlayer = QG.CreateVideo(new VideoParam()
        {
            url = "http://10.117.224.49:8080/alpha-webm.webm",
            ParamX = 500,
            ParamY = 500,
            ParamWidth = 800,
            ParamHeight = 600,
            initialTime = 0.1f,
            playbackRate = 0.5f,
            live = false,
            objectFit = "contain",
            autoplay = false,
            loop = true,
            muted = false,
            controls = true,
            showCenterPlayBtn = true,
            enableProgressGesture = false
        });

        qgVideoPlayer
            .OnPlay(() =>
            {
                QG.ShowToast(new ShowToastParam()
                {
                    title = "视频播放成功",
                    iconType = "success",
                    durationTime = 1500,
                });
                Debug
                    .Log("视频播放成功");
            });

        qgVideoPlayer
      .OnPause(() =>
      {
          QG.ShowToast(new ShowToastParam()
          {
              title = "监听视频暂停事件",
              iconType = "none",
              durationTime = 1500,
          });
          Debug
              .Log("监听视频暂停事件");
      });

        qgVideoPlayer
.OnEnded(() =>
{
    QG.ShowToast(new ShowToastParam()
    {
        title = "监听视频自然播放至结束的事件",
        iconType = "none",
        durationTime = 1500,
    });
    Debug
      .Log("监听视频自然播放至结束的事件");
});

        qgVideoPlayer
.OnTimeUpdate(() =>
{
    //调用太频繁了 先注释掉
    // QG.ShowToast(new ShowToastParam()
    // {
    //     title = "监听视频播放进度更新事件",
    //     iconType = "none",
    //     durationTime = 1500,
    // });
    // Debug
    //  .Log("监听视频播放进度更新事件");
});

        qgVideoPlayer
.OnError(() =>
{
    QG.ShowToast(new ShowToastParam()
    {
        title = "监听视频播放错误事件",
        iconType = "none",
        durationTime = 1500,
    });
    Debug
    .Log("监听视频播放错误事件");
});

        qgVideoPlayer
.OnWaiting(() =>
{
    //调用太频繁了 先注释掉
    // QG.ShowToast(new ShowToastParam()
    // {
    //     title = "监听视频加载中事件。当视频因为数据不足，需要停下来加载时会触发",
    //     iconType = "none",
    //     durationTime = 1500,
    // });
    // Debug
    // .Log("监听视频加载中事件。当视频因为数据不足，需要停下来加载时会触发");
});

    }

    public void playVideofunc()
    {
        if (qgVideoPlayer != null)
        {
            qgVideoPlayer.Play();
            QG.ShowToast(new ShowToastParam()
            {
                title = "开始播放视频",
                iconType = "success",
                durationTime = 1000,
            });
        }
        else
        {
            QG.ShowToast(new ShowToastParam()
            {
                title = "需要创建视频",
                iconType = "error",
                durationTime = 1000,
            });
        }
    }

    public void pauseVideofunc()
    {
        if (qgVideoPlayer != null)
        {
            qgVideoPlayer.Pause();
        }
        else
        {
            QG.ShowToast(new ShowToastParam()
            {
                title = "需要创建视频",
                iconType = "error",
                durationTime = 1000,
            });
        }
    }

    public void stopVideofunc()
    {
        if (qgVideoPlayer != null)
        {
            qgVideoPlayer.Stop();
        }
        else
        {
            QG.ShowToast(new ShowToastParam()
            {
                title = "需要创建视频",
                iconType = "error",
                durationTime = 1000,
            });
        }
    }

    public void seekVideofunc()
    {
        if (qgVideoPlayer != null)
        {
            float tempTime = 5.555f;
            qgVideoPlayer.Seek(tempTime);
        }
        else
        {
            QG.ShowToast(new ShowToastParam()
            {
                title = "需要创建视频",
                iconType = "error",
                durationTime = 1000,
            });
        }
    }
    public void destroyVideofunc()
    {
        if (qgVideoPlayer != null)
        {
            QG.ShowToast(new ShowToastParam()
            {
                title = "销毁视频",
                iconType = "success",
                durationTime = 1500,
            });
            qgVideoPlayer.Destroy();
            qgVideoPlayer = null;
        }
        else
        {
            QG.ShowToast(new ShowToastParam()
            {
                title = "需要创建视频",
                iconType = "error",
                durationTime = 1000,
            });
        }
    }
}
