namespace Hidistro.UI.SaleSystem.CodeBehind
{
    using Hidistro.Entities.VShop;
    using Hidistro.UI.Common.Controls;
    using System;

    public class VResourceNotFound : VshopTemplatedWebControl
    {
        private HiLiteral litDescription;
        private HiLiteral litImgIndex;

        protected override void AttachChildControls()
        {
            string str = this.Page.Request.QueryString["type"];
            ErrorType type = ErrorType.前台404;
            if (!string.IsNullOrWhiteSpace(str))
            {
                try
                {
                    type = (ErrorType) Enum.Parse(typeof(ErrorType), str);
                }
                catch (Exception)
                {
                    type = ErrorType.前台其它错误;
                }
            }
            this.litImgIndex = (HiLiteral) this.FindControl("litImgIndex");
            this.litDescription = (HiLiteral) this.FindControl("litDescription");
            switch (type)
            {
                case ErrorType.前台404:
                    this.litDescription.Text = "糟糕...这个页面打不开了";
                    this.litImgIndex.Text = "1";
                    break;

                case ErrorType.前台商品下架:
                    this.litDescription.Text = "哎呀...这个商品已经下架了";
                    this.litImgIndex.Text = "2";
                    break;

                default:
                    this.litDescription.Text = "很抱歉！我们正在修复中……";
                    this.litImgIndex.Text = "1";
                    break;
            }
            PageTitle.AddSiteNameTitle("出错了");
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.SkinName == null)
            {
                this.SkinName = "Skin-VResourceNotFound.html";
            }
            base.OnInit(e);
        }
    }
}

