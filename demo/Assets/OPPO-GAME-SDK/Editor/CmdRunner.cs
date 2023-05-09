using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static System.Net.Mime.MediaTypeNames;

namespace QGMiniGame
{
    public class CmdRunner
    {
        public static System.Diagnostics.Process CreateShellExProcess(string cmd, string args, string workingDir = "")
        {
            var pStartInfo = new System.Diagnostics.ProcessStartInfo(cmd);
            pStartInfo.Arguments = args;
            pStartInfo.CreateNoWindow = false;
            pStartInfo.UseShellExecute = true;
            pStartInfo.RedirectStandardError = false;
            pStartInfo.RedirectStandardInput = false;
            pStartInfo.RedirectStandardOutput = false;
            if (!string.IsNullOrEmpty(workingDir))
                pStartInfo.WorkingDirectory = workingDir;
            return System.Diagnostics.Process.Start(pStartInfo);
        }

        public static void RunBat(string batfile, string args, string workingDir = "")
        {
            var p = CreateShellExProcess(batfile, args, workingDir);
            p.Close();
        }

        public static string FormatPath(string path)
        {
            path = path.Replace("/", "\\");
            if (UnityEngine.Application.platform == RuntimePlatform.OSXEditor)
                path = path.Replace("\\", "/");
            return path;
        }
    }
}
