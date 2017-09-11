namespace Hidistro.UI.Web.Admin.Goods
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.TransferManager;
    using Ionic.Zip;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.ProductBatchUpload)]
    public class ImportFromPP : AdminPage
    {
        private string _dataPath;
        protected Button btnImport;
        protected Button btnUpload;
        protected BrandCategoriesDropDownList dropBrandList;
        protected ProductCategoriesDropDownList dropCategories;
        protected DropDownList dropFiles;
        protected DropDownList dropImportVersions;
        protected FileUpload fileUploader;
        protected RadioButton radInStock;
        protected RadioButton radOnSales;
        protected RadioButton radUnSales;

        protected ImportFromPP() : base("m02", "spp04")
        {
        }

        private void BindFiles()
        {
            this.dropFiles.Items.Clear();
            this.dropFiles.Items.Add(new ListItem("-请选择-", ""));
            DirectoryInfo info = new DirectoryInfo(this._dataPath);
            foreach (FileInfo info2 in info.GetFiles("*.zip", SearchOption.TopDirectoryOnly))
            {
                string name = info2.Name;
                this.dropFiles.Items.Add(new ListItem(name, name));
            }
        }

        private void BindImporters()
        {
            this.dropImportVersions.Items.Clear();
            this.dropImportVersions.Items.Add(new ListItem("-请选择-", ""));
            Dictionary<string, string> importAdapters = TransferHelper.GetImportAdapters(new YfxTarget("1.2"), "拍拍助理");
            foreach (string str in importAdapters.Keys)
            {
                this.dropImportVersions.Items.Add(new ListItem(importAdapters[str].Replace("4.0", "2013"), str));
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (this.CheckItems())
            {
                string selectedValue = this.dropFiles.SelectedValue;
                string dir = Path.Combine(this._dataPath, Path.GetFileNameWithoutExtension(selectedValue));
                ImportAdapter importer = TransferHelper.GetImporter(this.dropImportVersions.SelectedValue, new object[0]);
                int categoryId = this.dropCategories.SelectedValue.Value;
                int? brandId = this.dropBrandList.SelectedValue;
                ProductSaleStatus delete = ProductSaleStatus.Delete;
                if (this.radInStock.Checked)
                {
                    delete = ProductSaleStatus.OnStock;
                }
                if (this.radUnSales.Checked)
                {
                    delete = ProductSaleStatus.UnSale;
                }
                if (this.radOnSales.Checked)
                {
                    delete = ProductSaleStatus.OnSale;
                }
                selectedValue = Path.Combine(this._dataPath, selectedValue);
                if (!File.Exists(selectedValue))
                {
                    this.ShowMsg("选择的数据包文件有问题！", false);
                }
                else
                {
                    this.PrepareDataFiles(dir, selectedValue);
                    try
                    {
                        ProductHelper.ImportProducts((DataTable) importer.ParseProductData(new object[] { dir })[0], categoryId, 0, brandId, delete, false);
                        File.Delete(selectedValue);
                        Directory.Delete(dir, true);
                        this.BindFiles();
                        this.ShowMsg("此次商品批量导入操作已成功！", true);
                    }
                    catch (Exception)
                    {
                        this.ShowMsg("选择的数据包文件有问题！", false);
                    }
                }
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (this.dropImportVersions.SelectedValue.Length == 0)
            {
                this.ShowMsg("请先选择一个导入插件", false);
            }
            else if (!this.fileUploader.HasFile)
            {
                this.ShowMsg("请先选择一个数据包文件", false);
            }
            else if ((this.fileUploader.PostedFile.ContentLength == 0) || (((this.fileUploader.PostedFile.ContentType != "application/x-zip-compressed") && (this.fileUploader.PostedFile.ContentType != "application/zip")) && (this.fileUploader.PostedFile.ContentType != "application/octet-stream")))
            {
                this.ShowMsg("请上传正确的数据包文件", false);
            }
            else
            {
                string fileName = Path.GetFileName(this.fileUploader.PostedFile.FileName);
                this.fileUploader.PostedFile.SaveAs(Path.Combine(this._dataPath, fileName));
                this.BindFiles();
                this.dropFiles.SelectedValue = fileName;
            }
        }

        private bool CheckItems()
        {
            string str = "";
            if (this.dropImportVersions.SelectedValue.Length == 0)
            {
                str = str + Formatter.FormatErrorMessage("请选择一个导入插件！");
            }
            if (this.dropFiles.SelectedValue.Length == 0)
            {
                str = str + Formatter.FormatErrorMessage("请选择要导入的数据包文件！");
            }
            if (!this.dropCategories.SelectedValue.HasValue)
            {
                str = str + Formatter.FormatErrorMessage("请选择要导入的商品分类！");
            }
            if (string.IsNullOrEmpty(str) && (str.Length <= 0))
            {
                return true;
            }
            this.ShowMsg(str, false);
            return false;
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            this._dataPath = this.Page.Request.MapPath("~/App_Data/data/paipai");
            this.btnImport.Click += new EventHandler(this.btnImport_Click);
            this.btnUpload.Click += new EventHandler(this.btnUpload_Click);
            if (!this.Page.IsPostBack)
            {
                this.dropCategories.DataBind();
                this.dropBrandList.DataBind();
                this.BindImporters();
                this.BindFiles();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private void PrepareDataFiles(string dir, string filename)
        {
            using (ZipFile file = ZipFile.Read(Path.Combine(new string[] { filename })))
            {
                foreach (ZipEntry entry in file)
                {
                    entry.Extract(dir, ExtractExistingFileAction.OverwriteSilently);
                }
            }
        }
    }
}

