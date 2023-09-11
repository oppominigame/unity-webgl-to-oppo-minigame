using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScene : MonoBehaviour 

{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void onBackClick()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void onPlatformClick()   
    {
        SceneManager.LoadScene("PlatformScene");

    }

    public void onPayClick()   
    {
        SceneManager.LoadScene("PayScene");
    }

    public void onAdClick()   
    {
        SceneManager.LoadScene("AdScene");
    }

    public void onBannerClick()   
    {
        SceneManager.LoadScene("BannerScene");
    }

    public void onKeyboardClick()   
    {
        SceneManager.LoadScene("KeyboardScene");
    }

    public void onDeviceClick()
    {
        SceneManager.LoadScene("DeviceScene");
    }

    public void onSystemEventClick()   
    {
        SceneManager.LoadScene("SystemEventScene");
    }

    public void onSystemInfoClick()
    {
        SceneManager.LoadScene("SystemInfoScene");
    }

    public void onFileSystemClick()
    {
        SceneManager.LoadScene("FileSystemScene");
    }

    public void onPlayerPrefsClick()
    {
        SceneManager.LoadScene("PlayerPrefsScene");
    }

    public void onStorageClick()
    {
        SceneManager.LoadScene("StorageScene");
    }

    public void onTouchClick()
    {
        SceneManager.LoadScene("TouchScene");
    }

    public void onNetworkTypeClick()
    {
        SceneManager.LoadScene("NetworkTypeScene");
    }

    public void onVibrateClick()
    {
        SceneManager.LoadScene("VibrateScene");
    }
}
