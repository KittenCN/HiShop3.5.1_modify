namespace Hidistro.Core.Encrypt
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public class Hash
    {
        public static string DESDecrypt(string encryptedValue, string key)
        {
            return DESDecrypt(encryptedValue, key, key);
        }

        public static string DESDecrypt(string encryptedValue, string key, string IV)
        {
            key = key + "12345678";
            IV = IV + "12345678";
            key = key.Substring(0, 8);
            IV = IV.Substring(0, 8);
            ICryptoTransform transform = new DESCryptoServiceProvider { Key = Encoding.UTF8.GetBytes(key), IV = Encoding.UTF8.GetBytes(IV) }.CreateDecryptor();
            byte[] buffer = Convert.FromBase64String(encryptedValue);
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Write);
            stream2.Write(buffer, 0, buffer.Length);
            stream2.FlushFinalBlock();
            stream2.Close();
            return Encoding.UTF8.GetString(stream.ToArray());
        }

        public static string DESEncrypt(string originalValue, string key)
        {
            return DESEncrypt(originalValue, key, key);
        }

        public static string DESEncrypt(string originalValue, string key, string IV)
        {
            key = key + "12345678";
            IV = IV + "12345678";
            key = key.Substring(0, 8);
            IV = IV.Substring(0, 8);
            ICryptoTransform transform = new DESCryptoServiceProvider { Key = Encoding.UTF8.GetBytes(key), IV = Encoding.UTF8.GetBytes(IV) }.CreateEncryptor();
            byte[] bytes = Encoding.UTF8.GetBytes(originalValue);
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Write);
            stream2.Write(bytes, 0, bytes.Length);
            stream2.FlushFinalBlock();
            stream2.Close();
            return Convert.ToBase64String(stream.ToArray());
        }

        private static byte[] GetKeyByteArray(string strKey)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] buffer = new byte[strKey.Length - 1];
            return encoding.GetBytes(strKey);
        }

        private static string getstrIN(string strIN)
        {
            if (!string.IsNullOrEmpty(strIN))
            {
                return strIN;
            }
            return "~NULL~";
        }

        private static string GetStringValue(byte[] Byte)
        {
            string str = "";
            for (int i = 0; i < Byte.Length; i++)
            {
                str = str + Byte[i].ToString();
            }
            return str;
        }

        public static string MD5(string strIN)
        {
            System.Security.Cryptography.MD5 md = new MD5CryptoServiceProvider();
            byte[] @byte = md.ComputeHash(GetKeyByteArray(getstrIN(strIN)));
            md.Clear();
            return GetStringValue(@byte);
        }

        public static string MD5(string strIN, string strKey)
        {
            return MD5(strIN, strKey, "utf-8");
        }

        public static string MD5(string strIN, string strKey, string encoding)
        {
            byte[] buffer = new HMACMD5 { Key = Encoding.GetEncoding(encoding).GetBytes(strKey) }.ComputeHash(Encoding.GetEncoding(encoding).GetBytes(strIN));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < buffer.Length; i++)
            {
                builder.Append(buffer[i].ToString("x").PadLeft(2, '0'));
            }
            return builder.ToString();
        }

        public static string SHA1Encrypt(string strIN)
        {
            SHA1 sha = new SHA1CryptoServiceProvider();
            byte[] @byte = sha.ComputeHash(GetKeyByteArray(strIN));
            sha.Clear();
            return GetStringValue(@byte);
        }

        public static string SHA256Encrypt(string strIN)
        {
            SHA256 sha = new SHA256Managed();
            byte[] @byte = sha.ComputeHash(GetKeyByteArray(strIN));
            sha.Clear();
            return GetStringValue(@byte);
        }

        public static string SHA512Encrypt(string strIN)
        {
            SHA512 sha = new SHA512Managed();
            byte[] @byte = sha.ComputeHash(GetKeyByteArray(strIN));
            sha.Clear();
            return GetStringValue(@byte);
        }
    }
}

