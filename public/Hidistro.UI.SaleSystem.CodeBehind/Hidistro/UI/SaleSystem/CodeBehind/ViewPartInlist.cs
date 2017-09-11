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

    public class ViewPartInlist : VMemberTemplatedWebControl
    {
        protected override void AttachChildControls()
        {
            PageTitle.AddSiteNameTitle("夺宝记录");
        }

        private void DoAction(string Action)
        {
            string s = "{\"state\":false,\"msg\":\"未定义操作\"}";
            int num = Globals.RequestFormNum("pageIndex");
            string str2 = Globals.RequestFormStr("ActivityId");
            if ((num > 0) && !string.IsNullOrEmpty(str2))
            {
                OneyuanTaoPartInQuery query = new OneyuanTaoPartInQuery {
                    PageIndex = num,
                    PageSize = 10,
                    ActivityId = str2,
                    IsPay = 1,
                    SortBy = "Pid"
                };
                DbQueryResult oneyuanPartInDataTable = OneyuanTaoHelp.GetOneyuanPartInDataTable(query);
                IsoDateTimeConverter converter = new IsoDateTimeConverter {
                    DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
                };
                string str3 = JsonConvert.SerializeObject(oneyuanPartInDataTable.Data, new JsonConverter[] { converter });
                s = "{\"state\":true,\"msg\":\"读取成功\",\"Data\":" + str3 + "}";
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
                this.SkinName = "Skin-ViewPartInlist.html";
            }
            base.OnInit(e);
        }
    }
}

