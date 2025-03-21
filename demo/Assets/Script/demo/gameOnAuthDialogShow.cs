using QGMiniGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gameOnAuthDialogShow : MonoBehaviour
{
    public Text authDialogShowMessage;
    public Text authDialogCloseMessage;
    public Button onAuthDialogShowBtn;
    public Button offAuthDialogShowBtn;
    public Button onAuthDialogCloseBtn;
    public Button offAuthDialogCloseBtn;
    public Button comebackbtn;
    void Start()
    {
        comebackbtn.onClick.AddListener(comebackfunc);

        onAuthDialogShowBtn.onClick.AddListener(OnAuthDialogShow);
        offAuthDialogShowBtn.onClick.AddListener(OffAuthDialogShow);
        onAuthDialogCloseBtn.onClick.AddListener(OnAuthDialogClose);
        offAuthDialogCloseBtn.onClick.AddListener(OffAuthDialogClose);
    }

    public void comebackfunc()
    {
        SceneManager.LoadScene("main");
    }

    public void AuthDialogShowMessage(string str)
    {
        Debug.Log(str);
        authDialogShowMessage.text = str;
    }

    public void AuthDialogCloseMessage(string str)
    {
        Debug.Log(str);
        authDialogCloseMessage.text = str;
    }

    public void OnAuthDialogShow()
    {
        QG.OnAuthDialogShow((msg) =>
         {
             Debug.Log("QG.OnAuthDialogShow = " + JsonUtility.ToJson(msg));
             AuthDialogShowMessage("QG.OnAuthDialogShow = " + JsonUtility.ToJson(msg));
         });
    }

    public void OffAuthDialogShow()
    {
        AuthDialogShowMessage("回调日志:");
        QG.OffAuthDialogShow((msg) =>
         {
             Debug.Log("QG.OffAuthDialogShow = " + JsonUtility.ToJson(msg));
             AuthDialogShowMessage("QG.OffAuthDialogShow = " + JsonUtility.ToJson(msg));
         });
    }

    public void OnAuthDialogClose()
    {
        QG.OnAuthDialogClose((msg) =>
         {
             Debug.Log("QG.OnAuthDialogClose = " + JsonUtility.ToJson(msg));
             AuthDialogCloseMessage("QG.OnAuthDialogClose = " + JsonUtility.ToJson(msg));
         });
    }

    public void OffAuthDialogClose()
    {
        AuthDialogCloseMessage("回调日志:");
        QG.OffAuthDialogClose((msg) =>
         {
             Debug.Log("QG.OffAuthDialogClose = " + JsonUtility.ToJson(msg));
             AuthDialogCloseMessage("QG.OffAuthDialogClose = " + JsonUtility.ToJson(msg));
         });
    }
}
