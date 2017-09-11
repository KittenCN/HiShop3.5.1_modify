namespace Hidistro.UI.Web.Admin.Fenxiao
{
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class EditDistributorGrade : AdminPage
    {
        protected Button btnEditUser;
        protected HtmlGenericControl EditTitle;
        protected HtmlGenericControl GIsDefault;
        private int GradeId;
        protected string htmlOperatorName;
        protected RadioButtonList rbtnlIsDefault;
        protected string ReUrl;
        protected TextBox txtCommissionsLimit;
        protected TextBox txtDescription;
        protected TextBox txtFirstCommissionRise;
        protected TextBox txtName;
        protected TextBox txtSecondCommissionRise;
        protected TextBox txtThirdCommissionRise;
        protected UpImg uploader1;

        protected EditDistributorGrade() : base("m05", "fxp04")
        {
            this.ReUrl = "distributorgradelist.aspx";
            this.htmlOperatorName = "编辑";
        }

        protected void btnEditUser_Click(object sender, EventArgs e)
        {
            decimal result = 0.0M;
            decimal num2 = 0.0M;
            decimal num3 = 0.0M;
            decimal num4 = 0.0M;
            DistributorGradeInfo distributorgrade = new DistributorGradeInfo();
            if (this.GradeId > 0)
            {
                distributorgrade = DistributorGradeBrower.GetDistributorGradeInfo(this.GradeId);
            }
            distributorgrade.Name = this.txtName.Text.Trim();
            decimal.TryParse(this.txtCommissionsLimit.Text.Trim(), out result);
            decimal.TryParse(this.txtFirstCommissionRise.Text.Trim(), out num2);
            decimal.TryParse(this.txtSecondCommissionRise.Text.Trim(), out num3);
            decimal.TryParse(this.txtThirdCommissionRise.Text.Trim(), out num4);
            distributorgrade.CommissionsLimit = result;
            distributorgrade.FirstCommissionRise = num2;
            distributorgrade.SecondCommissionRise = num3;
            distributorgrade.ThirdCommissionRise = num4;
            distributorgrade.IsDefault = this.rbtnlIsDefault.SelectedIndex == 0;
            distributorgrade.Description = this.txtDescription.Text.Trim();
            distributorgrade.Ico = this.uploader1.UploadedImageUrl;
            if (DistributorGradeBrower.IsExistsMinAmount(this.GradeId, result))
            {
                this.ShowMsg("已存在相同佣金的分销商等级", false);
            }
            else if (this.GradeId > 0)
            {
                if (DistributorGradeBrower.UpdateDistributor(distributorgrade))
                {
                    if (base.Request.QueryString["reurl"] != null)
                    {
                        this.ReUrl = base.Request.QueryString["reurl"].ToString();
                    }
                    if (string.IsNullOrEmpty(this.ReUrl))
                    {
                        this.ReUrl = "distributorgradelist.aspx";
                    }
                    this.ShowMsgAndReUrl("成功修改了分销商等级", true, this.ReUrl);
                }
                else
                {
                    this.ShowMsg("分销商等级修改失败", false);
                }
            }
            else if (DistributorGradeBrower.CreateDistributorGrade(distributorgrade))
            {
                this.ShowMsgAndReUrl("成功新增了分销商等级", true, this.ReUrl);
            }
            else
            {
                this.ShowMsg("分销商等级新增失败", false);
            }
        }

        private void LoadDistributorGradeInfo()
        {
            if (this.GradeId > 0)
            {
                DistributorGradeInfo distributorGradeInfo = DistributorGradeBrower.GetDistributorGradeInfo(this.GradeId);
                if (distributorGradeInfo == null)
                {
                    base.GotoResourceNotFound();
                }
                else
                {
                    this.txtName.Text = distributorGradeInfo.Name;
                    this.txtCommissionsLimit.Text = distributorGradeInfo.CommissionsLimit.ToString("F2");
                    this.txtFirstCommissionRise.Text = distributorGradeInfo.FirstCommissionRise.ToString();
                    this.txtSecondCommissionRise.Text = distributorGradeInfo.SecondCommissionRise.ToString();
                    this.txtThirdCommissionRise.Text = distributorGradeInfo.ThirdCommissionRise.ToString();
                    this.rbtnlIsDefault.SelectedIndex = distributorGradeInfo.IsDefault ? 0 : 1;
                    if (distributorGradeInfo.IsDefault)
                    {
                        this.GIsDefault.Style.Add("display", "none");
                    }
                    if (distributorGradeInfo.IsDefault)
                    {
                        this.rbtnlIsDefault.Enabled = false;
                    }
                    this.txtDescription.Text = distributorGradeInfo.Description;
                    string ico = distributorGradeInfo.Ico;
                    if (ico != "/utility/pics/grade.png")
                    {
                        this.uploader1.UploadedImageUrl = ico;
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Page.Request.QueryString["ID"] != null)
            {
                if (!int.TryParse(this.Page.Request.QueryString["ID"], out this.GradeId))
                {
                    base.GotoResourceNotFound();
                    return;
                }
            }
            else
            {
                this.htmlOperatorName = "新增";
            }
            this.btnEditUser.Click += new EventHandler(this.btnEditUser_Click);
            if (!this.Page.IsPostBack)
            {
                this.LoadDistributorGradeInfo();
            }
        }

        private bool ValidationMember(MemberInfo member)
        {
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<MemberInfo>(member, new string[] { "ValMember" });
            string msg = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                this.ShowMsg(msg, false);
            }
            return results.IsValid;
        }
    }
}

