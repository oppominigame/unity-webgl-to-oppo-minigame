using UnityEngine;
using UnityEngine.UI;
using QGMiniGame;
using UnityEngine.SceneManagement;

public class gameRecorder : MonoBehaviour
{
    public Button getRecorderManagerbtn; //创建录音
    public Button startRecorderbtn;   //开始录音
    public Button pauseRecorderbtn;  //暂停录音
    public Button resumeRecorderbtn;   //继续录音
    public Button stopRecorderbtn;   //停止录音

    public Button playAudiobtn;   //播放录音
    public Button destroyAudiobtn;//销毁音频

    public Button comebackbtn;
    QGAudioPlayer qGAudioPlayer; //音频对象
    QGRecordManager qGRecordManager; //录音对象

    private string audioUrl;
    void Start()
    {
        comebackbtn.onClick.AddListener(comebackfunc);

        getRecorderManagerbtn.onClick.AddListener(getRecorderManagerfunc);
        startRecorderbtn.onClick.AddListener(startRecorderfunc);
        pauseRecorderbtn.onClick.AddListener(pauseRecorderfunc);
        resumeRecorderbtn.onClick.AddListener(resumeRecorderfunc);
        stopRecorderbtn.onClick.AddListener(stopRecorderfunc);

        playAudiobtn.onClick.AddListener(playAudiofunc);
        destroyAudiobtn.onClick.AddListener(destroyAudiofunc);
    }

    public void comebackfunc()
    {
        if (qGAudioPlayer != null)
        {
            qGAudioPlayer.Destroy();
            qGAudioPlayer = null;
        }
        if (qGRecordManager != null)
        {
            qGRecordManager = null;
        }
        SceneManager.LoadScene("main");
    }

    public void getRecorderManagerfunc()
    {
        qGRecordManager = QG.GetRecorderManager();

        qGRecordManager
            .OnStart(() =>
            {
                QG.ShowToast(new ShowToastParam()
                {
                    title = "监听录音开始事件",
                    iconType = "success",
                    durationTime = 1500,
                });
                Debug
                    .Log("监听录音开始事件");
            });

        qGRecordManager
      .OnResume(() =>
      {
          QG.ShowToast(new ShowToastParam()
          {
              title = "监听录音继续事件",
              iconType = "none",
              durationTime = 1500,
          });
          Debug
              .Log("监听录音继续事件");
      });

        qGRecordManager
.OnPause(() =>
{
    QG.ShowToast(new ShowToastParam()
    {
        title = "监听录音暂停事件",
        iconType = "none",
        durationTime = 1500,
    });
    Debug
      .Log("监听录音暂停事件");
});

        qGRecordManager
.OnStop((QGBaseResponse res) =>
{
    QG.ShowToast(new ShowToastParam()
    {
        title = "监听录音结束事件",
        iconType = "none",
        durationTime = 1500,
    });
    Debug
     .Log("监听录音结束事件:" + res.errMsg);
    audioUrl = res.errMsg;
});

        qGRecordManager
.OnFrameRecorded(() =>
{
    QG.ShowToast(new ShowToastParam()
    {
        title = "监听已录制完指定帧大小的文件事件",
        iconType = "none",
        durationTime = 1500,
    });
    Debug
    .Log("监听已录制完指定帧大小的文件事件");
});

        qGRecordManager
.OnError(() =>
{
    QG.ShowToast(new ShowToastParam()
    {
        title = "监听录音错误事件",
        iconType = "none",
        durationTime = 1500,
    });
    Debug
    .Log("监听录音错误事件");
});
    }

    public void startRecorderfunc()
    {
        if (qGRecordManager != null)
        {
            qGRecordManager.Start(new RecordParam()
            {
                duration = 80000,
                sampleRate = 8000,
                numberOfChannels = 2,
            });

            // qGRecordManager.Start(new RecordParam());
        }
        else
        {
            QG.ShowToast(new ShowToastParam()
            {
                title = "需要创建录音",
                iconType = "error",
                durationTime = 1000,
            });
        }
    }

    public void pauseRecorderfunc()
    {
        if (qGRecordManager != null)
        {
            qGRecordManager.Pause();
        }
        else
        {
            QG.ShowToast(new ShowToastParam()
            {
                title = "需要创建录音",
                iconType = "error",
                durationTime = 1000,
            });
        }
    }

    public void resumeRecorderfunc()
    {
        if (qGRecordManager != null)
        {
            qGRecordManager.Resume();
        }
        else
        {
            QG.ShowToast(new ShowToastParam()
            {
                title = "需要创建录音",
                iconType = "error",
                durationTime = 1000,
            });
        }
    }

    public void stopRecorderfunc()
    {
        if (qGRecordManager != null)
        {
            qGRecordManager.Stop();
        }
        else
        {
            QG.ShowToast(new ShowToastParam()
            {
                title = "需要创建录音",
                iconType = "error",
                durationTime = 1000,
            });
        }
    }

    public void playAudiofunc()
    {
        if (qGRecordManager != null && audioUrl.Length > 0)
        {
            if (qGAudioPlayer == null)
            {
                qGAudioPlayer = QG.PlayAudio(new AudioParam()
                {
                    url = audioUrl,
                    startTime = 0f,
                    loop = true,
                    volume = 1f
                });
            }
            if (qGAudioPlayer != null)
            {
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

            }
        }
        else
        {
            QG.ShowToast(new ShowToastParam()
            {
                title = "需完成音频录制",
                iconType = "error",
                durationTime = 1000,
            });
        }
    }


    public void destroyAudiofunc()
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
            qGAudioPlayer = null;
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
}
