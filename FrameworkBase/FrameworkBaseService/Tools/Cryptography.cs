using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace FrameworkBaseService.Tools
{
    public static class Cryptography
    {
        public static string GetMD5Hash(string salt, string inputString)
        {
            salt += "0123456789";//My Private Key
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] digest = md5.ComputeHash(Encoding.UTF8.GetBytes(inputString + salt));
            string base64digest = Convert.ToBase64String(digest, 0, digest.Length);

            return base64digest.Substring(0, base64digest.Length - 2);
        }
    }
}