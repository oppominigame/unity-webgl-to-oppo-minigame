using QGMiniGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gameOnShow : MonoBehaviour
{
    public Text showMessage;
    public Text hideMessage;
    public Button onShowBtn;
    public Button offShowBtn;
    public Button onHideBtn;
    public Button offHideBtn;
    public Button comebackbtn;
    void Start()
    {
        comebackbtn.onClick.AddListener(comebackfunc);

        onShowBtn.onClick.AddListener(OnShow);
        offShowBtn.onClick.AddListener(OffShow);
        onHideBtn.onClick.AddListener(OnHide);
        offHideBtn.onClick.AddListener(OffHide);
    }

    public void comebackfunc()
    {
        SceneManager.LoadScene("main");
    }

    public void ShowMessage(string str)
    {
        Debug.Log(str);
        showMessage.text = str;
    }

    public void HideMessage(string str)
    {
        Debug.Log(str);
        hideMessage.text = str;
    }

    public void OnShow()
    {
        QG.OnShow((msg) =>
         {
             Debug.Log("QG.OnShow = " + JsonUtility.ToJson(msg));
             ShowMessage("QG.OnShow = " + JsonUtility.ToJson(msg));
         });
    }

    public void OffShow()
    {
        ShowMessage("回调日志:");
        QG.OffShow((msg) =>
         {
             Debug.Log("QG.OffShow = " + JsonUtility.ToJson(msg));
             ShowMessage("QG.OffShow = " + JsonUtility.ToJson(msg));
         });
    }

    public void OnHide()
    {
        QG.OnHide((msg) =>
         {
             Debug.Log("QG.OnHide = " + JsonUtility.ToJson(msg));
             HideMessage("QG.OnHide = " + JsonUtility.ToJson(msg));
         });
    }

    public void OffHide()
    {
        HideMessage("回调日志:");
        QG.OffHide((msg) =>
         {
             Debug.Log("QG.OffHide = " + JsonUtility.ToJson(msg));
             HideMessage("QG.OffHide = " + JsonUtility.ToJson(msg));
         });
    }
}
