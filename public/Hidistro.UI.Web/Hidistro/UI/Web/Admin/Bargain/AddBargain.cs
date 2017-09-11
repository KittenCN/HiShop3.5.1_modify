namespace Hidistro.UI.Web.Admin.Bargain
{
    using Hidistro.ControlPanel.Bargain;
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.Core;
    using Hidistro.Entities.Bargain;
    using Hidistro.Entities.Commodities;
    using Hidistro.UI.ControlPanel.Utility;
    using Hidistro.UI.Web.Admin.Ascx;
    using System;
    using System.Text;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class AddBargain : AdminPage
    {
        protected Button btnSave;
        protected ucDateTimePicker calendarEndDate;
        protected ucDateTimePicker calendarStartDate;
        protected CheckBox ckIsCommission;
        protected TextBox discount;
        protected HiddenField hiddProductId;
        protected HiddenField hidpic;
        protected HiddenField hidpicdel;
        protected Label lbProductName;
        protected Label lbtitle;
        protected Image productImage;
        protected string productInfoHtml;
        protected RadioButton rbtBargainTypeOne;
        protected RadioButton rbtBargainTypeTwo;
        protected HtmlForm thisForm;
        protected TextBox txt_img;
        protected TextBox txtActivityStock;
        protected TextBox txtBargainTypeOneValue;
        protected TextBox txtBargainTypeTwoValue1;
        protected TextBox txtBargainTypeTwoValue2;
        protected TextBox txtFloorPrice;
        protected TextBox txtInitialPrice;
        protected TextBox txtPurchaseNumber;
        protected TextBox txtRemarks;
        protected TextBox txtTitle;

        public AddBargain() : base("m08", "yxp21")
        {
            this.productInfoHtml = "活动商品尚未设置,请选择活动商品";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int num = Globals.RequestQueryNum("id");
            string text = this.txtTitle.Text;
            if (string.IsNullOrWhiteSpace(text))
            {
                this.ShowMsg("分享标题不能为空！", false);
            }
            else
            {
                string str2 = this.calendarStartDate.Text;
                string str3 = this.calendarEndDate.Text;
                if (string.IsNullOrWhiteSpace(str2) || string.IsNullOrWhiteSpace(str3))
                {
                    this.ShowMsg("活动时间不能为空！", false);
                }
                else if (DateTime.Parse(str2) >= DateTime.Parse(str3))
                {
                    this.ShowMsg("结束时间必须大于开始时间！", false);
                }
                else
                {
                    string str4 = this.hidpic.Value;
                    if (string.IsNullOrWhiteSpace(str4))
                    {
                        this.ShowMsg("请先上传活动封面！", false);
                    }
                    else
                    {
                        string str5 = this.txtRemarks.Text;
                        if (str5.Length > 200)
                        {
                            this.ShowMsg("活动说明不能超过200个字节！", false);
                        }
                        else
                        {
                            int productId = Globals.ToNum(this.hiddProductId.Value);
                            if (productId <= 0)
                            {
                                this.ShowMsg("请先选择参加活动的产品！", false);
                            }
                            else
                            {
                                int num3 = Globals.ToNum(this.txtActivityStock.Text.Trim());
                                if (num3 <= 0)
                                {
                                    this.ShowMsg("活动库存必须为大于0的正整数！", false);
                                }
                                else
                                {
                                    int num4 = Globals.ToNum(this.txtPurchaseNumber.Text.Trim());
                                    if (num4 <= 0)
                                    {
                                        this.ShowMsg("限购数量必须为大于0的正整数！", false);
                                    }
                                    else
                                    {
                                        float result = 0f;
                                        if (!float.TryParse(this.txtInitialPrice.Text, out result))
                                        {
                                            this.ShowMsg("初始价格输入不正确！", false);
                                        }
                                        else
                                        {
                                            float num6 = 0f;
                                            if (!float.TryParse(this.txtFloorPrice.Text, out num6))
                                            {
                                                this.ShowMsg("活动底价输入不正确！", false);
                                            }
                                            else if ((num6 <= 0f) || (result <= 0f))
                                            {
                                                this.ShowMsg("活动底价必须或初始价格必须大于0！", false);
                                            }
                                            else if (result < num6)
                                            {
                                                this.ShowMsg("初始价格不能小于活动底价！", false);
                                            }
                                            else
                                            {
                                                long productSumStock = ProductHelper.GetProductSumStock(productId);
                                                if (num3 > productSumStock)
                                                {
                                                    this.ShowMsg("活动库存不能大于商品库存！", false);
                                                }
                                                else if (num4 > num3)
                                                {
                                                    this.ShowMsg("限购数量不能大于活动库存！", false);
                                                }
                                                else
                                                {
                                                    bool flag = this.ckIsCommission.Checked;
                                                    string str6 = this.discount.Text;
                                                    if (flag && string.IsNullOrEmpty(str6))
                                                    {
                                                        this.ShowMsg("请填写佣金折扣值！", false);
                                                    }
                                                    else if (flag && (str6 == "0"))
                                                    {
                                                        this.ShowMsg("佣金折扣值必须大于0！", false);
                                                    }
                                                    else
                                                    {
                                                        int num8 = this.rbtBargainTypeOne.Checked ? 0 : 1;
                                                        BargainInfo bargain = new BargainInfo {
                                                            Title = text,
                                                            BeginDate = DateTime.Parse(str2),
                                                            EndDate = DateTime.Parse(str3),
                                                            ActivityCover = str4,
                                                            Remarks = str5,
                                                            ProductId = productId,
                                                            ActivityStock = num3,
                                                            PurchaseNumber = num4,
                                                            TranNumber = 0,
                                                            InitialPrice = (decimal) result,
                                                            FloorPrice = (decimal) num6,
                                                            BargainType = num8,
                                                            CreateDate = DateTime.Now,
                                                            IsCommission = this.ckIsCommission.Checked,
                                                            CommissionDiscount = flag ? ((int) (float.Parse(str6) * 10f)) : 0
                                                        };
                                                        if (num8 == 0)
                                                        {
                                                            string str7 = this.txtBargainTypeOneValue.Text;
                                                            if (string.IsNullOrWhiteSpace(str7))
                                                            {
                                                                this.ShowMsg("每次砍掉价格不能为空！", false);
                                                                return;
                                                            }
                                                            bargain.BargainTypeMinVlue = float.Parse(str7);
                                                        }
                                                        else
                                                        {
                                                            string str8 = this.txtBargainTypeTwoValue1.Text;
                                                            string str9 = this.txtBargainTypeTwoValue2.Text;
                                                            if (string.IsNullOrWhiteSpace(str8) || string.IsNullOrWhiteSpace(str9))
                                                            {
                                                                this.ShowMsg("随机砍价最小值或最大值不能为空！", false);
                                                                return;
                                                            }
                                                            float num9 = 0f;
                                                            float num10 = 0f;
                                                            if (!float.TryParse(str8, out num9))
                                                            {
                                                                this.ShowMsg("随机砍价最小值必须为数值！", false);
                                                                return;
                                                            }
                                                            if (!float.TryParse(str9, out num10))
                                                            {
                                                                this.ShowMsg("随机砍价最大值必须为数值！", false);
                                                                return;
                                                            }
                                                            if (num9 > num10)
                                                            {
                                                                this.ShowMsg("随机砍价最大值必须大于最小值！", false);
                                                                return;
                                                            }
                                                            if ((num9 < 0f) || (num10 < 0f))
                                                            {
                                                                this.ShowMsg("随机砍价最大值,最小值都必须大于零！", false);
                                                                return;
                                                            }
                                                            bargain.BargainTypeMinVlue = num9;
                                                            bargain.BargainTypeMaxVlue = num10;
                                                        }
                                                        if (num > 0)
                                                        {
                                                            bargain.Id = num;
                                                            if (BargainHelper.UpdateBargain(bargain))
                                                            {
                                                                this.ShowMsgAndReUrl("修改成功", true, "ManagerBargain.aspx?Type=0");
                                                            }
                                                        }
                                                        else if (BargainHelper.InsertBargain(bargain))
                                                        {
                                                            this.ShowMsgAndReUrl("添加成功", true, "ManagerBargain.aspx?Type=0");
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public string GetProductInfoHtml(ProductInfo product)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<div class='shop-img fl'>");
            builder.Append("<img src='" + (string.IsNullOrEmpty(product.ImageUrl1) ? "/utility/pics/none.gif" : product.ImageUrl1) + "' width='60' height='60' >");
            builder.Append("</div>");
            builder.Append("<div class='shop-username fl ml10'>");
            builder.Append("<p>" + product.ProductName + "</p>");
            builder.Append("</div>");
            builder.Append(" <p class='fl ml20'>现价：￥" + product.MarketPrice.Value.ToString("f2") + "</p>");
            builder.Append(" <p class='fl ml20'>库存：" + ProductHelper.GetProductSumStock(product.ProductId) + "</p>");
            return builder.ToString();
        }

        private void LoadData()
        {
            int result = 0;
            if (int.TryParse(this.Page.Request.QueryString["Id"], out result))
            {
                BargainInfo bargainInfo = BargainHelper.GetBargainInfo(result);
                if (bargainInfo != null)
                {
                    if (bargainInfo.ProductId > 0)
                    {
                        ProductInfo productDetails = ProductHelper.GetProductDetails(bargainInfo.ProductId);
                        this.productImage.ImageUrl = string.IsNullOrEmpty(productDetails.ImageUrl1) ? "/utility/pics/none.gif" : productDetails.ImageUrl1;
                        this.lbProductName.Text = productDetails.ProductName;
                        this.lbtitle.Text = bargainInfo.Title;
                        this.productInfoHtml = this.GetProductInfoHtml(productDetails);
                    }
                    this.txtTitle.Text = bargainInfo.Title;
                    this.calendarStartDate.Text = bargainInfo.BeginDate.ToString();
                    this.calendarEndDate.Text = bargainInfo.EndDate.ToString();
                    this.hidpic.Value = bargainInfo.ActivityCover;
                    this.hiddProductId.Value = bargainInfo.ProductId.ToString();
                    this.txtRemarks.Text = bargainInfo.Remarks;
                    this.txtActivityStock.Text = bargainInfo.ActivityStock.ToString();
                    this.txtPurchaseNumber.Text = bargainInfo.PurchaseNumber.ToString();
                    this.txtInitialPrice.Text = bargainInfo.InitialPrice.ToString("f2");
                    this.txtFloorPrice.Text = bargainInfo.FloorPrice.ToString("f2");
                    this.ckIsCommission.Checked = bargainInfo.IsCommission;
                    this.discount.Text = (((double) bargainInfo.CommissionDiscount) / 10.0).ToString();
                    if (bargainInfo.BargainType == 0)
                    {
                        this.rbtBargainTypeOne.Checked = true;
                        this.rbtBargainTypeTwo.Checked = false;
                        this.txtBargainTypeOneValue.Text = bargainInfo.BargainTypeMinVlue.ToString("f2");
                    }
                    else
                    {
                        this.rbtBargainTypeOne.Checked = false;
                        this.rbtBargainTypeTwo.Checked = true;
                        this.txtBargainTypeTwoValue1.Text = bargainInfo.BargainTypeMinVlue.ToString("f2");
                        this.txtBargainTypeTwoValue2.Text = bargainInfo.BargainTypeMaxVlue.ToString("f2");
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSave.Click += new EventHandler(this.btnSave_Click);
            if (!base.IsPostBack)
            {
                this.LoadData();
            }
        }
    }
}

