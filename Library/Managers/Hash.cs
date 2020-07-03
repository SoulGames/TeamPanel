using BCrypt.Net;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Library.Managers
{
    public class Hash
    {
        public static string CalculateMD5(string text)
        {
            byte[] TextBytes = Encoding.ASCII.GetBytes(text);

            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(TextBytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }

        public static string CalculateBCryptPassword(string text)
        {
            return BCrypt.Net.BCrypt.HashPassword(text);
        }
        public static string CalculateBCryptString(string text)
        {
            return BCrypt.Net.BCrypt.HashString(text);
        }
    }
}
