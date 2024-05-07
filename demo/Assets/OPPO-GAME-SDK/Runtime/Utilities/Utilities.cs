using System.Text;
using UnityEngine;

namespace QGMiniGame
{
    public static class Utilities
    {
        public static string MD5(this byte[] data)
        {
            byte[] hashBytes;
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                hashBytes = md5.ComputeHash(data);
            }
            var sb = new StringBuilder();
            foreach (var hash in hashBytes)
            {
                sb.Append(hash.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}