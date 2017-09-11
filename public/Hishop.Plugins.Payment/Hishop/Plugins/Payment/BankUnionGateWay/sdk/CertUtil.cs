namespace Hishop.Plugins.Payment.BankUnionGateWay.sdk
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Security.Cryptography.X509Certificates;

    public class CertUtil
    {
        public static string GetSignCertId()
        {
            X509Certificate2 certificate = new X509Certificate2(SDKConfig.signCertPath, SDKConfig.SignCertPwd, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);
            return BigNum.ToDecimalStr(BigNum.ConvertFromHex(certificate.SerialNumber));
        }

        public static RSACryptoServiceProvider GetSignProviderFromPfx()
        {
            X509Certificate2 certificate = new X509Certificate2(SDKConfig.signCertPath, SDKConfig.SignCertPwd, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);
            return (RSACryptoServiceProvider) certificate.PrivateKey;
        }

        public static RSACryptoServiceProvider GetValidateProviderFromPath(string certId)
        {
            FileInfo[] files = new DirectoryInfo(SDKConfig.validateCertDir).GetFiles("*.cer");
            if ((files != null) && (0 != files.Length))
            {
                foreach (FileInfo info2 in files)
                {
                    X509Certificate2 certificate = new X509Certificate2(info2.DirectoryName + @"\" + info2.Name);
                    string str = BigNum.ToDecimalStr(BigNum.ConvertFromHex(certificate.SerialNumber));
                    if (certId.Equals(str))
                    {
                        return (RSACryptoServiceProvider) certificate.PublicKey.Key;
                    }
                }
            }
            return null;
        }

        public static bool ValidateCertId(string certId)
        {
            FileInfo[] files = new DirectoryInfo(SDKConfig.validateCertDir).GetFiles("*.cer");
            if ((files != null) && (0 != files.Length))
            {
                foreach (FileInfo info2 in files)
                {
                    X509Certificate2 certificate = new X509Certificate2(info2.DirectoryName + @"\" + info2.Name);
                    string str = BigNum.ToDecimalStr(BigNum.ConvertFromHex(certificate.SerialNumber));
                    if (certId.Equals(str))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}

