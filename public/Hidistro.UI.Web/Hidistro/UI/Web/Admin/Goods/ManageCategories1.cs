namespace Hidistro.UI.Web.Admin.Goods
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.IO;
    using System.Net;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.ProductCategory)]
    public class ManageCategories1 : AdminPage
    {
        protected LinkButton btnOrder;
        protected Button btnSetCommissions;
        protected Repeater rptList;
        protected HtmlInputText txtcategoryId;
        protected HtmlInputText txtfirst;
        protected HtmlInputText txtsecond;
        protected HtmlInputText txtthird;

        protected ManageCategories1() : base("m02", "spp06")
        {
        }

        private void BindData()
        {
            this.rptList.DataSource = CatalogHelper.GetSequenceCategories();
            this.rptList.DataBind();
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.rptList.Items.Count; i++)
            {
                int result = 0;
                TextBox box = (TextBox) this.rptList.Items[i].FindControl("txtSequence");
                if (int.TryParse(box.Text.Trim(), out result))
                {
                    int categoryId = Globals.ToNum(((HiddenField) this.rptList.Items[i].FindControl("hdfCategoryID")).Value);
                    if (CatalogHelper.GetCategory(categoryId).DisplaySequence != result)
                    {
                        CatalogHelper.SwapCategorySequence(categoryId, result);
                    }
                }
            }
            HiCache.Remove("DataCache-Categories");
            HiCache.Remove("DataCache-CategoryList");
            this.BindData();
            this.ShowMsg("排序保存成功", true);
        }

        private void btnSetCommissions_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtcategoryId.Value) || (Convert.ToInt32(this.txtcategoryId.Value) <= 0))
            {
                this.ShowMsg("请选择要编辑的佣金分类", false);
            }
            else
            {
                CategoryInfo categorys = this.GetCategorys();
                if (categorys != null)
                {
                    if (categorys.ParentCategoryId == 0)
                    {
                        if (CatalogHelper.UpdateCategory(categorys) == CategoryActionStatus.Success)
                        {
                            HiCache.Remove("DataCache-Categories");
                            HiCache.Remove("DataCache-CategoryList");
                            this.ShowMsg("保存成功", true);
                            this.BindData();
                        }
                        else
                        {
                            this.ShowMsg("保存失败", false);
                        }
                    }
                    else
                    {
                        this.ShowMsg("只允许修改顶级类目佣金！", false);
                    }
                }
            }
        }

        public string FormatEditeCommission(object ParentCategoryId, object CategoryId, object FirstCommission, object SecondCommission, object ThirdCommission)
        {
            if (ParentCategoryId.ToString() == "0")
            {
                return string.Concat(new object[] { " onclick=\"EditeCommission(", CategoryId, ",'", FirstCommission, "','", SecondCommission, "','", ThirdCommission, "')\"" });
            }
            return " onclick=\"return false\" style=\"display:none\" ";
        }

        private CategoryInfo GetCategorys()
        {
            CategoryInfo category = CatalogHelper.GetCategory(Convert.ToInt32(this.txtcategoryId.Value));
            if (category == null)
            {
                this.ShowMsg("无法获取当前分类", false);
                return null;
            }
            bool flag = false;
            try
            {
                if (this.txtfirst.Value.Trim() == "")
                {
                    category.FirstCommission = "0";
                }
                else if ((Convert.ToDecimal(this.txtfirst.Value.Trim()) < 0M) || (Convert.ToDecimal(this.txtfirst.Value.Trim()) > 100M))
                {
                    this.ShowMsg("输入的佣金格式不正确！", false);
                    flag = true;
                }
                else
                {
                    category.FirstCommission = Convert.ToDecimal(this.txtfirst.Value.Trim()).ToString("F2");
                }
                if (this.txtsecond.Value.Trim() == "")
                {
                    category.SecondCommission = "0";
                }
                else if ((Convert.ToDecimal(this.txtsecond.Value.Trim()) < 0M) || (Convert.ToDecimal(this.txtsecond.Value.Trim()) > 100M))
                {
                    this.ShowMsg("输入的佣金格式不正确！", false);
                    flag = true;
                }
                else
                {
                    category.SecondCommission = Convert.ToDecimal(this.txtsecond.Value.Trim()).ToString("F2");
                }
                if (this.txtthird.Value.Trim() == "")
                {
                    category.ThirdCommission = "0";
                }
                else if ((Convert.ToDecimal(this.txtthird.Value.Trim()) < 0M) || (Convert.ToDecimal(this.txtthird.Value.Trim()) > 100M))
                {
                    this.ShowMsg("输入的佣金格式不正确！", false);
                    flag = true;
                }
                else
                {
                    category.ThirdCommission = Convert.ToDecimal(this.txtthird.Value.Trim()).ToString("F2");
                }
                if (flag)
                {
                    return null;
                }
            }
            catch (Exception)
            {
                this.ShowMsg("输入的佣金格式不正确！", false);
                if (true)
                {
                    return null;
                }
            }
            return category;
        }

        private byte[] GetImageContent(string picurl)
        {
            byte[] buffer2;
            string requestUriString = picurl;
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(requestUriString);
            request.AllowAutoRedirect = true;
            WebProxy proxy = new WebProxy {
                BypassProxyOnLocal = true,
                UseDefaultCredentials = true
            };
            request.Proxy = proxy;
            using (Stream stream = request.GetResponse().GetResponseStream())
            {
                using (MemoryStream stream2 = new MemoryStream())
                {
                    byte[] buffer = new byte[0x400];
                    int count = 0;
                    while ((count = stream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        stream2.Write(buffer, 0, count);
                    }
                    buffer2 = stream2.ToArray();
                }
            }
            return buffer2;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSetCommissions.Click += new EventHandler(this.btnSetCommissions_Click);
            this.btnOrder.Click += new EventHandler(this.btnOrder_Click);
            if (!this.Page.IsPostBack)
            {
                string str = Globals.RequestFormStr("picurl");
                if (!string.IsNullOrEmpty(str))
                {
                    byte[] imageContent = this.GetImageContent(str);
                    this.WriteResponse(imageContent);
                }
                this.BindData();
            }
        }

        protected void rptList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DeleteCagetory")
            {
                int categoryId = Globals.ToNum(e.CommandArgument.ToString());
                if (!CatalogHelper.IsExitProduct(categoryId.ToString()))
                {
                    if (CatalogHelper.DeleteCategory(categoryId))
                    {
                        HiCache.Remove("DataCache-Categories");
                        HiCache.Remove("DataCache-CategoryList");
                        this.ShowMsg("成功删除了指定的分类", true);
                    }
                    else
                    {
                        this.ShowMsg("分类删除失败，未知错误", false);
                    }
                }
                else
                {
                    this.ShowMsg("分类下有商品，请先删除商品再到商品回收站彻底删除。", false);
                }
            }
            this.BindData();
        }

        protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                int num = (int) DataBinder.Eval(e.Item.DataItem, "Depth");
                string str = DataBinder.Eval(e.Item.DataItem, "Name").ToString();
                if (num == 1)
                {
                    str = "<b>" + str + "</b>";
                }
                else
                {
                    HtmlGenericControl control = e.Item.FindControl("spShowImage") as HtmlGenericControl;
                    control.Visible = false;
                }
                for (int i = 1; i < num; i++)
                {
                    str = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + str;
                }
                Literal literal = e.Item.FindControl("lblCategoryName") as Literal;
                literal.Text = str;
                HiddenField field = e.Item.FindControl("hdfCategoryID") as HiddenField;
                field.Value = DataBinder.Eval(e.Item.DataItem, "CategoryID").ToString();
            }
        }

        private void WriteResponse(byte[] content)
        {
            string s = DateTime.Now.ToString("HHmmss") + ".png";
            base.Response.Clear();
            base.Response.ClearHeaders();
            base.Response.Buffer = false;
            base.Response.ContentType = "application/octet-stream";
            base.Response.AppendHeader("Content-Disposition", "attachment;filename=" + base.Server.UrlEncode(s));
            int length = content.Length;
            base.Response.AppendHeader("Content-Length", length.ToString());
            base.Response.BinaryWrite(content);
            base.Response.Flush();
            base.Response.End();
        }
    }
}

