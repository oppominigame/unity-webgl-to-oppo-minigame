using System;
using System.IO;
using YZL.Compress.GZip;

namespace ICSharpCode.SharpZipLib.GZip
{
    public static class GZip
    {
        public static void Decompress(Stream inStream, Stream outStream, bool isStreamOwner,GZipFile.CodeProgress progress)
        {
            if (inStream == null || outStream == null)
            {
                throw new Exception("Null Stream");
            }

            try
            {
                using (GZipInputStream bzipInput = new GZipInputStream(inStream))
                {
                    bzipInput.IsStreamOwner = isStreamOwner;
                    ICSharpCode.SharpZipLib.Core.StreamUtils.Copy(bzipInput, outStream, new byte[4096], progress);
                }
            }
            finally
            {
                if (isStreamOwner)
                {
                    // inStream is closed by the GZipInputStream if stream owner
                    outStream.Close();
                }
            }
        }

        public static void Compress(Stream inStream, Stream outStream, bool isStreamOwner, GZipFile.CodeProgress progress)
        {
            if (inStream == null || outStream == null)
            {
                throw new Exception("Null Stream");
            }

            try
            {
                using (GZipOutputStream bzipOutput = new GZipOutputStream(outStream))
                {
                    bzipOutput.IsStreamOwner = isStreamOwner; ;
                    ICSharpCode.SharpZipLib.Core.StreamUtils.Copy(inStream, bzipOutput, new byte[4096], progress);
                }
            }
            finally
            {
                if (isStreamOwner)
                {
                    // outStream is closed by the GZipOutputStream if stream owner
                    inStream.Close();
                }
            }
        }

    }
}
