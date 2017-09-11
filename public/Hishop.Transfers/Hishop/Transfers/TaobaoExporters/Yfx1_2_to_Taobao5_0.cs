namespace Hishop.Transfers.TaobaoExporters
{
    using Hishop.TransferManager;
    using Ionic.Zip;
    using Ionic.Zlib;
    using System;
    using System.Data;
    using System.IO;
    using System.Text;
    using System.Web;

    public class Yfx1_2_to_Taobao5_0 : ExportAdapter
    {
        private readonly string _applicationPath;
        private readonly DirectoryInfo _baseDir;
        private readonly Encoding _encoding;
        private readonly DataSet _exportData;
        private readonly Target _exportTo;
        private readonly string _flag;
        private readonly bool _includeCostPrice;
        private readonly bool _includeImages;
        private readonly bool _includeStock;
        private DirectoryInfo _productImagesDir;
        private readonly Target _source;
        private readonly string _url;
        private DirectoryInfo _workDir;
        private readonly string _zipFilename;
        private const string ExportVersion = "5.0";
        private const string ProductFilename = "products.csv";

        public Yfx1_2_to_Taobao5_0()
        {
            this._encoding = Encoding.UTF8;
            this._exportTo = new TbTarget("5.0");
            this._source = new YfxTarget("1.2");
        }

        public Yfx1_2_to_Taobao5_0(params object[] exportParams) : this()
        {
            this._exportData = (DataSet) exportParams[0];
            this._includeCostPrice = (bool) exportParams[1];
            this._includeStock = (bool) exportParams[2];
            this._includeImages = (bool) exportParams[3];
            this._url = (string) exportParams[4];
            this._applicationPath = (string) exportParams[5];
            this._baseDir = new DirectoryInfo(HttpContext.Current.Request.MapPath("~/storage/data/taobao"));
            this._flag = DateTime.Now.ToString("yyyyMMddHHmmss");
            this._zipFilename = string.Format("taobao.{0}.{1}.zip", "5.0", this._flag);
        }

        private string CopyImage(string imageUrl, int index)
        {
            string str = string.Empty;
            if (imageUrl.StartsWith("http://"))
            {
                return str;
            }
            imageUrl = this.Trim(imageUrl);
            string path = HttpContext.Current.Request.MapPath("~" + imageUrl);
            if (!File.Exists(path))
            {
                return str;
            }
            FileInfo info = new FileInfo(path);
            string str3 = info.Name.ToLower();
            if (((!str3.EndsWith(".jpg") && !str3.EndsWith(".gif")) && (!str3.EndsWith(".jpeg") && !str3.EndsWith(".png"))) && !str3.EndsWith(".bmp"))
            {
                return str;
            }
            str3 = str3.Replace(info.Extension.ToLower(), ".tbi");
            info.CopyTo(Path.Combine(this._productImagesDir.FullName, str3), true);
            return (str + str3.Replace(".tbi", string.Format(":1:{0}:|;", index - 1)));
        }

        public override void DoExport()
        {
            this._workDir = this._baseDir.CreateSubdirectory(this._flag);
            this._productImagesDir = this._workDir.CreateSubdirectory("products");
            string path = Path.Combine(this._workDir.FullName, "products.csv");
            using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                string productCSV = this.GetProductCSV();
                UnicodeEncoding encoding = new UnicodeEncoding();
                int byteCount = encoding.GetByteCount(productCSV);
                byte[] preamble = encoding.GetPreamble();
                byte[] dst = new byte[preamble.Length + byteCount];
                Buffer.BlockCopy(preamble, 0, dst, 0, preamble.Length);
                encoding.GetBytes(productCSV.ToCharArray(), 0, productCSV.Length, dst, preamble.Length);
                stream.Write(dst, 0, dst.Length);
            }
            using (ZipFile file = new ZipFile())
            {
                file.CompressionLevel = CompressionLevel.Default;
                file.AddFile(path, "");
                file.AddDirectory(this._productImagesDir.FullName, this._productImagesDir.Name);
                HttpResponse response = HttpContext.Current.Response;
                response.ContentType = "application/x-zip-compressed";
                response.ContentEncoding = this._encoding;
                response.AddHeader("Content-Disposition", "attachment; filename=" + this._zipFilename);
                response.Clear();
                file.Save(response.OutputStream);
                this._workDir.Delete(true);
                response.Flush();
                response.Close();
            }
        }

        private string GetProductCSV()
        {
            StringBuilder builder = new StringBuilder();
            string format = "\"{0}\"\t{1}\t\"{2}\"\t{3}\t\"{4}\"\t\"{5}\"\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}\t{13}\t{14}\t{15}\t{16}\t{17}\t{18}\t\"{19}\"\t\"{20}\"\t\"{21}\"\t{22}\t{23}\t\"{24}\"\t{25}\t\"{26}\"\t{27}\t\"{28}\"\t\"{29}\"\t\"{30}\"\t\"{31}\"\t\"{32}\"\t\"{33}\"\t\"{34}\"\t{35}\t{36}\t{37}\t{38}\t\"{39}\"\t{40}\r\n";
            builder.Append("version 1.00\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\r\n");
            builder.Append("title\tcid\tseller_cids\tstuff_status\tlocation_state\tlocation_city\titem_type\tprice\tauction_increment\tnum\tvalid_thru\tfreight_payer\tpost_fee\tems_fee");
            builder.Append("\texpress_fee\thas_invoice\thas_warranty\tapprove_status\thas_showcase\tlist_time\tdescription\tcateProps\tpostage_id\thas_discount\tmodified\tupload_fail_msg");
            builder.Append("\tpicture_status\tauction_point\tpicture\tvideo\tskuProps\tinputPids\tinputValues\touter_id\tpropAlias\tauto_fill\tnum_id\tlocal_cid\tnavigation_type\tuser_name\tsyncStatus\r\n");
            builder.Append("宝贝名称\t宝贝类目\t店铺类目\t新旧程度\t省\t城市\t出售方式\t宝贝价格\t加价幅度\t宝贝数量\t有效期\t运费承担\t平邮\tEMS\t快递\t发票\t保修\t放入仓库\t橱窗推荐\t开始时间\t宝贝描述");
            builder.Append("\t宝贝属性\t邮费模版ID\t会员打折\t修改时间\t上传状态\t图片状态\t返点比例\t新图片\t视频\t销售属性组合\t用户输入ID串\t用户输入名-值对\t商家编码\t销售属性别名\t代充类型\t数字ID\t本地ID");
            builder.Append("\t宝贝分类\t账户名称\t宝贝状态\r\n");
            foreach (DataRow row in this._exportData.Tables["products"].Rows)
            {
                string str2;
                if (row["Description"] != DBNull.Value)
                {
                    str2 = this.Trim((string) row["Description"]).Replace(string.Format("src=\"{0}/Storage/master/gallery", this._applicationPath), string.Format("src=\"{0}/Storage/master/gallery", this._url));
                }
                else
                {
                    str2 = string.Empty;
                }
                if (row["ShortDescription"] != DBNull.Value)
                {
                    string str3 = this.Trim(Convert.ToString(row["ShortDescription"]).Trim());
                    if (!string.IsNullOrEmpty(str3) && (str3.Length > 0))
                    {
                        str2 = str3 + "<br/>" + str2;
                    }
                }
                str2 = str2.Replace("\r\n", "").Replace("\r", "").Replace("\n", "").Replace("\"", "\"\"");
                string str4 = string.Empty;
                if (row["ImageUrl1"] != DBNull.Value)
                {
                    str4 = str4 + this.CopyImage((string) row["ImageUrl1"], 1);
                }
                if (row["ImageUrl2"] != DBNull.Value)
                {
                    str4 = str4 + this.CopyImage((string) row["ImageUrl2"], 2);
                }
                if (row["ImageUrl3"] != DBNull.Value)
                {
                    str4 = str4 + this.CopyImage((string) row["ImageUrl3"], 3);
                }
                if (row["ImageUrl4"] != DBNull.Value)
                {
                    str4 = str4 + this.CopyImage((string) row["ImageUrl4"], 4);
                }
                if (row["ImageUrl5"] != DBNull.Value)
                {
                    str4 = str4 + this.CopyImage((string) row["ImageUrl5"], 5);
                }
                DataRow[] rowArray = this._exportData.Tables["skus"].Select("ProductId=" + row["ProductId"].ToString(), "SalePrice desc");
                string str5 = "0";
                int num = 0;
                string str6 = "1";
                string str7 = "0";
                string str8 = "0";
                string str9 = "0";
                string str10 = "";
                string str11 = "";
                string str12 = "";
                string str13 = Convert.ToString(row["productcode"]);
                string str14 = "";
                string str15 = "";
                string str16 = "";
                string str17 = "";
                string str18 = "";
                string str19 = "";
                string str20 = "";
                string str21 = "";
                string str22 = "";
                string str23 = "";
                string str24 = "";
                string str25 = "";
                DataRow[] rowArray2 = this._exportData.Tables["TaobaoSku"].Select("ProductId=" + row["Productid"].ToString());
                if (rowArray2.Length > 0)
                {
                    if (this._includeStock)
                    {
                        if ((rowArray2[0]["SkuQuantities"] != null) && (rowArray2[0]["SkuQuantities"].ToString() != ""))
                        {
                            string[] strArray = null;
                            if (rowArray2[0]["SkuQuantities"].ToString().Contains(","))
                            {
                                strArray = rowArray2[0]["SkuQuantities"].ToString().Split(new char[] { ',' });
                            }
                            else
                            {
                                strArray = new string[] { rowArray2[0]["SkuQuantities"].ToString() };
                            }
                            foreach (string str26 in strArray)
                            {
                                num += Convert.ToInt32(str26);
                            }
                        }
                        else
                        {
                            num += Convert.ToInt32(rowArray2[0]["Num"]);
                        }
                    }
                    str20 = Convert.ToString(rowArray2[0]["LocationState"]);
                    str21 = Convert.ToString(rowArray2[0]["LocationCity"]);
                    str22 = (Convert.ToString(rowArray2[0]["HasInvoice"]).ToLower() == "true") ? "1" : "0";
                    str23 = (Convert.ToString(rowArray2[0]["HasWarranty"]).ToLower() == "true") ? "1" : "0";
                    str24 = (Convert.ToString(rowArray2[0]["HasDiscount"]).ToLower() == "true") ? "1" : "0";
                    str25 = (rowArray2[0]["StuffStatus"].ToString() == "new") ? "1" : "0";
                    if (Convert.ToString(rowArray2[0]["FreightPayer"]) == "buyer")
                    {
                        str6 = "2";
                        str7 = Convert.ToString(rowArray2[0]["PostFee"]);
                        str8 = Convert.ToString(rowArray2[0]["ExpressFee"]);
                        str9 = Convert.ToString(rowArray2[0]["EMSFee"]);
                    }
                    str11 = Convert.ToString(rowArray2[0]["Cid"]);
                    str14 = Convert.ToString(rowArray2[0]["PropertyAlias"]);
                    str10 = Convert.ToString(rowArray2[0]["inputpids"]);
                    str12 = Convert.ToString(rowArray2[0]["inputstr"]);
                    str17 = Convert.ToString(rowArray2[0]["SkuQuantities"]);
                    str18 = Convert.ToString(rowArray2[0]["skuPrices"]);
                    str16 = Convert.ToString(rowArray2[0]["SkuProperties"]);
                    str19 = Convert.ToString(rowArray2[0]["SkuOuterIds"]);
                    if (!string.IsNullOrEmpty(str17))
                    {
                        string[] strArray2 = str17.Split(new char[] { ',' });
                        string[] strArray3 = str18.Split(new char[] { ',' });
                        string[] strArray4 = str19.Split(new char[] { ',' });
                        string[] strArray5 = str16.Split(new char[] { ',' });
                        for (int i = 0; i < strArray2.Length; i++)
                        {
                            string str27 = str15;
                            str15 = str27 + strArray3[i] + ":" + strArray2[i] + ":" + strArray4[i] + ":" + strArray5[i] + ";";
                        }
                    }
                }
                else if (this._includeStock && (rowArray.Length > 0))
                {
                    foreach (DataRow row2 in rowArray)
                    {
                        num += (int) row2["Stock"];
                    }
                }
                str5 = num.ToString();
                if (rowArray.Length > 0)
                {
                    builder.AppendFormat(format, new object[] { 
                        this.Trim(Convert.ToString(row["ProductName"])), str11, "", str25, str20, str21, "1", rowArray[0]["SalePrice"], "", str5, "14", str6, str7, str9, str8, str22, 
                        str23, "0", "0", "", str2, str14, "0", str24, DateTime.Now, "100", "", "0", str4, string.Empty, str15, str10, 
                        str12, str13, string.Empty, "0", "0", "0", "1", "", "1"
                     });
                }
            }
            return builder.Remove(builder.Length - 2, 2).ToString();
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

        public override Target ExportTo
        {
            get
            {
                return this._exportTo;
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

