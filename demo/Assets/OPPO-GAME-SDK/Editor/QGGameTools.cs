using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using System.Text.RegularExpressions;
using UnityEngine.Rendering;
using System.Diagnostics;
using System.Threading;
using System.Net;
using System.Collections;
using ICSharpCode.SharpZipLib.Zip;
using static System.Net.Mime.MediaTypeNames;
using System.Threading.Tasks;
namespace QGMiniGame
{


    public class QGGameTools
    {
        //构建小游戏
        public static void ConvetWebGl(string buildSrc, string webglSrc)
        {
            QGLog.Log("[BuildMiniGame] Start: Please Waitting");

            /*         CopyDirectory(Path.Combine(UnityEngine.Application.dataPath, "OPPO-GAME-SDK/Default"), buildSrc, true);*/

            //拼接打包命令
            string commandStr ="quickgame unity";
            var getConfig = GetEditorConfig();

            if (getConfig.useAddressable)
            {
                commandStr += (" --addressable=" + getConfig.envConfig.streamingAssetsUrl);
            }

            commandStr += (" --gameName=" + getConfig.projectName);
            commandStr += (" --unityVer=" + UnityEngine.Application.unityVersion);
            commandStr += (" --icon=" + getConfig.iconPath);

            commandStr += (" --packageName=" + getConfig.packageName);
 
            var orientationArr = new[] { "portrait", "landscape" };
            commandStr += (" --orientation=" + orientationArr[getConfig.orientation]);
            commandStr += (" --versionName=" + getConfig.projectVersionName);
            commandStr += (" --versionCode=" + getConfig.projectVersion);
            commandStr += (" --minPlatformVersion=" + getConfig.minPlatVersion);

            if (getConfig.useSign)
            {
                commandStr += (" --signCertificate=" + getConfig.signCertificate);
                commandStr += (" --signPrivate=" + getConfig.signPrivate);
                commandStr += " release";
            }

            // 要执行的命令字符串
            string commandString = "cd /d " + webglSrc + " && " + commandStr;
            // 创建 ProcessStartInfo 对象
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/c \"" + commandString + "\"";
            // 设置 UseShellExecute 为 false
            startInfo.UseShellExecute = false;
            // 重定向标准输出流
            startInfo.RedirectStandardOutput = true;

            startInfo.CreateNoWindow = true;
            // 创建进程并执行命令
            Process commandProcess = new Process();
            commandProcess.StartInfo = startInfo;
            commandProcess.Start();
            // 创建 CancellationTokenSource 对象
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            // 启动后台任务异步读取标准输出流
            Task.Factory.StartNew(() =>
            {
                while (!tokenSource.IsCancellationRequested)
                {
                    string output = commandProcess.StandardOutput.ReadLine();
                    if (output != null)
                    {
                        QGLog.Log(output);
                    }
                    else
                    {
                        break;
                    }
                }
            }, tokenSource.Token);
            // 等待命令执行完毕
            commandProcess.WaitForExit();
            // 终止异步任务
            tokenSource.Cancel();

            QGLog.Log(commandString);

            /*        CmdRunner.RunBat("build.bat", "", buildSrc);*/
            MyCommandEnd(buildSrc);
        }

        [MenuItem("MyMenu/MyCommand")]
        static void MyCommandEnd(string buildSrc)
        {
            // 创建一个提示框，显示"Hello World!"消息，点击确定按钮后返回true
            bool result = EditorUtility.DisplayDialog("提示", "完成打包！", "确定");

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

        private static bool CopyDirectory(string SourcePath, string DestinationPath, bool overwriteexisting)
        {
            bool ret = false;
            var separator = Path.DirectorySeparatorChar;
            var ignoreFiles = new List<string>() { "webgl.loader.js", "webgl.wasm.framework.unityweb", "webgl.framework.js", "UnityLoader.js" };

            RenameFile[] renameFiles = {
                new RenameFile()
                {
                    oldName = "webgl.data",
                    newName = "webgl.data.unityweb"
                },
                 new RenameFile()
                {
                    oldName = "webgl.wasm",
                    newName = "webgl.wasm.code.unityweb"
                }
            };
            var ignoreDirs = new List<string>() { };
            try
            {

                if (Directory.Exists(SourcePath))
                {
                    if (Directory.Exists(DestinationPath) == false)
                    {
                        Directory.CreateDirectory(DestinationPath);
                    }
                    else
                    {
                        // 已经存在，删掉目录下无用的文件
                        foreach (string filename in ignoreFiles)
                        {
                            var filepath = Path.Combine(DestinationPath, filename);
                            if (File.Exists(filepath))
                            {
                                File.Delete(filepath);
                            }
                        }
                        foreach (string dir in ignoreDirs)
                        {
                            var dirpath = Path.Combine(DestinationPath, dir);
                            if (Directory.Exists(dirpath))
                            {
                                Directory.Delete(dirpath);
                            }
                        }
                    }

                    foreach (string fls in Directory.GetFiles(SourcePath))
                    {

                        FileInfo flinfo = new FileInfo(fls);
                        if (flinfo.Extension == ".meta" || ignoreFiles.Contains(flinfo.Name))
                        {
                            continue;
                        }

                        string targetFileName = flinfo.Name;
                        foreach (RenameFile renameFile in renameFiles)
                        {
                            if (renameFile.oldName.Equals(flinfo.Name))
                            {
                                targetFileName = renameFile.newName;
                                break;
                            }
                        }

                        flinfo.CopyTo(Path.Combine(DestinationPath, targetFileName), overwriteexisting);

                    }
                    foreach (string drs in Directory.GetDirectories(SourcePath))
                    {
                        DirectoryInfo drinfo = new DirectoryInfo(drs);
                        if (ignoreDirs.Contains(drinfo.Name))
                        {
                            continue;
                        }
                        if (CopyDirectory(drs, Path.Combine(DestinationPath, drinfo.Name), overwriteexisting) == false)
                            ret = false;
                    }
                }
                ret = true;
            }
            catch (Exception ex)
            {
                ret = false;
                UnityEngine.Debug.LogError(ex);
            }
            return ret;
        }

        //构建WebGL
        public static void BuildWebGL(string srcPath,string buildSrc)
        {
            string webglPath = Path.Combine(buildSrc, "webgl");
            DelectDir(webglPath);

            QGLog.Log("[BuildWebGL] Start: Please Waitting");
            BuildOptions option = BuildOptions.None;
            BuildPipeline.BuildPlayer(GetScenePaths(), srcPath, BuildTarget.WebGL, option);
            QGLog.Log("[BuildWebGL] Done: " + srcPath);
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
            PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.WebGL, false);
            GraphicsDeviceType[] targets = { GraphicsDeviceType.OpenGLES2 };
            PlayerSettings.SetGraphicsAPIs(BuildTarget.WebGL, targets);
            PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Disabled;
            PlayerSettings.WebGL.template = "APPLICATION:Minimal";
            EditorSettings.spritePackerMode = SpritePackerMode.AlwaysOnAtlas;
            PlayerSettings.WebGL.linkerTarget = WebGLLinkerTarget.Wasm;
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

        //配置文件获取
        public static QGGameConfig GetEditorConfig()
        {
            var config = AssetDatabase.LoadAssetAtPath("Assets/OPPO-GAME-SDK/Editor/QGGameConfig.asset", typeof(QGGameConfig)) as QGGameConfig;
            if (config == null)
            {
                AssetDatabase.CreateAsset(EditorWindow.CreateInstance<QGGameConfig>(), "Assets/OPPO-GAME-SDK/Editor/QGGameConfig.asset");
                config = AssetDatabase.LoadAssetAtPath("Assets/OPPO-GAME-SDK/Editor/QGGameConfig.asset", typeof(QGGameConfig)) as QGGameConfig;
            }
            return config;
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

        //配置文件设置
        public static void setEditorConfig(string buildSrc, string packageName, string projectName, int orientation, string projectVersionName, string projectVersion, string minPlatVersion,bool useAddressable, string streamingAssetsUrl,string iconPath,bool useSign,string signCertificate,string signPrivate)
        {
            var config = GetEditorConfig();
            if (buildSrc != String.Empty)
            {
                config.buildSrc = buildSrc;
            }
            if (packageName != String.Empty)
            {
                config.packageName = packageName;
            }
            if (projectName != String.Empty)
            {
                config.projectName = projectName;
            }
            config.orientation = orientation;

            if (projectVersionName != String.Empty)
            {
                config.projectVersionName = projectVersionName;
            }
            if (projectVersion != String.Empty)
            {
                config.projectVersion = projectVersion;
            }
            if (minPlatVersion != String.Empty)
            {
                config.minPlatVersion = minPlatVersion;
            }
            if (streamingAssetsUrl != String.Empty)
            {
                config.envConfig.streamingAssetsUrl = streamingAssetsUrl;
            }
            if (iconPath != String.Empty)
            {
                config.iconPath = iconPath;
            }
            if (signCertificate != String.Empty)
            {
                config.signCertificate = signCertificate;
            }
            if (signPrivate != String.Empty)
            {
                config.signPrivate = signPrivate;
            }
            config.useAddressable = useAddressable;
            config.useSign = useSign;
            EditorUtility.SetDirty(config);
            AssetDatabase.SaveAssets();
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
    }
}
