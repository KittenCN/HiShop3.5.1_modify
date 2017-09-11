namespace Hidistro.UI.Web.Admin.promotion
{
   using  global:: ControlPanel.Promotions;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Promotions;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Data;
    using System.Web.UI.WebControls;

    public class PrizeDeliveryDetail : AdminPage
    {
        private string Did;
        protected Literal txtAddress;
        protected Literal txtCourierNumber;
        protected Literal txtDeliever;
        protected Literal txtDTel;
        protected Literal txtExpressName;
        protected Literal txtGameTitle;
        protected Literal txtGameType;
        protected ListImage txtImage;
        protected Literal txtPlayTime;
        protected Literal txtPrizeGrade;
        protected Literal txtProductName;
        protected Literal txtReceiver;
        protected Literal txtRegionName;
        protected Literal txtStatus;
        protected Literal txtTel;

        protected PrizeDeliveryDetail() : base("m08", "yxp16")
        {
            this.Did = "";
        }

        private string Dbnull2str(object data)
        {
            return ((data == DBNull.Value) ? "" : data.ToString());
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Did = base.Request.QueryString["LogId"];
            string str = Globals.RequestQueryStr("pid");
            if (!string.IsNullOrEmpty(this.Did))
            {
                PrizesDeliveQuery query = new PrizesDeliveQuery {
                    Status = -1,
                    PageIndex = 1,
                    PageSize = 2,
                    PrizeType = -1
                };
                DbQueryResult result = null;
                if (!string.IsNullOrEmpty(str) && (str != "0"))
                {
                    query.Pid = str;
                    result = GameHelper.GetAllPrizesDeliveryList(query, "", "*");
                }
                else
                {
                    query.LogId = this.Did;
                    query.SortBy = "LogId";
                    result = GameHelper.GetPrizesDeliveryList(query, "", "*");
                }
                DataTable data = (DataTable) result.Data;
                if ((data != null) && (data.Rows.Count > 0))
                {
                    DataRow row = data.Rows[0];
                    this.txtStatus.Text = GameHelper.GetPrizesDeliveStatus(this.Dbnull2str(row["status"]), this.Dbnull2str(row["IsLogistics"]), this.Dbnull2str(row["PrizeType"]), row["gametype"].ToString());
                    this.txtCourierNumber.Text = this.Dbnull2str(row["CourierNumber"]);
                    this.txtExpressName.Text = this.Dbnull2str(row["ExpressName"]);
                    this.txtTel.Text = this.Dbnull2str(row["Tel"]);
                    this.txtDTel.Text = this.Dbnull2str(row["Tel"]);
                    this.txtReceiver.Text = this.Dbnull2str(row["Receiver"]);
                    this.txtDeliever.Text = this.Dbnull2str(row["Receiver"]);
                    this.txtAddress.Text = this.Dbnull2str(row["Address"]);
                    this.txtRegionName.Text = this.Dbnull2str(row["ReggionPath"]);
                    this.txtImage.ImageUrl = (row["ThumbnailUrl100"] == DBNull.Value) ? "/utility/pics/none.gif" : row["ThumbnailUrl100"].ToString();
                    if (this.txtRegionName.Text.Trim() != "")
                    {
                        string[] strArray = this.txtRegionName.Text.Trim().Split(new char[] { ',' });
                        this.txtRegionName.Text = RegionHelper.GetFullRegion(int.Parse(strArray[strArray.Length - 1]), ",");
                    }
                    if (!string.IsNullOrEmpty(str) && (str != "0"))
                    {
                        if (this.txtTel.Text == "")
                        {
                            this.txtTel.Text = this.Dbnull2str(row["Tel"]);
                            this.txtDTel.Text = this.Dbnull2str(row["Tel"]);
                        }
                        this.txtPlayTime.Text = ((DateTime) row["WinTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                        this.txtGameTitle.Text = row["Title"].ToString();
                    }
                    else
                    {
                        if (((this.txtImage.ImageUrl == "/utility/pics/none.gif") && (row["PrizeId"] != DBNull.Value)) && (Globals.ToNum(row["PrizeId"]) > 0))
                        {
                            int prizeId = Globals.ToNum(row["PrizeId"]);
                            if (prizeId > 0)
                            {
                                GamePrizeInfo gamePrizeInfoById = GameHelper.GetGamePrizeInfoById(prizeId);
                                if (gamePrizeInfoById != null)
                                {
                                    this.txtImage.ImageUrl = gamePrizeInfoById.PrizeImage;
                                }
                            }
                        }
                        if (this.txtDeliever.Text == "")
                        {
                            this.txtReceiver.Text = this.Dbnull2str(row["RealName"]);
                            this.txtDeliever.Text = this.Dbnull2str(row["RealName"]);
                        }
                        if (this.txtTel.Text == "")
                        {
                            this.txtTel.Text = this.Dbnull2str(row["CellPhone"]);
                            this.txtDTel.Text = this.Dbnull2str(row["CellPhone"]);
                        }
                        this.txtGameTitle.Text = row["GameTitle"].ToString();
                        this.txtPlayTime.Text = ((DateTime) row["PlayTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    this.txtPrizeGrade.Text = GameHelper.GetPrizeGradeName(this.Dbnull2str(row["PrizeGrade"]));
                    this.txtGameType.Text = GameHelper.GetGameTypeName(row["GameType"].ToString());
                    this.txtProductName.Text = row["ProductName"].ToString();
                }
            }
        }
    }
}

