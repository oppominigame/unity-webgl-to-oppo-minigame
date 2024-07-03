using UnityEngine;
using UnityEngine.UI;
using QGMiniGame;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.EventSystems;
using UnityEngine.Windows;
public class showModal : MonoBehaviour
{
    public Button comebackbtn;

    public Button showLoadingBtn;

    public Button showToastBtn;

    public Button showModalBtn;

    public InputField showLoadingtitle;
    public InputField showLoadingTime;

    public InputField showToastInput;

    public InputField showModaltitle;
    public InputField showModalTime;

    public Dropdown showToastDrop;

    public Dropdown showModalDrop;

    private string loadingText = "进度提示文本";
    private int loadingTimes = 2000;

    private string showToastText = "显示消息提示框";
    private int showToastTimes = 2000;

    private string showToastType = "success";

    private string showModalText = "模态对话标题";
    private string showModalContext = "模态对话内容";

    private bool isShowCancel = true;
    void Start()
    {
        showLoadingBtn.onClick.AddListener(showLoadingFunc);
        showToastBtn.onClick.AddListener(showToastFunc);
        showModalBtn.onClick.AddListener(showModalFunc);
        comebackbtn.onClick.AddListener(comebackfunc);
        // 添加点击事件的监听器

        EventTrigger trigger = showLoadingtitle.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((eventData) => { OnInputFieldClicked(); });
        trigger.triggers.Add(entry);

        EventTrigger trigger2 = showLoadingTime.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry2.eventID = EventTriggerType.PointerClick;
        entry2.callback.AddListener((eventData) => { OnInputFieldClicked2(); });
        trigger2.triggers.Add(entry2);

        EventTrigger trigger3 = showToastInput.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry3 = new EventTrigger.Entry();
        entry3.eventID = EventTriggerType.PointerClick;
        entry3.callback.AddListener((eventData) => { OnInputFieldClicked3(); });
        trigger3.triggers.Add(entry3);

        EventTrigger trigger4 = showModaltitle.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry4 = new EventTrigger.Entry();
        entry4.eventID = EventTriggerType.PointerClick;
        entry4.callback.AddListener((eventData) => { OnInputFieldClicked4(); });
        trigger4.triggers.Add(entry4);

        EventTrigger trigger5 = showModalTime.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry5 = new EventTrigger.Entry();
        entry5.eventID = EventTriggerType.PointerClick;
        entry5.callback.AddListener((eventData) => { OnInputFieldClicked5(); });
        trigger5.triggers.Add(entry5);

        SetShowToastType();
        IsShowCancel();
    }

    void SetShowToastType()
    {
        Dropdown.OptionData selectedOption = showToastDrop.options[showToastDrop.value];
        showToastType = selectedOption.text;
    }

    void IsShowCancel()
    {
        isShowCancel = showModalDrop.value == 0;
    }

    private void OnInputFieldClicked()
    {
        // 在这里处理InputField被点击的逻辑
        string keyboardId = QG.ShowKeyboard(new KeyboardParam()
        {
            defaultValue = loadingText,
            maxLength = 100,
            multiple = true,
            confirmHold = true
        });

        QG.OnKeyboardInput((msg) =>
        {
            QGResKeyBoardponse data = JsonUtility.FromJson<QGResKeyBoardponse>(JsonUtility.ToJson(msg));
            if (data.keyboardId == keyboardId)
            {
                loadingText = data.value;
                showLoadingtitle.text = data.value;
            }
        });
    }

    private void OnInputFieldClicked2()
    {
        // 在这里处理InputField被点击的逻辑
        string keyboardId = QG.ShowKeyboard(new KeyboardParam()
        {
            defaultValue = "" + loadingTimes,
            maxLength = 100,
            multiple = true,
            confirmHold = true
        });

        QG.OnKeyboardInput((msg) =>
        {
            QGResKeyBoardponse data = JsonUtility.FromJson<QGResKeyBoardponse>(JsonUtility.ToJson(msg));
            if (data.keyboardId == keyboardId)
            {
                try
                {
                    int result = int.Parse(data.value);
                    showLoadingTime.text = data.value;
                    loadingTimes = result;
                }
                catch (FormatException)
                {
                    Debug.Log("Conversion failed.");
                }

            }
        });
    }

    private void OnInputFieldClicked3()
    {
        // 在这里处理InputField被点击的逻辑
        string keyboardId = QG.ShowKeyboard(new KeyboardParam()
        {
            defaultValue = showToastText,
            maxLength = 100,
            multiple = true,
            confirmHold = true
        });

        QG.OnKeyboardInput((msg) =>
        {
            QGResKeyBoardponse data = JsonUtility.FromJson<QGResKeyBoardponse>(JsonUtility.ToJson(msg));
            if (data.keyboardId == keyboardId)
            {
                showToastText = data.value;
                showToastInput.text = data.value;
            }
        });
    }

    private void OnInputFieldClicked4()
    {
        // 在这里处理InputField被点击的逻辑
        string keyboardId = QG.ShowKeyboard(new KeyboardParam()
        {
            defaultValue = showModalText,
            maxLength = 100,
            multiple = true,
            confirmHold = true
        });

        QG.OnKeyboardInput((msg) =>
        {
            QGResKeyBoardponse data = JsonUtility.FromJson<QGResKeyBoardponse>(JsonUtility.ToJson(msg));
            if (data.keyboardId == keyboardId)
            {
                showModalText = data.value;
                showModaltitle.text = data.value;
            }
        });
    }

    private void OnInputFieldClicked5()
    {
        // 在这里处理InputField被点击的逻辑
        string keyboardId = QG.ShowKeyboard(new KeyboardParam()
        {
            defaultValue = showModalContext,
            maxLength = 100,
            multiple = true,
            confirmHold = true
        });

        QG.OnKeyboardInput((msg) =>
        {
            QGResKeyBoardponse data = JsonUtility.FromJson<QGResKeyBoardponse>(JsonUtility.ToJson(msg));
            if (data.keyboardId == keyboardId)
            {
                showModalContext = data.value;
                showModalTime.text = data.value;
            }
        });
    }
    void showLoadingFunc()
    {
        QG.ShowLoading(loadingText);
        QG.SetTimeout(loadingTimes, (msg) =>
        {
            QG.HideLoading((msg) =>
            {
                Debug.Log("关闭进度成功");
            });
        });
    }

    void showToastFunc()
    {
        SetShowToastType();
        QG.ShowToast(new ShowToastParam()
        {
            title = showToastText,
            iconType = showToastType,
            durationTime = showToastTimes,
        });
    }

    void showModalFunc()
    {
        IsShowCancel();
        QG.ShowModal(new ShowModalParam()
        {
            title = showModalText,
            content = showModalContext,
            showCancel = isShowCancel
        },
        (success) =>
        {
            if (success.data.confirm)
            {
                Debug.Log("确认");
            }
            if (success.data.cancel)
            {
                Debug.Log("取消");
            }
        },
        (fail) =>
        {
            Debug.Log("失败返回");
        },
        (complete) =>
        {
            Debug.Log("跳过返回");
        });
    }

    public void comebackfunc()
    {
        SceneManager.LoadScene("main");
    }

}
