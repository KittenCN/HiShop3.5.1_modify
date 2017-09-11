namespace Hidistro.UI.Web.Admin.Oneyuan
{
    using Hidistro.Entities.VShop;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.ControlPanel.Utility;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.HtmlControls;

    public class OneTaoResult : AdminPage
    {
        protected string DataJson;
        protected HtmlGenericControl txtEditInfo;
        protected OneTaoViewTab ViewTab1;

        protected OneTaoResult() : base("m08", "yxp20")
        {
            this.DataJson = "DataJson=null";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string str = base.Request.QueryString["vaid"];
            if (!string.IsNullOrEmpty(str))
            {
                OneyuanTaoInfo oneyuanTaoInfoById = OneyuanTaoHelp.GetOneyuanTaoInfoById(str);
                if (oneyuanTaoInfoById != null)
                {
                    string prizeCountInfo = oneyuanTaoInfoById.PrizeCountInfo;
                    oneyuanTaoInfoById.PrizeCountInfo = "";
                    IsoDateTimeConverter converter = new IsoDateTimeConverter {
                        DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
                    };
                    this.DataJson = "DataJson=" + JsonConvert.SerializeObject(oneyuanTaoInfoById, new JsonConverter[] { converter });
                    IList<LuckInfo> list = OneyuanTaoHelp.getLuckInfoList(true, str);
                    string str3 = JsonConvert.SerializeObject(list, new JsonConverter[] { converter });
                    if (!string.IsNullOrEmpty(prizeCountInfo))
                    {
                        this.DataJson = this.DataJson + ";\r\n PrizeCountInfo=" + prizeCountInfo;
                    }
                    if (list.Count > 0)
                    {
                        this.DataJson = this.DataJson + ";\r\nWinInfo=" + str3;
                    }
                }
            }
        }
    }
}

