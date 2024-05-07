using UnityEditor;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace QGMiniGame
{
    public class ShaderStaticDetector
    {
        public class ResultBehaviour
        {
            public ResultBehaviour(string message, string ok, Func<bool> okAction = null, string cancel = "", Func<bool> cancelAction = null)
            {
                this.message = message;
                this.ok = ok;
                this.okAction = okAction;
                this.cancel = cancel;
                this.cancelAction = cancelAction;
            }

            public Func<bool> okAction;
            public Func<bool> cancelAction;
            public string message;
            public string ok;
            public string cancel;

            public void Execute()
            {
                if (EditorUtility.DisplayDialog("提示", message, ok, cancel))
                {
                    ExecuteAction(okAction);
                }
                else
                {
                    ExecuteAction(cancelAction);
                }
            }

            private void ExecuteAction(Func<bool> action)
            {
                var showLogFilePath = action?.Invoke();
                if (showLogFilePath.HasValue && showLogFilePath.Value)
                {
                    ShowLogFilePath();
                }
            }
        }

        /// <summary>
        /// JSON 文件的路径
        /// </summary>
        private const string FINDER_JSON_PATH = "Assets/OPPO-GAME-SDK/Runtime/ShaderTools/ShaderFinder.json";
        private const string SHADER_CHECK_TOOL_MENU_ITEM = GlobalDefines.MINIGAME_MENU_ITEM_ROOT + "/Shader检测工具/";
        private const string TEMP_FOLDER_PATH = "Assets/Resources/TempFolder";

        private static string logFilePath;

        private readonly List<string> prefabList = new List<string>();
        private readonly List<ShaderParam> shaderList = new List<ShaderParam>();
        private readonly StringBuilder sb = new StringBuilder();
        private readonly StringBuilder writeSB = new StringBuilder();
        private readonly StringBuilder sceneSB = new StringBuilder();
        private readonly StringBuilder sceneWriteSB = new StringBuilder();

        private ResultBehaviour successBehaviour = new ResultBehaviour("完成检测，无异常", "确定");
        private ResultBehaviour failBehaviour = new ResultBehaviour("完成检测，存在异常", "确定", () => true);
        private int senceCount = 0;
        private int foundCount = 0;

        public static bool DoDetection(ResultBehaviour successBehaviour = null, ResultBehaviour failBehaviour = null)
        {
            var detector = new ShaderStaticDetector();
            if (successBehaviour != null)
            {
                detector.successBehaviour = successBehaviour;
            }
            if (failBehaviour != null)
            {
                detector.failBehaviour = failBehaviour;
            }
            return detector.DetectionProcess();
        }

        private static void ShowLogFilePath()
        {
            if (!logFilePath.IsValid()) return;
            QGGameTools.ShowInExplorer(logFilePath);
        }

        private bool DetectionProcess()
        {
            string jsonText;
            string[] list;
            // 读取 JSON 文件内容
            using (StreamReader reader = new StreamReader(FINDER_JSON_PATH))
            {
                jsonText = reader.ReadToEnd();
            }

            // 解析 JSON 数据为对象
            ShaderFinderJson dataObject = JsonUtility.FromJson<ShaderFinderJson>(jsonText);

            // 访问对象的属性
            list = dataObject.ShaderApi;

            string[] MaterialGuids = AssetDatabase.FindAssets("t:Material");
            string[] shaderGuids = AssetDatabase.FindAssets("t:Shader");
            foundCount = 0;
            int foundMaterialCount = 0;
            sb.Clear();
            writeSB.Clear();
            shaderList.Clear();
            prefabList.Clear();
            var localSB = new StringBuilder();
            var materialSB = new StringBuilder();
            var materials = new List<Material>();
            localSB.Append("不支持的shader脚本路径: " + "\n");

            foreach (string MaterialGuid in MaterialGuids)
            {
                string materialPath = AssetDatabase.GUIDToAssetPath(MaterialGuid);
                Material material = AssetDatabase.LoadAssetAtPath<Material>(materialPath);

                materials.Add(material);
            }

            foreach (string shaderGuid in shaderGuids)
            {
                string shaderPath = AssetDatabase.GUIDToAssetPath(shaderGuid);
                Shader shader = AssetDatabase.LoadAssetAtPath<Shader>(shaderPath);

                if (shader != null)
                {
                    int tempCount = 0;
                    string[] lines = System.IO.File.ReadAllLines(shaderPath);
                    StringBuilder tempSb = new StringBuilder();
                    bool insideComment = false;

                    for (int i = 0; i < lines.Length; i++)
                    {
                        string line = lines[i];
                        // 检查行是否以注释标记开头
                        line = line.TrimStart();
                        int commentIndex = line.IndexOf("//");
                        if (commentIndex >= 0)
                        {
                            line = line.Substring(0, commentIndex);
                        }
                        string processedLine = line;
                        if (insideComment)
                        {
                            int commentEndIndex = line.IndexOf("*/");
                            if (commentEndIndex >= 0)
                            {
                                insideComment = false;
                                processedLine = line.Substring(commentEndIndex + 2);
                            }
                            else
                            {
                                processedLine = "";
                            }
                        }
                        else
                        {
                            int commentStartIndex = line.IndexOf("/*");
                            int commentEndIndex = line.IndexOf("*/");

                            if (commentStartIndex >= 0 && commentEndIndex >= 0 && commentStartIndex < commentEndIndex)
                            {
                                processedLine = line.Substring(0, commentStartIndex) + line.Substring(commentEndIndex + 2);
                            }
                            else if (commentStartIndex >= 0)
                            {
                                insideComment = true;
                                processedLine = line.Substring(0, commentStartIndex);
                            }
                            else if (commentEndIndex >= 0)
                            {
                                processedLine = line.Substring(commentEndIndex + 2);
                            }
                        }

                        string shaderText;
                        // 打印处理后的行
                        for (int j = 0; j < list.Length; j++)
                        {
                            if (processedLine.Contains(list[j]) && !processedLine.Contains("target"))
                            {
                                shaderText = "包含不支持的API: " + list[j] + ", 行数：" + (i + 1) + ", " + processedLine;
                                tempSb.Append(shaderText + "\n");
                                tempCount++;
                            }
                        }
                        if (processedLine.Contains("target"))
                        {
                            string pattern = @"\b\d+(\.\d+)?\b";
                            Match match = Regex.Match(processedLine, pattern);
                            if (match.Success)
                            {
                                string version = match.Value;
                                float floatValue = float.Parse(version);
                                if (floatValue > 3.0)
                                {
                                    shaderText = "包含不支持的API: target " + version + ", 行数：" + (i + 1) + ", " + processedLine;
                                    tempSb.Append(shaderText + "\n");
                                    tempCount++;
                                }
                            }
                        }
                    }
                    if (tempCount > 0)
                    {
                        foreach (Material material in materials)
                        {
                            if (shader == material.shader)
                            {
                                materialSB.Append(material.name + ".mat, ");

                                foundMaterialCount++;
                            }
                        }
                        if (foundMaterialCount > 0)
                        {
                            string tex = OpenScriptAll(shaderPath, shaderPath);
                            localSB.Append(tex + "\n");
                            shaderList.Add(new ShaderParam()
                            {
                                shader = shader, //shader对象
                                shaderPath = shaderPath, //shader脚本路径
                                shaderNote = tempSb.ToString(), //详细文本
                            });

                            // sb.Append(shaderPath + "\n");
                            localSB.Append(tempSb.ToString());
                            localSB.Append("Shader正在被材质使用: " + materialSB.ToString() + ",数量：" + foundMaterialCount + "\n\n");

                            foundMaterialCount = 0;
                            materialSB.Clear();
                        }
                    }
                }
            }

            //筛选场景和资源中正在使用的shader
            sb.AppendLine(string.Format("<color=green><b>{0}</b></color>", ">>>>>>>>>>>>>>>>>>>>>场景检测<<<<<<<<<<<<<<<<<<<<"));
            writeSB.AppendLine(">>>>>>>>>>>>>>>>>>>>>场景检测<<<<<<<<<<<<<<<<<<<<");
            GetScenesInBuild();

            sb.AppendLine(string.Format("<color=green><b>{0}</b></color>", ">>>>>>>>>>>>>>>>>>>>>预制检测<<<<<<<<<<<<<<<<<<<<"));
            writeSB.AppendLine(">>>>>>>>>>>>>>>>>>>>>预制检测<<<<<<<<<<<<<<<<<<<<");
            GetAllPrefab();
            var shaderText2 = "webgl1.0不支持的shader节点数量: " + foundCount;
            sb.Append(shaderText2);
            writeSB.Append(shaderText2);

            WriteLog(writeSB.ToString());

            Debug.Log(sb.ToString());
            sb.Length = 0;
            writeSB.Length = 0;

            prefabList.Clear();
            shaderList.Clear();

            var isSuccess = foundCount == 0;
            if (isSuccess)
            {
                successBehaviour.Execute();
            }
            else
            {
                failBehaviour.Execute();
            }
            return isSuccess;
        }

        private void WriteLog(string text)
        {
            var rootPath = Application.dataPath + "/..";
            string tempPath = "ShaderLog";
            string tempPath2 = "ShaderLog/HistoryLog";
            string folderPath = Path.Combine(rootPath, tempPath);
            string folderPath2 = Path.Combine(rootPath, tempPath2);

            CreatFolder(folderPath);
            CreatFolder(folderPath2);

            string fileName = GetFileName();

            logFilePath = Path.Combine(folderPath, fileName);
            string filePath2 = Path.Combine(folderPath2, fileName);

            string[] txtFilePaths = Directory.GetFiles(folderPath, "*.txt");
            string[] metaFilePaths = Directory.GetFiles(folderPath, "*.meta");
            string[] metaFilePaths2 = Directory.GetFiles(folderPath2, "*.meta");

            DeleteFile(txtFilePaths);
            DeleteFile(metaFilePaths);
            DeleteFile(metaFilePaths2);

            WriteFile(text, logFilePath);
            WriteFile(text, filePath2);
        }

        [MenuItem(SHADER_CHECK_TOOL_MENU_ITEM + "一键检查")]
        private static void OneClickCheck()
        {
            DoDetection();
        }

        [MenuItem(SHADER_CHECK_TOOL_MENU_ITEM + "下载示例")]
        private static void DownloadExamplePack()
        {
            // @todo 张祖瑞: 现在下载的是VIVO的东西，改成正确的地址
            QGGameTools.ShaderTestTool();
        }

        //创建文件夹
        private void CreatFolder(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }

        //获取以时间命名的txt文件名
        private string GetFileName()
        {
            DateTime localTime = DateTime.Now;
            string tempHour = localTime.Hour < 10 ? ("0" + localTime.Hour) : localTime.Hour.ToString();
            string tempMinute = localTime.Minute < 10 ? ("0" + localTime.Minute) : localTime.Minute.ToString();
            string tempSecond = localTime.Second < 10 ? ("0" + localTime.Second) : localTime.Second.ToString();
            string fileName = string.Format("{0}-{1}-{2} {3}-{4}-{5} {6}", localTime.Year, localTime.Month, localTime.Day, tempHour, tempMinute, tempSecond, "ShaderLog.txt");
            return fileName;
        }

        private void DeleteFile(string[] filePaths)
        {
            foreach (string filePath in filePaths)
            {
                File.Delete(filePath);
            }
        }

        private void WriteFile(string text, string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(text);
                }
            }
        }

        private void OpenScript(string scriptPath, string str)
        {
            // 在控制台输出蓝色文本，并添加可跳转的链接
            Debug.LogFormat("<color=blue><b><a href=\"{0}\">{1}</a></b></color>", scriptPath, str);
        }

        private string OpenScriptAll(string scriptPath, string str)
        {
            string unityVersion = Application.unityVersion;
            string tempstr = string.Format("<color=blue><b><a href=\"{0}\">{1}</a></b></color>", scriptPath, str);
            if (unityVersion.StartsWith("2018"))
            {
                //2018不支持直接渲染超链接
                tempstr = string.Format("<color=blue><b>{0}</b></color>", scriptPath);
            }
            return tempstr;
        }

        //筛选场景和资源中正在使用的shader
        private void GetScenesInBuild()
        {
            EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;

            for (int i = 0; i < scenes.Length; i++)
            {
                senceCount = 0;
                sceneSB.Clear();
                sceneWriteSB.Clear();
                EditorBuildSettingsScene scene = scenes[i];
                string scenePath = scene.path;
                if (!string.IsNullOrEmpty(scenePath))
                {
                    UnityEngine.SceneManagement.Scene loadedScene = UnityEditor.SceneManagement.EditorSceneManager.OpenScene(scenePath, UnityEditor.SceneManagement.OpenSceneMode.Single);
                    if (loadedScene.isLoaded)
                    {
                        GameObject[] rootObjects = loadedScene.GetRootGameObjects();
                        foreach (GameObject rootObject in rootObjects)
                        {
                            ProcessGameObject(rootObject);
                        }
                        if (senceCount > 0)
                        {
                            string str = "不适配的场景路径: " + OpenScriptAll(scenePath, scenePath) + ",不适配的节点数量: " + senceCount;
                            string str2 = "不适配的场景路径: " + scenePath + ",不适配的节点数量: " + senceCount;
                            sb.AppendLine(str);
                            sb.AppendLine(sceneSB.ToString());

                            writeSB.AppendLine(str2);
                            writeSB.AppendLine(sceneWriteSB.ToString());
                            foundCount += senceCount;
                        }
                    }
                }
            }
        }

        private void ProcessGameObject(GameObject gameObject)
        {
            if (PrefabUtility.GetPrefabInstanceStatus(gameObject) == PrefabInstanceStatus.Connected)
            {
                string prefabAssetPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(gameObject);
                prefabList.Add(prefabAssetPath);
            }

            Renderer[] renderers = gameObject.GetComponents<Renderer>();

            if (renderers.Length > 0)
            {
                foreach (Renderer renderer in renderers)
                {
                    Material[] materials = renderer.sharedMaterials;
                    if (materials.Length > 0)
                    {
                        foreach (Material material in materials)
                        {
                            if (material && material.shader)
                            {
                                for (int i = 0; i < shaderList.Count; i++)
                                {
                                    if (shaderList[i].shader == material.shader)
                                    {
                                        string str1 = "场景节点名字: " + gameObject.name;
                                        string str2 = "Renderer材质: " + material.name + ",材质上的shader: " + material.shader;
                                        string str3 = "包含不支持的Shader脚本:" + OpenScriptAll(shaderList[i].shaderPath, shaderList[i].shaderPath);
                                        string str33 = "包含不支持的Shader脚本:" + shaderList[i].shaderPath;
                                        string str4 = shaderList[i].shaderNote;

                                        sceneSB.AppendLine(str1);
                                        sceneSB.AppendLine(str2);
                                        sceneSB.AppendLine(str3);
                                        sceneSB.AppendLine(str4);

                                        sceneWriteSB.AppendLine(str1);
                                        sceneWriteSB.AppendLine(str2);
                                        sceneWriteSB.AppendLine(str33);
                                        sceneWriteSB.AppendLine(str4);
                                        senceCount++;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // 递归处理游戏对象的子对象
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                GameObject childObject = gameObject.transform.GetChild(i).gameObject;
                ProcessGameObject(childObject);
            }
        }

        private void GetAllPrefab()
        {
            // 在项目中搜索所有预制体文件
            string[] originalPrefabGUIDs = AssetDatabase.FindAssets("t:Prefab");
            string[] ignorePrefabGUIDs = AssetDatabase.FindAssets("t:Prefab", new string[] { TEMP_FOLDER_PATH });
            // 忽略临时文件下的预制
            string[] prefabGUIDs = RemoveString(originalPrefabGUIDs, ignorePrefabGUIDs);

            foreach (string prefabGUID in prefabGUIDs)
            {
                // 根据 GUID 加载预制体
                string prefabPath = AssetDatabase.GUIDToAssetPath(prefabGUID);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                GetAllPrefabRender(prefab);
            }
        }

        //忽略临时文件下的预制
        private static string[] RemoveString(string[] array, string[] ignoreArray)
        {
            int tempCount = 0;
            int tempIndex = 0;
            for (int i = 0; i < ignoreArray.Length; i++)
            {
                int index = Array.IndexOf(array, ignoreArray[i]);
                if (index >= 0)
                {
                    tempCount++;
                }
            }
            if (tempCount > 0)
            {
                string[] strArray = new string[array.Length - tempCount];

                for (int i = 0; i < array.Length; i++)
                {
                    bool tembl = false;
                    for (int j = 0; j < ignoreArray.Length; j++)
                    {
                        if (array[i] == ignoreArray[j])
                        {
                            tembl = true;
                        }
                    }
                    if (!tembl)
                    {
                        strArray[tempIndex] = array[i];
                        tempIndex++;
                    }
                }
                return strArray;
            }
            return array;
        }

        private void GetAllPrefabRender(GameObject prefab)
        {
            // 处理预制体
            if (prefab != null)
            {
                bool bl = true;
                //是否是已被检测过的场景预制
                for (int i = 0; i < prefabList.Count; i++)
                {
                    if (IsSamePrefabPath(prefabList[i], prefab))
                    {
                        //Debug.Log("该预制 " + prefabList[i].name + " 已在场景中被检测");
                        bl = false;
                    }
                }

                if (bl)
                {
                    Renderer[] renderers = prefab.GetComponents<Renderer>();

                    foreach (Renderer renderer in renderers)
                    {
                        Material[] materials = renderer.sharedMaterials;

                        foreach (Material material in materials)
                        {
                            if (material.shader)
                            {
                                for (int i = 0; i < shaderList.Count; i++)
                                {
                                    if (material.shader == shaderList[i].shader)
                                    {
                                        string prefabPath = AssetDatabase.GetAssetPath(prefab);
                                        AssetImporter importer = AssetImporter.GetAtPath(prefabPath);

                                        string bundleName = !ReferenceEquals(importer, null) ? importer.assetBundleName : "None";
                                        string str1 = "预制路径: " + OpenScriptAll(prefabPath, prefabPath) + "，节点名字: " + prefab.name + ",AB名: " + bundleName;
                                        string str11 = "预制路径: " + prefabPath + "，节点名字: " + prefab.name + ",AB名: " + bundleName;
                                        string str2 = "Renderer材质: " + material.name + ",材质上的shader: " + material.shader;
                                        string str3 = "包含不支持的Shader脚本:" + OpenScriptAll(shaderList[i].shaderPath, shaderList[i].shaderPath);
                                        string str33 = "包含不支持的Shader脚本:" + shaderList[i].shaderPath;
                                        string str4 = shaderList[i].shaderNote;

                                        sb.AppendLine(str1);
                                        sb.AppendLine(str2);
                                        sb.AppendLine(str3);
                                        sb.AppendLine(str4);

                                        writeSB.AppendLine(str11);
                                        writeSB.AppendLine(str2);
                                        writeSB.AppendLine(str33);
                                        writeSB.AppendLine(str4);

                                        foundCount++;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // 递归处理游戏对象的子对象
            for (int i = 0; i < prefab.transform.childCount; i++)
            {
                GameObject childObject = prefab.transform.GetChild(i).gameObject;
                GetAllPrefabRender(childObject);
            }
        }
        private bool IsSamePrefabPath(string path, GameObject obj2)
        {
            if (obj2 == null)
            {
                return false;
            }
            string prefabAssetPath2 = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(obj2);
            return path == prefabAssetPath2;
        }
        private bool IsSamePrefab(GameObject obj1, GameObject obj2)
        {
            if (obj1 == null || obj2 == null)
            {
                return false;
            }
            string prefabAssetPath1 = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(obj1);
            string prefabAssetPath2 = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(obj2);
            return prefabAssetPath1 == prefabAssetPath2;
        }
    }

    class ShaderFinderJson
    {
        public string[] ShaderApi;
    }

    class ShaderParam
    {
        public Shader shader;
        public string shaderPath;
        public string shaderNote;
    }
}