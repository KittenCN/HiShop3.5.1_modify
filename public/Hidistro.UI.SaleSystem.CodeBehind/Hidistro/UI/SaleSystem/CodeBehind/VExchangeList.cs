namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using ControlPanel.Promotions;
    using global::ControlPanel.Promotions;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Promotions;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using System;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    [ParseChildren(true)]
    public class VExchangeList : VshopTemplatedWebControl
    {
        private HtmlInputHidden hiddisInRange;
        private HtmlInputHidden hiddUserId;
        private HtmlInputHidden hideDesc;
        private HtmlInputHidden hideImgUrl;
        private HtmlInputHidden hideTitle;
        private int id;
        private Image imgCover;
        private Literal litPoints;

        protected override void AttachChildControls()
        {
            if (!int.TryParse(this.Page.Request.QueryString["id"], out this.id))
            {
                base.GotoResourceNotFound("");
            }
            PointExChangeInfo info = PointExChangeHelper.Get(this.id);
            if (info != null)
            {
                this.hideImgUrl = (HtmlInputHidden) this.FindControl("hideImgUrl");
                this.hideTitle = (HtmlInputHidden) this.FindControl("hideTitle");
                this.hideDesc = (HtmlInputHidden) this.FindControl("hideDesc");
                this.hideTitle.Value = info.Name;
                this.hideDesc.Value = "活动时间：" + info.BeginDate.ToString("yyyy-MM-dd HH:mm:ss") + "至" + info.EndDate.ToString("yyyy-MM-dd HH:mm:ss");
                Uri url = HttpContext.Current.Request.Url;
                string imgUrl = info.ImgUrl;
                string str2 = string.Empty;
                if (!string.IsNullOrEmpty(imgUrl))
                {
                    if (!imgUrl.StartsWith("http"))
                    {
                        str2 = url.Scheme + "://" + url.Host + ((url.Port == 80) ? "" : (":" + url.Port.ToString()));
                    }
                    this.hideImgUrl.Value = str2 + imgUrl;
                }
                PageTitle.AddSiteNameTitle(info.Name);
                this.imgCover = (Image) this.FindControl("imgCover");
                if (!string.IsNullOrEmpty(info.ImgUrl))
                {
                    this.imgCover.ImageUrl = info.ImgUrl;
                }
                else
                {
                    this.imgCover.Visible = false;
                }
                MemberInfo currentMember = MemberProcessor.GetCurrentMember();
                this.litPoints = (Literal) this.FindControl("litPoints");
                if (currentMember != null)
                {
                    this.hiddisInRange = (HtmlInputHidden) this.FindControl("hiddisInRange");
                    this.hiddUserId = (HtmlInputHidden) this.FindControl("hiddUserId");
                    this.litPoints.Text = currentMember.Points.ToString();
                    this.hiddUserId.Value = currentMember.UserId.ToString();
                    if (MemberProcessor.CheckCurrentMemberIsInRange(info.MemberGrades, info.DefualtGroup, info.CustomGroup))
                    {
                        this.hiddisInRange.Value = "true";
                    }
                    else
                    {
                        this.hiddisInRange.Value = "false";
                    }
                }
                else
                {
                    this.litPoints.Text = "请先登录";
                }
            }
            else
            {
                HttpContext.Current.Response.Redirect("/default.aspx");
                HttpContext.Current.Response.End();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VExchangeList.html";
            }
            base.OnInit(e);
        }
    }
}

