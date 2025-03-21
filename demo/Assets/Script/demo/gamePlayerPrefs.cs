using UnityEngine;
using UnityEngine.UI;
using QGMiniGame;
using UnityEngine.SceneManagement;
using System;
using System.Text;
using UnityEngine.EventSystems;
public class gamePlayerPrefs : MonoBehaviour
{
    //返回按钮
    public Button comebackbtn;
    //PlayerPrefs.Set
    public InputField SetKeyInput;
    public InputField SetValueInput;
    public Dropdown SetValueTypeDropdown;
    public Button SetBtn;
    private string SetValueType = "string";
    //PlayerPrefs.Get
    public InputField GetKeyInput;
    public Text GetValueText;
    public Dropdown GetValueTypeDropdown;
    public Button GetBtn;
    private string GetValueType = "string";
    //PlayerPrefs.HasKey
    public InputField HasKeyInput;
    public Button HasKeyBtn;
    //PlayerPrefs.DeleteKey
    public InputField DeleteKeyInput;
    public Button DeleteKeyBtn;
    //PlayerPrefs.DeleteAll
    public Button DeleteAllBtn;
    //showLog
    public Text PlayerPrefsMessage;
    StringBuilder sb = new StringBuilder();
    void Start()
    {
        comebackbtn.onClick.AddListener(comebackfunc);
        // 添加监听器
        SetValueTypeDropdown.onValueChanged.AddListener(OnSetValueTypeChanged);
        SetBtn.onClick.AddListener(SetFunc);

        GetValueTypeDropdown.onValueChanged.AddListener(OnGetKeyTypeChanged);
        GetBtn.onClick.AddListener(GetFunc);

        HasKeyBtn.onClick.AddListener(HasKeyFunc);
        DeleteKeyBtn.onClick.AddListener(DeleteKeyFunc);
        DeleteAllBtn.onClick.AddListener(DeleteAllFunc);

        // 添加点击事件的监听器

        EventTrigger trigger = SetKeyInput.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((eventData) => { OnSetKeyInputClicked(); });
        trigger.triggers.Add(entry);

        EventTrigger trigger2 = SetValueInput.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry2.eventID = EventTriggerType.PointerClick;
        entry2.callback.AddListener((eventData) => { OnSetValueInputClicked(); });
        trigger2.triggers.Add(entry2);

        EventTrigger trigger3 = GetKeyInput.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry3 = new EventTrigger.Entry();
        entry3.eventID = EventTriggerType.PointerClick;
        entry3.callback.AddListener((eventData) => { OnGetKeyInputClicked(); });
        trigger3.triggers.Add(entry3);

        EventTrigger trigger4 = HasKeyInput.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry4 = new EventTrigger.Entry();
        entry4.eventID = EventTriggerType.PointerClick;
        entry4.callback.AddListener((eventData) => { OnHasKeyInputClicked(); });
        trigger4.triggers.Add(entry4);

        EventTrigger trigger5 = DeleteKeyInput.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry5 = new EventTrigger.Entry();
        entry5.eventID = EventTriggerType.PointerClick;
        entry5.callback.AddListener((eventData) => { OnDeleteKeyInputClicked(); });
        trigger5.triggers.Add(entry5);
    }

    private void OnSetValueTypeChanged(int index)
    {
        SetValueType = SetValueTypeDropdown.options[index].text;
        ClearLog();
        ShowLog("SetValueType index: " + index);
        ShowLog("SetValueType value: " + SetValueTypeDropdown.options[index].text);
    }

    private void OnGetKeyTypeChanged(int index)
    {
        GetValueType = GetValueTypeDropdown.options[index].text;
        ClearLog();
        ShowLog("GetValueType index: " + index);
        ShowLog("GetValueType value: " + GetValueTypeDropdown.options[index].text);
    }

    private void OnSetKeyInputClicked()
    {
        // 在这里处理InputField被点击的逻辑
        string keyboardId = QG.ShowKeyboard(new KeyboardParam()
        {
            defaultValue = "Key-",
            maxLength = 100,
            multiple = true,
            confirmHold = true
        });

        QG.OnKeyboardInput((msg) =>
        {
            QGResKeyBoardponse data = JsonUtility.FromJson<QGResKeyBoardponse>(JsonUtility.ToJson(msg));
            if (data.keyboardId == keyboardId)
            {
                SetKeyInput.text = data.value;
            }
        });
    }

    private void OnSetValueInputClicked()
    {
        // 在这里处理InputField被点击的逻辑
        string keyboardId = QG.ShowKeyboard(new KeyboardParam()
        {
            defaultValue = "Value-",
            maxLength = 100,
            multiple = true,
            confirmHold = true
        });
        QG.OnKeyboardInput((msg) =>
        {
            QGResKeyBoardponse data = JsonUtility.FromJson<QGResKeyBoardponse>(JsonUtility.ToJson(msg));
            if (data.keyboardId == keyboardId)
            {
                SetValueInput.text = data.value;
            }
        });
    }

    private void OnGetKeyInputClicked()
    {
        // 在这里处理InputField被点击的逻辑
        string keyboardId = QG.ShowKeyboard(new KeyboardParam()
        {
            defaultValue = "Key-",
            maxLength = 100,
            multiple = true,
            confirmHold = true
        });
        QG.OnKeyboardInput((msg) =>
        {
            QGResKeyBoardponse data = JsonUtility.FromJson<QGResKeyBoardponse>(JsonUtility.ToJson(msg));
            if (data.keyboardId == keyboardId)
            {
                GetKeyInput.text = data.value;
            }
        });
    }

    private void OnHasKeyInputClicked()
    {
        // 在这里处理InputField被点击的逻辑
        string keyboardId = QG.ShowKeyboard(new KeyboardParam()
        {
            defaultValue = "Key-",
            maxLength = 100,
            multiple = true,
            confirmHold = true
        });
        QG.OnKeyboardInput((msg) =>
        {
            QGResKeyBoardponse data = JsonUtility.FromJson<QGResKeyBoardponse>(JsonUtility.ToJson(msg));
            if (data.keyboardId == keyboardId)
            {
                HasKeyInput.text = data.value;
            }
        });
    }

    private void OnDeleteKeyInputClicked()
    {
        // 在这里处理InputField被点击的逻辑
        string keyboardId = QG.ShowKeyboard(new KeyboardParam()
        {
            defaultValue = "Key-",
            maxLength = 100,
            multiple = true,
            confirmHold = true
        });
        QG.OnKeyboardInput((msg) =>
        {
            QGResKeyBoardponse data = JsonUtility.FromJson<QGResKeyBoardponse>(JsonUtility.ToJson(msg));
            if (data.keyboardId == keyboardId)
            {
                DeleteKeyInput.text = data.value;
            }
        });
    }

    void SetFunc()
    {
        ClearLog();
        if (SetValueType == "int")
        {
            PlayerPrefs.SetInt(SetKeyInput.text, int.Parse(SetValueInput.text));
            ShowLog("PlayerPrefs.SetInt, Key: " + SetKeyInput.text + ",Value: " + SetValueInput.text);
        }
        else if (SetValueType == "string")
        {
            PlayerPrefs.SetString(SetKeyInput.text, SetValueInput.text);
            ShowLog("PlayerPrefs.SetString, Key: " + SetKeyInput.text + ",Value: " + SetValueInput.text);
        }
        else if (SetValueType == "float")
        {
            PlayerPrefs.SetFloat(SetKeyInput.text, float.Parse(SetValueInput.text));
            ShowLog("PlayerPrefs.SetFloat, Key: " + SetKeyInput.text + ",Value: " + SetValueInput.text);
        }
        else
        {
            ShowLog("无效储存");
        }
    }

    void GetFunc()
    {
        ClearLog();
        if (GetValueType == "int")
        {
            int GetIntValue = PlayerPrefs.GetInt(GetKeyInput.text);
            GetValueText.text = GetIntValue.ToString();
            ShowLog("PlayerPrefs.GetInt, Key: " + GetKeyInput.text + ",Value: " + GetIntValue);
        }
        else if (GetValueType == "string")
        {
            string GetValueStr = PlayerPrefs.GetString(GetKeyInput.text);
            GetValueText.text = GetValueStr;
            ShowLog("PlayerPrefs.GetString, Key: " + GetKeyInput.text + ",Value: " + GetValueStr);
        }
        else if (GetValueType == "float")
        {
            float GetFloatValue = PlayerPrefs.GetFloat(GetKeyInput.text);
            GetValueText.text = GetFloatValue.ToString();
            ShowLog("PlayerPrefs.GetFloat, Key: " + GetKeyInput.text + ",Value: " + GetFloatValue);
        }
        else
        {
            ShowLog("无效获取");
        }
    }

    void HasKeyFunc()
    {
        ClearLog();
        bool isHasKey = PlayerPrefs.HasKey(HasKeyInput.text);
        ShowLog(HasKeyInput.text + " 是否已存在Key: " + isHasKey);
    }

    void DeleteKeyFunc()
    {
        ClearLog();
        PlayerPrefs.DeleteKey(DeleteKeyInput.text);
        ShowLog("删除Key: " + DeleteKeyInput.text);
    }

    void DeleteAllFunc()
    {
        ClearLog();
        PlayerPrefs.DeleteAll();
        ShowLog("调用该方法会清空存储中的所有键名");
    }

    void ShowLog(string str)
    {
        Debug.Log(str);
        sb.Append(str + "\n");
        PlayerPrefsMessage.text = sb.ToString();
    }

    void ClearLog()
    {
        sb.Clear();
        PlayerPrefsMessage.text = "";
    }

    public void comebackfunc()
    {
        // 移除监听器，避免内存泄漏
        SetValueTypeDropdown.onValueChanged.RemoveListener(OnSetValueTypeChanged);
        GetValueTypeDropdown.onValueChanged.RemoveListener(OnGetKeyTypeChanged);
        SceneManager.LoadScene("main");
    }

}
