namespace Hidistro.UI.Common.Controls
{
    using Hidistro.Entities.Promotions;
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class GroupBuyStatusLabel : Label
    {
        private object _groupbuyStartTime;

        protected override void Render(HtmlTextWriter writer)
        {
            switch (((GroupBuyStatus) this.GroupBuyStatusCode))
            {
                case GroupBuyStatus.UnderWay:
                    if (DateTime.Compare(Convert.ToDateTime(this.GroupBuyStartTime), DateTime.Now) > 0)
                    {
                        base.Text = "还未开始";
                        break;
                    }
                    base.Text = "正在进行中";
                    break;

                case GroupBuyStatus.EndUntreated:
                    base.Text = "结束未处理";
                    break;

                case GroupBuyStatus.Success:
                    base.Text = "成功结束";
                    break;

                case GroupBuyStatus.Failed:
                    base.Text = "失败结束";
                    break;

                case GroupBuyStatus.FailedWaitForReFund:
                    base.Text = "失败待退款";
                    break;

                default:
                    base.Text = "-";
                    break;
            }
            base.Render(writer);
        }

        public object GroupBuyStartTime
        {
            get
            {
                return this._groupbuyStartTime;
            }
            set
            {
                this._groupbuyStartTime = value;
            }
        }

        public object GroupBuyStatusCode
        {
            get
            {
                return this.ViewState["GroupBuyStatusCode"];
            }
            set
            {
                this.ViewState["GroupBuyStatusCode"] = value;
            }
        }
    }
}

