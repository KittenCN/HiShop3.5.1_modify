namespace Hidistro.UI.Web.Admin.Oneyuan
{
    using ASPNET.WebControls;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.VShop;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data;
    using System.Globalization;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class OneTaoPartInList : AdminPage
    {
        protected Button btnSearchButton;
        protected Repeater Datalist;
        private Dictionary<string, string> LuckAllDic;
        private Dictionary<string, string> LuckDic;
        private string NickName;
        protected Pager pager;
        protected CheckBox ShowIsPrize;
        private bool ShowType;
        protected HtmlGenericControl txtEditInfo;
        protected TextBox txtUserName;
        private string VaidStr;
        protected OneTaoViewTab ViewTab1;

        protected OneTaoPartInList() : base("m08", "yxp20")
        {
            this.NickName = "";
            this.VaidStr = "";
            this.LuckDic = new Dictionary<string, string>();
            this.LuckAllDic = new Dictionary<string, string>();
        }

        private void BindData()
        {
            OneyuanTaoPartInQuery query = new OneyuanTaoPartInQuery {
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                ActivityId = this.VaidStr,
                IsPay = 1,
                SortBy = "Pid",
                UserName = this.NickName
            };
            if (this.ShowType)
            {
                query.state = 3;
            }
            DbQueryResult oneyuanPartInDataTable = OneyuanTaoHelp.GetOneyuanPartInDataTable(query);
            IList<LuckInfo> luckWinlist = OneyuanTaoHelp.getLuckInfoList(true, this.VaidStr);
            IList<LuckInfo> luckLoselist = OneyuanTaoHelp.getLuckInfoList(false, this.VaidStr);
            this.initLuckDic(luckWinlist, luckLoselist);
            this.Datalist.DataSource = oneyuanPartInDataTable.Data;
            this.Datalist.DataBind();
            this.pager.TotalRecords = oneyuanPartInDataTable.TotalRecords;
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            this.ReBind(true);
        }

        private void initLuckDic(IList<LuckInfo> luckWinlist, IList<LuckInfo> luckLoselist)
        {
            foreach (LuckInfo info in luckWinlist)
            {
                if (!this.LuckDic.ContainsKey(info.Pid))
                {
                    this.LuckDic.Add(info.Pid, info.PrizeNum);
                }
                else
                {
                    Dictionary<string, string> dictionary;
                    string str;
                    (dictionary = this.LuckDic)[str = info.Pid] = dictionary[str] + "<br/>" + info.PrizeNum;
                }
            }
            this.LuckAllDic = new Dictionary<string, string>(this.LuckDic);
            foreach (LuckInfo info2 in luckLoselist)
            {
                if (!this.LuckAllDic.ContainsKey(info2.Pid))
                {
                    this.LuckAllDic.Add(info2.Pid, info2.PrizeNum);
                }
                else
                {
                    Dictionary<string, string> dictionary2;
                    string str2;
                    (dictionary2 = this.LuckAllDic)[str2 = info2.Pid] = dictionary2[str2] + "<br/>" + info2.PrizeNum;
                }
            }
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["NickName"]))
                {
                    this.NickName = base.Server.UrlDecode(this.Page.Request.QueryString["NickName"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ShowType"]))
                {
                    bool.TryParse(this.Page.Request.QueryString["ShowType"], out this.ShowType);
                }
                this.VaidStr = this.Page.Request.QueryString["vaid"];
                this.ShowIsPrize.Checked = this.ShowType;
                this.txtUserName.Text = this.NickName;
            }
            else
            {
                this.ShowType = this.ShowIsPrize.Checked;
                this.NickName = this.txtUserName.Text;
                this.VaidStr = this.Page.Request.QueryString["vaid"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadParameters();
            if (!this.Page.IsPostBack)
            {
                this.BindData();
            }
            this.btnSearchButton.Click += new EventHandler(this.btnSearchButton_Click);
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("vaid", this.VaidStr);
            queryStrings.Add("NickName", this.NickName);
            queryStrings.Add("ShowType", this.ShowType.ToString());
            queryStrings.Add("pageSize", this.pager.PageSize.ToString(CultureInfo.InvariantCulture));
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            base.ReloadPage(queryStrings);
        }

        protected void rptypelist_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                Literal literal = e.Item.FindControl("PrizeNum") as Literal;
                Literal literal2 = e.Item.FindControl("AllPrizeNum") as Literal;
                DataRowView dataItem = (DataRowView) e.Item.DataItem;
                if ((bool) dataItem["IsWin"])
                {
                    if (this.LuckDic.ContainsKey(dataItem["Pid"].ToString()))
                    {
                        literal.Text = this.LuckDic[dataItem["Pid"].ToString()];
                    }
                    else
                    {
                        literal.Text = "";
                    }
                }
                else
                {
                    literal.Text = "";
                }
                if (this.LuckAllDic.ContainsKey(dataItem["Pid"].ToString()))
                {
                    literal2.Text = this.LuckAllDic[dataItem["Pid"].ToString()];
                }
                else
                {
                    literal2.Text = "无号码";
                }
            }
        }

        protected void ShowIsPrize_CheckedChanged1(object sender, EventArgs e)
        {
            this.ReBind(false);
        }
    }
}

