namespace Hidistro.Core
{
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Security;
    using System.Security.Permissions;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    public sealed class Serializer
    {
        private static bool CanBinarySerialize;

        static Serializer()
        {
            SecurityPermission permission = new SecurityPermission(SecurityPermissionFlag.SerializationFormatter);
            try
            {
                permission.Demand();
                CanBinarySerialize = true;
            }
            catch (SecurityException)
            {
                CanBinarySerialize = false;
            }
        }

        private Serializer()
        {
        }

        public static object ConvertFileToObject(string path, Type objectType)
        {
            object obj2 = null;
            if ((path == null) || (path.Length <= 0))
            {
                return obj2;
            }
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                XmlSerializer serializer = new XmlSerializer(objectType);
                return serializer.Deserialize(stream);
            }
        }

        public static void ConvertFromNameValueCollection(NameValueCollection nvc, ref string keys, ref string values)
        {
            if ((nvc != null) && (nvc.Count != 0))
            {
                StringBuilder builder = new StringBuilder();
                StringBuilder builder2 = new StringBuilder();
                int num = 0;
                foreach (string str in nvc.AllKeys)
                {
                    if (str.IndexOf(':') != -1)
                    {
                        throw new ArgumentException("ExtendedAttributes Key can not contain the character \":\"");
                    }
                    string str2 = nvc[str];
                    if (!string.IsNullOrEmpty(str2))
                    {
                        builder.AppendFormat("{0}:S:{1}:{2}:", str, num, str2.Length);
                        builder2.Append(str2);
                        num += str2.Length;
                    }
                }
                keys = builder.ToString();
                values = builder2.ToString();
            }
        }

        public static byte[] ConvertToBytes(object objectToConvert)
        {
            byte[] buffer = null;
            if (CanBinarySerialize)
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (MemoryStream stream = new MemoryStream())
                {
                    formatter.Serialize(stream, objectToConvert);
                    stream.Position = 0L;
                    buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, buffer.Length);
                }
            }
            return buffer;
        }

        public static NameValueCollection ConvertToNameValueCollection(string keys, string values)
        {
            NameValueCollection values2 = new NameValueCollection();
            if (((keys != null) && (values != null)) && ((keys.Length > 0) && (values.Length > 0)))
            {
                char[] separator = new char[] { ':' };
                string[] strArray = keys.Split(separator);
                for (int i = 0; i < (strArray.Length / 4); i++)
                {
                    int startIndex = int.Parse(strArray[(i * 4) + 2], CultureInfo.InvariantCulture);
                    int length = int.Parse(strArray[(i * 4) + 3], CultureInfo.InvariantCulture);
                    string str = strArray[i * 4];
                    if (((strArray[(i * 4) + 1] == "S") && (startIndex >= 0)) && ((length > 0) && (values.Length >= (startIndex + length))))
                    {
                        values2[str] = values.Substring(startIndex, length);
                    }
                }
            }
            return values2;
        }

        public static object ConvertToObject(byte[] byteArray)
        {
            object obj2 = null;
            if ((CanBinarySerialize && (byteArray != null)) && (byteArray.Length > 0))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (MemoryStream stream = new MemoryStream())
                {
                    stream.Write(byteArray, 0, byteArray.Length);
                    stream.Position = 0L;
                    if (byteArray.Length > 4)
                    {
                        obj2 = formatter.Deserialize(stream);
                    }
                }
            }
            return obj2;
        }

        public static object ConvertToObject(string xml, Type objectType)
        {
            object obj2 = null;
            if (string.IsNullOrEmpty(xml))
            {
                return obj2;
            }
            using (StringReader reader = new StringReader(xml))
            {
                XmlSerializer serializer = new XmlSerializer(objectType);
                return serializer.Deserialize(reader);
            }
        }

        public static object ConvertToObject(XmlNode node, Type objectType)
        {
            object obj2 = null;
            if (node == null)
            {
                return obj2;
            }
            using (StringReader reader = new StringReader(node.OuterXml))
            {
                XmlSerializer serializer = new XmlSerializer(objectType);
                return serializer.Deserialize(reader);
            }
        }

        public static string ConvertToString(object objectToConvert)
        {
            string str = null;
            if (objectToConvert == null)
            {
                return str;
            }
            XmlSerializer serializer = new XmlSerializer(objectToConvert.GetType());
            using (StringWriter writer = new StringWriter(CultureInfo.InvariantCulture))
            {
                serializer.Serialize((TextWriter) writer, objectToConvert);
                return writer.ToString();
            }
        }

        public static string ConvertToString(object objectToConvert, params Type[] extra)
        {
            string str = null;
            if (objectToConvert == null)
            {
                return str;
            }
            XmlSerializer serializer = new XmlSerializer(objectToConvert.GetType(), extra);
            using (StringWriter writer = new StringWriter(CultureInfo.InvariantCulture))
            {
                serializer.Serialize((TextWriter) writer, objectToConvert);
                return writer.ToString();
            }
        }

        public static object LoadBinaryFile(string path)
        {
            byte[] buffer;
            if (!File.Exists(path))
            {
                return null;
            }
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    buffer = new byte[stream.Length];
                    reader.Read(buffer, 0, (int) stream.Length);
                }
            }
            return ConvertToObject(buffer);
        }

        public static bool SaveAsBinary(object objectToSave, string path)
        {
            if ((objectToSave != null) && CanBinarySerialize)
            {
                byte[] buffer = ConvertToBytes(objectToSave);
                if (buffer != null)
                {
                    using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        using (BinaryWriter writer = new BinaryWriter(stream))
                        {
                            writer.Write(buffer);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static void SaveAsXML(object objectToConvert, string path)
        {
            if (objectToConvert != null)
            {
                XmlSerializer serializer = new XmlSerializer(objectToConvert.GetType());
                using (StreamWriter writer = new StreamWriter(path))
                {
                    serializer.Serialize((TextWriter) writer, objectToConvert);
                }
            }
        }
    }
}

