using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using UnityEngine.Networking;

namespace QGMiniGame
{
    public class ShaderRuntimeDetector
    {
        static string scenesPath = "Assets/OPPO-GAME-SDK/Runtime/ShaderTools/ShaderScenes/ShaderDetection.unity";
        static string tempResourcesPath = "Assets/OPPO-GAME-SDK/Runtime/ShaderTools/Resources";
        static string tempFolderPath = "Assets/OPPO-GAME-SDK/Runtime/ShaderTools/Resources/tempFolder";
        static string fontPath = "Assets/OPPO-GAME-SDK/Runtime/ShaderTools/ShaderFont/OPPOFont-Bold.ttf";
        static string PackageDownloadUrl = "https://ie-activity-cn.heytapimage.com/static/minigame/OPPO-GAME-SDK/tools/AddShaderScenes.unitypackage";
        static string packagePath = Application.temporaryCachePath + "/AddShaderScenes.unitypackage";


        static bool isShader;
        static List<string> prefabList;
        static List<string> prefabObjects;
        static List<GameObject> saveObjects;
        static List<ShaderAssets> shaderAssetsList;
        static ShaderAssets shaderAssets;
        static List<string> assetsNames;

        public static void AddShaderDetection(bool bl)
        {
            Application.SetStackTraceLogType(LogType.Error, StackTraceLogType.ScriptOnly);
            Application.SetStackTraceLogType(LogType.Warning, StackTraceLogType.ScriptOnly);
            Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.ScriptOnly);

            bool isAddScenes = false;
            List<EditorBuildSettingsScene> editorBuildSettingsScenes = new List<EditorBuildSettingsScene>();
            EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;

            for (int i = 0; i < scenes.Length; i++)
            {
                if (bl && scenes[i].path == scenesPath)
                {
                    isAddScenes = true;
                }
            }

            if (bl && !isAddScenes)
            {
                if (scenesPath.IsValid())
                {
                    editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(scenesPath, true));
                }
                for (int i = 0; i < scenes.Length; i++)
                {
                    editorBuildSettingsScenes.Add(scenes[i]);
                }
                EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();
            }

            if (!bl)
            {
                for (int i = 0; i < scenes.Length; i++)
                {
                    if (scenes[i].path == scenesPath)
                    {
                        Debug.Log("Shader test scenario removed");
                    }
                    else
                    {
                        editorBuildSettingsScenes.Add(scenes[i]);
                    }
                }
                EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();
            }
            AssetDatabase.Refresh();
        }

        public static bool checkAddShaderSence()
        {
            bool sceneExists = AssetDatabase.IsValidFolder(scenesPath) || AssetDatabase.LoadAssetAtPath<SceneAsset>(scenesPath);
            bool fontExists = AssetDatabase.IsValidFolder(fontPath) || AssetDatabase.LoadAssetAtPath<Font>(fontPath);
            return !sceneExists || !fontExists;
        }

        public static bool checkLocalShaderSence()
        {
            return File.Exists(packagePath);
        }

        public static void StartDownload()
        {
            if (File.Exists(packagePath))
            {
                AssetDatabase.ImportPackage(packagePath, true);
            }
            else
            {
                var request = UnityWebRequest.Get(PackageDownloadUrl);
                request.downloadHandler = new DownloadHandlerBuffer();

                var operation = request.SendWebRequest();
                operation.completed += DownloadCompleted;
            }
        }

        private static void DownloadCompleted(AsyncOperation operation)
        {
            var request = (UnityWebRequestAsyncOperation)operation;
            if (request.webRequest.isNetworkError || request.webRequest.isHttpError)
            {
                Debug.LogError("Package download failed: " + request.webRequest.error);
            }
            else
            {
                var downloadHandler = request.webRequest.downloadHandler;
                if (downloadHandler != null)
                {
                    File.WriteAllBytes(packagePath, downloadHandler.data);

                    AssetDatabase.ImportPackage(packagePath, true);
                }
            }
        }

        public static void SaveAssetsInTempFolder(bool bl)
        {
            if (AssetDatabase.IsValidFolder(tempResourcesPath))
            {
                AssetDatabase.DeleteAsset(tempResourcesPath);
            }

            if (AssetDatabase.IsValidFolder(tempFolderPath))
            {
                AssetDatabase.DeleteAsset(tempFolderPath);
            }

            if (!bl)
            {
                AssetDatabase.Refresh();
                return;
            }

            if (!AssetDatabase.IsValidFolder(tempResourcesPath))
            {
                AssetDatabase.CreateFolder(tempResourcesPath.Substring(0, tempResourcesPath.LastIndexOf('/')),
                tempResourcesPath.Substring(tempResourcesPath.LastIndexOf('/') + 1));
            }

            if (!AssetDatabase.IsValidFolder(tempFolderPath))
            {
                AssetDatabase.CreateFolder(tempFolderPath.Substring(0, tempFolderPath.LastIndexOf('/')),
                tempFolderPath.Substring(tempFolderPath.LastIndexOf('/') + 1));
            }
            assetsNames = new List<string>();
            prefabObjects = new List<string>();
            SaveAllScenePrefab();
            SaveAllAssetsPrefab();
            SaveAllAsseteBundle();
            CheckSkyboxMaterial();
            RecordAssetsPath();
            AssetDatabase.Refresh();
        }

        public static void SaveAllAsseteBundle()
        {
            string streamingAssetsPath = Application.streamingAssetsPath;
            if (!AssetDatabase.IsValidFolder(streamingAssetsPath))
            {
                return;
            }
            string[] allAssetBundles = Directory.GetFiles(streamingAssetsPath);

            foreach (string bundlePath in allAssetBundles)
            {
                if (!bundlePath.EndsWith(".meta"))
                {
                    string bundleName = Path.GetFileName(bundlePath);
                    AssetBundle bundle = AssetBundle.LoadFromFile(bundlePath);

                    if (bundle != null)
                    {
                        string[] assetNames = bundle.GetAllAssetNames();

                        foreach (string name in assetNames)
                        {
                            if (IsSamePrefabName(name))
                            {
                                shaderAssets = new ShaderAssets();
                                shaderAssets.prefabNodeList = new List<string>();
                                shaderAssets.rendererNameList = new List<string>();
                                shaderAssets.materialList = new List<Material>();
                                shaderAssets.shaderList = new List<Shader>();

                                isShader = false;
                                GameObject prefab = bundle.LoadAsset<GameObject>(name);
                                getAllPrefabRender(prefab);

                                if (isShader)
                                {
                                    GameObject clone = GameObject.Instantiate(prefab, prefab.transform.position, prefab.transform.rotation);

                                    clone.AddComponent<ShaderAssets>();
                                    clone.GetComponent<ShaderAssets>().sceneName = shaderAssets.sceneName;
                                    clone.GetComponent<ShaderAssets>().scenePath = shaderAssets.scenePath;
                                    clone.GetComponent<ShaderAssets>().objectType = 3;
                                    clone.GetComponent<ShaderAssets>().prefabName = prefab.name;
                                    clone.GetComponent<ShaderAssets>().prefabPath = "None";
                                    clone.GetComponent<ShaderAssets>().prefabABName = bundleName;
                                    clone.GetComponent<ShaderAssets>().prefabNodeList = shaderAssets.prefabNodeList;
                                    clone.GetComponent<ShaderAssets>().rendererNameList = shaderAssets.rendererNameList;
                                    clone.GetComponent<ShaderAssets>().materialList = shaderAssets.materialList;
                                    clone.GetComponent<ShaderAssets>().shaderList = shaderAssets.shaderList;
                                    clone.GetComponent<ShaderAssets>().skyBoxPath = "None";

                                    string path = tempFolderPath + "/AssetBundle_" + prefab.name + ".prefab";
                                    string tempPrefabPath = "TempFolder/AssetBundle_" + prefab.name + ".prefab";
                                    assetsNames.Add(tempPrefabPath);
                                    prefabObjects.Add(prefab.name);
                                    GameObject saveObj = PrefabUtility.SaveAsPrefabAsset(clone, path);
                                    GameObject.DestroyImmediate(clone);
                                }
                            }
                        }

                        bundle.Unload(false);
                    }
                    else
                    {
                        Debug.LogError("Failed to load AssetBundle: " + bundleName);
                    }
                }
            }
            AssetDatabase.Refresh();
        }

        private static void SaveAllScenePrefab()
        {
            shaderAssetsList = new List<ShaderAssets>();
            prefabList = new List<string>();
            saveObjects = new List<GameObject>();
            EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;

            for (int i = 0; i < scenes.Length; i++)
            {
                EditorBuildSettingsScene scene = scenes[i];
                string scenePath = scene.path;
                if (!string.IsNullOrEmpty(scenePath))
                {
                    Scene loadedScene = UnityEditor.SceneManagement.EditorSceneManager.OpenScene(scenePath, UnityEditor.SceneManagement.OpenSceneMode.Single);
                    if (loadedScene.isLoaded)
                    {
                        shaderAssets = new ShaderAssets();
                        shaderAssets.prefabNodeList = new List<string>();
                        shaderAssets.rendererNameList = new List<string>();
                        shaderAssets.materialList = new List<Material>();
                        shaderAssets.shaderList = new List<Shader>();
                        isShader = false;
                        GameObject[] rootObjects = loadedScene.GetRootGameObjects();
                        foreach (GameObject rootObject in rootObjects)
                        {
                            ProcessGameObject(rootObject);
                            saveObjects.Add(rootObject);
                        }
                        if (isShader)
                        {
                            GameObject emptyGameObject = new GameObject("EmptyObject");
                            shaderAssets.sceneName = loadedScene.name;
                            shaderAssets.scenePath = loadedScene.path;
                            shaderAssets.objectType = 1;
                            shaderAssetsList.Add(shaderAssets);
                            foreach (var saveObject in saveObjects)
                            {
                                if (saveObject != null && saveObject != emptyGameObject)
                                {
                                    GameObject clone = GameObject.Instantiate(saveObject, saveObject.transform.position, saveObject.transform.rotation);
                                    clone.transform.SetParent(emptyGameObject.transform);
                                }
                            }
                            string tempPrefabPath = "TempFolder/Scenes_" + loadedScene.name + ".prefab";
                            assetsNames.Add(tempPrefabPath);
                            string path = tempFolderPath + "/Scenes_" + loadedScene.name + ".prefab";
                            GameObject saveObj = PrefabUtility.SaveAsPrefabAsset(emptyGameObject, path);
                            saveObj.AddComponent<ShaderAssets>();
                            saveObj.GetComponent<ShaderAssets>().sceneName = shaderAssets.sceneName;
                            saveObj.GetComponent<ShaderAssets>().scenePath = shaderAssets.scenePath;
                            saveObj.GetComponent<ShaderAssets>().objectType = shaderAssets.objectType;
                            saveObj.GetComponent<ShaderAssets>().prefabName = shaderAssets.prefabName;
                            saveObj.GetComponent<ShaderAssets>().prefabPath = shaderAssets.prefabPath;
                            saveObj.GetComponent<ShaderAssets>().prefabABName = shaderAssets.prefabABName;
                            saveObj.GetComponent<ShaderAssets>().prefabNodeList = shaderAssets.prefabNodeList;
                            saveObj.GetComponent<ShaderAssets>().rendererNameList = shaderAssets.rendererNameList;
                            saveObj.GetComponent<ShaderAssets>().materialList = shaderAssets.materialList;
                            saveObj.GetComponent<ShaderAssets>().shaderList = shaderAssets.shaderList;
                            saveObj.GetComponent<ShaderAssets>().skyBoxPath = "None";

                            GameObject.DestroyImmediate(emptyGameObject);
                            UnityEditor.SceneManagement.EditorSceneManager.SaveScene(loadedScene);
                            AssetDatabase.Refresh();
                        }
                    }
                }
            }
            saveObjects.Clear();
        }

        private static void ProcessGameObject(GameObject gameObject)
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
                                ShaderNode node = new ShaderNode();
                                node.nodeName = gameObject.name;
                                node.material = material;
                                node.shader = material.shader;
                                node.renderer = renderer;

                                shaderAssets.prefabNodeList.Add(gameObject.name);
                                shaderAssets.rendererNameList.Add(renderer.name);
                                shaderAssets.materialList.Add(material);
                                shaderAssets.shaderList.Add(material.shader);

                                isShader = true;
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                GameObject childObject = gameObject.transform.GetChild(i).gameObject;
                ProcessGameObject(childObject);
            }
        }

        private static void SaveAllAssetsPrefab()
        {
            string[] originalPrefabGUIDs = AssetDatabase.FindAssets("t:Prefab");
            string[] ignorePrefabGUIDs = AssetDatabase.FindAssets("t:Prefab", new string[] { tempFolderPath });
            string[] prefabGUIDs = RemoveString(originalPrefabGUIDs, ignorePrefabGUIDs);
            foreach (string prefabGUID in prefabGUIDs)
            {
                shaderAssets = new ShaderAssets();
                shaderAssets.prefabNodeList = new List<string>();
                shaderAssets.rendererNameList = new List<string>();
                shaderAssets.materialList = new List<Material>();
                shaderAssets.shaderList = new List<Shader>();
                isShader = false;
                string prefabPath = AssetDatabase.GUIDToAssetPath(prefabGUID);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                getAllPrefabRender(prefab);

                if (isShader)
                {
                    AssetImporter importer = AssetImporter.GetAtPath(prefabPath);
                    string bundleName = !ReferenceEquals(importer, null) ? importer.assetBundleName : "None";

                    GameObject clone = GameObject.Instantiate(prefab, prefab.transform.position, prefab.transform.rotation);
                    clone.AddComponent<ShaderAssets>();
                    clone.GetComponent<ShaderAssets>().sceneName = shaderAssets.sceneName;
                    clone.GetComponent<ShaderAssets>().scenePath = shaderAssets.scenePath;
                    clone.GetComponent<ShaderAssets>().objectType = 2;
                    clone.GetComponent<ShaderAssets>().prefabName = prefab.name;
                    clone.GetComponent<ShaderAssets>().prefabPath = prefabPath;
                    clone.GetComponent<ShaderAssets>().prefabABName = bundleName;
                    clone.GetComponent<ShaderAssets>().prefabNodeList = shaderAssets.prefabNodeList;
                    clone.GetComponent<ShaderAssets>().rendererNameList = shaderAssets.rendererNameList;
                    clone.GetComponent<ShaderAssets>().materialList = shaderAssets.materialList;
                    clone.GetComponent<ShaderAssets>().shaderList = shaderAssets.shaderList;
                    clone.GetComponent<ShaderAssets>().skyBoxPath = "None";

                    string path = tempFolderPath + "/Prefab_" + prefab.name + ".prefab";
                    string tempPrefabPath = "TempFolder/Prefab_" + prefab.name + ".prefab";
                    assetsNames.Add(tempPrefabPath);
                    prefabObjects.Add(prefab.name);
                    GameObject saveObj = PrefabUtility.SaveAsPrefabAsset(clone, path);
                    GameObject.DestroyImmediate(clone);
                }
            }
            AssetDatabase.Refresh();
        }

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

        private static void getAllPrefabRender(GameObject prefab)
        {
            if (prefab != null)
            {
                bool bl = true;
                for (int i = 0; i < prefabList.Count; i++)
                {
                    if (IsSamePrefabPath(prefabList[i], prefab))
                    {
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
                                ShaderNode node = new ShaderNode();
                                node.nodeName = prefab.name;
                                node.material = material;
                                node.shader = material.shader;
                                node.renderer = renderer;

                                shaderAssets.prefabNodeList.Add(prefab.name);
                                shaderAssets.rendererNameList.Add(renderer.name);
                                shaderAssets.materialList.Add(material);
                                shaderAssets.shaderList.Add(material.shader);
                                isShader = true;
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < prefab.transform.childCount; i++)
            {
                GameObject childObject = prefab.transform.GetChild(i).gameObject;
                getAllPrefabRender(childObject);
            }
        }
        private static bool IsSamePrefabPath(string path, GameObject obj)
        {
            if (obj == null)
            {
                return false;
            }
            string prefabAssetPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(obj);
            return path == prefabAssetPath;
        }

        private static bool IsSamePrefabName(string prefabname)
        {
            bool bl = false;
            for (global::System.Int32 i = 0; i < prefabObjects.Count; i++)
            {
                if (prefabObjects[i] == prefabname)
                {
                    bl = true;
                }
            }
            return bl;
        }

        public static void RecordAssetsPath()
        {
            assetsNames = new List<string>();
            string[] prefabPaths = Directory.GetFiles(tempFolderPath, "*.prefab", SearchOption.AllDirectories);

            foreach (string path in prefabPaths)
            {
                if (!string.IsNullOrEmpty(path))
                {
                    string prefabName = Path.GetFileNameWithoutExtension(path);
                    assetsNames.Add("TempFolder/" + prefabName);
                }
            }
            Scene targetScene = UnityEditor.SceneManagement.EditorSceneManager.OpenScene(scenesPath, UnityEditor.SceneManagement.OpenSceneMode.Single);

            if (targetScene.IsValid())
            {
                GameObject[] rootObjects = targetScene.GetRootGameObjects();

                foreach (GameObject rootObject in rootObjects)
                {
                    if (rootObject.name == "ShaderDetectionStarter")
                    {
                        TempPrefabName tf = rootObject.GetComponent<TempPrefabName>();
                        if (tf == null)
                        {
                            tf = rootObject.AddComponent<TempPrefabName>();
                        }
                        tf._prefabNames = assetsNames;
                        UnityEditor.SceneManagement.EditorSceneManager.SaveScene(targetScene);
                        AssetDatabase.Refresh();
                        break;
                    }
                }
            }
            else
            {
                Debug.Log("Invalid scene: " + targetScene);
            }
        }

        public static void CheckSkyboxMaterial()
        {
            string[] guids = AssetDatabase.FindAssets("t:Material");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                Material material = AssetDatabase.LoadAssetAtPath<Material>(path);

                if (material != null)
                {
                    Shader shader = material.shader;
                    if (shader && shader.name.StartsWith("Skybox/"))
                    {
                        shaderAssets = new ShaderAssets();
                        shaderAssets.prefabNodeList = new List<string>();
                        shaderAssets.rendererNameList = new List<string>();
                        shaderAssets.materialList = new List<Material>();
                        shaderAssets.shaderList = new List<Shader>();
                        shaderAssets.materialList.Add(material);
                        shaderAssets.shaderList.Add(shader);
                        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        Renderer cubeRenderer = cube.GetComponent<Renderer>();
                        cubeRenderer.material = material;
                        cube.AddComponent<ShaderAssets>();
                        cube.GetComponent<ShaderAssets>().sceneName = "None";
                        cube.GetComponent<ShaderAssets>().scenePath = "None";
                        cube.GetComponent<ShaderAssets>().objectType = 4;
                        cube.GetComponent<ShaderAssets>().prefabName = "None";
                        cube.GetComponent<ShaderAssets>().prefabPath = "None";
                        cube.GetComponent<ShaderAssets>().prefabABName = "None";
                        cube.GetComponent<ShaderAssets>().prefabNodeList = shaderAssets.prefabNodeList;
                        cube.GetComponent<ShaderAssets>().rendererNameList = shaderAssets.rendererNameList;
                        cube.GetComponent<ShaderAssets>().materialList = shaderAssets.materialList;
                        cube.GetComponent<ShaderAssets>().shaderList = shaderAssets.shaderList;
                        cube.GetComponent<ShaderAssets>().skyBoxPath = path;

                        string cubePath = tempFolderPath + "/SkyBox_" + material.name + ".prefab";
                        string tempPrefabPath = "TempFolder/SkyBox_" + material.name + ".prefab";
                        assetsNames.Add(tempPrefabPath);
                        GameObject saveObj = PrefabUtility.SaveAsPrefabAsset(cube, cubePath);
                        GameObject.DestroyImmediate(cube);
                    }
                }
            }
            AssetDatabase.Refresh();

        }
    }
}