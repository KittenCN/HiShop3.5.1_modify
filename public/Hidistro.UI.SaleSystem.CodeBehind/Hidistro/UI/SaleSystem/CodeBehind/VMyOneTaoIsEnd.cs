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
    using System.Collections.Generic;
    using System.Data;

    public class VMyOneTaoIsEnd : VMemberTemplatedWebControl
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
                    state = 2,
                    SortBy = "BuyTime",
                    IsPay = -1
                };
                DbQueryResult oneyuanPartInDataTable = OneyuanTaoHelp.GetOneyuanPartInDataTable(query);
                DataTable data = new DataTable();
                IsoDateTimeConverter converter = new IsoDateTimeConverter {
                    DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
                };
                if (oneyuanPartInDataTable.Data != null)
                {
                    data = (DataTable) oneyuanPartInDataTable.Data;
                    data = (DataTable) oneyuanPartInDataTable.Data;
                    data.Columns.Add("LuckNumList");
                    data.Columns.Add("AState");
                    foreach (DataRow row in data.Rows)
                    {
                        IList<LuckInfo> list = OneyuanTaoHelp.getWinnerLuckInfoList(row["ActivityId"].ToString(), "");
                        if (list != null)
                        {
                            row["LuckNumList"] = JsonConvert.SerializeObject(list, new JsonConverter[] { converter });
                        }
                        OneyuanTaoInfo info = OneyuanTaoHelp.DataRowToOneyuanTaoInfo(row);
                        OneTaoPrizeState state = OneyuanTaoHelp.getPrizeState(info);
                        row["AState"] = state;
                        if (info.IsSuccess)
                        {
                            row["AState"] = "已开奖";
                        }
                        else if (info.HasCalculate)
                        {
                            row["AState"] = "已结束（开奖失败）";
                        }
                    }
                }
                string str2 = JsonConvert.SerializeObject(data, new JsonConverter[] { converter });
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
                this.SkinName = "Skin-VMyOneTaoIsEnd.html";
            }
            base.OnInit(e);
        }
    }
}

