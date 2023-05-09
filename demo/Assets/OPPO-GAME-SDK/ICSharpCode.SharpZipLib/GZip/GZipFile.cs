using System;
using System.IO;
using System.Threading;
using YZL.Compress.Info;

namespace YZL.Compress.GZip
{
    public class GZipFile
    {
        /**  进度  **/
        public class CodeProgress
        {
            public ProgressDelegate m_ProgressDelegate = null;

            public CodeProgress(ProgressDelegate del)
            {
                m_ProgressDelegate = del;
            }

            public void SetProgress(Int64 inSize, Int64 outSize)
            {
            }

            public void SetProgressPercent(Int64 fileSize, Int64 processSize)
            {
                m_ProgressDelegate(fileSize, processSize);
            }
        }

        /**  异步压缩一个文件  **/
        public static void CompressAsync(string inpath, string outpath, ProgressDelegate progress)
        {
            Thread compressThread = new Thread(new ParameterizedThreadStart(Compress));
            FileChangeInfo info = new FileChangeInfo();
            info.inpath = inpath;
            info.outpath = outpath;
            info.progressDelegate = progress;
            compressThread.Start(info);
        }

        /**  异步解压一个文件  **/
        public static void DeCompressAsync(string inpath, string outpath, ProgressDelegate progress)
        {
            Thread decompressThread = new Thread(new ParameterizedThreadStart(DeCompress));
            FileChangeInfo info = new FileChangeInfo();
            info.inpath = inpath;
            info.outpath = outpath;
            info.progressDelegate = progress;
            decompressThread.Start(info);
        }

        /**  同步压缩一个文件  **/
        private static void Compress(object obj)
        {
            FileChangeInfo info = (FileChangeInfo)obj;
            string inpath = info.inpath;
            string outpath = info.outpath;
            CodeProgress codeProgress = null;
            if (info.progressDelegate != null)
                codeProgress = new CodeProgress(info.progressDelegate);

            ICSharpCode.SharpZipLib.GZip.GZip.Compress(File.OpenRead(inpath), File.Create(outpath), true, codeProgress);

        }

        public static void Compress(string inpath, string outpath, ProgressDelegate progress)
        {
            FileChangeInfo info = new FileChangeInfo();
            info.inpath = inpath;
            info.outpath = outpath;
            info.progressDelegate = progress;
            Compress(info);
        }

        /**  同步解压一个文件  **/
        private static void DeCompress(object obj)
        {
            FileChangeInfo info = (FileChangeInfo)obj;
            string inpath = info.inpath;
            string outpath = info.outpath;
            CodeProgress codeProgress = null;
            if (info.progressDelegate != null)
                codeProgress = new CodeProgress(info.progressDelegate);

            ICSharpCode.SharpZipLib.GZip.GZip.Decompress(File.OpenRead(inpath), File.Create(outpath), true, codeProgress);
        }

        public static void DeCompress(string inpath, string outpath, ProgressDelegate progress)
        {
            FileChangeInfo info = new FileChangeInfo();
            info.inpath = inpath;
            info.outpath = outpath;
            info.progressDelegate = progress;
            DeCompress(info);
        }
    }
}
