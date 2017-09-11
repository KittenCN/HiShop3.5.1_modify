namespace Hidistro.Core
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Security.Cryptography;
    using System.Text;

    public sealed class HiCryptographer
    {
        private static byte[] CreateHash(byte[] plaintext)
        {
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            return provider.ComputeHash(plaintext);
        }

        public static string CreateHash(string plaintext)
        {
            byte[] buffer = CreateHash(Encoding.ASCII.GetBytes(plaintext));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < buffer.Length; i++)
            {
                builder.Append(buffer[i].ToString("x2"));
            }
            return builder.ToString();
        }

        public static string Decrypt(string text)
        {
            using (RijndaelManaged managed = new RijndaelManaged())
            {
                managed.Key = Convert.FromBase64String(ConfigurationManager.AppSettings["Key"]);
                managed.IV = Convert.FromBase64String(ConfigurationManager.AppSettings["IV"]);
                ICryptoTransform transform = managed.CreateDecryptor();
                byte[] inputBuffer = Convert.FromBase64String(text);
                byte[] bytes = transform.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
                transform.Dispose();
                return Encoding.UTF8.GetString(bytes);
            }
        }

        public static string Encrypt(string text)
        {
            using (RijndaelManaged managed = new RijndaelManaged())
            {
                managed.Key = Convert.FromBase64String(ConfigurationManager.AppSettings["Key"]);
                managed.IV = Convert.FromBase64String(ConfigurationManager.AppSettings["IV"]);
                ICryptoTransform transform = managed.CreateEncryptor();
                byte[] bytes = Encoding.UTF8.GetBytes(text);
                byte[] inArray = transform.TransformFinalBlock(bytes, 0, bytes.Length);
                transform.Dispose();
                return Convert.ToBase64String(inArray);
            }
        }

        public static string Md5Encrypt(string sourceData)
        {
            string str3;
            Encoding encoding = new UTF8Encoding();
            byte[] bytes = encoding.GetBytes("12345678");
            byte[] rgbIV = new byte[] { 1, 2, 3, 4, 5, 6, 8, 7 };
            string s = sourceData;
            try
            {
                ICryptoTransform transform = new DESCryptoServiceProvider().CreateEncryptor(bytes, rgbIV);
                byte[] inputBuffer = encoding.GetBytes(s);
                str3 = Convert.ToBase64String(transform.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length));
            }
            catch
            {
                throw;
            }
            return str3;
        }

        public static string SignTopRequest(IDictionary<string, string> parameters, string appSecret)
        {
            IDictionary<string, string> dictionary = new SortedDictionary<string, string>(parameters, StringComparer.Ordinal);
            IEnumerator<KeyValuePair<string, string>> enumerator = dictionary.GetEnumerator();
            StringBuilder builder = new StringBuilder();
            while (enumerator.MoveNext())
            {
                KeyValuePair<string, string> current = enumerator.Current;
                string key = current.Key;
                KeyValuePair<string, string> pair2 = enumerator.Current;
                string str2 = pair2.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(str2))
                {
                    builder.Append(key).Append(str2);
                }
            }
            if (!string.IsNullOrEmpty(appSecret))
            {
                builder.Append(appSecret);
            }
            byte[] buffer = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(builder.ToString()));
            StringBuilder builder2 = new StringBuilder();
            for (int i = 0; i < buffer.Length; i++)
            {
                builder2.Append(buffer[i].ToString("X2"));
            }
            return builder2.ToString();
        }
    }
}

