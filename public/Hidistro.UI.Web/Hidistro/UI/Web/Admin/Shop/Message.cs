namespace Hidistro.UI.Web.Admin.Shop
{
    using ASPNET.WebControls;
    using  global:: ControlPanel.WeiBo;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Weibo;
    using Hidistro.UI.ControlPanel.Utility;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Specialized;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class Message : AdminPage
    {
        public string alldata;
        protected HtmlInputHidden hidstatus;
        public string nodata;
        protected Pager pager;
        private string Reply;
        protected CheckBox replycheck;
        protected Repeater RepMessage;
        private string status;

        protected Message() : base("m07", "wbp04")
        {
            this.status = "-1";
            this.Reply = "2";
            this.alldata = "class=\"active\"";
            this.nodata = "";
        }

        public void bind()
        {
            MessageQuery messageQuery = new MessageQuery();
            if (!this.replycheck.Checked)
            {
                messageQuery.Status = int.Parse(this.status);
            }
            else
            {
                messageQuery.Status = 2;
            }
            messageQuery.PageSize = this.pager.PageSize;
            messageQuery.PageIndex = this.pager.PageIndex;
            DbQueryResult messages = WeiboHelper.GetMessages(messageQuery);
            this.RepMessage.DataSource = messages.Data;
            this.RepMessage.DataBind();
            this.pager.TotalRecords = messages.TotalRecords;
        }

        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            this.ReBind(true);
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Status"]))
                {
                    this.status = base.Server.UrlDecode(this.Page.Request.QueryString["Status"]);
                }
                if (this.status == "-1")
                {
                    this.alldata = "class=\"active\"";
                    this.nodata = "";
                }
                else
                {
                    this.alldata = "";
                    this.nodata = "class=\"active\"";
                }
                this.hidstatus.Value = this.status;
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Reply"]))
                {
                    this.Reply = base.Server.UrlDecode(this.Page.Request.QueryString["Reply"]);
                    if (this.Reply == "2")
                    {
                        this.replycheck.Checked = true;
                    }
                }
            }
            else
            {
                this.status = this.hidstatus.Value;
                this.Reply = this.replycheck.Checked ? "2" : "";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.RepMessage.ItemDataBound += new RepeaterItemEventHandler(this.RepMessage_ItemDataBound);
            this.LoadParameters();
            if (!base.IsPostBack)
            {
                this.bind();
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("Status", this.hidstatus.Value);
            queryStrings.Add("Reply", this.replycheck.Checked ? "2" : "");
            base.ReloadPage(queryStrings);
        }

        private void RepMessage_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                string id = DataBinder.Eval(e.Item.DataItem, "Sender_id").ToString();
                global::ControlPanel.WeiBo.WeiBo bo = new global::ControlPanel.WeiBo.WeiBo();
                JObject obj2 = JObject.Parse(bo.userinfo(id));
                Literal literal = (Literal) e.Item.FindControl("LitUserName");
                if (obj2["id"] != null)
                {
                    Image image = (Image) e.Item.FindControl("Pic");
                    image.ImageUrl = obj2["profile_image_url"].ToString();
                    literal.Text = obj2["screen_name"].ToString();
                }
                if (string.IsNullOrEmpty(literal.Text))
                {
                    literal.Text = "您访问太过频繁，微博接口禁止调用。";
                }
            }
        }
    }
}

