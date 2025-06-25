using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine.Rendering;
using UnityEditor.Build.Reporting;

namespace QGMiniGame
{
    public static class QGGameTools
    {
        private const string EDITOR_CONFIG_PATH = "Assets/OPPO-GAME-SDK/Editor/BuildTool/QGBuildConfig.asset";

        public static bool BuildGame(bool showResultWindow = false)
        {
            // 获取导出路径
            var exportPath = BuildConfigAsset.Fundamentals.exportPath;
            var webGLExportPath = Path.Combine(exportPath, GlobalDefines.WEBGL_BUILD_DIR);
            // 进行符合小游戏规范的项目设置
            SetPlayer();
            // 获取当前是否使用WEBGL2.0
            GetUserWebGLVersion();
            // 删除之前构建的目录
            DelectDir(webGLExportPath);
            // 构建导出 WebGL 工程
            var buildReport = BuildWebGL(webGLExportPath);
            var success = buildReport.summary.result == BuildResult.Succeeded;
            if (success)
            {
                // 开始将 WebGL 转化为小游戏工程
                success = ConvetWebGL(exportPath, webGLExportPath, showResultWindow);
            }
            return success;
        }

        //构建小游戏
        public static bool ConvetWebGL(string buildSrc, string webglSrc, bool showResultWindow = false)
        {
            QGLog.Log("[BuildMiniGame] Start: Please Waitting");

            /*         CopyDirectory(Path.Combine(UnityEngine.Application.dataPath, "OPPO-GAME-SDK/Default"), buildSrc, true);*/

            //拼接打包命令
            string commandStr = "quickgame unity";

            if (BuildConfigAsset.Fundamentals.useRemoteStreamingAssets && BuildConfigAsset.Fundamentals.streamingAssetsURL.IsValid())
            {
                commandStr += (" --addressable=" + BuildConfigAsset.Fundamentals.streamingAssetsURL.ToPlatformQuoted());
            }

            commandStr += (" --gameName=" + BuildConfigAsset.Fundamentals.projectName.ToPlatformQuoted());
            commandStr += (" --unityVer=" + Application.unityVersion.ToPlatformQuoted());
            commandStr += (" --icon=" + BuildConfigAsset.Fundamentals.iconPath.ToPlatformQuoted());

            commandStr += (" --packageName=" + BuildConfigAsset.Fundamentals.packageName.ToPlatformQuoted());

            var orientationArr = new[] { "portrait", "landscape", "landscapeLeft", "landscapeRight" };
            commandStr += (" --orientation=" + orientationArr[BuildConfigAsset.Fundamentals.orientation].ToPlatformQuoted());
            commandStr += (" --versionName=" + BuildConfigAsset.Fundamentals.projectVersionName.ToPlatformQuoted());
            commandStr += (" --versionCode=" + BuildConfigAsset.Fundamentals.projectVersion);
            commandStr += (" --minPlatformVersion=" + BuildConfigAsset.Fundamentals.minPlatformVersion);

            //AssetBundle
            if (BuildConfigAsset.AssetCache.available)
            {
                if (!BuildConfigAsset.AssetCache.enableBundleCache)
                {
                    commandStr += (" --disableBundleCache=true");
                }
                commandStr += (" --keepOldVersion=" + BuildConfigAsset.AssetCache.keepOldVersion);
                commandStr += (" --enableCacheLog=" + BuildConfigAsset.AssetCache.enableCacheLog);
                commandStr += (" --gameCDNRoot=" + BuildConfigAsset.AssetCache.gameCDNRoot.ToPlatformQuoted());
                commandStr += (" --bundlePathIdentifier=" + BuildConfigAsset.AssetCache.bundlePathIdentifier.ToPlatformQuoted());
                commandStr += (" --excludeFileExtensions=" + BuildConfigAsset.AssetCache.excludeFileExtensions.ToPlatformQuoted());
                commandStr += (" --excludeClearFiles=" + BuildConfigAsset.AssetCache.excludeClearFiles.ToPlatformQuoted());
                commandStr += (" --bundleHashLength=" + BuildConfigAsset.AssetCache.bundleHashLength);
                commandStr += (" --defaultReleaseSize=" + BuildConfigAsset.AssetCache.defaultReleaseSize);
            }

            //UnityWebGLVersion
            if (BuildConfigAsset.AssetCache.unityUseWebGL2)
            {
                commandStr += (" --unityUseWebGL2=" + BuildConfigAsset.AssetCache.unityUseWebGL2);
            }

            if (BuildConfigAsset.Fundamentals.useCustomSign)
            {
                commandStr += (" --signCertificate=" + BuildConfigAsset.Fundamentals.signCertificate.ToPlatformQuoted());
                commandStr += (" --signPrivate=" + BuildConfigAsset.Fundamentals.signPrivate.ToPlatformQuoted());
                commandStr += " release";
            }

            // 执行打包命令
            var success = true;
            try
            {
                ShellHelper.ExecuteCommand(commandStr, webglSrc);
            }
            catch
            {
                success = false;
            }

            if (success && showResultWindow)
            {
                // 创建一个提示框，显示"Hello World!"消息，点击确定按钮后返回true
                string unityUseWebGL2 = $"渲染版本: {(BuildConfigAsset.AssetCache.unityUseWebGL2 ? "WebGL2.0" : "WebGL1.0")}";
                string useCustomSign = $"发布类型: {(BuildConfigAsset.Fundamentals.useCustomSign ? "release" : "debug")}";
                bool result = EditorUtility.DisplayDialog("提示", $"完成打包\n\n{unityUseWebGL2}\n{useCustomSign}", "确定");

                // 检查返回值
                if (result)
                {
                    ShowInExplorer(buildSrc);
                }
                else
                {
                    QGLog.Log("");
                }
            }

            return success;
        }

        // 删除文件夹
        public static void DelectDir(string srcPath)
        {
            if (!Directory.Exists(srcPath))
            {
                return;
            }
            try
            {
                DirectoryInfo dir = new DirectoryInfo(srcPath);
                FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();

                foreach (FileSystemInfo i in fileinfo)
                {
                    if (i is DirectoryInfo)
                    {
                        DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                        subdir.Delete(true);
                    }
                    else
                    {
                        File.Delete(i.FullName);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //构建WebGL
        public static BuildReport BuildWebGL(string path)
        {
            QGLog.Log("[BuildWebGL] Start: Please Waitting");
            BuildOptions option = BuildOptions.None;
            var report = BuildPipeline.BuildPlayer(GetScenePaths(), path, BuildTarget.WebGL, option);
            QGLog.Log("[BuildWebGL] Done: " + path);
            return report;
        }

        // 设置打包成 webgl的参数 
        public static void SetPlayer()
        {
#if UNITY_2021 || UNITY_2020
            PlayerSettings.colorSpace = ColorSpace.Gamma;
            PlayerSettings.WebGL.decompressionFallback = false;
#endif
            PlayerSettings.runInBackground = false;
            PlayerSettings.WebGL.threadsSupport = false;
            // PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.WebGL, false);
            // GraphicsDeviceType[] targets = { GraphicsDeviceType.OpenGLES2 };
            // PlayerSettings.SetGraphicsAPIs(BuildTarget.WebGL, targets);
            PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Disabled;
            PlayerSettings.WebGL.template = "APPLICATION:Minimal";
            EditorSettings.spritePackerMode = SpritePackerMode.AlwaysOnAtlas;
            PlayerSettings.WebGL.linkerTarget = WebGLLinkerTarget.Wasm;
            //取消文件名以哈希命名
            PlayerSettings.WebGL.nameFilesAsHashes = false;
        }

        //获取游戏中的场景
        public static string[] GetScenePaths()
        {
            List<string> scenes = new List<string>();
            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                var scene = EditorBuildSettings.scenes[i];

                if (scene.enabled)
                {
                    scenes.Add(scene.path);
                }
            }

            return scenes.ToArray();
        }

        //打开指定的文件 
        public static void ShowInExplorer(string path)
        {
            if (IsInWinOS)
            {
                OpenInWin(path);
            }
            else if (IsInMacOS)
            {
                OpenInMac(path);
            }
            else
            {
                QGLog.LogError("ShowInExplorer error not mac and win");
            }
        }

        private static bool IsInMacOS
        {
            get
            {
                return SystemInfo.operatingSystem.IndexOf("Mac OS") != -1;
            }
        }

        private static bool IsInWinOS
        {
            get
            {
                return SystemInfo.operatingSystem.IndexOf("Windows") != -1;
            }
        }

        private static void OpenInWin(string path)
        {
            bool openInsidesOfFolder = false;

            string winPath = path.Replace("/", "\\");

            if (Directory.Exists(winPath))
            {
                openInsidesOfFolder = true;
            }

            try
            {
                System.Diagnostics.Process.Start("explorer.exe", (openInsidesOfFolder ? "/root," : "/select,") + winPath);
            }
            catch (System.ComponentModel.Win32Exception e)
            {
                e.HelpLink = "";
            }
        }

        private static void OpenInMac(string path)
        {
            bool openInsidesOfFolder = false;

            // try mac
            string macPath = path.Replace("\\", "/");

            if (Directory.Exists(macPath))
            {
                openInsidesOfFolder = true;
            }

            if (!macPath.StartsWith("\""))
            {
                macPath = "\"" + macPath;
            }

            if (!macPath.EndsWith("\""))
            {
                macPath = macPath + "\"";
            }

            string arguments = (openInsidesOfFolder ? "" : "-R ") + macPath;

            try
            {
                System.Diagnostics.Process.Start("open", arguments);
            }
            catch (System.ComponentModel.Win32Exception e)
            {
                e.HelpLink = "";
            }
        }

        public static void ShaderTestTool()
        {

            string shaderTestToolUrl = "https://ie-activity-cn.heytapimage.com/static/minigame/hall/example123456/assets/com.oppo.ShaderDemo.rpk";

            UnityEngine.Application.OpenURL(shaderTestToolUrl);
        }

        //当前是否使用WebGL2.0版本
        public static void GetUserWebGLVersion()
        {
            BuildConfigAsset.AssetCache.unityUseWebGL2 = false;
            // 获取当前图形 API 列表
            GraphicsDeviceType[] graphicsAPIs = PlayerSettings.GetGraphicsAPIs(BuildTarget.WebGL);

            // 检查使用的图形 API
            foreach (var api in graphicsAPIs)
            {
                QGLog.Log("检查使用的图形 API: " + api);
                if (api == GraphicsDeviceType.OpenGLES3)
                {
                    BuildConfigAsset.AssetCache.unityUseWebGL2 = true;
                }
            }
        }
    }
}