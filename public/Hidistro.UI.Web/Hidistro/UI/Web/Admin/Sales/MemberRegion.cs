namespace Hidistro.UI.Web.Admin.Sales
{
    using Hidistro.ControlPanel.Members;
    using Hidistro.ControlPanel.VShop;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.StatisticsReport;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Data;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class MemberRegion : AdminPage
    {
        protected HtmlForm form1;
        public int MaxQty;
        public string QtyList1;
        protected Repeater rptList;

        protected MemberRegion() : base("m10", "tjp07")
        {
            this.QtyList1 = "";
        }

        private void BindData()
        {
            OrderStatisticsQuery entity = new OrderStatisticsQuery {
                PageIndex = 1,
                PageSize = 50,
                SortOrder = SortAction.Desc,
                SortBy = "TotalRec"
            };
            Globals.EntityCoding(entity, true);
            DbQueryResult result = ShopStatisticHelper.Member_GetRegionReport(entity);
            int num = 0;
            DataTable table = ((DataTable) result.Data).Clone();
            foreach (DataRow row in ((DataTable) result.Data).Rows)
            {
                num++;
                DataRow row2 = table.NewRow();
                row2["RegionName"] = row["RegionName"];
                row2["TotalRec"] = row["TotalRec"];
                table.Rows.Add(row2);
                if (num == 1)
                {
                    this.MaxQty = int.Parse(row2["TotalRec"].ToString());
                }
                if (num >= 10)
                {
                    break;
                }
            }
            this.rptList.DataSource = table;
            this.rptList.DataBind();
            this.QtyList1 = "";
            num = 0;
            int count = ((DataTable) result.Data).Rows.Count;
            foreach (DataRow row3 in ((DataTable) result.Data).Rows)
            {
                this.QtyList1 = this.QtyList1 + "{name: '" + base.GetFieldValue(row3, "RegionName") + "',value: " + base.GetFieldIntValue(row3, "TotalRec").ToString() + "}";
                if (num < (count - 1))
                {
                    this.QtyList1 = this.QtyList1 + ",\n";
                }
                num++;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                this.UpdateMemberTopRegion();
                this.BindData();
            }
        }

        private void UpdateMemberTopRegion()
        {
            DataTable table = MemberHelper.GetTop50NotTopRegionIdBind();
            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    MemberInfo member = MemberHelper.GetMember(Globals.ToNum(table.Rows[i]["UserID"]));
                    if (member != null)
                    {
                        member.TopRegionId = RegionHelper.GetTopRegionId(member.RegionId);
                        MemberHelper.Update(member);
                    }
                }
            }
        }
    }
}

