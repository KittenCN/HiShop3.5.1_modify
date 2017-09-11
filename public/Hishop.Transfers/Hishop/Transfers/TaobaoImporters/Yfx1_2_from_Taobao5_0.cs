namespace Hishop.Transfers.TaobaoImporters
{
    using Hishop.TransferManager;
    using Ionic.Zip;
    using LumenWorks.Framework.IO.Csv;
    using System;
    using System.Data;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;

    public class Yfx1_2_from_Taobao5_0 : ImportAdapter
    {
        private static char[] constant = new char[] { 
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 
            'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 
            'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 
            'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'Z', 'Y', 
            'X', 'W', 'V', 'U', 'T', 'S', 'R', 'Q', 'P', 'O', 'N', 'M', 'L', 'K', 'J', 'I', 
            'H', 'G', 'F', 'E', 'D', 'C', 'B', 'A', 'z', 'y', 'x', 'w', 'v', 'u', 't', 's', 
            'r', 'q', 'p', 'o', 'n', 'm', 'l', 'k', 'j', 'i', 'h', 'g', 'f', 'e', 'd', 'c', 
            'b', 'a', '9', '8', '7', '6', '5', '4', '3', '2', '1', '0'
         };
        private static readonly string[] csvFields = new string[] { 
            "宝贝名称", "宝贝类目", "新旧程度", "省", "城市", "宝贝价格", "宝贝数量", "有效期", "运费承担", "平邮", "EMS", "快递", "发票", "保修", "开始时间", "宝贝描述", 
            "宝贝属性", "会员打折", "商家编码", "用户输入ID串", "用户输入名-值对", "销售属性别名"
         };
        private string csvfmtfile = "products.csv";
        private string imagefolder = "products";
        private DirectoryInfo m_baseDir = new DirectoryInfo(HttpContext.Current.Request.MapPath("~/storage/data/taobao"));
        private Encoding m_EncodingType = Encoding.Default;
        private readonly Target m_importTo = new YfxTarget("1.2");
        private readonly Target m_source = new TbTarget("5.0");
        private string m_webDir = AppDomain.CurrentDomain.BaseDirectory;
        private DirectoryInfo m_workDir;

        public Yfx1_2_from_Taobao5_0()
        {
            this.m_EncodingType = Encoding.Default;
            try
            {
                if (!Directory.Exists(this.m_webDir + @"\Storage\master\product\images\"))
                {
                    Directory.CreateDirectory(this.m_webDir + @"\Storage\master\product\images\");
                }
            }
            catch
            {
            }
        }

        public override object[] CreateMapping(params object[] initParams)
        {
            throw new NotImplementedException();
        }

        private string GenerateId()
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random(Guid.NewGuid().GetHashCode());
            char ch = '0';
            for (int i = 0; i < 8; i++)
            {
                ch = constant[random.Next(0, 0x7c)];
                if (builder.Length == 0)
                {
                    builder.Append(ch);
                }
                else if (builder.ToString().Contains(ch.ToString()))
                {
                    i--;
                }
                else
                {
                    builder.Append(ch);
                }
            }
            return builder.ToString();
        }

        public static string[] GetHtmlImageUrlList(string sHtmlText)
        {
            MatchCollection matchs = new Regex("<img\\b[^<>]*?\\bsrc[\\s\\t\\r\\n]*=[\\s\\t\\r\\n]*[\"']?[\\s\\t\\r\\n]*(?<imgUrl>[^\\s\\t\\r\\n\"'<>]*)[^<>]*?/?[\\s\\t\\r\\n]*>", RegexOptions.IgnoreCase).Matches(sHtmlText);
            int num = 0;
            string[] strArray = new string[matchs.Count];
            foreach (Match match in matchs)
            {
                strArray[num++] = match.Groups["imgUrl"].Value;
            }
            return strArray;
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
            DataColumn column12 = new DataColumn("Cid") {
                DataType = Type.GetType("System.Int64")
            };
            table.Columns.Add(column12);
            DataColumn column13 = new DataColumn("StuffStatus") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column13);
            DataColumn column14 = new DataColumn("Num") {
                DataType = Type.GetType("System.Int64")
            };
            table.Columns.Add(column14);
            DataColumn column15 = new DataColumn("LocationState") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column15);
            DataColumn column16 = new DataColumn("LocationCity") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column16);
            DataColumn column17 = new DataColumn("FreightPayer") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column17);
            DataColumn column18 = new DataColumn("PostFee") {
                DataType = Type.GetType("System.Decimal")
            };
            table.Columns.Add(column18);
            DataColumn column19 = new DataColumn("ExpressFee") {
                DataType = Type.GetType("System.Decimal")
            };
            table.Columns.Add(column19);
            DataColumn column20 = new DataColumn("EMSFee") {
                DataType = Type.GetType("System.Decimal")
            };
            table.Columns.Add(column20);
            DataColumn column21 = new DataColumn("HasInvoice") {
                DataType = Type.GetType("System.Boolean")
            };
            table.Columns.Add(column21);
            DataColumn column22 = new DataColumn("HasWarranty") {
                DataType = Type.GetType("System.Boolean")
            };
            table.Columns.Add(column22);
            DataColumn column23 = new DataColumn("HasDiscount") {
                DataType = Type.GetType("System.Boolean")
            };
            table.Columns.Add(column23);
            DataColumn column24 = new DataColumn("ValidThru") {
                DataType = Type.GetType("System.Int64")
            };
            table.Columns.Add(column24);
            DataColumn column25 = new DataColumn("ListTime") {
                DataType = Type.GetType("System.DateTime")
            };
            table.Columns.Add(column25);
            DataColumn column26 = new DataColumn("PropertyAlias") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column26);
            DataColumn column27 = new DataColumn("InputPids") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column27);
            DataColumn column28 = new DataColumn("InputStr") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column28);
            DataColumn column29 = new DataColumn("SkuProperties") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column29);
            DataColumn column30 = new DataColumn("SkuQuantities") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column30);
            DataColumn column31 = new DataColumn("SkuPrices") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column31);
            DataColumn column32 = new DataColumn("SkuOuterIds") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column32);
            DataColumn column33 = new DataColumn("Props") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column33);
            DataColumn column34 = new DataColumn("TaobaoProductId") {
                DataType = Type.GetType("System.Int64")
            };
            table.Columns.Add(column34);
            return table;
        }

        public override object[] ParseIndexes(params object[] importParams)
        {
            throw new NotImplementedException();
        }

        public override object[] ParseProductData(params object[] importParams)
        {
            string str = "";
            string[] strArray = null;
            string str2 = "";
            string str3 = "";
            string str4 = "";
            string str5 = string.Empty;
            string str6 = string.Empty;
            string str7 = string.Empty;
            string str8 = string.Empty;
            string pattern = @"(?<Price>[^:]+):(?<Quantities>[^:]+):(?<Outid>[^:]*):(?<Skuprop>[^;]+;(?:\d+:\d+;)?)";
            Regex regex = null;
            string str10 = "tbi";
            StringBuilder builder = new StringBuilder();
            HttpContext current = HttpContext.Current;
            string path = (string) importParams[0];
            string[] directories = Directory.GetDirectories(path);
            int index = 0;
            while (index < directories.Length)
            {
                string str12 = directories[index];
                this.imagefolder = str12;
                break;
            }
            if (!Directory.Exists(this.imagefolder))
            {
                return null;
            }
            string[] strArray4 = Directory.GetFiles(path, "*.csv", SearchOption.TopDirectoryOnly);
            int num8 = 0;
            while (num8 < strArray4.Length)
            {
                string str13 = strArray4[num8];
                this.csvfmtfile = str13;
                break;
            }
            if (!File.Exists(this.csvfmtfile))
            {
                return null;
            }
            DataTable productSet = this.GetProductSet();
            using (StreamReader reader = new StreamReader(this.csvfmtfile, this.m_EncodingType))
            {
                builder.Append(reader.ReadToEnd());
            }
            if (builder.ToString().StartsWith("version"))
            {
                int num = 0;
                int length = 0;
                length = 0;
                while (length < builder.Length)
                {
                    char ch = builder[length];
                    if (ch.Equals('\n'))
                    {
                        num++;
                    }
                    if (num == 2)
                    {
                        break;
                    }
                    length++;
                }
                builder.Remove(0, length);
            }
            using (StreamWriter writer = new StreamWriter(this.csvfmtfile, false, this.m_EncodingType))
            {
                writer.Write(builder.Replace("\t", ",").ToString());
            }
            using (CsvReader reader2 = new CsvReader(new StreamReader(this.csvfmtfile, this.m_EncodingType), true, ','))
            {
                foreach (string str14 in csvFields)
                {
                    if (reader2.GetFieldIndex(str14) < 0)
                    {
                        throw new Exception("检查数据包格式出错，非淘宝助理5.0导出的数据包,缺少必要的字段：" + str14);
                    }
                }
                DataRow row = null;
                while ((reader2 != null) && reader2.ReadNextRecord())
                {
                    row = productSet.NewRow();
                    row.BeginEdit();
                    str = "";
                    if (!string.IsNullOrEmpty(reader2["宝贝名称"]))
                    {
                        row["ProductName"] = reader2["宝贝名称"];
                    }
                    row["Cid"] = 0;
                    if (!string.IsNullOrEmpty(reader2["宝贝类目"]))
                    {
                        try
                        {
                            row["Cid"] = Convert.ToInt32(reader2["宝贝类目"]);
                        }
                        catch
                        {
                            row["Cid"] = 0;
                        }
                    }
                    if (!string.IsNullOrEmpty(reader2["新旧程度"]))
                    {
                        row["StuffStatus"] = (reader2["新旧程度"] == "1") ? "new" : "second";
                    }
                    else
                    {
                        row["StuffStatus"] = "new";
                    }
                    row["LocationState"] = reader2["省"];
                    row["LocationCity"] = reader2["城市"];
                    if (!string.IsNullOrEmpty(reader2["宝贝价格"]))
                    {
                        try
                        {
                            row["SalePrice"] = Convert.ToDecimal(reader2["宝贝价格"]);
                        }
                        catch
                        {
                            row["SalePrice"] = 0;
                        }
                    }
                    if (!string.IsNullOrEmpty(reader2["宝贝数量"]))
                    {
                        try
                        {
                            row["Num"] = row["Stock"] = Convert.ToInt32(reader2["宝贝数量"]);
                        }
                        catch
                        {
                            row["Num"] = row["Stock"] = "0";
                        }
                    }
                    if (!string.IsNullOrEmpty(reader2["有效期"]))
                    {
                        try
                        {
                            row["ValidThru"] = long.Parse(reader2["有效期"]);
                        }
                        catch
                        {
                            row["ValidThru"] = "7";
                        }
                    }
                    row["FreightPayer"] = (reader2["运费承担"] == "1") ? "seller" : "buyer";
                    if (!string.IsNullOrEmpty(reader2["平邮"]))
                    {
                        try
                        {
                            row["PostFee"] = decimal.Parse(reader2["平邮"]);
                        }
                        catch
                        {
                            row["PostFee"] = 0.0;
                        }
                    }
                    if (!string.IsNullOrEmpty(reader2["EMS"]))
                    {
                        try
                        {
                            row["EMSFee"] = decimal.Parse(reader2["EMS"]);
                        }
                        catch
                        {
                            row["EMSFee"] = 0.0;
                        }
                    }
                    if (!string.IsNullOrEmpty(reader2["快递"]))
                    {
                        try
                        {
                            row["ExpressFee"] = decimal.Parse(reader2["快递"]);
                        }
                        catch
                        {
                            row["ExpressFee"] = 0.0;
                        }
                    }
                    row["HasInvoice"] = reader2["发票"] == "1";
                    row["HasWarranty"] = reader2["保修"] == "1";
                    if (!string.IsNullOrEmpty(reader2["开始时间"]))
                    {
                        try
                        {
                            row["ListTime"] = DateTime.Parse(reader2["开始时间"]);
                        }
                        catch
                        {
                            row["ListTime"].ToString();
                        }
                    }
                    if (!string.IsNullOrEmpty(reader2["宝贝描述"]))
                    {
                        string str15 = string.Format(@"\Storage\master\gallery\{0}\", DateTime.Now.ToString("yyyyMM"));
                        string str16 = this.m_webDir + str15;
                        if (!Directory.Exists(str16))
                        {
                            Directory.CreateDirectory(str16);
                        }
                        StringBuilder builder2 = new StringBuilder();
                        builder2.Append(this.Trim(reader2["宝贝描述"].Replace("\"\"", "\"").Replace("alt=\"\"", "").Replace("alt=\"", "").Replace("alt=''", "")));
                        string str17 = path + @"\products\contentPic";
                        if (Directory.Exists(str17))
                        {
                            int num3 = 0;
                            string[] htmlImageUrlList = GetHtmlImageUrlList(builder2.ToString());
                            string str18 = "";
                            foreach (string str19 in htmlImageUrlList)
                            {
                                num3 = str19.IndexOf("contentPic");
                                if (num3 > 0)
                                {
                                    str18 = str17 + str19.Substring(num3 + 10).Replace("/", @"\");
                                    if (!File.Exists(str16 + Path.GetFileName(str18)))
                                    {
                                        File.Copy(str18, str16 + Path.GetFileName(str18));
                                    }
                                    builder2.Replace(str19, str15 + Path.GetFileName(str18));
                                }
                            }
                        }
                        row["Description"] = builder2.ToString();
                    }
                    row["Props"] = reader2["宝贝属性"];
                    row["HasDiscount"] = reader2["会员打折"] == "1";
                    str = this.Trim(reader2["新图片"]);
                    if (!string.IsNullOrEmpty(str))
                    {
                        strArray = str.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < strArray.Length; i++)
                        {
                            if (i > 4)
                            {
                                break;
                            }
                            str2 = strArray[i].Substring(0, strArray[i].IndexOf(":"));
                            str3 = str2 + ".jpg";
                            if (File.Exists(Path.Combine(this.imagefolder, str2 + ".tbi")))
                            {
                                str10 = ".tbi";
                            }
                            else if (File.Exists(Path.Combine(this.imagefolder, str2 + ".jpg")))
                            {
                                str10 = ".jpg";
                            }
                            else if (File.Exists(Path.Combine(this.imagefolder, str2 + ".gif")))
                            {
                                str10 = ".gif";
                            }
                            else if (File.Exists(Path.Combine(this.imagefolder, str2 + ".png")))
                            {
                                str10 = ".png";
                            }
                            if (File.Exists(Path.Combine(this.imagefolder, str2 + str10)))
                            {
                                try
                                {
                                    File.Copy(Path.Combine(this.imagefolder, str2 + str10), this.m_webDir + @"\Storage\master\product\images\" + str3, true);
                                    switch (i)
                                    {
                                        case 0:
                                        {
                                            row["ImageUrl1"] = "/Storage/master/product/images/" + str3;
                                            continue;
                                        }
                                        case 1:
                                        {
                                            row["ImageUrl2"] = "/Storage/master/product/images/" + str3;
                                            continue;
                                        }
                                        case 2:
                                        {
                                            row["ImageUrl3"] = "/Storage/master/product/images/" + str3;
                                            continue;
                                        }
                                        case 3:
                                        {
                                            row["ImageUrl4"] = "/Storage/master/product/images/" + str3;
                                            continue;
                                        }
                                        case 4:
                                        {
                                            row["ImageUrl5"] = "/Storage/master/product/images/" + str3;
                                            continue;
                                        }
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(reader2["商家编码"]))
                    {
                        row["SKU"] = reader2["商家编码"];
                    }
                    else
                    {
                        row["SKU"] = this.GenerateId();
                    }
                    str4 = reader2["销售属性组合"];
                    str5 = string.Empty;
                    str6 = string.Empty;
                    str7 = string.Empty;
                    str8 = string.Empty;
                    if (!string.IsNullOrEmpty(str4))
                    {
                        regex = new Regex(pattern, RegexOptions.IgnoreCase);
                        int num5 = 1;
                        foreach (Match match in regex.Matches(str4))
                        {
                            str6 = str6 + match.Groups["Quantities"].Value + ",";
                            str5 = str5 + string.Format("{0:F2}", decimal.Parse(match.Groups["Price"].Value.Replace(";", ""))) + ",";
                            str7 = str7 + (string.IsNullOrEmpty(match.Groups["Outid"].Value) ? (row["SKU"] + "_" + num5++) : match.Groups["Outid"].Value) + ",";
                            str8 = str8 + match.Groups["Skuprop"].Value.ToString().Substring(0, match.Groups["Skuprop"].Value.ToString().Length - 1) + ",";
                        }
                        if (str6.Length > 0)
                        {
                            str6 = str6.Substring(0, str6.Length - 1);
                        }
                        if (str5.Length > 0)
                        {
                            str5 = str5.Substring(0, str5.Length - 1);
                        }
                        if (str7.Length > 0)
                        {
                            str7 = str7.Substring(0, str7.Length - 1);
                        }
                        if (str8.Length > 0)
                        {
                            str8 = str8.Substring(0, str8.Length - 1);
                        }
                    }
                    row["SkuProperties"] = str8;
                    row["SkuQuantities"] = str6;
                    row["SkuPrices"] = str5;
                    row["SkuOuterIds"] = str7;
                    row["InputPids"] = reader2["用户输入ID串"];
                    row["InputStr"] = reader2["用户输入名-值对"];
                    row["PropertyAlias"] = reader2["销售属性别名"];
                    try
                    {
                        if ((reader2["物流重量"] != null) && !string.IsNullOrEmpty(reader2["物流重量"].ToString()))
                        {
                            decimal result = 0M;
                            if (decimal.TryParse(reader2["物流重量"], out result))
                            {
                                row["Weight"] = decimal.ToInt32(result * 1000M);
                            }
                            else
                            {
                                row["Weight"] = 0;
                            }
                        }
                    }
                    catch
                    {
                        row["Weight"] = 0;
                    }
                    row.EndEdit();
                    productSet.Rows.Add(row);
                }
            }
            return new object[] { productSet };
        }

        public override string PrepareDataFiles(params object[] initParams)
        {
            try
            {
                string path = initParams[0].ToString();
                this.m_workDir = this.m_baseDir.CreateSubdirectory(Path.GetFileNameWithoutExtension(path));
                using (ZipFile file = ZipFile.Read(Path.Combine(this.m_baseDir.FullName, path)))
                {
                    int count = file.Count;
                    foreach (ZipEntry entry in file)
                    {
                        entry.Extract(this.m_workDir.FullName, ExtractExistingFileAction.OverwriteSilently);
                    }
                }
                DirectoryInfo info = null;
                foreach (string str2 in Directory.GetDirectories(this.m_workDir.FullName))
                {
                    try
                    {
                        info = new DirectoryInfo(str2);
                        if (info.Name.ToLowerInvariant() != "products")
                        {
                            info.MoveTo(Path.Combine(this.m_workDir.FullName, "products"));
                        }
                    }
                    catch
                    {
                    }
                }
                FileInfo info2 = null;
                foreach (string str3 in Directory.GetFiles(this.m_workDir.FullName))
                {
                    try
                    {
                        info2 = new FileInfo(str3);
                        if (info2.Name != "products.csv")
                        {
                            info2.MoveTo(Path.Combine(this.m_workDir.FullName, "products.csv"));
                        }
                    }
                    catch
                    {
                    }
                }
                return this.m_workDir.FullName;
            }
            catch (Exception)
            {
                return "";
            }
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
                return this.m_importTo;
            }
        }

        public override Target Source
        {
            get
            {
                return this.m_source;
            }
        }
    }
}

