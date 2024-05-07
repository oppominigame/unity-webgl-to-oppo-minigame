using QGMiniGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gameLogin : MonoBehaviour
{
    public Text loginMessage;

    public Button comebackbtn;
    void Start()
    {

        comebackbtn.onClick.AddListener(comebackfunc);

        QG
         .Login((msg) =>
         {
             Debug.Log("QG.Login success = " + JsonUtility.ToJson(msg));
             loginMessage.text = "QG.Login success = " + JsonUtility.ToJson(msg);
         },
         (msg) =>
         {
             Debug.Log("QG.Login fail = " + msg.errMsg);
             loginMessage.text = "QG.Login fail = " + msg.errMsg;
         });
    }

    public void comebackfunc()
    {
        SceneManager.LoadScene("main");
    }
}
