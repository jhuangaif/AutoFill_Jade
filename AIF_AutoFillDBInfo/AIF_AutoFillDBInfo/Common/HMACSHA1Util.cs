using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AIFAutoFillDB.Common
{
    class HMACSHA1Util
    {
        public static string CalculateSignature(string text, string secretKey)
        {
            using (var hmacsha1 = new HMACSHA1(Encoding.UTF8.GetBytes(secretKey)))
            {
                byte[] hashmessage = hmacsha1.ComputeHash(Encoding.UTF8.GetBytes(Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(text))));
                return Convert.ToBase64String(hashmessage);
            }
        }
    }
}
