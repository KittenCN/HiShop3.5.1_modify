namespace Hidistro.UI.Web.Admin.Fenxiao
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Data;
    using System.Web;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class DistributorDetails : AdminPage
    {
        protected HiImage ListImage1;
        protected HtmlGenericControl OrdersTotal;
        protected Pager pager;
        protected Repeater reCommissions;
        protected HtmlGenericControl ReferralBlance;
        protected HtmlGenericControl ReferralOrders;
        protected HtmlGenericControl ReferralRequestBalance;
        protected HiImage StoreCode;
        protected HtmlGenericControl TotalReferral;
        protected HtmlGenericControl txtCellPhone;
        protected HtmlGenericControl txtCreateTime;
        protected HtmlGenericControl txtMicroName;
        protected HtmlGenericControl txtName;
        protected HtmlGenericControl txtRealName;
        protected HtmlGenericControl txtStoreName;
        protected HtmlGenericControl txtUrl;
        protected HtmlGenericControl txtUserBindName;
        private int userid;

        protected DistributorDetails() : base("m05", "fxp03")
        {
        }

        private void BindData(int UserId)
        {
            BalanceDrawRequestQuery entity = new BalanceDrawRequestQuery {
                CheckTime = "",
                UserId = UserId.ToString(),
                RequestTime = "",
                StoreName = "",
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                SortOrder = SortAction.Desc,
                SortBy = "RequestTime",
                RequestEndTime = "",
                RequestStartTime = "",
                IsCheck = ""
            };
            Globals.EntityCoding(entity, true);
            DbQueryResult balanceDrawRequest = VShopHelper.GetBalanceDrawRequest(entity);
            this.reCommissions.DataSource = balanceDrawRequest.Data;
            this.reCommissions.DataBind();
            this.pager.TotalRecords = balanceDrawRequest.TotalRecords;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(this.Page.Request.QueryString["UserId"], out this.userid))
            {
                this.Page.Response.Redirect("DistributorList.aspx");
            }
            this.ListImage1.ImageUrl = "/Templates/common/images/user.png";
            DistributorsQuery entity = new DistributorsQuery {
                UserId = this.userid,
                ReferralStatus = -1,
                PageIndex = 1,
                PageSize = 1,
                SortOrder = SortAction.Desc,
                SortBy = "userid"
            };
            Globals.EntityCoding(entity, true);
            DbQueryResult result = VShopHelper.GetDistributors(entity, null, null);
            DataTable data = new DataTable();
            if (result.Data != null)
            {
                data = (DataTable) result.Data;
            }
            else
            {
                this.Page.Response.Redirect("DistributorList.aspx");
            }
            if ((data.Rows[0]["UserHead"] != DBNull.Value) && (data.Rows[0]["UserHead"].ToString().Trim() != ""))
            {
                this.ListImage1.ImageUrl = data.Rows[0]["UserHead"].ToString();
            }
            this.txtCellPhone.InnerText = (data.Rows[0]["CellPhone"] == DBNull.Value) ? "" : ((string) data.Rows[0]["CellPhone"]);
            this.txtStoreName.InnerText = (string) data.Rows[0]["StoreName"];
            this.txtMicroName.InnerText = (string) data.Rows[0]["UserName"];
            this.txtUserBindName.InnerText = (data.Rows[0]["UserBindName"] == DBNull.Value) ? "" : data.Rows[0]["UserBindName"].ToString();
            this.txtRealName.InnerText = (data.Rows[0]["RealName"] == DBNull.Value) ? "" : ((string) data.Rows[0]["RealName"]);
            this.txtCreateTime.InnerText = ((DateTime) data.Rows[0]["CreateTime"]).ToString("yyyy-MM-dd HH:mm:ss");
            this.txtName.InnerText = (data.Rows[0]["Name"] == DBNull.Value) ? "" : ((string) data.Rows[0]["Name"]);
            string str = Globals.HostPath(HttpContext.Current.Request.Url) + "/Default.aspx?ReferralId=" + entity.UserId;
            this.txtUrl.InnerText = str;
            this.StoreCode.ImageUrl = "http://s.jiathis.com/qrcode.php?url=" + str;
            this.OrdersTotal.InnerText = "￥" + Convert.ToDouble(data.Rows[0]["OrdersTotal"]).ToString("0.00");
            this.ReferralOrders.InnerText = data.Rows[0]["ReferralOrders"].ToString();
            this.ReferralBlance.InnerText = "￥" + Convert.ToDouble(data.Rows[0]["ReferralBlance"]).ToString("0.00");
            this.ReferralRequestBalance.InnerText = "￥" + Convert.ToDouble(data.Rows[0]["ReferralRequestBalance"]).ToString("0.00");
            decimal num = decimal.Parse(data.Rows[0]["ReferralBlance"].ToString()) + decimal.Parse(data.Rows[0]["ReferralRequestBalance"].ToString());
            this.TotalReferral.InnerText = "￥" + Convert.ToDouble(num.ToString()).ToString("0.00");
            this.BindData(entity.UserId);
        }
    }
}

