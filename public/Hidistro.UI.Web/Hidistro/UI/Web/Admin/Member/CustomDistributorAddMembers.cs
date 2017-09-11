namespace Hidistro.UI.Web.Admin.Member
{
    using Hidistro.ControlPanel.Members;
    using Hidistro.Entities.Members;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class CustomDistributorAddMembers : AdminPage
    {
        protected Button btnJoin;
        protected Button btnSelect;
        protected int currentGroupId;
        protected Literal GroupName;
        protected HiddenField hdCustomGroup;
        protected HiddenField hdDefualtGroup;
        protected HiddenField hdGrades;
        protected HiddenField hdRegisterDate;
        protected HiddenField hdTradeDate;
        protected Literal litMembersNum;
        protected string localUrl;
        protected HtmlGenericControl resultDiv;
        protected HtmlForm thisForm;
        protected TrimTextBox txtStroeName;
        protected TrimTextBox txtTradeMoney1;
        protected TrimTextBox txtTradeMoney2;
        protected TrimTextBox txtTradeNum1;
        protected TrimTextBox txtTradeNum2;

        protected CustomDistributorAddMembers() : base("m04", "hyp05")
        {
            this.localUrl = string.Empty;
        }

        protected void btnJoin_Click(object sender, EventArgs e)
        {
            IList<int> memberList = CustomGroupingHelper.GetMemberList(this.GetMemberQuery());
            if (memberList != null)
            {
                CustomGroupingHelper.AddCustomGroupingUser(memberList, this.currentGroupId);
                this.ShowMsgAndReUrl("添加成功！", true, "/Admin/member/CustomDistributorDetail.aspx?GroupId=" + this.currentGroupId);
            }
            else
            {
                this.ShowMsg("未找到符合条件的会员，请重新选择条件！", false);
            }
        }

        protected void btnSelect_Click(object sender, EventArgs e)
        {
            IList<int> memberList = CustomGroupingHelper.GetMemberList(this.GetMemberQuery());
            if (memberList != null)
            {
                this.resultDiv.Visible = true;
                this.litMembersNum.Text = memberList.Count.ToString();
            }
        }

        protected string GetMemberCustomGroup()
        {
            StringBuilder builder = new StringBuilder();
            IList<CustomGroupingInfo> customGroupingList = CustomGroupingHelper.GetCustomGroupingList();
            if ((customGroupingList != null) && (customGroupingList.Count > 0))
            {
                foreach (CustomGroupingInfo info in customGroupingList)
                {
                    if (info.Id != this.currentGroupId)
                    {
                        builder.Append(" <label class=\"middle mr20\">");
                        builder.AppendFormat("<input type=\"checkbox\" class=\"CustomGroup\" value=\"{0}\">{1}", info.Id, info.GroupName);
                        builder.Append("  </label>");
                    }
                }
            }
            return builder.ToString();
        }

        protected string GetMemberGrande()
        {
            StringBuilder builder = new StringBuilder();
            IList<MemberGradeInfo> memberGrades = MemberHelper.GetMemberGrades();
            if ((memberGrades != null) && (memberGrades.Count > 0))
            {
                foreach (MemberGradeInfo info in memberGrades)
                {
                    builder.Append(" <label class=\"middle mr20\">");
                    builder.AppendFormat("<input type=\"checkbox\" class=\"memberGradeCheck\" value=\"{0}\">{1}", info.GradeId, info.Name);
                    builder.Append("  </label>");
                }
            }
            return builder.ToString();
        }

        private MemberQuery GetMemberQuery()
        {
            string str;
            string str2;
            MemberQuery query = new MemberQuery();
            if (!string.IsNullOrEmpty(this.txtStroeName.Text))
            {
                query.StoreName = this.txtStroeName.Text.Trim();
            }
            if (!string.IsNullOrEmpty(this.txtTradeMoney1.Text))
            {
                decimal result = 0M;
                if (decimal.TryParse(this.txtTradeMoney1.Text, out result))
                {
                    query.TradeMoneyStart = new decimal?(result);
                }
            }
            if (!string.IsNullOrEmpty(this.txtTradeMoney2.Text))
            {
                decimal num2 = 0M;
                if (decimal.TryParse(this.txtTradeMoney2.Text, out num2))
                {
                    query.TradeMoneyEnd = new decimal?(num2);
                }
            }
            if (!string.IsNullOrEmpty(this.txtTradeNum1.Text))
            {
                int num3 = 0;
                if (int.TryParse(this.txtTradeNum1.Text, out num3))
                {
                    query.TradeNumStart = new int?(num3);
                }
            }
            if (!string.IsNullOrEmpty(this.txtTradeNum2.Text))
            {
                int num4 = 0;
                if (int.TryParse(this.txtTradeNum2.Text, out num4))
                {
                    query.TradeNumEnd = new int?(num4);
                }
            }
            query.Stutas = (UserStatus)1;
            if (!string.IsNullOrEmpty(this.hdGrades.Value) && !this.hdGrades.Value.Equals("-1"))
            {
                query.GradeIds = this.hdGrades.Value;
            }
            if (!string.IsNullOrEmpty(this.hdDefualtGroup.Value) && !this.hdDefualtGroup.Value.Equals("-1"))
            {
                query.ClientType = this.hdDefualtGroup.Value;
            }
            if (!string.IsNullOrEmpty(this.hdCustomGroup.Value) && !this.hdCustomGroup.Value.Equals("-1"))
            {
                query.GroupIds = this.hdCustomGroup.Value;
            }
            if ((!string.IsNullOrEmpty(this.hdRegisterDate.Value) && !this.hdRegisterDate.Value.Equals("all")) && ((str = this.hdRegisterDate.Value) != null))
            {
                if (!(str == "week"))
                {
                    if (str == "month")
                    {
                        query.RegisterStartTime = new DateTime?(DateTime.Now.AddMonths(-1));
                        query.RegisterEndTime = new DateTime?(DateTime.Now);
                    }
                    else if (str == "threeMonth")
                    {
                        query.RegisterStartTime = new DateTime?(DateTime.Now.AddMonths(-3));
                        query.RegisterEndTime = new DateTime?(DateTime.Now);
                    }
                    else if (str == "moreMonth")
                    {
                        query.RegisterEndTime = new DateTime?(DateTime.Now.AddMonths(-3));
                    }
                }
                else
                {
                    query.RegisterStartTime = new DateTime?(DateTime.Now.AddDays(-7.0));
                    query.RegisterEndTime = new DateTime?(DateTime.Now);
                }
            }
            if ((!string.IsNullOrEmpty(this.hdTradeDate.Value) && !this.hdTradeDate.Value.Equals("all")) && ((str2 = this.hdTradeDate.Value) != null))
            {
                if (!(str2 == "week"))
                {
                    if (str2 == "month")
                    {
                        query.StartTime = new DateTime?(DateTime.Now.AddMonths(-1));
                        query.EndTime = new DateTime?(DateTime.Now);
                        return query;
                    }
                    if (str2 == "threeMonth")
                    {
                        query.StartTime = new DateTime?(DateTime.Now.AddMonths(-3));
                        query.EndTime = new DateTime?(DateTime.Now);
                        return query;
                    }
                    if (str2 == "moreMonth")
                    {
                        query.EndTime = new DateTime?(DateTime.Now.AddMonths(-3));
                    }
                    return query;
                }
                query.StartTime = new DateTime?(DateTime.Now.AddDays(-7.0));
                query.EndTime = new DateTime?(DateTime.Now);
            }
            return query;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.localUrl = base.Request.Url.ToString();
            if (!int.TryParse(this.Page.Request.QueryString["GroupId"], out this.currentGroupId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.btnSelect.Click += new EventHandler(this.btnSelect_Click);
                this.btnJoin.Click += new EventHandler(this.btnJoin_Click);
                if (!base.IsPostBack)
                {
                    CustomGroupingInfo groupInfoById = CustomGroupingHelper.GetGroupInfoById(this.currentGroupId);
                    if (groupInfoById != null)
                    {
                        this.GroupName.Text = groupInfoById.GroupName;
                        this.resultDiv.Visible = false;
                    }
                    else
                    {
                        this.ShowMsgAndReUrl("参数错误！", false, "CustomDistributorList.aspx");
                    }
                }
            }
        }
    }
}

