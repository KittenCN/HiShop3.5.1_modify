namespace Hidistro.UI.Web.Admin.Goods
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Commodities;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Globalization;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class EditSpecificationValues : AdminPage
    {
        private int attributeId;
        protected HtmlInputHidden currentAttributeId;
        protected Grid grdAttributeValues;

        protected EditSpecificationValues() : base("m02", "spp07")
        {
        }

        private void BindData()
        {
            AttributeInfo attribute = ProductTypeHelper.GetAttribute(this.attributeId);
            this.grdAttributeValues.DataSource = attribute.AttributeValues;
            this.grdAttributeValues.DataBind();
        }

        private void grdAttributeValues_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = ((GridViewRow) ((Control) e.CommandSource).NamingContainer).RowIndex;
            int attributeValueId = (int) this.grdAttributeValues.DataKeys[rowIndex].Value;
            int displaySequence = int.Parse((this.grdAttributeValues.Rows[rowIndex].FindControl("lblDisplaySequence") as Literal).Text, NumberStyles.None);
            string imageUrl = e.CommandArgument.ToString();
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
            if (e.CommandName == "dele")
            {
                if (ProductTypeHelper.DeleteAttributeValue(attributeValueId))
                {
                    StoreHelper.DeleteImage(imageUrl);
                }
                else
                {
                    this.ShowMsg("该规格下存在商品", false);
                }
            }
            if (replaceAttributeValueId > 0)
            {
                ProductTypeHelper.SwapAttributeValueSequence(attributeValueId, replaceAttributeValueId, displaySequence, replaceDisplaySequence);
            }
            this.BindData();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.grdAttributeValues.RowCommand += new GridViewCommandEventHandler(this.grdAttributeValues_RowCommand);
            if (!int.TryParse(this.Page.Request.QueryString["AttributeId"], out this.attributeId))
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

