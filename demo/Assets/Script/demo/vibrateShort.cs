using UnityEngine;
using UnityEngine.UI;
using QGMiniGame;
using UnityEngine.SceneManagement;

public class vibrateShort : MonoBehaviour
{
    public Button comebackbtn;

    public Button vibrateShortbtn;

    public Button vibrateLongbtn;

    void Start()
    {
        comebackbtn.onClick.AddListener(comebackfunc);
        vibrateShortbtn.onClick.AddListener(vibrateShortFunc);
        vibrateLongbtn.onClick.AddListener(vibrateLongFunc);
    }

    void vibrateShortFunc()
    {
        QG.VibrateShort();
    }
    void vibrateLongFunc()
    {
        QG.VibrateLong();
    }

    public void comebackfunc()
    {
        SceneManager.LoadScene("main");
    }
}
