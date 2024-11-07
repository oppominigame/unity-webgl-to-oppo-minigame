using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using UnityEngine.Networking;
using System.Timers;

namespace QGMiniGame
{
    using ResultBehaviour = ShaderStaticDetector.ResultBehaviour;
    using Debug = UnityEngine.Debug;

    public class BuildEditorWindow : EditorWindow
    {
        private enum RequestVersionStatus
        {
            Fail = -1,
            InProgress,
            Success
        }

        private enum ImportStatus : int
        {
            FirstInjection = 0, //首次注入
            Importimg = 1,      //正常注入
            UpgradeCompleted = 2, //升级完成
        }

        private const string WEBGL_BUILD_DIR = "webgl";
        private const string ASSET_CACHE_SYSTEM_MIN_VERSION = "2.1.8-beta.0";
        private const string SDK_SERVER_URL = "https://ie-activity-cn.heytapimage.com/static/minigame/OPPO-GAME-SDK/tools";
        private const string UPDATE_LOG_URL = "https://github.com/oppominigame/unity-webgl-to-oppo-minigame/blob/main/CHANGELOG.md";
        private const string CONTACT_US_URL = "https://github.com/oppominigame/unity-webgl-to-oppo-minigame/blob/main/doc/IssueAndContact.md.md";
        private const string OPENSSL_EXE_URL = "https://ie-activity-cn.heytapimage.com/static/minigame/hall/example/20240918/assets/OpenSSL-Win64.zip";
        private static string OPENSSL_EXE_UNZIP_PATH = "";
        private static string OPENSSL_EXE_LOCAL_PATH = "";
        private static string GenerateCertificatePath = "";     //证书路径
        private static bool isHaveCertificatePath = false;      //是否导入证书
        private static bool isInstallOpenssl = false;           //是否安装openssl(全局)
        private static bool isHaveOpenSslZIP = false;           //是否解压openssl(zip)
        private static float ImportPackagTime = 0;      //导入计时   
        private static float ImportPackagOvertime = 10; //导入超时
        private static Timer timer;
        private static string ImportingClass = $"{nameof(BuildEditorWindow)}.ImportingStatus";

        private static BuildEditorWindow instance;

        private void OnEnable()
        {
            OPENSSL_EXE_UNZIP_PATH = $"{Application.temporaryCachePath}/OpenSSL-Win64";
            OPENSSL_EXE_LOCAL_PATH = $"{OPENSSL_EXE_UNZIP_PATH}/bin/openssl.exe";
        }
        private static bool HasOpenInstances
#if UNITY_2019_3_OR_NEWER
            => HasOpenInstances<BuildEditorWindow>();
#else
        {
            get
            {
                var instances = Resources.FindObjectsOfTypeAll(typeof(BuildEditorWindow));
                return instances != null && instances.Length != 0;
            }
        }
#endif

        private string LatestSDKPackageTempPath => Path.Combine(Application.temporaryCachePath, latestSDKPackageName);

        /// <summary>
        /// 校验集合，值为空代表通过，不为空代表错误信息
        /// </summary>
        private readonly Dictionary<string, string> validationMap = new Dictionary<string, string>();
        /// <summary>
        /// 立即触发1次校验的键列表
        /// </summary>
        private readonly HashSet<string> validateForOnceKeyList = new HashSet<string>();
        /// <summary>
        /// 是否产生了键盘输入提交型脏数据
        /// </summary>
        private bool hasAnyInputDirtyData;
        /// <summary>
        /// 是否正在构建
        /// </summary>
        private bool isBuilding = false;
        /// <summary>
        /// 是否正在升级 SDK
        /// </summary>
        private bool isUpgradingSDK = false;
        /// <summary>
        /// 是否正在升级打包工具
        /// </summary>
        private bool isUpgradingQGBuildTool = false;
        /// <summary>
        /// 是否使用Shader真机检测
        /// </summary>
        private bool useRuntimeShaderDetection;
        /// <summary>
        /// 是否打包时检查Shader
        /// </summary>
        private bool checkShaderOnBuild;
        /// <summary>
        /// 构建验证时的帧计数器（要等待界面下一帧刷新来验证）
        /// </summary>
        private int buildValidateFrameCounter;
        /// <summary>
        /// 当前输入焦点id
        /// </summary>
        private int keyboardControlID;
        /// <summary>
        /// 获取打包工具版本进程
        /// </summary>
        private Process qgBuildToolVersionProcess;
        /// <summary>
        /// 获取打包工具版本信息状态
        /// </summary>
        private RequestVersionStatus requestQGBuildToolVersionStatus = RequestVersionStatus.InProgress;
        /// <summary>
        /// 当前打包工具版本
        /// </summary>
        private string currentBuildToolVersion;
        /// <summary>
        /// 最新打包工具版本
        /// </summary>
        private string latestBuildToolVersion;
        /// <summary>
        /// 获取工具包版本信息状态
        /// </summary>
        private RequestVersionStatus requestSDKVersionStatus = RequestVersionStatus.InProgress;
        /// <summary>
        /// 最新 SDK 版本
        /// </summary>
        private string latestSDKVersion;
        /// <summary>
        /// 最新 SDK 包名称
        /// </summary>
        private string latestSDKPackageName;
        /// <summary>
        /// 最新 SDK 包的 MD5
        /// </summary>
        private string latestSDKPackageMD5;
        /// <summary>
        /// 主面板滚动视图当前位置
        /// </summary>
        private Vector2 configPanelScrollPosition;

        [MenuItem(GlobalDefines.MINIGAME_MENU_ITEM_ROOT + "/打包工具", false, 1)]
        private static void Open()
        {
#if !(UNITY_2018_1_OR_NEWER)
            UnityEngine.Debug.LogError("目前仅支持 Unity2018及以上的版本！");
#endif
            // 已打开则聚焦
            if (HasOpenInstances)
            {
                instance = GetOrCreateWindow();
                return;
            }
            // 初始化并打开窗口
            instance = GetOrCreateWindow();
            instance.minSize = new Vector2(650, 800);
            instance.maxSize = new Vector2(1600, 950);
            // 立即更新版本信息
            instance.OnWindowOpen();
        }

        private static bool IsProcessSafeExit(Process process) => process.ExitCode == 0;

        private static BuildEditorWindow GetOrCreateWindow() => GetWindow(typeof(BuildEditorWindow), false, "打包小游戏", true) as BuildEditorWindow;
        private static Process RunCommandProcess(string command)
        {
            return CmdRunner.CreateShellExProcess("cmd.exe", $"/c \"{command}\"");
        }

        private static bool RunCommandProcessWait(string command, out string output)
        {
            // 等待进程执行完成退出
            var process = RunCommandProcess(command);
            process.WaitForExit();
            // 成功返回输出，失败返回错误信息
            var isSuccess = IsProcessSafeExit(process);
            var standardOutput = process.StandardOutput.ReadToEnd();
            output = isSuccess ? standardOutput : process.StartInfo.Arguments + standardOutput;
            // 控制台输出错误信息
            if (!isSuccess)
            {
                Debug.LogError(output);
            }
            // 返回执行结果
            return isSuccess;
        }

        public static async Task GenerateCertificate(string command, string[] generateInfo, string path)
        {
            GenerateCertificatePath = path;
            await DisplayProgressAndCheckOpensslInstallation();

            if (!isHaveOpenSslZIP && !isInstallOpenssl) //既没有全局openssl,也没有本地压缩包
            {
                EditorUtility.DisplayDialog("提示", "请安装openssl环境", "确认");
                return;
            }
            // Openssl 已安装，开始创建证书
            EditorUtility.DisplayProgressBar("创建证书", "openssl 已安装", 0.7f);

            var error = await CreateCertificate(command, generateInfo);
            if (HandleError(error, "证书生成失败")) return;

            // 证书生成成功
            DisplayProgress("创建证书", "证书已生成", 0.9f);
            EditorUtility.ClearProgressBar();

            if (EditorUtility.DisplayDialog("提示", "证书已生成", "确认")) ;
            QGGameTools.ShowInExplorer(GenerateCertificatePath);
            isHaveCertificatePath = true;
        }

        private static async Task DisplayProgressAndCheckOpensslInstallation()
        {
            DisplayProgress("创建证书", "openssl 正在检测环境: ", 0.1f);
            var error = await IsInstallOpenssl();

            if (error.IsValid())
            {
                if (await HandleOpensslDownloadAndInstall()) return;
            }
        }

        private static async Task<bool> HandleOpensslDownloadAndInstall()
        {
            if (!File.Exists(OPENSSL_EXE_LOCAL_PATH) && !File.Exists($"{OPENSSL_EXE_UNZIP_PATH}/OpenSSL-Win64.zip"))
            {
                if (!ShowDownloadDialog("需要下载openssl")) return true;

                DisplayProgress("创建证书", "openssl 下载中...", 0.3f);
                var error = await StartDownloadOpenssl();
                if (HandleError(error, "openssl下载失败")) return true;

                DisplayProgress("创建证书", "openssl 下载成功", 0.4f);
            }

            // if (!ShowDownloadDialog("需要安装openssl")) return true; 默认安装
            DisplayProgress("创建证书", "openssl 安装中...", 0.5f);
            var installError = await StartsInstallOpenssl();
            return HandleError(installError, "openssl安装失败");
        }

        private static bool ShowDownloadDialog(string message)
        {
            return EditorUtility.DisplayDialog("提示", message, "确认", "取消");
        }

        private static void DisplayProgress(string title, string message, float progress)
        {
            EditorUtility.DisplayProgressBar(title, message, progress);
        }

        private static bool HandleError(string error, string errorMessage)
        {
            if (error.IsValid())
            {
                EditorUtility.ClearProgressBar();
                return EditorUtility.DisplayDialog("提示", errorMessage, "确认");
            }
            return false;
        }

        /// <summary>
        /// 异步判断是否安装openssl,以及是否已解压本地openssl
        /// </summary>
        /// <returns>错误信息（没有代表成功）</returns>
        private static Task<string> IsInstallOpenssl()
        {
            // 异步执行
            return Task.Run(() =>
            {
                isInstallOpenssl = RunCommandProcessWait("openssl -v", out var output);
                isHaveOpenSslZIP = File.Exists(OPENSSL_EXE_LOCAL_PATH);
                if (isInstallOpenssl || isHaveOpenSslZIP)
                {
                    return string.Empty;
                }
                // 失败返回错误
                return output;
            });
        }

        /// <summary>
        /// 异步下载openssl(下载压缩包)
        /// </summary>
        /// <returns>错误信息（没有代表成功）</returns>
        private static async Task<string> StartDownloadOpenssl()
        {
            // 异步执行
            return await Task.Run(() =>
            {
                if (!Directory.Exists(OPENSSL_EXE_UNZIP_PATH))
                {
                    Directory.CreateDirectory(OPENSSL_EXE_UNZIP_PATH);
                }
                var isSuccess = RunCommandProcessWait($"curl -o {OPENSSL_EXE_UNZIP_PATH}/OpenSSL-Win64.zip {OPENSSL_EXE_URL}", out var output);
                if (isSuccess)
                {
                    return string.Empty;
                }
                return output;
            });
        }


        /// <summary>
        /// 异步安装openssl(解压缩包)
        /// </summary>
        /// <returns>错误信息（没有代表成功）</returns>
        private static async Task<string> StartsInstallOpenssl()
        {
            // 异步执行
            return await Task.Run(() =>
            {
                var isSuccess = RunCommandProcessWait($"tar -xf {OPENSSL_EXE_UNZIP_PATH}/OpenSSL-Win64.zip -C {OPENSSL_EXE_UNZIP_PATH}", out var output);

                if (isSuccess)
                {
                    isHaveOpenSslZIP = true;
                    return string.Empty;
                }
                return output;
            });
        }

        /// <summary>
        /// 异步生成证书
        /// </summary>
        /// <returns>错误信息（没有代表成功）</returns>
        private static async Task<string> CreateCertificate(string command, string[] generateInfo)
        {
            // 异步执行
            return await Task.Run(() =>
            {
                if (isInstallOpenssl)
                {
                    command = "openssl" + command;
                }
                else if (isHaveOpenSslZIP)
                {
                    command = $"{OPENSSL_EXE_LOCAL_PATH}" + command;
                }
                var isSuccess = RunCommandProcessWait(command, out var output);
                if (isSuccess)
                {
                    // 目标文件路径
                    string CertificateTxtPath = GenerateCertificatePath + "/证书.txt";
                    try
                    {
                        // 使用 StreamWriter 写入文件
                        using (StreamWriter writer = new StreamWriter(CertificateTxtPath))
                        {
                            foreach (string line in generateInfo)
                            {
                                writer.WriteLine(line);
                            }
                        }
                        Debug.LogError("证书写入成功: " + CertificateTxtPath);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError("证书写入异常: " + ex.Message);
                    }
                    return string.Empty;
                }
                return output;
            });
        }


        /// <summary>
        /// 是否为有效的版本号
        /// </summary>
        /// <param name="version">例如 1.0.0 或 1.1.0-beta.1</param>
        /// <returns></returns>
        private static bool IsValidVersion(string version)
        {
            var beta = int.MaxValue;
            return GetVersion(version, out _, out _, out _, ref beta);
        }

        /// <summary>
        /// 获取版本号
        /// </summary>
        /// <param name="version">例如 1.0.0 或 1.1.0-beta.1</param>
        /// <param name="major"></param>
        /// <param name="minor"></param>
        /// <param name="patch"></param>
        /// <param name="beta"></param>
        /// <returns></returns>
        private static bool GetVersion(string version, out int major, out int minor, out int patch, ref int beta)
        {
#if NET_UNITY_4_8
            var betaSplit = version.Split("-beta.");
#else
            var betaSplit = version.Split('-');
            const string BETA_SIGN = "beta.";
            if (betaSplit.Length > 1 && betaSplit[1].StartsWith(BETA_SIGN))
            {
                betaSplit[1] = betaSplit[1].Substring(BETA_SIGN.Length);
            }
#endif
            if (betaSplit.Length > 1 && int.TryParse(betaSplit[1], out beta))
            {
                version = betaSplit[0];
            }
            var versionSplit = version.Split('.');
            if (!int.TryParse(versionSplit[0], out major))
            {
                minor = patch = 0;
                return false;
            }
            var splitLen = versionSplit.Length;
            if (splitLen > 1)
            {
                if (!int.TryParse(versionSplit[1], out minor))
                {
                    patch = 0;
                    return false;
                }
                if (splitLen > 2)
                {
                    if (!int.TryParse(versionSplit[2], out patch))
                    {
                        return false;
                    }
                    return true;
                }
                patch = 0;
                return true;
            }
            minor = patch = 0;
            return true;
        }

        /// <summary>
        /// 比较版本
        /// 负数代表版本低，0代表版本相等，1代表版本高
        /// </summary>
        /// <param name="version1"></param>
        /// <param name="version2"></param>
        /// <returns></returns>
        private static int? CompareVersion(string version1, string version2)
        {
            var beta1 = int.MaxValue;
            var beta2 = int.MaxValue;
            if (!GetVersion(version1, out var major1, out var minor1, out var patch1, ref beta1)) return null;
            if (!GetVersion(version2, out var major2, out var minor2, out var patch2, ref beta2)) return null;
            if (major1 > major2) return 1;
            if (major1 < major2) return -1;
            if (minor1 > minor2) return 1;
            if (minor1 < minor2) return -1;
            if (patch1 > patch2) return 1;
            if (patch1 < patch2) return -1;
            if (beta1 > beta2) return 1;
            if (beta1 < beta2) return -1;
            return 0;
        }

        private static Vector2 GetLabelSize(string text) => EditorStyles.label.CalcSize(new GUIContent(text));
        private static Vector2 GetLabelSize(GUIContent content) => EditorStyles.label.CalcSize(content);

        private static void AddCursorRect(MouseCursor cursor)
        {
            EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), cursor);
        }

        private static void MiniLinkButton(string text, Action onClick = null)
        {
            if (GUILayout.Button(text, new GUIStyle(EditorUtil.linkLabel)
            {
                fontSize = EditorStyles.miniFont.fontSize,
                alignment = TextAnchor.LowerLeft
            }))
            {
                onClick?.Invoke();
            }
            AddCursorRect(MouseCursor.Link);
        }

        private static void SaveConfigAsset()
        {
            BuildConfigAsset.Save();
        }

        private void OnGUI()
        {
            // 绘制界面
            GUILayout.BeginVertical(new GUIStyle()
            {
                margin = new RectOffset(0, 4, 0, 4)
            });
            configPanelScrollPosition = EditorGUILayout.BeginScrollView(configPanelScrollPosition);
            Separator(2);
            DrawFundamentals();
            Separator();
            DrawAssetCache();
            Separator();
            DrawAssetCheck();
            EditorGUILayout.EndScrollView();
            GUILayout.FlexibleSpace();
            DrawBuildActionArea();
            GUILayout.EndVertical();
            Separator();
            DrawSDKVersionInfo();
            DrawQGBuildToolVersionInfo();
            Separator();
            // 处理键鼠输入相关逻辑
            HandleHardwareInput();
        }

        private void Update()
        {
            BuildExecutor();
        }

        private void OnDestroy()
        {
            // 清理数据
            if (qgBuildToolVersionProcess != null)
            {
                qgBuildToolVersionProcess.Kill();
                qgBuildToolVersionProcess = null;
            }
            currentBuildToolVersion = null;
            requestQGBuildToolVersionStatus = RequestVersionStatus.InProgress;
            instance = null;
            // 注销回调
            AssetDatabase.importPackageCompleted -= OnImportPackageCompleted;
            AssetDatabase.importPackageFailed -= OnImportPackageFailed;
        }

        private async Task WaitWebRequestComplete(UnityWebRequest uwr)
        {
            while (!uwr.isDone)
            {
                await Task.Yield();
            }
        }

        /// <summary>
        /// 异步获取当前打包工具版本号
        /// </summary>
        /// <returns>错误信息（没有代表成功）</returns>
        private async Task<string> RequestQGBuildToolVersionAsync()
        {
            // 异步执行
            return await Task.Run(() =>
            {
                // 打开控制台进程获取版本号
                var isSuccess = RunCommandProcessWait("quickgame -V", out var output);
                // 成功缓存版本号，返回无错误
                if (isSuccess)
                {
                    currentBuildToolVersion = output.TrimEnd('\n');
                    return string.Empty;
                }
                // 失败返回错误
                return output;
            });
        }

        /// <summary>
        /// 异步获取打包工具最新版本号
        /// </summary>
        /// <returns>错误信息（没有代表成功）</returns>
        private async Task<string> RequestQGBuildToolLatestVersionAsync()
        {
            // 异步执行
            return await Task.Run(() =>
            {
                // 打开控制台进程获取所有版本号
                var isSuccess = RunCommandProcessWait("npm show @oppo-minigame/cli versions", out var output);
                // 成功后进一步解析
                if (isSuccess)
                {
                    // 缓存最新版本号
                    var versions = output.Replace("\n", string.Empty).Replace(" ", string.Empty).TrimStart('[').TrimEnd(']').Split(',');
                    latestBuildToolVersion = versions[versions.Length - 1].TrimStart('\'').TrimEnd('\'');
                    // 返回成功
                    return string.Empty;
                }
                // 失败返回错误
                return output;
            });
        }

        /// <summary>
        /// 异步升级打包工具到最新版本
        /// </summary>
        /// <returns>错误信息（没有代表成功）</returns>
        private async Task<string> UpgradeQGBuildToolLatestVersionAsync()
        {
            // 异步执行
            return await Task.Run(() =>
            {
                // 打开控制台进程执行升级命令
                var isSuccess = RunCommandProcessWait($"npm install @oppo-minigame/cli@{latestBuildToolVersion} -g", out var output);
                // 返回成功
                if (isSuccess) return string.Empty;
                // 失败返回错误
                return output;
            });
        }

        /// <summary>
        /// 异步获取最新 SDK 版本
        /// </summary>
        /// <returns>错误信息（没有代表成功）</returns>
        private async Task<string> RequestLatestSDKVersionAsync()
        {
            // 发送网络请求版本号
            var uwr = UnityWebRequest.Get($"{SDK_SERVER_URL}/version");
            uwr.SendWebRequest();
            // 等待网络请求完成
            await WaitWebRequestComplete(uwr);
            // 当前请求成功
            var error = uwr.error;
            var isSuccess = !error.IsValid();
            if (isSuccess)
            {
                // 版本信息解析成功则进行缓存
                var versionText = (uwr.downloadHandler as DownloadHandlerBuffer).text;
                var versionInfo = versionText.Split('\n');
                if (versionInfo.Length > 2 && versionInfo[0].IsValid() && IsValidVersion(versionInfo[0]))
                {
                    latestSDKVersion = versionInfo[0].Trim();
                    latestSDKPackageName = versionInfo[1].Trim();
                    latestSDKPackageMD5 = versionInfo[2].Trim();
                }
                // 版本信息解析失败则此次请求失败
                else
                {
                    isSuccess = false;
                    error = "Parse version failed";
                }
            }
            if (!isSuccess)
            {
                latestSDKVersion = latestSDKPackageName = string.Empty;
                // 输出错误信息到控制台
                Debug.LogError(error);
            }
            // 返回结果
            return isSuccess ? string.Empty : error;
        }

        /// <summary>
        /// 控制台输出处理器
        /// </summary>
        /// <param name="output"></param>
        /// <returns>是否成功</returns>
        private bool CommandOutputHandler(string output)
        {
            if (!output.IsValid()) return true;
            EditorUtility.DisplayDialog("提示", $"出错了，请根据控制台错误，查阅技术文档或联系技术客服", "确认");
            Debug.LogError(output);
            EditorUtility.ClearProgressBar();
            return false;
        }

        /// <summary>
        /// 异步更新打包工具版本状态
        /// </summary>
        private async void UpdateQGBuildToolVersionStatusAsync()
        {
            // 异步获取版本号信息
            requestQGBuildToolVersionStatus = RequestVersionStatus.InProgress;
            var error = await RequestQGBuildToolVersionAsync();
            // 获取当前版本失败，中断退出
            if (error.IsValid())
            {
                requestQGBuildToolVersionStatus = RequestVersionStatus.Fail;
                Repaint();
                return;
            }
            error = await RequestQGBuildToolLatestVersionAsync();
            // 获取最新版本失败，中断退出
            if (error.IsValid())
            {
                requestQGBuildToolVersionStatus = RequestVersionStatus.Fail;
                Repaint();
                return;
            }
            // 设置获取成功
            requestQGBuildToolVersionStatus = RequestVersionStatus.Success;
            Repaint();
        }

        /// <summary>
        /// 异步更新 SDK 版本状态
        /// </summary>
        private async void UpdateSDKVersionStatusAsync()
        {
            // 异步获取最新版本信息
            requestSDKVersionStatus = RequestVersionStatus.InProgress;
            var error = await RequestLatestSDKVersionAsync();
            // 获取最新版本失败，中断退出
            if (error.IsValid())
            {
                requestSDKVersionStatus = RequestVersionStatus.Fail;
                Repaint();
                return;
            }
            // 设置获取成功
            requestSDKVersionStatus = RequestVersionStatus.Success;
            // 状态更新后立即重绘，否则要等很久
            Repaint();
        }

        private void UpdateAssetCacheSystemAvailability()
        {
            if (currentBuildToolVersion.IsValid())
            {
                var versionCompare = CompareVersion(currentBuildToolVersion, ASSET_CACHE_SYSTEM_MIN_VERSION);
                BuildConfigAsset.AssetCache.available = versionCompare.HasValue && versionCompare >= 0;
            }
            else
            {
                BuildConfigAsset.AssetCache.available = false;
            }
        }

        private void OnWindowOpen()
        {
            // 立即更新版本状态
            UpdateQGBuildToolVersionStatusAsync();
            UpdateSDKVersionStatusAsync();
            // 注册回调
            AssetDatabase.importPackageCompleted += OnImportPackageCompleted;
            AssetDatabase.importPackageFailed += OnImportPackageFailed;
        }

        private bool Foldout(string title, Action contentBuilder = null, int indentLevelPlus = 2, bool initialState = true)
        {
            var foldout = EditorUtil.Foldout(title, initialState, GlobalDefines.BUILD_TOOL_MODULE);
            if (foldout && contentBuilder != null)
            {
                EditorGUI.indentLevel += indentLevelPlus;
                contentBuilder();
                EditorGUI.indentLevel -= indentLevelPlus;
            }
            return foldout;
        }

        private void Separator(int num = 1)
        {
            for (var i = 0; i < num; i++)
            {
                EditorGUILayout.Separator();
            }
        }

        private string StringNoEmpty(string target, string valueIfEmpty) => !target.IsValid() ? valueIfEmpty : target;

        private void ClearTextFieldFocus()
        {
            EditorGUI.FocusTextInControl(null);
        }

        private void MiniLabelField(string text, Color color, bool widthFit = false, TextAnchor alignment = TextAnchor.LowerRight)
        {
            var style = new GUIStyle()
            {
                normal = new GUIStyleState()
                {
                    textColor = color
                },
                alignment = alignment,
                fontSize = EditorStyles.miniFont.fontSize
            };
            if (widthFit)
            {
                EditorGUILayout.LabelField(text, style, GUILayout.Width(GetLabelSize(text).x));
            }
            else
            {
                EditorGUILayout.LabelField(text, style);
            }
        }

        private void RefreshButton(Action onClick)
        {
            if (GUILayout.Button(EditorGUIUtility.IconContent("Refresh"), new GUIStyle(EditorUtil.linkLabel)))
            {
                onClick?.Invoke();
            }
            AddCursorRect(MouseCursor.Link);
        }

        private void ValidateLabel(string text, int indentPlus = -1)
        {
            if (!text.IsValid()) return;
            EditorGUI.indentLevel += indentPlus;
            EditorGUILayout.LabelField(text, new GUIStyle(EditorStyles.miniLabel)
            {
                normal = new GUIStyleState()
                {
                    textColor = Color.red,
                },
                padding = new RectOffset((int)EditorGUIUtility.labelWidth - 12, 0, 0, 0),
            });
            EditorGUI.indentLevel -= indentPlus;
        }

        private string ValidationTextField(ref string target, string key, string label, string text, bool allowEmpty, string emptyText = "不能为空", string validatePattern = "", string validateText = "校验失败", Func<bool> validateFunc = null, bool showErrorGUI = true)
        {
            return ValidationTextField(ref target, key, new GUIContent(label), text, allowEmpty, emptyText, validatePattern, validateText, validateFunc, showErrorGUI);
        }

        private string ValidationTextField(ref string target, string key, GUIContent label, string text, bool allowEmpty, string emptyText = "不能为空", string validatePattern = "", string validateText = "校验失败", Func<bool> validateFunc = null, bool showErrorGUI = true)
        {
            // 记录需要校验的属性，默认校验通过
            if (!validationMap.ContainsKey(key))
            {
                validationMap.Add(key, string.Empty);
            }
            // 输入变化时做校验
            EditorGUI.BeginChangeCheck();
            target = EditorGUILayout.TextField(label, text);
            var hasChange = EditorGUI.EndChangeCheck();
            // 输入发生改变则记录需要存脏数据
            if (hasChange)
            {
                RecordDirty();
            }
            // 若强制触发1次校验，则在校验后标记已完成
            if (hasChange || validateForOnceKeyList.Contains(key))
            {
                ValidateTextProperty(target, key, allowEmpty, emptyText, validatePattern, validateText, validateFunc);
                validateForOnceKeyList.Remove(key);
            }
            // 校验失败且需要立即显示错误提示
            if (validationMap.TryGetValue(key, out var errorText) && errorText.IsValid() && showErrorGUI)
            {
                ValidateLabel(errorText);
            }
            return errorText;
        }

        private string ValidateTextProperty(string target, string key, bool allowEmpty, string emptyText = "不能为空", string validatePattern = "", string validateText = "校验失败", Func<bool> validateFunc = null)
        {
            // 校验输入为空
            if (!target.IsValid() && !allowEmpty)
            {
                validationMap[key] = emptyText;
            }
            // 校验传入的方法
            else if (validateFunc != null)
            {
                validationMap[key] = validateFunc() ? string.Empty : validateText;
            }
            // 校验正则表达式
            else if (target.IsValid() && validatePattern.IsValid())
            {
                validationMap[key] = Regex.IsMatch(target, validatePattern) ? string.Empty : validateText;
            }
            // 其他情况都校验通过
            else
            {
                validationMap[key] = string.Empty;
            }
            return validationMap[key];
        }

        private void ValidateTextFieldForOnce(string key)
        {
            validateForOnceKeyList.Add(key);
        }

        private void ValidateAllTextFieldForOnce()
        {
            var keys = validationMap.Keys;
            foreach (var key in keys)
            {
                ValidateTextFieldForOnce(key);
            }
        }

        private void DirtySaveField(Action fieldBuilder)
        {
            EditorGUI.BeginChangeCheck();
            fieldBuilder();
            if (EditorGUI.EndChangeCheck())
            {
                SaveConfigAsset();
            }
        }

        private void SaveProperty(ref string current, string value)
        {
            // 判断是否发生改变
            var diff = current != value;
            // 记录当前值
            current = value;
            // 发生改变则立即存储
            if (diff)
            {
                SaveConfigAsset();
            }
        }

        private bool IsAllValidatePass
        {
            get
            {
                var validateValues = validationMap.Values;
                foreach (var value in validateValues)
                {
                    // 任意1项有错误内容，则验证失败
                    if (value.IsValid()) return false;
                }
                // 全部验证通过
                return true;
            }
        }

        /// <summary>
        /// 画上一个 UI 的框
        /// @note: 此方法用于调试，别删除
        /// </summary>
        private void DrawLastRect()
        {
            EditorUtil.DrawBackgroundRect(GUILayoutUtility.GetLastRect(), new Color(0f, 0f, 0f, 0f), Color.red);
        }

        private void DrawFundamentals()
        {
            Foldout("基本设置", () =>
            {
                var fundamentals = BuildConfigAsset.Fundamentals;
                void DrawIconSelector()
                {
                    GUILayout.BeginHorizontal();
                    var errorText = ValidationTextField(
                        target: ref fundamentals.iconPath,
                        key: nameof(fundamentals.iconPath),
                        label: "游戏图标",
                        text: fundamentals.iconPath,
                        allowEmpty: false,
                        emptyText: "请选择游戏图标路径",
                        validatePattern: string.Empty,
                        validateText: "图标不存在，请重新选择",
                        validateFunc: () => { return File.Exists(fundamentals.iconPath); },
                        showErrorGUI: false);
                    if (GUILayout.Button("选择", GUILayout.Width(64f)))
                    {
                        ClearTextFieldFocus();
                        var iconPath = EditorUtility.OpenFilePanel("选择游戏图标", "", "png,jpg,jpeg");
                        if (iconPath.IsValid())
                        {
                            SaveProperty(ref fundamentals.iconPath, iconPath);
                            // 选择了即有效，直接修改结果
                            validationMap[nameof(fundamentals.iconPath)] = string.Empty;
                        }
                        GUIUtility.ExitGUI();
                    }
                    GUILayout.EndHorizontal();
                    ValidateLabel(errorText);
                }
                void DrawCertificateSelector()
                {
                    var useCustomSign = false;
                    DirtySaveField(() =>
                    {
                        useCustomSign = EditorGUILayout.Toggle(new GUIContent("自定义密钥库", "是否使用自己生成的certificate.pem证书，和private.pem私钥"), fundamentals.useCustomSign);
                    });
                    GUI.enabled = useCustomSign;
                    ++EditorGUI.indentLevel;
                    // 证书
                    GUILayout.BeginHorizontal();
                    var certificateErrorText = ValidationTextField(
                        target: ref fundamentals.signCertificate,
                        key: nameof(fundamentals.signCertificate),
                        label: "证书文件路径",
                        text: fundamentals.signCertificate,
                        allowEmpty: !useCustomSign,
                        emptyText: "请选择证书路径",
                        validatePattern: string.Empty,
                        validateText: "证书不存在，请重新选择",
                        validateFunc: () => { return useCustomSign ? File.Exists(fundamentals.signCertificate) : true; },
                        showErrorGUI: false);
                    if (GUILayout.Button("选择", GUILayout.Width(64f)))
                    {
                        ClearTextFieldFocus();
                        var certificatePath = EditorUtility.OpenFilePanel("选择证书文件", "", "pem");
                        if (certificatePath.IsValid())
                        {
                            SaveProperty(ref fundamentals.signCertificate, certificatePath);
                            // 选择了即有效，直接修改结果
                            validationMap[nameof(fundamentals.signCertificate)] = string.Empty;
                        }
                        GUIUtility.ExitGUI();
                    }

                    if (GUILayout.Button("新建", GUILayout.Width(64f)))
                    {
                        ClearTextFieldFocus();
                        GetWindow<OpensslPlugin>("生成签名文件");
                        GUIUtility.ExitGUI();
                    }
                    GUILayout.EndHorizontal();
                    ValidateLabel(certificateErrorText, -2);
                    // 私钥
                    GUILayout.BeginHorizontal();
                    var privateErrorText = ValidationTextField(
                        target: ref fundamentals.signPrivate,
                        key: nameof(fundamentals.signPrivate),
                        label: "私钥文件路径",
                        text: fundamentals.signPrivate,
                        allowEmpty: !useCustomSign,
                        emptyText: "请选择私钥路径",
                        validatePattern: string.Empty,
                        validateText: "私钥不存在，请重新选择",
                        validateFunc: () => { return useCustomSign ? File.Exists(fundamentals.signPrivate) : true; },
                        showErrorGUI: false);
                    if (GUILayout.Button("选择", GUILayout.Width(64f)))
                    {
                        ClearTextFieldFocus();
                        var privatePath = EditorUtility.OpenFilePanel("选择私钥文件", "", "pem");
                        if (privatePath.IsValid())
                        {
                            SaveProperty(ref fundamentals.signPrivate, privatePath);
                            // 选择了即有效，直接修改结果
                            validationMap[nameof(fundamentals.signPrivate)] = string.Empty;
                        }
                        GUIUtility.ExitGUI();
                    }
                    if (isHaveCertificatePath)
                    {
                        var certificatePaths = $"{GenerateCertificatePath}/certificate.pem";
                        var privatePaths = $"{GenerateCertificatePath}/private.pem";
                        SaveProperty(ref fundamentals.signCertificate, certificatePaths);
                        SaveProperty(ref fundamentals.signPrivate, privatePaths);
                        validationMap[nameof(fundamentals.signCertificate)] = string.Empty;
                        validationMap[nameof(fundamentals.signPrivate)] = string.Empty;
                        isHaveCertificatePath = false;
                    }

                    GUILayout.EndHorizontal();
                    ValidateLabel(privateErrorText, -2);
                    --EditorGUI.indentLevel;
                    GUI.enabled = true;
                    // 在使用/不使用自定义密钥库间发生改变
                    if (useCustomSign != fundamentals.useCustomSign)
                    {
                        if (useCustomSign)
                        {
                            // 使用的情况下立即进行一次校验
                            ValidateTextFieldForOnce(nameof(fundamentals.signCertificate));
                            ValidateTextFieldForOnce(nameof(fundamentals.signPrivate));
                        }
                        else
                        {
                            // 不使用的情况下清除校验
                            validationMap[nameof(fundamentals.signCertificate)] = string.Empty;
                            validationMap[nameof(fundamentals.signPrivate)] = string.Empty;
                        }
                    }
                    // 记录使用状态
                    fundamentals.useCustomSign = useCustomSign;
                }
                void DrawStreamingAssetsURLSelector()
                {
                    var useRemoteStreamingAssets = false;
                    DirtySaveField(() =>
                    {
                        useRemoteStreamingAssets = EditorGUILayout.Toggle(new GUIContent("使用远程资源", "是否将StreamingAssets放到服务器上"), fundamentals.useRemoteStreamingAssets);
                    });
                    GUI.enabled = fundamentals.useRemoteStreamingAssets;
                    ++EditorGUI.indentLevel;
                    var errorText = ValidationTextField(
                        target: ref fundamentals.streamingAssetsURL,
                        key: nameof(fundamentals.streamingAssetsURL),
                        label: new GUIContent("服务器地址", "例如http://localhost:8080/StreamingAssets"), fundamentals.streamingAssetsURL,
                        allowEmpty: !useRemoteStreamingAssets,
                        emptyText: "地址不能为空",
                        validatePattern: @"https?://[-A-Za-z0-9+&@#/%?=~_|!:,.;]+[-A-Za-z0-9+&@#/%=~_|]",
                        validateText: "需填写填写http://或https://开头的有效URL地址",
                        showErrorGUI: false);
                    ValidateLabel(errorText, -2);
                    --EditorGUI.indentLevel;
                    GUI.enabled = true;
                    // 在使用/不使用之间发生改变
                    if (fundamentals.useRemoteStreamingAssets != useRemoteStreamingAssets)
                    {
                        if (useRemoteStreamingAssets)
                        {
                            // 使用的情况下立即进行一次校验
                            ValidateTextFieldForOnce(nameof(fundamentals.streamingAssetsURL));
                        }
                        else
                        {
                            // 不使用的情况下清除校验
                            validationMap[nameof(fundamentals.streamingAssetsURL)] = string.Empty;
                        }
                    }
                    fundamentals.useRemoteStreamingAssets = useRemoteStreamingAssets;
                }
                void DrawExportPathSelector()
                {
                    bool isExportPathExist() => Directory.Exists(fundamentals.exportPath);
                    GUILayout.BeginHorizontal();
                    var errorText = ValidationTextField(
                        target: ref fundamentals.exportPath,
                        key: nameof(fundamentals.exportPath),
                        label: new GUIContent("导出路径", "导出的WebGL和小游戏工程根目录"),
                        text: fundamentals.exportPath,
                        allowEmpty: false,
                        emptyText: "请选择导出路径",
                        validatePattern: string.Empty,
                        validateText: "路径不存在，请重新选择",
                        validateFunc: () => { return isExportPathExist(); },
                        showErrorGUI: false);
                    GUI.enabled = fundamentals.exportPath.IsValid() && isExportPathExist();
                    if (GUILayout.Button("打开", GUILayout.Width(64f)))
                    {
                        ClearTextFieldFocus();
                        QGGameTools.ShowInExplorer(fundamentals.exportPath);
                    }
                    GUI.enabled = true;
                    if (GUILayout.Button("选择", GUILayout.Width(64f)))
                    {
                        ClearTextFieldFocus();
                        var exportPath = EditorUtility.SaveFolderPanel("选择导出目录", string.Empty, "Build");
                        if (exportPath.IsValid())
                        {
                            SaveProperty(ref fundamentals.exportPath, exportPath);
                            // 选择了即有效，直接修改结果
                            validationMap[nameof(fundamentals.exportPath)] = string.Empty;
                        }
                        GUIUtility.ExitGUI();
                    }
                    GUILayout.EndHorizontal();
                    ValidateLabel(errorText);
                }
                // 游戏包名
                ValidationTextField(
                    ref fundamentals.packageName,
                    nameof(fundamentals.packageName),
                    label: new GUIContent("游戏包名", "默认为 com.[PlayerSettings.companyName].[PlayerSettings.productName]"),
                    StringNoEmpty(fundamentals.packageName, $"com.{PlayerSettings.companyName}.{PlayerSettings.productName}"),
                    true,
                    string.Empty,
                    @"^[A-Za-z][A-Za-z0-9]*(\.[A-Za-z][A-Za-z0-9]*)*$",
                    "需以英文句号分隔，每一段以字母开头，且只能包含字母和数字");
                // 游戏名称
                ValidationTextField(
                    target: ref fundamentals.projectName,
                    key: nameof(fundamentals.projectName),
                    label: "游戏名称",
                    text: StringNoEmpty(fundamentals.projectName, "我的游戏"),
                    allowEmpty: true);
                DirtySaveField(() =>
                {
                    // 游戏方向
                    fundamentals.orientation = EditorGUILayout.IntPopup("游戏方向", fundamentals.orientation, new[] { "Portrait", "Landscape", "LandscapeLeft", "LandscapeRight" }, new[] { 0, 1, 2, 3 });
                });
                // 游戏版本号
                DirtySaveField(() =>
                {
                    fundamentals.projectVersion = EditorGUILayout.IntField(
                        new GUIContent("游戏版本号", "整数，一般从1开始"),
                        fundamentals.projectVersion < BuildFundamentalConfig.MIN_PROJECT_VERSION ? BuildFundamentalConfig.MIN_PROJECT_VERSION : fundamentals.projectVersion);
                });
                // 游戏版本名称
                ValidationTextField(
                    target: ref fundamentals.projectVersionName,
                    key: nameof(fundamentals.projectVersionName),
                    label: new GUIContent("游戏版本名称", "一般与版本号相匹配，例如版本号为1，则版本名称为1.0.0"),
                    text: StringNoEmpty(fundamentals.projectVersionName, "1.0.0"),
                    allowEmpty: true,
                    emptyText: string.Empty,
                    validatePattern: @"^[0-9]+(\.[0-9]+)*$",
                    validateText: "需以英文句号分隔，每一段只能包含数字");
                DirtySaveField(() =>
                {
                    fundamentals.minPlatformVersion = EditorGUILayout.IntField(
                        new GUIContent("最小平台版本号", "整数，一般最低填写1103"),
                        fundamentals.minPlatformVersion < BuildFundamentalConfig.MIN_PLATFORM_VERSION ? BuildFundamentalConfig.MIN_PLATFORM_VERSION : fundamentals.minPlatformVersion);
                });
                DrawIconSelector();
                DrawCertificateSelector();
                DrawStreamingAssetsURLSelector();
                DrawExportPathSelector();
            });
        }

        private void DrawAssetCache()
        {
            Foldout("缓存设置", () =>
            {
                // 提示缓存系统依赖库版本过低
                UpdateAssetCacheSystemAvailability();
                var isSystemAvailable = BuildConfigAsset.AssetCache.available;
                if (!isSystemAvailable && requestQGBuildToolVersionStatus != RequestVersionStatus.InProgress)
                {
                    GUILayout.BeginHorizontal();
                    var text = $"小游戏打包工具版本不符，缓存系统不可用，请升级至 {ASSET_CACHE_SYSTEM_MIN_VERSION} 及以上版本";
                    EditorGUILayout.LabelField(text, new GUIStyle()
                    {
                        normal = new GUIStyleState()
                        {
                            textColor = Color.red
                        },
                        fontSize = EditorStyles.miniFont.fontSize,
                        alignment = TextAnchor.LowerLeft
                    }, GUILayout.Width(GetLabelSize(text).x));
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                }
                GUI.enabled = isSystemAvailable;
                var assetCache = BuildConfigAsset.AssetCache;
                DirtySaveField(() =>
                {
                    assetCache.enableBundleCache = EditorGUILayout.Toggle("启用缓存", assetCache.enableBundleCache);
                });
                GUI.enabled &= assetCache.enableBundleCache;
                ValidationTextField(
                    target: ref assetCache.bundlePathIdentifier,
                    key: nameof(assetCache.bundlePathIdentifier),
                    label: new GUIContent("缓存路径标识", "不填写代表所有路径都进行缓存判断。填写时多个时使用英文分号分隔，例如 StreamingAssets;bundles"),
                    text: assetCache.bundlePathIdentifier,
                    allowEmpty: true,
                    emptyText: string.Empty,
                    validatePattern: @"^\w+(;\w+)*$",
                    validateText: "路径只能使用字母、数字、下划线，多个路径之间用英文分号分隔");
                ValidationTextField(
                    target: ref assetCache.excludeFileExtensions,
                    key: nameof(assetCache.excludeFileExtensions),
                    label: new GUIContent("不缓存的文件类型", "不填写代表所有文件都进行缓存判断。填写多个时使用英文分号分隔，例如 .json;.hash"),
                    text: assetCache.excludeFileExtensions,
                    allowEmpty: true,
                    emptyText: string.Empty,
                    validatePattern: @"^\.[A-Za-z0-9]+(;\.[A-Za-z0-9]+)*$",
                    validateText: "类型必须以英文句号开头，名称只能使用英文和数字，多个路径之间用英文分号分隔");
                DirtySaveField(() =>
                {
                    assetCache.bundleHashLength = EditorGUILayout.IntPopup(
                    new GUIContent("哈希长度", "资源hash占多少长度，默认32位"),
                    assetCache.bundleHashLength,
                    new GUIContent[] { new GUIContent("32位"), new GUIContent("64位"), new GUIContent("128位") },
                    new[] { 32, 64, 128 });
                });
                DirtySaveField(() =>
                {
                    assetCache.defaultReleaseSize = EditorGUILayout.IntPopup(
                    new GUIContent("额外清理大小", "清理缓存时默认额外清理的大小，单位MB，默认30MB"),
                    assetCache.defaultReleaseSize,
                    new GUIContent[] { new GUIContent("30MB"), new GUIContent("60MB"), new GUIContent("120MB"), new GUIContent("160MB") },
                    new[] { 30, 60, 120, 160 });
                });
                DirtySaveField(() =>
                {
                    assetCache.keepOldVersion = EditorGUILayout.Toggle(new GUIContent("保留旧版本", "资源更新后是否保留旧版本资源，默认删除不保留"), assetCache.keepOldVersion);
                });
                ValidationTextField(
                    target: ref assetCache.excludeClearFiles,
                    key: nameof(assetCache.excludeClearFiles),
                    label: new GUIContent("清理忽略文件", "自动清理时忽略的文件，支持纯hash或名称，名称尽量不要使用特殊字符。不填写代表所有缓存都有可能被清理。填写多个时使用英文分号分隔，例如 8d265a9dfd6cb7669cdb8b726f0afb1e;asset1"),
                    text: assetCache.excludeClearFiles,
                    allowEmpty: true,
                    emptyText: string.Empty,
                    validatePattern: @"^[\w!@#\$%\^&\(\)\-=\+\[\]\{\}',\.`~]+(;[\w!@#\$%\^&\(\)\-=\+\[\]\{\}',\.`~]+)*$",
                    validateText: "不支持/?<>\\:*|\"等特殊字符，多个文件之间用英文分号分隔");
                DirtySaveField(() =>
                {
                    assetCache.enableCacheLog = EditorGUILayout.Toggle(new GUIContent("开启日志", "是否将缓存信息输出到控制台，便于调试"), assetCache.enableCacheLog);
                });
                GUI.enabled = true;
            });
        }

        private bool isDownLoadShaderScenes = false;
        private void DrawAssetCheck()
        {
            Foldout("资源检查", () =>
            {
                DirtySaveField(() =>
                {
                    checkShaderOnBuild = EditorGUILayout.Toggle("检查Shader兼容性", checkShaderOnBuild);
                });
                DirtySaveField(() =>
                {
                    useRuntimeShaderDetection = EditorGUILayout.Toggle(new GUIContent("启用Shader真机检测", "仅供自测，正式打包请勿使用"), useRuntimeShaderDetection);

                    if (useRuntimeShaderDetection && ShaderRuntimeDetector.checkAddShaderSence())
                    {
                        string checkShaderSence = ShaderRuntimeDetector.checkLocalShaderSence() ? "导入" : "下载";
                        if (EditorUtility.DisplayDialog("提示", $"{"需要"}{checkShaderSence}{"Shader场景包"}", checkShaderSence, "取消"))
                        {
                            ShaderRuntimeDetector.StartDownload();
                            isDownLoadShaderScenes = true;
                        }
                        useRuntimeShaderDetection = false;
                    }
                    if (isDownLoadShaderScenes && !ShaderRuntimeDetector.checkAddShaderSence())
                    {
                        isDownLoadShaderScenes = false;
                        useRuntimeShaderDetection = true;
                    }
                });
            });
        }

        private void DrawBuildActionArea()
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (EditorUtil.LinkButton("查看小游戏打包文档"))
            {
                Application.OpenURL("https://ie-activity-cn.heytapimage.com/static/minigame/CN/docs/index.html#/develop/games/quickgame");
                GUIUtility.ExitGUI();
            }
            GUILayout.EndHorizontal();
            EditorUtil.Space(4f);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            var sdkVersionReady = requestSDKVersionStatus == RequestVersionStatus.Success;
            var qgBuildToolVersionReady = requestQGBuildToolVersionStatus == RequestVersionStatus.Success;
            GUI.enabled = qgBuildToolVersionReady && sdkVersionReady && !isUpgradingSDK && !isUpgradingQGBuildTool;
            if (GUILayout.Button("打包", GUILayout.Width(100f)))
            {
                StartBuild();
                ClearTextFieldFocus();
                GUIUtility.ExitGUI();
            }
            GUI.enabled = true;
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        private void DrawSDKVersionInfo()
        {
            void CheckLatestVersionButton()
            {
                MiniLinkButton("检查更新", UpdateSDKVersionStatusAsync);
            }
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            MiniLabelField($"SDK 版本: {VersionInfo.VERSION}", Color.green, false);
            switch (requestSDKVersionStatus)
            {
                case RequestVersionStatus.InProgress:
                    MiniLabelField("正在检测 SDK 版本更新...", Color.red, true);
                    break;
                case RequestVersionStatus.Success:
                    EditorGUILayout.BeginHorizontal();
                    CheckLatestVersionButton();
                    var versionCompare = CompareVersion(VersionInfo.VERSION, latestSDKVersion);
                    if (versionCompare.HasValue && versionCompare < 0)
                    {
                        MiniLinkButton("升级版本", () =>
                        {
                            SDKUpdateProcess();
                        });
                    }
                    else
                    {
                        MiniLabelField("已是最新版本", Color.gray, true);
                    }
                    EditorGUILayout.EndHorizontal();
                    break;
                case RequestVersionStatus.Fail:
                    EditorGUILayout.BeginHorizontal();
                    CheckLatestVersionButton();
                    MiniLabelField("更新获取失败", Color.red, true);
                    EditorGUILayout.EndHorizontal();
                    break;
            }
            EditorGUILayout.EndHorizontal();
        }

        private async void SDKUpdateProcess()
        {
            // 弹窗让用户确认升级
            var option = EditorUtility.DisplayDialogComplex("提示", $"是否确认安装SDK最新版本 {latestSDKVersion}", "确认", "取消", "查看更新日志");
            switch (option)
            {
                case 0:
                    if (isUpgradingSDK) return;
                    isUpgradingSDK = true;
                    var progressDialogTitle = $"安装 SDK V{latestSDKVersion}";
                    void ContactUsDialog(string message)
                    {
                        if (!EditorUtility.DisplayDialog("提示", message, "查看联系方式", "确认")) return;
                        Application.OpenURL(CONTACT_US_URL);
                    }
                    // 没有缓存的安装包则先进行下载
                    if (!File.Exists(LatestSDKPackageTempPath))
                    {
                        // 下载版本包
                        void DisplayDownloadProgress(float progress)
                        {
                            EditorUtility.DisplayProgressBar(progressDialogTitle, "正在下载...", progress);
                        }
                        var uwr = UnityWebRequest.Get(SDK_SERVER_URL + $"/{latestSDKPackageName}");
                        uwr.SendWebRequest();
                        while (!uwr.isDone)
                        {
                            // 实时更新下载进度
                            DisplayDownloadProgress(uwr.downloadProgress);
                            await Task.Yield();
                        }
                        // 下载失败报错并中断后续流程
                        if (uwr.error.IsValid())
                        {
                            EditorUtility.ClearProgressBar();
                            ContactUsDialog("下载出错，请根据控制台错误输出检查您的环境，或直接联系我们");
                            Debug.LogError(uwr.error);
                            isUpgradingSDK = false;
                            return;
                        }
                        // 下载成功校验数据完整性
                        var packageData = (uwr.downloadHandler as DownloadHandlerBuffer).data;
                        // 校验失败报错并中断后续流程
                        if (packageData.MD5() != latestSDKPackageMD5)
                        {
                            EditorUtility.ClearProgressBar();
                            ContactUsDialog("安装包内容错误或丢失，请联系我们");
                            isUpgradingSDK = false;
                            return;
                        }
                        // 将安装包存储到临时目录
                        try
                        {
                            File.WriteAllBytes(LatestSDKPackageTempPath, packageData);
                        }
                        // 写入文件异常报错并中断后续流程
                        catch (Exception e)
                        {
                            EditorUtility.ClearProgressBar();
                            ContactUsDialog("安装包解析失败，请根据控制台错误输出检查您的环境后重试。若仍无法解决，请直接联系我们");
                            Debug.LogError(e.Message);
                            isUpgradingSDK = false;
                            return;
                        }
                        EditorUtility.ClearProgressBar();
                    }
                    // 导入安装包
                    EditorPrefs.SetInt(ImportingClass, (int)ImportStatus.Importimg);
                    AssetDatabase.ImportPackage(LatestSDKPackageTempPath, false);
                    isUpgradingSDK = false;
                    // 导入超时处理
                    ImportTiming();
                    break;
                case 1:
                    break;
                case 2:
                    Application.OpenURL(UPDATE_LOG_URL);
                    break;
            }
        }

        private void DrawQGBuildToolVersionInfo()
        {
            void RefreshVersionButton()
            {
                RefreshButton(UpdateQGBuildToolVersionStatusAsync);
            }
            void UpgradeButton()
            {
                MiniLinkButton("升级版本", async () =>
                {
                    if (!EditorUtility.DisplayDialog("提示", $"是否确认安装小游戏打包工具最新版本 {latestBuildToolVersion}", "确认", "取消"))
                    {
                        return;
                    }
                    if (isUpgradingQGBuildTool) return;
                    isUpgradingQGBuildTool = true;
                    var title = "安装小游戏打包工具";
                    EditorUtility.DisplayProgressBar(title, $"正在安装最新版本: {latestBuildToolVersion}", 0.2f);
                    var error = await UpgradeQGBuildToolLatestVersionAsync();
                    if (!CommandOutputHandler(error))
                    {
                        isUpgradingQGBuildTool = false;
                        return;
                    }
                    EditorUtility.DisplayProgressBar(title, "正在校验版本...", 0.5f);
                    error = await RequestQGBuildToolVersionAsync();
                    if (!CommandOutputHandler(error))
                    {
                        isUpgradingQGBuildTool = false;
                        return;
                    }
                    EditorUtility.ClearProgressBar();
                    if (currentBuildToolVersion == latestBuildToolVersion)
                    {
                        EditorUtility.DisplayDialog("提示", "升级成功", "确认");
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("提示", $"校验失败，当前版本 {currentBuildToolVersion} 与最新版本 {latestBuildToolVersion} 不同，请重新尝试安装", "确认");
                    }
                    isUpgradingQGBuildTool = false;
                });
            }
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            switch (requestQGBuildToolVersionStatus)
            {
                case RequestVersionStatus.InProgress:
                    MiniLabelField("正在检查打包工具版本...", Color.red);
                    break;
                case RequestVersionStatus.Success:
                    EditorGUILayout.BeginHorizontal();
                    MiniLabelField($"小游戏打包工具版本: {currentBuildToolVersion}", Color.green);
                    RefreshVersionButton();
                    var versionCompare = CompareVersion(currentBuildToolVersion, latestBuildToolVersion);
                    if (versionCompare.HasValue && versionCompare < 0)
                    {
                        UpgradeButton();
                    }
                    else
                    {
                        MiniLabelField("已是最新版本", Color.gray, true);
                    }
                    EditorGUILayout.EndHorizontal();
                    break;
                case RequestVersionStatus.Fail:
                    EditorGUILayout.BeginHorizontal();
                    MiniLabelField("获取打包工具版本失败，请安装后重试", Color.red);
                    RefreshVersionButton();
                    UpgradeButton();
                    EditorGUILayout.EndHorizontal();
                    break;
            }
            GUILayout.EndHorizontal();
        }

        private void HandleHardwareInput()
        {
            // 点击面板会触发此事件
            if (Event.current.type == EventType.MouseDown)
            {
                ClearTextFieldFocus();
                // 立即重绘，否则会出现状态滞留的问题
                Repaint();
            }
            // 输入框发生内容改变处理
            HandleInputFieldBlur();
        }

        private void HandleInputFieldBlur()
        {
            // 有脏数据，且此时通过回车键进行提交，则需要立即进行持久化
            if (hasAnyInputDirtyData)
            {
                // 回车键提交，或焦点移动到其他地方
                var isCommitOperation =
                    (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter) ||
                    (keyboardControlID != 0 && keyboardControlID != GUIUtility.keyboardControl);
                // 发生了提交操作则立即存储脏数据
                if (isCommitOperation)
                {
                    hasAnyInputDirtyData = false;
                    SaveConfigAsset();
                }
            }
            // 记录当前输入焦点id
            keyboardControlID = GUIUtility.keyboardControl;
        }

        private void RecordDirty()
        {
            if (hasAnyInputDirtyData) return;
            hasAnyInputDirtyData = true;
        }

        private void StartBuild()
        {
            if (isBuilding) return;
            // 标记立即做一次所有属性的验证
            ValidateAllTextFieldForOnce();
            // 开始构建
            isBuilding = true;
        }

        private void BuildExecutor()
        {
            // 等待发起构建命令
            if (!isBuilding) return;
            // 等待界面下1帧做校验
            if (buildValidateFrameCounter == 0)
            {
                ++buildValidateFrameCounter;
                return;
            }
            // 校验成功
            if (IsAllValidatePass)
            {
                // 先切换到 WebGL 平台
                var platformPass = EditorUserBuildSettings.activeBuildTarget == BuildTarget.WebGL;
                if (!platformPass)
                {
                    platformPass = EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.WebGL, BuildTarget.WebGL);
                }
                // 平台切换成功，继续执行打包流程
                if (platformPass)
                {
                    do
                    {
                        // 检查 Shader 兼容性
                        if (checkShaderOnBuild)
                        {
                            var continueBuild = false;
                            bool ContinueBuildHandler()
                            {
                                continueBuild = true;
                                return false;
                            }
                            ShaderStaticDetector.DoDetection(
                                new ResultBehaviour("完成检测，无异常", "继续打包", ContinueBuildHandler),
                                new ResultBehaviour("完成检测，存在异常，是否继续打包？", "确定", ContinueBuildHandler, "取消", () => true));
                            // 检查不通过且用户选择终止打包
                            if (!continueBuild) break;
                        }
                        // 若开启 Shader 真机检测，则准备好测试资源
                        EditorUtility.DisplayProgressBar("Shader真机测试", "正在构建测试资源...", 0.2f);
                        ShaderRuntimeDetector.SaveAssetsInTempFolder(useRuntimeShaderDetection);
                        EditorUtility.DisplayProgressBar("Shader真机测试", "正在构建测试场景...", 0.5f);
                        ShaderRuntimeDetector.AddShaderDetection(useRuntimeShaderDetection);
                        EditorUtility.ClearProgressBar();
                        // 获取导出路径
                        var exportPath = BuildConfigAsset.Fundamentals.exportPath;
                        var webGLExportPath = Path.Combine(exportPath, WEBGL_BUILD_DIR);
                        // 进行符合小游戏规范的项目设置
                        QGGameTools.SetPlayer();
                        // 获取当前是否使用WEBGL2.0
                        QGGameTools.GetUserWebGLVersion();
                        // 删除之前构建的目录
                        QGGameTools.DelectDir(webGLExportPath);
                        // 构建导出 WebGL 工程
                        var buildReport = QGGameTools.BuildWebGL(webGLExportPath);
                        if (buildReport.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
                        {
                            // 开始将 WebGL 转化为小游戏工程
                            QGGameTools.ConvetWebGL(exportPath, webGLExportPath);
                        }
                        else
                        {
                            ShowNotification(new GUIContent("构建 WebGL 失败，请根据控制台输出修复错误后，再重新打包"));
                        }
                    }
                    while (false);
                }
                // 平台切换失败
                else
                {
                    ShowNotification(new GUIContent("无法切换到 WebGL 平台，请检查游戏工程"));
                }
            }
            // 校验失败
            else
            {
                Debug.LogError("请根据提示修复打包配置后，再尝试打包");
            }
            // 构建完成
            isBuilding = false;
            ShaderRuntimeDetector.SaveAssetsInTempFolder(false);
            ShaderRuntimeDetector.AddShaderDetection(false);
        }

        private void OnImportPackageCompleted(string packageName)
        {
            if (packageName != latestSDKPackageName) return;
            Debug.Log($"{packageName} 安装成功");
        }

        private void OnImportPackageFailed(string packageName, string errorMessage)
        {
            if (packageName != latestSDKPackageName) return;
            if (EditorUtility.DisplayDialog("提示", "安装失败，请根据控制台错误输出检查您的环境后重试。若仍无法解决，请直接联系我们", "查看联系方式", "确认"))
            {
                Application.OpenURL(CONTACT_US_URL);
            }
            Debug.LogError(errorMessage);
        }

        public static void ImportTiming()
        {
            timer = new Timer(1000);
            timer.Elapsed += TimerElapsed;
            timer.AutoReset = true;
            timer.Start();
        }

        public static void StopImportTiming()
        {
            timer.Stop();
            timer.Elapsed -= TimerElapsed;
            timer.Dispose();
        }

        private static void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            ImportPackagTime++;
            // Debug.Log("监控导入 耗时:" + ImportPackagTime + ",是否编译完成: " + !EditorApplication.isCompiling);
            if (ImportPackagTime > ImportPackagOvertime || !EditorApplication.isCompiling) //超时 或 编译完成
            {
                EditorUtility.ClearProgressBar();   //关闭导入进度条
                ImportPackagTime = 0;
                StopImportTiming();
            }
        }

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptReload()
        {
            bool isBuildEditorWindowRunning = false;
#if UNITY_2019_1_OR_NEWER
        // Use the HasOpenInstances method if available (Unity 2019 and later)
        isBuildEditorWindowRunning =  EditorWindow.HasOpenInstances<BuildEditorWindow>();
#else
            // Use the GetWindow method and check if the result is not null (Unity 2018 and earlier)
            isBuildEditorWindowRunning = EditorWindow.GetWindow<BuildEditorWindow>(false) != null;
#endif
            if (isBuildEditorWindowRunning && (EditorPrefs.GetInt(ImportingClass) == (int)ImportStatus.Importimg
                || EditorPrefs.GetInt(ImportingClass) == (int)ImportStatus.FirstInjection))
            {
                EditorUtility.ClearProgressBar();
                EditorPrefs.SetInt(ImportingClass, (int)ImportStatus.UpgradeCompleted);
            }
        }
    }
}