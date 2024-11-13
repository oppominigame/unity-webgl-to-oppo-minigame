using System;
using UnityEngine;


namespace QGMiniGame
{
    [Serializable]
    public class QGBaseResponse
    {
        public string callbackId;
        public string errMsg;
        public string errCode;

    }

    [Serializable]
    public class QGCommonResponse<H> : QGBaseResponse
    {
        [SerializeField] public H data;
    }
    [Serializable]
    public class QGInstallShortcutBean : QGBaseResponse
    {
        public string code;
        public string data;
    }
    [Serializable]
    public class QGHasShortcutInstalledBean : QGBaseResponse
    {
        public bool hasShortcutInstalled; // 是否创建桌面图标
    }
    [Serializable]
    public class QGOnNetworkStatus : QGBaseResponse
    {
        public string isConnected;
        public string networkType;
    }
    [Serializable]
    public class QGResKeyBoardponse : QGBaseResponse
    {
        public string callbackId;
        public string errMsg;
        public string errCode;
        public string value;
        public string keyboardId;
    }

    [Serializable]
    public class QGProviderRponse
    {
        public string provider;  //渠道信息
    }

    [Serializable]
    public class QGManifestInfoRponse
    {
        public string package;  //游戏包名
        public string name;     //游戏名
        public string versionName; //游戏版本名
        public string versionCode; //游戏版本号
        public string minPlatformVersion; //最小平台版本号
        public string icon; //桌面图标
        public string orientation; //设备方向
        public string type; //不填或者默认值为 app，取值为 app 或 game
        public object config; //logLevel 取值
        public object subpackages; //分包功能，有分包时才需要，可选字段
    }

    [Serializable]
    public class QGLoginBean
    {   //public string userId;   //无返回
        //public string userName;  //无返回
        public string avatar;
        public string sex;
        //public string location;  //无返回
        //public string constellation;  //无返回
        public string age;
        public string token; //调用接口获取登录凭证（token）。通过凭证进而换取用户登录态信息，包括用户的唯一标识（openid）
        public string nickName;
        public string uid;
        public string time;
        public string code;
        public string phoneNum;
    }



    [Serializable]
    public class QGPayBean
    {
        public string orderId; // 商户订单号ID
        public string code; //错误码
    }

    [Serializable]
    public class QGPayBeanFail
    {
        public string data; // 错误信息和错误码
        public string code; //错误码
    }

    // 获取网络状态 
    [Serializable]
    public class QGGetNetworkType
    {
        public string networkType; // 网络类型
                                   // public string isConnected; // 当前是否有网络链接
    }

    // 获取系统信息
    [Serializable]
    public class QGSystemInfo
    {
        public string brand; // 手机品牌
        public string language; // 系统语言
        public string model; // 手机型号
        public string statusBarHeight; // 状态栏/异形缺口高度
        public string pixelRatio; // 设备像素比
        public string platformVersionName; // 客户端平台
        public string platformVersionCode; // 网络类型
        public string screenHeight; // 屏幕高度
        public string screenWidth; // 屏幕宽度
        public string system; // 系统版本
        public string windowHeight; // 可使用窗口高度
        public string windowWidth; // 可使用窗口宽度
        public string theme; // 系统当前主题
        public string deviceOrientation; // 设备方向
        public string COREVersion; // 版本号
    }
    // [Serializable]
    // public class QGFileBean
    // {
    //     public bool hasShortcutInstalled; // 是否创建桌面图标
    // }

    // [Serializable]
    // public class QGUserInfoBean
    // {
    //     public string nickName; // 用户昵称
    //     public string smallAvatar; // 用户社区小头像
    //     public string biggerAvatar; // 用户社区大头像
    //     public int gender; //性别：0，保密；1，男；2，女
    // }



    [Serializable]
    public class QGFileResponse : QGBaseResponse
    {
        public string textStr;  // 读取的文本
        public byte[] textData; // 读取的二进制数据
        public string encoding;
        public int byteLength;
    }

    [Serializable]
    public class QGRewardedVideoResponse : QGBaseResponse
    {
        [SerializeField] public bool isEnded; // 视频是否是在用户完整观看的情况下被关闭的，true 表示用户是在视频播放完以后关闭的视频，false 表示用户在视频播放过程中关闭了视频
    }

    [Serializable]
    public class NativeItemBean
    {
        public string adId; // 广告标识，用来上报曝光与点击
        public string title; // 广告标题
        public string desc; // 广告描述
        public string icon; // 推广应用的Icon图标
        public string[] imgUrlList; // 广告图片，建议使用该图片资源
        public string logoUrl; // 广告标签图片
        public string clickBtnTxt; // 点击按钮文本描述
        public int creativeType; // 获取广告类型，取值说明：0：混合
        public int interactionType; // 获取广告点击之后的交互类型，取值说明： 1：网址类 2：应用下载类 8：快应用生态应用
    }

    public class QGNativeReportParam
    {
        public string adId; //广告位id
    }

    [Serializable]
    public class QGKeyboardInputResponse : QGBaseResponse
    {
        public string value;
    }


    public class QGCommonAdParam
    {
        public string posId; //广告位id（必填 非常重要）
        public string adUnitId; //广告位id（必填 非常重要）
    }

    public class QGCreateCustomAdParam : QGCommonAdParam
    {
        public Style style; // 广告位置
    }

    public class QGCreateBannerAdParam : QGCommonAdParam
    {
        public string adUnitId; //广告位id（必填 非常重要）
        // public int adIntervals; // 刷新时间
        public Style style; // 广告位置

    }

    public class QGCreateGameDrawerAdParam : QGCommonAdParam
    {
        public string adUnitId; //广告位id（必填 非常重要）
        public Style style;
    }

    public class Style
    {
        public int left;
        public int top;
    }


    //正式支付接口, 该统一下单接口只针对 https://jits.open.oppomobile.com/jitsopen/api/pay/v1.0/preOrder
    public class PayParam
    {
        //必填
        public int appId; // 平台分配的游戏 appId
        public string openId;  //qg.login 成功时获得的用户 token
        public long timestamp; //时间戳，当前计算机时间和GMT时间(格林威治时间)1970年1月1号0时0分0秒所差的毫秒数
        public string orderNo; //下单生成的预付订单号
        public string paySign; //支付签名，CP 服务端生成
    }

    //示例参数，CP请勿使用
    public class PayTestParam
    {
        //必填
        public int appId; // 平台分配的游戏 appId
        public string openId; // qg.login 成功时获得的用户 token
        public long timestamp; //时间戳，当前计算机时间和GMT时间(格林威治时间)1970年1月1号0时0分0秒所差的毫秒数
        /// <summary>
        /// sign 签名详细见
        /// https://ie-activity-cn.heytapimage.com/static/minigame/CN/docs/index.html#/develop/pay/order
        /// 辅助工具选择代码生成 https://ie-activity-cn.heytapimage.com/static/minigame/CN/docs/index.html#/develop/pay/pay-tool
        /// </summary>
        public string sign; //签名 
        public string productName; //商品名称 
        public string productDesc; //商品描述 
        public int count; //商品数量（只能传1） 
        public int price; //商品价格，以分为单位
        public string currency; //币种，人民币如：CNY
        public string callBackUrl; // 服务器接收平台返回数据的接口回调地址
        public string cpOrderId; //CP自己的订单号
        public string appVersion; //游戏版本
        public string engineVersion; //快应用引擎版本(通过 getSystemInfo 获取 platformVersionCode)
        //可不填
        public string deviceInfo; //设备号 
        public string model; //机型 
        public string ip; //终端IP 
        public string attach; //附加信息
    }

    public class QGAccessFileParam
    {
        public string path; //判断是否存在的文件/目录路径
    }

    public class QGFileParam
    {

        public string uri; //需要读取的本地文件uri，不能是tmp类型的uri
        public int position = 0; //读取二进制数据的起始位置，默认值为文件的起始位置
        public int length = int.MaxValue; //读取二进制的长度，不填写则读取到文件结尾
        public string textStr;  // 写入的文本
        public byte[] textData; // 写入的二进制数据


        public string filePath; // 要写入的文件路径
        public string encoding = "utf8"; //指定写入文件的字符编码 utf8 or binary，默认值为 utf8
        public object data;  // 要写入的文本或二进制数据
        public string append; // 默认为 false，覆盖旧文件
    }

    public class KeyboardParam
    {
        public string defaultValue; //键盘输入框显示的默认值
        public int maxLength; //键盘中文本的最大长度
        public bool multiple; //是否为多行输入
        public bool confirmHold; //当点击完成时键盘是否收起
        public string confirmType; //键盘右下角confirm按钮类型，只影响按钮的文本内容
    }


    public class ReplaceRule
    {
        public string oldStr;
        public string newStr;
    }

    [Serializable]
    public class VideoParam
    {
        public int ParamX;
        public int ParamY;
        public int ParamWidth;
        public int ParamHeight;
        public string url;
        public string poster;
    }

    [Serializable]
    public class DownLoadFileParam
    {
        public string path;
        public string url;
    }

    [Serializable]
    public class UpLoadFileParam
    {
        public string path; //"/test.png"
        public string url;  //"http://example.com/resource";
        public string name; //wgb
    }


    [Serializable]
    public class ShowToastParam
    {
        public string title;
        public string iconType;
        public int durationTime;
    }

    [Serializable]
    public class AudioParam
    {
        public string url;

        public float startTime;

        public bool loop;

        public float volume = 1f;

    }

    [Serializable]
    public class ExitApplicationParam
    {
        public string data;
    }

    [Serializable]
    public class ShowModalParam
    {
        public string title;
        public string content;
        public bool showCancel = true;
        public string cancelText = "取消";
        public string cancelColor = "#FFCB1B";
        public string confirmText = "确定";
        public string confirmColor = "#FFCB1B";
    }

    [Serializable]
    public class ShowModalResponse
    {
        public bool confirm; //确定
        public bool cancel; //取消
    }

    [Serializable]
    public class ARCameraParam
    {
        public IntPtr ptr;  //Ar相机返回对象内存地址
        public int length;  //byte数组长度
        public int width;   //摄像头画面宽度
        public int height;  //摄像头画面高度
    }

    [Serializable]
    public class ARCameraYuvParam
    {
        public IntPtr ptr;  //Yuv Ar相机返回对象内存地址
        public int length;  //Yuv byte数组长度
    }

    [Serializable]
    public class ARPostParam
    {
        public IntPtr arpost;  //位姿坐标内存地址
        public IntPtr arrotation;   //位姿旋转内存地址
        public int modelMatrixArrayLen;  //位姿坐标数组长度
        public int modelViewMatrixArrayLen; //位姿旋转数组长度
        public string aRPotStr;    //JS数据转成的字符串
        public string aRRotStr;    //JS数据转成的字符串
    }

    [Serializable]
    public class ARPostData
    {
        public float posX = 0;  //坐标X
        public float posY = 0;   //坐标Y
        public float posZ = 0;  //坐标Z
        public float rotX = 0; //四元数X
        public float rotY = 0; //四元数Y
        public float rotZ = 0; //四元数Z
        public float rotW = 0; //四元数W
    }

    [Serializable]
    public class UserCloudStorageParam
    {
        public string key;  //云储存 key
        public string value;//云储存 value
    }

    [Serializable]
    public class BatteryInfoParam
    {
        public float level; //设备电量，范围 1 - 100
        public bool isCharging; //是否正在充电中
    }

    [Serializable]
    public class DeviceIdParam
    {
        public string deviceId; //设备唯一标识
    }

    [Serializable]
    public class ScreenBrightnessParam
    {
        public float value; //屏幕亮度
    }

    [Serializable]
    public class GetLocationParam
    {
        public float latitude;  //纬度，范围为 -90~90，负数表示南纬
        public float longitude; //经度，范围为 -180~180，负数表示西经
        public float speed;     //速度，单位 m/s
        public float accuracy;  //位置的精确度
        public float altitude;  //高度，单位 m
        public float verticalAccuracy;  //垂直精度，单位 m（Android 无法获取，返回 0）
        public float horizontalAccuracy;//水平精度，单位 m
    }

    [Serializable]
    public class onAccelerometerChangeParam
    {
        public float QgParamX;  //x 轴
        public float QgParamY;  //y 轴
        public float QgParamZ;  //z 轴

    }

    [Serializable]
    public class DeviceMotionChangeParam
    {
        public float alpha;
        public float beta;
        public float gamma;
    }

    [Serializable]
    public class ARPlaneParam
    {
        public string projectionMatrix;
        public string viewMatrix;
        public string modelMatrix;
        public string modelViewMatrix;
        public string modelViewProjectionMatrix;
        public string planeAngleUvMatrix;
        public string normalVector;
        public string msg;
    }

    [Serializable]
    public class ARPlaneData
    {
        public float[] projectionMatrix = new float[] { }; //投影矩阵(摄像头)
        public float[] viewMatrix = new float[] { };       //视图矩阵(摄像头)
        public float[] modelMatrix = new float[] { };      //模型矩阵
        public float[] modelViewMatrix = new float[] { };  //模型视图矩阵
        public float[] modelViewProjectionMatrix = new float[] { }; //模型视图投影矩阵
        public float[] planeAngleUvMatrix = new float[] { };  //平面角矩阵
        public float[] normalVector = new float[] { }; //向量矩阵
        public string msg = ""; //响应消息
    }

    [Serializable]
    public class ReadFileResponse : QGBaseResponse
    {
        public string encoding;     //读取文件编码
        public string dataUtf8;     //读取文件UTF8
        public byte[] dataBytes;    //读取文件字节
    }

    [Serializable]
    public class ReadDirResponse : QGBaseResponse
    {
        public string filesStr; //文件列表字符串
        public string[] files;  //文件列表字符串数组
    }

    [Serializable]
    public class StatResponse : QGBaseResponse
    {
        public int mode; //文件 mode
        public int size; //文件大小
        public int lastAccessedTime; //最后一次读取的时间
        public int lastModifiedTime; //最后一次修改时间
        public bool isDirectory; //判断当前文件是否一个目录
        public bool isFile; //判断当前文件是否一个普通文件
    }

    [Serializable]
    public class ReadFileParam
    {
        public IntPtr ptr;  //ReadFile 返回对象内存地址
        public int length;  //ReadFile byte数组长度
        public string readFileUtf8;
    }

}
