namespace Hidistro.UI.Web.Admin.Insurance
{
    using Core;
    using Entities;
    using Hidistro.ControlPanel.CashBack;
    using Hidistro.ControlPanel.Commodities;
    using Hidistro.ControlPanel.Members;
    using Hidistro.Entities.CashBack;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Xml;

    public class ManageInsuranceArea : AdminPage
    {
        protected Button btnaddtag;
        protected Button btnupdatetag;
        protected HtmlInputHidden hdtagId;
        protected Repeater rp_prducttag;
        protected TextBox txtaddtagname;
        protected TextBox txttagname;
        protected CheckBoxList txtInsuranceCompanyType;
        protected RegionSelector rsddlRegion;

        protected ManageInsuranceArea() : base("m108", "bxp02")
        {
        }

        protected void btnaddtag_Click(object sender, EventArgs e)
        {
            
            int? pnameid = rsddlRegion.GetSelectedRegionId();

            if (!pnameid.HasValue)
                this.ShowMsg("保险地区不能为空！", false);
            else
            {
                XmlNode region = RegionHelper.GetRegion(int.Parse(pnameid.ToString()));
                string insuranceAreaCiteId = region.Attributes["id"].Value;
                string insuranceAreaCiteName = region.Attributes["name"].Value;

                string insuranceAreaProvinceId = region.ParentNode.Attributes["id"].Value;
                string insuranceAreaName = region.ParentNode.Attributes["name"].Value;


                string insuranceCompanyTypes = "", insuranceCompanyTypesIds="";
                int tmpfirst = 0;
                for (int i = 0; i < txtInsuranceCompanyType.Items.Count; i++)
                {
                    if (txtInsuranceCompanyType.Items[i].Selected)
                    {
                        insuranceCompanyTypes += "" + txtInsuranceCompanyType.Items[i].Text + "" + ",";
                        if (tmpfirst != 0)
                            insuranceCompanyTypesIds += txtInsuranceCompanyType.Items[i].Value + ",";
                        else
                            insuranceCompanyTypesIds += "," + txtInsuranceCompanyType.Items[i].Value + ",";
                        tmpfirst++;
                    }
                }
                if (insuranceCompanyTypes.Length > 1)
                {
                    insuranceCompanyTypes = insuranceCompanyTypes.Substring(0, insuranceCompanyTypes.Length - 1);
                    //insuranceCompanyTypesIds = insuranceCompanyTypesIds.Substring(0, insuranceCompanyTypesIds.Length - 1);
                }

                int rnum=CatalogHelper.AddInsuranceCompanyArea(insuranceAreaCiteId, insuranceAreaCiteName, insuranceAreaProvinceId, insuranceAreaName, insuranceCompanyTypes, insuranceCompanyTypesIds);

                if (rnum > 0)
                {
                    this.ShowMsg("添加保险公司成功！", true);
                    this.ProductTagsBind();
                }
                else
                {
                    this.ShowMsg("添加保险公司失败，已经添加过该地区！", false);
                }

            }
            
            
           
        }

        protected void btnupdatetag_Click(object sender, EventArgs e)
        {
            string str = this.hdtagId.Value.Trim();
            int? pnameid = rsddlRegion.GetSelectedRegionId();

            if (!pnameid.HasValue)
                this.ShowMsg("保险地区不能为空！", false);

            else
            {
                XmlNode region = RegionHelper.GetRegion(int.Parse(pnameid.ToString()));
                string insuranceAreaCiteId = region.Attributes["id"].Value;
                string insuranceAreaCiteName = region.Attributes["name"].Value;

                string insuranceAreaProvinceId = region.ParentNode.Attributes["id"].Value;
                string insuranceAreaName = region.ParentNode.Attributes["name"].Value;


                string insuranceCompanyTypes = "", insuranceCompanyTypesIds = "";
                int tmpfirst = 0;
                for (int i = 0; i < txtInsuranceCompanyType.Items.Count; i++)
                {
                    if (txtInsuranceCompanyType.Items[i].Selected)
                    {
                        insuranceCompanyTypes += "" + txtInsuranceCompanyType.Items[i].Text + "" + ",";
                        if (tmpfirst != 0)
                            insuranceCompanyTypesIds += txtInsuranceCompanyType.Items[i].Value + ",";
                        else
                            insuranceCompanyTypesIds += "," + txtInsuranceCompanyType.Items[i].Value + ",";
                        tmpfirst++;
                    }
                }
                if (insuranceCompanyTypes.Length > 1)
                {
                    insuranceCompanyTypes = insuranceCompanyTypes.Substring(0, insuranceCompanyTypes.Length - 1);
                   
                }

                if (CatalogHelper.UpdateInsuranceArea(Convert.ToInt32(str), insuranceAreaCiteId, insuranceAreaCiteName, insuranceAreaProvinceId, insuranceAreaName, insuranceCompanyTypes, insuranceCompanyTypesIds))
                {
                    this.ShowMsg("修改保险地区成功", true);
                    this.ProductTagsBind();
                }
                else
                {
                    this.ShowMsg("修改保险地区失败，请确认输入的保险公地区是否存在", false);
                }
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
                this.txtInsuranceCompanyType.DataTextField = "InsuranceCompanyName";
                this.txtInsuranceCompanyType.DataValueField = "InsuranceCompanyID";
                txtInsuranceCompanyType.DataSource= CatalogHelper.GetInsuranceCompany();
                txtInsuranceCompanyType.DataBind();

                //this.txtEditInsuranceCompanyType.DataTextField = "InsuranceCompanyName";
                //this.txtEditInsuranceCompanyType.DataValueField = "InsuranceCompanyID";
                //txtEditInsuranceCompanyType.DataSource = CatalogHelper.GetInsuranceCompany();
                //txtEditInsuranceCompanyType.DataBind();


                
            }
        }

        protected void ProductTagsBind()
        {
            this.rp_prducttag.DataSource = CatalogHelper.GetInsuranceArea();
            this.rp_prducttag.DataBind();
        }

        protected void rp_prducttag_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.Equals("delete"))
            {
                string str = e.CommandArgument.ToString();
                if (!string.IsNullOrEmpty(str) && (Convert.ToInt32(str) > 0))
                {
                    if (CatalogHelper.DeleteInsuranceArea(Convert.ToInt32(str)))
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

