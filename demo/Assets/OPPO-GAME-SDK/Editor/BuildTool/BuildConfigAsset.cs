using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace QGMiniGame
{
    [Serializable]
    public class BuildFundamentalConfig
    {
        public const int MIN_PLATFORM_VERSION = 1103;
        public const int MIN_PROJECT_VERSION = 1;

        public bool useCustomSign;
        public bool useRemoteStreamingAssets;
        public int minPlatformVersion = MIN_PLATFORM_VERSION;
        public int orientation;
        public int projectVersion = MIN_PROJECT_VERSION;
        public string exportPath;
        public string iconPath;
        public string packageName;
        public string projectName;
        public string projectVersionName;
        public string signCertificate;
        public string signPrivate;
        public string streamingAssetsURL;
    }

    [Serializable]
    public class BuildAssetCacheConfig
    {
        public bool enableBundleCache = true;
        public bool enableCacheLog;
        public bool keepOldVersion;
        public int bundleHashLength = 32;
        public int defaultReleaseSize = 30;
        public string bundlePathIdentifier;
        public string excludeClearFiles;
        public string excludeFileExtensions = ".json;.hash";
        public bool unityUseWebGL2;

        [NonSerialized]
        public bool available;
    }

    public class BuildConfigAsset : ScriptableObject
    {
        public static BuildFundamentalConfig Fundamentals => Instance.fundamentals;
        [SerializeField]
        private BuildFundamentalConfig fundamentals;

        public static BuildAssetCacheConfig AssetCache =>  Instance.assetCache;
        [SerializeField]
        private BuildAssetCacheConfig assetCache;

        private BuildConfigAsset() { }

        private const string ASSET_DIR_NAME = "OPPO-GAME-SDK_UserData";
        private static readonly string asset_path = Path.Combine("Assets", ASSET_DIR_NAME, "BuildConfig.asset");

        private static BuildConfigAsset Instance
        {
            get
            {
                if (!instance)
                {
                    instance = AssetDatabase.LoadAssetAtPath<BuildConfigAsset>(asset_path);
                    if (!instance)
                    {
                        instance = CreateInstance<BuildConfigAsset>();
                        instance.hideFlags = HideFlags.HideInInspector;
                        if (!AssetDatabase.IsValidFolder(Path.Combine("Assets", ASSET_DIR_NAME)))
                        {
                            AssetDatabase.CreateFolder("Assets", ASSET_DIR_NAME);
                        }
                        AssetDatabase.CreateAsset(instance, asset_path);
                    }
                }
                return instance;
            }
        }
        private static BuildConfigAsset instance;

        public static void Save()
        {
            if (!instance) return;
            EditorUtility.SetDirty(instance);
#if UNITY_2020_3_OR_NEWER
            AssetDatabase.SaveAssetIfDirty(instance);
#else
            AssetDatabase.SaveAssets();
#endif
        }
    }
}