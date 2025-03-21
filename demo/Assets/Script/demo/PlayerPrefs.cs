using QGMiniGame;
using UnityEngine;
using System;
//覆盖unity的PlayerPrefs
public static class PlayerPrefs
{
    public static void SetInt(string key, int value)
    {
        string numberStr = value.ToString();
        QG.StorageSetItem(key, numberStr);
    }
    public static int GetInt(string key, int defaultValue = 0)
    {
        try
        {
            string numberStr = QG.StorageGetItem(key);
            int number = int.Parse(numberStr);
            return number;
        }
        catch (Exception error)
        {
            Debug.LogError(error);
            return defaultValue;
        }
    }
    public static void SetString(string key, string value)
    {
        QG.StorageSetItem(key, value);
    }
    public static string GetString(string key, string defaultValue = "")
    {
        try
        {
            string value = QG.StorageGetItem(key);
            return value;
        }
        catch (Exception error)
        {
            Debug.LogError(error);
            return defaultValue;
        }
    }
    public static void SetFloat(string key, float value)
    {
        string numberStr = value.ToString();
        QG.StorageSetItem(key, numberStr);
    }
    public static float GetFloat(string key, float defaultValue = 0)
    {
        try
        {
            string numberStr = QG.StorageGetItem(key);
            float number = float.Parse(numberStr);
            return number;
        }
        catch (Exception error)
        {
            Debug.LogError(error);
            return defaultValue;
        }
    }
    public static void DeleteAll()
    {
        QG.StorageClear();
    }
    public static void DeleteKey(string key)
    {
        QG.StorageRemoveItem(key);
    }
    public static bool HasKey(string key)
    {
        string value = QG.StorageGetItem(key);
        return !string.IsNullOrWhiteSpace(value);
    }
    public static void Save() { }
}
