namespace Hidistro.UI.Web.Admin.Shop
{
    using  global:: ControlPanel.WeiBo;
    using  global:: ControlPanel.WeiXin;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Data;

    public class Articles : AdminPage
    {
        protected int articleid;
        protected string ArticleTitle;
        protected int articletype;

        protected Articles() : base("m01", "dpp06")
        {
            this.ArticleTitle = string.Empty;
        }

        private void LoadParameters(string _stype)
        {
            int.TryParse(_stype, out this.articletype);
            if (!string.IsNullOrEmpty(this.Page.Request.QueryString["key"]))
            {
                this.ArticleTitle = base.Server.UrlDecode(this.Page.Request.QueryString["key"]);
            }
        }

        [PrivilegeCheck(Privilege.Summary)]
        protected void Page_Load(object sender, EventArgs e)
        {
            string str = base.Request.QueryString["articletype"];
            string str2 = base.Request.Form["posttype"];
            string s = base.Request.Form["id"];
            int.TryParse(s, out this.articleid);
            if ((str2 == "del") && (this.articleid > 0))
            {
                base.Response.ContentType = "application/json";
                string str4 = "{\"type\":\"0\",\"tips\":\"操作失败\"}";
                DataSet set = ArticleHelper.ArticleIsInWeiXinReply(this.articleid);
                DataTable table = set.Tables[0];
                DataTable table2 = set.Tables[1];
                DataTable table3 = set.Tables[2];
                if (Globals.ToNum(table.Rows[0][0]) > 0)
                {
                    str4 = "{\"type\":\"0\",\"tips\":\"删除失败，该素材已在栏目“微信->自动回复”中使用。\"}";
                }
                else if (Globals.ToNum(table2.Rows[0][0]) > 0)
                {
                    str4 = "{\"type\":\"0\",\"tips\":\"删除失败，该素材已在栏目“微博->自动回复”中使用。\"}";
                }
                else if (Globals.ToNum(table3.Rows[0][0]) > 0)
                {
                    str4 = "{\"type\":\"0\",\"tips\":\"删除失败，该素材已在栏目“服务窗->自动回复”中使用。\"}";
                }
                else if (ArticleHelper.DeleteArticle(this.articleid))
                {
                    str4 = "{\"type\":\"1\",\"tips\":\"删除成功\"}";
                }
                base.Response.Write(str4);
                base.Response.End();
            }
            else if (str2 == "clearweixin")
            {
                base.Response.ContentType = "application/json";
                WeiXinHelper.ClearWeiXinMediaID();
                string str5 = "{\"type\":\"1\",\"tips\":\"更新成功\"}";
                base.Response.Write(str5);
                base.Response.End();
            }
            else
            {
                this.LoadParameters(str);
            }
        }
    }
}

