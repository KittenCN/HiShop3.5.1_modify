namespace Hidistro.UI.Web.Admin.Shop
{
    using Hidistro.ControlPanel.VShop;
    using Hidistro.Entities.StatisticsReport;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Data;

    public class MemberGaiKuang : AdminPage
    {
        public string ActiveUserQty;
        public string CartUserQty;
        public string CollectUserQty;
        public string MemberGradeList;
        public string MemberQty;
        public string PotentialUserQty;
        public string PotentialUserQty_Yesterday;
        public string QtyList_Grade;
        public string RegionNameList;
        public string RegionQtyList;
        public string SleepUserQty;
        public string SuccessTradeUserQty;
        public string SuccessTradeUserQty_Yesterday;

        protected MemberGaiKuang() : base("m04", "hyp01")
        {
            this.ActiveUserQty = "0";
            this.SleepUserQty = "0";
            this.SuccessTradeUserQty = "0";
            this.SuccessTradeUserQty_Yesterday = "0";
            this.PotentialUserQty = "0";
            this.PotentialUserQty_Yesterday = "0";
            this.CollectUserQty = "0";
            this.CartUserQty = "0";
            this.MemberQty = "0";
            this.MemberGradeList = "";
            this.QtyList_Grade = "";
            this.RegionNameList = "";
            this.RegionQtyList = "";
        }

        private void LoadData()
        {
            DataRow drOne = ShopStatisticHelper.MemberGlobal_GetCountInfo();
            if (drOne != null)
            {
                this.ActiveUserQty = base.GetFieldValue(drOne, "ActiveUserQty");
                this.SleepUserQty = base.GetFieldValue(drOne, "SleepUserQty");
                this.SuccessTradeUserQty = base.GetFieldValue(drOne, "SuccessTradeUserQty");
                this.SuccessTradeUserQty_Yesterday = base.GetFieldValue(drOne, "SuccessTradeUserQty_Yesterday");
                this.PotentialUserQty = base.GetFieldValue(drOne, "PotentialUserQty");
                this.PotentialUserQty_Yesterday = base.GetFieldValue(drOne, "PotentialUserQty_Yesterday");
                this.CollectUserQty = base.GetFieldValue(drOne, "CollectUserQty");
                this.CartUserQty = base.GetFieldValue(drOne, "CartUserQty");
                DataRow row2 = ShopStatisticHelper.ShopGlobal_GetMemberCount();
                this.MemberQty = base.GetFieldValue(row2, "MemberQty");
            }
            DataTable table = ShopStatisticHelper.MemberGlobal_GetStatisticList(1);
            DataTable table2 = ShopStatisticHelper.MemberGlobal_GetStatisticList(2);
            this.MemberGradeList = "";
            int num = 0;
            int count = table.Rows.Count;
            foreach (DataRow row3 in table.Rows)
            {
                this.MemberGradeList = this.MemberGradeList + "'" + base.GetFieldValue(row3, "Name") + "'";
                this.QtyList_Grade = this.QtyList_Grade + "{" + string.Format("value:{0}, name:'{1}'", base.GetFieldValue(row3, "Total"), base.GetFieldValue(row3, "Name")) + "}";
                if (num < (count - 1))
                {
                    this.MemberGradeList = this.MemberGradeList + ",";
                    this.QtyList_Grade = this.QtyList_Grade + ",";
                }
                this.QtyList_Grade = this.QtyList_Grade + "\n";
                num++;
            }
            DataView defaultView = table2.DefaultView;
            defaultView.Sort = "Total Desc";
            table2 = defaultView.ToTable();
            this.RegionNameList = "";
            this.RegionQtyList = "";
            num = 0;
            count = table2.Rows.Count;
            if (count > 9)
            {
                count = 9;
            }
            foreach (DataRow row4 in table2.Rows)
            {
                if (num >= 9)
                {
                    break;
                }
                this.RegionNameList = this.RegionNameList + "'" + base.GetFieldValue(row4, "RegionName") + "'";
                this.RegionQtyList = this.RegionQtyList + base.GetFieldValue(row4, "Total");
                if (num < (count - 1))
                {
                    this.RegionNameList = this.RegionNameList + ",";
                    this.RegionQtyList = this.RegionQtyList + ",";
                }
                num++;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                string retInfo = "";
                ShopStatisticHelper.StatisticsOrdersByRecDate(DateTime.Today, UpdateAction.AllUpdate, 0, out retInfo);
                this.LoadData();
            }
        }
    }
}

