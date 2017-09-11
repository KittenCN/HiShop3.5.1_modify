namespace Hidistro.UI.Web.Admin.Fenxiao
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class SuperiorSelect : AdminPage
    {
        protected Button btnSearch;
        protected Panel divEmpty;
        protected HtmlForm form1;
        protected string htmlStoreName;
        protected string htmlSuperName;
        protected Pager pager;
        protected int ReferralUserId;
        protected Repeater rptList;
        protected string searchName;
        protected TextBox txtKey;
        protected int userid;

        protected SuperiorSelect() : base("m05", "00000")
        {
            this.searchName = string.Empty;
            this.userid = Globals.RequestQueryNum("userid");
            this.htmlStoreName = string.Empty;
            this.htmlSuperName = "主站";
        }

        private void BindData(string title)
        {
            DistributorsQuery entity = new DistributorsQuery {
                GradeId = 0,
                StoreName = title,
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                SortOrder = SortAction.Desc,
                SortBy = "userid"
            };
            Globals.EntityCoding(entity, true);
            DbQueryResult result = VShopHelper.GetDistributors(entity, null, null);
            if (result.TotalRecords > 0)
            {
                this.rptList.DataSource = result.Data;
                this.rptList.DataBind();
                this.pager.TotalRecords = result.TotalRecords;
                if (this.pager.TotalRecords <= this.pager.PageSize)
                {
                    this.pager.Visible = false;
                }
            }
            else
            {
                this.divEmpty.Visible = true;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string s = this.txtKey.Text.Trim();
            base.Response.Redirect(string.Concat(new object[] { "superiorselect.aspx?userid=", this.userid, "&key=", base.Server.UrlEncode(s) }));
            base.Response.End();
        }

        protected string FormatOperBtn(object touserid, object storename)
        {
            string s = DistributorsBrower.IsCanUpdateDistributorSuperior(this.userid, Globals.ToNum(touserid));
            if ((s == "1") && (this.ReferralUserId != Convert.ToInt32(touserid)))
            {
                return string.Concat(new object[] { "<input type='button' id='dist", touserid, "' class='btn btn-primary btn-xs' value='设为上级' onclick=\"setsuper(this,", touserid, ",'", base.Server.HtmlEncode(storename.ToString()), "')\" />" });
            }
            return ("<span title='" + base.Server.HtmlEncode(s) + "'><input type='button' class='btn btn-primary btn-xs' style='background-color:#5cb85c;border-color:#4cae4c' value='设为上级' disabled='disabled'/></span>");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Globals.RequestFormStr("posttype") == "update")
            {
                base.Response.ContentType = "application/json";
                string s = "{\"type\":\"0\",\"tips\":\"操作失败！\"}";
                this.userid = Globals.RequestFormNum("userid");
                int tosuperuserid = Globals.RequestFormNum("touserid");
                string str3 = DistributorsBrower.UpdateDistributorSuperior(this.userid, tosuperuserid);
                if (str3 == "1")
                {
                    s = "{\"type\":\"1\",\"tips\":\"修改成功！\"}";
                }
                else
                {
                    s = "{\"type\":\"0\",\"tips\":\"" + str3 + "\"}";
                }
                base.Response.Write(s);
                base.Response.End();
            }
            if (this.userid > 0)
            {
                DistributorsInfo distributorInfo = DistributorsBrower.GetDistributorInfo(this.userid);
                if (distributorInfo == null)
                {
                    this.divEmpty.Visible = true;
                }
                else
                {
                    this.htmlStoreName = distributorInfo.StoreName;
                    if ((distributorInfo.ReferralUserId > 0) && (distributorInfo.UserId != distributorInfo.ReferralUserId))
                    {
                        this.ReferralUserId = distributorInfo.ReferralUserId;
                        distributorInfo = DistributorsBrower.GetDistributorInfo(distributorInfo.ReferralUserId);
                        if (distributorInfo != null)
                        {
                            this.htmlSuperName = distributorInfo.StoreName;
                        }
                    }
                    this.searchName = Globals.RequestQueryStr("key").Trim();
                    if (!base.IsPostBack)
                    {
                        this.txtKey.Text = this.searchName;
                        this.BindData(this.searchName);
                    }
                }
            }
        }
    }
}

