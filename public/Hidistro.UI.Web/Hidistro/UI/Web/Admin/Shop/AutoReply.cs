namespace Hidistro.UI.Web.Admin.Shop
{
    using ASPNET.WebControls;
    using  global:: ControlPanel.WeiBo;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Weibo;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class AutoReply : AdminPage
    {
        protected bool _enable;
        protected Button BtnKey;
        protected HtmlInputHidden hidid;
        protected HtmlInputRadioButton Matching1;
        protected HtmlInputRadioButton Matching2;
        protected Pager pager;
        protected Repeater repreplykey;
        protected HtmlInputText txtkey;

        protected AutoReply() : base("m07", "wbp07")
        {
        }

        public void bind()
        {
            this.repreplykey.DataSource = WeiboHelper.GetTopReplyInfos(1);
            this.repreplykey.DataBind();
        }

        private void BtnKey_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtkey.Value.Trim()))
            {
                this.ShowMsg("关键词不能为空！", false);
            }
            else
            {
                ReplyKeyInfo replyKeyInfo = new ReplyKeyInfo {
                    Keys = this.txtkey.Value,
                    Type = 1
                };
                if (!string.IsNullOrEmpty(this.hidid.Value))
                {
                    replyKeyInfo.Id = int.Parse(this.hidid.Value);
                    if (WeiboHelper.UpdateReplyKeyInfo(replyKeyInfo))
                    {
                        this.ShowMsg("关键词修改成功！", true);
                        this.bind();
                    }
                    else
                    {
                        this.ShowMsg("关键词修改失败！", false);
                    }
                }
                else if (WeiboHelper.SaveReplyKeyInfo(replyKeyInfo))
                {
                    this.ShowMsg("关键词添加成功！", true);
                    this.bind();
                }
                else
                {
                    this.ShowMsg("关键词添加失败！", false);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.BtnKey.Click += new EventHandler(this.BtnKey_Click);
            this.repreplykey.ItemDataBound += new RepeaterItemEventHandler(this.repreplykey_ItemDataBound);
            this.repreplykey.ItemCommand += new RepeaterCommandEventHandler(this.repreplykey_ItemCommand);
            if (!base.IsPostBack)
            {
                this.bind();
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                this._enable = masterSettings.CustomReply;
            }
        }

        private void repreplykey_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                if (WeiboHelper.GetReplyInfo(int.Parse(e.CommandArgument.ToString())).Count > 0)
                {
                    this.ShowMsg("关键词中有回复信息，请先删除回复信息！", false);
                }
                else if (WeiboHelper.DeleteReplyKeyInfo(int.Parse(e.CommandArgument.ToString())))
                {
                    this.ShowMsg("关键词删除成功！", true);
                    this.bind();
                }
                else
                {
                    this.ShowMsg("关键词删除失败！", false);
                }
            }
        }

        private void repreplykey_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                Repeater repeater = (Repeater) e.Item.FindControl("repreply");
                int replyKeyId = (int) DataBinder.Eval(e.Item.DataItem, "id");
                repeater.DataSource = WeiboHelper.GetReplyInfo(replyKeyId);
                repeater.DataBind();
            }
        }
    }
}

