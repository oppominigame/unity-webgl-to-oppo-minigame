using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using System.Text.RegularExpressions;
using UnityEngine.Rendering;
using System.Collections;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace QGMiniGame
{
    public class OppoEditorWindow : EditorWindow
    {
        public static string packageName = "";
        public static string projectName = "";
        public static int orientation = 0;
        public static string projectVersionName = "";
        public static string projectVersion = "";
        public static string minPlatVersion = "1103";
        public static string streamingAssetsUrl = "";
        public static bool useSign;
        public static string signCertificate = "";
        public static string signPrivate = "";
        public static bool useAddressable;
        public static string buildSrc = "";  //用户选择构建的webgl路径
        public static string webglDir = "webgl";       //构建的webgl文件夹
        public static string iconPath = "";

        [MenuItem("OPPO小游戏 / 构建", false, 1)]
        public static void Open()
        {

            QGLog.Log("Version of the runtime: " + UnityEngine.Application.unityVersion);

#if !(UNITY_2018_1_OR_NEWER)
            UnityEngine.Debug.LogError("目前仅支持 Unity2018及以上的版本！");
#endif
            var win = GetWindow(typeof(OppoEditorWindow), false, "构建OPPO小游戏", true);//创建窗口
            win.minSize = new Vector2(650, 800);
            win.maxSize = new Vector2(1600, 950);

            win.Show();

            InitData();
        }

        public static void InitData()
        {
            QGGameConfig config = QGGameTools.GetEditorConfig();
            buildSrc = config.buildSrc;
            packageName = config.packageName;
            projectName = config.projectName;
            orientation = config.orientation;
            projectVersionName = config.projectVersionName;
            projectVersion = config.projectVersion;
            minPlatVersion = config.minPlatVersion;
            useSign = config.useSign;
            signCertificate = config.signCertificate;
            signPrivate = config.signPrivate;
            useAddressable = config.useAddressable;
            streamingAssetsUrl = config.envConfig.streamingAssetsUrl;
            iconPath = config.iconPath;
        }

        private void OnGUI()
        {
            var labelStyle = new GUIStyle(EditorStyles.boldLabel);
            labelStyle.fontSize = 14;
            labelStyle.margin.left = 20;
            labelStyle.margin.top = 10;
            labelStyle.margin.bottom = 10;
            GUILayout.Label("基本设置", labelStyle);

            var inputStyle = new GUIStyle(EditorStyles.textField);
            inputStyle.fontSize = 14;
            inputStyle.margin.left = 20;
            inputStyle.margin.bottom = 10;
            inputStyle.margin.right = 20;

            var intPopupStyle = new GUIStyle(EditorStyles.popup);
            intPopupStyle.fontSize = 14;
            intPopupStyle.margin.left = 20;
            intPopupStyle.margin.bottom = 15;
            intPopupStyle.margin.right = 20;

            GUIStyle toggleStyle = new GUIStyle(GUI.skin.toggle);
            toggleStyle.margin.left = 20;
            toggleStyle.margin.right = 20;
            toggleStyle.margin.top = 10;
            toggleStyle.fontSize = 12;

            packageName = EditorGUILayout.TextField("游戏包名", packageName, inputStyle);
            projectName = EditorGUILayout.TextField("游戏名称", projectName, inputStyle);
            orientation = EditorGUILayout.IntPopup("游戏方向", orientation, new[] { "Portrait", "Landscape" }, new[] { 0, 1, 2, 3 }, intPopupStyle);
            projectVersionName = EditorGUILayout.TextField("游戏版本名称", projectVersionName, inputStyle);
            projectVersion = EditorGUILayout.TextField("游戏版本号", projectVersion, inputStyle);
            minPlatVersion = EditorGUILayout.TextField("支持的最小平台版本号", minPlatVersion, inputStyle);//最小支持版本号1103

            GUILayout.BeginHorizontal();
            iconPath = EditorGUILayout.TextField("游戏图标", iconPath, inputStyle);
            if (GUILayout.Button("选择", GUILayout.Width(50f)))
            {
                iconPath = EditorUtility.OpenFilePanel("选择游戏图标", "", "png,jpg,jpeg");
            }
            GUILayout.EndHorizontal();

            useSign = GUILayout.Toggle(useSign, "使用秘钥库", toggleStyle);
            if (useSign)
            {
                GUILayout.BeginHorizontal();
                signCertificate = EditorGUILayout.TextField("certificate.pem路径", signCertificate, inputStyle);
                if (GUILayout.Button("选择", GUILayout.Width(50f)))
                {
                    signCertificate = EditorUtility.OpenFilePanel("选择certificate.pem路径", "", "");
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                signPrivate = EditorGUILayout.TextField("private.pem路径", signPrivate, inputStyle);
                if (GUILayout.Button("选择", GUILayout.Width(50f)))
                {
                    signPrivate = EditorUtility.OpenFilePanel("选择private.pem路径", "", "");
                }
                GUILayout.EndHorizontal();
            }

            useAddressable = GUILayout.Toggle(useAddressable, "使用Addressable", toggleStyle);
            if (useAddressable)
            {
                streamingAssetsUrl = EditorGUILayout.TextField("Addressable地址", streamingAssetsUrl, inputStyle);
            }

            /***************************************设置项目导出的路径*********************************************************/
            GUILayout.Label("WEBGL导出路径(必填)", labelStyle);
            var chooseBuildPathClick = false;
            var openBuildPathClick = false;
            var resetBuildPathClick = false;


            if (buildSrc == String.Empty)
            {
                GUIStyle pathButtonStyle = new GUIStyle(GUI.skin.button);
                pathButtonStyle.fontSize = 12;
                pathButtonStyle.margin.left = 20;
                chooseBuildPathClick = GUILayout.Button("选择导出路径 *", pathButtonStyle, GUILayout.Height(30), GUILayout.Width(200));
            }
            else
            {
                int pathButtonHeight = 28;
                GUIStyle pathLabelStyle = new GUIStyle(GUI.skin.textField);
                pathLabelStyle.fontSize = 12;
                pathLabelStyle.alignment = TextAnchor.MiddleLeft;
                pathLabelStyle.margin.top = 6;
                pathLabelStyle.margin.bottom = 6;
                pathLabelStyle.margin.left = 20;

                GUILayout.BeginHorizontal();
                // 路径框
                GUILayout.Label(buildSrc, pathLabelStyle, GUILayout.Height(pathButtonHeight - 6), GUILayout.ExpandWidth(true), GUILayout.MaxWidth(EditorGUIUtility.currentViewWidth - 126));
                openBuildPathClick = GUILayout.Button("打开", GUILayout.Height(pathButtonHeight), GUILayout.Width(40));
                resetBuildPathClick = GUILayout.Button("重选", GUILayout.Height(pathButtonHeight), GUILayout.Width(40));
                GUILayout.EndHorizontal();
            }

            EditorGUILayout.Space();

            if (chooseBuildPathClick)
            {
                var dstPath = EditorUtility.SaveFolderPanel("选择导出目录", "", "");

                if (dstPath != "")
                {
                    buildSrc = dstPath;
                }
            }

            if (openBuildPathClick)
            {
                QGGameTools.ShowInExplorer(buildSrc);
            }

            if (resetBuildPathClick)
            {
                buildSrc = "";
            }

            /*********************************************开始构建WEBGL项目*********************************************************/

            GUIStyle exportButtonStyle = new GUIStyle(GUI.skin.button);
            exportButtonStyle.fontSize = 14;
            exportButtonStyle.margin.left = 20;
            exportButtonStyle.margin.top = 40;

            var isExportBtnPressed = GUILayout.Button("构建WEBGL并转换小游戏", exportButtonStyle, GUILayout.Height(40), GUILayout.Width(EditorGUIUtility.currentViewWidth - 40));

            if (isExportBtnPressed)
            {
                DoBuild();
            }
        }
        // 构建webgl
        public void DoBuild()
        {
            QGGameTools.setEditorConfig(buildSrc, packageName, projectName, orientation, projectVersionName, projectVersion, minPlatVersion, useAddressable,streamingAssetsUrl, iconPath, useSign, signCertificate, signPrivate);
            //test start
            /*            var webGlPath = Path.Combine(buildSrc, webglDir);
                        QGGameTools.ConvetWebGl(buildSrc, webGlPath);*/
            //test end
            if (useSign)
            {
                if (signCertificate == String.Empty || signPrivate == String.Empty)
                {
                    ShowNotification(new GUIContent("请选择正确的签名路径"));
                    return;
                }
                else
                {
                    if (signCertificate.EndsWith(".pem") && signPrivate.EndsWith(".pem"))
                    {
                        //指定签名
                        QGLog.Log("");
                    }
                    else
                    {
                        ShowNotification(new GUIContent("请选择正确的签名路径"));
                        return;
                    }
                }
            }

            if (buildSrc == String.Empty)
            {
                ShowNotification(new GUIContent("请先选择游戏导出路径"));
            }
            else
            {
                // 判断是否是webgl平台
                if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.WebGL)
                {
                    if (!EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.WebGL, BuildTarget.WebGL))
                    {
                        ShowNotification(new GUIContent("构建失败，请配置unity webgl构建环境"));
                        return;
                    }
                }

                var webGlPath = Path.Combine(buildSrc, webglDir);
                QGGameTools.SetPlayer();
                QGGameTools.DelectDir(webGlPath);
                QGGameTools.BuildWebGL(webGlPath, buildSrc);
                if (!Directory.Exists(webGlPath))
                {
                    ShowNotification(new GUIContent("构建失败，WebGl项目未成功生成"));
                    return;
                }

                //QGGameTools.CreateEnvConfig(useSelfLoading ? wasmUrl : "", streamingAssetsUrl, webGlPath, assetsList);
                QGGameTools.ConvetWebGl(buildSrc, webGlPath);
            }
        }
    }
}
