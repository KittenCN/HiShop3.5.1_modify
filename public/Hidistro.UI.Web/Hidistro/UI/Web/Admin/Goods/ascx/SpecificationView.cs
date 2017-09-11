namespace Hidistro.UI.Web.Admin.Goods.ascx
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class SpecificationView : UserControl
    {
        protected Button btnCreate;
        protected Grid grdSKU;
        protected UseAttributeImageRadioButtonList radIsImage;
        protected TextBox txtName;
        protected int typeId;

        private void BindAttribute()
        {
            this.grdSKU.DataSource = ProductTypeHelper.GetAttributes(this.typeId, AttributeUseageMode.Choose);
            this.grdSKU.DataBind();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            AttributeInfo target = new AttributeInfo {
                TypeId = this.typeId,
                AttributeName = Globals.HtmlEncode(this.txtName.Text).Replace("，", ","),
                UsageMode = AttributeUseageMode.Choose,
                UseAttributeImage = this.radIsImage.SelectedValue
            };
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<AttributeInfo>(target, new string[] { "ValAttribute" });
            string str = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                {
                    str = str + Formatter.FormatErrorMessage(result.Message);
                }
            }
            else
            {
                ProductTypeHelper.GetAttributes(this.typeId, AttributeUseageMode.Choose);
                if (ProductTypeHelper.AddAttributeName(target))
                {
                    base.Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
                }
            }
        }

        private void grdSKU_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = ((GridViewRow) ((Control) e.CommandSource).NamingContainer).RowIndex;
            int attributeId = Convert.ToInt32(this.grdSKU.DataKeys[rowIndex].Value);
            if (e.CommandName == "saveSKUName")
            {
                TextBox box = this.grdSKU.Rows[rowIndex].Cells[2].FindControl("txtSKUName") as TextBox;
                AttributeInfo attribute = new AttributeInfo {
                    AttributeId = attributeId
                };
                if (string.IsNullOrEmpty(box.Text.Trim()) || (box.Text.Trim().Length > 50))
                {
                    string str = string.Format("ShowMsg(\"{0}\", {1});", "规格名称限制在1-50个字符以内", "false");
                    this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript2", "<script language='JavaScript' defer='defer'>setTimeout(function(){" + str + "},300);</script>");
                    return;
                }
                attribute.AttributeName = Globals.HtmlEncode(box.Text);
                attribute.UsageMode = AttributeUseageMode.Choose;
                ProductTypeHelper.UpdateAttributeName(attribute);
                base.Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
            }
            int displaySequence = int.Parse((this.grdSKU.Rows[rowIndex].FindControl("lblDisplaySequence") as Literal).Text, NumberStyles.None);
            int replaceAttributeId = 0;
            int replaceDisplaySequence = 0;
            if (e.CommandName == "Fall")
            {
                if (rowIndex < (this.grdSKU.Rows.Count - 1))
                {
                    replaceAttributeId = (int) this.grdSKU.DataKeys[rowIndex + 1].Value;
                    replaceDisplaySequence = int.Parse((this.grdSKU.Rows[rowIndex + 1].FindControl("lblDisplaySequence") as Literal).Text, NumberStyles.None);
                }
            }
            else if ((e.CommandName == "Rise") && (rowIndex > 0))
            {
                replaceAttributeId = (int) this.grdSKU.DataKeys[rowIndex - 1].Value;
                replaceDisplaySequence = int.Parse((this.grdSKU.Rows[rowIndex - 1].FindControl("lblDisplaySequence") as Literal).Text, NumberStyles.None);
            }
            if (replaceAttributeId > 0)
            {
                ProductTypeHelper.SwapAttributeSequence(attributeId, replaceAttributeId, displaySequence, replaceDisplaySequence);
            }
            this.BindAttribute();
        }

        private void grdSKU_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal literal = e.Row.FindControl("litUseAttributeImage") as Literal;
                if (literal.Text == "True")
                {
                    literal.Text = "图";
                }
                else
                {
                    literal.Text = "文";
                }
            }
        }

        private void grdSKU_RowDeleting(object source, GridViewDeleteEventArgs e)
        {
            int attributeId = (int) this.grdSKU.DataKeys[e.RowIndex].Value;
            AttributeInfo attribute = ProductTypeHelper.GetAttribute(attributeId);
            if (ProductTypeHelper.DeleteAttribute(attributeId))
            {
                foreach (AttributeValueInfo info2 in attribute.AttributeValues)
                {
                    StoreHelper.DeleteImage(info2.ImageUrl);
                }
                base.Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
            }
            else
            {
                this.BindAttribute();
                string str = string.Format("ShowMsg(\"{0}\", {1});", "有商品在使用此规格，无法删除", "false");
                this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript2", "<script language='JavaScript' defer='defer'>setTimeout(function(){" + str + "},300);</script>");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["typeId"]))
            {
                int.TryParse(this.Page.Request.QueryString["typeId"], out this.typeId);
            }
            this.grdSKU.RowCommand += new GridViewCommandEventHandler(this.grdSKU_RowCommand);
            this.btnCreate.Click += new EventHandler(this.btnCreate_Click);
            this.grdSKU.RowDataBound += new GridViewRowEventHandler(this.grdSKU_RowDataBound);
            this.grdSKU.RowDeleting += new GridViewDeleteEventHandler(this.grdSKU_RowDeleting);
            if (!this.Page.IsPostBack)
            {
                this.BindAttribute();
            }
        }
    }
}

