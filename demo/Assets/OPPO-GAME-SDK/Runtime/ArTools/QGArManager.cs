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
        static bool isGetARCameraYuvData = false;//已获取Yuv相机数据?
        static bool isGetARPostData = false;//已获取位姿数据?
        static bool isGetARPlaneData = false;//已获取平面检测数据?
        static bool isGetARPlaneInfo = false;//已转换平面检测数据?
        static bool isGetDeviceMotionData = false;//已获取设备方向数据?
        public delegate void CameraImageCallback(IntPtr ptr, int length, int width, int height);
        public delegate void CameraImageYuvCallback(IntPtr ptr, int length);
        //public delegate void CameraArPoseCallback(IntPtr ptr, int modelMatrixArrayLen);
        public delegate void CameraArPoseCallback(string posStr, string rotStr);
        public delegate void CameraArPlaneCallback(string projectionMatrix, string viewMatrix, string modelMatrix, string modelViewMatrix, string modelViewProjectionMatrix, string planeAngleUvMatrix, string normalVector, string msg);
        public delegate void DeviceMotionChangeCallback(string alpha, string beta, string gamma);//设备方向
        public static ARCameraParam CameraParam = new ARCameraParam();
        public static ARCameraYuvParam CameraYuvParam = new ARCameraYuvParam();
        public static ARPostParam PostParam = new ARPostParam();
        public static ARPlaneParam PlaneParam = new ARPlaneParam();
        public static ARPostData PostData = new ARPostData();
        public static ARPlaneData PlaneData = new ARPlaneData();
        public static DeviceMotionChangeParam deviceMotionChangeParam = new DeviceMotionChangeParam();
        [DllImport("__Internal")]
        public static extern void QGStartARCamera(CameraImageCallback callback);
        [DllImport("__Internal")]
        public static extern void QGDestroyARCamera();
        [DllImport("__Internal")]
        public static extern void QGRequireARCameraImage();
        [DllImport("__Internal")]
        public static extern void QGStartARCameraYuv(CameraImageYuvCallback callback);
        [DllImport("__Internal")]
        public static extern void QGDestroyARCameraYuv();
        [DllImport("__Internal")]
        public static extern void QGRequireARCameraImageYuv();
        [DllImport("__Internal")]
        public static extern void QGCreateOppoARPose(CameraArPoseCallback cameraArPoseCallback);
        [DllImport("__Internal")]
        public static extern void QGRequireARPose();
        [DllImport("__Internal")]
        public static extern void QGCreateOppoARPlane(CameraArPlaneCallback cameraArPlaneCallback);
        [DllImport("__Internal")]
        public static extern void QGRequireARPlane();
        [DllImport("__Internal")]
        private static extern void QGStartDeviceMotionListening(string interval);
        [DllImport("__Internal")]
        private static extern void QGStopDeviceMotionListening();
        [DllImport("__Internal")]
        private static extern void QGOnDeviceMotionChange(DeviceMotionChangeCallback deviceMotionChangeCallback);
        [DllImport("__Internal")]
        private static extern void QGOffDeviceMotionChange();
        [DllImport("__Internal")]
        private static extern void QGGetDeviceMotionChange();

        [MonoPInvokeCallback(typeof(CameraImageCallback))]
        public static void OnARCameraImageCallback(IntPtr ptr, int length, int width, int height)
        {
            CameraParam.ptr = ptr;
            CameraParam.length = length;
            CameraParam.width = width;
            CameraParam.height = height;
            isGetARCameraData = true;
        }

        [MonoPInvokeCallback(typeof(CameraImageYuvCallback))]
        public static void OnARCameraImageYuvCallback(IntPtr ptr, int length)
        {
            CameraYuvParam.ptr = ptr;
            CameraYuvParam.length = length;
            isGetARCameraYuvData = true;
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

        [MonoPInvokeCallback(typeof(Action<string, string, string, string, string, string, string, string>))]
        public static void OnARPlaneCallback(string projectionMatrix, string viewMatrix, string modelMatrix, string modelViewMatrix, string modelViewProjectionMatrix, string planeAngleUvMatrix, string normalVector, string msg)
        {
            PlaneParam.projectionMatrix = projectionMatrix;
            PlaneParam.viewMatrix = viewMatrix;
            PlaneParam.modelMatrix = modelMatrix;
            PlaneParam.modelViewMatrix = modelViewMatrix;
            PlaneParam.modelViewProjectionMatrix = modelViewProjectionMatrix;
            PlaneParam.planeAngleUvMatrix = planeAngleUvMatrix;
            PlaneParam.normalVector = normalVector;
            PlaneParam.msg = msg;
            isGetARPlaneData = true;
        }


        [MonoPInvokeCallback(typeof(Action<string, string, string>))]
        public static void OnDeviceMotionChangeCallback(string alpha, string beta, string gamma)
        {
            deviceMotionChangeParam.alpha = float.Parse(alpha);
            deviceMotionChangeParam.beta = float.Parse(beta);
            deviceMotionChangeParam.gamma = float.Parse(gamma);
            isGetDeviceMotionData = true;
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

        //Ar Yuv相机接口(初始化)
        public static void StartARCameraYuv(Action<string> successCallback, Action<string> failCallback)
        {
            QGMiniGameManager.Instance.AddARCameraYuvCallBack(successCallback, failCallback);
            QGStartARCameraYuv(OnARCameraImageYuvCallback);
        }

        public static void DestroyARCameraYuv()
        {
            QGDestroyARCameraYuv();
        }

        //获取Yuv byte数组指针 长度对象
        public static ARCameraYuvParam GetARCameraYuvParam()
        {
            QGRequireARCameraImageYuv();
            if (!isGetARCameraYuvData)
            {
                Debug.LogError("暂无数据, 请开启YUV相机");
            }
            isGetARCameraYuvData = false;
            return CameraYuvParam;
        }

        //获取Yuv数组
        public static byte[] GetARCameraYuvData()
        {
            QGRequireARCameraImageYuv();
            if (!isGetARCameraYuvData)
            {
                Debug.LogError("暂无数据, 请开启YUV相机");
                return new byte[0];
            }
            isGetARCameraYuvData = false;
            byte[] managedArray = new byte[CameraYuvParam.length];
            Marshal.Copy(CameraYuvParam.ptr, managedArray, 0, CameraYuvParam.length);
            return managedArray;
        }


        //调用位姿接口(初始化)
        public static void CreateOppoARPose()
        {
            QGCreateOppoARPose(OnARPostCallback);
        }

        public static void CreateOppoARPlane()
        {
            QGCreateOppoARPlane(OnARPlaneCallback);
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

        //获取平面检测数据 每n帧执行一次 (成功回调)
        public static ARPlaneData GetRequireARPlane(Action successCallback)
        {
            QGRequireARPlane();
            isGetARPlaneInfo = false;
            if (!isGetARPlaneData)
                return PlaneData;

            PlaneData.projectionMatrix = ParseFloatArray(PlaneParam.projectionMatrix);
            PlaneData.viewMatrix = ParseFloatArray(PlaneParam.viewMatrix);
            PlaneData.modelMatrix = ParseFloatArray(PlaneParam.modelMatrix);
            PlaneData.modelViewMatrix = ParseFloatArray(PlaneParam.modelViewMatrix);
            PlaneData.modelViewProjectionMatrix = ParseFloatArray(PlaneParam.modelViewProjectionMatrix);
            PlaneData.planeAngleUvMatrix = ParseFloatArray(PlaneParam.planeAngleUvMatrix);
            PlaneData.normalVector = ParseFloatArray(PlaneParam.normalVector);
            PlaneData.msg = PlaneParam.msg;

            successCallback?.Invoke(); // 使用 null 条件运算符

            isGetARPlaneData = false;
            isGetARPlaneInfo = true;
            return PlaneData;
        }

        // 辅助方法：将以逗号分隔的字符串解析为 float 数组
        private static float[] ParseFloatArray(string input)
        {
            string[] stringArray = input.Split(',');
            float[] floatArray = new float[stringArray.Length];

            for (int i = 0; i < stringArray.Length; i++)
            {
                floatArray[i] = float.Parse(stringArray[i]);
            }

            return floatArray;
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

        //已获取平面检测数据?
        public static bool IsGetARPlaneInfo()
        {
            return isGetARPlaneInfo;
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


        //设备方向接口 interval 监听设备方向的变化回调函数的执行频率 
        //game	适用于更新游戏的回调频率，在 20ms/次 左右
        //ui	适用于更新 UI 的回调频率，在 60ms/次 左右
        //normal	普通的回调频率，在 200ms/次 左右
        public static void StartDeviceMotionListening(string interval, Action<string> successCallback, Action<string> failCallback)
        {
            QGMiniGameManager.Instance.AddStartDeviceMotionListeningCallBack(successCallback, failCallback);
            QGStartDeviceMotionListening(interval);
        }

        public static void StopDeviceMotionListening(Action<string> successCallback, Action<string> failCallback)
        {
            QGMiniGameManager.Instance.AddStopDeviceMotionListeningCallBack(successCallback, failCallback);
            QGStopDeviceMotionListening();
        }

        public static void OnDeviceMotionChange()
        {
            QGOnDeviceMotionChange(OnDeviceMotionChangeCallback);
        }

        public static void OffDeviceMotionChange()
        {
            QGOffDeviceMotionChange();
        }

        public static DeviceMotionChangeParam GetDeviceMotionChange()
        {
            QGGetDeviceMotionChange();
            if (isGetDeviceMotionData)
            {
                return deviceMotionChangeParam;
            }
            return null;
        }
    }
}
