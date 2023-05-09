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
        public string result; //商户订单号
        public string code; //错误码
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
    public class QGShortcutBean
    {
        public bool hasShortcutInstalled; // 是否创建桌面图标
    }

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

    public class PayParam
    {
        public string appId; // 平台分配的游戏 appId
        public string token; // qg.login 成功时获得的用户 token
        public long timestamp; // 时间戳，CP 服务端参与签名的时间戳
        public string orderNo; // 下单订单号，由统一下单接口返回
        public string paySign; // 支付签名，CP 服务端生成
        
    }

    // 数据存储
    // public class StorageParam
    // {
    //   public string keyName; // 字符串，要创建或更新的键名
    //   public string keyValue; // 要创建或更新的键名对应的值。
    // }
    
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


    public class ReplaceRule
    {
        public string oldStr;
        public string newStr;
    }

}
