namespace Hishop.TransferManager
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Web;
    using System.Xml;

    public static class TransferHelper
    {
        public static byte[] ConvertToBytes(string imageUrl)
        {
            byte[] buffer = new byte[0];
            if (!string.IsNullOrEmpty(imageUrl))
            {
                string path = HttpContext.Current.Request.MapPath("~" + imageUrl);
                if (!File.Exists(path))
                {
                    return buffer;
                }
                try
                {
                    buffer = File.ReadAllBytes(path);
                }
                catch
                {
                }
            }
            return buffer;
        }

        public static Dictionary<string, string> GetExportAdapters(Target source, string exportToName)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            DataRow[] exporterList = TransferContainer.Instance().GetExporterList(source.Name, exportToName);
            if ((exporterList != null) && (exporterList.Length != 0))
            {
                string str = null;
                int index = 0;
                do
                {
                    Version version = new Version(exporterList[index]["sourceVersion"].ToString());
                    if (version <= source.Version)
                    {
                        str = exporterList[index]["sourceVersion"].ToString();
                    }
                    index++;
                }
                while (string.IsNullOrEmpty(str) && (index < exporterList.Length));
                if (string.IsNullOrEmpty(str))
                {
                    return dictionary;
                }
                foreach (DataRow row in exporterList)
                {
                    if (row["sourceVersion"].ToString().Equals(str))
                    {
                        dictionary.Add(row["fullName"].ToString(), row["exportToName"].ToString() + row["exportToVersion"].ToString());
                    }
                }
            }
            return dictionary;
        }

        public static ExportAdapter GetExporter(string fullName, params object[] exportParams)
        {
            if (string.IsNullOrEmpty(fullName))
            {
                return null;
            }
            Type exporter = TransferContainer.Instance().GetExporter(fullName);
            if (exporter == null)
            {
                return null;
            }
            if ((exportParams != null) && (exportParams.Length > 0))
            {
                return (Activator.CreateInstance(exporter, exportParams) as ExportAdapter);
            }
            return (Activator.CreateInstance(exporter) as ExportAdapter);
        }

        public static Dictionary<string, string> GetImportAdapters(Target importTo, string sourceName)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            DataRow[] importerList = TransferContainer.Instance().GetImporterList(sourceName, importTo.Name);
            if ((importerList != null) && (importerList.Length != 0))
            {
                string str = null;
                int index = 0;
                do
                {
                    Version version = new Version(importerList[index]["importToVersion"].ToString());
                    if (version <= importTo.Version)
                    {
                        str = importerList[index]["importToVersion"].ToString();
                    }
                    index++;
                }
                while (string.IsNullOrEmpty(str) && (index < importerList.Length));
                if (string.IsNullOrEmpty(str))
                {
                    return dictionary;
                }
                foreach (DataRow row in importerList)
                {
                    if (row["importToVersion"].ToString().Equals(str))
                    {
                        dictionary.Add(row["fullName"].ToString(), row["sourceName"].ToString() + row["sourceVersion"].ToString());
                    }
                }
            }
            return dictionary;
        }

        public static ImportAdapter GetImporter(string fullName, params object[] exportParams)
        {
            if (string.IsNullOrEmpty(fullName))
            {
                return null;
            }
            Type importer = TransferContainer.Instance().GetImporter(fullName);
            if (importer == null)
            {
                return null;
            }
            if ((exportParams != null) && (exportParams.Length > 0))
            {
                return (Activator.CreateInstance(importer, exportParams) as ImportAdapter);
            }
            return (Activator.CreateInstance(importer) as ImportAdapter);
        }

        public static void WriteCDataElement(XmlWriter writer, string nodeName, string text)
        {
            writer.WriteStartElement(nodeName);
            writer.WriteCData(text);
            writer.WriteEndElement();
        }

        public static void WriteImageElement(XmlWriter writer, string nodeName, bool includeImages, string imageUrl)
        {
            writer.WriteStartElement(nodeName);
            if (includeImages)
            {
                byte[] buffer = ConvertToBytes(imageUrl);
                writer.WriteBase64(buffer, 0, buffer.Length);
            }
            else
            {
                writer.WriteString(imageUrl);
            }
            writer.WriteEndElement();
        }

        public static void WriteImageElement(XmlWriter writer, string nodeName, bool includeImages, string imageUrl, DirectoryInfo destDir)
        {
            writer.WriteStartElement(nodeName);
            if (!string.IsNullOrEmpty(imageUrl))
            {
                if (includeImages)
                {
                    string path = HttpContext.Current.Request.MapPath("~" + imageUrl);
                    string fileName = Path.GetFileName(path);
                    writer.WriteString(fileName);
                    if (File.Exists(path))
                    {
                        File.Copy(path, Path.Combine(destDir.FullName, fileName), true);
                    }
                }
                else
                {
                    writer.WriteString(imageUrl);
                }
            }
            writer.WriteEndElement();
        }
    }
}

