namespace Hidistro.UI.Web.Admin.Goods
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.Entities.Commodities;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Globalization;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class EditAttributeValues : AdminPage
    {
        private int attributeId;
        protected Button btnCreate;
        protected Button btnUpdate;
        protected Grid grdAttributeValues;
        protected HtmlInputHidden hidvalue;
        protected HtmlInputHidden hidvalueId;
        protected TextBox txtOldValue;
        protected TextBox txtValue;
        private int typeId;

        protected EditAttributeValues() : base("m02", "spp07")
        {
        }

        private void BindData()
        {
            AttributeInfo attribute = ProductTypeHelper.GetAttribute(this.attributeId);
            this.grdAttributeValues.DataSource = attribute.AttributeValues;
            this.grdAttributeValues.DataBind();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AttributeValueInfo attributeValue = new AttributeValueInfo();
            if ((this.txtValue.Text.Trim().Length > 15) || (this.txtValue.Text.Trim().Length == 0))
            {
                this.ShowMsg("属性值必须小于15个字符，不能为空", false);
            }
            else
            {
                attributeValue.ValueStr = this.txtValue.Text.Trim().Replace("+", "");
                attributeValue.AttributeId = this.attributeId;
                if (ProductTypeHelper.AddAttributeValue(attributeValue) > 0)
                {
                    this.BindData();
                    this.ShowMsg("添加成功", true);
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if ((this.txtOldValue.Text.Trim().Length > 15) || (this.txtOldValue.Text.Trim().Length == 0))
            {
                this.ShowMsg("属性值必须小于15个字符，不能为空", false);
            }
            else
            {
                AttributeValueInfo attributeValueInfo = ProductTypeHelper.GetAttributeValueInfo(Convert.ToInt32(this.hidvalueId.Value));
                attributeValueInfo.ValueStr = this.txtOldValue.Text.Trim().Replace("+", "");
                if (ProductTypeHelper.UpdateAttributeValue(attributeValueInfo))
                {
                    this.BindData();
                    this.ShowMsg("修改成功", true);
                }
            }
        }

        private void grdAttributeValues_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = ((GridViewRow) ((Control) e.CommandSource).NamingContainer).RowIndex;
            int attributeValueId = (int) this.grdAttributeValues.DataKeys[rowIndex].Value;
            int displaySequence = int.Parse((this.grdAttributeValues.Rows[rowIndex].FindControl("lblDisplaySequence") as Literal).Text, NumberStyles.None);
            int replaceAttributeValueId = 0;
            int replaceDisplaySequence = 0;
            if (e.CommandName == "Fall")
            {
                if (rowIndex < (this.grdAttributeValues.Rows.Count - 1))
                {
                    replaceAttributeValueId = (int) this.grdAttributeValues.DataKeys[rowIndex + 1].Value;
                    replaceDisplaySequence = int.Parse((this.grdAttributeValues.Rows[rowIndex + 1].FindControl("lblDisplaySequence") as Literal).Text, NumberStyles.None);
                }
            }
            else if ((e.CommandName == "Rise") && (rowIndex > 0))
            {
                replaceAttributeValueId = (int) this.grdAttributeValues.DataKeys[rowIndex - 1].Value;
                replaceDisplaySequence = int.Parse((this.grdAttributeValues.Rows[rowIndex - 1].FindControl("lblDisplaySequence") as Literal).Text, NumberStyles.None);
            }
            if (replaceAttributeValueId > 0)
            {
                ProductTypeHelper.SwapAttributeValueSequence(attributeValueId, replaceAttributeValueId, displaySequence, replaceDisplaySequence);
                this.BindData();
            }
        }

        private void grdAttributeValues_RowDeleting(object source, GridViewDeleteEventArgs e)
        {
            int attributeValueId = (int) this.grdAttributeValues.DataKeys[e.RowIndex].Value;
            if (ProductTypeHelper.DeleteAttributeValue(attributeValueId))
            {
                this.BindData();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnUpdate.Click += new EventHandler(this.btnUpdate_Click);
            this.btnCreate.Click += new EventHandler(this.btnAdd_Click);
            this.grdAttributeValues.RowDeleting += new GridViewDeleteEventHandler(this.grdAttributeValues_RowDeleting);
            this.grdAttributeValues.RowCommand += new GridViewCommandEventHandler(this.grdAttributeValues_RowCommand);
            if (!int.TryParse(this.Page.Request.QueryString["AttributeId"], out this.attributeId))
            {
                base.GotoResourceNotFound();
            }
            else if (!int.TryParse(this.Page.Request.QueryString["TypeId"], out this.typeId))
            {
                base.GotoResourceNotFound();
            }
            else if (!base.IsPostBack)
            {
                this.BindData();
            }
        }
    }
}

