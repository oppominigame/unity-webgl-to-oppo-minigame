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
            Idle,
            InProgress,
            Success
        }

        private enum ImportStatus : int
        {
            FirstInjection = 0, //首次注入
            Importimg = 1,      //正常注入
            UpgradeCompleted = 2, //升级完成
        }

        private const string ASSET_CACHE_SYSTEM_MIN_VERSION = "2.1.9-beta.11";
        private const string SDK_SERVER_URL = "https://ie-activity-cn.heytapimage.com/static/minigame/OPPO-GAME-SDK/tools";
        private const string UPDATE_LOG_URL = "https://github.com/oppominigame/unity-webgl-to-oppo-minigame/blob/main/CHANGELOG.md";
        private const string CONTACT_US_URL = "https://github.com/oppominigame/unity-webgl-to-oppo-minigame/blob/main/doc/IssueAndContact.md.md";
        private const string OPENSSL_EXE_URL = "https://ie-activity-cn.heytapimage.com/static/minigame/hall/example/20240918/assets/OpenSSL-Win64.zip";
        /// <summary>
        /// spin 序列帧切换间隔（内置 12 张序列帧 icon，命名为 WaitSpin00-WaitSpin11）
        /// </summary>
        private const float SPIN_GAP = 1.0f / 12;

        private static string GenerateCertificatePath = "";     //证书路径
        private static bool isHaveCertificatePath = false;      //是否导入证书
        private static bool isInstallOpenssl = false;           //是否安装openssl(全局)
        private static bool isHaveOpenSslZIP = false;           //是否解压openssl(zip)
        private static float ImportPackagTime = 0;      //导入计时   
        private static float ImportPackagOvertime = 10; //导入超时
        private static Timer timer;
        private static string ImportingClass = $"{nameof(BuildEditorWindow)}.ImportingStatus";

        private static BuildEditorWindow instance;

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

        public static string OpenSSLExeUnzipPath => $"{Application.temporaryCachePath}/OpenSSL-Win64";
        public static string OpenSSLExeLocalPath => $"{OpenSSLExeUnzipPath}/bin/openssl.exe";

        private string LatestSDKPackageTempPath => Path.Combine(Application.temporaryCachePath, latestSDKPackageName);

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
        /// 获取打包工具当前版本信息状态
        /// </summary>
        private RequestVersionStatus requestQGBuildToolCurrentVersionStatus = RequestVersionStatus.Idle;
        /// <summary>
        /// 获取打包工具最新版本信息状态
        /// </summary>
        private RequestVersionStatus requestQGBuildToolLatestVersionStatus = RequestVersionStatus.Idle;
        /// <summary>
        /// 当前打包工具版本
        /// </summary>
        private string currentBuildToolVersion;
        /// <summary>
        /// 最新打包工具版本
        /// </summary>
        private string latestBuildToolVersion;
        /// <summary>
        /// 获取工具包最新版本信息状态
        /// </summary>
        private RequestVersionStatus requestSDKLatestVersionStatus = RequestVersionStatus.Idle;
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

        private static BuildEditorWindow GetOrCreateWindow() => GetWindow(typeof(BuildEditorWindow), false, "打包小游戏", true) as BuildEditorWindow;

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

            var success = await CreateCertificate(command, generateInfo);
            if (HandleError(success, "证书生成失败")) return;

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
            if (!await IsInstallOpenssl())
            {
                if (await HandleOpensslDownloadAndInstall()) return;
            }
        }

        private static async Task<bool> HandleOpensslDownloadAndInstall()
        {
            if (!File.Exists(OpenSSLExeLocalPath) && !File.Exists($"{OpenSSLExeUnzipPath}/OpenSSL-Win64.zip"))
            {
                if (!ShowDownloadDialog("需要下载openssl")) return true;

                DisplayProgress("创建证书", "openssl 下载中...", 0.3f);
                var downloadSuccess = await StartDownloadOpenssl();
                if (HandleError(downloadSuccess, "openssl下载失败")) return true;

                DisplayProgress("创建证书", "openssl 下载成功", 0.4f);
            }

            // if (!ShowDownloadDialog("需要安装openssl")) return true; 默认安装
            DisplayProgress("创建证书", "openssl 安装中...", 0.5f);
            var installSuccess = await StartsInstallOpenssl();
            return HandleError(installSuccess, "openssl安装失败");
        }

        private static bool ShowDownloadDialog(string message)
        {
            return EditorUtility.DisplayDialog("提示", message, "确认", "取消");
        }

        private static void DisplayProgress(string title, string message, float progress)
        {
            EditorUtility.DisplayProgressBar(title, message, progress);
        }

        private static bool HandleError(bool success, string errorMessage)
        {
            if (!success)
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
        private static Task<bool> IsInstallOpenssl()
        {
            // 异步执行
            return Task.Run(() =>
            {
                try
                {
                    ShellHelper.ExecuteCommand("openssl -v");
                    return true;
                }
                catch
                {
                    return File.Exists(OpenSSLExeLocalPath);
                }
            });
        }

        /// <summary>
        /// 异步下载openssl(下载压缩包)
        /// </summary>
        /// <returns>错误信息（没有代表成功）</returns>
        private static async Task<bool> StartDownloadOpenssl()
        {
            // 异步执行
            return await Task.Run(() =>
            {
                if (!Directory.Exists(OpenSSLExeUnzipPath))
                {
                    Directory.CreateDirectory(OpenSSLExeUnzipPath);
                }
                try
                {
                    ShellHelper.ExecuteCommand($"curl -o {OpenSSLExeUnzipPath}/OpenSSL-Win64.zip {OPENSSL_EXE_URL}");
                    return true;
                }
                catch
                {
                    return false;
                }
            });
        }


        /// <summary>
        /// 异步安装openssl(解压缩包)
        /// </summary>
        /// <returns>错误信息（没有代表成功）</returns>
        private static async Task<bool> StartsInstallOpenssl()
        {
            // 异步执行
            return await Task.Run(() =>
            {
                try
                {
                    ShellHelper.ExecuteCommand($"tar -xf {OpenSSLExeUnzipPath}/OpenSSL-Win64.zip -C {OpenSSLExeUnzipPath}");
                    isHaveOpenSslZIP = true;
                    return true;
                }
                catch
                {
                    return false;
                }
            });
        }

        /// <summary>
        /// 异步生成证书
        /// </summary>
        /// <returns>错误信息（没有代表成功）</returns>
        private static async Task<bool> CreateCertificate(string command, string[] generateInfo)
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
                    command = $"{OpenSSLExeLocalPath}" + command;
                }
                try
                {
                    ShellHelper.ExecuteCommand(command);
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
                    return true;
                }
                catch
                {
                    return false;
                }
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
            DrawOtherSettings();
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
            requestQGBuildToolCurrentVersionStatus = RequestVersionStatus.InProgress;
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
        private async Task<bool> RequestQGBuildToolCurrentVersionAsync()
        {
            // 先重置当前版本
            currentBuildToolVersion = string.Empty;
            // 异步执行
            return await Task.Run(() =>
            {
                try
                {
                    var output = ShellHelper.ExecuteCommand("quickgame -V");
                    currentBuildToolVersion = output.TrimEnd('\n');
                    return true;
                }
                catch
                {
                    return false;
                }
            });
        }

        /// <summary>
        /// 异步获取打包工具最新版本号
        /// </summary>
        /// <returns>错误信息（没有代表成功）</returns>
        private async Task<bool> RequestQGBuildToolLatestVersionAsync()
        {
            // 先重置当前版本
            latestBuildToolVersion = string.Empty;
            // 异步执行
            return await Task.Run(() =>
            {
                try
                {
                    var output = ShellHelper.ExecuteCommand("npm show @oppo-minigame/cli versions");
                    // 缓存最新版本号
                    var versions = output.Replace("\n", string.Empty).Replace(" ", string.Empty).TrimStart('[').TrimEnd(']').Split(',');
                    latestBuildToolVersion = versions[versions.Length - 1].TrimStart('\'').TrimEnd('\'');
                    return true;
                }
                catch
                {
                    return false;
                }
            });
        }

        /// <summary>
        /// 异步升级打包工具到最新版本
        /// </summary>
        /// <returns>错误信息（没有代表成功）</returns>
        private async Task<bool> UpgradeQGBuildToolLatestVersionAsync()
        {
            // 异步执行
            return await Task.Run(() =>
            {
                try
                {
                    ShellHelper.ExecuteCommand($"npm install @oppo-minigame/cli@{latestBuildToolVersion} -g");
                    return true;
                }
                catch
                {
                    return false;
                }
            });
        }

        /// <summary>
        /// 异步获取最新 SDK 版本
        /// </summary>
        /// <returns></returns>
        private async Task<bool> RequestLatestSDKVersionAsync()
        {
            // 先重置当前版本数据
            latestSDKVersion = latestSDKPackageName = latestSDKPackageMD5 = string.Empty;
            // 发送网络请求版本号
            var uwr = UnityWebRequest.Get($"{SDK_SERVER_URL}/version");
            uwr.SendWebRequest();
            // 等待网络请求完成
            await WaitWebRequestComplete(uwr);
            // 当前请求成功
            var error = uwr.error;
            var success = !error.IsValid();
            if (success)
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
                    success = false;
                    error = "Parse version failed";
                }
            }
            if (!success)
            {
                latestSDKVersion = latestSDKPackageName = string.Empty;
                // 输出错误信息到控制台
                Debug.LogError(error);
            }
            // 返回结果
            return success;
        }

        /// <summary>
        /// 控制台输出处理器
        /// </summary>
        /// <param name="success"></param>
        /// <returns>是否成功</returns>
        private bool CommandOutputHandler(bool success)
        {
            if (success)
            {
                return true;
            }
            EditorUtility.DisplayDialog("提示", $"出错了，请根据控制台错误，查阅技术文档或联系技术客服", "确认");
            EditorUtility.ClearProgressBar();
            return false;
        }

        /// <summary>
        /// 异步更新打包工具当前版本状态
        /// </summary>
        private async void UpdateQGBuildToolCurrentVersionStatusAsync()
        {
            // 异步获取版本号信息
            requestQGBuildToolCurrentVersionStatus = RequestVersionStatus.InProgress;
            var success = await RequestQGBuildToolCurrentVersionAsync();
            // 获取当前版本失败，中断退出
            if (!success)
            {
                requestQGBuildToolCurrentVersionStatus = RequestVersionStatus.Fail;
                Repaint();
                return;
            }
            // 设置获取成功
            requestQGBuildToolCurrentVersionStatus = RequestVersionStatus.Success;
            Repaint();
        }

        /// <summary>
        /// 异步更新打包工具最新版本状态
        /// </summary>
        private async void UpdateQGBuildToolLatestVersionStatusAsync()
        {
            // 异步获取最新版本
            requestQGBuildToolLatestVersionStatus = RequestVersionStatus.InProgress;
            var success = await RequestQGBuildToolLatestVersionAsync();
            // 获取最新版本失败，中断退出
            if (!success)
            {
                requestQGBuildToolLatestVersionStatus = RequestVersionStatus.Fail;
                Repaint();
                return;
            }
            // 设置获取成功
            requestQGBuildToolLatestVersionStatus = RequestVersionStatus.Success;
            Repaint();
        }

        /// <summary>
        /// 异步更新 SDK 最新版本状态
        /// </summary>
        private async void UpdateSDKLatestVersionStatusAsync()
        {
            // 异步获取最新版本信息
            requestSDKLatestVersionStatus = RequestVersionStatus.InProgress;
            var success = await RequestLatestSDKVersionAsync();
            // 获取最新版本失败，中断退出
            if (!success)
            {
                requestSDKLatestVersionStatus = RequestVersionStatus.Fail;
                Repaint();
                return;
            }
            // 设置获取成功
            requestSDKLatestVersionStatus = RequestVersionStatus.Success;
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
            UpdateQGBuildToolCurrentVersionStatusAsync();
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

        private void WaitSpinIcon()
        {
            // 使用内置序列帧图片，每秒转一圈
            var frac = Time.realtimeSinceStartupAsDouble - Math.Truncate(Time.realtimeSinceStartup);
            var index = (int)Math.Truncate(frac / SPIN_GAP);
            var indexStr = index.ToString();
            if (index < 10)
            {
                indexStr = "0" + indexStr;
            }
            GUILayout.Label(EditorGUIUtility.IconContent($"WaitSpin{indexStr}"));
            // 需要立即重新绘制，不然界面不会动
            Repaint();
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

        private string ValidationTextField(ref string target, string key, string label, string text, bool allowEmpty, string emptyError = "不能为空", string pattern = "", string invalidError = "校验失败", Func<bool> customFunc = null, bool showErrorGUI = true)
        {
            return ValidationTextField(ref target, key, new GUIContent(label), text, allowEmpty, emptyError, pattern, invalidError, customFunc, showErrorGUI);
        }

        private string ValidationTextField(ref string target, string key, GUIContent label, string text, bool allowEmpty, string emptyError = "不能为空", string pattern = "", string invalidError = "校验失败", Func<bool> customFunc = null, bool showErrorGUI = true)
        {
            // 记录需要校验的属性，默认校验通过
            Builder.InitValidation(key);
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
                Builder.Validate(new Builder.ValidateParameterData
                {
                    target = target,
                    key = key,
                    allowEmpty = allowEmpty,
                    emptyError = emptyError,
                    pattern = pattern,
                    invalidError = invalidError,
                    customFunc = customFunc
                });
                validateForOnceKeyList.Remove(key);
            }
            // 校验失败且需要立即显示错误提示
            if (Builder.GetValidation(key, out var error) && error.IsValid() && showErrorGUI)
            {
                ValidateLabel(error);
            }
            return error;
        }

        private void ValidateTextFieldForOnce(string key)
        {
            validateForOnceKeyList.Add(key);
        }

        private void ValidateAllTextFieldForOnce()
        {
            var keys = Builder.ValidationKeys;
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
                        emptyError: "请选择游戏图标路径",
                        pattern: string.Empty,
                        invalidError: "图标不存在，请重新选择",
                        customFunc: () => { return File.Exists(fundamentals.iconPath); },
                        showErrorGUI: false);
                    if (GUILayout.Button("选择", GUILayout.Width(64f)))
                    {
                        ClearTextFieldFocus();
                        var iconPath = EditorUtility.OpenFilePanel("选择游戏图标", "", "png,jpg,jpeg");
                        if (iconPath.IsValid())
                        {
                            SaveProperty(ref fundamentals.iconPath, iconPath);
                            // 选择了即有效，直接修改结果
                            Builder.ModifyValidation(nameof(fundamentals.iconPath), string.Empty);
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
                        emptyError: "请选择证书路径",
                        pattern: string.Empty,
                        invalidError: "证书不存在，请重新选择",
                        customFunc: () => { return useCustomSign ? File.Exists(fundamentals.signCertificate) : true; },
                        showErrorGUI: false);
                    if (GUILayout.Button("选择", GUILayout.Width(64f)))
                    {
                        ClearTextFieldFocus();
                        var certificatePath = EditorUtility.OpenFilePanel("选择证书文件", "", "pem");
                        if (certificatePath.IsValid())
                        {
                            SaveProperty(ref fundamentals.signCertificate, certificatePath);
                            // 选择了即有效，直接修改结果
                            Builder.ModifyValidation(nameof(fundamentals.signCertificate), string.Empty);
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
                        emptyError: "请选择私钥路径",
                        pattern: string.Empty,
                        invalidError: "私钥不存在，请重新选择",
                        customFunc: () => { return useCustomSign ? File.Exists(fundamentals.signPrivate) : true; },
                        showErrorGUI: false);
                    if (GUILayout.Button("选择", GUILayout.Width(64f)))
                    {
                        ClearTextFieldFocus();
                        var privatePath = EditorUtility.OpenFilePanel("选择私钥文件", "", "pem");
                        if (privatePath.IsValid())
                        {
                            SaveProperty(ref fundamentals.signPrivate, privatePath);
                            // 选择了即有效，直接修改结果
                            Builder.ModifyValidation(nameof(fundamentals.signPrivate), string.Empty);
                        }
                        GUIUtility.ExitGUI();
                    }
                    if (isHaveCertificatePath)
                    {
                        var certificatePaths = $"{GenerateCertificatePath}/certificate.pem";
                        var privatePaths = $"{GenerateCertificatePath}/private.pem";
                        SaveProperty(ref fundamentals.signCertificate, certificatePaths);
                        SaveProperty(ref fundamentals.signPrivate, privatePaths);
                        Builder.ModifyValidation(nameof(fundamentals.signCertificate), string.Empty);
                        Builder.ModifyValidation(nameof(fundamentals.signPrivate), string.Empty);
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
                            Builder.ModifyValidation(nameof(fundamentals.signCertificate), string.Empty);
                            Builder.ModifyValidation(nameof(fundamentals.signPrivate), string.Empty);
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
                        emptyError: "地址不能为空",
                        pattern: @"https?://[-A-Za-z0-9+&@#/%?=~_|!:,.;]+[-A-Za-z0-9+&@#/%=~_|]",
                        invalidError: "需填写填写http://或https://开头的有效URL地址",
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
                            Builder.ModifyValidation(nameof(fundamentals.streamingAssetsURL), string.Empty);
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
                        emptyError: "请选择导出路径",
                        pattern: string.Empty,
                        invalidError: "路径不存在，请重新选择",
                        customFunc: () => { return isExportPathExist(); },
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
                            Builder.ModifyValidation(nameof(fundamentals.exportPath), string.Empty);
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
                    emptyError: string.Empty,
                    pattern: @"^[0-9]+(\.[0-9]+)*$",
                    invalidError: "需以英文句号分隔，每一段只能包含数字");
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
                if (!isSystemAvailable && requestQGBuildToolCurrentVersionStatus != RequestVersionStatus.InProgress)
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
                    target: ref assetCache.gameCDNRoot,
                    key: nameof(assetCache.gameCDNRoot),
                    label: new GUIContent("缓存CDN路径(必填)", "缓存路径必填项，例如 http://10.117.224.49:8080/StreamingAssets"),
                    text: assetCache.gameCDNRoot,
                    allowEmpty: !assetCache.enableBundleCache,
                    emptyError: "地址不能为空",
                    pattern: @"https?://[-A-Za-z0-9+&@#/%?=~_|!:,.;]+[-A-Za-z0-9+&@#/%=~_|]",
                    invalidError: "需填写填写http://或https://开头的有效URL地址");
                ValidationTextField(
                    target: ref assetCache.bundlePathIdentifier,
                    key: nameof(assetCache.bundlePathIdentifier),
                    label: new GUIContent("缓存路径标识", "不填写代表所有路径都进行缓存判断。填写时多个时使用英文分号分隔，例如 StreamingAssets;bundles"),
                    text: assetCache.bundlePathIdentifier,
                    allowEmpty: true,
                    emptyError: string.Empty,
                    pattern: @"^([\w/]+)(;[\w/]+)*$",
                    invalidError: "路径只能使用字母、数字、下划线，多个路径之间用英文分号分隔");
                ValidationTextField(
                    target: ref assetCache.excludeFileExtensions,
                    key: nameof(assetCache.excludeFileExtensions),
                    label: new GUIContent("不缓存的文件类型", "不填写代表所有文件都进行缓存判断。填写多个时使用英文分号分隔，例如 .json;.hash"),
                    text: assetCache.excludeFileExtensions,
                    allowEmpty: true,
                    emptyError: string.Empty,
                    pattern: @"^\.[A-Za-z0-9]+(;\.[A-Za-z0-9]+)*$",
                    invalidError: "类型必须以英文句号开头，名称只能使用英文和数字，多个路径之间用英文分号分隔");
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
                    emptyError: string.Empty,
                    pattern: @"^[\w!@#\$%\^&\(\)\-=\+\[\]\{\}',\.`~]+(;[\w!@#\$%\^&\(\)\-=\+\[\]\{\}',\.`~]+)*$",
                    invalidError: "不支持/?<>\\:*|\"等特殊字符，多个文件之间用英文分号分隔");
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

        private void DrawOtherSettings()
        {
            Foldout("其他设置", () =>
            {
                ValidationTextField(
                    target: ref BuildConfigAsset.OtherSettingsConfig.environmentVariablePath,
                    key: nameof(BuildConfigAsset.OtherSettingsConfig.environmentVariablePath),
                    label: new GUIContent("环境变量 Path", "自定义的环境变量集合，Windows 以分号分隔路径，MacOS 以冒号分隔路径"),
                    text: BuildConfigAsset.OtherSettingsConfig.environmentVariablePath,
                    allowEmpty: true);
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
            var sdkVersionReady =
                requestSDKLatestVersionStatus == RequestVersionStatus.Idle ||
                requestSDKLatestVersionStatus == RequestVersionStatus.Success;
            var qgBuildToolVersionReady = requestQGBuildToolCurrentVersionStatus == RequestVersionStatus.Success;
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
                MiniLinkButton("检查更新", UpdateSDKLatestVersionStatusAsync);
            }
            void UpgradeButton()
            {
                MiniLinkButton("下载更新", SDKUpdateProcess);
            }
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            MiniLabelField($"SDK 版本: {VersionInfo.VERSION}", Color.green, false);
            switch (requestSDKLatestVersionStatus)
            {
                case RequestVersionStatus.Idle:
                    CheckLatestVersionButton();
                    break;
                case RequestVersionStatus.InProgress:
                    WaitSpinIcon();
                    break;
                case RequestVersionStatus.Success:
                    EditorGUILayout.BeginHorizontal();
                    CheckLatestVersionButton();
                    var versionCompare = CompareVersion(VersionInfo.VERSION, latestSDKVersion);
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
                    CheckLatestVersionButton();
                    MiniLabelField("检查更新失败", Color.red, true);
                    EditorGUILayout.EndHorizontal();
                    break;
            }
            EditorGUILayout.EndHorizontal();
        }

        private async void SDKUpdateProcess()
        {
            // 弹窗让用户确认升级
            var option = EditorUtility.DisplayDialogComplex("提示", $"是否确认下载SDK最新版本 {latestSDKVersion}", "确认", "取消", "查看更新日志");
            switch (option)
            {
                case 0:
                    if (isUpgradingSDK) return;
                    isUpgradingSDK = true;
                    var progressDialogTitle = $"下载 SDK V{latestSDKVersion}";
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
                    // 安装完成后打开安装包目录，提示用户导入手动导入升级
                    isUpgradingSDK = false;
                    if (EditorUtility.DisplayDialog("提示", "下载完成，建议删除【OPPO-GAME-SDK】目录后重新导入，避免出现兼容问题", "确定"))
                    {
                        QGGameTools.ShowInExplorer(LatestSDKPackageTempPath);
                    }
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
            void CheckCurrentVersionButton()
            {
                RefreshButton(UpdateQGBuildToolCurrentVersionStatusAsync);
            }
            void CheckLatestVersionButton()
            {
                MiniLinkButton("检查更新", UpdateQGBuildToolLatestVersionStatusAsync);
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
                    var success = await UpgradeQGBuildToolLatestVersionAsync();
                    if (!CommandOutputHandler(success))
                    {
                        isUpgradingQGBuildTool = false;
                        return;
                    }
                    EditorUtility.DisplayProgressBar(title, "正在校验版本...", 0.5f);
                    success = await RequestQGBuildToolCurrentVersionAsync();
                    if (!CommandOutputHandler(success))
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
            switch (requestQGBuildToolCurrentVersionStatus)
            {
                case RequestVersionStatus.InProgress:
                    MiniLabelField("获取版本中", Color.red);
                    WaitSpinIcon();
                    break;
                case RequestVersionStatus.Success:
                    EditorGUILayout.BeginHorizontal();
                    MiniLabelField($"小游戏打包工具版本: {currentBuildToolVersion}", Color.green);
                    switch (requestQGBuildToolLatestVersionStatus)
                    {
                        case RequestVersionStatus.Idle:
                            CheckCurrentVersionButton();
                            CheckLatestVersionButton();
                            break;
                        case RequestVersionStatus.InProgress:
                            WaitSpinIcon();
                            break;
                        case RequestVersionStatus.Success:
                            CheckCurrentVersionButton();
                            CheckLatestVersionButton();
                            var versionCompare = CompareVersion(currentBuildToolVersion, latestBuildToolVersion);
                            if (versionCompare.HasValue && versionCompare < 0)
                            {
                                UpgradeButton();
                            }
                            else
                            {
                                MiniLabelField("已是最新版本", Color.gray, true);
                            }
                            break;
                        case RequestVersionStatus.Fail:
                            CheckCurrentVersionButton();
                            CheckLatestVersionButton();
                            MiniLabelField("检查更新失败", Color.red, true);
                            break;
                    }
                    if (requestQGBuildToolLatestVersionStatus == RequestVersionStatus.Idle)
                    {
                    }
                    EditorGUILayout.EndHorizontal();
                    break;
                case RequestVersionStatus.Fail:
                    EditorGUILayout.BeginHorizontal();
                    MiniLabelField("获取打包工具版本失败，请根据日志指引操作", Color.red);
                    CheckCurrentVersionButton();
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
            if (Builder.IsAllValidationPass)
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
                        // 构建游戏
                        if (!QGGameTools.BuildGame(true))
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
            if (HasOpenInstances && (EditorPrefs.GetInt(ImportingClass) == (int)ImportStatus.Importimg
                || EditorPrefs.GetInt(ImportingClass) == (int)ImportStatus.FirstInjection))
            {
                EditorUtility.ClearProgressBar();
                EditorPrefs.SetInt(ImportingClass, (int)ImportStatus.UpgradeCompleted);
            }
        }
    }
}