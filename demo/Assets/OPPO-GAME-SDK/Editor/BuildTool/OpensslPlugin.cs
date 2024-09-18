using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;
using QGMiniGame;
public class OpensslPlugin : EditorWindow
{
    private const string SavePathLabel = "保存证书的路径";
    private const string EmailAddressLabel = "Email Address";
    private const string CountryLabel = "国家";
    private const string TwoLetterPattern = @"^[A-Za-z]{2}$";
    private const string ValidCharactersPattern = @"^[a-zA-Z0-9!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]+$";
    private const string ValidEmailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
    private Dictionary<string, string> inputFields = new Dictionary<string, string>
    {
        { CountryLabel, "" },
        { "省/市/地区", "" },
        { "城市/区域", "" },
        { "组织", "" },
        { "组织单位", "" },
        { "姓名", "" },
        { EmailAddressLabel, "" },
        { SavePathLabel, "" }
    };

    private void OnGUI()
    {
        var inputStyle = new GUIStyle(EditorStyles.textField)
        {
            fontSize = 14,
            margin = { left = 20, bottom = 10, right = 20 },
            fixedHeight = 25f
        };

        foreach (var key in inputFields.Keys.ToList())
        {
            if (key == SavePathLabel)
            {
                DrawSavePathField(key);
            }
            else
            {
                DrawTextField(key);
            }
        }

        if (GUILayout.Button("保存"))
        {
            if (ValidateInputs())
            {
                SaveConfiguration();
            }
        }
    }

    private bool ValidateInputs()
    {
        foreach (var key in inputFields.Keys)
        {
            if (string.IsNullOrEmpty(inputFields[key]) ||
                (key != EmailAddressLabel && !IsAllEnglish(inputFields[key])) ||
                (key == CountryLabel && !IsTwoLetterEnglish(inputFields[key])) ||
                (key == SavePathLabel && !Directory.Exists(inputFields[key])) ||
                (key == EmailAddressLabel && !IsValidEmail(inputFields[key])))
            {
                return false;
            }
        }
        return true;
    }

    private void DrawSavePathField(string key)
    {
        GUILayout.BeginHorizontal();
        EditorGUI.BeginChangeCheck();
        inputFields[key] = EditorGUILayout.TextField(key, inputFields[key]);
        // 检查是否发生变化
        if (EditorGUI.EndChangeCheck())
        {
            // 如果有变化，可以在这里添加其他逻辑
        }
        if (GUILayout.Button("选择", GUILayout.Width(32f)))
        {
            EditorGUI.FocusTextInControl(null);
            Repaint();
            var exportPath = EditorUtility.SaveFolderPanel(key, string.Empty, "release");
            if (!string.IsNullOrEmpty(exportPath))
            {
                if (EditorGUI.EndChangeCheck())
                {
                    inputFields[key] = exportPath;
                }
            }
        }
        GUILayout.EndHorizontal();
        ValidateField(key);
    }

    private void ValidateField(string key)
    {
        if (string.IsNullOrEmpty(inputFields[key]))
        {
            Tips("不能为空");
        }
        else if (key != EmailAddressLabel && !IsAllEnglish(inputFields[key]))
        {
            Tips($"{key} 输入有误,请输入含英文,数字!");
        }
        else if (key == CountryLabel && !IsTwoLetterEnglish(inputFields[key]))
        {
            Tips($"{key} 输入有误,必须为两个字母的英文字符!");
        }
        else if (key == SavePathLabel && !Directory.Exists(inputFields[key]))
        {
            Tips($"{key} 输入有误,导出路径不存在!");
        }
        else if (key == EmailAddressLabel && !IsValidEmail(inputFields[key]))
        {
            Tips($"{key} 输入有误,不是有效邮箱!");
        }
    }

    private void DrawTextField(string key)
    {
        EditorGUI.BeginChangeCheck();
        inputFields[key] = EditorGUILayout.TextField(key, inputFields[key]);
        ValidateField(key);
    }

    private void SaveConfiguration()
    {
        string privatePath = Path.Combine(inputFields[SavePathLabel], "private.pem");
        string certificatePath = Path.Combine(inputFields[SavePathLabel], "certificate.pem");
        string result = $" req -newkey rsa:2048 -nodes -keyout {privatePath} -x509 -days 3650 -out {certificatePath} -subj /C={inputFields[CountryLabel]}/ST={inputFields["省/市/地区"]}/L={inputFields["城市/区域"]}/O={inputFields["组织"]}/OU={inputFields["组织单位"]}/CN={inputFields["姓名"]}/emailAddress={inputFields[EmailAddressLabel]}";

        BuildEditorWindow.GenerateCertificate(result, GetGenerateInfo(), inputFields[SavePathLabel]);
        Close();
    }

    private string GenerateLog()
    {
        return string.Join(", ", inputFields.Select(kvp => $"{kvp.Key}: {kvp.Value}"));
    }

    private string[] GetGenerateInfo()
    {
        return inputFields.Select(kvp => $"{kvp.Key}: {kvp.Value}").ToArray();
    }

    private bool IsTwoLetterEnglish(string str)
    {
        return Regex.IsMatch(str, TwoLetterPattern);
    }

    private bool IsAllEnglish(string str)
    {
        return Regex.IsMatch(str, ValidCharactersPattern);
    }

    private bool IsValidEmail(string str)
    {
        return Regex.IsMatch(str, ValidEmailPattern);
    }

    private void Tips(string title)
    {
        EditorGUILayout.LabelField(title, new GUIStyle(EditorStyles.miniLabel)
        {
            normal = new GUIStyleState() { textColor = Color.red },
            padding = new RectOffset((int)EditorGUIUtility.labelWidth + 5, 0, 0, 0)
        });
    }
}