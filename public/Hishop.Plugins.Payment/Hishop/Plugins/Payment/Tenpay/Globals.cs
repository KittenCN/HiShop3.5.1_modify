namespace Hishop.Plugins.Payment.Tenpay
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    internal static class Globals
    {
        internal static string GetMD5(string encypStr)
        {
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            byte[] bytes = Encoding.UTF8.GetBytes(encypStr);
            return BitConverter.ToString(provider.ComputeHash(bytes)).Replace("-", "").ToUpper();
        }
    }
}

