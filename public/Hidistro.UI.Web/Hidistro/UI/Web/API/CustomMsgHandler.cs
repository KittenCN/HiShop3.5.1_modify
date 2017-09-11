namespace Hidistro.UI.Web.API
{
    using  global:: ControlPanel.WeiBo;
    using  global:: ControlPanel.WeiXin;
    using Hidistro.ControlPanel.Store;
    using Hidistro.ControlPanel.VShop;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.VShop;
    using Hidistro.Entities.Weibo;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.SaleSystem.CodeBehind;
    using Hishop.Weixin.MP;
    using Hishop.Weixin.MP.Api;
    using Hishop.Weixin.MP.Domain;
    using Hishop.Weixin.MP.Handler;
    using Hishop.Weixin.MP.Request;
    using Hishop.Weixin.MP.Request.Event;
    using Hishop.Weixin.MP.Response;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Web;

    public class CustomMsgHandler : RequestHandler
    {
        public CustomMsgHandler(Stream inputStream) : base(inputStream)
        {
        }

        private bool CreatMember(string OpenId, int ReferralUserId, string AceessTokenDefault = "")
        {
            if (string.IsNullOrEmpty(AceessTokenDefault))
            {
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
                AceessTokenDefault = TokenApi.GetToken_Message(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret);
            }
            string nickName = "";
            string headImageUrl = "";
            string retInfo = "";
            BarCodeApi.GetHeadImageUrlByOpenID(AceessTokenDefault, OpenId, out retInfo, out nickName, out headImageUrl);
            string generateId = Globals.GetGenerateId();
            MemberInfo info = new MemberInfo {
                GradeId = MemberProcessor.GetDefaultMemberGrade(),
                UserName = Globals.UrlDecode(nickName),
                OpenId = OpenId,
                CreateDate = DateTime.Now,
                SessionId = generateId,
                SessionEndTime = DateTime.Now.AddYears(10),
                UserHead = headImageUrl,
                ReferralUserId = ReferralUserId,
                Password = HiCryptographer.Md5Encrypt("888888")
            };
            Globals.Debuglog(JsonConvert.SerializeObject(info), "_DebuglogScanRegisterUserInfo.txt");
            return MemberProcessor.CreateMember(info);
        }

        public override AbstractResponse DefaultResponse(AbstractRequest requestMessage)
        {
            WeiXinHelper.UpdateRencentOpenID(requestMessage.FromUserName);
            Hidistro.Entities.VShop.ReplyInfo mismatchReply = ReplyHelper.GetMismatchReply();
            if ((mismatchReply == null) || this.IsOpenManyService())
            {
                return this.GotoManyCustomerService(requestMessage);
            }
            AbstractResponse response = this.GetResponse(mismatchReply, requestMessage.FromUserName);
            if (response == null)
            {
                return this.GotoManyCustomerService(requestMessage);
            }
            response.ToUserName = requestMessage.FromUserName;
            response.FromUserName = requestMessage.ToUserName;
            return response;
        }

        private string FormatImgUrl(string img)
        {
            if (!img.StartsWith("http"))
            {
                img = string.Format("http://{0}{1}", HttpContext.Current.Request.Url.Host, img);
            }
            return img;
        }

        private AbstractResponse GetKeyResponse(string key, AbstractRequest request)
        {
            IList<Hidistro.Entities.VShop.ReplyInfo> replies = ReplyHelper.GetReplies(ReplyType.Vote);
            if ((replies != null) && (replies.Count > 0))
            {
                foreach (Hidistro.Entities.VShop.ReplyInfo info in replies)
                {
                    if (info.Keys == key)
                    {
                        VoteInfo voteById = StoreHelper.GetVoteById((long) info.ActivityId);
                        if ((voteById != null) && voteById.IsBackup)
                        {
                            NewsResponse response = new NewsResponse {
                                CreateTime = DateTime.Now,
                                FromUserName = request.ToUserName,
                                ToUserName = request.FromUserName,
                                Articles = new List<Article>()
                            };
                            Article item = new Article {
                                Description = voteById.VoteName,
                                PicUrl = this.FormatImgUrl(voteById.ImageUrl),
                                Title = voteById.VoteName,
                                Url = string.Format("http://{0}/vshop/Vote.aspx?voteId={1}", HttpContext.Current.Request.Url.Host, voteById.VoteId)
                            };
                            response.Articles.Add(item);
                            return response;
                        }
                    }
                }
            }
            return null;
        }

        public AbstractResponse GetResponse(Hidistro.Entities.VShop.ReplyInfo reply, string openId)
        {
            Globals.Debuglog(reply.MessageType.ToString() + "||" + reply.MessageType.ToString() + "||" + reply.MessageTypeName, "_DebuglogYY.txt");
            if (reply.MessageType == Hidistro.Entities.VShop.MessageType.Text)
            {
                TextReplyInfo info = reply as TextReplyInfo;
                TextResponse response = new TextResponse {
                    CreateTime = DateTime.Now,
                    Content = Globals.FormatWXReplyContent(info.Text)
                };
                if (reply.Keys == "登录")
                {
                    string str2 = Globals.GetWebUrlStart() + "/Vshop/MemberCenter.aspx";
                    response.Content = response.Content.Replace("$login$", string.Format("<a href=\"{0}\">一键登录</a>", str2));
                }
                return response;
            }
            NewsResponse response2 = new NewsResponse {
                CreateTime = DateTime.Now,
                Articles = new List<Article>()
            };
            if (reply.ArticleID > 0)
            {
                ArticleInfo articleInfo = ArticleHelper.GetArticleInfo(reply.ArticleID);
                if (articleInfo.ArticleType == ArticleType.News)
                {
                    Article item = new Article {
                        Description = articleInfo.Memo,
                        PicUrl = this.FormatImgUrl(articleInfo.ImageUrl),
                        Title = articleInfo.Title,
                        Url = string.IsNullOrEmpty(articleInfo.Url) ? string.Format("http://{0}/Vshop/ArticleDetail.aspx?sid={1}", HttpContext.Current.Request.Url.Host, articleInfo.ArticleId) : articleInfo.Url
                    };
                    response2.Articles.Add(item);
                    return response2;
                }
                if (articleInfo.ArticleType == ArticleType.List)
                {
                    Article article3 = new Article {
                        Description = articleInfo.Memo,
                        PicUrl = this.FormatImgUrl(articleInfo.ImageUrl),
                        Title = articleInfo.Title,
                        Url = string.IsNullOrEmpty(articleInfo.Url) ? string.Format("http://{0}/Vshop/ArticleDetail.aspx?sid={1}", HttpContext.Current.Request.Url.Host, articleInfo.ArticleId) : articleInfo.Url
                    };
                    response2.Articles.Add(article3);
                    foreach (ArticleItemsInfo info3 in articleInfo.ItemsInfo)
                    {
                        article3 = new Article {
                            Description = "",
                            PicUrl = this.FormatImgUrl(info3.ImageUrl),
                            Title = info3.Title,
                            Url = string.IsNullOrEmpty(info3.Url) ? string.Format("http://{0}/Vshop/ArticleDetail.aspx?iid={1}", HttpContext.Current.Request.Url.Host, info3.Id) : info3.Url
                        };
                        response2.Articles.Add(article3);
                    }
                }
                return response2;
            }
            foreach (NewsMsgInfo info4 in (reply as NewsReplyInfo).NewsMsg)
            {
                Article article6 = new Article {
                    Description = info4.Description,
                    PicUrl = string.Format("http://{0}{1}", HttpContext.Current.Request.Url.Host, info4.PicUrl),
                    Title = info4.Title,
                    Url = string.IsNullOrEmpty(info4.Url) ? string.Format("http://{0}/Vshop/ImageTextDetails.aspx?messageId={1}", HttpContext.Current.Request.Url.Host, info4.Id) : info4.Url
                };
                response2.Articles.Add(article6);
            }
            return response2;
        }

        public AbstractResponse GotoManyCustomerService(AbstractRequest requestMessage)
        {
            WeiXinHelper.UpdateRencentOpenID(requestMessage.FromUserName);
            if (!this.IsOpenManyService())
            {
                return null;
            }
            return new AbstractResponse { FromUserName = requestMessage.ToUserName, ToUserName = requestMessage.FromUserName, MsgType = ResponseMsgType.transfer_customer_service };
        }

        public bool IsOpenManyService()
        {
            return SettingsManager.GetMasterSettings(false).OpenManyService;
        }

        public override AbstractResponse OnEvent_ClickRequest(ClickEventRequest clickEventRequest)
        {
            string userOpenId = clickEventRequest.FromUserName;
            WeiXinHelper.UpdateRencentOpenID(userOpenId);
            try
            {
                Hidistro.Entities.VShop.MenuInfo menu = VShopHelper.GetMenu(Convert.ToInt32(clickEventRequest.EventKey));
                if (menu == null)
                {
                    return null;
                }
                if (menu.BindType == BindType.StoreCard)
                {
                    try
                    {
                        SiteSettings siteSettings = SettingsManager.GetMasterSettings(false);
                        string access_token = TokenApi.GetToken(siteSettings.WeixinAppId, siteSettings.WeixinAppSecret);
                        access_token = JsonConvert.DeserializeObject<Token>(access_token).access_token;
                        MemberInfo member = MemberProcessor.GetOpenIdMember(userOpenId, "wx");
                        if (member == null)
                        {
                            this.CreatMember(userOpenId, 0, access_token);
                            member = MemberProcessor.GetOpenIdMember(userOpenId, "wx");
                        }
                        string userHead = member.UserHead;
                        string storeLogo = siteSettings.DistributorLogoPic;
                        string webStart = Globals.GetWebUrlStart();
                        string imageUrl = "/Storage/master/DistributorCards/MemberCard" + member.UserId + ".jpg";
                        string mediaid = string.Empty;
                        int ReferralId = 0;
                        string storeName = siteSettings.SiteName;
                        string NotSuccessMsg = string.Empty;
                        DistributorsInfo distributorInfo = DistributorsBrower.GetDistributorInfo(member.UserId);
                        if (distributorInfo != null)
                        {
                            ReferralId = member.UserId;
                            if (siteSettings.IsShowDistributorSelfStoreName)
                            {
                                storeName = distributorInfo.StoreName;
                                storeLogo = distributorInfo.Logo;
                            }
                            imageUrl = "/Storage/master/DistributorCards/StoreCard" + ReferralId + ".jpg";
                        }
                        else if (!siteSettings.IsShowSiteStoreCard)
                        {
                            string str = "您还不是分销商，不能为您生成推广图片，立即<a href='" + webStart + "/Vshop/DistributorCenter.aspx'>申请分销商</a>";
                            if (!string.IsNullOrEmpty(siteSettings.ToRegistDistributorTips))
                            {
                                str = Regex.Replace(siteSettings.ToRegistDistributorTips, "{{申请分销商}}", "<a href='" + webStart + "/Vshop/DistributorCenter.aspx'>申请分销商</a>");
                            }
                            return new TextResponse { CreateTime = DateTime.Now, ToUserName = userOpenId, FromUserName = clickEventRequest.ToUserName, Content = str };
                        }
                        string postData = string.Empty;
                        string creatingStoreCardTips = siteSettings.CreatingStoreCardTips;
                        if (!string.IsNullOrEmpty(creatingStoreCardTips))
                        {
                            postData = "{\"touser\":\"" + userOpenId + "\",\"msgtype\":\"text\",\"text\":{\"content\":\"" + Globals.String2Json(creatingStoreCardTips) + "\"}}";
                            NewsApi.KFSend(access_token, postData);
                        }
                        string filePath = HttpContext.Current.Request.MapPath(imageUrl);
                        Task.Factory.StartNew(delegate {
                            try
                            {
                                File.Exists(filePath);
                                string str = File.ReadAllText(HttpRuntime.AppDomainAppPath + "/Storage/Utility/StoreCardSet.js");
                                string qRImageUrlByTicket = webStart + "/Follow.aspx?ReferralId=" + ReferralId.ToString();
                                ScanInfos info = ScanHelp.GetScanInfosByUserId(ReferralId, 0, "WX");
                                if (info == null)
                                {
                                    ScanHelp.CreatNewScan(ReferralId, "WX", 0);
                                    info = ScanHelp.GetScanInfosByUserId(ReferralId, 0, "WX");
                                }
                                if ((info != null) && !string.IsNullOrEmpty(info.CodeUrl))
                                {
                                    qRImageUrlByTicket = BarCodeApi.GetQRImageUrlByTicket(info.CodeUrl);
                                }
                                else
                                {
                                    string token = TokenApi.GetToken_Message(siteSettings.WeixinAppId, siteSettings.WeixinAppSecret);
                                    if (TokenApi.CheckIsRightToken(token))
                                    {
                                        string str4 = BarCodeApi.CreateTicket(token, info.Sceneid, "QR_LIMIT_SCENE", "2592000");
                                        if (!string.IsNullOrEmpty(str4))
                                        {
                                            qRImageUrlByTicket = BarCodeApi.GetQRImageUrlByTicket(str4);
                                            info.CodeUrl = str4;
                                            info.CreateTime = DateTime.Now;
                                            info.LastActiveTime = DateTime.Now;
                                            ScanHelp.updateScanInfosCodeUrl(info);
                                        }
                                    }
                                }
                                StoreCardCreater creater = new StoreCardCreater(str, userHead, storeLogo, qRImageUrlByTicket, member.UserName, storeName, ReferralId, member.UserId);
                                if (creater.ReadJson() && creater.CreadCard(out NotSuccessMsg))
                                {
                                    if (ReferralId > 0)
                                    {
                                        DistributorsBrower.UpdateStoreCard(ReferralId, NotSuccessMsg);
                                    }
                                    string msg = NewsApi.GetMedia_IDByPath(access_token, webStart + imageUrl);
                                    mediaid = NewsApi.GetJsonValue(msg, "media_id");
                                }
                                else
                                {
                                    Globals.Debuglog(NotSuccessMsg, "_DebugCreateStoreCardlog.txt");
                                }
                                postData = "{\"touser\":\"" + userOpenId + "\",\"msgtype\":\"image\",\"image\":{\"media_id\":\"" + mediaid + "\"}}";
                                NewsApi.KFSend(access_token, postData);
                            }
                            catch (Exception exception)
                            {
                                postData = "{\"touser\":\"" + userOpenId + "\",\"msgtype\":\"text\",\"text\":{\"content\":\"生成图片失败，" + Globals.String2Json(exception.ToString()) + "\"}}";
                                NewsApi.KFSend(access_token, postData);
                            }
                        });
                        return null;
                    }
                    catch (Exception exception)
                    {
                        return new TextResponse { CreateTime = DateTime.Now, ToUserName = userOpenId, FromUserName = clickEventRequest.ToUserName, Content = "问题:" + exception.ToString() };
                    }
                }
                Hidistro.Entities.VShop.ReplyInfo reply = ReplyHelper.GetReply(menu.ReplyId);
                if (reply == null)
                {
                    return null;
                }
                if (reply.MessageType != Hidistro.Entities.VShop.MessageType.Image)
                {
                    AbstractResponse keyResponse = this.GetKeyResponse(reply.Keys, clickEventRequest);
                    if (keyResponse != null)
                    {
                        return keyResponse;
                    }
                }
                AbstractResponse response = this.GetResponse(reply, clickEventRequest.FromUserName);
                if (response == null)
                {
                    this.GotoManyCustomerService(clickEventRequest);
                }
                response.ToUserName = clickEventRequest.FromUserName;
                response.FromUserName = clickEventRequest.ToUserName;
                return response;
            }
            catch (Exception exception2)
            {
                return new TextResponse { CreateTime = DateTime.Now, ToUserName = clickEventRequest.FromUserName, FromUserName = clickEventRequest.ToUserName, Content = "问题:" + exception2.ToString() };
            }
        }

        public override AbstractResponse OnEvent_MassendJobFinishEventRequest(MassendJobFinishEventRequest massendJobFinishEventRequest)
        {
            string returnjsondata = string.Concat(new object[] { "公众号的微信号(加密的):", massendJobFinishEventRequest.ToUserName, ",发送完成时间：", massendJobFinishEventRequest.CreateTime, "，过滤通过条数：", massendJobFinishEventRequest.FilterCount, "，发送失败的粉丝数：", massendJobFinishEventRequest.ErrorCount });
            switch (massendJobFinishEventRequest.Status)
            {
                case "send success":
                    returnjsondata = returnjsondata + "(发送成功)";
                    break;

                case "send fail":
                    returnjsondata = returnjsondata + "(发送失败)";
                    break;

                case "err(10001)":
                    returnjsondata = returnjsondata + "(涉嫌广告)";
                    break;

                case "err(20001)":
                    returnjsondata = returnjsondata + "(涉嫌政治)";
                    break;

                case "err(20004)":
                    returnjsondata = returnjsondata + "(涉嫌社会)";
                    break;

                case "err(20002)":
                    returnjsondata = returnjsondata + "(涉嫌色情)";
                    break;

                case "err(20006)":
                    returnjsondata = returnjsondata + "(涉嫌违法犯罪)";
                    break;

                case "err(20008)":
                    returnjsondata = returnjsondata + "(涉嫌欺诈)";
                    break;

                case "err(20013)":
                    returnjsondata = returnjsondata + "(涉嫌版权)";
                    break;

                case "err(22000)":
                    returnjsondata = returnjsondata + "(涉嫌互相宣传)";
                    break;

                case "err(21000)":
                    returnjsondata = returnjsondata + "(涉嫌其他)";
                    break;

                default:
                    returnjsondata = returnjsondata + "(" + massendJobFinishEventRequest.Status + ")";
                    break;
            }
            WeiXinHelper.UpdateMsgId(0, massendJobFinishEventRequest.MsgId.ToString(), (massendJobFinishEventRequest.Status == "send success") ? 1 : 2, Globals.ToNum(massendJobFinishEventRequest.SentCount), Globals.ToNum(massendJobFinishEventRequest.TotalCount), returnjsondata);
            return null;
        }

        public override AbstractResponse OnEvent_ScanRequest(ScanEventRequest scanEventRequest)
        {
            string eventKey = scanEventRequest.EventKey;
            if (eventKey == "1")
            {
                if (WeiXinHelper.BindAdminOpenId.Count > 10)
                {
                    WeiXinHelper.BindAdminOpenId.Clear();
                }
                if (WeiXinHelper.BindAdminOpenId.ContainsKey(scanEventRequest.Ticket))
                {
                    WeiXinHelper.BindAdminOpenId[scanEventRequest.Ticket] = scanEventRequest.FromUserName;
                }
                else
                {
                    WeiXinHelper.BindAdminOpenId.Add(scanEventRequest.Ticket, scanEventRequest.FromUserName);
                }
                return new TextResponse { CreateTime = DateTime.Now, Content = "您正在扫描尝试绑定管理员身份，身份已识别", ToUserName = scanEventRequest.FromUserName, FromUserName = scanEventRequest.ToUserName };
            }
            ScanInfos scanInfosByTicket = ScanHelp.GetScanInfosByTicket(scanEventRequest.Ticket);
            Globals.Debuglog(eventKey + ":" + scanEventRequest.Ticket, "_Debuglog.txt");
            bool flag = MemberProcessor.IsExitOpenId(scanEventRequest.FromUserName);
            if ((!flag && (scanInfosByTicket != null)) && (scanInfosByTicket.BindUserId > 0))
            {
                this.CreatMember(scanEventRequest.FromUserName, scanInfosByTicket.BindUserId, "");
            }
            if (scanInfosByTicket != null)
            {
                ScanHelp.updateScanInfosLastActiveTime(DateTime.Now, scanInfosByTicket.Sceneid);
            }
            string str2 = "";
            DataSet set = new DataSet();
            string path = HttpContext.Current.Server.MapPath("/config/WifiConfig.xml");
            if (File.Exists(path))
            {
                set.ReadXml(path);
                if ((set != null) && (set.Tables.Count > 0))
                {
                    foreach (DataRow row in set.Tables[0].Rows)
                    {
                        if (row["id"].ToString() == eventKey)
                        {
                            str2 = row["WifiDescribe"].ToString() + "\r\nWIFI帐号：" + row["WifiName"].ToString() + "\r\n WIFI密码：" + row["WifiPwd"].ToString();
                        }
                    }
                }
            }
            if (str2 != "")
            {
                return new TextResponse { CreateTime = DateTime.Now, Content = str2, ToUserName = scanEventRequest.FromUserName, FromUserName = scanEventRequest.ToUserName };
            }
            if (flag)
            {
                return new TextResponse { CreateTime = DateTime.Now, Content = "您刚扫描了分销商公众号二维码！", ToUserName = scanEventRequest.FromUserName, FromUserName = scanEventRequest.ToUserName };
            }
            Hidistro.Entities.VShop.ReplyInfo subscribeReply = ReplyHelper.GetSubscribeReply();
            if (subscribeReply == null)
            {
                return null;
            }
            subscribeReply.Keys = "扫描";
            AbstractResponse response = this.GetResponse(subscribeReply, scanEventRequest.FromUserName);
            response.ToUserName = scanEventRequest.FromUserName;
            response.FromUserName = scanEventRequest.ToUserName;
            return response;
        }

        public override AbstractResponse OnEvent_SubscribeRequest(SubscribeEventRequest subscribeEventRequest)
        {
            string eventKey = "";
            if (subscribeEventRequest.EventKey != null)
            {
                eventKey = subscribeEventRequest.EventKey;
            }
            if (eventKey.Contains("qrscene_"))
            {
                eventKey = eventKey.Replace("qrscene_", "").Trim();
                if (eventKey == "1")
                {
                    if (WeiXinHelper.BindAdminOpenId.Count > 10)
                    {
                        WeiXinHelper.BindAdminOpenId.Clear();
                    }
                    if (WeiXinHelper.BindAdminOpenId.ContainsKey(subscribeEventRequest.Ticket))
                    {
                        WeiXinHelper.BindAdminOpenId[subscribeEventRequest.Ticket] = subscribeEventRequest.FromUserName;
                    }
                    else
                    {
                        WeiXinHelper.BindAdminOpenId.Add(subscribeEventRequest.Ticket, subscribeEventRequest.FromUserName);
                    }
                    return new TextResponse { CreateTime = DateTime.Now, Content = "您正在扫描尝试绑定管理员身份，身份已识别", ToUserName = subscribeEventRequest.FromUserName, FromUserName = subscribeEventRequest.ToUserName };
                }
                ScanInfos scanInfosByTicket = ScanHelp.GetScanInfosByTicket(subscribeEventRequest.Ticket);
                bool flag = MemberProcessor.IsExitOpenId(subscribeEventRequest.FromUserName);
                int bindUserId = scanInfosByTicket.BindUserId;
                if (bindUserId < 0)
                {
                    bindUserId = 0;
                }
                if (!flag && (scanInfosByTicket != null))
                {
                    this.CreatMember(subscribeEventRequest.FromUserName, bindUserId, "");
                    ScanHelp.updateScanInfosLastActiveTime(DateTime.Now, scanInfosByTicket.Sceneid);
                }
            }
            else
            {
                bool flag2 = MemberProcessor.IsExitOpenId(subscribeEventRequest.FromUserName);
                Globals.Debuglog("关注公众号1", "_DebuglogConcern.txt");
                int referralUserId = 0;
                if (referralUserId < 0)
                {
                    referralUserId = 0;
                }
                if (!flag2)
                {
                    Globals.Debuglog("关注公众号生成用户1", "_DebuglogConcern.txt");
                    this.CreatMember(subscribeEventRequest.FromUserName, referralUserId, "");
                }
            }
            WeiXinHelper.UpdateRencentOpenID(subscribeEventRequest.FromUserName);
            string str2 = "";
            DataSet set = new DataSet();
            string path = HttpContext.Current.Server.MapPath("/config/WifiConfig.xml");
            if (File.Exists(path))
            {
                set.ReadXml(path);
                if ((set != null) && (set.Tables.Count > 0))
                {
                    foreach (DataRow row in set.Tables[0].Rows)
                    {
                        if (row["id"].ToString() == eventKey)
                        {
                            str2 = row["WifiDescribe"].ToString() + "\r\nWIFI帐号：" + row["WifiName"].ToString() + "\r\n WIFI密码：" + row["WifiPwd"].ToString();
                        }
                    }
                }
            }
            if (str2 != "")
            {
                return new TextResponse { CreateTime = DateTime.Now, Content = str2, ToUserName = subscribeEventRequest.FromUserName, FromUserName = subscribeEventRequest.ToUserName };
            }
            Hidistro.Entities.VShop.ReplyInfo subscribeReply = ReplyHelper.GetSubscribeReply();
            if (subscribeReply == null)
            {
                return null;
            }
            subscribeReply.Keys = "登录";
            AbstractResponse response = this.GetResponse(subscribeReply, subscribeEventRequest.FromUserName);
            if (response == null)
            {
                this.GotoManyCustomerService(subscribeEventRequest);
            }
            response.ToUserName = subscribeEventRequest.FromUserName;
            response.FromUserName = subscribeEventRequest.ToUserName;
            return response;
        }

        public override AbstractResponse OnEvent_UnSubscribeRequest(UnSubscribeEventRequest unSubscribeEventRequest)
        {
            string fromUserName = unSubscribeEventRequest.FromUserName;
            Globals.Debuglog("取消关注：" + fromUserName, "_DebugUnSubscribeEventRequestlog.txt");
            MemberProcessor.UpdateUserFollowStateByOpenId(fromUserName, 0);
            return this.DefaultResponse(unSubscribeEventRequest);
        }

        public override AbstractResponse OnTextRequest(TextRequest textRequest)
        {
            WeiXinHelper.UpdateRencentOpenID(textRequest.FromUserName);
            AbstractResponse keyResponse = this.GetKeyResponse(textRequest.Content, textRequest);
            if (keyResponse != null)
            {
                return keyResponse;
            }
            IList<Hidistro.Entities.VShop.ReplyInfo> replies = ReplyHelper.GetReplies(ReplyType.Keys);
            if ((replies == null) || ((replies.Count == 0) && this.IsOpenManyService()))
            {
                this.GotoManyCustomerService(textRequest);
            }
            foreach (Hidistro.Entities.VShop.ReplyInfo info in replies)
            {
                if ((info.MatchType == MatchType.Equal) && (info.Keys == textRequest.Content))
                {
                    AbstractResponse response = this.GetResponse(info, textRequest.FromUserName);
                    response.ToUserName = textRequest.FromUserName;
                    response.FromUserName = textRequest.ToUserName;
                    return response;
                }
                if ((info.MatchType == MatchType.Like) && info.Keys.Contains(textRequest.Content))
                {
                    AbstractResponse response3 = this.GetResponse(info, textRequest.FromUserName);
                    response3.ToUserName = textRequest.FromUserName;
                    response3.FromUserName = textRequest.ToUserName;
                    return response3;
                }
            }
            return this.DefaultResponse(textRequest);
        }
    }
}

