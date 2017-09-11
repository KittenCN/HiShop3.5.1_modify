namespace Hishop.Transfers.PaipaiImporters
{
    using Hishop.TransferManager;
    using Ionic.Zip;
    using LumenWorks.Framework.IO.Csv;
    using System;
    using System.Data;
    using System.IO;
    using System.Text;
    using System.Web;

    public class Yfx1_2_from_Paipai4_0 : ImportAdapter
    {
        private readonly DirectoryInfo _baseDir = new DirectoryInfo(HttpContext.Current.Request.MapPath("~/storage/data/paipai"));
        private readonly Target _importTo = new YfxTarget("1.2");
        private readonly Target _source = new PPTarget("4.0");
        private DirectoryInfo _workDir;
        private const string ProductFilename = "products.csv";

        public override object[] CreateMapping(params object[] initParams)
        {
            throw new NotImplementedException();
        }

        private DataTable GetProductSet()
        {
            DataTable table = new DataTable("products");
            DataColumn column = new DataColumn("ProductName") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column);
            DataColumn column2 = new DataColumn("Description") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column2);
            DataColumn column3 = new DataColumn("ImageUrl1") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column3);
            DataColumn column4 = new DataColumn("ImageUrl2") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column4);
            DataColumn column5 = new DataColumn("ImageUrl3") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column5);
            DataColumn column6 = new DataColumn("ImageUrl4") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column6);
            DataColumn column7 = new DataColumn("ImageUrl5") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column7);
            DataColumn column8 = new DataColumn("SKU") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column8);
            DataColumn column9 = new DataColumn("Stock") {
                DataType = Type.GetType("System.Int32")
            };
            table.Columns.Add(column9);
            DataColumn column10 = new DataColumn("SalePrice") {
                DataType = Type.GetType("System.Decimal")
            };
            table.Columns.Add(column10);
            DataColumn column11 = new DataColumn("Weight") {
                DataType = Type.GetType("System.Decimal")
            };
            table.Columns.Add(column11);
            return table;
        }

        public override object[] ParseIndexes(params object[] importParams)
        {
            throw new NotImplementedException();
        }

        public override object[] ParseProductData(params object[] importParams)
        {
            string str = (string) importParams[0];
            HttpContext current = HttpContext.Current;
            DataTable productSet = this.GetProductSet();
            using (CsvReader reader = new CsvReader(new StreamReader(Path.Combine(str, "products.csv"), Encoding.Default), true, '\t'))
            {
                int num = 0;
                while (reader.ReadNextRecord())
                {
                    num++;
                    DataRow row = productSet.NewRow();
                    Random random = new Random();
                    row["SKU"] = string.Format("{0}{1}", string.Concat(new object[] { random.Next(9).ToString(), random.Next(9), random.Next(9), random.Next(9), random.Next(9) }), num);
                    row["SalePrice"] = decimal.Parse(reader[10]);
                    if (!string.IsNullOrEmpty(reader[6]))
                    {
                        row["Weight"] = decimal.Parse(reader[6]);
                    }
                    if (!string.IsNullOrEmpty(reader[5]))
                    {
                        row["Stock"] = int.Parse(reader[5]);
                    }
                    row["ProductName"] = this.Trim(reader[1]);
                    if (!string.IsNullOrEmpty(reader[0x1f]))
                    {
                        string path = Path.Combine(str + @"\products", reader[0x1f]);
                        if (File.Exists(path))
                        {
                            row["Description"] = File.ReadAllText(path, Encoding.GetEncoding("gb2312"));
                        }
                    }
                    string str3 = this.Substring(reader[0x1a]);
                    if (!string.IsNullOrEmpty(str3))
                    {
                        str3 = str3.Substring(str3.LastIndexOf(@"\") + 1);
                        if (File.Exists(Path.Combine(str + @"\products", str3)))
                        {
                            File.Copy(Path.Combine(str + @"\products", str3), current.Request.MapPath("~/Storage/master/product/images/" + str3), true);
                            row["ImageUrl1"] = "/Storage/master/product/images/" + str3;
                        }
                    }
                    str3 = this.Substring(reader[0x1b]);
                    if (!string.IsNullOrEmpty(str3))
                    {
                        str3 = str3.Substring(str3.LastIndexOf(@"\") + 1);
                        if (File.Exists(Path.Combine(str + @"\products", str3)))
                        {
                            File.Copy(Path.Combine(str + @"\products", str3), current.Request.MapPath("~/Storage/master/product/images/" + str3), true);
                            row["ImageUrl2"] = "/Storage/master/product/images/" + str3;
                        }
                    }
                    str3 = this.Substring(reader[0x1c]);
                    if (!string.IsNullOrEmpty(str3))
                    {
                        str3 = str3.Substring(str3.LastIndexOf(@"\") + 1);
                        if (File.Exists(Path.Combine(str + @"\products", str3)))
                        {
                            File.Copy(Path.Combine(str + @"\products", str3), current.Request.MapPath("~/Storage/master/product/images/" + str3), true);
                            row["ImageUrl3"] = "/Storage/master/product/images/" + str3;
                        }
                    }
                    str3 = this.Substring(reader[0x1d]);
                    if (!string.IsNullOrEmpty(str3))
                    {
                        str3 = str3.Substring(str3.LastIndexOf(@"\") + 1);
                        if (File.Exists(Path.Combine(str + @"\products", str3)))
                        {
                            File.Copy(Path.Combine(str + @"\products", str3), current.Request.MapPath("~/Storage/master/product/images/" + str3), true);
                            row["ImageUrl4"] = "/Storage/master/product/images/" + str3;
                        }
                    }
                    str3 = this.Substring(reader[30]);
                    if (!string.IsNullOrEmpty(str3))
                    {
                        str3 = str3.Substring(str3.LastIndexOf(@"\") + 1);
                        if (File.Exists(Path.Combine(str + @"\products", str3)))
                        {
                            File.Copy(Path.Combine(str + @"\products", str3), current.Request.MapPath("~/Storage/master/product/images/" + str3), true);
                            row["ImageUrl5"] = "/Storage/master/product/images/" + str3;
                        }
                    }
                    productSet.Rows.Add(row);
                }
            }
            return new object[] { productSet };
        }

        public override string PrepareDataFiles(params object[] initParams)
        {
            string path = (string) initParams[0];
            this._workDir = this._baseDir.CreateSubdirectory(Path.GetFileNameWithoutExtension(path));
            using (ZipFile file = ZipFile.Read(Path.Combine(this._baseDir.FullName, path)))
            {
                foreach (ZipEntry entry in file)
                {
                    entry.Extract(this._workDir.FullName, ExtractExistingFileAction.OverwriteSilently);
                }
            }
            return this._workDir.FullName;
        }

        private string Substring(string str)
        {
            if (str.StartsWith("\""))
            {
                str = str.Substring(1);
            }
            if (str.EndsWith("\""))
            {
                str = str.Substring(0, str.Length - 1);
            }
            return str;
        }

        private string Trim(string str)
        {
            while (str.StartsWith("\""))
            {
                str = str.Substring(1);
            }
            while (str.EndsWith("\""))
            {
                str = str.Substring(0, str.Length - 1);
            }
            return str;
        }

        public override Target ImportTo
        {
            get
            {
                return this._importTo;
            }
        }

        public override Target Source
        {
            get
            {
                return this._source;
            }
        }
    }
}

