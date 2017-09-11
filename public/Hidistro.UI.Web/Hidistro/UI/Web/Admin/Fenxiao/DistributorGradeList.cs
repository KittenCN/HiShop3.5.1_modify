namespace Hidistro.UI.Web.Admin.Fenxiao
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Members;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data;
    using System.Web.UI.WebControls;

    public class DistributorGradeList : AdminPage
    {
        private Dictionary<int, int> GradeCountDic;
        protected string LocalUrl;
        private string Name;
        protected Repeater rptList;

        protected DistributorGradeList() : base("m05", "fxp04")
        {
            this.LocalUrl = string.Empty;
            this.Name = "";
        }

        private void BindData()
        {
            DistributorGradeQuery entity = new DistributorGradeQuery {
                Name = this.Name,
                SortBy = "GradeID",
                SortOrder = SortAction.Asc
            };
            Globals.EntityCoding(entity, true);
            entity.PageIndex = 1;
            entity.PageSize = 100;
            DbQueryResult distributorGradeRequest = DistributorGradeBrower.GetDistributorGradeRequest(entity);
            if (this.GradeCountDic == null)
            {
                this.GradeCountDic = DistributorGradeBrower.GetGradeCount("0");
            }
            this.rptList.DataSource = distributorGradeRequest.Data;
            this.rptList.DataBind();
        }

        protected string FormatCommissionRise(object commissionrise)
        {
            decimal result = 0.00M;
            decimal.TryParse(commissionrise.ToString(), out result);
            if (result == 0.00M)
            {
                return "-";
            }
            return ("+" + result + "%");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                this.BindData();
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("Name", "");
            queryStrings.Add("pageSize", "100");
            queryStrings.Add("pageIndex", "1");
            base.ReloadPage(queryStrings);
        }

        protected void rptList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string str2;
            int result = 0;
            int.TryParse(e.CommandArgument.ToString(), out result);
            if ((result > 0) && ((str2 = e.CommandName) != null))
            {
                if (!(str2 == "setdefault"))
                {
                    if (!(str2 == "del"))
                    {
                        return;
                    }
                }
                else
                {
                    DistributorGradeBrower.SetGradeDefault(result);
                    this.ReBind(true);
                    return;
                }
                switch (DistributorGradeBrower.DelOneGrade(result))
                {
                    case "-1":
                        this.ShowMsg("不能删除，因为该等级下面已经有分销商！", false);
                        return;

                    case "1":
                        this.ShowMsg("分销商等级删除成功！", true);
                        this.BindData();
                        return;
                }
                this.ShowMsg("删除失败", false);
            }
        }

        protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                Button button = (Button) e.Item.FindControl("lbtnDel");
                Literal literal = (Literal) e.Item.FindControl("GradeSum");
                literal.Text = ((DataRowView) e.Item.DataItem).Row["GradeId"].ToString();
                int key = int.Parse(literal.Text);
                if (this.GradeCountDic.ContainsKey(key))
                {
                    literal.Text = this.GradeCountDic[key].ToString();
                }
                else
                {
                    literal.Text = "0";
                }
                if (((DataRowView) e.Item.DataItem).Row["IsDefault"].ToString() != "False")
                {
                    button.Enabled = false;
                    button.Visible = false;
                }
            }
        }
    }
}

