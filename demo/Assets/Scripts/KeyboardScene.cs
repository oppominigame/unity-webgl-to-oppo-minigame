using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using QGMiniGame;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// using System.Runtime.InteropServices;
public class KeyboardScene : MonoBehaviour
{
    public InputField defalutText;

    public GameObject defalutInput;

    private void Start()
    {
        //input输入框添加点击事件
        this.AddInputNameClickEvent();
    }

    private void AddInputNameClickEvent() //可以在Awake中调用
    {
        var eventTrigger = this.defalutInput.AddComponent<EventTrigger>();
        UnityAction<BaseEventData> selectEvent = OnInputFieldClicked;
        EventTrigger.Entry onClick =
            new EventTrigger.Entry()
            { eventID = EventTriggerType.PointerClick };

        onClick.callback.AddListener (selectEvent);
        eventTrigger.triggers.Add (onClick);
    }

    private void OnInputFieldClicked(BaseEventData data)
    {
        //TODO: 点击事件
        Debug.Log("键盘事件");
        this.playQGShowKeyboard();
    }

    // 界面/键盘
    /***
    显示键盘
    隐藏键盘
    监听输入
    监听完成
    监听收起
    ***/
    public void playQGShowKeyboard()
    {
        KeyboardParam param =
            new KeyboardParam()
            {
                defaultValue = this.defalutText.text, // 键盘输入框显示的默认值
                maxLength = 100, // 键盘中文本的最大长度
                multiple = false, // 是否为多行输入
                confirmHold = true, // 当点击完成时键盘是否收起
                confirmType = "done" // 键盘右下角confirm按钮类型，只影响按钮的文本内容
            };
        QG
            .ShowKeyboard(param,
            (msg) =>
            {
                Debug.Log("显示键盘");
            });
    }

    public void playQGHideKeyboard()
    {
        QG.HideKeyboard();
        Debug.Log("隐藏键盘");
    }

    public void playQGOnKeyboardInput()
    {
        QG
            .OnKeyboardInput((str) =>
            {
                this.defalutText.text = "" + str.value;
                Debug.Log("监听输入结果-->" + str.value);
            });
    }

    public void playQGOffKeyboardInput()
    {
        QG.OffKeyboardInput();
        Debug.Log("取消监听键盘输入事件");
    }

    public void playQGOnKeyboardConfirm()
    {
        QG
            .OnKeyboardConfirm((res) =>
            {
                Debug.Log("监听完成结果-->" + res.value);
            });
    }

    public void playQGOffKeyboardConfirm()
    {
        QG.OffKeyboardConfirm();
        Debug.Log("取消监听用户点击键盘 Confirm 按钮时的事件");
    }

    public void playQGOnKeyboardComplete()
    {
        QG
            .OnKeyboardComplete((res) =>
            {
                Debug.Log("监听收起结果-->" + res.value);
            });
    }

    public void playQGOffKeyboardComplete()
    {
        QG.OffKeyboardComplete();
        Debug.Log("取消监听监听键盘收起的事件");
    }
}
