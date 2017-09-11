namespace Hidistro.Core.Encrypt
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public class Des
    {
        private const string IV_64 = "Key_Dflt";
        private const string KEY_64 = "Key_Dflt";

        public static string Decrypt(string decryptString, string KEY, string IV)
        {
            KEY = KEY + "Key_Dflt";
            IV = IV + "Key_Dflt";
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(KEY.Substring(0, 8));
                byte[] rgbIV = Encoding.UTF8.GetBytes(IV.Substring(0, 8));
                byte[] buffer = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                MemoryStream stream = new MemoryStream();
                CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(bytes, rgbIV), CryptoStreamMode.Write);
                stream2.Write(buffer, 0, buffer.Length);
                stream2.FlushFinalBlock();
                return Encoding.UTF8.GetString(stream.ToArray());
            }
            catch
            {
                return decryptString;
            }
        }

        public static string Encrypt(string encryptString, string KEY, string IV)
        {
            KEY = KEY + "Key_Dflt";
            IV = IV + "Key_Dflt";
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(KEY.Substring(0, 8));
                byte[] rgbIV = Encoding.UTF8.GetBytes(IV.Substring(0, 8));
                byte[] buffer = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                MemoryStream stream = new MemoryStream();
                CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(bytes, rgbIV), CryptoStreamMode.Write);
                stream2.Write(buffer, 0, buffer.Length);
                stream2.FlushFinalBlock();
                return Convert.ToBase64String(stream.ToArray());
            }
            catch (Exception)
            {
                return encryptString;
            }
        }
    }
}

