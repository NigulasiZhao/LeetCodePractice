using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace GISWaterSupplyAndSewageServer.CommonTools
{
    public static class Sha1Helper
    {
        /// <summary>
        /// 基于Sha1的自定义加密字符串方法：输入一个字符串，返回一个由40个字符组成的十六进制的哈希散列（字符串）。
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <returns>加密后的十六进制的哈希散列（字符串）</returns>
        public static string Encryption(this string str)
        {
            var buffer = Encoding.UTF8.GetBytes(str);
            var data = SHA1.Create().ComputeHash(buffer);

            var sb = new StringBuilder();
            foreach (var t in data)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString();
        }
        public static string GetHash(string path)
        {
            //var hash = SHA256.Create();
            //var hash = MD5.Create();
            var hash = SHA1.Create();
            var stream = new FileStream(path, FileMode.Open);
            byte[] hashByte = hash.ComputeHash(stream);
            stream.Close();
            return BitConverter.ToString(hashByte).Replace("-", "");
        }
    }
}
