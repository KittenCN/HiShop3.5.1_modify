namespace Hidistro.UI.Web.Admin.Oneyuan
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.VShop;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.Admin;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System;
    using System.Collections.Specialized;
    using System.Web.UI.HtmlControls;

    public class AddOneyuanInfo : AdminPage
    {
        protected string EditId;
        protected string EditJsonDataStr;
        protected HtmlImage idImg;
        protected Hidistro.SaleSystem.Vshop.OneTaoState OneTaoState;
        protected Hidistro.UI.Web.Admin.SetMemberRange SetMemberRange;
        protected HtmlForm thisForm;
        protected HtmlGenericControl txtEditInfo;
        protected string viewAid;
        protected OneTaoViewTab ViewTab1;

        protected AddOneyuanInfo() : base("m08", "yxp20")
        {
            this.EditId = "";
            this.OneTaoState = Hidistro.SaleSystem.Vshop.OneTaoState.NONE;
            this.EditJsonDataStr = "EditJsonData=''";
            this.viewAid = "";
        }

        private void AjaxAction(string action)
        {
            string s = "{\"state\":false,\"msg\":\"未定义操作\"}";
            NameValueCollection form = base.Request.Form;
            string str2 = action;
            if (str2 != null)
            {
                if (!(str2 == "edit"))
                {
                    if (str2 == "save")
                    {
                        try
                        {
                            OneyuanTaoInfo info2 = new OneyuanTaoInfo {
                                IsOn = true,
                                IsEnd = false,
                                PrizeNumber = int.Parse(form["PrizeNumber"]),
                                ActivityDec = form["ActivityDec"],
                                Title = form["Title"],
                                StartTime = DateTime.Parse(form["StartTime"]),
                                EndTime = DateTime.Parse(form["EndTime"]),
                                EachPrice = decimal.Parse(form["EachPrice"]),
                                ReachNum = int.Parse(form["ReachNum"]),
                                EachCanBuyNum = int.Parse(form["EachCanBuyNum"]),
                                FitMember = form["FitMember"],
                                DefualtGroup = form["DefualtGroup"],
                                CustomGroup = form["CustomGroup"],
                                FinishedNum = 0,
                                SkuId = "N",
                                ProductImg = form["ProductImg"],
                                ProductId = int.Parse(form["ProductId"]),
                                HeadImgage = form["HeadImgage"],
                                ReachType = int.Parse(form["ReachType"]),
                                ProductPrice = decimal.Parse(form["ProductPrice"]),
                                ProductTitle = form["ProductTitle"]
                            };
                            if (info2.ActivityDec.Length > 100)
                            {
                                s = "{\"state\":false,\"msg\":\"活动描述信息太长！\"}";
                            }
                            else if (OneyuanTaoHelp.AddOneyuanTao(info2))
                            {
                                s = "{\"state\":true,\"msg\":\"保存活动成功\"}";
                            }
                            else
                            {
                                s = "{\"state\":false,\"msg\":\"保存活动失败\"}";
                            }
                        }
                        catch (Exception exception)
                        {
                            s = "{\"state\":false,\"msg\":\"" + exception.Message.Replace("'", " ").Replace("\r\n", " ") + "\"}";
                        }
                    }
                    else if (str2 == "read")
                    {
                        s = "{\"state\":false,\"msg\":\"读取数据\"}";
                    }
                }
                else
                {
                    OneyuanTaoInfo oneyuanTaoInfoById = OneyuanTaoHelp.GetOneyuanTaoInfoById(form["ActivityId"]);
                    if (oneyuanTaoInfoById != null)
                    {
                        this.OneTaoState = OneyuanTaoHelp.getOneTaoState(oneyuanTaoInfoById);
                        if (this.OneTaoState == Hidistro.SaleSystem.Vshop.OneTaoState.已结束)
                        {
                            s = "{\"state\":false,\"msg\":\"当前活动已结束，不能再修改！\"}";
                        }
                        else
                        {
                            oneyuanTaoInfoById.ActivityDec = form["ActivityDec"];
                            oneyuanTaoInfoById.Title = form["Title"];
                            oneyuanTaoInfoById.FitMember = form["FitMember"];
                            oneyuanTaoInfoById.DefualtGroup = form["DefualtGroup"];
                            oneyuanTaoInfoById.CustomGroup = form["CustomGroup"];
                            oneyuanTaoInfoById.HeadImgage = form["HeadImgage"];
                            if (this.OneTaoState == Hidistro.SaleSystem.Vshop.OneTaoState.未开始)
                            {
                                oneyuanTaoInfoById.ProductId = int.Parse(form["ProductId"]);
                                oneyuanTaoInfoById.StartTime = DateTime.Parse(form["StartTime"]);
                                oneyuanTaoInfoById.EndTime = DateTime.Parse(form["EndTime"]);
                                oneyuanTaoInfoById.EachPrice = decimal.Parse(form["EachPrice"]);
                                oneyuanTaoInfoById.ReachNum = int.Parse(form["ReachNum"]);
                                oneyuanTaoInfoById.EachCanBuyNum = int.Parse(form["EachCanBuyNum"]);
                                oneyuanTaoInfoById.PrizeNumber = int.Parse(form["PrizeNumber"]);
                                oneyuanTaoInfoById.ReachType = int.Parse(form["ReachType"]);
                                oneyuanTaoInfoById.ProductPrice = decimal.Parse(form["ProductPrice"]);
                                oneyuanTaoInfoById.ProductTitle = form["ProductTitle"];
                                oneyuanTaoInfoById.FinishedNum = 0;
                                oneyuanTaoInfoById.SkuId = "N";
                                oneyuanTaoInfoById.ProductImg = form["ProductImg"];
                            }
                            if (oneyuanTaoInfoById.ActivityDec.Length > 100)
                            {
                                s = "{\"state\":false,\"msg\":\"活动描述信息太长！\"}";
                            }
                            else if (OneyuanTaoHelp.UpdateOneyuanTao(oneyuanTaoInfoById))
                            {
                                s = "{\"state\":true,\"msg\":\"活动修改成功！\"}";
                            }
                            else
                            {
                                s = "{\"state\":false,\"msg\":\"修改失败！\"}";
                            }
                        }
                    }
                    else
                    {
                        s = "{\"state\":false,\"msg\":\"活动信息不存在，可能已删除！\"}";
                    }
                }
            }
            base.Response.ClearContent();
            base.Response.ContentType = "application/json";
            base.Response.Write(s);
            base.Response.End();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string str = base.Request.Form["action"];
            if (!string.IsNullOrEmpty(str))
            {
                this.AjaxAction(str);
            }
            this.EditId = base.Request.QueryString["aid"];
            this.viewAid = base.Request.QueryString["vaid"];
            if (string.IsNullOrEmpty(this.EditId))
            {
                this.EditId = this.viewAid;
            }
            if (!string.IsNullOrEmpty(this.EditId))
            {
                OneyuanTaoInfo oneyuanTaoInfoById = OneyuanTaoHelp.GetOneyuanTaoInfoById(this.EditId);
                if (oneyuanTaoInfoById != null)
                {
                    this.SetMemberRange.Grade = oneyuanTaoInfoById.FitMember;
                    this.SetMemberRange.DefualtGroup = oneyuanTaoInfoById.DefualtGroup;
                    this.SetMemberRange.CustomGroup = oneyuanTaoInfoById.CustomGroup;
                    this.OneTaoState = OneyuanTaoHelp.getOneTaoState(oneyuanTaoInfoById);
                    if ((this.OneTaoState == Hidistro.SaleSystem.Vshop.OneTaoState.已结束) && string.IsNullOrEmpty(this.viewAid))
                    {
                        base.Response.Redirect("OneyuanList.aspx");
                        base.Response.End();
                    }
                    ProductInfo productBaseInfo = ProductHelper.GetProductBaseInfo(oneyuanTaoInfoById.ProductId);
                    if (productBaseInfo != null)
                    {
                        oneyuanTaoInfoById.MaxPrice = productBaseInfo.MarketPrice.Value;
                        oneyuanTaoInfoById.storeKc = ProductHelper.GetProductSumStock(oneyuanTaoInfoById.ProductId);
                    }
                    IsoDateTimeConverter converter = new IsoDateTimeConverter {
                        DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
                    };
                    this.EditJsonDataStr = "EditJsonData=" + JsonConvert.SerializeObject(oneyuanTaoInfoById, new JsonConverter[] { converter });
                    if (!string.IsNullOrEmpty(this.viewAid))
                    {
                        this.txtEditInfo.InnerHtml = "查看一元夺宝<small>当前为查看模式，不可编辑活动内容</small>";
                    }
                    else
                    {
                        this.txtEditInfo.InnerHtml = "编辑一元夺宝<small>进行中的活动，只可以修改活动标题及活动详情，其它信息不能修改</small>";
                    }
                }
            }
        }
    }
}

