using UnityEngine;
using System;
using System.Collections;
using System.Diagnostics;
using System.Text;

namespace QGMiniGame
{
    using Debug = UnityEngine.Debug;

    public static class ShellHelper
    {
        private static readonly Encoding GB2312 = Encoding.GetEncoding("GB2312");

        /// <summary>
        /// 执行 shell 命令
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="cwd">当前工作目录</param>
        /// <returns>成功的情况下返回控制台输出，失败抛异常</returns>
        /// <exception cref="Exception"></exception>
        public static string ExecuteCommand(string command, string cwd = "")
        {
            // Windows 使用 cmd.exe，MacOS 使用 /bin/bash
            var isWindows = Application.platform == RuntimePlatform.WindowsEditor;
            var shell = isWindows ? "cmd.exe" : "/bin/bash";
            var args = isWindows ? $"/c \"{command}\"" : $"-c \"{command}\"";
            // 创建进程
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = shell,
                    Arguments = args,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    StandardOutputEncoding = GB2312,
                    StandardErrorEncoding = GB2312,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            // 设置当前工作目录
            if (!string.IsNullOrEmpty(cwd))
            {
                process.StartInfo.WorkingDirectory = cwd;
            }
            // 附加用户自己配置的环境变量
            if (!string.IsNullOrEmpty(BuildConfigAsset.OtherSettingsConfig.environmentVariablePath))
            {
                var separator = isWindows ? ";" : ":";
                var envPathName = isWindows ? "Path" : "PATH";
                var envPath = process.StartInfo.EnvironmentVariables[envPathName];
                if (!envPath.EndsWith(separator))
                {
                    envPath += separator;
                }
                envPath += BuildConfigAsset.OtherSettingsConfig.environmentVariablePath;
                process.StartInfo.EnvironmentVariables[envPathName] = envPath;
            }
            // 启动进程并获取输出
            process.Start();
            var output = process.StandardOutput.ReadToEnd();
            var error = process.StandardError.ReadToEnd();
            process.WaitForExit();
            // 兼容 error 其实是警告的情况
            if (error.Contains("npm warn") ||
                error.Contains("Warning:"))
            {
                error = string.Empty;
            }
            // 返回执行结果
            if (!string.IsNullOrEmpty(error))
            {
                // 处理找不到命令的情况
                Debug.LogError($"{args} failed: {error}");
                if (error.Contains("不是内部或外部命令") ||
                    error.Contains("is not recognized as an internal or external command") ||
                    error.Contains("command not found"))
                {
                    // 找不到 quickgame 可能是用户未安装，或环境变量 Path 获取失败，则给予用户相应提示
                    if (error.Contains("quickgame"))
                    {
                        Debug.LogError("未安装 @oppo-minigame/cli 或无法正确获取 node 所在目录的环境变量，请通过命令行输入 \"quickgame -V\" 确认输出版本号代表已安装\n若未安装，请点击面板右下角\"升级版本\"按钮，或通过命令行输入 \"npm i @oppo-minigame/cli -g\"进行安装\n若安装后仍获取版本失败，请将 node 所在路径手动配置到\"其他设置 -> 环境变量 Path\"后重试");
                    }
                    else
                    {
                        Debug.LogError("无法正确获取环境变量，请手动配置\"其他设置 -> 环境变量 Path\"后重试");
                    }
                }
                throw new Exception(error);
            }
            return output;
        }
    }
}
