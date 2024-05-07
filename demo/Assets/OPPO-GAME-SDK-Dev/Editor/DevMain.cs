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

        [MenuItem(GlobalDefines.MINIGAME_MENU_ITEM_ROOT + "/���� SDK " + VersionInfo.VERSION)]
        private static void ExportSDKBundle()
        {
            // ���� SDK
            var sdkPackageName = string.Format(SDK_PACKAGE_FILE_FORMAT, VersionInfo.VERSION);
            AssetDatabase.ExportPackage(SDK_ROOT_PATH, sdkPackageName, ExportPackageOptions.Recurse);
            // �����ɵİ��Ƶ���ȷ��Ŀ¼
            // @note: AssetDatabase.ExportPackage Ĭ�����ɵ���Ŀ��Ŀ¼���� Assets ����һ��
            var sdkPackageDefaultDirPath = Path.Combine(Application.dataPath, "..");
            var sdkPackageDefaultPath = Path.Combine(sdkPackageDefaultDirPath, sdkPackageName);
            if (!File.Exists(sdkPackageDefaultPath))
            {
                Debug.LogError("SDK ����ʧ��");
                return;
            }
            var sdkPackagePublishDirPath = Path.Combine(sdkPackageDefaultDirPath, $"../tools");
            var sdkPackagePublishPath = Path.Combine(sdkPackagePublishDirPath, sdkPackageName);
            if (File.Exists(sdkPackagePublishPath))
            {
                File.Delete(sdkPackagePublishPath);
            }
            FileUtil.MoveFileOrDirectory(sdkPackageDefaultPath, sdkPackagePublishPath);
            // д��汾��Ϣ
            var sdkVersionFilePath = Path.Combine(sdkPackagePublishDirPath, "version");
            var md5 = File.ReadAllBytes(sdkPackagePublishPath).MD5();
            var sb = new StringBuilder();
            sb.AppendLine(VersionInfo.VERSION)
                .AppendLine(sdkPackageName)
                .Append(md5);
            File.WriteAllText(sdkVersionFilePath, sb.ToString());
            Debug.Log($"���� SDK �ɹ�: package={sdkPackageName}, md5={md5}");
            // ������Ŀ¼
            EditorUtility.RevealInFinder(sdkPackagePublishPath);
        }
    }
}