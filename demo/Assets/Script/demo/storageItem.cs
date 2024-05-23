﻿using UnityEngine;
using UnityEngine.UI;
using QGMiniGame;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.EventSystems;
public class storageItem : MonoBehaviour
{
    public Button comebackbtn;

    public Button storageSetItemBtn;

    public Button storageGetItemBtn;

    public Button storageRemoveItemBtn;

    public InputField storageSetItemKey;
    public InputField storageSetItemValue;

    public InputField storageGetItemKey;
    public Text storageGetItemValue;

    public InputField storageRemoveItemKey;

    void Start()
    {
        storageSetItemBtn.onClick.AddListener(StorageSetItem);
        storageGetItemBtn.onClick.AddListener(StorageGetItem);
        storageRemoveItemBtn.onClick.AddListener(StorageRemoveItem);
        comebackbtn.onClick.AddListener(comebackfunc);
        // 添加点击事件的监听器

        EventTrigger trigger = storageSetItemKey.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((eventData) => { OnInputFieldClicked(); });
        trigger.triggers.Add(entry);


        EventTrigger trigger2 = storageSetItemValue.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry2.eventID = EventTriggerType.PointerClick;
        entry2.callback.AddListener((eventData) => { OnInputFieldClicked2(); });
        trigger2.triggers.Add(entry2);


        EventTrigger trigger3 = storageGetItemKey.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry3 = new EventTrigger.Entry();
        entry3.eventID = EventTriggerType.PointerClick;
        entry3.callback.AddListener((eventData) => { OnInputFieldClicked3(); });
        trigger3.triggers.Add(entry3);

        EventTrigger trigger4 = storageRemoveItemKey.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry4 = new EventTrigger.Entry();
        entry4.eventID = EventTriggerType.PointerClick;
        entry4.callback.AddListener((eventData) => { OnInputFieldClicked4(); });
        trigger4.triggers.Add(entry4);
    }


    private void OnInputFieldClicked()
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
                storageSetItemKey.text = data.value;
            }
        });
    }

    private void OnInputFieldClicked2()
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
                storageSetItemValue.text = data.value;
            }
        });
    }
    private void OnInputFieldClicked3()
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
                storageGetItemKey.text = data.value;
            }
        });
    }

    private void OnInputFieldClicked4()
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
                storageRemoveItemKey.text = data.value;
            }
        });
    }

    void StorageSetItem()
    {
        QG.StorageSetItem(storageSetItemKey.text, storageSetItemValue.text);
        Debug.Log("数据存储,Key: " + storageSetItemKey.text + ",Value: " + storageSetItemValue.text);
    }
    void StorageGetItem()
    {
        string GetItemValue = QG.StorageGetItem(storageGetItemKey.text);
        storageGetItemValue.text = GetItemValue;
        Debug.Log("数据读取,Key: " + storageGetItemKey.text + ",Value: " + GetItemValue);
    }
    void StorageRemoveItem()
    {
        QG.StorageRemoveItem(storageRemoveItemKey.text);
        Debug.Log("删除数据,Key: " + storageRemoveItemKey.text);
    }

    public void comebackfunc()
    {
        SceneManager.LoadScene("main");
    }

}
