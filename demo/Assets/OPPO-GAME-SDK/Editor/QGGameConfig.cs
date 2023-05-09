using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

namespace QGMiniGame
{
  
    [Serializable]
    public class RunCmdConfig
    {
        public string command;
        public string workSrc;
        public bool startTerminal;
    }

    [Serializable]
    public class UpdateResonse
    {
        [SerializeField] public UpdateConfig data;
    }

    [Serializable]
    public class UpdateConfig
    {
        public string pluginUrl = "";
        public int version ;
    }

    [Serializable]
    public class EnvConfig
    {
        public string wasmUrl;
        public string streamingAssetsUrl;
        public string preloadUrl;
    }

    [Serializable]
    public class WebGlConfig
    {
        public string companyName = "DefaultCompany";
        public string productName = "minigame-package";
        public string productVersion = "0.1";
        public string dataUrl = "webgl.data.unityweb";
        public string wasmCodeUrl = "webgl.wasm.code.unityweb";
        public string wasmFrameworkUrl = "webgl.wasm.framework.unityweb";
        public string[] graphicsAPI = { "WebGL 1.0" };
        public WebglContext webglContextAttributes = new WebglContext();
        public string splashScreenStyle = "Dark";
        public string backgroundColor = "#231F20";
        public bool developmentBuild = false;
        public bool multithreading = false;
        public string unityVersion = Application.unityVersion;
        public int unityPluginVersion = QG.SDK_VERSION;
    }

    [Serializable]
    public class WebglContext
    {
        public bool preserveDrawingBuffer = false;
    }

    [Serializable]
    public class RenameFile 
    {
        public string oldName;
        public string newName;
    }

    [Serializable]
    public class QGGameConfig : ScriptableObject
    {
        public EnvConfig envConfig;
        public string buildSrc = "";
        public string packageName = "";
        public string projectName = "";
        public int orientation = 0;
        public string projectVersionName = "";
        public string projectVersion = "";
        public string minPlatVersion = "";
        public bool useAddressable;
        public bool useSign;
        public string signCertificate = "";
        public string signPrivate = "";
        public string iconPath = "";
    }
}



