using System.Collections.Generic;
using UnityEngine;

namespace QGMiniGame
{
    public class ShaderAssets : MonoBehaviour
    {
        //������    
        public string sceneName;
        //����·��
        public string scenePath;
        //��Դ����
        public int objectType;
        //Ԥ����
        public string prefabName;
        //Ԥ��·��
        public string prefabPath;
        //Ԥ��AB��
        public string prefabABName;
        //����shader�Ľڵ���
        public List<string> prefabNodeList;
        public List<string> rendererNameList;
        public List<Material> materialList;
        public List<Shader> shaderList;
        //�쳣shader
        public List<string> errorShaderData;
        //��պв���·��
        public string skyBoxPath;
    }

    public class ShaderNode
    {
        //�ڵ�����
        public string nodeName;
        //��Ⱦ�� 
        public Renderer renderer;
        //����
        public Material material;
        //��ɫ��
        public Shader shader;
    }
}