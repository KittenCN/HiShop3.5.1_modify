namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Entities.VShop;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    public class VOneyuanTaoResult : VMemberTemplatedWebControl
    {
        protected string DataJson = "DataJson=null";
        private Literal LitDataJson;

        protected override void AttachChildControls()
        {
            string str = Globals.RequestQueryStr("vaid");
            this.LitDataJson = (Literal) this.FindControl("LitDataJson");
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
                    this.LitDataJson.Text = this.DataJson;
                }
                else
                {
                    this.LitDataJson.Text = this.DataJson;
                }
            }
            else
            {
                this.LitDataJson.Text = this.DataJson;
            }
            PageTitle.AddSiteNameTitle("开奖计算详情");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VOneyuanTaoResult.html";
            }
            base.OnInit(e);
        }
    }
}

