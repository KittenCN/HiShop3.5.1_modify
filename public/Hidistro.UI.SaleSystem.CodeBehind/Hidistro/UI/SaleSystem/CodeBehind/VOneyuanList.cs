namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.VShop;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System;

    public class VOneyuanList : VMemberTemplatedWebControl
    {
        private int Otype = 1;
        private VshopTemplatedRepeater repList;

        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("一元夺宝");
        }

        private void DoAction(string Action)
        {
            string s = "{\"state\":false,\"msg\":\"未定义操作\"}";
            int num = Globals.RequestFormNum("pageIndex");
            if (num > 0)
            {
                this.Otype = Globals.RequestQueryNum("Otype");
                OneyuanTaoQuery query = new OneyuanTaoQuery {
                    PageIndex = num,
                    PageSize = 10,
                    title = "",
                    state = 1,
                    ReachType = 0
                };
                if (this.Otype == 0)
                {
                    this.Otype = 1;
                }
                if (this.Otype == 3)
                {
                    query.SortBy = "(FinishedNum*1.0/ReachNum)";
                }
                else if (this.Otype == 2)
                {
                    query.SortBy = "ActivityId";
                }
                else if (this.Otype == 4)
                {
                    query.state = 3;
                    query.SortBy = "ActivityId";
                }
                else
                {
                    this.Otype = 1;
                    query.SortBy = "FinishedNum";
                }
                DbQueryResult oneyuanTao = OneyuanTaoHelp.GetOneyuanTao(query);
                IsoDateTimeConverter converter = new IsoDateTimeConverter {
                    DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
                };
                string str2 = JsonConvert.SerializeObject(oneyuanTao.Data, new JsonConverter[] { converter });
                s = "{\"state\":true,\"msg\":\"读取成功\",\"Data\":" + str2 + "}";
            }
            this.Page.Response.ClearContent();
            this.Page.Response.ContentType = "application/json";
            this.Page.Response.Write(s);
            this.Page.Response.End();
        }

        protected override void OnInit(EventArgs e)
        {
            string str = Globals.RequestFormStr("action");
            if (!string.IsNullOrEmpty(str))
            {
                this.DoAction(str);
                this.Page.Response.End();
            }
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-OneyuanList.html";
            }
            base.OnInit(e);
        }
    }
}

