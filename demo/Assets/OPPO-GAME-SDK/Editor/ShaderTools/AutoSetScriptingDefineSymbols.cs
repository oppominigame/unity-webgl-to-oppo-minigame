using UnityEditor;
using System.Collections.Generic;
using UnityEngine;

namespace QGMiniGame
{
    public class AutoSetScriptingDefineSymbols : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            Dictionary<string, string> scoreDict = new Dictionary<string, string>();
            //Key 新增资源 Value 新增脚本宏
            scoreDict.Add("OppoGameSDK", "OppoGameSDK");
            foreach (string importedAsset in importedAssets)
            {
                foreach (KeyValuePair<string, string> kvp in scoreDict)
                {
                    if (importedAsset.Contains(kvp.Key))
                    {
                        SetScriptingDefineSymbols(kvp.Value);
                        scoreDict.Clear();
                        break;
                    }
                }
            }
            scoreDict.Clear();
        }

        private static void SetScriptingDefineSymbols(string DefineSymbols)
        {
            // 获取当前的构建平台
            BuildTargetGroup targetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;

            // 获取当前的脚本宏
            string currentDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);

            string[] splitStrings = currentDefines.Split(';');

            if (splitStrings.Length > 0)
            {
                for (global::System.Int32 i = 0; i < splitStrings.Length; i++)
                {
                    if (splitStrings[i] == DefineSymbols)
                    {
                        //当前已有这个脚本宏
                        return;
                    }
                }
            }
            // 添加新的脚本宏
            string newDefines = currentDefines + ";" + DefineSymbols;

            // 设置新的脚本宏
            PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, newDefines);
        }
    }
}