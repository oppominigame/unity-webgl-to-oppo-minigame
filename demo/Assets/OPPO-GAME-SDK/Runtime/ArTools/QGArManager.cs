using UnityEngine;
using UnityEngine.UI;
using System;
using System.Runtime.InteropServices;
using AOT;
using static System.Net.Mime.MediaTypeNames;

namespace QGMiniGame
{
    public class QGArManager
    {
        static bool isGetARCameraData = false;//已获取相机数据?

        static bool isGetARPostData = false;//已获取位姿数据?
        public delegate void CameraImageCallback(IntPtr ptr, int length, int width, int height);

        //public delegate void CameraArPoseCallback(IntPtr ptr, int modelMatrixArrayLen);
        public delegate void CameraArPoseCallback(string posStr, string rotStr);
        public static ARCameraParam CameraParam = new ARCameraParam();
        public static ARPostParam PostParam = new ARPostParam();
        public static ARPostData PostData = new ARPostData();

        [DllImport("__Internal")]
        public static extern void QGStartARCamera(CameraImageCallback callback);
        [DllImport("__Internal")]
        public static extern void QGDestroyARCamera();
        [DllImport("__Internal")]
        public static extern void QGRequireARCameraImage();
        [DllImport("__Internal")]
        public static extern void QGCreateOppoARPose(CameraArPoseCallback cameraArPoseCallback);
        [DllImport("__Internal")]
        public static extern void QGRequireARPose();

        [MonoPInvokeCallback(typeof(CameraImageCallback))]
        public static void OnARCameraImageCallback(IntPtr ptr, int length, int width, int height)
        {
            CameraParam.ptr = ptr;
            CameraParam.length = length;
            CameraParam.width = width;
            CameraParam.height = height;
            isGetARCameraData = true;
        }

        /*
         * 
        [MonoPInvokeCallback(typeof(CameraArPoseCallback))]
        public static void OnARPostCallback(IntPtr ptr, int modelMatrixArrayLen)
        {
            PostParam.arpost = ptr;
            PostParam.modelMatrixArrayLen = modelMatrixArrayLen;
            isGetARPostData = true;
        }
        */

        [MonoPInvokeCallback(typeof(Action<string, string>))]
        public static void OnARPostCallback(string posStr, string rotStr)
        {
            PostParam.aRPotStr = posStr;
            PostParam.aRRotStr = rotStr;
            isGetARPostData = true;
        }

        //Ar相机接口(初始化)
        public static void StartARCamera(Action<string> successCallback, Action<string> failCallback)
        {
            QGMiniGameManager.Instance.AddARCameraCallBack(successCallback, failCallback);
            QGStartARCamera(OnARCameraImageCallback);
        }

        public static void DestroyARCamera()
        {
            QGDestroyARCamera();
        }

        //调用位姿接口(初始化)
        public static void CreateOppoARPose()
        {
            QGCreateOppoARPose(OnARPostCallback);
        }

        //渲染相机到RawImage 每n帧执行一次 (RawImage,成功回调)
        public static void LoadRawTexture(RawImage rawImage, Action successCallback)
        {
            QGRequireARCameraImage();
            if (!isGetARCameraData)
            {
                return;
            }
            if (!rawImage.texture)
            {
                rawImage.texture = new Texture2D(CameraParam.width, CameraParam.height, TextureFormat.RGBA32, false);
            }
            var tex = (rawImage.texture as Texture2D);
            tex.LoadRawTextureData(CameraParam.ptr, CameraParam.length);
            tex.Apply();
            rawImage.color = CameraParam.length > 0 ? Color.white : Color.black;
            rawImage.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(CameraParam.width, CameraParam.height);
            if (successCallback != null)
            {
                successCallback();
            }
            isGetARCameraData = false;
        }

        //渲染相机到RawImage 每n帧执行一次 (RawImage,成功回调)
        public static Texture LoadTexture()
        {
            QGRequireARCameraImage();
            if (!isGetARCameraData)
            {
                return null;
            }
            Texture2D texture = Texture2D.CreateExternalTexture(CameraParam.width, CameraParam.height, TextureFormat.RGBA32, false, false, CameraParam.ptr);
            texture.LoadRawTextureData(CameraParam.ptr, CameraParam.length);
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Bilinear;
            isGetARCameraData = false;
            return texture;
        }

        //获取相机数据
        public static ARCameraParam GetARCameraParam()
        {
            QGRequireARCameraImage();
            if (!isGetARCameraData)
            {
                return null;
            }
            isGetARCameraData = false;
            return CameraParam;
        }

        //获取位姿数据 每n帧执行一次 (成功回调)
        public static ARPostData GetRequireARPose(Action successCallback)
        {
            QGRequireARPose();
            if (isGetARPostData)
            {
                /*
                 * 
                 * 
                byte[] modelByteArray = new byte[PostParam.modelMatrixArrayLen];
                Marshal.Copy(PostParam.arpost, modelByteArray, 0, PostParam.modelMatrixArrayLen);

                float[] modelMatrixArray = ConvertByteArrayToFloatArray(modelByteArray);
                string myFloatArrayString = string.Join(", ", modelMatrixArray);
                PostData.posX = modelMatrixArray[0];
                PostData.posY = modelMatrixArray[1];
                PostData.posZ = modelMatrixArray[2];

                PostData.rotX = modelMatrixArray[3];
                PostData.rotY = modelMatrixArray[4];
                PostData.rotZ = modelMatrixArray[5];
                PostData.rotW = modelMatrixArray[6];
                GC.Collect();
                Array.Clear(modelByteArray, 0, modelByteArray.Length);
                Array.Clear(modelMatrixArray, 0, modelMatrixArray.Length);
                modelByteArray = null;
                modelMatrixArray = null;

                Marshal.FreeCoTaskMem(PostParam.arpost);
                Marshal.FreeHGlobal(PostParam.arpost);
                */
                string[] PotStr = PostParam.aRPotStr.Split(',');
                string[] RotStr = PostParam.aRRotStr.Split(',');

                float[] potStrArray = new float[PotStr.Length];
                for (int i = 0; i < PotStr.Length; i++)
                {
                    potStrArray[i] = float.Parse(PotStr[i]);
                }

                float[] rotStrArray = new float[RotStr.Length];
                for (int i = 0; i < RotStr.Length; i++)
                {
                    rotStrArray[i] = float.Parse(RotStr[i]);
                }

                PostData.posX = potStrArray[0];
                PostData.posY = potStrArray[1];
                PostData.posZ = potStrArray[2];

                PostData.rotX = rotStrArray[0];
                PostData.rotY = rotStrArray[1];
                PostData.rotZ = rotStrArray[2];
                PostData.rotW = rotStrArray[3];

                if (successCallback != null)
                {
                    successCallback();
                }

                isGetARPostData = false;

            }
            return PostData;
        }

        //已获取相机数据?
        public static bool IsGetARCameraData()
        {
            return isGetARCameraData;
        }

        //已获取位姿数据?
        public static bool IsGetARPostData()
        {
            return isGetARPostData;
        }

        public static float[] ConvertByteArrayToFloatArray(byte[] bytes)
        {
            int floatSize = sizeof(float);
            int floatCount = bytes.Length / floatSize;
            float[] floats = new float[floatCount];

            for (int i = 0; i < floatCount; i++)
            {
                byte[] floatBytes = new byte[floatSize];
                Array.Copy(bytes, i * floatSize, floatBytes, 0, floatSize);
                floats[i] = BitConverter.ToSingle(floatBytes, 0);
            }

            return floats;
        }
    }
}
