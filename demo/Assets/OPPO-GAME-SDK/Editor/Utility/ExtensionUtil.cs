using UnityEngine;

namespace QGMiniGame
{
    public static class ExtensionUtil
    {
        public static string ToSingleQuoted(this string str)
        {
            return $"'{str}'";
        }

        public static string ToDoubleQuoted(this string str)
        {
            return $"\"{str}\"";
        }

        public static string ToPlatformQuoted(this string str)
        {
            return Application.platform == RuntimePlatform.WindowsEditor ? str.ToDoubleQuoted() : str.ToSingleQuoted();
        }

        public static bool IsValid(this string str) => !string.IsNullOrEmpty(str);
    }
}