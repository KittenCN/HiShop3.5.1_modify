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

    public class VMyOneTao : VMemberTemplatedWebControl
    {
        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("我的夺宝");
        }

        private void DoAction(string Action)
        {
            string s = "{\"state\":false,\"msg\":\"未定义操作\"}";
            int num = Globals.RequestFormNum("pageIndex");
            if (num > 0)
            {
                OneyuanTaoPartInQuery query = new OneyuanTaoPartInQuery {
                    PageIndex = num,
                    PageSize = 10,
                    ActivityId = "",
                    UserId = Globals.GetCurrentMemberUserId(false),
                    state = 1,
                    SortBy = "BuyTime",
                    IsPay = -1
                };
                DbQueryResult oneyuanPartInDataTable = OneyuanTaoHelp.GetOneyuanPartInDataTable(query);
                IsoDateTimeConverter converter = new IsoDateTimeConverter {
                    DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
                };
                string str2 = JsonConvert.SerializeObject(oneyuanPartInDataTable.Data, new JsonConverter[] { converter });
                s = "{\"state\":true,\"msg\":\"读取成功\",\"Data\":" + str2 + "}";
            }
            else
            {
                s = "{\"state\":false,\"msg\":\"参数不正确\"}";
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
                this.SkinName = "Skin-VMyOneTao.html";
            }
            base.OnInit(e);
        }
    }
}

