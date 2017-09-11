namespace Hidistro.UI.Web.Admin.Goods
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Data;
    using System.Web.UI.WebControls;
    using System.Xml;

    [PrivilegeCheck(Privilege.EditProducts)]
    public class EditMemberPrices : AdminPage
    {
        protected Button btnOperationOK;
        protected Button btnSavePrice;
        protected Button btnTargetOK;
        protected MemberPriceDropDownList ddlMemberPrice;
        protected MemberPriceDropDownList ddlMemberPrice2;
        protected OperationDropDownList ddlOperation;
        protected MemberPriceDropDownList ddlSalePrice;
        private string productIds;
        protected TextBox txtOperationPrice;
        protected TrimTextBox txtPrices;
        protected TextBox txtTargetPrice;

        protected EditMemberPrices() : base("m01", "00000")
        {
            this.productIds = string.Empty;
        }

        private void btnOperationOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.productIds))
            {
                this.ShowMsgToTarget("没有要修改的商品", false, "parent");
            }
            else if (!this.ddlMemberPrice2.SelectedValue.HasValue)
            {
                this.ShowMsgToTarget("请选择要修改的价格", false, "parent");
            }
            else if (((this.ddlMemberPrice2.SelectedValue.Value == -2) || (this.ddlMemberPrice2.SelectedValue.Value == -3)) && ((this.ddlSalePrice.SelectedValue.Value != -2) && (this.ddlSalePrice.SelectedValue.Value != -3)))
            {
                this.ShowMsgToTarget("一口价或成本价不能用会员等级价作为标准来按公式计算", false, "parent");
            }
            else
            {
                decimal result = 0M;
                if (!decimal.TryParse(this.txtOperationPrice.Text.Trim(), out result))
                {
                    this.ShowMsgToTarget("请输入正确的价格", false, "parent");
                }
                else if ((this.ddlOperation.SelectedValue == "*") && (result <= 0M))
                {
                    this.ShowMsgToTarget("必须乘以一个正数", false, "parent");
                }
                else
                {
                    if ((this.ddlOperation.SelectedValue == "+") && (result < 0M))
                    {
                        decimal checkPrice = -result;
                        if (ProductHelper.CheckPrice(this.productIds, this.ddlSalePrice.SelectedValue.Value, checkPrice, true))
                        {
                            this.ShowMsgToTarget("加了一个太小的负数，导致价格中有负数的情况", false, "parent");
                            return;
                        }
                    }
                    if (!this.ddlMemberPrice2.SelectedValue.HasValue || (this.ddlMemberPrice2.SelectedValue.HasValue && (this.ddlMemberPrice2.SelectedValue.Value <= 0)))
                    {
                        this.ShowMsgToTarget("请先选择目标价格！", false, "parent");
                    }
                    else if ((this.ddlSalePrice.SelectedValue.HasValue && (this.ddlSalePrice.SelectedValue.Value > 0)) && !ProductHelper.GetSKUMemberPrice(this.productIds, this.ddlSalePrice.SelectedValue.Value))
                    {
                        this.ShowMsgToTarget("请先设置" + this.ddlSalePrice.SelectedItem.Text, false, "parent");
                    }
                    else if (ProductHelper.UpdateSkuMemberPrices(this.productIds, this.ddlMemberPrice2.SelectedValue.Value, this.ddlSalePrice.SelectedValue.Value, this.ddlOperation.SelectedValue, result))
                    {
                        this.ShowMsgToTarget("修改商品的价格成功", true, "parent");
                    }
                }
            }
        }

        private void btnSavePrice_Click(object sender, EventArgs e)
        {
            DataSet skuPrices = this.GetSkuPrices();
            if (((skuPrices == null) || (skuPrices.Tables["skuPriceTable"] == null)) || (skuPrices.Tables["skuPriceTable"].Rows.Count == 0))
            {
                this.ShowMsgToTarget("没有任何要修改的项", false, "parent");
            }
            else if (ProductHelper.UpdateSkuMemberPrices(skuPrices))
            {
                string str = Globals.RequestQueryStr("reurl");
                if (string.IsNullOrEmpty(str))
                {
                    str = "productonsales.aspx";
                }
                this.ShowMsgAndReUrl("修改成功", true, str, "parent");
            }
        }

        private void btnTargetOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.productIds))
            {
                this.ShowMsgToTarget("没有要修改的商品", false, "parent");
            }
            else if (!this.ddlMemberPrice.SelectedValue.HasValue)
            {
                this.ShowMsgToTarget("请选择要修改的价格", false, "parent");
            }
            else
            {
                decimal result = 0M;
                if (!decimal.TryParse(this.txtTargetPrice.Text.Trim(), out result))
                {
                    this.ShowMsgToTarget("请输入正确的价格", false, "parent");
                }
                else if (result <= 0M)
                {
                    this.ShowMsgToTarget("直接调价必须输入正数", false, "parent");
                }
                else if (result > 10000000M)
                {
                    this.ShowMsgToTarget("直接调价超出了系统表示范围", false, "parent");
                }
                else if (ProductHelper.UpdateSkuMemberPrices(this.productIds, this.ddlMemberPrice.SelectedValue.Value, result))
                {
                    this.ShowMsgToTarget("修改成功", true, "parent");
                }
            }
        }

        private DataSet GetSkuPrices()
        {
            DataSet set = null;
            XmlDocument document = new XmlDocument();
            try
            {
                document.LoadXml(this.txtPrices.Text);
                XmlNodeList list = document.SelectNodes("//item");
                if ((list == null) || (list.Count == 0))
                {
                    return null;
                }
                set = new DataSet();
                DataTable table = new DataTable("skuPriceTable");
                table.Columns.Add("skuId");
                table.Columns.Add("costPrice");
                table.Columns.Add("salePrice");
                DataTable table2 = new DataTable("skuMemberPriceTable");
                table2.Columns.Add("skuId");
                table2.Columns.Add("gradeId");
                table2.Columns.Add("memberPrice");
                foreach (XmlNode node in list)
                {
                    DataRow row = table.NewRow();
                    row["skuId"] = node.Attributes["skuId"].Value;
                    row["costPrice"] = string.IsNullOrEmpty(node.Attributes["costPrice"].Value) ? 0M : decimal.Parse(node.Attributes["costPrice"].Value);
                    row["salePrice"] = decimal.Parse(node.Attributes["salePrice"].Value);
                    table.Rows.Add(row);
                    foreach (XmlNode node2 in node.SelectSingleNode("skuMemberPrices").ChildNodes)
                    {
                        DataRow row2 = table2.NewRow();
                        row2["skuId"] = node.Attributes["skuId"].Value;
                        row2["gradeId"] = int.Parse(node2.Attributes["gradeId"].Value);
                        row2["memberPrice"] = decimal.Parse(node2.Attributes["memberPrice"].Value);
                        table2.Rows.Add(row2);
                    }
                }
                set.Tables.Add(table);
                set.Tables.Add(table2);
            }
            catch
            {
            }
            return set;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.productIds = this.Page.Request.QueryString["productIds"];
            this.btnSavePrice.Click += new EventHandler(this.btnSavePrice_Click);
            this.btnTargetOK.Click += new EventHandler(this.btnTargetOK_Click);
            this.btnOperationOK.Click += new EventHandler(this.btnOperationOK_Click);
            if (!this.Page.IsPostBack)
            {
                this.ddlMemberPrice.DataBind();
                this.ddlMemberPrice.SelectedValue = -3;
                this.ddlMemberPrice2.DataBind();
                this.ddlMemberPrice2.SelectedValue = -3;
                this.ddlSalePrice.DataBind();
                this.ddlSalePrice.SelectedValue = -3;
                this.ddlOperation.DataBind();
                this.ddlOperation.SelectedValue = "+";
                this.ddlMemberPrice2.Items.RemoveAt(0);
            }
        }
    }
}

