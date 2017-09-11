namespace Hishop.TransferManager
{
    using System;
    using System.Data;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Web;
    using System.Web.Caching;

    internal class TransferContainer
    {
        private static volatile TransferContainer _instance = null;
        private const string CacheKey = "Hishop_TransferIndexes";
        private static readonly object LockHelper = new object();
        private static volatile Cache TransferCache = HttpRuntime.Cache;

        private TransferContainer()
        {
            TransferCache.Remove("Hishop_TransferIndexes");
        }

        private static void AddToExportIndex(Type t, string filename, DataTable dtExporters)
        {
            ExportAdapter adapter = Activator.CreateInstance(t) as ExportAdapter;
            DataRow row = dtExporters.NewRow();
            row["fullName"] = t.FullName.ToLower();
            row["filePath"] = filename;
            row["sourceName"] = adapter.Source.Name;
            row["sourceVersion"] = adapter.Source.Version.ToString();
            row["exportToName"] = adapter.ExportTo.Name;
            row["exportToVersion"] = adapter.ExportTo.Version.ToString();
            dtExporters.Rows.Add(row);
        }

        private static void AddToImportIndex(Type t, string filename, DataTable dtImporters)
        {
            ImportAdapter adapter = Activator.CreateInstance(t) as ImportAdapter;
            DataRow row = dtImporters.NewRow();
            row["fullName"] = t.FullName.ToLower();
            row["filePath"] = filename;
            row["sourceName"] = adapter.Source.Name;
            row["sourceVersion"] = adapter.Source.Version.ToString();
            row["importToName"] = adapter.ImportTo.Name;
            row["importToVersion"] = adapter.ImportTo.Version.ToString();
            dtImporters.Rows.Add(row);
        }

        private static void BuildIndex(string pluginPath, DataTable dtExporters, DataTable dtImporters)
        {
            if (Directory.Exists(pluginPath))
            {
                foreach (string str in Directory.GetFiles(pluginPath, "*.dll", SearchOption.AllDirectories))
                {
                    foreach (Type type in Assembly.Load(LoadPluginFile(str)).GetExportedTypes())
                    {
                        if (type.BaseType != null)
                        {
                            if (type.BaseType.Name == "ExportAdapter")
                            {
                                AddToExportIndex(type, str, dtExporters);
                            }
                            else if (type.BaseType.Name == "ImportAdapter")
                            {
                                AddToImportIndex(type, str, dtImporters);
                            }
                        }
                    }
                }
            }
        }

        internal Type GetExporter(string fullName)
        {
            return GetPlugin(fullName, "Exporters");
        }

        internal DataRow[] GetExporterList(string sourceName, string exportToName)
        {
            DataSet set = TransferCache.Get("Hishop_TransferIndexes") as DataSet;
            return set.Tables["Exporters"].Select(string.Format("sourceName='{0}' and exportToName='{1}'", sourceName, exportToName), "sourceVersion desc");
        }

        internal Type GetImporter(string fullName)
        {
            return GetPlugin(fullName, "Importers");
        }

        internal DataRow[] GetImporterList(string sourceName, string importToName)
        {
            DataSet set = TransferCache.Get("Hishop_TransferIndexes") as DataSet;
            return set.Tables["Importers"].Select(string.Format("sourceName='{0}' and importToName='{1}'", sourceName, importToName), "importToVersion desc");
        }

        private static Type GetPlugin(string fullName, string tableName)
        {
            DataSet set = TransferCache.Get("Hishop_TransferIndexes") as DataSet;
            DataRow[] rowArray = set.Tables[tableName].Select("fullName='" + fullName.ToLower() + "'");
            if ((rowArray.Length != 0) && File.Exists(rowArray[0]["filePath"].ToString()))
            {
                return Assembly.Load(LoadPluginFile(rowArray[0]["filePath"].ToString())).GetType(fullName, false, true);
            }
            return null;
        }

        private static void Init()
        {
            if (TransferCache.Get("Hishop_TransferIndexes") == null)
            {
                string pluginPath = HttpContext.Current.Request.MapPath("~/plugins/transfer");
                DataSet set = new DataSet();
                DataTable table = new DataTable("Exporters");
                DataTable table2 = new DataTable("Importers");
                InitTable(table);
                InitTable(table2);
                table.Columns.Add(new DataColumn("exportToName"));
                table.Columns.Add(new DataColumn("exportToVersion"));
                table2.Columns.Add(new DataColumn("importToName"));
                table2.Columns.Add(new DataColumn("importToVersion"));
                set.Tables.Add(table);
                set.Tables.Add(table2);
                BuildIndex(pluginPath, table, table2);
                TransferCache.Insert("Hishop_TransferIndexes", set, new CacheDependency(pluginPath));
            }
        }

        private static void InitTable(DataTable table)
        {
            DataColumn column = new DataColumn("fullName") {
                Unique = true
            };
            table.Columns.Add(column);
            table.Columns.Add(new DataColumn("filePath"));
            table.Columns.Add(new DataColumn("sourceName"));
            table.Columns.Add(new DataColumn("sourceVersion"));
            table.PrimaryKey = new DataColumn[] { table.Columns["fullName"] };
        }

        internal static TransferContainer Instance()
        {
            if (_instance == null)
            {
                lock (LockHelper)
                {
                    if (_instance == null)
                    {
                        _instance = new TransferContainer();
                    }
                }
            }
            Init();
            return _instance;
        }

        private static byte[] LoadPluginFile(string filename)
        {
            byte[] buffer;
            using (FileStream stream = new FileStream(filename, FileMode.Open))
            {
                buffer = new byte[(int) stream.Length];
                stream.Read(buffer, 0, buffer.Length);
            }
            return buffer;
        }
    }
}

