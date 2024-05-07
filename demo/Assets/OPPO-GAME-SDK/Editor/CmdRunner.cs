using System.Diagnostics;
using UnityEngine;

namespace QGMiniGame
{
    public static class CmdRunner
    {
        public static Process CreateShellExProcess(string cmd, string args, string cwd = "", bool noWindow = true)
        {
            var pStartInfo = new ProcessStartInfo(cmd)
            {
                Arguments = args,
                CreateNoWindow = noWindow,
                UseShellExecute = false,
                RedirectStandardError = false,
                RedirectStandardInput = false,
                RedirectStandardOutput = true
            };
            if (cwd.IsValid())
            {
                pStartInfo.WorkingDirectory = cwd;
            }
            return Process.Start(pStartInfo);
        }

        public static void RunBat(string batfile, string args, string workingDir = "")
        {
            var p = CreateShellExProcess(batfile, args, workingDir);
            p.Close();
        }

        public static string FormatPath(string path)
        {
            path = path.Replace("/", "\\");
            if (Application.platform == RuntimePlatform.OSXEditor)
                path = path.Replace("\\", "/");
            return path;
        }
    }
}
