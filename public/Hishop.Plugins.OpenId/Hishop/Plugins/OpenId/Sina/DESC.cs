namespace Hishop.Plugins.OpenId.Sina
{
    using System;
    using System.IO;
    using System.Security.Cryptography;

    public static class DESC
    {
        private static byte[] IV_192 = new byte[] { 
            0x37, 0x67, 0xf6, 0x4f, 0x24, 0x63, 0xa7, 3, 0x2a, 5, 0x3e, 0x53, 0xb8, 7, 0xd1, 13, 
            0x91, 0x17, 200, 0x3a, 0xad, 10, 0x79, 0xb5
         };
        private static byte[] IV_64 = new byte[] { 0x63, 0x67, 0xf6, 0x4f, 0x24, 0x63, 0xa7, 3 };
        private static byte[] KEY_192 = new byte[] { 
            0x2a, 0x10, 0x5d, 0x9c, 0x4e, 4, 0xda, 0x20, 15, 0xa7, 0x2c, 80, 0x1a, 250, 0x9b, 0x70, 
            2, 0x5e, 11, 0xcc, 0x77, 0x23, 0xb8, 0xc2
         };
        private static byte[] KEY_64 = new byte[] { 0x21, 0x10, 0x5d, 0x9c, 0x4e, 4, 0xda, 0x20 };

        public static string Decrypt(string value)
        {
            if ((value == null) || (value == ""))
            {
                return string.Empty;
            }
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            MemoryStream stream = new MemoryStream(Convert.FromBase64String(value));
            CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(KEY_64, IV_64), CryptoStreamMode.Read);
            StreamReader reader = new StreamReader(stream2);
            return reader.ReadToEnd();
        }

        public static string DecryptTripleDES(string value)
        {
            if ((value == null) || (value == ""))
            {
                return string.Empty;
            }
            TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider();
            MemoryStream stream = new MemoryStream(Convert.FromBase64String(value));
            CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(KEY_192, IV_192), CryptoStreamMode.Read);
            StreamReader reader = new StreamReader(stream2);
            return reader.ReadToEnd();
        }

        public static string Encrypt(string value)
        {
            if ((value == null) || (value == ""))
            {
                return string.Empty;
            }
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(KEY_64, IV_64), CryptoStreamMode.Write);
            StreamWriter writer = new StreamWriter(stream2);
            writer.Write(value);
            writer.Flush();
            stream2.FlushFinalBlock();
            stream.Flush();
            return Convert.ToBase64String(stream.GetBuffer(), 0, Convert.ToInt32(stream.Length));
        }

        public static string EncryptTripleDES(string value)
        {
            if ((value == null) || (value == ""))
            {
                return string.Empty;
            }
            TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider();
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(KEY_192, IV_192), CryptoStreamMode.Write);
            StreamWriter writer = new StreamWriter(stream2);
            writer.Write(value);
            writer.Flush();
            stream2.FlushFinalBlock();
            stream.Flush();
            return Convert.ToBase64String(stream.GetBuffer(), 0, Convert.ToInt32(stream.Length));
        }
    }
}

