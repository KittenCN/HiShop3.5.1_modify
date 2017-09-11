namespace Hishop.Plugins.Payment.Xpay
{
    using System;
    using System.Globalization;
    using System.Security.Cryptography;
    using System.Text;

    internal static class Globals
    {
        internal static string GetXpayMD5(string s)
        {
            using (MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider())
            {
                return BitConverter.ToString(provider.ComputeHash(Encoding.Default.GetBytes(s))).Replace("-", "").ToLower(CultureInfo.InvariantCulture);
            }
        }
    }
}

