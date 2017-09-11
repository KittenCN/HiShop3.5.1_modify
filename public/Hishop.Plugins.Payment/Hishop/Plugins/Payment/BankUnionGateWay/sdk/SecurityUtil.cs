namespace Hishop.Plugins.Payment.BankUnionGateWay.sdk
{
    using ICSharpCode.SharpZipLib.Zip.Compression;
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;

    public static class SecurityUtil
    {
        public static string ALGORITHM_SHA1 = "SHA1";

        public static string DecodeBase64(Encoding encode, string result)
        {
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                return encode.GetString(bytes);
            }
            catch
            {
                return result;
            }
        }

        public static byte[] deflater(byte[] inputByte)
        {
            byte[] buffer = new byte[0x400];
            MemoryStream stream = new MemoryStream();
            Deflater deflater = new Deflater();
            deflater.SetInput(inputByte);
            deflater.Finish();
            while (!deflater.IsFinished)
            {
                int count = deflater.Deflate(buffer);
                if (count > 0)
                {
                    stream.Write(buffer, 0, count);
                }
                else
                {
                    break;
                }
            }
            return stream.ToArray();
        }

        public static string EncodeBase64(Encoding encode, string source)
        {
            byte[] bytes = encode.GetBytes(source);
            try
            {
                return Convert.ToBase64String(bytes);
            }
            catch
            {
                return source;
            }
        }

        public static string EncryptData(string dataString, string encoding)
        {
            byte[] bytes = null;
            try
            {
                bytes = encryptedData(Encoding.UTF8.GetBytes(dataString));
                return EncodeBase64(Encoding.Default, Encoding.Default.GetString(bytes));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return "";
            }
        }

        public static byte[] encryptedData(byte[] encData)
        {
            try
            {
                X509Certificate2 certificate = new X509Certificate2(SDKConfig.publicCertPath);
                RSACryptoServiceProvider key = new RSACryptoServiceProvider();
                key = (RSACryptoServiceProvider) certificate.PublicKey.Key;
                return key.Encrypt(encData, false);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            return null;
        }

        public static string EncryptPin(string pin, string card, string encoding)
        {
            byte[] encData = pin2PinBlockWithCardNO(pin, card);
            byte[] bytes = null;
            try
            {
                bytes = encryptedData(encData);
                return EncodeBase64(Encoding.Default, Encoding.Default.GetString(bytes));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return "";
            }
        }

        private static byte[] formatPan(string aPan)
        {
            int length = aPan.Length;
            byte[] buffer = new byte[8];
            int startIndex = length - 13;
            try
            {
                buffer[0] = 0;
                buffer[1] = 0;
                for (int i = 2; i < 8; i++)
                {
                    string str = aPan.Substring(startIndex, 2).Trim();
                    buffer[i] = (byte) Convert.ToInt32(str, 0x10);
                    startIndex += 2;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            return buffer;
        }

        public static byte[] inflater(byte[] inputByte)
        {
            byte[] buffer = new byte[0x400];
            MemoryStream stream = new MemoryStream();
            Inflater inflater = new Inflater();
            inflater.SetInput(inputByte);
            while (!inflater.IsFinished)
            {
                int count = inflater.Inflate(buffer);
                if (count > 0)
                {
                    stream.Write(buffer, 0, count);
                }
                else
                {
                    break;
                }
            }
            return stream.ToArray();
        }

        private static byte[] pin2PinBlock(string aPin)
        {
            int index = 1;
            int length = aPin.Length;
            byte[] buffer = new byte[8];
            try
            {
                int num3;
                string str;
                int num4;
                buffer[0] = (byte) Convert.ToInt32(length.ToString(), 10);
                if ((length % 2) == 0)
                {
                    for (num3 = 0; num3 < length; num3 += 2)
                    {
                        str = aPin.Substring(num3, 2).Trim();
                        buffer[index] = (byte) Convert.ToInt32(str, 0x10);
                        if ((num3 == (length - 2)) && (index < 7))
                        {
                            num4 = index + 1;
                            while (num4 < 8)
                            {
                                buffer[num4] = 0xff;
                                num4++;
                            }
                        }
                        index++;
                    }
                    return buffer;
                }
                for (num3 = 0; num3 < (length - 1); num3 += 2)
                {
                    str = aPin.Substring(num3, 2);
                    buffer[index] = (byte) Convert.ToInt32(str, 0x10);
                    if (num3 == (length - 3))
                    {
                        string str2 = aPin.Substring(length - 1) + "F";
                        buffer[index + 1] = (byte) Convert.ToInt32(str2, 0x10);
                        if ((index + 1) < 7)
                        {
                            for (num4 = index + 2; num4 < 8; num4++)
                            {
                                buffer[num4] = 0xff;
                            }
                        }
                    }
                    index++;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            return buffer;
        }

        public static byte[] pin2PinBlockWithCardNO(string aPin, string aCardNO)
        {
            byte[] buffer = pin2PinBlock(aPin);
            if (aCardNO.Length == 11)
            {
                aCardNO = "00" + aCardNO;
            }
            else if (aCardNO.Length == 12)
            {
                aCardNO = "0" + aCardNO;
            }
            byte[] buffer2 = formatPan(aCardNO);
            byte[] buffer3 = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                buffer3[i] = (byte) (buffer[i] ^ buffer2[i]);
            }
            return buffer3;
        }

        public static byte[] Sha1X16(string dataStr, Encoding encoding)
        {
            byte[] buffer3;
            try
            {
                byte[] bytes = encoding.GetBytes(dataStr);
                buffer3 = new SHA1CryptoServiceProvider().ComputeHash(bytes, 0, bytes.Length);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return buffer3;
        }

        public static byte[] SignBySoft(RSACryptoServiceProvider provider, byte[] data)
        {
            byte[] buffer = null;
            try
            {
                HashAlgorithm halg = new SHA1CryptoServiceProvider();
                buffer = provider.SignData(data, halg);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            if (null == buffer)
            {
                return null;
            }
            return buffer;
        }

        public static bool ValidateBySoft(RSACryptoServiceProvider provider, byte[] base64DecodingSignStr, byte[] srcByte)
        {
            HashAlgorithm halg = new SHA1CryptoServiceProvider();
            return provider.VerifyData(srcByte, halg, base64DecodingSignStr);
        }
    }
}

