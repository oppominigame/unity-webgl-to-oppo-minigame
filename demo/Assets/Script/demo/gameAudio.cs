using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QGMiniGame;
using UnityEngine.SceneManagement;

public class gameAudio : MonoBehaviour
{
    public Button comebackbtn;

    public Button createInnerAudioContextbtn;

    public Button playInnerAudioContextbtn;

    public Button pauseInnerAudioContextbtn;

    public Button stopInnerAudioContextbtn;

    public Button seekInnerAudioContextbtn;

    public Button destroyInnerAudioContextbtn;

    QGAudioPlayer qGAudioPlayer;
    void Start()
    {
        comebackbtn.onClick.AddListener(comebackfunc);
        createInnerAudioContextbtn.onClick.AddListener(createInnerAudioContextfunc);
        playInnerAudioContextbtn.onClick.AddListener(playInnerAudioContextfunc);
        pauseInnerAudioContextbtn.onClick.AddListener(pauseInnerAudioContextfunc);
        stopInnerAudioContextbtn.onClick.AddListener(stopInnerAudioContextfunc);
        seekInnerAudioContextbtn.onClick.AddListener(seekInnerAudioContextfunc);
        destroyInnerAudioContextbtn.onClick.AddListener(destroyInnerAudioContextfunc);
    }

    public void comebackfunc()
    {
        destroyInnerAudioContextfunc();
        SceneManager.LoadScene("main");
    }

    public void createInnerAudioContextfunc()
    {
        qGAudioPlayer = QG.PlayAudio(new AudioParam()
        {
            url = "https://activity-cdo.heytapimage.com/cdo-activity/static/minigame/test/demo/music/huxia-4M.mp3", //播放链接
            startTime = 0f,
            loop = true,
            volume = 1f
        });

        qGAudioPlayer
            .OnPlay(() =>
            {
                QG.ShowToast(new ShowToastParam()
                {
                    title = "音频播放成功",
                    iconType = "success",
                    durationTime = 1500,
                });
                Debug
                    .Log("音频播放成功");
            });

        qGAudioPlayer
       .OnCanPlay(() =>
       {
           QG.ShowToast(new ShowToastParam()
           {
               title = "监听音频进入可以播放状态的事件",
               iconType = "none",
               durationTime = 1500,
           });
           Debug
               .Log("监听音频进入可以播放状态的事件");
       });

        qGAudioPlayer
      .OnPause(() =>
      {
          QG.ShowToast(new ShowToastParam()
          {
              title = "监听音频暂停事件",
              iconType = "none",
              durationTime = 1500,
          });
          Debug
              .Log("监听音频暂停事件");
      });

        qGAudioPlayer
      .OnStop(() =>
      {
          QG.ShowToast(new ShowToastParam()
          {
              title = "监听音频停止事件",
              iconType = "none",
              durationTime = 1500,
          });
          Debug
              .Log("监听音频停止事件");
      });

        qGAudioPlayer
.OnEnded(() =>
{
    QG.ShowToast(new ShowToastParam()
    {
        title = "监听音频自然播放至结束的事件",
        iconType = "none",
        durationTime = 1500,
    });
    Debug
      .Log("监听音频自然播放至结束的事件");
});

        qGAudioPlayer
.OnTimeUpdate(() =>
{
    // QG.ShowToast(new ShowToastParam()
    // {
    //     title = "7",
    //     iconType = "none",
    //     durationTime = 1500,
    // });
    Debug
     .Log("监听音频播放进度更新事件");
});

        qGAudioPlayer
.OnError(() =>
{
    QG.ShowToast(new ShowToastParam()
    {
        title = "监听音频播放错误事件",
        iconType = "none",
        durationTime = 1500,
    });
    Debug
    .Log("监听音频播放错误事件");
});

        qGAudioPlayer
.OnWaiting(() =>
{
    QG.ShowToast(new ShowToastParam()
    {
        title = "监听音频加载中事件。当音频因为数据不足，需要停下来加载时会触发",
        iconType = "none",
        durationTime = 1500,
    });
    Debug
    .Log("监听音频加载中事件。当音频因为数据不足，需要停下来加载时会触发");
});

        qGAudioPlayer
.OnSeeking(() =>
{
    QG.ShowToast(new ShowToastParam()
    {
        title = "监听音频进行跳转操作的事件",
        iconType = "none",
        durationTime = 1500,
    });
    Debug
    .Log("监听音频进行跳转操作的事件");
});

        qGAudioPlayer
.OnSeeked(() =>
{
    QG.ShowToast(new ShowToastParam()
    {
        title = "监听音频完成跳转操作的事件",
        iconType = "none",
        durationTime = 1500,
    });
    Debug
    .Log("监听音频完成跳转操作的事件");
});
    }

    public void playInnerAudioContextfunc()
    {
        if (qGAudioPlayer != null)
        {
        qGAudioPlayer.Play();
        }
    }

    public void pauseInnerAudioContextfunc()
    {
        if (qGAudioPlayer != null)
        {
        qGAudioPlayer.Pause();
        }
    }

    public void stopInnerAudioContextfunc()
    {
        if (qGAudioPlayer != null)
        {
        qGAudioPlayer.Stop();
        }
    }

    public void seekInnerAudioContextfunc()
    {
        if (qGAudioPlayer != null)
        {
        float tempTime = 3.555f;
        qGAudioPlayer.Seek(tempTime);
        }
    }
    public void destroyInnerAudioContextfunc()
    {
        if (qGAudioPlayer != null)
        {
            QG.ShowToast(new ShowToastParam()
            {
                title = "销毁音频",
                iconType = "success",
                durationTime = 1500,
            });
            qGAudioPlayer.Destroy();
        }
    }
}
