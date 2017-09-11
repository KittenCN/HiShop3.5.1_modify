namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Entities.VShop;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    public class VMyluckNum : VMemberTemplatedWebControl
    {
        private Literal litBuysum;
        private Literal litList;
        private Literal litTitle;

        protected override void AttachChildControls()
        {
            this.litBuysum = (Literal) this.FindControl("litBuysum");
            this.litList = (Literal) this.FindControl("litList");
            this.litTitle = (Literal) this.FindControl("litTitle");
            string str = Globals.RequestQueryStr("vaid");
            if (string.IsNullOrEmpty(str))
            {
                base.GotoResourceNotFound("");
            }
            OneyuanTaoInfo oneyuanTaoInfoById = OneyuanTaoHelp.GetOneyuanTaoInfoById(str);
            if (oneyuanTaoInfoById == null)
            {
                base.GotoResourceNotFound("");
            }
            this.litTitle.Text = oneyuanTaoInfoById.ProductTitle;
            IList<LuckInfo> list = OneyuanTaoHelp.getLuckInfoListByAId(str, base.CurrentMemberInfo.UserId);
            if (list == null)
            {
                base.GotoResourceNotFound("");
            }
            this.litBuysum.Text = list.Count.ToString();
            DateTime buyTime = list[0].BuyTime;
            string str2 = "<p class='timeline'>" + buyTime.ToString("yyyy-MM-dd HH:mm:ss.fff") + "</p><ul class='luckul'>";
            int num = 0;
            foreach (LuckInfo info2 in list)
            {
                if ((num == 0) && (buyTime != info2.BuyTime))
                {
                    str2 = "<p class='timeline'>" + info2.BuyTime.ToString("yyyy-MM-dd HH:mm:ss.fff") + "</p><ul class='luckul'>";
                }
                num++;
                str2 = str2 + "<li";
                if (info2.IsWin)
                {
                    str2 = str2 + " class='Win' ";
                }
                str2 = str2 + ">";
                str2 = str2 + info2.PrizeNum + "</li>";
            }
            this.litList.Text = str2;
            PageTitle.AddSiteNameTitle("我的号码");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VMyluckNum.html";
            }
            base.OnInit(e);
        }
    }
}

