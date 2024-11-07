using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QGMiniGame;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Newtonsoft.Json.Linq;

public class gameAudioNew : MonoBehaviour
{
    public Button downLoadFilebtn; //下载音频
    public Button createInnerAudioContextbtn; //音频创建
    public Button playInnerAudioContextbtn;   //播放音频
    public Button playInnerAudioLianXuContextbtn; //连续点击播放音频
    public Button pauseInnerAudioContextbtn; //暂停音频
    public Button stopInnerAudioContextbtn;  //停止音频
    public Button seekInnerAudioContextbtn;  //跳转音频
    public Button destroyInnerAudioContextbtn; //销毁音频

    public Button comebackbtn;
    public Slider slider;
    private bool isSliderDragging = false;
    public Text sliderTex;
    QGAudioPlayer qGAudioPlayer; //音频对象
    private float volumValue = 0f;
    void Start()
    {
        comebackbtn.onClick.AddListener(comebackfunc);
        downLoadFilebtn.onClick.AddListener(downLoadFilefunc);
        createInnerAudioContextbtn.onClick.AddListener(createInnerAudioContextfunc);
        playInnerAudioContextbtn.onClick.AddListener(playInnerAudioContextfunc);
        playInnerAudioLianXuContextbtn.onClick.AddListener(playInnerAudioLianXuContextfunc);
        pauseInnerAudioContextbtn.onClick.AddListener(pauseInnerAudioContextfunc);
        stopInnerAudioContextbtn.onClick.AddListener(stopInnerAudioContextfunc);
        seekInnerAudioContextbtn.onClick.AddListener(seekInnerAudioContextfunc);
        destroyInnerAudioContextbtn.onClick.AddListener(destroyInnerAudioContextfunc);

        sliderEvent();
        sliderTex.text = "音量:" + volumValue;
        slider.value = volumValue;
    }

    public void comebackfunc()
    {
        destroyInnerAudioContextfunc();
        SceneManager.LoadScene("main");
    }

    private void sliderEvent()
    {
        // 订阅 Slider 的 onPointerDown、onPointerUp 事件
        EventTrigger eventTrigger = slider.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry();
        pointerDownEntry.eventID = EventTriggerType.PointerDown;
        pointerDownEntry.callback.AddListener(OnSliderPointerDown);
        eventTrigger.triggers.Add(pointerDownEntry);

        EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry();
        pointerUpEntry.eventID = EventTriggerType.PointerUp;
        pointerUpEntry.callback.AddListener(OnSliderPointerUp);
        eventTrigger.triggers.Add(pointerUpEntry);

        // 订阅 Slider 的 onValueChanged 事件
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }
    private void OnSliderPointerDown(BaseEventData data)
    {
        // 记录滑块被按下的状态
        isSliderDragging = true;
    }

    private void OnSliderPointerUp(BaseEventData data)
    {
        // 记录滑块被松开的状态
        isSliderDragging = false;
    }

    private void OnSliderValueChanged(float value)
    {
        // 检测 Slider 值的变化
        if (!isSliderDragging)
        {
            // Slider 被释放(松开)
            Debug.Log("Slider released: " + value);
            sliderTex.text = "音量:" + value.ToString();
            volumValue = value;
            if (qGAudioPlayer != null)
            {
                qGAudioPlayer.SetVolume(volumValue);
                QG.ShowToast(new ShowToastParam()
                {
                    title = "音量设置成功",
                    iconType = "success",
                    durationTime = 1000,
                });
            }
        }
    }

    public void downLoadFilefunc()
    {
        QG.DownLoadFile(new DownLoadFileParam()
        {
            path = "/test.mp3",
            url = "https://activity-cdo.heytapimage.com/cdo-activity/static/minigame/test/demo/music/huxia-4M.mp3",
        }, (success) =>
        {
            Debug.Log("QG.DownLoadFile success = " + JsonUtility.ToJson(success));
            QG.ShowToast(new ShowToastParam()
            {
                title = "音频下载成功",
                iconType = "success",
                durationTime = 1500,
            });
        },
        (fail) =>
        {
            Debug.Log("QG.DownLoadFile fail = " + JsonUtility.ToJson(fail));
            QG.ShowToast(new ShowToastParam()
            {
                title = "音频下载失败",
                iconType = "error",
                durationTime = 1500,
            });
        }
       );
    }

    public void createInnerAudioContextfunc()
    {
        bool isAccess = QG.AccessSync("/test.mp3");
        if (!isAccess)
        {
            QG.ShowToast(new ShowToastParam()
            {
                title = "需要下载音频",
                iconType = "error",
                durationTime = 1500,
            });
            return;
        }
        Debug.Log("音量:" + volumValue);
        qGAudioPlayer = QG.PlayAudio(new AudioParam()
        {
            url = "/test.mp3",
            startTime = 0f,
            loop = true,
            volume = volumValue
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
    // Debug
    //  .Log("监听音频播放进度更新事件");
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
            QG.ShowToast(new ShowToastParam()
            {
                title = "开始播放",
                iconType = "success",
                durationTime = 1000,
            });
        }
        else
        {
            QG.ShowToast(new ShowToastParam()
            {
                title = "需要创建音频",
                iconType = "error",
                durationTime = 1000,
            });
        }
    }

    public void playInnerAudioLianXuContextfunc()
    {
        if (qGAudioPlayer != null)
        {
            qGAudioPlayer.Stop();
            qGAudioPlayer.Play();
        }
        else
        {
            QG.ShowToast(new ShowToastParam()
            {
                title = "需要创建音频",
                iconType = "error",
                durationTime = 1000,
            });
        }
    }
    public void pauseInnerAudioContextfunc()
    {
        if (qGAudioPlayer != null)
        {
            qGAudioPlayer.Pause();
        }
        else
        {
            QG.ShowToast(new ShowToastParam()
            {
                title = "需要创建音频",
                iconType = "error",
                durationTime = 1000,
            });
        }
    }

    public void stopInnerAudioContextfunc()
    {
        if (qGAudioPlayer != null)
        {
            qGAudioPlayer.Stop();
        }
        else
        {
            QG.ShowToast(new ShowToastParam()
            {
                title = "需要创建音频",
                iconType = "error",
                durationTime = 1000,
            });
        }
    }

    public void seekInnerAudioContextfunc()
    {
        if (qGAudioPlayer != null)
        {
            float tempTime = 5.555f;
            qGAudioPlayer.Seek(tempTime);
        }
        else
        {
            QG.ShowToast(new ShowToastParam()
            {
                title = "需要创建音频",
                iconType = "error",
                durationTime = 1000,
            });
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
