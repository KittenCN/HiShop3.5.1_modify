namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using ControlPanel.Promotions;
    using global::ControlPanel.Promotions;
    using Hidistro.ControlPanel.Promotions;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VMyPrizeList : VMemberTemplatedWebControl
    {
        private Dictionary<int, string> CouponList = new Dictionary<int, string>();
        private Literal litAddAddress;
        private Literal litAddress;
        private Literal litCellPhone;
        private Literal litShipTo;
        private Literal litShowMes;
        private VshopTemplatedRepeater rptAddress;
        private VshopTemplatedRepeater rptList;
        private HtmlInputHidden selectShipTo;
        private HtmlSelect txtRevAddress;
        private HtmlInputHidden txtShowTabNum;
        private HtmlInputHidden txtTotal;

        protected override void AttachChildControls()
        {
            int num;
            int num2;
            PageTitle.AddSiteNameTitle("我的奖品");
            this.rptList = (VshopTemplatedRepeater) this.FindControl("rptList");
            this.txtTotal = (HtmlInputHidden) this.FindControl("txtTotal");
            this.txtShowTabNum = (HtmlInputHidden) this.FindControl("txtShowTabNum");
            this.litShipTo = (Literal) this.FindControl("litShipTo");
            this.litCellPhone = (Literal) this.FindControl("litCellPhone");
            this.litAddress = (Literal) this.FindControl("litAddress");
            this.rptAddress = (VshopTemplatedRepeater) this.FindControl("rptAddress");
            IList<ShippingAddressInfo> shippingAddresses = MemberProcessor.GetShippingAddresses();
            this.rptAddress.DataSource = from item in shippingAddresses
                orderby item.IsDefault
                select item;
            this.rptAddress.DataBind();
            this.litAddAddress = (Literal) this.FindControl("litAddAddress");
            this.selectShipTo = (HtmlInputHidden) this.FindControl("selectShipTo");
            ShippingAddressInfo info = shippingAddresses.FirstOrDefault<ShippingAddressInfo>(item => item.IsDefault);
            if (info == null)
            {
                info = (shippingAddresses.Count > 0) ? shippingAddresses[0] : null;
            }
            if (info != null)
            {
                this.litShipTo.Text = info.ShipTo;
                this.litCellPhone.Text = info.CellPhone;
                this.litAddress.Text = info.Address;
                this.selectShipTo.SetWhenIsNotNull(info.ShippingId.ToString());
            }
            this.litAddAddress.Text = " href='/Vshop/AddShippingAddress.aspx?returnUrl=" + Globals.UrlEncode(HttpContext.Current.Request.Url.ToString()) + "'";
            string str = Globals.RequestQueryStr("ShowTab");
            if (string.IsNullOrEmpty(str))
            {
                str = "0";
            }
            PrizesDeliveQuery entity = new PrizesDeliveQuery();
            if (!int.TryParse(this.Page.Request.QueryString["page"], out num))
            {
                num = 1;
            }
            if (!int.TryParse(this.Page.Request.QueryString["size"], out num2))
            {
                num2 = 20;
            }
            string extendLimits = "";
            if (str == "0")
            {
                string str3 = DateTime.Now.ToString("yyyy-MM-dd");
                extendLimits = " and (status in(0,1,2) or playtime>'" + str3 + "') ";
            }
            else
            {
                extendLimits = " and (status=3 or status=4)";
            }
            this.txtShowTabNum.Value = str;
            entity.Status = -1;
            entity.SortBy = "LogId";
            entity.SortOrder = SortAction.Desc;
            entity.PageIndex = num;
            entity.PrizeType = -1;
            entity.PageSize = num2;
            entity.UserId = Globals.GetCurrentMemberUserId(false);
            Globals.EntityCoding(entity, true);
            DbQueryResult result = GameHelper.GetPrizesDeliveryList(entity, extendLimits, "*");
            this.rptList.ItemDataBound += new RepeaterItemEventHandler(this.refriendscircle_ItemDataBound);
            this.CouponList.Clear();
            string str4 = "";
            DataTable data = (DataTable) result.Data;
            if ((data != null) && (data.Rows.Count > 1))
            {
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    if ((data.Rows[i]["GiveCouponId"] != DBNull.Value) && (data.Rows[i]["GiveCouponId"].ToString() != ""))
                    {
                        str4 = str4 + "," + data.Rows[i]["GiveCouponId"].ToString();
                    }
                }
            }
            if (str4.Length > 1)
            {
                DataTable couponsListByIds = CouponHelper.GetCouponsListByIds(Array.ConvertAll<string, int>(str4.Remove(0, 1).Split(new char[] { ',' }), s => int.Parse(s)));
                if ((couponsListByIds != null) && (couponsListByIds.Rows.Count > 0))
                {
                    for (int j = 0; j < couponsListByIds.Rows.Count; j++)
                    {
                        int key = (int) couponsListByIds.Rows[j]["CouponId"];
                        if (!this.CouponList.ContainsKey(key))
                        {
                            this.CouponList.Add(key, couponsListByIds.Rows[j]["CouponName"].ToString() + "[面值" + couponsListByIds.Rows[j]["CouponValue"].ToString() + "元]");
                        }
                    }
                }
            }
            this.rptList.DataSource = result.Data;
            this.rptList.DataBind();
            this.txtTotal.SetWhenIsNotNull(result.TotalRecords.ToString());
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "skin-vMyPrizeList.html";
            }
            base.OnInit(e);
        }

        private void refriendscircle_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                Literal literal = e.Item.Controls[0].FindControl("ItemHtml") as Literal;
                DataRowView dataItem = (DataRowView) e.Item.DataItem;
                if ((dataItem["GiveCouponId"] != DBNull.Value) && (dataItem["GiveCouponId"].ToString() != ""))
                {
                    int key = int.Parse(dataItem["GiveCouponId"].ToString());
                    if (this.CouponList.ContainsKey(key))
                    {
                        literal.Text = " CouponInfo='" + this.CouponList[key] + "'";
                    }
                    else
                    {
                        literal.Text = " CouponInfo=''";
                    }
                }
                else
                {
                    literal.Text = " CouponInfo=''";
                }
            }
        }
    }
}

