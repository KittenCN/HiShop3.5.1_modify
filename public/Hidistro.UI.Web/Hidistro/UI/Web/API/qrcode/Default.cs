namespace Hidistro.UI.Web.API.qrcode
{
    using Hidistro.ControlPanel.VShop;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.VShop;
    using Hishop.Weixin.MP.Api;
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Net;
    using System.Text.RegularExpressions;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    public class Default : Page
    {
        protected HtmlForm form1;
        protected string ReferralId = "0";
        private string webStart = Globals.GetWebUrlStart();

        private Bitmap GetBitMapImage(string path)
        {
            if (path.StartsWith("http"))
            {
                WebRequest request = WebRequest.Create(path);
                request.Timeout = 0x2710;
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                return new Bitmap(Image.FromStream(response.GetResponseStream()));
            }
            return new Bitmap(base.Server.MapPath(path));
        }

        protected string GetImgUrl()
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            string pattern = "ReferralId=(?<url>d+)";
            Match match = Regex.Match(base.Request.UrlReferrer.ToString(), pattern, RegexOptions.IgnoreCase);
            if (match.Success)
            {
                this.ReferralId = match.Value;
            }
            int userid = Globals.ToNum(this.ReferralId);
            string qRImageUrlByTicket = this.webStart + "/Follow.aspx?ReferralId=" + this.ReferralId;
            ScanInfos info = ScanHelp.GetScanInfosByUserId(userid, 0, "WX");
            if (info == null)
            {
                ScanHelp.CreatNewScan(userid, "WX", 0);
                info = ScanHelp.GetScanInfosByUserId(userid, 0, "WX");
            }
            if ((info != null) && !string.IsNullOrEmpty(info.CodeUrl))
            {
                return BarCodeApi.GetQRImageUrlByTicket(info.CodeUrl);
            }
            if (string.IsNullOrEmpty(masterSettings.WeixinAppId) || string.IsNullOrEmpty(masterSettings.WeixinAppSecret))
            {
                return "";
            }
            string token = TokenApi.GetToken_Message(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret);
            if (TokenApi.CheckIsRightToken(token))
            {
                string str5 = BarCodeApi.CreateTicket(token, info.Sceneid, "QR_LIMIT_SCENE", "2592000");
                if (!string.IsNullOrEmpty(str5))
                {
                    qRImageUrlByTicket = BarCodeApi.GetQRImageUrlByTicket(str5);
                    info.CodeUrl = str5;
                    info.CreateTime = DateTime.Now;
                    info.LastActiveTime = DateTime.Now;
                    ScanHelp.updateScanInfosCodeUrl(info);
                }
            }
            return qRImageUrlByTicket;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((base.Request.UrlReferrer != null) && base.Request.UrlReferrer.ToString().StartsWith(this.webStart))
            {
                string imgUrl = this.GetImgUrl();
                if (!string.IsNullOrEmpty(imgUrl))
                {
                    Image bitMapImage = this.GetBitMapImage(imgUrl);
                    MemoryStream stream = new MemoryStream();
                    bitMapImage.Save(stream, ImageFormat.Png);
                    base.Response.ClearContent();
                    base.Response.ContentType = "image/png";
                    base.Response.BinaryWrite(stream.ToArray());
                    stream.Dispose();
                    bitMapImage.Dispose();
                    base.Response.End();
                }
            }
        }
    }
}

