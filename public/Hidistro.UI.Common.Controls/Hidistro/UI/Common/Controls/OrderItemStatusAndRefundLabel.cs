namespace Hidistro.UI.Common.Controls
{
    using Hidistro.ControlPanel.Sales;
    using Hidistro.Entities;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class OrderItemStatusAndRefundLabel : Literal
    {
        private string _DetailUrl = string.Empty;
        private string _OrderID = string.Empty;
        private int _OrderItemID;
        private string _SkuID = string.Empty;
        private int _Type;

        protected override void Render(HtmlTextWriter writer)
        {
            if ((this._OrderItemID == 0) && (string.IsNullOrEmpty(this._OrderID) || string.IsNullOrEmpty(this._SkuID)))
            {
                base.Text = "";
            }
            else
            {
                string str = string.Empty;
                if (this._Type != 1)
                {
                    RefundInfo info = RefundHelper.GetByOrderIdAndProductID(this._OrderID, 0, this._SkuID, this._OrderItemID);
                    if (info != null)
                    {
                        switch (info.HandleStatus)
                        {
                            case RefundInfo.Handlestatus.Applied:
                                str = "退款中";
                                break;

                            case RefundInfo.Handlestatus.Refunded:
                                str = "已退款";
                                break;

                            case RefundInfo.Handlestatus.Refused:
                                str = "拒绝退款";
                                break;

                            case RefundInfo.Handlestatus.NoneAudit:
                                str = "退款待审核";
                                break;

                            case RefundInfo.Handlestatus.HasTheAudit:
                                str = "退款已审核";
                                break;

                            case RefundInfo.Handlestatus.NoRefund:
                                str = "待退款";
                                break;

                            case RefundInfo.Handlestatus.AuditNotThrough:
                                str = "拒绝退款";
                                break;

                            case RefundInfo.Handlestatus.RefuseRefunded:
                                str = "拒绝退款";
                                break;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(this._DetailUrl) && !string.IsNullOrEmpty(str))
                {
                    base.Text = "<a href='" + this._DetailUrl + "' target='_blank'>" + str + "</a>";
                }
                else
                {
                    base.Text = str;
                }
            }
            base.Render(writer);
        }

        public string DetailUrl
        {
            get
            {
                return this._DetailUrl;
            }
            set
            {
                this._DetailUrl = value;
            }
        }

        public string OrderID
        {
            get
            {
                return this._OrderID;
            }
            set
            {
                this._OrderID = value;
            }
        }

        public int OrderItemID
        {
            get
            {
                return this._OrderItemID;
            }
            set
            {
                this._OrderItemID = value;
            }
        }

        public object OrderStatusCode
        {
            get
            {
                return this.ViewState["OrderStatusCode"];
            }
            set
            {
                this.ViewState["OrderStatusCode"] = value;
            }
        }

        public string SkuID
        {
            get
            {
                return this._SkuID;
            }
            set
            {
                this._SkuID = value;
            }
        }

        public int Type
        {
            get
            {
                return this._Type;
            }
            set
            {
                this._Type = value;
            }
        }
    }
}

