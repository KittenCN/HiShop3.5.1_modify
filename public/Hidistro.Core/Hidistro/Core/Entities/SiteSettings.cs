namespace Hidistro.Core.Entities
{
    using Hidistro.Core;
    using System;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Xml;

    public class SiteSettings
    {
        public SiteSettings(string siteUrl)
        {
            this.SiteUrl = siteUrl;
            this.Theme = "default";
            this.VTheme = "default";
            this.Disabled = false;
            this.SiteName = "销客多";
            this.LogoUrl = "/utility/pics/logo.jpg";
            this.ShopTel = "";
            this.DefaultProductImage = "/utility/pics/none.gif";
            this.DefaultProductThumbnail1 = "/utility/pics/none.gif";
            this.DefaultProductThumbnail2 = "/utility/pics/none.gif";
            this.DefaultProductThumbnail3 = "/utility/pics/none.gif";
            this.DefaultProductThumbnail4 = "/utility/pics/none.gif";
            this.DefaultProductThumbnail5 = "/utility/pics/none.gif";
            this.DefaultProductThumbnail6 = "/utility/pics/none.gif";
            this.DefaultProductThumbnail7 = "/utility/pics/none.gif";
            this.DefaultProductThumbnail8 = "/utility/pics/none.gif";
            this.WeiXinCodeImageUrl = "/Storage/master/WeiXinCodeImageUrl.jpg";
            this.VipCardBG = "/Storage/master/Vipcard/vipbg.png";
            this.VipCardQR = "/Storage/master/Vipcard/vipqr.jpg";
            this.VipCardPrefix = "100000";
            this.VipRequireName = true;
            this.VipRequireMobile = true;
            this.EnablePodRequest = true;
            this.CustomReply = true;
            this.SubscribeReply = true;
            this.ByRemind = true;
            this.DecimalLength = 2;
            this.PointsRate = 1M;
            this.ShoppingScoreUnit = 1;
            this.ShowCopyRight = "";
            this.OrderShowDays = 7;
            this.CloseOrderDays = 3;
            this.FinishOrderDays = 7;
            this.MaxReturnedDays = 15;
            this.OpenManyService = false;
            this.BatchAliPay = false;
            this.BatchWeixinPay = false;
            this.BatchWeixinPayCheckRealName = 2;
            this.DrawPayType = "";
            this.AlipayAppid = "";
            this.AliOHFollowRelayTitle = "";
            this.IsAddCommission = 0;
            this.Main_PayKey = "";
            this.Main_Mch_ID = "";
            this.Main_AppId = "";
            this.EnableSP = false;
            this.RechargeMoneyToDistributor = 0M;
            this.EnableBalancePayment = true;
            this.EnabelBalanceWithdrawal = true;
            this.EnabeHomePageBottomLink = true;
            this.EnableHomePageBottomCopyright = true;
            this.DistributionLinkName = "申请分销";
            this.DistributionLink = "/Vshop/DistributorRegCheck.aspx";
            this.AppKey = "";
            this.CopyrightLinkName = "Hishop技术支持";
            this.CopyrightLink = "http://www.hishop.com.cn/support/";
            this.EnableMemberAutoToDistributor = false;
            this.IsDistributorBuyCanGetCommission = true;
            this.IsShowDistributorSelfStoreName = true;
            this.EnableGuidePageSet = true;
            this.IsAutoGuide = false;
            this.GuideConcernType = 0;
            this.ConcernMsg = "";
            this.IsMustConcern = false;
            this.IsHomeShowFloatMenu = true;
            this.TelReg = @"/^([0-9]{3,4}\-)?([0-9]{3,8})+(\-[0-9]{1,4})?$|(^\d{11}$)/";
            this.DistributorCenterName = "分销中心";
            this.CommissionName = "佣金";
            this.DistributionTeamName = "我的下属";
            this.MyShopName = "店铺会员";
            this.FirstShopName = "一级分店";
            this.SecondShopName = "二级分店";
            this.MyCommissionName = "我的佣金";
            this.DistributionDescriptionName = "分销说明";
            this.Exp_appKey = "";
            this.Exp_appSecret = "";
            this.Exp_apiUrl = "";
        }

        public static SiteSettings FromXml(XmlDocument doc)
        {
            XmlNode node = doc.SelectSingleNode("Settings");
            SiteSettings settings = new SiteSettings(node.SelectSingleNode("SiteUrl").InnerText) {
                Theme = node.SelectSingleNode("Theme").InnerText,
                VTheme = node.SelectSingleNode("VTheme").InnerText,
                ServiceMeiQia = node.SelectSingleNode("ServiceMeiQia").InnerText,
                MeiQiaEntId = node.SelectSingleNode("MeiQiaEntId").InnerText,
                IsShowSiteStoreCard = bool.Parse(node.SelectSingleNode("IsShowSiteStoreCard").InnerText),
                EnableBalancePayment = bool.Parse(node.SelectSingleNode("EnableBalancePayment").InnerText),
                CommissionAutoToBalance = bool.Parse(node.SelectSingleNode("CommissionAutoToBalance").InnerText),
                CreatingStoreCardTips = node.SelectSingleNode("CreatingStoreCardTips").InnerText,
                ToRegistDistributorTips = node.SelectSingleNode("ToRegistDistributorTips").InnerText,
                DecimalLength = int.Parse(node.SelectSingleNode("DecimalLength").InnerText),
                DefaultProductImage = node.SelectSingleNode("DefaultProductImage").InnerText,
                DefaultProductThumbnail1 = node.SelectSingleNode("DefaultProductThumbnail1").InnerText,
                DefaultProductThumbnail2 = node.SelectSingleNode("DefaultProductThumbnail2").InnerText,
                DefaultProductThumbnail3 = node.SelectSingleNode("DefaultProductThumbnail3").InnerText,
                DefaultProductThumbnail4 = node.SelectSingleNode("DefaultProductThumbnail4").InnerText,
                DefaultProductThumbnail5 = node.SelectSingleNode("DefaultProductThumbnail5").InnerText,
                DefaultProductThumbnail6 = node.SelectSingleNode("DefaultProductThumbnail6").InnerText,
                DefaultProductThumbnail7 = node.SelectSingleNode("DefaultProductThumbnail7").InnerText,
                DefaultProductThumbnail8 = node.SelectSingleNode("DefaultProductThumbnail8").InnerText,
                CheckCode = node.SelectSingleNode("CheckCode").InnerText,
                App_Secret = node.SelectSingleNode("App_Secret").InnerText,
                Access_Token = node.SelectSingleNode("Access_Token").InnerText,
                Disabled = bool.Parse(node.SelectSingleNode("Disabled").InnerText),
                Footer = node.SelectSingleNode("Footer").InnerText,
                RegisterAgreement = node.SelectSingleNode("RegisterAgreement").InnerText,
                LogoUrl = node.SelectSingleNode("LogoUrl").InnerText,
                ShopTel = node.SelectSingleNode("ShopTel").InnerText,
                ShowCopyRight = node.SelectSingleNode("ShowCopyRight").InnerText,
                OrderShowDays = int.Parse(node.SelectSingleNode("OrderShowDays").InnerText),
                CloseOrderDays = int.Parse(node.SelectSingleNode("CloseOrderDays").InnerText),
                FinishOrderDays = int.Parse(node.SelectSingleNode("FinishOrderDays").InnerText),
                MaxReturnedDays = int.Parse(node.SelectSingleNode("MaxReturnedDays").InnerText),
                TaxRate = decimal.Parse(node.SelectSingleNode("TaxRate").InnerText),
                RechargeMoneyToDistributor = decimal.Parse(node.SelectSingleNode("RechargeMoneyToDistributor").InnerText),
                PointsRate = decimal.Parse(node.SelectSingleNode("PointsRate").InnerText),
                ShoppingScoreUnit = Globals.ToNum(node.SelectSingleNode("ShoppingScoreUnit").InnerText),
                SiteName = node.SelectSingleNode("SiteName").InnerText,
                SiteUrl = node.SelectSingleNode("SiteUrl").InnerText,
                YourPriceName = node.SelectSingleNode("YourPriceName").InnerText,
                EmailSender = node.SelectSingleNode("EmailSender").InnerText,
                EmailSettings = node.SelectSingleNode("EmailSettings").InnerText,
                SMSSender = node.SelectSingleNode("SMSSender").InnerText,
                SMSSettings = node.SelectSingleNode("SMSSettings").InnerText,
                EnabledCnzz = bool.Parse(node.SelectSingleNode("EnabledCnzz").InnerText),
                CnzzUsername = node.SelectSingleNode("CnzzUsername").InnerText,
                CnzzPassword = node.SelectSingleNode("CnzzPassword").InnerText,
                WeixinAppId = node.SelectSingleNode("WeixinAppId").InnerText,
                WeixinAppSecret = node.SelectSingleNode("WeixinAppSecret").InnerText,
                WeixinPaySignKey = node.SelectSingleNode("WeixinPaySignKey").InnerText,
                WeixinPartnerID = node.SelectSingleNode("WeixinPartnerID").InnerText,
                WeixinPartnerKey = node.SelectSingleNode("WeixinPartnerKey").InnerText,
                IsValidationService = bool.Parse(node.SelectSingleNode("IsValidationService").InnerText),
                IsAutoToLogin = bool.Parse(node.SelectSingleNode("IsAutoToLogin").InnerText),
                WeixinToken = node.SelectSingleNode("WeixinToken").InnerText,
                WeixinNumber = node.SelectSingleNode("WeixinNumber").InnerText,
                WeixinLoginUrl = node.SelectSingleNode("WeixinLoginUrl").InnerText,
                WeiXinCodeImageUrl = node.SelectSingleNode("WeiXinCodeImageUrl").InnerText,
                VipCardLogo = node.SelectSingleNode("VipCardLogo").InnerText,
                VipCardBG = node.SelectSingleNode("VipCardBG").InnerText,
                VipCardQR = node.SelectSingleNode("VipCardQR").InnerText,
                VipCardName = node.SelectSingleNode("VipCardName").InnerText,
                VipCardPrefix = node.SelectSingleNode("VipCardPrefix").InnerText,
                VipRequireName = bool.Parse(node.SelectSingleNode("VipRequireName").InnerText),
                VipRequireMobile = bool.Parse(node.SelectSingleNode("VipRequireMobile").InnerText),
                CustomReply = bool.Parse(node.SelectSingleNode("CustomReply").InnerText),
                EnableSaleService = bool.Parse(node.SelectSingleNode("EnableSaleService").InnerText),
                ByRemind = bool.Parse(node.SelectSingleNode("ByRemind").InnerText),
                EnableShopMenu = bool.Parse(node.SelectSingleNode("EnableShopMenu").InnerText),
                ShopDefault = bool.Parse(node.SelectSingleNode("ShopDefault").InnerText),
                ActivityMenu = bool.Parse(node.SelectSingleNode("ActivityMenu").InnerText),
                DistributorsMenu = bool.Parse(node.SelectSingleNode("DistributorsMenu").InnerText),
                GoodsListMenu = bool.Parse(node.SelectSingleNode("GoodsListMenu").InnerText),
                BrandMenu = bool.Parse(node.SelectSingleNode("BrandMenu").InnerText),
                MemberDefault = bool.Parse(node.SelectSingleNode("MemberDefault").InnerText),
                GoodsType = bool.Parse(node.SelectSingleNode("GoodsType").InnerText),
                GoodsCheck = bool.Parse(node.SelectSingleNode("GoodsCheck").InnerText),
                ShopMenuStyle = node.SelectSingleNode("ShopMenuStyle").InnerText,
                SubscribeReply = bool.Parse(node.SelectSingleNode("SubscribeReply").InnerText),
                VipRequireAdress = bool.Parse(node.SelectSingleNode("VipRequireAdress").InnerText),
                VipRequireQQ = bool.Parse(node.SelectSingleNode("VipRequireQQ").InnerText),
                VipEnableCoupon = bool.Parse(node.SelectSingleNode("VipEnableCoupon").InnerText),
                VipRemark = node.SelectSingleNode("VipRemark").InnerText,
                EnablePodRequest = bool.Parse(node.SelectSingleNode("EnablePodRequest").InnerText),
                EnableCommission = bool.Parse(node.SelectSingleNode("EnableCommission").InnerText),
                EnabelBalanceWithdrawal = bool.Parse(node.SelectSingleNode("EnabelBalanceWithdrawal").InnerText),
                EnabeHomePageBottomLink = bool.Parse(node.SelectSingleNode("EnabeHomePageBottomLink").InnerText),
                EnableHomePageBottomCopyright = bool.Parse(node.SelectSingleNode("EnableHomePageBottomCopyright").InnerText),
                DistributionLinkName = node.SelectSingleNode("DistributionLinkName").InnerText,
                DistributionLink = node.SelectSingleNode("DistributionLink").InnerText,
                AppKey = node.SelectSingleNode("AppKey").InnerText,
                TelReg = node.SelectSingleNode("TelReg").InnerText,
                CopyrightLinkName = node.SelectSingleNode("CopyrightLinkName").InnerText,
                EnableMemberAutoToDistributor = bool.Parse(node.SelectSingleNode("EnableMemberAutoToDistributor").InnerText),
                IsDistributorBuyCanGetCommission = bool.Parse(node.SelectSingleNode("IsDistributorBuyCanGetCommission").InnerText),
                IsShowDistributorSelfStoreName = bool.Parse(node.SelectSingleNode("IsShowDistributorSelfStoreName").InnerText),
                IsAutoGuide = bool.Parse(node.SelectSingleNode("IsAutoGuide").InnerText),
                GuideConcernType = int.Parse(node.SelectSingleNode("GuideConcernType").InnerText),
                ConcernMsg = node.SelectSingleNode("ConcernMsg").InnerText,
                IsMustConcern = bool.Parse(node.SelectSingleNode("IsMustConcern").InnerText),
                IsHomeShowFloatMenu = bool.Parse(node.SelectSingleNode("IsHomeShowFloatMenu").InnerText),
                CopyrightLink = node.SelectSingleNode("CopyrightLink").InnerText,
                EnableAlipayRequest = bool.Parse(node.SelectSingleNode("EnableAlipayRequest").InnerText),
                EnableWeiXinRequest = bool.Parse(node.SelectSingleNode("EnableWeiXinRequest").InnerText),
                EnableOffLineRequest = bool.Parse(node.SelectSingleNode("EnableOffLineRequest").InnerText),
                EnableWapShengPay = bool.Parse(node.SelectSingleNode("EnableWapShengPay").InnerText),
                OffLinePayContent = node.SelectSingleNode("OffLinePayContent").InnerText,
                DistributorDescription = node.SelectSingleNode("DistributorDescription").InnerText,
                DistributorBackgroundPic = node.SelectSingleNode("DistributorBackgroundPic").InnerText,
                DistributorLogoPic = node.SelectSingleNode("DistributorLogoPic").InnerText,
                SaleService = node.SelectSingleNode("SaleService").InnerText,
                MentionNowMoney = node.SelectSingleNode("MentionNowMoney").InnerText,
                ShopIntroduction = node.SelectSingleNode("ShopIntroduction").InnerText,
                ApplicationDescription = node.SelectSingleNode("ApplicationDescription").InnerText,
                EnableGuidePageSet = bool.Parse(node.SelectSingleNode("EnableGuidePageSet").InnerText),
                GuidePageSet = node.SelectSingleNode("GuidePageSet").InnerText,
                EnableAliPayFuwuGuidePageSet = bool.Parse(node.SelectSingleNode("EnableAliPayFuwuGuidePageSet").InnerText),
                AliPayFuwuGuidePageSet = node.SelectSingleNode("AliPayFuwuGuidePageSet").InnerText,
                ManageOpenID = node.SelectSingleNode("ManageOpenID").InnerText,
                WeixinCertPath = node.SelectSingleNode("WeixinCertPath").InnerText,
                WeixinCertPassword = node.SelectSingleNode("WeixinCertPassword").InnerText,
                GoodsPic = node.SelectSingleNode("GoodsPic").InnerText,
                GoodsName = node.SelectSingleNode("GoodsName").InnerText,
                GoodsDescription = node.SelectSingleNode("GoodsDescription").InnerText,
                ShopHomePic = node.SelectSingleNode("ShopHomePic").InnerText,
                ShopHomeName = node.SelectSingleNode("ShopHomeName").InnerText,
                ShopHomeDescription = node.SelectSingleNode("ShopHomeDescription").InnerText,
                ShopSpreadingCodePic = node.SelectSingleNode("ShopSpreadingCodePic").InnerText,
                ShopSpreadingCodeName = node.SelectSingleNode("ShopSpreadingCodeName").InnerText,
                ShopSpreadingCodeDescription = node.SelectSingleNode("ShopSpreadingCodeDescription").InnerText,
                OpenManyService = bool.Parse(node.SelectSingleNode("OpenManyService").InnerText),
                IsRequestDistributor = bool.Parse(node.SelectSingleNode("IsRequestDistributor").InnerText),
                DistributorApplicationCondition = bool.Parse(node.SelectSingleNode("DistributorApplicationCondition").InnerText),
                EnableDistributorApplicationCondition = bool.Parse(node.SelectSingleNode("EnableDistributorApplicationCondition").InnerText),
                DistributorProducts = node.SelectSingleNode("DistributorProducts").InnerText,
                DistributorProductsDate = node.SelectSingleNode("DistributorProductsDate").InnerText,
                FinishedOrderMoney = int.Parse(node.SelectSingleNode("FinishedOrderMoney").InnerText),
                RegisterDistributorsPoints = int.Parse(node.SelectSingleNode("RegisterDistributorsPoints").InnerText),
                OrdersPoints = int.Parse(node.SelectSingleNode("OrdersPoints").InnerText),
                ChinaBank_DES = node.SelectSingleNode("ChinaBank_DES").InnerText,
                ChinaBank_Enable = bool.Parse(node.SelectSingleNode("ChinaBank_Enable").InnerText),
                ChinaBank_MD5 = node.SelectSingleNode("ChinaBank_MD5").InnerText,
                ChinaBank_mid = node.SelectSingleNode("ChinaBank_mid").InnerText,
                Alipay_Key = node.SelectSingleNode("Alipay_Key").InnerText,
                Alipay_mid = node.SelectSingleNode("Alipay_mid").InnerText,
                Alipay_mName = node.SelectSingleNode("Alipay_mName").InnerText,
                Alipay_Pid = node.SelectSingleNode("Alipay_Pid").InnerText,
                OfflinePay_BankCard_Name = node.SelectSingleNode("OfflinePay_BankCard_Name").InnerText,
                OfflinePay_BankCard_BankName = node.SelectSingleNode("OfflinePay_BankCard_BankName").InnerText,
                OfflinePay_BankCard_CardNo = node.SelectSingleNode("OfflinePay_BankCard_CardNo").InnerText,
                OfflinePay_Alipay_id = node.SelectSingleNode("OfflinePay_Alipay_id").InnerText,
                ShenPay_mid = node.SelectSingleNode("ShenPay_mid").InnerText,
                ShenPay_key = node.SelectSingleNode("ShenPay_key").InnerText,
                EnableWeixinRed = bool.Parse(node.SelectSingleNode("EnableWeixinRed").InnerText),
                MemberRoleContent = node.SelectSingleNode("MemberRoleContent").InnerText,
                sign_EverDayScore = int.Parse(node.SelectSingleNode("sign_EverDayScore").InnerText),
                sign_StraightDay = int.Parse(node.SelectSingleNode("sign_StraightDay").InnerText),
                sign_RewardScore = int.Parse(node.SelectSingleNode("sign_RewardScore").InnerText),
                sign_score_Enable = bool.Parse(node.SelectSingleNode("sign_score_Enable").InnerText),
                open_signContinuity = bool.Parse(node.SelectSingleNode("open_signContinuity").InnerText),
                shopping_reward_Enable = bool.Parse(node.SelectSingleNode("shopping_reward_Enable").InnerText),
                shopping_score_Enable = bool.Parse(node.SelectSingleNode("shopping_score_Enable").InnerText),
                shopping_Score = int.Parse(node.SelectSingleNode("shopping_Score").InnerText),
                shopping_reward_Score = int.Parse(node.SelectSingleNode("shopping_reward_Score").InnerText),
                shopping_reward_OrderValue = double.Parse(node.SelectSingleNode("shopping_reward_OrderValue").InnerText),
                share_score_Enable = bool.Parse(node.SelectSingleNode("share_score_Enable").InnerText),
                share_Score = int.Parse(node.SelectSingleNode("share_Score").InnerText),
                PonitToCash_Enable = bool.Parse(node.SelectSingleNode("PonitToCash_Enable").InnerText),
                PointToCashRate = int.Parse(node.SelectSingleNode("PointToCashRate").InnerText),
                PonitToCash_MaxAmount = decimal.Parse(node.SelectSingleNode("PonitToCash_MaxAmount").InnerText),
                BatchAliPay = bool.Parse(node.SelectSingleNode("BatchAliPay").InnerText),
                BatchWeixinPay = bool.Parse(node.SelectSingleNode("BatchWeixinPay").InnerText),
                DrawPayType = node.SelectSingleNode("DrawPayType").InnerText,
                BatchWeixinPayCheckRealName = int.Parse(node.SelectSingleNode("BatchWeixinPayCheckRealName").InnerText),
                ShareAct_Enable = bool.Parse(node.SelectSingleNode("ShareAct_Enable").InnerText),
                SignWhere = int.Parse(node.SelectSingleNode("SignWhere").InnerText),
                SignWherePoint = int.Parse(node.SelectSingleNode("SignWherePoint").InnerText),
                SignPoint = int.Parse(node.SelectSingleNode("SignPoint").InnerText),
                ActiveDay = int.Parse(node.SelectSingleNode("ActiveDay").InnerText),
                AlipayAppid = node.SelectSingleNode("AlipayAppid").InnerText,
                AliOHFollowRelayTitle = node.SelectSingleNode("AliOHFollowRelayTitle").InnerText,
                IsAddCommission = (node.SelectSingleNode("IsAddCommission").InnerText == "1") ? 1 : 0,
                AddCommissionStartTime = node.SelectSingleNode("AddCommissionStartTime").InnerText,
                AddCommissionEndTime = node.SelectSingleNode("AddCommissionEndTime").InnerText,
                IsRegisterSendCoupon = bool.Parse(node.SelectSingleNode("IsRegisterSendCoupon").InnerText),
                RegisterSendCouponId = int.Parse(node.SelectSingleNode("RegisterSendCouponId").InnerText),
                Main_PayKey = node.SelectSingleNode("Main_PayKey ").InnerText,
                Main_Mch_ID = node.SelectSingleNode("Main_Mch_ID ").InnerText,
                Main_AppId = node.SelectSingleNode("Main_AppId ").InnerText,
                EnableSP = bool.Parse(node.SelectSingleNode("EnableSP ").InnerText),
                DistributorCenterName = node.SelectSingleNode("DistributorCenterName ").InnerText,
                CommissionName = node.SelectSingleNode("CommissionName ").InnerText,
                DistributionTeamName = node.SelectSingleNode("DistributionTeamName ").InnerText,
                MyShopName = node.SelectSingleNode("MyShopName ").InnerText,
                FirstShopName = node.SelectSingleNode("FirstShopName ").InnerText,
                SecondShopName = node.SelectSingleNode("SecondShopName ").InnerText,
                MyCommissionName = node.SelectSingleNode("MyCommissionName ").InnerText,
                DistributionDescriptionName = node.SelectSingleNode("DistributionDescriptionName ").InnerText,
                Exp_appKey = node.SelectSingleNode("Exp_appKey ").InnerText,
                Exp_appSecret = node.SelectSingleNode("Exp_appSecret ").InnerText,
                Exp_apiUrl = node.SelectSingleNode("Exp_apiUrl ").InnerText
            };
            string innerText = node.SelectSingleNode("RegisterSendCouponBeginTime").InnerText;
            if (!string.IsNullOrWhiteSpace(innerText))
            {
                settings.RegisterSendCouponBeginTime = new DateTime?(DateTime.Parse(innerText));
            }
            string str2 = node.SelectSingleNode("RegisterSendCouponEndTime").InnerText;
            if (!string.IsNullOrWhiteSpace(str2))
            {
                settings.RegisterSendCouponEndTime = new DateTime?(DateTime.Parse(str2));
            }
            return settings;
        }

        private static void SetNodeValue(XmlDocument doc, XmlNode root, string nodeName, string nodeValue)
        {
            XmlNode newChild = root.SelectSingleNode(nodeName);
            if (newChild == null)
            {
                newChild = doc.CreateElement(nodeName);
                root.AppendChild(newChild);
            }
            newChild.InnerText = nodeValue;
        }

        public void WriteToXml(XmlDocument doc)
        {
            XmlNode root = doc.SelectSingleNode("Settings");
            SetNodeValue(doc, root, "SiteUrl", this.SiteUrl);
            SetNodeValue(doc, root, "Theme", this.Theme);
            SetNodeValue(doc, root, "VTheme", this.VTheme);
            SetNodeValue(doc, root, "ServiceMeiQia", this.ServiceMeiQia);
            SetNodeValue(doc, root, "MeiQiaEntId", this.MeiQiaEntId);
            SetNodeValue(doc, root, "IsShowSiteStoreCard", this.IsShowSiteStoreCard ? "true" : "false");
            SetNodeValue(doc, root, "EnableBalancePayment", this.EnableBalancePayment ? "true" : "false");
            SetNodeValue(doc, root, "CommissionAutoToBalance", this.CommissionAutoToBalance ? "true" : "false");
            SetNodeValue(doc, root, "CreatingStoreCardTips", this.CreatingStoreCardTips);
            SetNodeValue(doc, root, "ToRegistDistributorTips", this.ToRegistDistributorTips);
            SetNodeValue(doc, root, "DecimalLength", this.DecimalLength.ToString(CultureInfo.InvariantCulture));
            SetNodeValue(doc, root, "DefaultProductImage", this.DefaultProductImage);
            SetNodeValue(doc, root, "DefaultProductThumbnail1", this.DefaultProductThumbnail1);
            SetNodeValue(doc, root, "DefaultProductThumbnail2", this.DefaultProductThumbnail2);
            SetNodeValue(doc, root, "DefaultProductThumbnail3", this.DefaultProductThumbnail3);
            SetNodeValue(doc, root, "DefaultProductThumbnail4", this.DefaultProductThumbnail4);
            SetNodeValue(doc, root, "DefaultProductThumbnail5", this.DefaultProductThumbnail5);
            SetNodeValue(doc, root, "DefaultProductThumbnail6", this.DefaultProductThumbnail6);
            SetNodeValue(doc, root, "DefaultProductThumbnail7", this.DefaultProductThumbnail7);
            SetNodeValue(doc, root, "DefaultProductThumbnail8", this.DefaultProductThumbnail8);
            SetNodeValue(doc, root, "App_Secret", this.App_Secret);
            SetNodeValue(doc, root, "CheckCode", this.CheckCode);
            SetNodeValue(doc, root, "Access_Token", this.Access_Token);
            SetNodeValue(doc, root, "Disabled", this.Disabled ? "true" : "false");
            SetNodeValue(doc, root, "Footer", this.Footer);
            SetNodeValue(doc, root, "RegisterAgreement", this.RegisterAgreement);
            SetNodeValue(doc, root, "ShopTel", this.ShopTel);
            SetNodeValue(doc, root, "LogoUrl", this.LogoUrl);
            SetNodeValue(doc, root, "OrderShowDays", this.OrderShowDays.ToString(CultureInfo.InvariantCulture));
            SetNodeValue(doc, root, "ShowCopyRight", this.ShowCopyRight);
            SetNodeValue(doc, root, "CloseOrderDays", this.CloseOrderDays.ToString(CultureInfo.InvariantCulture));
            SetNodeValue(doc, root, "FinishOrderDays", this.FinishOrderDays.ToString(CultureInfo.InvariantCulture));
            SetNodeValue(doc, root, "MaxReturnedDays", this.MaxReturnedDays.ToString(CultureInfo.InvariantCulture));
            SetNodeValue(doc, root, "TaxRate", this.TaxRate.ToString(CultureInfo.InvariantCulture));
            SetNodeValue(doc, root, "RechargeMoneyToDistributor", this.RechargeMoneyToDistributor.ToString(CultureInfo.InvariantCulture));
            SetNodeValue(doc, root, "PointsRate", this.PointsRate.ToString("F"));
            SetNodeValue(doc, root, "ShoppingScoreUnit", this.ShoppingScoreUnit.ToString());
            SetNodeValue(doc, root, "SiteName", this.SiteName);
            SetNodeValue(doc, root, "YourPriceName", this.YourPriceName);
            SetNodeValue(doc, root, "EmailSender", this.EmailSender);
            SetNodeValue(doc, root, "EmailSettings", this.EmailSettings);
            SetNodeValue(doc, root, "SMSSender", this.SMSSender);
            SetNodeValue(doc, root, "SMSSettings", this.SMSSettings);
            SetNodeValue(doc, root, "EnabledCnzz", this.EnabledCnzz ? "true" : "false");
            SetNodeValue(doc, root, "CnzzUsername", this.CnzzUsername);
            SetNodeValue(doc, root, "CnzzPassword", this.CnzzPassword);
            SetNodeValue(doc, root, "WeixinAppId", this.WeixinAppId);
            SetNodeValue(doc, root, "WeixinAppSecret", this.WeixinAppSecret);
            SetNodeValue(doc, root, "WeixinPaySignKey", this.WeixinPaySignKey);
            SetNodeValue(doc, root, "WeixinPartnerID", this.WeixinPartnerID);
            SetNodeValue(doc, root, "WeixinPartnerKey", this.WeixinPartnerKey);
            SetNodeValue(doc, root, "IsValidationService", this.IsValidationService ? "true" : "false");
            SetNodeValue(doc, root, "IsAutoToLogin", this.IsAutoToLogin ? "true" : "false");
            SetNodeValue(doc, root, "WeixinToken", this.WeixinToken);
            SetNodeValue(doc, root, "WeixinNumber", this.WeixinNumber);
            SetNodeValue(doc, root, "WeixinLoginUrl", this.WeixinLoginUrl);
            SetNodeValue(doc, root, "WeiXinCodeImageUrl", this.WeiXinCodeImageUrl);
            SetNodeValue(doc, root, "VipCardBG", this.VipCardBG);
            SetNodeValue(doc, root, "VipCardLogo", this.VipCardLogo);
            SetNodeValue(doc, root, "VipCardQR", this.VipCardQR);
            SetNodeValue(doc, root, "VipCardPrefix", this.VipCardPrefix);
            SetNodeValue(doc, root, "VipCardName", this.VipCardName);
            SetNodeValue(doc, root, "VipRequireName", this.VipRequireName ? "true" : "false");
            SetNodeValue(doc, root, "VipRequireMobile", this.VipRequireMobile ? "true" : "false");
            SetNodeValue(doc, root, "CustomReply", this.CustomReply ? "true" : "false");
            SetNodeValue(doc, root, "EnableSaleService", this.EnableSaleService ? "true" : "false");
            SetNodeValue(doc, root, "ByRemind", this.ByRemind ? "true" : "false");
            SetNodeValue(doc, root, "ShopMenuStyle", this.ShopMenuStyle);
            SetNodeValue(doc, root, "EnableShopMenu", this.EnableShopMenu ? "true" : "false");
            SetNodeValue(doc, root, "ShopDefault", this.ShopDefault ? "true" : "false");
            SetNodeValue(doc, root, "MemberDefault", this.MemberDefault ? "true" : "false");
            SetNodeValue(doc, root, "GoodsType", this.GoodsType ? "true" : "false");
            SetNodeValue(doc, root, "GoodsCheck", this.GoodsCheck ? "true" : "false");
            SetNodeValue(doc, root, "ActivityMenu", this.ActivityMenu ? "true" : "false");
            SetNodeValue(doc, root, "DistributorsMenu", this.DistributorsMenu ? "true" : "false");
            SetNodeValue(doc, root, "GoodsListMenu", this.GoodsListMenu ? "true" : "false");
            SetNodeValue(doc, root, "BrandMenu", this.BrandMenu ? "true" : "false");
            SetNodeValue(doc, root, "SubscribeReply", this.SubscribeReply ? "true" : "false");
            SetNodeValue(doc, root, "VipRequireQQ", this.VipRequireQQ ? "true" : "false");
            SetNodeValue(doc, root, "VipRequireAdress", this.VipRequireAdress ? "true" : "false");
            SetNodeValue(doc, root, "VipEnableCoupon", this.VipEnableCoupon ? "true" : "false");
            SetNodeValue(doc, root, "VipRemark", this.VipRemark);
            SetNodeValue(doc, root, "EnablePodRequest", this.EnablePodRequest ? "true" : "false");
            SetNodeValue(doc, root, "EnableCommission", this.EnableCommission ? "true" : "false");
            SetNodeValue(doc, root, "EnabelBalanceWithdrawal", this.EnabelBalanceWithdrawal ? "true" : "false");
            SetNodeValue(doc, root, "EnabeHomePageBottomLink", this.EnabeHomePageBottomLink ? "true" : "false");
            SetNodeValue(doc, root, "EnableHomePageBottomCopyright", this.EnableHomePageBottomCopyright ? "true" : "false");
            SetNodeValue(doc, root, "DistributionLinkName", this.DistributionLinkName);
            SetNodeValue(doc, root, "DistributionLink", this.DistributionLink);
            SetNodeValue(doc, root, "AppKey", this.AppKey);
            SetNodeValue(doc, root, "TelReg", this.TelReg);
            SetNodeValue(doc, root, "CopyrightLinkName", this.CopyrightLinkName);
            SetNodeValue(doc, root, "EnableMemberAutoToDistributor", this.EnableMemberAutoToDistributor ? "true" : "false");
            SetNodeValue(doc, root, "IsDistributorBuyCanGetCommission", this.IsDistributorBuyCanGetCommission ? "true" : "false");
            SetNodeValue(doc, root, "IsShowDistributorSelfStoreName", this.IsShowDistributorSelfStoreName ? "true" : "false");
            SetNodeValue(doc, root, "IsAutoGuide", this.IsAutoGuide ? "true" : "false");
            SetNodeValue(doc, root, "GuideConcernType", this.GuideConcernType.ToString(CultureInfo.InvariantCulture));
            SetNodeValue(doc, root, "ConcernMsg", this.ConcernMsg);
            SetNodeValue(doc, root, "IsMustConcern", this.IsMustConcern ? "true" : "false");
            SetNodeValue(doc, root, "IsHomeShowFloatMenu", this.IsHomeShowFloatMenu ? "true" : "false");
            SetNodeValue(doc, root, "CopyrightLink", this.CopyrightLink);
            SetNodeValue(doc, root, "EnableAlipayRequest", this.EnableAlipayRequest ? "true" : "false");
            SetNodeValue(doc, root, "EnableWeiXinRequest", this.EnableWeiXinRequest ? "true" : "false");
            SetNodeValue(doc, root, "EnableOffLineRequest", this.EnableOffLineRequest ? "true" : "false");
            SetNodeValue(doc, root, "EnableWapShengPay", this.EnableWapShengPay ? "true" : "false");
            SetNodeValue(doc, root, "OffLinePayContent", this.OffLinePayContent);
            SetNodeValue(doc, root, "DistributorDescription", this.DistributorDescription);
            SetNodeValue(doc, root, "DistributorBackgroundPic", this.DistributorBackgroundPic);
            SetNodeValue(doc, root, "DistributorLogoPic", this.DistributorLogoPic);
            SetNodeValue(doc, root, "SaleService", this.SaleService);
            SetNodeValue(doc, root, "MentionNowMoney", this.MentionNowMoney);
            SetNodeValue(doc, root, "ShopIntroduction", this.ShopIntroduction);
            SetNodeValue(doc, root, "ApplicationDescription", this.ApplicationDescription);
            SetNodeValue(doc, root, "AliPayFuwuGuidePageSet", this.AliPayFuwuGuidePageSet);
            SetNodeValue(doc, root, "EnableAliPayFuwuGuidePageSet", this.EnableAliPayFuwuGuidePageSet ? "true" : "false");
            SetNodeValue(doc, root, "GuidePageSet", this.GuidePageSet);
            SetNodeValue(doc, root, "EnableGuidePageSet", this.EnableGuidePageSet ? "true" : "false");
            SetNodeValue(doc, root, "ManageOpenID", this.ManageOpenID);
            SetNodeValue(doc, root, "WeixinCertPath", this.WeixinCertPath);
            SetNodeValue(doc, root, "WeixinCertPassword", this.WeixinCertPassword);
            SetNodeValue(doc, root, "GoodsPic", this.GoodsPic);
            SetNodeValue(doc, root, "GoodsName", this.GoodsName);
            SetNodeValue(doc, root, "GoodsDescription", this.GoodsDescription);
            SetNodeValue(doc, root, "ShopHomePic", this.ShopHomePic);
            SetNodeValue(doc, root, "ShopHomeName", this.ShopHomeName);
            SetNodeValue(doc, root, "ShopHomeDescription", this.ShopHomeDescription);
            SetNodeValue(doc, root, "ShopSpreadingCodePic", this.ShopSpreadingCodePic);
            SetNodeValue(doc, root, "ShopSpreadingCodeName", this.ShopSpreadingCodeName);
            SetNodeValue(doc, root, "ShopSpreadingCodeDescription", this.ShopSpreadingCodeDescription);
            SetNodeValue(doc, root, "OpenManyService", this.OpenManyService ? "true" : "false");
            SetNodeValue(doc, root, "IsRequestDistributor", this.IsRequestDistributor ? "true" : "false");
            SetNodeValue(doc, root, "FinishedOrderMoney", this.FinishedOrderMoney.ToString());
            SetNodeValue(doc, root, "DistributorApplicationCondition", this.DistributorApplicationCondition ? "true" : "false");
            SetNodeValue(doc, root, "EnableDistributorApplicationCondition", this.EnableDistributorApplicationCondition ? "true" : "false");
            SetNodeValue(doc, root, "DistributorProducts", this.DistributorProducts);
            SetNodeValue(doc, root, "DistributorProductsDate", this.DistributorProductsDate);
            SetNodeValue(doc, root, "RegisterDistributorsPoints", this.RegisterDistributorsPoints.ToString());
            SetNodeValue(doc, root, "OrdersPoints", this.OrdersPoints.ToString());
            SetNodeValue(doc, root, "ChinaBank_Enable", this.ChinaBank_Enable ? "true" : "false");
            SetNodeValue(doc, root, "ChinaBank_DES", this.ChinaBank_DES);
            SetNodeValue(doc, root, "ChinaBank_MD5", this.ChinaBank_MD5);
            SetNodeValue(doc, root, "ChinaBank_mid", this.ChinaBank_mid);
            SetNodeValue(doc, root, "Alipay_Key", this.Alipay_Key);
            SetNodeValue(doc, root, "Alipay_mid", this.Alipay_mid);
            SetNodeValue(doc, root, "Alipay_mName", this.Alipay_mName);
            SetNodeValue(doc, root, "Alipay_Pid", this.Alipay_Pid);
            SetNodeValue(doc, root, "OfflinePay_Alipay_id", this.OfflinePay_Alipay_id);
            SetNodeValue(doc, root, "OfflinePay_BankCard_Name", this.OfflinePay_BankCard_Name);
            SetNodeValue(doc, root, "OfflinePay_BankCard_BankName", this.OfflinePay_BankCard_BankName);
            SetNodeValue(doc, root, "OfflinePay_BankCard_CardNo", this.OfflinePay_BankCard_CardNo);
            SetNodeValue(doc, root, "ShenPay_mid", this.ShenPay_mid);
            SetNodeValue(doc, root, "ShenPay_key", this.ShenPay_key);
            SetNodeValue(doc, root, "EnableWeixinRed", this.EnableWeixinRed ? "true" : "false");
            SetNodeValue(doc, root, "MemberRoleContent", this.MemberRoleContent);
            SetNodeValue(doc, root, "sign_EverDayScore", this.sign_EverDayScore.ToString());
            SetNodeValue(doc, root, "sign_StraightDay", this.sign_StraightDay.ToString());
            SetNodeValue(doc, root, "sign_RewardScore", this.sign_RewardScore.ToString());
            SetNodeValue(doc, root, "sign_score_Enable", this.sign_score_Enable ? "true" : "false");
            SetNodeValue(doc, root, "open_signContinuity", this.open_signContinuity ? "true" : "false");
            SetNodeValue(doc, root, "shopping_score_Enable", this.shopping_score_Enable ? "true" : "false");
            SetNodeValue(doc, root, "shopping_reward_Enable", this.shopping_reward_Enable ? "true" : "false");
            SetNodeValue(doc, root, "shopping_Score", this.shopping_Score.ToString());
            SetNodeValue(doc, root, "shopping_reward_OrderValue", this.shopping_reward_OrderValue.ToString("F2"));
            SetNodeValue(doc, root, "shopping_reward_Score", this.shopping_reward_Score.ToString());
            SetNodeValue(doc, root, "share_score_Enable", this.share_score_Enable ? "true" : "false");
            SetNodeValue(doc, root, "share_Score", this.share_Score.ToString());
            SetNodeValue(doc, root, "PonitToCash_Enable", this.PonitToCash_Enable ? "true" : "false");
            SetNodeValue(doc, root, "PointToCashRate", this.PointToCashRate.ToString());
            SetNodeValue(doc, root, "PonitToCash_MaxAmount", this.PonitToCash_MaxAmount.ToString("F2"));
            SetNodeValue(doc, root, "DrawPayType", this.DrawPayType.ToString());
            SetNodeValue(doc, root, "BatchAliPay", this.BatchAliPay ? "true" : "false");
            SetNodeValue(doc, root, "BatchWeixinPay", this.BatchWeixinPay ? "true" : "false");
            SetNodeValue(doc, root, "BatchWeixinPayCheckRealName", this.BatchWeixinPayCheckRealName.ToString());
            SetNodeValue(doc, root, "ShareAct_Enable", this.ShareAct_Enable ? "true" : "false");
            SetNodeValue(doc, root, "SignWhere", this.SignWhere.ToString());
            SetNodeValue(doc, root, "SignWherePoint", this.SignWherePoint.ToString());
            SetNodeValue(doc, root, "SignPoint", this.SignPoint.ToString());
            SetNodeValue(doc, root, "ActiveDay", this.ActiveDay.ToString());
            SetNodeValue(doc, root, "AlipayAppid", this.AlipayAppid.ToString());
            SetNodeValue(doc, root, "AliOHFollowRelayTitle", this.AliOHFollowRelayTitle.ToString());
            SetNodeValue(doc, root, "IsAddCommission", this.IsAddCommission.ToString());
            SetNodeValue(doc, root, "AddCommissionStartTime", this.AddCommissionStartTime);
            SetNodeValue(doc, root, "AddCommissionEndTime", this.AddCommissionEndTime);
            SetNodeValue(doc, root, "IsRegisterSendCoupon", this.IsRegisterSendCoupon ? "true" : "false");
            SetNodeValue(doc, root, "RegisterSendCouponId", this.RegisterSendCouponId.ToString());
            SetNodeValue(doc, root, "RegisterSendCouponBeginTime", this.RegisterSendCouponBeginTime.HasValue ? this.RegisterSendCouponBeginTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "");
            SetNodeValue(doc, root, "RegisterSendCouponEndTime", this.RegisterSendCouponEndTime.HasValue ? this.RegisterSendCouponEndTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "");
            SetNodeValue(doc, root, "EnableSP", this.EnableSP ? "true" : "false");
            SetNodeValue(doc, root, "Main_Mch_ID", this.Main_Mch_ID);
            SetNodeValue(doc, root, "Main_PayKey", this.Main_PayKey);
            SetNodeValue(doc, root, "Main_AppId", this.Main_AppId);
            SetNodeValue(doc, root, "DistributorCenterName", this.DistributorCenterName);
            SetNodeValue(doc, root, "CommissionName", this.CommissionName);
            SetNodeValue(doc, root, "DistributionTeamName", this.DistributionTeamName);
            SetNodeValue(doc, root, "MyShopName", this.MyShopName);
            SetNodeValue(doc, root, "FirstShopName", this.FirstShopName);
            SetNodeValue(doc, root, "SecondShopName", this.SecondShopName);
            SetNodeValue(doc, root, "MyCommissionName", this.MyCommissionName);
            SetNodeValue(doc, root, "DistributionDescriptionName", this.DistributionDescriptionName);
            SetNodeValue(doc, root, "Exp_appKey", this.Exp_appKey);
            SetNodeValue(doc, root, "Exp_appSecret", this.Exp_appSecret);
            SetNodeValue(doc, root, "Exp_apiUrl", this.Exp_apiUrl);
        }

        public string Access_Token { get; set; }

        public int ActiveDay { get; set; }

        public bool ActivityMenu { get; set; }

        public string AddCommissionEndTime { get; set; }

        public string AddCommissionStartTime { get; set; }

        public string AliOHFollowRelayTitle { get; set; }

        public string Alipay_Key { get; set; }

        public string Alipay_mid { get; set; }

        public string Alipay_mName { get; set; }

        public string Alipay_Pid { get; set; }

        public string AlipayAppid { get; set; }

        public string AliPayFuwuGuidePageSet { get; set; }

        public string App_Secret { get; set; }

        public string AppKey { get; set; }

        public string ApplicationDescription { get; set; }

        public bool BatchAliPay { get; set; }

        public bool BatchWeixinPay { get; set; }

        public int BatchWeixinPayCheckRealName { get; set; }

        public bool BrandMenu { get; set; }

        public bool ByRemind { get; set; }

        public string CheckCode { get; set; }

        public string ChinaBank_DES { get; set; }

        public bool ChinaBank_Enable { get; set; }

        public string ChinaBank_MD5 { get; set; }

        public string ChinaBank_mid { get; set; }

        public int CloseOrderDays { get; set; }

        public string CnzzPassword { get; set; }

        public string CnzzUsername { get; set; }

        public bool CommissionAutoToBalance { get; set; }

        public string CommissionName { get; set; }

        public string ConcernMsg { get; set; }

        public string CopyrightLink { get; set; }

        public string CopyrightLinkName { get; set; }

        public string CreatingStoreCardTips { get; set; }

        public bool CustomReply { get; set; }

        public int DecimalLength { get; set; }

        public string DefaultProductImage { get; set; }

        public string DefaultProductThumbnail1 { get; set; }

        public string DefaultProductThumbnail2 { get; set; }

        public string DefaultProductThumbnail3 { get; set; }

        public string DefaultProductThumbnail4 { get; set; }

        public string DefaultProductThumbnail5 { get; set; }

        public string DefaultProductThumbnail6 { get; set; }

        public string DefaultProductThumbnail7 { get; set; }

        public string DefaultProductThumbnail8 { get; set; }

        public bool Disabled { get; set; }

        public string DistributionDescriptionName { get; set; }

        public string DistributionLink { get; set; }

        public string DistributionLinkName { get; set; }

        public string DistributionTeamName { get; set; }

        public bool DistributorApplicationCondition { get; set; }

        public string DistributorBackgroundPic { get; set; }

        public string DistributorCenterName { get; set; }

        public string DistributorDescription { get; set; }

        public string DistributorLogoPic { get; set; }

        public string DistributorProducts { get; set; }

        public string DistributorProductsDate { get; set; }

        public bool DistributorsMenu { get; set; }

        public string DrawPayType { get; set; }

        public bool EmailEnabled
        {
            get
            {
                return (((!string.IsNullOrEmpty(this.EmailSender) && !string.IsNullOrEmpty(this.EmailSettings)) && (this.EmailSender.Trim().Length > 0)) && (this.EmailSettings.Trim().Length > 0));
            }
        }

        public string EmailSender { get; set; }

        public string EmailSettings { get; set; }

        public bool EnabeHomePageBottomLink { get; set; }

        public bool EnabelBalanceWithdrawal { get; set; }

        public bool EnableAliPayFuwuGuidePageSet { get; set; }

        public bool EnableAlipayRequest { get; set; }

        public bool EnableBalancePayment { get; set; }

        public bool EnableCommission { get; set; }

        public bool EnabledCnzz { get; set; }

        public bool EnableDistributorApplicationCondition { get; set; }

        public bool EnableGuidePageSet { get; set; }

        public bool EnableHomePageBottomCopyright { get; set; }

        public bool EnableMemberAutoToDistributor { get; set; }

        public bool EnableOffLineRequest { get; set; }

        public bool EnablePodRequest { get; set; }

        public bool EnableSaleService { get; set; }

        public bool EnableShopMenu { get; set; }

        public bool EnableSP { get; set; }

        public bool EnableWapShengPay { get; set; }

        public bool EnableWeixinRed { get; set; }

        public bool EnableWeiXinRequest { get; set; }

        public string Exp_apiUrl { get; set; }

        public string Exp_appKey { get; set; }

        public string Exp_appSecret { get; set; }

        public int FinishedOrderMoney { get; set; }

        public int FinishOrderDays { get; set; }

        public string FirstShopName { get; set; }

        public string Footer { get; set; }

        public bool GoodsCheck { get; set; }

        public string GoodsDescription { get; set; }

        public bool GoodsListMenu { get; set; }

        public string GoodsName { get; set; }

        public string GoodsPic { get; set; }

        public bool GoodsType { get; set; }

        public int GuideConcernType { get; set; }

        public string GuidePageSet { get; set; }

        public int IsAddCommission { get; set; }

        public bool IsAutoGuide { get; set; }

        public bool IsAutoToLogin { get; set; }

        public bool IsDistributorBuyCanGetCommission { get; set; }

        public bool IsHomeShowFloatMenu { get; set; }

        public bool IsMustConcern { get; set; }

        public bool IsRegisterSendCoupon { get; set; }

        public bool IsRequestDistributor { get; set; }

        public bool IsShowDistributorSelfStoreName { get; set; }

        public bool IsShowSiteStoreCard { get; set; }

        public bool IsValidationService { get; set; }

        public string LogoUrl { get; set; }

        public string Main_AppId { get; set; }

        public string Main_Mch_ID { get; set; }

        public string Main_PayKey { get; set; }

        public string ManageOpenID { get; set; }

        public int MaxReturnedDays { get; set; }

        public string MeiQiaEntId { get; set; }

        public bool MemberDefault { get; set; }

        public string MemberRoleContent { get; set; }

        public string MentionNowMoney { get; set; }

        public string MyCommissionName { get; set; }

        public string MyShopName { get; set; }

        public string OfflinePay_Alipay_id { get; set; }

        public string OfflinePay_BankCard_BankName { get; set; }

        public string OfflinePay_BankCard_CardNo { get; set; }

        public string OfflinePay_BankCard_Name { get; set; }

        public string OffLinePayContent { get; set; }

        public bool open_signContinuity { get; set; }

        public bool OpenManyService { get; set; }

        public int OrderShowDays { get; set; }

        public int OrdersPoints { get; set; }

        public decimal PointsRate { get; set; }

        public int PointToCashRate { get; set; }

        public bool PonitToCash_Enable { get; set; }

        public decimal PonitToCash_MaxAmount { get; set; }

        public decimal RechargeMoneyToDistributor { get; set; }

        public string RegisterAgreement { get; set; }

        public int RegisterDistributorsPoints { get; set; }

        public DateTime? RegisterSendCouponBeginTime { get; set; }

        public DateTime? RegisterSendCouponEndTime { get; set; }

        public int RegisterSendCouponId { get; set; }

        public string SaleService { get; set; }

        public string SecondShopName { get; set; }

        public string ServiceMeiQia { get; set; }

        public int share_Score { get; set; }

        public bool share_score_Enable { get; set; }

        public bool ShareAct_Enable { get; set; }

        public string ShenPay_key { get; set; }

        public string ShenPay_mid { get; set; }

        public bool ShopDefault { get; set; }

        public string ShopHomeDescription { get; set; }

        public string ShopHomeName { get; set; }

        public string ShopHomePic { get; set; }

        public string ShopIntroduction { get; set; }

        public string ShopMenuStyle { get; set; }

        public bool shopping_reward_Enable { get; set; }

        public double shopping_reward_OrderValue { get; set; }

        public int shopping_reward_Score { get; set; }

        public int shopping_Score { get; set; }

        public bool shopping_score_Enable { get; set; }

        public int ShoppingScoreUnit { get; set; }

        public string ShopSpreadingCodeDescription { get; set; }

        public string ShopSpreadingCodeName { get; set; }

        public string ShopSpreadingCodePic { get; set; }

        public string ShopTel { get; set; }

        public string ShowCopyRight { get; set; }

        public int sign_EverDayScore { get; set; }

        public int sign_RewardScore { get; set; }

        public bool sign_score_Enable { get; set; }

        public int sign_StraightDay { get; set; }

        public int SignPoint { get; set; }

        public int SignWhere { get; set; }

        public int SignWherePoint { get; set; }

        public string SiteName { get; set; }

        public string SiteUrl { get; set; }

        public bool SMSEnabled
        {
            get
            {
                return (((!string.IsNullOrEmpty(this.SMSSender) && !string.IsNullOrEmpty(this.SMSSettings)) && (this.SMSSender.Trim().Length > 0)) && (this.SMSSettings.Trim().Length > 0));
            }
        }

        public string SMSSender { get; set; }

        public string SMSSettings { get; set; }

        public bool SubscribeReply { get; set; }

        public decimal TaxRate { get; set; }

        public string TelReg { get; set; }

        public string Theme { get; set; }

        public string ToRegistDistributorTips { get; set; }

        public string VipCardBG { get; set; }

        public string VipCardLogo { get; set; }

        public string VipCardName { get; set; }

        public string VipCardPrefix { get; set; }

        public string VipCardQR { get; set; }

        public bool VipEnableCoupon { get; set; }

        public string VipRemark { get; set; }

        public bool VipRequireAdress { get; set; }

        public bool VipRequireMobile { get; set; }

        public bool VipRequireName { get; set; }

        public bool VipRequireQQ { get; set; }

        public string VTheme { get; set; }

        public string WeixinAppId { get; set; }

        public string WeixinAppSecret { get; set; }

        public string WeixinCertPassword { get; set; }

        public string WeixinCertPath { get; set; }

        public string WeiXinCodeImageUrl { get; set; }

        public string WeixinLoginUrl { get; set; }

        public string WeixinNumber { get; set; }

        public string WeixinPartnerID { get; set; }

        public string WeixinPartnerKey { get; set; }

        public string WeixinPaySignKey { get; set; }

        public string WeixinToken { get; set; }

        public string YourPriceName { get; set; }
    }
}

