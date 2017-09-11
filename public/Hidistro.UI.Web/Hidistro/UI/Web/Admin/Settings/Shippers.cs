namespace Hidistro.UI.Web.Admin.Settings
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Core;
    using Hidistro.Entities.Sales;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class Shippers : AdminPage
    {
        protected Button btnSave;
        protected RegionSelector ddlReggion;
        protected Literal editType;
        protected Pager pager;
        protected Script Script4;
        protected HiddenField ShipperId;
        protected Repeater ShipperList;
        protected HiddenField Task;
        protected HtmlForm thisForm;
        protected TextBox txtAddress;
        protected TextBox txtShipperName;
        protected CheckBoxList txtShipperType;
        protected TextBox txtTelPhone;

        protected Shippers() : base("m09", "szp09")
        {
        }

        private void BindShippers()
        {
            this.ShipperList.DataSource = SalesHelper.GetShippers(false);
            this.ShipperList.DataBind();
        }

        private void btnAddShipper_Click(object sender, EventArgs e)
        {
            ShippersInfo shipper = new ShippersInfo();
            string str = "0";
            if (this.txtShipperType.Items[0].Selected && !this.txtShipperType.Items[1].Selected)
            {
                str = "1";
            }
            else if (!this.txtShipperType.Items[0].Selected && this.txtShipperType.Items[1].Selected)
            {
                str = "2";
            }
            else if (this.txtShipperType.Items[0].Selected && this.txtShipperType.Items[1].Selected)
            {
                str = "3";
            }
            shipper.ShipperTag = str;
            shipper.ShipperName = this.txtShipperName.Text.Trim();
            if (!this.ddlReggion.GetSelectedRegionId().HasValue)
            {
                this.ShowMsg("请选择地区", false);
            }
            else
            {
                shipper.RegionId = this.ddlReggion.GetSelectedRegionId().Value;
                shipper.Address = this.txtAddress.Text.Trim();
                shipper.CellPhone = this.txtTelPhone.Text.Trim();
                shipper.TelPhone = this.txtTelPhone.Text.Trim();
                shipper.Zipcode = "";
                shipper.IsDefault = true;
                shipper.Remark = "";
                int result = 0;
                int.TryParse(this.ShipperId.Value, out result);
                shipper.ShipperId = result;
                if (this.ValidationShipper(shipper))
                {
                    if (string.IsNullOrEmpty(shipper.CellPhone) && string.IsNullOrEmpty(shipper.TelPhone))
                    {
                        this.ShowMsg("手机号码和电话号码必填其一", false);
                    }
                    else if (SalesHelper.AddShipper(shipper))
                    {
                        if (this.Task.Value == "EDIT")
                        {
                            this.Task.Value = "ADD";
                            this.txtShipperType.SelectedValue = "";
                            this.txtShipperType.Enabled = true;
                            this.txtShipperName.Text = "";
                            this.ddlReggion.SetSelectedRegionId(0);
                            this.txtAddress.Text = "";
                            this.ShipperId.Value = "";
                            this.txtTelPhone.Text = "";
                            this.txtShipperType.ClearSelection();
                            this.editType.Text = "新增发货地址信息";
                            this.ShowMsg("成功修改了一个发货信息", true);
                        }
                        else
                        {
                            this.ShowMsg("成功添加了一个发货信息", true);
                        }
                        this.BindShippers();
                    }
                    else
                    {
                        this.ShowMsg("添加发货信息失败", false);
                    }
                }
            }
        }

        protected void DeleteShiper_Click(object sender, EventArgs e)
        {
            int result = 0;
            if (int.TryParse(((Button) sender).CommandArgument, out result))
            {
                if (SalesHelper.DeleteShipper(result))
                {
                    this.BindShippers();
                    this.ShowMsg("已经成功删除选择的发货信息", true);
                }
                else
                {
                    this.ShowMsg("非正常删除！", true);
                }
            }
            else
            {
                this.ShowMsg("非正常删除！", true);
            }
        }

        protected void EditShiper(object sender, EventArgs e)
        {
            int result = 0;
            if (int.TryParse(((LinkButton) sender).CommandArgument, out result))
            {
                ShippersInfo shipper = SalesHelper.GetShipper(result);
                if (shipper == null)
                {
                    base.GotoResourceNotFound();
                }
                else
                {
                    Globals.EntityCoding(shipper, false);
                    string shipperTag = shipper.ShipperTag;
                    this.txtShipperType.ClearSelection();
                    switch (shipperTag)
                    {
                        case "1":
                            this.txtShipperType.Items[0].Selected = true;
                            break;

                        case "2":
                            this.txtShipperType.Items[1].Selected = true;
                            break;

                        case "3":
                            this.txtShipperType.Items[1].Selected = true;
                            this.txtShipperType.Items[0].Selected = true;
                            break;
                    }
                    this.txtShipperName.Text = shipper.ShipperName;
                    this.ddlReggion.SetSelectedRegionId(new int?(shipper.RegionId));
                    this.txtAddress.Text = shipper.Address;
                    this.txtTelPhone.Text = shipper.TelPhone;
                    this.ShipperId.Value = result.ToString();
                    this.Task.Value = "EDIT";
                    this.editType.Text = "修改发货地址信息";
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSave.Click += new EventHandler(this.btnAddShipper_Click);
            if (!this.Page.IsPostBack)
            {
                this.BindShippers();
            }
        }

        private void ShipperList_RowDeleting(object sender, RepeaterItemEventArgs e)
        {
        }

        protected void SwapShiper(object sender, EventArgs e)
        {
            int result = 0;
            if (int.TryParse(((LinkButton) sender).CommandArgument, out result))
            {
                ShippersInfo shipper = SalesHelper.GetShipper(result);
                if (shipper == null)
                {
                    base.GotoResourceNotFound();
                }
                else if (SalesHelper.SwapShipper(result, shipper.ShipperTag))
                {
                    this.BindShippers();
                    this.ShowMsg("已成功交换了退发货地址信息！", true);
                }
                else
                {
                    this.ShowMsg("交换地址信息异常！", true);
                }
            }
        }

        private bool ValidationShipper(ShippersInfo shipper)
        {
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<ShippersInfo>(shipper, new string[] { "Valshipper" });
            string msg = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                this.ShowMsg(msg, false);
            }
            return results.IsValid;
        }
    }
}

