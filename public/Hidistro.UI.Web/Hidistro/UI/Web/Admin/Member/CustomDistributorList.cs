namespace Hidistro.UI.Web.Admin.Member
{
    using Hidistro.ControlPanel.Members;
    using Hidistro.Core;
    using Hidistro.Entities.Members;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class CustomDistributorList : AdminPage
    {
        protected Button btnaddgroup;
        protected Button btnupdategroup;
        protected HtmlInputHidden hdgroupId;
        protected Repeater rptList;
        protected TextBox txtaddgroupname;
        protected TextBox txtgroupname;

        protected CustomDistributorList() : base("m04", "hyp05")
        {
        }

        private void BindData()
        {
            this.rptList.DataSource = CustomGroupingHelper.GetCustomGroupingDataTable();
            this.rptList.DataBind();
        }

        protected void btnaddgroup_Click(object sender, EventArgs e)
        {
            string str = Globals.HtmlEncode(this.txtaddgroupname.Text.Trim());
            if (string.IsNullOrEmpty(str))
            {
                this.ShowMsg("分组名称不允许为空！", false);
            }
            else
            {
                CustomGroupingInfo customGroupingInfo = new CustomGroupingInfo {
                    GroupName = str
                };
                string s = CustomGroupingHelper.AddCustomGrouping(customGroupingInfo);
                if (Globals.ToNum(s) > 0)
                {
                    this.ShowMsg("添加分组成功！", true);
                    this.BindData();
                }
                else
                {
                    this.ShowMsg("添加分组失败，" + s, false);
                }
            }
        }

        protected void btnupdategroup_Click(object sender, EventArgs e)
        {
            int s = Globals.ToNum(this.hdgroupId.Value.Trim());
            string str = Globals.HtmlEncode(this.txtgroupname.Text.Trim());
            if (Globals.ToNum(s) <= 0)
            {
                this.ShowMsg("选择的分组有误,请重新选择", false);
            }
            else if (string.IsNullOrEmpty(str))
            {
                this.ShowMsg("分组名称不允许为空", false);
            }
            else
            {
                CustomGroupingInfo customGroupingInfo = new CustomGroupingInfo {
                    GroupName = str,
                    Id = s
                };
                string str2 = CustomGroupingHelper.UpdateCustomGrouping(customGroupingInfo);
                if (Globals.ToNum(str2) > 0)
                {
                    this.ShowMsg("修改商品分组成功", true);
                    this.BindData();
                }
                else
                {
                    this.ShowMsg("修改商品分组失败，" + str2, false);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnaddgroup.Click += new EventHandler(this.btnaddgroup_Click);
            this.btnupdategroup.Click += new EventHandler(this.btnupdategroup_Click);
            if (!base.IsPostBack)
            {
                this.BindData();
            }
        }

        protected void rptList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string str;
            if (((str = e.CommandName) != null) && (str == "delete"))
            {
                if (CustomGroupingHelper.DelGroup(Globals.ToNum(e.CommandArgument.ToString())))
                {
                    this.ShowMsg("成功删除了指定的分组", true);
                    this.BindData();
                }
                else
                {
                    this.ShowMsg("删除分组失败", false);
                }
            }
        }

        protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                int groupId = (int) DataBinder.Eval(e.Item.DataItem, "Id");
                int length = 0;
                int num3 = 0;
                int num4 = 0;
                DataTable customGroupingUser = CustomGroupingHelper.GetCustomGroupingUser(groupId);
                if (customGroupingUser.Rows.Count > 0)
                {
                    length = customGroupingUser.Select("LastOrderDate is null").Length;
                    int activeDay = MemberHelper.GetActiveDay();
                    num3 = customGroupingUser.Select(" PayOrderDate is not null and PayOrderDate >='" + DateTime.Now.AddDays((double) -activeDay).ToString("yyyy-MM-dd HH:mm:ss") + "'").Length;
                    num4 = customGroupingUser.Select(" PayOrderDate is null or PayOrderDate <'" + DateTime.Now.AddDays((double) -activeDay).ToString("yyyy-MM-dd HH:mm:ss") + "'").Length;
                }
                Literal literal = e.Item.FindControl("ltMemberNumList") as Literal;
                literal.Text = string.Concat(new object[] { "<td>", length, "</td><td>", num3, "</td><td>", num4, "</td>" });
            }
        }
    }
}

