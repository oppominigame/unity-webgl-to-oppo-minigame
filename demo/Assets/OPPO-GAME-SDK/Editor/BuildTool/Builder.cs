using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace QGMiniGame
{
    using Debug = UnityEngine.Debug;

    public static class Builder
    {
        public class ValidateParameterData
        {
            public string target;
            public string key;
            public bool allowEmpty;
            public string emptyError = "不能为空";
            public string pattern = "";
            public string invalidError = "校验失败";
            public Func<bool> customFunc = null;
        }

        private static readonly Dictionary<string, string> parameterValidationMap = new Dictionary<string, string>();

        public static bool IsAllValidationPass
        {
            get
            {
                var validateValues = parameterValidationMap.Values;
                foreach (var value in validateValues)
                {
                    // 任意1项有错误内容，则验证失败
                    if (value.IsValid())
                    {
                        return false;
                    }
                }
                // 全部验证通过
                return true;
            }
        }

        public static Dictionary<string, string>.KeyCollection ValidationKeys => parameterValidationMap.Keys;

        public static void InitValidation(string key)
        {
            if (parameterValidationMap.ContainsKey(key))
            {
                return;
            }
            parameterValidationMap.Add(key, string.Empty);
        }

        /// <summary>
        /// 校验参数
        /// </summary>
        /// <param name="data"></param>
        /// <returns>成功返回空字符串，错误返回具体错误信息</returns>
        public static string Validate(ValidateParameterData data)
        {
            // 校验输入为空
            if (!data.target.IsValid() && !data.allowEmpty)
            {
                parameterValidationMap[data.key] = data.emptyError;
            }
            // 校验传入的方法
            else if (data.customFunc != null)
            {
                parameterValidationMap[data.key] = data.customFunc() ? string.Empty : data.invalidError;
            }
            // 校验正则表达式
            else if (data.target.IsValid() && data.pattern.IsValid())
            {
                parameterValidationMap[data.key] = Regex.IsMatch(data.target, data.pattern) ? string.Empty : data.invalidError;
            }
            // 其他情况都校验通过
            else
            {
                parameterValidationMap[data.key] = string.Empty;
            }
            return parameterValidationMap[data.key];
        }

        public static void ModifyValidation(string key, string value)
        {
            if (!parameterValidationMap.ContainsKey(key))
            {
                return;
            }
            parameterValidationMap[key] = value;
        }

        public static bool GetValidation(string key, out string error)
        {
            return parameterValidationMap.TryGetValue(key, out error);
        }

        public static bool Build()
        {
            var invalidList = GetAllInvalidParameters();
            if (invalidList.Count > 0)
            {
                Debug.LogError("未配置关键参数或参数不合法，请根据日志前往【OPPO小游戏】->【打包工具】进行正确配置后重试");
                foreach (var pair in invalidList)
                {
                    Debug.LogError($"{pair.Key}: {pair.Value}");
                }
                return false;
            }
            return QGGameTools.BuildGame();
        }

        private static List<KeyValuePair<string, string>> GetAllInvalidParameters()
        {
            var retList = new List<KeyValuePair<string, string>>();
            foreach (var pair in parameterValidationMap)
            {
                if (pair.Value.IsValid())
                {
                    retList.Add(new KeyValuePair<string, string>(pair.Key, pair.Value));
                }
            }
            return retList;
        }
    }
}
