using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace QGMiniGame
{
    public static class DevMain
    {
        private const string SDK_PACKAGE_FILE_FORMAT = "unity_webgl_rpk_oppo_v{0:s}.unitypackage";
        private const string SDK_ROOT_PATH = "Assets/OPPO-GAME-SDK";

        [MenuItem(GlobalDefines.MINIGAME_MENU_ITEM_ROOT + "/导出 SDK " + VersionInfo.VERSION)]
        private static void ExportSDKBundle()
        {
            // 导出 SDK
            var sdkPackageName = string.Format(SDK_PACKAGE_FILE_FORMAT, VersionInfo.VERSION);
            AssetDatabase.ExportPackage(SDK_ROOT_PATH, sdkPackageName, ExportPackageOptions.Recurse);
            // 将生成的包移到正确的目录
            // @note: AssetDatabase.ExportPackage 默认生成到项目根目录，即 Assets 的上一层
            var sdkPackageDefaultDirPath = Path.Combine(Application.dataPath, "..");
            var sdkPackageDefaultPath = Path.Combine(sdkPackageDefaultDirPath, sdkPackageName);
            if (!File.Exists(sdkPackageDefaultPath))
            {
                Debug.LogError("SDK 导出失败");
                return;
            }
            var sdkPackagePublishDirPath = Path.Combine(sdkPackageDefaultDirPath, $"../tools");
            var sdkPackagePublishPath = Path.Combine(sdkPackagePublishDirPath, sdkPackageName);
            if (File.Exists(sdkPackagePublishPath))
            {
                File.Delete(sdkPackagePublishPath);
            }
            FileUtil.MoveFileOrDirectory(sdkPackageDefaultPath, sdkPackagePublishPath);
            // 写入版本信息
            var sdkVersionFilePath = Path.Combine(sdkPackagePublishDirPath, "version");
            var md5 = File.ReadAllBytes(sdkPackagePublishPath).MD5();
            var sb = new StringBuilder();
            sb.AppendLine(VersionInfo.VERSION)
                .AppendLine(sdkPackageName)
                .Append(md5);
            File.WriteAllText(sdkVersionFilePath, sb.ToString());
            Debug.Log($"导出 SDK 成功: package={sdkPackageName}, md5={md5}");
            // 打开生成目录
            EditorUtility.RevealInFinder(sdkPackagePublishPath);
        }
    }
}