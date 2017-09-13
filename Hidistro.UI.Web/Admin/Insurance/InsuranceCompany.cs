namespace Hidistro.UI.Web.Admin.Insurance
{
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.Core;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class InsuranceCompany : AdminPage
    {
        protected Button btnaddtag;
        protected Button btnupdatetag;
        protected HtmlInputHidden hdtagId;
        protected Repeater rp_prducttag;
        protected TextBox txtaddtagname;
        protected TextBox txttagname;

        protected InsuranceCompany() : base("m108", "bxp03")
        {
        }

        protected void btnaddtag_Click(object sender, EventArgs e)
        {
            string str = Globals.HtmlEncode(this.txtaddtagname.Text.Trim());
            if (string.IsNullOrEmpty(str))
            {
                this.ShowMsg("保险公司名称不允许为空！", false);
            }
            else if (CatalogHelper.AddInsuranceCompany(str) > 0)
            {
                this.ShowMsg("添加保险公司成功！", true);
                this.ProductTagsBind();
            }
            else
            {
                this.ShowMsg("添加保险公司失败，请确认是否存在重名保险公司名称", false);
            }
        }

        protected void btnupdatetag_Click(object sender, EventArgs e)
        {
            string str = this.hdtagId.Value.Trim();
            string str2 = Globals.HtmlEncode(this.txttagname.Text.Trim());
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(str2))
            {
                this.ShowMsg("请选择要修改的保险公司或输入保险公司名称", false);
            }
            else if (Convert.ToInt32(str) <= 0)
            {
                this.ShowMsg("选择的保险公司有误", false);
            }
            else if (CatalogHelper.UpdateInsuranceCompany(Convert.ToInt32(str), str2))
            {
                this.ShowMsg("修改保险公司成功", true);
                this.ProductTagsBind();
            }
            else
            {
                this.ShowMsg("修改保险公司失败，请确认输入的保险公司名称是否存在同名", false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(base.Request["isAjax"]) && (base.Request["isAjax"] == "true"))
            {
                string str5;
                string str = base.Request["Mode"].ToString();
                string str2 = "";
                string str3 = "false";
                if (((str5 = str) != null) && (str5 == "Add"))
                {
                    str2 = "保险公司名称不允许为空";
                    if (!string.IsNullOrEmpty(base.Request["TagValue"].Trim()))
                    {
                        str2 = "添加保险公司名称失败，请确认保险公司名是否已存在";
                        int num = CatalogHelper.AddInsuranceCompany(Globals.HtmlEncode(base.Request["TagValue"].ToString()));
                        if (num > 0)
                        {
                            str3 = "true";
                            str2 = num.ToString();
                        }
                    }
                    base.Response.Clear();
                    base.Response.ContentType = "application/json";
                    base.Response.Write("{\"Status\":\"" + str3 + "\",\"msg\":\"" + str2 + "\"}");
                    base.Response.End();
                }
            }
            this.btnaddtag.Click += new EventHandler(this.btnaddtag_Click);
            this.btnupdatetag.Click += new EventHandler(this.btnupdatetag_Click);
            this.rp_prducttag.ItemCommand += new RepeaterCommandEventHandler(this.rp_prducttag_ItemCommand);
            if (!base.IsPostBack)
            {
                this.ProductTagsBind();
            }
        }

        protected void ProductTagsBind()
        {
            this.rp_prducttag.DataSource = CatalogHelper.GetInsuranceCompany();
            this.rp_prducttag.DataBind();
        }

        protected void rp_prducttag_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.Equals("delete"))
            {
                string str = e.CommandArgument.ToString();
                if (!string.IsNullOrEmpty(str) && (Convert.ToInt32(str) > 0))
                {
                    if (CatalogHelper.DeleteInsuranceCompany(Convert.ToInt32(str)))
                    {
                        this.ShowMsg("删除保险公司成功", true);
                        this.ProductTagsBind();
                    }
                    else
                    {
                        this.ShowMsg("删除保险公司失败", false);
                    }
                }
            }
        }
    }
}

