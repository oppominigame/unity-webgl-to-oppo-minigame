using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class LogDisplay : MonoBehaviour
{
    public Text logText;

    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        logText.text = ""; // 清除上一次的打印信息

        // // 转义逗号和冒号
        // logString = logString.Replace("，", "，");
        // logString = logString.Replace("：;", "：");
        logText.text += logString + "\n";
    }
}
