namespace Hidistro.Core.ExtensionMethods
{
    using Hidistro.Core.Encrypt;
    using System;
    using System.Runtime.CompilerServices;
    using System.Text;

    public static class StringExt
    {
        public static string DecodeBase64(this string str, Encoding encode)
        {
            byte[] bytes = Convert.FromBase64String(str);
            try
            {
                return encode.GetString(bytes);
            }
            catch
            {
                return str;
            }
        }

        public static string DecodeBase64Utf8(this string str)
        {
            return str.DecodeBase64(Encoding.UTF8);
        }

        public static string DeXml(this string str)
        {
            return str.Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&quot;", "\"").Replace("&apos;", "'");
        }

        public static string EncodeBase64(this string str, Encoding encode)
        {
            byte[] bytes = encode.GetBytes(str);
            try
            {
                return Convert.ToBase64String(bytes);
            }
            catch
            {
                return str;
            }
        }

        public static string EncodeBase64Utf8(this string str)
        {
            return str.EncodeBase64(Encoding.UTF8);
        }

        public static string GB2312ToUTF8(this string str)
        {
            string str3;
            try
            {
                Encoding dstEncoding = Encoding.GetEncoding(0xfde9);
                Encoding encoding = Encoding.GetEncoding("gb2312");
                byte[] bytes = encoding.GetBytes(str);
                byte[] buffer2 = Encoding.Convert(encoding, dstEncoding, bytes);
                str3 = dstEncoding.GetString(buffer2);
            }
            catch (Exception exception)
            {
                throw new Exception("字符集转换GB2312 To UTF8错误：" + exception.Message);
            }
            return str3;
        }

        public static string MD5(this string str)
        {
            return Hash.MD5(str.EncodeBase64Utf8());
        }

        public static string UTF8ToGB2312(this string str)
        {
            string str3;
            try
            {
                Encoding srcEncoding = Encoding.GetEncoding(0xfde9);
                Encoding encoding = Encoding.GetEncoding("gb2312");
                byte[] bytes = srcEncoding.GetBytes(str);
                byte[] buffer2 = Encoding.Convert(srcEncoding, encoding, bytes);
                str3 = encoding.GetString(buffer2);
            }
            catch (Exception exception)
            {
                throw new Exception("字符集转换UTF8 To GB2312错误：" + exception.Message);
            }
            return str3;
        }

        public static string Xml(this string str)
        {
            return str.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;");
        }
    }
}

