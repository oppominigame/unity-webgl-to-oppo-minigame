using QGMiniGame;

//覆盖unity的PlayerPrefs
public static class PlayerPrefs
{
    public static void SetInt(string key, int value)
    {
        QG.StorageSetIntSync(key, value);
    }
    public static int GetInt(string key, int defaultValue = 0)
    {
        return QG.StorageGetIntSync(key, defaultValue);
    }
    public static void SetString(string key, string value)
    {
        QG.StorageSetStringSync(key, value);
    }
    public static string GetString(string key, string defaultValue = "")
    {
        return QG.StorageGetStringSync(key, defaultValue);
    }
    public static void SetFloat(string key, float value)
    {
        QG.StorageSetFloatSync(key, value);
    }
    public static float GetFloat(string key, float defaultValue = 0)
    {
        return QG.StorageGetFloatSync(key, defaultValue);
    }
    public static void DeleteAll()
    {
        QG.StorageDeleteAllSync();
    }
    public static void DeleteKey(string key)
    {
        QG.StorageDeleteKeySync(key);
    }
    public static bool HasKey(string key)
    {
        return QG.StorageHasKeySync(key);
    }
    public static void Save() { }
}
