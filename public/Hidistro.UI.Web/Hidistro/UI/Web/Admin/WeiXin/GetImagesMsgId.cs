namespace Hidistro.UI.Web.Admin.WeiXin
{
    using global::ControlPanel.WeiBo;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hishop.Weixin.MP.Api;
    using Hishop.Weixin.MP.Domain;
    using Newtonsoft.Json;
    using System;
    using System.Data;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    public class GetImagesMsgId : Page
    {
        protected HtmlForm form1;

        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable noImgMsgIdArticleList = ArticleHelper.GetNoImgMsgIdArticleList();
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            string str = JsonConvert.DeserializeObject<Token>(TokenApi.GetToken(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret)).access_token;
            if (noImgMsgIdArticleList.Rows.Count > 0)
            {
                for (int i = 0; i < noImgMsgIdArticleList.Rows.Count; i++)
                {
                    string jsonValue = NewsApi.GetJsonValue(NewsApi.GetMedia_IDByPath(str, noImgMsgIdArticleList.Rows[i]["ImageUrl"].ToString()), "media_id");
                    if (!string.IsNullOrEmpty(jsonValue))
                    {
                        ArticleHelper.UpdateMedia_Id(0, Globals.ToNum(noImgMsgIdArticleList.Rows[i]["ArticleId"].ToString()), jsonValue);
                    }
                }
            }
            noImgMsgIdArticleList = ArticleHelper.GetNoImgMsgIdArticleItemList();
            if (noImgMsgIdArticleList.Rows.Count > 0)
            {
                for (int j = 0; j < noImgMsgIdArticleList.Rows.Count; j++)
                {
                    string str3 = NewsApi.GetJsonValue(NewsApi.GetMedia_IDByPath(str, noImgMsgIdArticleList.Rows[j]["ImageUrl"].ToString()), "media_id");
                    if (!string.IsNullOrEmpty(str3))
                    {
                        ArticleHelper.UpdateMedia_Id(1, Globals.ToNum(noImgMsgIdArticleList.Rows[j]["ID"].ToString()), str3);
                    }
                }
            }
            HttpContext.Current.Response.Write("document.write('');");
            HttpContext.Current.Response.End();
        }
    }
}

