using System;
using System.Security.Cryptography;
using System.Text;

namespace SirenMapper
{
    public static class StringExtensions
    {
        public static string Sha1(this string input)
        {
            var provider = new SHA1CryptoServiceProvider();
            var encoding = new UnicodeEncoding();
            var bytehash = provider.ComputeHash(encoding.GetBytes(input));
            return Convert.ToBase64String(bytehash);
        }
    }
}