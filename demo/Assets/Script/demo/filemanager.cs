using UnityEngine;
using UnityEngine.UI;
using QGMiniGame;
using UnityEngine.SceneManagement;

public class filemanager : MonoBehaviour
{
    public Button comebackbtn;

    public Button downLoadFilebtn;

    public Button getFileInfobtn;

    public Text loginMessage;

    void Start()
    {
        comebackbtn.onClick.AddListener(comebackfunc);
        downLoadFilebtn.onClick.AddListener(downLoadFilefunc);
        getFileInfobtn.onClick.AddListener(getFileInfofunc);
    }

    public void comebackfunc()
    {
        SceneManager.LoadScene("main");
    }

    public void downLoadFilefunc()
    {
        QG.DownLoadFile(new DownLoadFileParam()
        {
            path = "/BeAttack.ogg",
            url = "https://ocs-cn-south1.heytapcs.com/ar-sdk-store-read/ar_games/sound/BeAttack.ogg",
        }, (success) =>
        {
            Debug.Log("QG.DownLoadFile success = " + JsonUtility.ToJson(success));
            loginMessage.text = "QG.DownLoadFile success = " + JsonUtility.ToJson(success);
        },
        (fail) =>
        {
            Debug.Log("QG.DownLoadFile fail = " + JsonUtility.ToJson(fail));
            loginMessage.text = "QG.DownLoadFile fail = " + JsonUtility.ToJson(fail);
        }
       );
    }

    public void getFileInfofunc()
    {
        string filename = "/BeAttack.ogg";
        QG.GetFileInfo(filename, (success) =>
      {
          Debug.Log("QG.GetFileInfo success = " + JsonUtility.ToJson(success));
          loginMessage.text = "QG.GetFileInfo success = " + JsonUtility.ToJson(success);
      },
      (fail) =>
      {
          Debug.Log("QG.GetFileInfo fail = " + JsonUtility.ToJson(fail));
          loginMessage.text = "QG.GetFileInfo fail = " + JsonUtility.ToJson(fail);
      }
     );
    }
}
