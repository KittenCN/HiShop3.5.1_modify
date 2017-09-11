namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VActivityDetail : VMemberTemplatedWebControl
    {
        private Literal litdescription;
        private VshopTemplatedRepeater rptProducts;

        protected override void AttachChildControls()
        {
            this.rptProducts = (VshopTemplatedRepeater) this.FindControl("rptProducts");
            this.litdescription = (Literal) this.FindControl("litdescription");
            ProductQuery query = new ProductQuery();
            int num = 0;
            int num2 = 0;
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["CategoryId"]) && !string.IsNullOrEmpty(this.Page.Request.QueryString["ActivitiesId"]))
            {
                if (!int.TryParse(this.Page.Request.QueryString["CategoryId"], out num))
                {
                    this.Page.Response.Redirect("Default.aspx");
                }
                else
                {
                    int.TryParse(this.Page.Request.QueryString["ActivitiesId"], out num2);
                    query.CategoryId = new int?(num);
                    DataTable activitie = ProductBrowser.GetActivitie(num2);
                    if (activitie.Rows.Count > 0)
                    {
                        this.litdescription.Text = activitie.Rows[0]["ActivitiesDescription"].ToString();
                        if ((this.rptProducts != null) && (query.CategoryId > 0))
                        {
                            query.PageSize = 20;
                            query.PageIndex = 1;
                            DbQueryResult homeProduct = ProductBrowser.GetHomeProduct(MemberProcessor.GetCurrentMember(), query);
                            this.rptProducts.DataSource = homeProduct.Data;
                            this.rptProducts.DataBind();
                        }
                    }
                    else
                    {
                        this.Page.Response.Redirect("Default.aspx");
                    }
                }
            }
            PageTitle.AddSiteNameTitle("满减活动");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-vActivityDetail.html";
            }
            base.OnInit(e);
        }
    }
}

