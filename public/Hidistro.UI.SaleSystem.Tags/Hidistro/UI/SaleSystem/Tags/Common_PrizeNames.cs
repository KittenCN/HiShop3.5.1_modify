namespace Hidistro.UI.SaleSystem.Tags
{
    using Hidistro.Entities.VShop;
    using System;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class Common_PrizeNames : WebControl
    {
        protected override void Render(HtmlTextWriter writer)
        {
            if (this.Activity != null)
            {
                StringBuilder builder = new StringBuilder();
                foreach (PrizeSetting setting in this.Activity.PrizeSettingList)
                {
                    builder.AppendFormat("{0}：{1} ({2}名)<br/>", setting.PrizeLevel, setting.PrizeName, setting.PrizeNum);
                }
                writer.Write(builder.ToString());
            }
        }

        public LotteryActivityInfo Activity { get; set; }
    }
}

