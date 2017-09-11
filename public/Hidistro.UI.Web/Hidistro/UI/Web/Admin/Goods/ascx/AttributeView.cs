namespace Hidistro.UI.Web.Admin.Goods.ascx
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.Core;
    using Hidistro.Entities.Commodities;
    using Hidistro.UI.Common.Controls;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class AttributeView : UserControl
    {
        protected Button btnCreate;
        protected Button btnCreateValue;
        protected CheckBox chkMulti;
        protected CheckBox chkMulti_copy;
        protected HtmlInputHidden currentAttributeId;
        protected Grid grdAttribute;
        protected TextBox txtName;
        protected TextBox txtValues;
        protected TextBox txtValueStr;
        private int typeId;

        private void BindAttribute()
        {
            this.grdAttribute.DataSource = ProductTypeHelper.GetAttributes(this.typeId, AttributeUseageMode.View);
            this.grdAttribute.DataBind();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (this.txtName.Text.Trim().Length > 15)
            {
                string str = string.Format("ShowMsg(\"{0}\", {1});", "属性名称限制在15个字符以内", "false");
                this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript2", "<script language='JavaScript' defer='defer'>setTimeout(function(){" + str + "},300);</script>");
            }
            else
            {
                AttributeInfo target = new AttributeInfo {
                    TypeId = this.typeId,
                    AttributeName = Globals.HtmlEncode(this.txtName.Text.Trim())
                };
                if (this.chkMulti.Checked)
                {
                    target.UsageMode = AttributeUseageMode.MultiView;
                }
                else
                {
                    target.UsageMode = AttributeUseageMode.View;
                }
                if (!string.IsNullOrEmpty(this.txtValues.Text.Trim()))
                {
                    string[] strArray = this.txtValues.Text.Trim().Replace("，", ",").Split(new char[] { ',' });
                    for (int i = 0; i < strArray.Length; i++)
                    {
                        if (strArray[i].Length > 100)
                        {
                            break;
                        }
                        AttributeValueInfo item = new AttributeValueInfo();
                        if (strArray[i].Length > 15)
                        {
                            string str3 = string.Format("ShowMsg(\"{0}\", {1});", "扩展属性的值，每个值的字符数最多15个字符", "false");
                            this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript2", "<script language='JavaScript' defer='defer'>setTimeout(function(){" + str3 + "},300);</script>");
                            return;
                        }
                        item.ValueStr = Globals.HtmlEncode(strArray[i]);
                        target.AttributeValues.Add(item);
                    }
                }
                ValidationResults results = Hishop.Components.Validation.Validation.Validate<AttributeInfo>(target, new string[] { "ValAttribute" });
                string str4 = string.Empty;
                if (!results.IsValid)
                {
                    foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                    {
                        str4 = str4 + Formatter.FormatErrorMessage(result.Message);
                    }
                }
                else if (ProductTypeHelper.AddAttribute(target))
                {
                    this.txtName.Text = string.Empty;
                    this.txtValues.Text = string.Empty;
                    base.Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
                }
            }
        }

        private void btnCreateValueAdd_Click(object sender, EventArgs e)
        {
            AttributeValueInfo info = new AttributeValueInfo();
            IList<AttributeValueInfo> list = new List<AttributeValueInfo>();
            int num = int.Parse(this.currentAttributeId.Value);
            info.AttributeId = num;
            if (!string.IsNullOrEmpty(this.txtValueStr.Text.Trim()))
            {
                string[] strArray = this.txtValueStr.Text.Trim().Replace("，", ",").Split(new char[] { ',' });
                for (int i = 0; i < strArray.Length; i++)
                {
                    if (strArray[i].Trim().Length > 100)
                    {
                        break;
                    }
                    AttributeValueInfo item = new AttributeValueInfo();
                    if (strArray[i].Trim().Length > 50)
                    {
                        string str2 = string.Format("ShowMsg(\"{0}\", {1});", "属性值限制在50个字符以内", "false");
                        this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript2", "<script language='JavaScript' defer='defer'>setTimeout(function(){" + str2 + "},300);</script>");
                        return;
                    }
                    item.ValueStr = Globals.HtmlEncode(strArray[i].Trim()).Replace("+", "");
                    item.AttributeId = num;
                    list.Add(item);
                }
                foreach (AttributeValueInfo info3 in list)
                {
                    ProductTypeHelper.AddAttributeValue(info3);
                }
                this.txtValueStr.Text = string.Empty;
                base.Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
            }
        }

        private void grdAttribute_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = ((GridViewRow) ((Control) e.CommandSource).NamingContainer).RowIndex;
            int attributeId = (int) this.grdAttribute.DataKeys[rowIndex].Value;
            int displaySequence = int.Parse((this.grdAttribute.Rows[rowIndex].FindControl("lblDisplaySequence") as Literal).Text, NumberStyles.None);
            int replaceAttributeId = 0;
            int replaceDisplaySequence = 0;
            if (e.CommandName == "saveAttributeName")
            {
                TextBox box = this.grdAttribute.Rows[rowIndex].FindControl("txtAttributeName") as TextBox;
                AttributeInfo attribute = ProductTypeHelper.GetAttribute(attributeId);
                if (string.IsNullOrEmpty(box.Text.Trim()) || (box.Text.Trim().Length > 15))
                {
                    string str = string.Format("ShowMsg(\"{0}\", {1});", "属性名称限制在1-15个字符以内", "false");
                    this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript2", "<script language='JavaScript' defer='defer'>setTimeout(function(){" + str + "},300);</script>");
                    return;
                }
                attribute.AttributeName = Globals.HtmlEncode(box.Text);
                ProductTypeHelper.UpdateAttributeName(attribute);
                base.Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
            }
            if (e.CommandName == "SetYesOrNo")
            {
                AttributeInfo info2 = ProductTypeHelper.GetAttribute(attributeId);
                if (info2.IsMultiView)
                {
                    info2.UsageMode = AttributeUseageMode.View;
                }
                else
                {
                    info2.UsageMode = AttributeUseageMode.MultiView;
                }
                ProductTypeHelper.UpdateAttributeName(info2);
                base.Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
            }
            if (e.CommandName == "Fall")
            {
                if (rowIndex < (this.grdAttribute.Rows.Count - 1))
                {
                    replaceAttributeId = (int) this.grdAttribute.DataKeys[rowIndex + 1].Value;
                    replaceDisplaySequence = int.Parse((this.grdAttribute.Rows[rowIndex + 1].FindControl("lblDisplaySequence") as Literal).Text, NumberStyles.None);
                }
            }
            else if ((e.CommandName == "Rise") && (rowIndex > 0))
            {
                replaceAttributeId = (int) this.grdAttribute.DataKeys[rowIndex - 1].Value;
                replaceDisplaySequence = int.Parse((this.grdAttribute.Rows[rowIndex - 1].FindControl("lblDisplaySequence") as Literal).Text, NumberStyles.None);
            }
            if (replaceAttributeId > 0)
            {
                ProductTypeHelper.SwapAttributeSequence(attributeId, replaceAttributeId, displaySequence, replaceDisplaySequence);
                this.BindAttribute();
            }
        }

        private void grdAttribute_RowDeleting(object source, GridViewDeleteEventArgs e)
        {
            int attriubteId = (int) this.grdAttribute.DataKeys[e.RowIndex].Value;
            if (ProductTypeHelper.DeleteAttribute(attriubteId))
            {
                base.Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
            }
            else
            {
                this.BindAttribute();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["typeId"]))
            {
                int.TryParse(this.Page.Request.QueryString["typeId"], out this.typeId);
            }
            this.grdAttribute.RowDeleting += new GridViewDeleteEventHandler(this.grdAttribute_RowDeleting);
            this.grdAttribute.RowCommand += new GridViewCommandEventHandler(this.grdAttribute_RowCommand);
            this.btnCreate.Click += new EventHandler(this.btnCreate_Click);
            this.btnCreateValue.Click += new EventHandler(this.btnCreateValueAdd_Click);
            if (!this.Page.IsPostBack)
            {
                this.BindAttribute();
            }
        }
    }
}

