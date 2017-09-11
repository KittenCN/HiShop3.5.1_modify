namespace Hishop.Transfers.YfxImporters
{
    using Hishop.TransferManager;
    using Ionic.Zip;
    using System;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Web;
    using System.Xml;

    public class Yfx1_2_from_Yfx1_2 : ImportAdapter
    {
        private readonly DirectoryInfo _baseDir = new DirectoryInfo(HttpContext.Current.Request.MapPath("~/storage/data/yfx"));
        private readonly Target _importTo = new YfxTarget("1.2");
        private DirectoryInfo _productImagesDir;
        private readonly Target _source = new YfxTarget("1.2");
        private DirectoryInfo _typeImagesDir;
        private DirectoryInfo _workDir;
        private const string IndexFilename = "indexes.xml";
        private const string ProductFilename = "products.xml";

        public override object[] CreateMapping(params object[] initParams)
        {
            XmlDocument document = (XmlDocument) initParams[0];
            string str = (string) initParams[1];
            DataSet mappingSet = this.GetMappingSet();
            XmlDocument indexesDoc = new XmlDocument();
            indexesDoc.Load(Path.Combine(str, "indexes.xml"));
            foreach (XmlNode node in document.DocumentElement.SelectNodes("//type"))
            {
                DataRow row = mappingSet.Tables["types"].NewRow();
                int mappedTypeId = int.Parse(node.Attributes["mappedTypeId"].Value);
                int num2 = int.Parse(node.Attributes["selectedTypeId"].Value);
                row["MappedTypeId"] = mappedTypeId;
                row["SelectedTypeId"] = num2;
                if (num2 == 0)
                {
                    XmlNode node2 = indexesDoc.SelectSingleNode("//type[typeId[text()='" + mappedTypeId + "']]");
                    row["TypeName"] = node2.SelectSingleNode("typeName").InnerText;
                    row["Remark"] = node2.SelectSingleNode("remark").InnerText;
                }
                mappingSet.Tables["types"].Rows.Add(row);
                XmlNodeList attributeNodeList = node.SelectNodes("attributes/attribute");
                this.MappingAttributes(mappedTypeId, mappingSet, attributeNodeList, indexesDoc, str);
            }
            mappingSet.AcceptChanges();
            return new object[] { mappingSet };
        }

        private DataSet GetMappingSet()
        {
            DataSet set = new DataSet();
            DataTable table = new DataTable("types");
            DataColumn column = new DataColumn("MappedTypeId") {
                Unique = true,
                DataType = Type.GetType("System.Int32")
            };
            table.Columns.Add(column);
            DataColumn column2 = new DataColumn("SelectedTypeId") {
                DataType = Type.GetType("System.Int32")
            };
            table.Columns.Add(column2);
            table.Columns.Add(new DataColumn("TypeName"));
            table.Columns.Add(new DataColumn("Remark"));
            table.PrimaryKey = new DataColumn[] { table.Columns["MappedTypeId"] };
            DataTable table2 = new DataTable("attributes");
            DataColumn column3 = new DataColumn("MappedAttributeId") {
                Unique = true,
                DataType = Type.GetType("System.Int32")
            };
            table2.Columns.Add(column3);
            DataColumn column4 = new DataColumn("SelectedAttributeId") {
                DataType = Type.GetType("System.Int32")
            };
            table2.Columns.Add(column4);
            table2.Columns.Add(new DataColumn("AttributeName"));
            table2.Columns.Add(new DataColumn("DisplaySequence"));
            DataColumn column5 = new DataColumn("MappedTypeId") {
                DataType = Type.GetType("System.Int32")
            };
            table2.Columns.Add(column5);
            table2.Columns.Add(new DataColumn("UsageMode"));
            table2.Columns.Add(new DataColumn("UseAttributeImage"));
            table2.PrimaryKey = new DataColumn[] { table2.Columns["MappedAttributeId"] };
            DataTable table3 = new DataTable("values");
            DataColumn column6 = new DataColumn("MappedValueId") {
                Unique = true,
                DataType = Type.GetType("System.Int32")
            };
            table3.Columns.Add(column6);
            DataColumn column7 = new DataColumn("SelectedValueId") {
                DataType = Type.GetType("System.Int32")
            };
            table3.Columns.Add(column7);
            DataColumn column8 = new DataColumn("MappedAttributeId") {
                DataType = Type.GetType("System.Int32")
            };
            table3.Columns.Add(column8);
            DataColumn column9 = new DataColumn("SelectedAttributeId") {
                DataType = Type.GetType("System.Int32")
            };
            table3.Columns.Add(column9);
            table3.Columns.Add(new DataColumn("DisplaySequence"));
            table3.Columns.Add(new DataColumn("ValueStr"));
            table3.Columns.Add(new DataColumn("ImageUrl"));
            set.Tables.Add(table);
            set.Tables.Add(table2);
            set.Tables.Add(table3);
            return set;
        }

        private DataSet GetProductSet()
        {
            DataSet set = new DataSet();
            DataTable table = new DataTable("products");
            DataColumn column = new DataColumn("ProductId") {
                Unique = true,
                DataType = Type.GetType("System.Int32")
            };
            table.Columns.Add(column);
            DataColumn column2 = new DataColumn("SelectedTypeId") {
                DataType = Type.GetType("System.Int32")
            };
            table.Columns.Add(column2);
            DataColumn column3 = new DataColumn("MappedTypeId") {
                DataType = Type.GetType("System.Int32")
            };
            table.Columns.Add(column3);
            DataColumn column4 = new DataColumn("ProductName") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column4);
            DataColumn column5 = new DataColumn("ProductCode") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column5);
            DataColumn column6 = new DataColumn("ShortDescription") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column6);
            DataColumn column7 = new DataColumn("Unit") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column7);
            DataColumn column8 = new DataColumn("Description") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column8);
            DataColumn column9 = new DataColumn("Title") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column9);
            DataColumn column10 = new DataColumn("Meta_Description") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column10);
            DataColumn column11 = new DataColumn("Meta_Keywords") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column11);
            DataColumn column12 = new DataColumn("SaleStatus") {
                DataType = Type.GetType("System.Int32")
            };
            table.Columns.Add(column12);
            DataColumn column13 = new DataColumn("ImageUrl1") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column13);
            DataColumn column14 = new DataColumn("ImageUrl2") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column14);
            DataColumn column15 = new DataColumn("ImageUrl3") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column15);
            DataColumn column16 = new DataColumn("ImageUrl4") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column16);
            DataColumn column17 = new DataColumn("ImageUrl5") {
                DataType = Type.GetType("System.String")
            };
            table.Columns.Add(column17);
            DataColumn column18 = new DataColumn("MarketPrice") {
                DataType = Type.GetType("System.Decimal")
            };
            table.Columns.Add(column18);
            DataColumn column19 = new DataColumn("LowestSalePrice") {
                DataType = Type.GetType("System.Decimal")
            };
            table.Columns.Add(column19);
            DataColumn column20 = new DataColumn("PenetrationStatus") {
                DataType = Type.GetType("System.Int32")
            };
            table.Columns.Add(column20);
            DataColumn column21 = new DataColumn("HasSKU") {
                DataType = Type.GetType("System.Boolean")
            };
            table.Columns.Add(column21);
            table.PrimaryKey = new DataColumn[] { table.Columns["ProductId"] };
            DataTable table2 = new DataTable("attributes");
            DataColumn column22 = new DataColumn("ProductId") {
                DataType = Type.GetType("System.Int32")
            };
            table2.Columns.Add(column22);
            DataColumn column23 = new DataColumn("SelectedAttributeId") {
                DataType = Type.GetType("System.Int32")
            };
            table2.Columns.Add(column23);
            DataColumn column24 = new DataColumn("MappedAttributeId") {
                DataType = Type.GetType("System.Int32")
            };
            table2.Columns.Add(column24);
            DataColumn column25 = new DataColumn("SelectedValueId") {
                DataType = Type.GetType("System.Int32")
            };
            table2.Columns.Add(column25);
            DataColumn column26 = new DataColumn("MappedValueId") {
                DataType = Type.GetType("System.Int32")
            };
            table2.Columns.Add(column26);
            table2.PrimaryKey = new DataColumn[] { table2.Columns["ProductId"], table2.Columns["MappedAttributeId"], table2.Columns["MappedValueId"] };
            DataTable table3 = new DataTable("skus");
            DataColumn column27 = new DataColumn("MappedSkuId") {
                Unique = true,
                DataType = Type.GetType("System.String")
            };
            table3.Columns.Add(column27);
            DataColumn column28 = new DataColumn("NewSkuId") {
                DataType = Type.GetType("System.String")
            };
            table3.Columns.Add(column28);
            DataColumn column29 = new DataColumn("ProductId") {
                DataType = Type.GetType("System.Int32")
            };
            table3.Columns.Add(column29);
            DataColumn column30 = new DataColumn("SKU") {
                DataType = Type.GetType("System.String")
            };
            table3.Columns.Add(column30);
            DataColumn column31 = new DataColumn("Weight") {
                DataType = Type.GetType("System.Decimal")
            };
            table3.Columns.Add(column31);
            DataColumn column32 = new DataColumn("Stock") {
                DataType = Type.GetType("System.Int32")
            };
            table3.Columns.Add(column32);
            DataColumn column33 = new DataColumn("AlertStock") {
                DataType = Type.GetType("System.Int32")
            };
            table3.Columns.Add(column33);
            DataColumn column34 = new DataColumn("CostPrice") {
                DataType = Type.GetType("System.Decimal")
            };
            table3.Columns.Add(column34);
            DataColumn column35 = new DataColumn("SalePrice") {
                DataType = Type.GetType("System.Decimal")
            };
            table3.Columns.Add(column35);
            DataColumn column36 = new DataColumn("PurchasePrice") {
                DataType = Type.GetType("System.Decimal")
            };
            table3.Columns.Add(column36);
            table3.PrimaryKey = new DataColumn[] { table3.Columns["MappedSkuId"] };
            DataTable table4 = new DataTable("skuItems");
            DataColumn column37 = new DataColumn("MappedSkuId") {
                DataType = Type.GetType("System.String")
            };
            table4.Columns.Add(column37);
            DataColumn column38 = new DataColumn("NewSkuId") {
                DataType = Type.GetType("System.String")
            };
            table4.Columns.Add(column38);
            DataColumn column39 = new DataColumn("MappedProductId") {
                DataType = Type.GetType("System.Int32")
            };
            table4.Columns.Add(column39);
            DataColumn column40 = new DataColumn("SelectedAttributeId") {
                DataType = Type.GetType("System.Int32")
            };
            table4.Columns.Add(column40);
            DataColumn column41 = new DataColumn("MappedAttributeId") {
                DataType = Type.GetType("System.Int32")
            };
            table4.Columns.Add(column41);
            DataColumn column42 = new DataColumn("SelectedValueId") {
                DataType = Type.GetType("System.Int32")
            };
            table4.Columns.Add(column42);
            DataColumn column43 = new DataColumn("MappedValueId") {
                DataType = Type.GetType("System.Int32")
            };
            table4.Columns.Add(column43);
            table4.PrimaryKey = new DataColumn[] { table4.Columns["MappedSkuId"], table4.Columns["MappedAttributeId"] };
            set.Tables.Add(table);
            set.Tables.Add(table2);
            set.Tables.Add(table3);
            set.Tables.Add(table4);
            return set;
        }

        private void loadProductAttributes(int productId, XmlNodeList attributeNodeList, DataSet productSet, DataSet mappingSet)
        {
            if ((attributeNodeList != null) && (attributeNodeList.Count != 0))
            {
                foreach (XmlNode node in attributeNodeList)
                {
                    int num = int.Parse(node.SelectSingleNode("attributeId").InnerText);
                    int num2 = int.Parse(node.SelectSingleNode("valueId").InnerText);
                    DataRow[] rowArray = mappingSet.Tables["attributes"].Select("MappedAttributeId=" + num);
                    DataRow[] rowArray2 = mappingSet.Tables["values"].Select("MappedValueId=" + num2);
                    if (((rowArray != null) && (rowArray.Length > 0)) && ((rowArray2 != null) && (rowArray2.Length > 0)))
                    {
                        int num3 = (int) rowArray[0]["SelectedAttributeId"];
                        int num4 = (int) rowArray2[0]["SelectedValueId"];
                        DataRow row = productSet.Tables["attributes"].NewRow();
                        row["ProductId"] = productId;
                        row["SelectedAttributeId"] = num3;
                        row["MappedAttributeId"] = num;
                        row["SelectedValueId"] = num4;
                        row["MappedValueId"] = num2;
                        productSet.Tables["attributes"].Rows.Add(row);
                    }
                }
            }
        }

        private void loadProductSkus(int productId, bool hasSku, XmlNodeList valueNodeList, DataSet productSet, DataSet mappingSet, bool includeCostPrice, bool includeStock)
        {
            if ((valueNodeList != null) && (valueNodeList.Count != 0))
            {
                foreach (XmlNode node in valueNodeList)
                {
                    DataRow row = productSet.Tables["skus"].NewRow();
                    string innerText = node.SelectSingleNode("skuId").InnerText;
                    row["MappedSkuId"] = innerText;
                    row["ProductId"] = productId;
                    row["SKU"] = node.SelectSingleNode("sKU").InnerText;
                    if (node.SelectSingleNode("weight").InnerText.Length > 0)
                    {
                        row["Weight"] = decimal.Parse(node.SelectSingleNode("weight").InnerText);
                    }
                    if (includeStock)
                    {
                        row["Stock"] = int.Parse(node.SelectSingleNode("stock").InnerText);
                    }
                    row["AlertStock"] = node.SelectSingleNode("alertStock").InnerText;
                    if (includeCostPrice && (node.SelectSingleNode("costPrice").InnerText.Length > 0))
                    {
                        row["CostPrice"] = node.SelectSingleNode("costPrice").InnerText;
                    }
                    row["SalePrice"] = node.SelectSingleNode("salePrice").InnerText;
                    row["PurchasePrice"] = node.SelectSingleNode("purchasePrice").InnerText;
                    XmlNodeList itemNodeList = node.SelectNodes("skuItems/skuItem");
                    string str2 = this.loadSkuItems(innerText, productId, itemNodeList, productSet, mappingSet);
                    row["NewSkuId"] = hasSku ? str2 : "0";
                    productSet.Tables["skus"].Rows.Add(row);
                }
            }
        }

        private string loadSkuItems(string mappedSkuId, int mappedProductId, XmlNodeList itemNodeList, DataSet productSet, DataSet mappingSet)
        {
            if ((itemNodeList == null) || (itemNodeList.Count == 0))
            {
                return "0";
            }
            string str = "";
            foreach (XmlNode node in itemNodeList)
            {
                str = str + mappingSet.Tables["values"].Select("MappedValueId=" + node.SelectSingleNode("valueId").InnerText)[0]["SelectedValueId"].ToString() + "_";
            }
            str = str.Substring(0, str.Length - 1);
            foreach (XmlNode node2 in itemNodeList)
            {
                int num = int.Parse(node2.SelectSingleNode("attributeId").InnerText);
                int num2 = int.Parse(node2.SelectSingleNode("valueId").InnerText);
                DataRow[] rowArray = mappingSet.Tables["attributes"].Select("MappedAttributeId=" + num);
                DataRow[] rowArray2 = mappingSet.Tables["values"].Select("MappedValueId=" + num2);
                if (((rowArray != null) && (rowArray.Length > 0)) && ((rowArray2 != null) && (rowArray2.Length > 0)))
                {
                    int num3 = (int) rowArray[0]["SelectedAttributeId"];
                    int num4 = (int) rowArray2[0]["SelectedValueId"];
                    DataRow row = productSet.Tables["skuItems"].NewRow();
                    row["MappedProductId"] = mappedProductId;
                    row["NewSkuId"] = str;
                    row["MappedSkuId"] = mappedSkuId;
                    row["SelectedAttributeId"] = num3;
                    row["MappedAttributeId"] = num;
                    row["SelectedValueId"] = num4;
                    row["MappedValueId"] = num2;
                    productSet.Tables["skuItems"].Rows.Add(row);
                }
            }
            return str;
        }

        private void MappingAttributes(int mappedTypeId, DataSet mappingSet, XmlNodeList attributeNodeList, XmlDocument indexesDoc, string workDir)
        {
            if ((attributeNodeList != null) && (attributeNodeList.Count != 0))
            {
                foreach (XmlNode node in attributeNodeList)
                {
                    DataRow row = mappingSet.Tables["attributes"].NewRow();
                    int mappedAttributeId = int.Parse(node.Attributes["mappedAttributeId"].Value);
                    int selectedAttributeId = int.Parse(node.Attributes["selectedAttributeId"].Value);
                    row["MappedAttributeId"] = mappedAttributeId;
                    row["SelectedAttributeId"] = selectedAttributeId;
                    row["MappedTypeId"] = mappedTypeId;
                    if (selectedAttributeId == 0)
                    {
                        XmlNode node2 = indexesDoc.SelectSingleNode("//attribute[attributeId[text()='" + mappedAttributeId + "']]");
                        row["AttributeName"] = node2.SelectSingleNode("attributeName").InnerText;
                        row["DisplaySequence"] = node2.SelectSingleNode("displaySequence").InnerText;
                        row["UsageMode"] = node2.SelectSingleNode("usageMode").InnerText;
                        row["UseAttributeImage"] = node2.SelectSingleNode("useAttributeImage").InnerText;
                    }
                    mappingSet.Tables["attributes"].Rows.Add(row);
                    XmlNodeList valueNodeList = node.SelectNodes("values/value");
                    this.MappingValues(mappedAttributeId, selectedAttributeId, mappingSet, valueNodeList, indexesDoc, workDir);
                }
            }
        }

        private void MappingValues(int mappedAttributeId, int selectedAttributeId, DataSet mappingSet, XmlNodeList valueNodeList, XmlDocument indexesDoc, string workDir)
        {
            if ((valueNodeList != null) && (valueNodeList.Count != 0))
            {
                foreach (XmlNode node in valueNodeList)
                {
                    DataRow row = mappingSet.Tables["values"].NewRow();
                    int num = int.Parse(node.Attributes["mappedValueId"].Value);
                    int num2 = int.Parse(node.Attributes["selectedValueId"].Value);
                    row["MappedValueId"] = num;
                    row["SelectedValueId"] = num2;
                    row["MappedAttributeId"] = mappedAttributeId;
                    row["SelectedAttributeId"] = selectedAttributeId;
                    if (num2 == 0)
                    {
                        XmlNode node2 = indexesDoc.SelectSingleNode("//value[valueId[text()='" + num + "']]");
                        row["DisplaySequence"] = node2.SelectSingleNode("displaySequence").InnerText;
                        row["ValueStr"] = node2.SelectSingleNode("valueStr").InnerText;
                        string innerText = node2.SelectSingleNode("image").InnerText;
                        if ((innerText.Length > 0) && File.Exists(Path.Combine(workDir + @"\images1", innerText)))
                        {
                            File.Copy(Path.Combine(workDir + @"\images1", innerText), HttpContext.Current.Request.MapPath("~/Storage/master/sku/" + innerText), true);
                            row["ImageUrl"] = "/Storage/master/sku/" + innerText;
                        }
                    }
                    mappingSet.Tables["values"].Rows.Add(row);
                }
            }
        }

        public override object[] ParseIndexes(params object[] importParams)
        {
            string path = (string) importParams[0];
            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException("directory:" + path + " does not found");
            }
            XmlDocument document = new XmlDocument();
            document.Load(Path.Combine(path, "indexes.xml"));
            XmlNode node = document.DocumentElement.SelectSingleNode("/indexes");
            string str2 = node.Attributes["version"].Value;
            int num = int.Parse(node.Attributes["QTY"].Value);
            bool flag = bool.Parse(node.Attributes["includeCostPrice"].Value);
            bool flag2 = bool.Parse(node.Attributes["includeStock"].Value);
            bool flag3 = bool.Parse(node.Attributes["includeImages"].Value);
            string str3 = "<xml>" + node.OuterXml + "</xml>";
            return new object[] { str2, num, flag, flag2, flag3, str3 };
        }

        public override object[] ParseProductData(params object[] importParams)
        {
            DataSet mappingSet = (DataSet) importParams[0];
            string str = (string) importParams[1];
            bool includeCostPrice = (bool) importParams[2];
            bool includeStock = (bool) importParams[3];
            bool flag3 = (bool) importParams[4];
            HttpContext current = HttpContext.Current;
            DataSet productSet = this.GetProductSet();
            XmlDocument document = new XmlDocument();
            document.Load(Path.Combine(str, "products.xml"));
            foreach (XmlNode node in document.DocumentElement.SelectNodes("//product"))
            {
                DataRow row = productSet.Tables["products"].NewRow();
                int productId = int.Parse(node.SelectSingleNode("productId").InnerText);
                int num2 = 0;
                int num3 = 0;
                if (node.SelectSingleNode("typeId").InnerText.Length > 0)
                {
                    num2 = int.Parse(node.SelectSingleNode("typeId").InnerText);
                    if (num2 != 0)
                    {
                        num3 = (int) mappingSet.Tables["types"].Select("MappedTypeId=" + num2.ToString(CultureInfo.InvariantCulture))[0]["SelectedTypeId"];
                    }
                }
                bool hasSku = bool.Parse(node.SelectSingleNode("hasSKU").InnerText);
                row["ProductId"] = productId;
                row["SelectedTypeId"] = num3;
                row["MappedTypeId"] = num2;
                row["ProductName"] = node.SelectSingleNode("productName").InnerText;
                row["ProductCode"] = node.SelectSingleNode("productCode").InnerText;
                row["ShortDescription"] = node.SelectSingleNode("shortDescription").InnerText;
                row["Unit"] = node.SelectSingleNode("unit").InnerText;
                row["Description"] = node.SelectSingleNode("description").InnerText;
                row["Title"] = node.SelectSingleNode("title").InnerText;
                row["Meta_Description"] = node.SelectSingleNode("meta_Description").InnerText;
                row["Meta_Keywords"] = node.SelectSingleNode("meta_Keywords").InnerText;
                row["SaleStatus"] = int.Parse(node.SelectSingleNode("saleStatus").InnerText);
                string innerText = string.Empty;
                string str3 = string.Empty;
                string str4 = string.Empty;
                string str5 = string.Empty;
                string str6 = string.Empty;
                if (node.SelectSingleNode("image1") != null)
                {
                    innerText = node.SelectSingleNode("image1").InnerText;
                }
                if (node.SelectSingleNode("image2") != null)
                {
                    str3 = node.SelectSingleNode("image2").InnerText;
                }
                if (node.SelectSingleNode("image3") != null)
                {
                    str4 = node.SelectSingleNode("image3").InnerText;
                }
                if (node.SelectSingleNode("image4") != null)
                {
                    str5 = node.SelectSingleNode("image4").InnerText;
                }
                if (node.SelectSingleNode("image5") != null)
                {
                    str6 = node.SelectSingleNode("image5").InnerText;
                }
                row["ImageUrl1"] = innerText;
                row["ImageUrl2"] = str3;
                row["ImageUrl3"] = str4;
                row["ImageUrl4"] = str5;
                row["ImageUrl5"] = str6;
                if (flag3)
                {
                    if ((innerText.Length > 0) && File.Exists(Path.Combine(str + @"\images2", innerText)))
                    {
                        File.Copy(Path.Combine(str + @"\images2", innerText), current.Request.MapPath("~/Storage/master/product/images/" + innerText), true);
                        row["ImageUrl1"] = "/Storage/master/product/images/" + innerText;
                    }
                    if ((str3.Length > 0) && File.Exists(Path.Combine(str + @"\images2", str3)))
                    {
                        File.Copy(Path.Combine(str + @"\images2", str3), current.Request.MapPath("~/Storage/master/product/images/" + str3), true);
                        row["ImageUrl2"] = "/Storage/master/product/images/" + str3;
                    }
                    if ((str4.Length > 0) && File.Exists(Path.Combine(str + @"\images2", str4)))
                    {
                        File.Copy(Path.Combine(str + @"\images2", str4), current.Request.MapPath("~/Storage/master/product/images/" + str4), true);
                        row["ImageUrl3"] = "/Storage/master/product/images/" + str4;
                    }
                    if ((str5.Length > 0) && File.Exists(Path.Combine(str + @"\images2", str5)))
                    {
                        File.Copy(Path.Combine(str + @"\images2", str5), current.Request.MapPath("~/Storage/master/product/images/" + str5), true);
                        row["ImageUrl4"] = "/Storage/master/product/images/" + str5;
                    }
                    if ((str6.Length > 0) && File.Exists(Path.Combine(str + @"\images2", str6)))
                    {
                        File.Copy(Path.Combine(str + @"\images2", str6), current.Request.MapPath("~/Storage/master/product/images/" + str6), true);
                        row["ImageUrl5"] = "/Storage/master/product/images/" + str6;
                    }
                }
                if (node.SelectSingleNode("marketPrice").InnerText.Length > 0)
                {
                    row["MarketPrice"] = decimal.Parse(node.SelectSingleNode("marketPrice").InnerText);
                }
                row["LowestSalePrice"] = decimal.Parse(node.SelectSingleNode("lowestSalePrice").InnerText);
                row["PenetrationStatus"] = int.Parse(node.SelectSingleNode("penetrationStatus").InnerText);
                row["HasSKU"] = hasSku;
                productSet.Tables["products"].Rows.Add(row);
                XmlNodeList attributeNodeList = node.SelectNodes("attributes/attribute");
                this.loadProductAttributes(productId, attributeNodeList, productSet, mappingSet);
                XmlNodeList valueNodeList = node.SelectNodes("skus/sku");
                this.loadProductSkus(productId, hasSku, valueNodeList, productSet, mappingSet, includeCostPrice, includeStock);
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

