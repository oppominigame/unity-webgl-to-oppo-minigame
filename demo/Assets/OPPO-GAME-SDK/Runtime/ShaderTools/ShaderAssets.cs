using System.Collections.Generic;
using UnityEngine;

namespace QGMiniGame
{
    public class ShaderAssets : MonoBehaviour
    {
        //场景名    
        public string sceneName;
        //场景路径
        public string scenePath;
        //资源类型
        public int objectType;
        //预制名
        public string prefabName;
        //预制路径
        public string prefabPath;
        //预制AB名
        public string prefabABName;
        //挂载shader的节点名
        public List<string> prefabNodeList;
        public List<string> rendererNameList;
        public List<Material> materialList;
        public List<Shader> shaderList;
        //异常shader
        public List<string> errorShaderData;
        //天空盒材质路径
        public string skyBoxPath;
    }

    public class ShaderNode
    {
        //节点名字
        public string nodeName;
        //渲染器 
        public Renderer renderer;
        //材质
        public Material material;
        //着色器
        public Shader shader;
    }
}