using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QGMiniGame;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Newtonsoft.Json.Linq;
using System;

public class gameUnityAudio : MonoBehaviour
{
    public AudioClip audioClip; // 在 Inspector 中分配音频剪辑
    private AudioSource audioSource;
    public Button createUnityAudioBtn;  //创建音频
    public Button playUnityAudioBtn;   //播放音频
    public Button muteUnityAudioBtn;   //静音
    public Button pauseUnityAudioBtn;  //暂停音频
    public Button loopUnityAudioBtn;  //循环音频
    public Button stopUnityAudioBtn;  //停止音频
    public Button destroyUnityAudioBtn; //销毁音频
    public Slider slider;
    public Text sliderTex;
    private float sliderValue = 0f;
    public Button comebackbtn;

    void Update()
    {
        float roundedValue = RoundToTwoDecimals(slider.value);
        if (AreFloatsEqual(roundedValue, sliderValue))
        {
            // Debug.Log("两个浮点数相等。");
        }
        else
        {
            sliderValue = roundedValue;
            if (audioSource)
            {
                audioSource.volume = sliderValue;
                Debug.Log("调整音量为: " + sliderValue);
            }
        }
        sliderTex.text = "音量: " + sliderValue;
    }
    void Start()
    {
        comebackbtn.onClick.AddListener(comebackfunc);
        createUnityAudioBtn.onClick.AddListener(createUnityAudioFunc);
        playUnityAudioBtn.onClick.AddListener(playUnityAudioFunc);
        muteUnityAudioBtn.onClick.AddListener(muteUnityAudioFunc);
        pauseUnityAudioBtn.onClick.AddListener(pauseUnityAudioFunc);
        loopUnityAudioBtn.onClick.AddListener(loopUnityAudioFunc);
        stopUnityAudioBtn.onClick.AddListener(stopUnityAudioFunc);
        destroyUnityAudioBtn.onClick.AddListener(destroyUnityAudioFunc);
    }

    public void muteUnityAudioFunc()
    {
        try
        {
            audioSource.mute = !audioSource.mute;
            string _title = audioSource.mute ? "静音" : "取消静音";
            QG.ShowToast(new ShowToastParam()
            {
                title = _title,
                iconType = "success",
                durationTime = 1000,
            });
        }
        catch (Exception e)
        {
            // 捕获并打印异常信息
            QG.ShowToast(new ShowToastParam()
            {
                title = "静音失败" + e,
                iconType = "error",
                durationTime = 1000,
            });
        }
    }

    public void comebackfunc()
    {
        SceneManager.LoadScene("main");
    }
    public void createUnityAudioFunc()
    {
        try
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            QG.ShowToast(new ShowToastParam()
            {
                title = "创建U3D音频",
                iconType = "success",
                durationTime = 1000,
            });
        }
        catch (Exception e)
        {
            // 捕获并打印异常信息
            Debug.LogError($"创建U3D音频 捕获到异常: {e.Message}");
            Debug.LogError($"创建U3D音频 异常堆栈: {e.StackTrace}");
            QG.ShowToast(new ShowToastParam()
            {
                title = "创建失败" + e,
                iconType = "error",
                durationTime = 1000,
            });
        }
    }
    public void playUnityAudioFunc()
    {
        try
        {
            // 将音频剪辑赋值给 AudioSource
            audioSource.clip = audioClip;
            // 播放音频
            audioSource.Play();
            QG.ShowToast(new ShowToastParam()
            {
                title = "播放U3D音频",
                iconType = "success",
                durationTime = 1000,
            });
            Debug.Log("播放音频源: " + audioSource);
            Debug.Log("播放音量大小: " + audioSource.volume);
            Debug.Log("播放音频循环: " + audioSource.loop);
            Debug.Log("播放音频首次自动播放: " + audioSource.playOnAwake);
            Debug.Log("播放音频正在播放...: " + audioSource.isPlaying);
        }
        catch (Exception e)
        {
            // 捕获并打印异常信息
            Debug.LogError($"播放U3D音频 捕获到异常: {e.Message}");
            Debug.LogError($"播放U3D音频 异常堆栈: {e.StackTrace}");
            QG.ShowToast(new ShowToastParam()
            {
                title = "播放失败" + e,
                iconType = "error",
                durationTime = 1000,
            });
        }
    }

    public void pauseUnityAudioFunc()
    {
        try
        {
            audioSource.Pause();
            QG.ShowToast(new ShowToastParam()
            {
                title = "暂停U3D音频",
                iconType = "success",
                durationTime = 1000,
            });
        }
        catch (Exception e)
        {
            // 捕获并打印异常信息
            Debug.LogError($"暂停U3D音频 捕获到异常: {e.Message}");
            Debug.LogError($"暂停U3D音频 异常堆栈: {e.StackTrace}");
            QG.ShowToast(new ShowToastParam()
            {
                title = "暂停失败" + e,
                iconType = "error",
                durationTime = 1000,
            });
        }
    }

    public void loopUnityAudioFunc()
    {
        try
        {
            audioSource.loop = !audioSource.loop;
            QG.ShowToast(new ShowToastParam()
            {
                title = "循环U3D音频",
                iconType = "success",
                durationTime = 1000,
            });
        }
        catch (Exception e)
        {
            // 捕获并打印异常信息
            Debug.LogError($"循环U3D音频 捕获到异常: {e.Message}");
            Debug.LogError($"循环U3D音频 异常堆栈: {e.StackTrace}");
            QG.ShowToast(new ShowToastParam()
            {
                title = "循环失败",
                iconType = "error",
                durationTime = 1000,
            });
        }
    }

    public void stopUnityAudioFunc()
    {
        try
        {
            audioSource.Stop();
            QG.ShowToast(new ShowToastParam()
            {
                title = "停止U3D音频",
                iconType = "success",
                durationTime = 1000,
            });
        }
        catch (Exception e)
        {
            // 捕获并打印异常信息
            Debug.LogError($"停止U3D音频 捕获到异常: {e.Message}");
            Debug.LogError($"停止U3D音频 异常堆栈: {e.StackTrace}");
            QG.ShowToast(new ShowToastParam()
            {
                title = "停止失败",
                iconType = "error",
                durationTime = 1000,
            });
        }
    }

    public void destroyUnityAudioFunc()
    {
        try
        {
            Destroy(audioSource);
            QG.ShowToast(new ShowToastParam()
            {
                title = "销毁U3D音频",
                iconType = "success",
                durationTime = 1000,
            });
        }
        catch (Exception e)
        {
            // 捕获并打印异常信息
            Debug.LogError($"销毁U3D音频 捕获到异常: {e.Message}");
            Debug.LogError($"销毁U3D音频 异常堆栈: {e.StackTrace}");
            QG.ShowToast(new ShowToastParam()
            {
                title = "销毁失败",
                iconType = "error",
                durationTime = 1000,
            });
        }
    }

    float RoundToTwoDecimals(float value)
    {
        return Mathf.Round(value * 100) / 100; // 四舍五入到两位小数
    }

    bool AreFloatsEqual(float x, float y, float epsilon = 0.0001f)
    {
        return Mathf.Abs(x - y) < epsilon; // 判断两个浮点数的差是否小于容差值
    }
}
