namespace Hishop.Transfers.YfxExporters
{
    using Hishop.TransferManager;
    using Ionic.Zip;
    using Ionic.Zlib;
    using System;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Xml;

    public class Yfx1_2_to_Yfx1_2 : ExportAdapter
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
        private DirectoryInfo _typeImagesDir;
        private readonly string _url;
        private DirectoryInfo _workDir;
        private readonly string _zipFilename;
        private const string ExportVersion = "1.2";
        private const string IndexFilename = "indexes.xml";
        private const string ProductFilename = "products.xml";

        public Yfx1_2_to_Yfx1_2()
        {
            this._encoding = Encoding.UTF8;
            this._exportTo = new YfxTarget("1.2");
            this._source = new YfxTarget("1.2");
        }

        public Yfx1_2_to_Yfx1_2(params object[] exportParams) : this()
        {
            this._exportData = (DataSet) exportParams[0];
            this._includeCostPrice = (bool) exportParams[1];
            this._includeStock = (bool) exportParams[2];
            this._includeImages = (bool) exportParams[3];
            this._url = (string) exportParams[4];
            this._applicationPath = (string) exportParams[5];
            this._baseDir = new DirectoryInfo(HttpContext.Current.Request.MapPath("~/storage/data/yfx"));
            this._flag = DateTime.Now.ToString("yyyyMMddHHmmss");
            this._zipFilename = string.Format("YFX.{0}.{1}.zip", "1.2", this._flag);
        }

        public override void DoExport()
        {
            this._workDir = this._baseDir.CreateSubdirectory(this._flag);
            this._typeImagesDir = this._workDir.CreateSubdirectory("images1");
            this._productImagesDir = this._workDir.CreateSubdirectory("images2");
            string path = Path.Combine(this._workDir.FullName, "indexes.xml");
            using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                XmlWriter indexWriter = new XmlTextWriter(stream, this._encoding);
                indexWriter.WriteStartDocument();
                indexWriter.WriteStartElement("indexes");
                indexWriter.WriteAttributeString("version", "1.2");
                indexWriter.WriteAttributeString("QTY", this._exportData.Tables["products"].Rows.Count.ToString(CultureInfo.InvariantCulture));
                indexWriter.WriteAttributeString("includeCostPrice", this._includeCostPrice.ToString());
                indexWriter.WriteAttributeString("includeStock", this._includeStock.ToString());
                indexWriter.WriteAttributeString("includeImages", this._includeImages.ToString());
                this.WriteIndexes(indexWriter);
                indexWriter.WriteEndElement();
                indexWriter.WriteEndDocument();
                indexWriter.Close();
            }
            string str2 = Path.Combine(this._workDir.FullName, "products.xml");
            using (FileStream stream2 = new FileStream(str2, FileMode.Create, FileAccess.Write))
            {
                XmlWriter productWriter = new XmlTextWriter(stream2, this._encoding);
                productWriter.WriteStartDocument();
                productWriter.WriteStartElement("products");
                this.WriteProducts(productWriter);
                productWriter.WriteEndElement();
                productWriter.WriteEndDocument();
                productWriter.Close();
            }
            using (ZipFile file = new ZipFile())
            {
                file.CompressionLevel = CompressionLevel.Default;
                file.AddFile(path, "");
                file.AddFile(str2, "");
                file.AddDirectory(this._typeImagesDir.FullName, this._typeImagesDir.Name);
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

        private void WriteIndexes(XmlWriter indexWriter)
        {
            indexWriter.WriteStartElement("types");
            foreach (DataRow row in this._exportData.Tables["types"].Rows)
            {
                indexWriter.WriteStartElement("type");
                indexWriter.WriteElementString("typeId", row["TypeId"].ToString());
                TransferHelper.WriteCDataElement(indexWriter, "typeName", row["TypeName"].ToString());
                TransferHelper.WriteCDataElement(indexWriter, "remark", row["Remark"].ToString());
                indexWriter.WriteStartElement("attributes");
                foreach (DataRow row2 in this._exportData.Tables["attributes"].Select("TypeId=" + row["TypeId"].ToString()))
                {
                    indexWriter.WriteStartElement("attribute");
                    indexWriter.WriteElementString("attributeId", row2["AttributeId"].ToString());
                    TransferHelper.WriteCDataElement(indexWriter, "attributeName", row2["AttributeName"].ToString());
                    indexWriter.WriteElementString("displaySequence", row2["DisplaySequence"].ToString());
                    indexWriter.WriteElementString("usageMode", row2["UsageMode"].ToString());
                    indexWriter.WriteElementString("useAttributeImage", row2["UseAttributeImage"].ToString());
                    indexWriter.WriteStartElement("values");
                    foreach (DataRow row3 in this._exportData.Tables["values"].Select("AttributeId=" + row2["AttributeId"].ToString()))
                    {
                        indexWriter.WriteStartElement("value");
                        indexWriter.WriteElementString("valueId", row3["ValueId"].ToString());
                        indexWriter.WriteElementString("displaySequence", row3["DisplaySequence"].ToString());
                        TransferHelper.WriteCDataElement(indexWriter, "valueStr", row3["ValueStr"].ToString());
                        TransferHelper.WriteImageElement(indexWriter, "image", this._includeImages, row3["ImageUrl"].ToString(), this._typeImagesDir);
                        indexWriter.WriteEndElement();
                    }
                    indexWriter.WriteEndElement();
                    indexWriter.WriteEndElement();
                }
                indexWriter.WriteEndElement();
                indexWriter.WriteEndElement();
            }
            indexWriter.WriteEndElement();
            this._exportData.Tables.Remove("values");
            this._exportData.Tables.Remove("attributes");
            this._exportData.Tables.Remove("types");
        }

        private void WriteProducts(XmlWriter productWriter)
        {
            productWriter.WriteStartElement("products");
            foreach (DataRow row in this._exportData.Tables["products"].Rows)
            {
                productWriter.WriteStartElement("product");
                productWriter.WriteElementString("productId", row["ProductId"].ToString());
                productWriter.WriteElementString("typeId", row["TypeId"].ToString());
                TransferHelper.WriteCDataElement(productWriter, "productName", row["ProductName"].ToString());
                TransferHelper.WriteCDataElement(productWriter, "productCode", row["ProductCode"].ToString());
                TransferHelper.WriteCDataElement(productWriter, "shortDescription", row["ShortDescription"].ToString());
                productWriter.WriteElementString("unit", row["Unit"].ToString());
                TransferHelper.WriteCDataElement(productWriter, "description", row["Description"].ToString().Replace(string.Format("src=\"{0}/Storage/master/gallery", this._applicationPath), string.Format("src=\"{0}/Storage/master/gallery", this._url)));
                TransferHelper.WriteCDataElement(productWriter, "title", row["Title"].ToString());
                TransferHelper.WriteCDataElement(productWriter, "meta_Description", row["Meta_Description"].ToString());
                TransferHelper.WriteCDataElement(productWriter, "meta_Keywords", row["Meta_Keywords"].ToString());
                productWriter.WriteElementString("saleStatus", row["SaleStatus"].ToString());
                if ((row["ImageUrl1"] != DBNull.Value) && !row["ImageUrl1"].ToString().StartsWith("http://"))
                {
                    TransferHelper.WriteImageElement(productWriter, "image1", this._includeImages, row["ImageUrl1"].ToString(), this._productImagesDir);
                }
                if ((row["ImageUrl2"] != DBNull.Value) && !row["ImageUrl2"].ToString().StartsWith("http://"))
                {
                    TransferHelper.WriteImageElement(productWriter, "image2", this._includeImages, row["ImageUrl2"].ToString(), this._productImagesDir);
                }
                if ((row["ImageUrl3"] != DBNull.Value) && !row["ImageUrl3"].ToString().StartsWith("http://"))
                {
                    TransferHelper.WriteImageElement(productWriter, "image3", this._includeImages, row["ImageUrl3"].ToString(), this._productImagesDir);
                }
                if ((row["ImageUrl4"] != DBNull.Value) && !row["ImageUrl4"].ToString().StartsWith("http://"))
                {
                    TransferHelper.WriteImageElement(productWriter, "image4", this._includeImages, row["ImageUrl4"].ToString(), this._productImagesDir);
                }
                if ((row["ImageUrl5"] != DBNull.Value) && !row["ImageUrl5"].ToString().StartsWith("http://"))
                {
                    TransferHelper.WriteImageElement(productWriter, "image5", this._includeImages, row["ImageUrl5"].ToString(), this._productImagesDir);
                }
                productWriter.WriteElementString("marketPrice", row["MarketPrice"].ToString());
                productWriter.WriteElementString("lowestSalePrice", row["LowestSalePrice"].ToString());
                productWriter.WriteElementString("penetrationStatus", row["PenetrationStatus"].ToString());
                productWriter.WriteElementString("hasSKU", row["HasSKU"].ToString());
                DataRow[] rowArray = this._exportData.Tables["productAttributes"].Select("ProductId=" + row["ProductId"].ToString());
                productWriter.WriteStartElement("attributes");
                foreach (DataRow row2 in rowArray)
                {
                    productWriter.WriteStartElement("attribute");
                    productWriter.WriteElementString("attributeId", row2["AttributeId"].ToString());
                    productWriter.WriteElementString("valueId", row2["ValueId"].ToString());
                    productWriter.WriteEndElement();
                }
                productWriter.WriteEndElement();
                DataRow[] rowArray2 = this._exportData.Tables["skus"].Select("ProductId=" + row["ProductId"].ToString());
                productWriter.WriteStartElement("skus");
                foreach (DataRow row3 in rowArray2)
                {
                    productWriter.WriteStartElement("sku");
                    productWriter.WriteElementString("skuId", row3["SkuId"].ToString());
                    productWriter.WriteElementString("sKU", row3["SKU"].ToString());
                    if (this._includeCostPrice)
                    {
                        productWriter.WriteElementString("costPrice", row3["CostPrice"].ToString());
                    }
                    productWriter.WriteElementString("weight", row3["Weight"].ToString());
                    if (this._includeStock)
                    {
                        productWriter.WriteElementString("stock", row3["Stock"].ToString());
                    }
                    productWriter.WriteElementString("alertStock", row3["AlertStock"].ToString());
                    productWriter.WriteElementString("salePrice", row3["SalePrice"].ToString());
                    productWriter.WriteElementString("purchasePrice", row3["PurchasePrice"].ToString());
                    DataRow[] rowArray3 = this._exportData.Tables["skuItems"].Select("SkuId='" + row3["SkuId"].ToString() + "'");
                    productWriter.WriteStartElement("skuItems");
                    foreach (DataRow row4 in rowArray3)
                    {
                        productWriter.WriteStartElement("skuItem");
                        productWriter.WriteElementString("skuId", row4["SkuId"].ToString());
                        productWriter.WriteElementString("attributeId", row4["AttributeId"].ToString());
                        productWriter.WriteElementString("valueId", row4["ValueId"].ToString());
                        productWriter.WriteEndElement();
                    }
                    productWriter.WriteEndElement();
                    productWriter.WriteEndElement();
                }
                productWriter.WriteEndElement();
                productWriter.WriteEndElement();
            }
            productWriter.WriteEndElement();
            this._exportData.Tables.Remove("skuItems");
            this._exportData.Tables.Remove("skus");
            this._exportData.Tables.Remove("products");
            this._exportData.Tables.Remove("productAttributes");
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

