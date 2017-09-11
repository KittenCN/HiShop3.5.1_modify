namespace Hidistro.SqlDal.Orders
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Orders;
    using Hidistro.Entities.Promotions;
    using Hidistro.SqlDal.Commodities;
    using Hidistro.SqlDal.Members;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;

    public class OrderDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool AddMemberPointNumber(int PointNumber, OrderInfo orderInfo, DbTransaction dbTran)
        {
            IntegralDetailInfo point = new IntegralDetailInfo {
                IntegralChange = PointNumber,
                IntegralSource = "获取积分-订单号：" + orderInfo.OrderId,
                IntegralSourceType = 1
            };
            string activitiesName = orderInfo.ActivitiesName;
            if (!string.IsNullOrEmpty(activitiesName))
            {
                point.Remark = "活动送积分：" + activitiesName;
            }
            else
            {
                point.Remark = "购物获取积分";
            }
            point.Userid = orderInfo.UserId;
            point.GoToUrl = Globals.ApplicationPath + "/Vshop/MemberOrderDetails.aspx?OrderId=" + orderInfo.OrderId;
            point.IntegralStatus = Convert.ToInt32(IntegralDetailStatus.OrderToIntegral);
            if (!new IntegralDetailDao().AddIntegralDetail(point, dbTran))
            {
                dbTran.Rollback();
                return false;
            }
            return true;
        }

        private string BuildOrdersQuery(OrderQuery query, int? distributorUserId)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT OrderId FROM Hishop_Orders WHERE 1=1 and userid!=0 ");
            if ((query.OrderId != string.Empty) && (query.OrderId != null))
            {
                builder.AppendFormat(" AND OrderId = '{0}'", DataHelper.CleanSearchString(query.OrderId));
            }
            if ((query.ShipId != string.Empty) && (query.ShipId != null))
            {
                builder.AppendFormat(" AND ShipOrderNumber = '{0}'", DataHelper.CleanSearchString(query.ShipId));
            }
            if (query.StartDate.HasValue)
            {
                builder.AppendFormat(" and orderDate is not null AND datediff(ss,'{0}',orderDate)>=0", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
            }
            if (query.EndDate.HasValue)
            {
                builder.AppendFormat(" and orderDate is not null AND datediff(ss,'{0}',orderDate)<=0", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
            }
            if (query.PaymentType.HasValue)
            {
                builder.AppendFormat(" AND PaymentTypeId = '{0}'", query.PaymentType.Value);
            }
            if (query.GroupBuyId.HasValue)
            {
                builder.AppendFormat(" AND GroupBuyId = {0}", query.GroupBuyId.Value);
            }
            if (!string.IsNullOrEmpty(query.ProductName))
            {
                builder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Hishop_OrderItems WHERE ItemDescription LIKE '%{0}%')", DataHelper.CleanSearchString(query.ProductName));
            }
            if (!string.IsNullOrEmpty(query.ShipTo))
            {
                builder.AppendFormat(" AND ShipTo LIKE '%{0}%'", DataHelper.CleanSearchString(query.ShipTo));
            }
            if (query.RegionId.HasValue)
            {
                builder.AppendFormat(" AND ShippingRegion like '%{0}%'", DataHelper.CleanSearchString(RegionHelper.GetFullRegion(query.RegionId.Value, "，")));
            }
            if (!string.IsNullOrEmpty(query.UserName))
            {
                builder.AppendFormat(" AND  UserName  = '{0}' ", DataHelper.CleanSearchString(query.UserName));
            }
            if (query.Status == OrderStatus.History)
            {
                builder.AppendFormat(" AND OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2} AND OrderDate < '{3}'", new object[] { 1, 4, 9, DateTime.Now.AddMonths(-3) });
            }
            else if (query.Status != OrderStatus.All)
            {
                if (query.Status == OrderStatus.BuyerAlreadyPaid)
                {
                    builder.AppendFormat(" AND (OrderStatus = {0} OR (OrderStatus = {1} AND Gateway = 'hishop.plugins.payment.podrequest'))", 2, 1);
                }
                else
                {
                    builder.AppendFormat(" AND OrderStatus = {0}", (int) query.Status);
                }
            }
            if (query.StartDate.HasValue)
            {
                builder.AppendFormat(" AND datediff(ss,'{0}',UpdateDate)>=0", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
            }
            if (query.EndDate.HasValue)
            {
                builder.AppendFormat(" AND datediff(ss,'{0}',UpdateDate)<=0", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
            }
            if (query.ShippingModeId.HasValue)
            {
                builder.AppendFormat(" AND ShippingModeId = {0}", query.ShippingModeId.Value);
            }
            if (query.IsPrinted.HasValue)
            {
                builder.AppendFormat(" AND ISNULL(IsPrinted, 0)={0}", query.IsPrinted.Value);
            }
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                builder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
            }
            return builder.ToString();
        }

        public bool CheckRefund(string orderId, string Operator, string adminRemark, int refundType, bool accept)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("UPDATE Hishop_Orders SET OrderStatus = @OrderStatus WHERE OrderId = @OrderId;");
            builder.Append(" update Hishop_OrderRefund set Operator=@Operator,AdminRemark=@AdminRemark,HandleStatus=@HandleStatus,HandleTime=@HandleTime where HandleStatus=0 and OrderId = @OrderId;");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            if (accept)
            {
                this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 9);
                this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 1);
            }
            else
            {
                this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, 2);
                this.database.AddInParameter(sqlStringCommand, "HandleStatus", DbType.Int32, 2);
            }
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "Operator", DbType.String, Operator);
            this.database.AddInParameter(sqlStringCommand, "AdminRemark", DbType.String, adminRemark);
            this.database.AddInParameter(sqlStringCommand, "HandleTime", DbType.DateTime, DateTime.Now);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool CombineOrderToPay(string orderIds, string orderMarking)
        {
            if (!string.IsNullOrEmpty(orderIds) && !orderIds.Contains("'"))
            {
                orderIds = "'" + orderIds.Replace(",", "','") + "'";
            }
            string query = string.Format("update Hishop_Orders set OrderMarking=@OrderMarking WHERE OrderId IN({0})", orderIds);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "OrderMarking", DbType.String, orderMarking);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool CreatOrder(OrderInfo orderInfo, DbTransaction dbTran)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_CreateOrder");
            this.database.AddInParameter(storedProcCommand, "OrderId", DbType.String, orderInfo.OrderId);
            this.database.AddInParameter(storedProcCommand, "OrderMarking", DbType.String, orderInfo.OrderMarking);
            this.database.AddInParameter(storedProcCommand, "OrderDate", DbType.DateTime, orderInfo.OrderDate);
            this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, orderInfo.UserId);
            this.database.AddInParameter(storedProcCommand, "UserName", DbType.String, orderInfo.Username);
            this.database.AddInParameter(storedProcCommand, "Wangwang", DbType.String, orderInfo.Wangwang);
            this.database.AddInParameter(storedProcCommand, "RealName", DbType.String, orderInfo.RealName);
            this.database.AddInParameter(storedProcCommand, "EmailAddress", DbType.String, orderInfo.EmailAddress);
            this.database.AddInParameter(storedProcCommand, "Remark", DbType.String, orderInfo.Remark);
            this.database.AddInParameter(storedProcCommand, "ClientShortType", DbType.Int32, orderInfo.ClientShortType);
            this.database.AddInParameter(storedProcCommand, "AdjustedDiscount", DbType.Currency, orderInfo.AdjustedDiscount);
            this.database.AddInParameter(storedProcCommand, "OrderStatus", DbType.Int32, (int) orderInfo.OrderStatus);
            this.database.AddInParameter(storedProcCommand, "ShippingRegion", DbType.String, orderInfo.ShippingRegion);
            this.database.AddInParameter(storedProcCommand, "Address", DbType.String, orderInfo.Address);
            this.database.AddInParameter(storedProcCommand, "ZipCode", DbType.String, orderInfo.ZipCode);
            this.database.AddInParameter(storedProcCommand, "ShipTo", DbType.String, orderInfo.ShipTo);
            this.database.AddInParameter(storedProcCommand, "TelPhone", DbType.String, orderInfo.TelPhone);
            this.database.AddInParameter(storedProcCommand, "CellPhone", DbType.String, orderInfo.CellPhone);
            this.database.AddInParameter(storedProcCommand, "ShipToDate", DbType.String, orderInfo.ShipToDate);
            this.database.AddInParameter(storedProcCommand, "ShippingModeId", DbType.Int32, orderInfo.ShippingModeId);
            this.database.AddInParameter(storedProcCommand, "ModeName", DbType.String, orderInfo.ModeName);
            this.database.AddInParameter(storedProcCommand, "RegionId", DbType.Int32, orderInfo.RegionId);
            this.database.AddInParameter(storedProcCommand, "Freight", DbType.Currency, orderInfo.Freight);
            this.database.AddInParameter(storedProcCommand, "AdjustedFreight", DbType.Currency, orderInfo.AdjustedFreight);
            this.database.AddInParameter(storedProcCommand, "ShipOrderNumber", DbType.String, orderInfo.ShipOrderNumber);
            this.database.AddInParameter(storedProcCommand, "Weight", DbType.Int32, orderInfo.Weight);
            this.database.AddInParameter(storedProcCommand, "ExpressCompanyName", DbType.String, orderInfo.ExpressCompanyName);
            this.database.AddInParameter(storedProcCommand, "ExpressCompanyAbb", DbType.String, orderInfo.ExpressCompanyAbb);
            this.database.AddInParameter(storedProcCommand, "PaymentTypeId", DbType.Int32, orderInfo.PaymentTypeId);
            this.database.AddInParameter(storedProcCommand, "PaymentType", DbType.String, orderInfo.PaymentType);
            this.database.AddInParameter(storedProcCommand, "PayCharge", DbType.Currency, orderInfo.PayCharge);
            this.database.AddInParameter(storedProcCommand, "RefundStatus", DbType.Int32, (int) orderInfo.RefundStatus);
            this.database.AddInParameter(storedProcCommand, "Gateway", DbType.String, orderInfo.Gateway);
            this.database.AddInParameter(storedProcCommand, "OrderTotal", DbType.Currency, orderInfo.GetTotal());
            this.database.AddInParameter(storedProcCommand, "OrderPoint", DbType.Int32, orderInfo.Points);
            this.database.AddInParameter(storedProcCommand, "OrderCostPrice", DbType.Currency, orderInfo.GetCostPrice());
            this.database.AddInParameter(storedProcCommand, "OrderProfit", DbType.Currency, orderInfo.GetProfit());
            this.database.AddInParameter(storedProcCommand, "Amount", DbType.Currency, orderInfo.GetAmount());
            this.database.AddInParameter(storedProcCommand, "ReducedPromotionId", DbType.Int32, orderInfo.ReducedPromotionId);
            this.database.AddInParameter(storedProcCommand, "ReducedPromotionName", DbType.String, orderInfo.ReducedPromotionName);
            this.database.AddInParameter(storedProcCommand, "ReducedPromotionAmount", DbType.Currency, orderInfo.ReducedPromotionAmount);
            this.database.AddInParameter(storedProcCommand, "IsReduced", DbType.Boolean, orderInfo.IsReduced);
            this.database.AddInParameter(storedProcCommand, "SentTimesPointPromotionId", DbType.Int32, orderInfo.SentTimesPointPromotionId);
            this.database.AddInParameter(storedProcCommand, "SentTimesPointPromotionName", DbType.String, orderInfo.SentTimesPointPromotionName);
            this.database.AddInParameter(storedProcCommand, "TimesPoint", DbType.Currency, orderInfo.TimesPoint);
            this.database.AddInParameter(storedProcCommand, "IsSendTimesPoint", DbType.Boolean, orderInfo.IsSendTimesPoint);
            this.database.AddInParameter(storedProcCommand, "FreightFreePromotionId", DbType.Int32, orderInfo.FreightFreePromotionId);
            this.database.AddInParameter(storedProcCommand, "FreightFreePromotionName", DbType.String, orderInfo.FreightFreePromotionName);
            this.database.AddInParameter(storedProcCommand, "IsFreightFree", DbType.Boolean, orderInfo.IsFreightFree);
            this.database.AddInParameter(storedProcCommand, "CouponName", DbType.String, orderInfo.CouponName);
            this.database.AddInParameter(storedProcCommand, "CouponCode", DbType.String, orderInfo.CouponCode);
            this.database.AddInParameter(storedProcCommand, "CouponAmount", DbType.Currency, orderInfo.CouponAmount);
            this.database.AddInParameter(storedProcCommand, "CouponValue", DbType.Currency, orderInfo.CouponValue);
            this.database.AddInParameter(storedProcCommand, "RedPagerActivityName", DbType.String, orderInfo.RedPagerActivityName);
            this.database.AddInParameter(storedProcCommand, "RedPagerID", DbType.String, orderInfo.RedPagerID);
            this.database.AddInParameter(storedProcCommand, "RedPagerOrderAmountCanUse", DbType.Currency, orderInfo.RedPagerOrderAmountCanUse);
            this.database.AddInParameter(storedProcCommand, "RedPagerAmount", DbType.Currency, orderInfo.RedPagerAmount);
            if (orderInfo.GroupBuyId > 0)
            {
                this.database.AddInParameter(storedProcCommand, "GroupBuyId", DbType.Int32, orderInfo.GroupBuyId);
                this.database.AddInParameter(storedProcCommand, "NeedPrice", DbType.Currency, orderInfo.NeedPrice);
                this.database.AddInParameter(storedProcCommand, "GroupBuyStatus", DbType.Int32, 1);
            }
            else
            {
                this.database.AddInParameter(storedProcCommand, "GroupBuyId", DbType.Int32, DBNull.Value);
                this.database.AddInParameter(storedProcCommand, "NeedPrice", DbType.Currency, DBNull.Value);
                this.database.AddInParameter(storedProcCommand, "GroupBuyStatus", DbType.Int32, DBNull.Value);
            }
            if (orderInfo.CountDownBuyId > 0)
            {
                this.database.AddInParameter(storedProcCommand, "CountDownBuyId ", DbType.Int32, orderInfo.CountDownBuyId);
            }
            else
            {
                this.database.AddInParameter(storedProcCommand, "CountDownBuyId ", DbType.Int32, DBNull.Value);
            }
            if (orderInfo.BundlingID > 0)
            {
                this.database.AddInParameter(storedProcCommand, "BundlingID ", DbType.Int32, orderInfo.BundlingID);
                this.database.AddInParameter(storedProcCommand, "BundlingPrice", DbType.Currency, orderInfo.BundlingPrice);
            }
            else
            {
                this.database.AddInParameter(storedProcCommand, "BundlingID ", DbType.Int32, DBNull.Value);
                this.database.AddInParameter(storedProcCommand, "BundlingPrice", DbType.Currency, DBNull.Value);
            }
            this.database.AddInParameter(storedProcCommand, "Tax", DbType.Currency, orderInfo.Tax);
            this.database.AddInParameter(storedProcCommand, "InvoiceTitle", DbType.String, orderInfo.InvoiceTitle);
            this.database.AddInParameter(storedProcCommand, "ReferralUserId", DbType.Int32, orderInfo.ReferralUserId);
            this.database.AddInParameter(storedProcCommand, "ReferralPath", DbType.String, orderInfo.ReferralPath);
            this.database.AddInParameter(storedProcCommand, "DiscountAmount", DbType.Decimal, orderInfo.DiscountAmount);
            this.database.AddInParameter(storedProcCommand, "ActivitiesId", DbType.String, orderInfo.ActivitiesId);
            this.database.AddInParameter(storedProcCommand, "ActivitiesName", DbType.String, orderInfo.ActivitiesName);
            this.database.AddInParameter(storedProcCommand, "FirstCommission", DbType.Decimal, orderInfo.FirstCommission);
            this.database.AddInParameter(storedProcCommand, "SecondCommission", DbType.Decimal, orderInfo.SecondCommission);
            this.database.AddInParameter(storedProcCommand, "ThirdCommission", DbType.Decimal, orderInfo.ThirdCommission);
            this.database.AddInParameter(storedProcCommand, "PointToCash", DbType.Decimal, orderInfo.PointToCash);
            this.database.AddInParameter(storedProcCommand, "PointExchange", DbType.Int32, orderInfo.PointExchange);
            this.database.AddInParameter(storedProcCommand, "BargainDetialId", DbType.Int32, orderInfo.BargainDetialId);
            this.database.AddInParameter(storedProcCommand, "CouponFreightMoneyTotal", DbType.Decimal, orderInfo.CouponFreightMoneyTotal);
            this.database.AddInParameter(storedProcCommand, "LogisticsTools", DbType.Int32, (int) orderInfo.logisticsTools);
            return (this.database.ExecuteNonQuery(storedProcCommand, dbTran) > 0);
        }




        /// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(OrderTmpInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Hishop_Orders(");
            strSql.Append("OrderId,OrderMarking,Remark,ManagerMark,ManagerRemark,AdjustedDiscount,OrderStatus,CloseReason,OrderDate,PayDate,ShippingDate,FinishDate,UserId,Username,EmailAddress,RealName,QQ,Wangwang,MSN,ShippingRegion,Address,ZipCode,ShipTo,TelPhone,CellPhone,ShipToDate,ShippingModeId,ModeName,RealShippingModeId,RealModeName,RegionId,Freight,AdjustedFreight,ShipOrderNumber,Weight,ExpressCompanyName,ExpressCompanyAbb,PaymentTypeId,PaymentType,PayCharge,RefundStatus,RefundAmount,RefundRemark,Gateway,OrderTotal,OrderPoint,OrderCostPrice,OrderProfit,ActualFreight,OtherCost,OptionPrice,Amount,DiscountAmount,ActivitiesId,ActivitiesName,ReducedPromotionId,ReducedPromotionName,ReducedPromotionAmount,IsReduced,SentTimesPointPromotionId,SentTimesPointPromotionName,TimesPoint,IsSendTimesPoint,FreightFreePromotionId,FreightFreePromotionName,IsFreightFree,CouponName,CouponCode,CouponAmount,CouponValue,GroupBuyId,NeedPrice,GroupBuyStatus,CountDownBuyId,BundlingId,BundlingNum,BundlingPrice,GatewayOrderId,IsPrinted,Tax,InvoiceTitle,Sender,ReferralUserId,FirstCommission,SecondCommission,ThirdCommission,RedPagerActivityName,RedPagerID,RedPagerOrderAmountCanUse,RedPagerAmount,OldAddress,PointToCash,PointExchange,SplitState,DeleteBeforeState,ClientShortType,ReferralPath,BargainDetialId,BalancePayMoneyTotal,BalancePayFreightMoneyTotal,CouponFreightMoneyTotal,UpdateDate,LogisticsTools)");

            strSql.Append(" values (");
            strSql.Append("@OrderId,@OrderMarking,@Remark,@ManagerMark,@ManagerRemark,@AdjustedDiscount,@OrderStatus,@CloseReason,@OrderDate,@PayDate,@ShippingDate,@FinishDate,@UserId,@Username,@EmailAddress,@RealName,@QQ,@Wangwang,@MSN,@ShippingRegion,@Address,@ZipCode,@ShipTo,@TelPhone,@CellPhone,@ShipToDate,@ShippingModeId,@ModeName,@RealShippingModeId,@RealModeName,@RegionId,@Freight,@AdjustedFreight,@ShipOrderNumber,@Weight,@ExpressCompanyName,@ExpressCompanyAbb,@PaymentTypeId,@PaymentType,@PayCharge,@RefundStatus,@RefundAmount,@RefundRemark,@Gateway,@OrderTotal,@OrderPoint,@OrderCostPrice,@OrderProfit,@ActualFreight,@OtherCost,@OptionPrice,@Amount,@DiscountAmount,@ActivitiesId,@ActivitiesName,@ReducedPromotionId,@ReducedPromotionName,@ReducedPromotionAmount,@IsReduced,@SentTimesPointPromotionId,@SentTimesPointPromotionName,@TimesPoint,@IsSendTimesPoint,@FreightFreePromotionId,@FreightFreePromotionName,@IsFreightFree,@CouponName,@CouponCode,@CouponAmount,@CouponValue,@GroupBuyId,@NeedPrice,@GroupBuyStatus,@CountDownBuyId,@BundlingId,@BundlingNum,@BundlingPrice,@GatewayOrderId,@IsPrinted,@Tax,@InvoiceTitle,@Sender,@ReferralUserId,@FirstCommission,@SecondCommission,@ThirdCommission,@RedPagerActivityName,@RedPagerID,@RedPagerOrderAmountCanUse,@RedPagerAmount,@OldAddress,@PointToCash,@PointExchange,@SplitState,@DeleteBeforeState,@ClientShortType,@ReferralPath,@BargainDetialId,@BalancePayMoneyTotal,@BalancePayFreightMoneyTotal,@CouponFreightMoneyTotal,@UpdateDate,@LogisticsTools)");
           
            DbCommand dbCommand = database.GetSqlStringCommand(strSql.ToString());
            database.AddInParameter(dbCommand, "OrderId", DbType.String, model.OrderId);
            database.AddInParameter(dbCommand, "OrderMarking", DbType.String, model.OrderMarking);
            database.AddInParameter(dbCommand, "Remark", DbType.String, model.Remark);
            database.AddInParameter(dbCommand, "ManagerMark", DbType.Int32, model.ManagerMark);
            database.AddInParameter(dbCommand, "ManagerRemark", DbType.String, model.ManagerRemark);
            database.AddInParameter(dbCommand, "AdjustedDiscount", DbType.Currency, model.AdjustedDiscount);
            database.AddInParameter(dbCommand, "OrderStatus", DbType.Int32, model.OrderStatus);
            database.AddInParameter(dbCommand, "CloseReason", DbType.String, model.CloseReason);
            database.AddInParameter(dbCommand, "OrderDate", DbType.DateTime, model.OrderDate);
            database.AddInParameter(dbCommand, "PayDate", DbType.DateTime, model.PayDate);
            database.AddInParameter(dbCommand, "ShippingDate", DbType.DateTime, model.ShippingDate);
            database.AddInParameter(dbCommand, "FinishDate", DbType.DateTime, model.FinishDate);
            database.AddInParameter(dbCommand, "UserId", DbType.Int32, model.UserId);
            database.AddInParameter(dbCommand, "Username", DbType.String, model.Username);
            database.AddInParameter(dbCommand, "EmailAddress", DbType.String, model.EmailAddress);
            database.AddInParameter(dbCommand, "RealName", DbType.String, model.RealName);
            database.AddInParameter(dbCommand, "QQ", DbType.String, model.QQ);
            database.AddInParameter(dbCommand, "Wangwang", DbType.String, model.Wangwang);
            database.AddInParameter(dbCommand, "MSN", DbType.String, model.MSN);
            database.AddInParameter(dbCommand, "ShippingRegion", DbType.String, model.ShippingRegion);
            database.AddInParameter(dbCommand, "Address", DbType.String, model.Address);
            database.AddInParameter(dbCommand, "ZipCode", DbType.String, model.ZipCode);
            database.AddInParameter(dbCommand, "ShipTo", DbType.String, model.ShipTo);
            database.AddInParameter(dbCommand, "TelPhone", DbType.String, model.TelPhone);
            database.AddInParameter(dbCommand, "CellPhone", DbType.String, model.CellPhone);
            database.AddInParameter(dbCommand, "ShipToDate", DbType.String, model.ShipToDate);
            database.AddInParameter(dbCommand, "ShippingModeId", DbType.Int32, model.ShippingModeId);
            database.AddInParameter(dbCommand, "ModeName", DbType.String, model.ModeName);
            database.AddInParameter(dbCommand, "RealShippingModeId", DbType.Int32, model.RealShippingModeId);
            database.AddInParameter(dbCommand, "RealModeName", DbType.String, model.RealModeName);
            database.AddInParameter(dbCommand, "RegionId", DbType.Int32, model.RegionId);
            database.AddInParameter(dbCommand, "Freight", DbType.Currency, model.Freight);
            database.AddInParameter(dbCommand, "AdjustedFreight", DbType.Currency, model.AdjustedFreight);
            database.AddInParameter(dbCommand, "ShipOrderNumber", DbType.String, model.ShipOrderNumber);
            database.AddInParameter(dbCommand, "Weight", DbType.Currency, model.Weight);
            database.AddInParameter(dbCommand, "ExpressCompanyName", DbType.String, model.ExpressCompanyName);
            database.AddInParameter(dbCommand, "ExpressCompanyAbb", DbType.String, model.ExpressCompanyAbb);
            database.AddInParameter(dbCommand, "PaymentTypeId", DbType.Int32, model.PaymentTypeId);
            database.AddInParameter(dbCommand, "PaymentType", DbType.String, model.PaymentType);
            database.AddInParameter(dbCommand, "PayCharge", DbType.Currency, model.PayCharge);
            database.AddInParameter(dbCommand, "RefundStatus", DbType.Int32, model.RefundStatus);
            database.AddInParameter(dbCommand, "RefundAmount", DbType.Currency, model.RefundAmount);
            database.AddInParameter(dbCommand, "RefundRemark", DbType.String, model.RefundRemark);
            database.AddInParameter(dbCommand, "Gateway", DbType.String, model.Gateway);
            database.AddInParameter(dbCommand, "OrderTotal", DbType.Currency, model.OrderTotal);
            database.AddInParameter(dbCommand, "OrderPoint", DbType.Int32, model.OrderPoint);
            database.AddInParameter(dbCommand, "OrderCostPrice", DbType.Currency, model.OrderCostPrice);
            database.AddInParameter(dbCommand, "OrderProfit", DbType.Currency, model.OrderProfit);
            database.AddInParameter(dbCommand, "ActualFreight", DbType.Currency, model.ActualFreight);
            database.AddInParameter(dbCommand, "OtherCost", DbType.Currency, model.OtherCost);
            database.AddInParameter(dbCommand, "OptionPrice", DbType.Currency, model.OptionPrice);
            database.AddInParameter(dbCommand, "Amount", DbType.Currency, model.Amount);
            database.AddInParameter(dbCommand, "DiscountAmount", DbType.Currency, model.DiscountAmount);
            database.AddInParameter(dbCommand, "ActivitiesId", DbType.String, model.ActivitiesId);
            database.AddInParameter(dbCommand, "ActivitiesName", DbType.String, model.ActivitiesName);
            database.AddInParameter(dbCommand, "ReducedPromotionId", DbType.Int32, model.ReducedPromotionId);
            database.AddInParameter(dbCommand, "ReducedPromotionName", DbType.String, model.ReducedPromotionName);
            database.AddInParameter(dbCommand, "ReducedPromotionAmount", DbType.Currency, model.ReducedPromotionAmount);
            database.AddInParameter(dbCommand, "IsReduced", DbType.Boolean, model.IsReduced);
            database.AddInParameter(dbCommand, "SentTimesPointPromotionId", DbType.Int32, model.SentTimesPointPromotionId);
            database.AddInParameter(dbCommand, "SentTimesPointPromotionName", DbType.String, model.SentTimesPointPromotionName);
            database.AddInParameter(dbCommand, "TimesPoint", DbType.Currency, model.TimesPoint);
            database.AddInParameter(dbCommand, "IsSendTimesPoint", DbType.Boolean, model.IsSendTimesPoint);
            database.AddInParameter(dbCommand, "FreightFreePromotionId", DbType.Int32, model.FreightFreePromotionId);
            database.AddInParameter(dbCommand, "FreightFreePromotionName", DbType.String, model.FreightFreePromotionName);
            database.AddInParameter(dbCommand, "IsFreightFree", DbType.Boolean, model.IsFreightFree);
            database.AddInParameter(dbCommand, "CouponName", DbType.String, model.CouponName);
            database.AddInParameter(dbCommand, "CouponCode", DbType.String, model.CouponCode);
            database.AddInParameter(dbCommand, "CouponAmount", DbType.Currency, model.CouponAmount);
            database.AddInParameter(dbCommand, "CouponValue", DbType.Currency, model.CouponValue);
            database.AddInParameter(dbCommand, "GroupBuyId", DbType.Int32, model.GroupBuyId);
            database.AddInParameter(dbCommand, "NeedPrice", DbType.Currency, model.NeedPrice);
            database.AddInParameter(dbCommand, "GroupBuyStatus", DbType.Int32, model.GroupBuyStatus);
            database.AddInParameter(dbCommand, "CountDownBuyId", DbType.Int32, model.CountDownBuyId);
            database.AddInParameter(dbCommand, "BundlingId", DbType.Int32, model.BundlingId);
            database.AddInParameter(dbCommand, "BundlingNum", DbType.Int32, model.BundlingNum);
            database.AddInParameter(dbCommand, "BundlingPrice", DbType.Currency, model.BundlingPrice);
            database.AddInParameter(dbCommand, "GatewayOrderId", DbType.String, model.GatewayOrderId);
            database.AddInParameter(dbCommand, "IsPrinted", DbType.Boolean, model.IsPrinted);
            database.AddInParameter(dbCommand, "Tax", DbType.Currency, model.Tax);
            database.AddInParameter(dbCommand, "InvoiceTitle", DbType.String, model.InvoiceTitle);
            database.AddInParameter(dbCommand, "Sender", DbType.String, model.Sender);
            database.AddInParameter(dbCommand, "ReferralUserId", DbType.Int32, model.ReferralUserId);
            database.AddInParameter(dbCommand, "FirstCommission", DbType.Currency, model.FirstCommission);
            database.AddInParameter(dbCommand, "SecondCommission", DbType.Currency, model.SecondCommission);
            database.AddInParameter(dbCommand, "ThirdCommission", DbType.Currency, model.ThirdCommission);
            database.AddInParameter(dbCommand, "RedPagerActivityName", DbType.String, model.RedPagerActivityName);
            database.AddInParameter(dbCommand, "RedPagerID", DbType.Int32, model.RedPagerID);
            database.AddInParameter(dbCommand, "RedPagerOrderAmountCanUse", DbType.Currency, model.RedPagerOrderAmountCanUse);
            database.AddInParameter(dbCommand, "RedPagerAmount", DbType.Currency, model.RedPagerAmount);
            database.AddInParameter(dbCommand, "OldAddress", DbType.String, model.OldAddress);
            database.AddInParameter(dbCommand, "PointToCash", DbType.Currency, model.PointToCash);
            database.AddInParameter(dbCommand, "PointExchange", DbType.Int32, model.PointExchange);
            database.AddInParameter(dbCommand, "SplitState", DbType.Int32, model.SplitState);
            database.AddInParameter(dbCommand, "DeleteBeforeState", DbType.Int32, model.DeleteBeforeState);
            database.AddInParameter(dbCommand, "ClientShortType", DbType.Int32, model.ClientShortType);
            database.AddInParameter(dbCommand, "ReferralPath", DbType.AnsiString, model.ReferralPath);
            database.AddInParameter(dbCommand, "BargainDetialId", DbType.Int32, model.BargainDetialId);
            database.AddInParameter(dbCommand, "BalancePayMoneyTotal", DbType.Decimal, model.BalancePayMoneyTotal);
            database.AddInParameter(dbCommand, "BalancePayFreightMoneyTotal", DbType.Decimal, model.BalancePayFreightMoneyTotal);
            database.AddInParameter(dbCommand, "CouponFreightMoneyTotal", DbType.Decimal, model.CouponFreightMoneyTotal);
            database.AddInParameter(dbCommand, "UpdateDate", DbType.DateTime, model.UpdateDate);
            database.AddInParameter(dbCommand, "LogisticsTools", DbType.Int32, model.LogisticsTools);
            //int result;

            //dbCommand = database.GetSqlStringCommand("insert into Hishop_Orders(OrderId,OrderMarking,OrderStatus,UserId,Username,OrderDate)values('1222', '222', 1, 0, '22', '2016-12-26')");

            return (this.database.ExecuteNonQuery(dbCommand) > 0);
           
        }

        public int DeleteOrders(string orderIds)
        {
            if (!string.IsNullOrEmpty(orderIds) && !orderIds.Contains("'"))
            {
                orderIds = "'" + orderIds.Replace(",", "','") + "'";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("update Hishop_OrderItems set DeleteBeforeState=OrderItemsStatus  WHERE OrderId IN({0});update Hishop_Orders set DeleteBeforeState=OrderStatus  WHERE OrderId IN({0})", orderIds));
            this.database.ExecuteNonQuery(sqlStringCommand);
            sqlStringCommand = this.database.GetSqlStringCommand(string.Format("update  Hishop_OrderItems set OrderItemsStatus={0} WHERE OrderId IN({1})", 12, orderIds));
            this.database.ExecuteNonQuery(sqlStringCommand);
            sqlStringCommand = this.database.GetSqlStringCommand(string.Format("update Hishop_Orders set OrderStatus={0}  WHERE OrderId IN({1})", 12, orderIds));
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public bool DeleteReturnRecordForSendGoods(string orderid)
        {
            string[] strArray = new string[] { "delete from Hishop_OrderReturns where OrderID=@OrderID and (HandleStatus=", 1.ToString(), " or HandleStatus=", 6.ToString(), ")" };
            string query = string.Concat(strArray);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderid);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool EditOrderShipNumber(string orderId, string shipNumber)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Orders SET ShipOrderNumber=@ShipOrderNumber WHERE OrderId =@OrderId");
            this.database.AddInParameter(sqlStringCommand, "ShipOrderNumber", DbType.String, shipNumber);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool ExistsOrderByBargainDetialId(int userId, int bargainDetialId)
        {
            string query = "select count(*) from Hishop_Orders where BargainDetialId=@BargainDetialId and UserId=@UserId";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "BargainDetialId", DbType.Int32, bargainDetialId);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            return (int.Parse(this.database.ExecuteScalar(sqlStringCommand).ToString()) > 0);
        }

        public DataTable GetAllOrderID()
        {
            string query = "select OrderId,IsPrinted,OrderStatus,Gateway from Hishop_Orders  with (nolock) where userid!=0";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public DataSet GetAutoBatchOrdersIdList()
        {
            DataSet set = null;
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
            if (masterSettings.CloseOrderDays > 0)
            {
                object[] objArray = new object[] { "select OrderID from Hishop_Orders where OrderStatus=", 1.ToString(), " AND OrderDate <= @OrderDate AND Gateway<>'hishop.plugins.payment.podrequest';SELECT OrderId FROM  Hishop_Orders WHERE  OrderStatus=", 3.ToString(), " AND ShippingDate <= '", DateTime.Now.AddDays((double) -masterSettings.FinishOrderDays), "' AND Gateway<>'hishop.plugins.payment.podrequest'" };
                string query = string.Concat(objArray);
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
                this.database.AddInParameter(sqlStringCommand, "OrderDate", DbType.DateTime, DateTime.Now.AddDays((double) -masterSettings.CloseOrderDays));
                set = this.database.ExecuteDataSet(sqlStringCommand);
            }
            return set;
        }

        public OrderInfo GetCalculadtionCommission(OrderInfo order, int isModifyOrders)
        {
            DistributorsDao dao = new DistributorsDao();
            DistributorGradeDao dao2 = new DistributorGradeDao();
            DistributorsInfo distributorInfo = null;
            if (order.ReferralUserId > 0)
            {
                distributorInfo = dao.GetDistributorInfo(order.ReferralUserId);
            }
            if (distributorInfo != null)
            {
                decimal num = 0M;
                decimal num2 = 0M;
                decimal num3 = 0M;
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
                bool flag = true;
                bool flag2 = false;
                bool flag3 = false;
                DataView view = new DataView();
                DataView defaultView = new DataView();
                if (isModifyOrders == 0)
                {
                    defaultView = dao2.GetAllDistributorGrade().DefaultView;
                    flag = distributorInfo.ReferralStatus == 0;
                    if (distributorInfo.DistriGradeId.ToString() != "0")
                    {
                        defaultView.RowFilter = " GradeId=" + distributorInfo.DistriGradeId;
                        if (defaultView.Count > 0)
                        {
                            num = decimal.Parse(defaultView[0]["FirstCommissionRise"].ToString());
                        }
                    }
                    if ((masterSettings.EnableCommission && !string.IsNullOrEmpty(distributorInfo.ReferralPath)) && (distributorInfo.ReferralPath != "0"))
                    {
                        string[] strArray = distributorInfo.ReferralPath.Split(new char[] { '|' });
                        if (strArray.Length == 1)
                        {
                            DistributorsInfo info2 = dao.GetDistributorInfo(Globals.ToNum(strArray[0]));
                            if (info2 != null)
                            {
                                flag2 = info2.ReferralStatus == 0;
                                if (info2.DistriGradeId.ToString() != "0")
                                {
                                    defaultView.RowFilter = " GradeId=" + info2.DistriGradeId;
                                    if (defaultView.Count > 0)
                                    {
                                        num2 = decimal.Parse(defaultView[0]["SecondCommissionRise"].ToString());
                                    }
                                }
                            }
                        }
                        else
                        {
                            DistributorsInfo info3 = dao.GetDistributorInfo(Globals.ToNum(strArray[1]));
                            if (info3 != null)
                            {
                                flag2 = info3.ReferralStatus == 0;
                                if (info3.DistriGradeId.ToString() != "0")
                                {
                                    defaultView.RowFilter = " GradeId=" + info3.DistriGradeId;
                                    if (defaultView.Count > 0)
                                    {
                                        num2 = decimal.Parse(defaultView[0]["SecondCommissionRise"].ToString());
                                    }
                                }
                            }
                            DistributorsInfo info4 = dao.GetDistributorInfo(Globals.ToNum(strArray[0]));
                            if (info4 != null)
                            {
                                flag3 = info4.ReferralStatus == 0;
                                if (info4.DistriGradeId.ToString() != "0")
                                {
                                    defaultView.RowFilter = " GradeId=" + info4.DistriGradeId;
                                    if (defaultView.Count > 0)
                                    {
                                        num3 = decimal.Parse(defaultView[0]["ThirdCommissionRise"].ToString());
                                    }
                                }
                            }
                        }
                    }
                    view = new CategoryDao().GetCategories().DefaultView;
                }
                Dictionary<string, LineItemInfo> lineItems = order.LineItems;
                LineItemInfo info = new LineItemInfo();
                string str = null;
                string str2 = null;
                string str3 = null;
                foreach (KeyValuePair<string, LineItemInfo> pair in lineItems)
                {
                    info = pair.Value;
                    if (info.Type == 0)
                    {
                        decimal num4 = (info.GetSubTotal() - info.DiscountAverage) - info.ItemAdjustedCommssion;
                        if (isModifyOrders == 0)
                        {
                            if (num4 > 0M)
                            {
                                info = this.GetNewLineItemInfo(info);
                                if (info.IsSetCommission)
                                {
                                    info.ItemsCommissionScale = flag ? ((info.FirstCommission == 0M) ? 0M : ((info.FirstCommission + num) / 100M)) : 0M;
                                    info.SecondItemsCommissionScale = flag2 ? ((info.SecondCommission == 0M) ? 0M : ((info.SecondCommission + num2) / 100M)) : 0M;
                                    info.ThirdItemsCommissionScale = flag3 ? ((info.ThirdCommission == 0M) ? 0M : ((info.ThirdCommission + num3) / 100M)) : 0M;
                                    info.ItemsCommission = flag ? ((info.FirstCommission > 0M) ? (info.ItemsCommissionScale * num4) : 0M) : 0M;
                                    info.SecondItemsCommission = flag2 ? ((info.SecondCommission > 0M) ? (info.SecondItemsCommissionScale * num4) : 0M) : 0M;
                                    info.ThirdItemsCommission = flag3 ? ((info.ThirdCommission > 0M) ? (info.ThirdItemsCommissionScale * num4) : 0M) : 0M;
                                }
                                else
                                {
                                    DataTable productCategories = new ProductDao().GetProductCategories(info.ProductId);
                                    if ((productCategories.Rows.Count > 0) && (productCategories.Rows[0][0].ToString() != "0"))
                                    {
                                        view.RowFilter = " CategoryId=" + productCategories.Rows[0][0];
                                        str = view[0]["FirstCommission"].ToString();
                                        str2 = view[0]["SecondCommission"].ToString();
                                        str3 = view[0]["ThirdCommission"].ToString();
                                        if ((!string.IsNullOrEmpty(str) && !string.IsNullOrEmpty(str2)) && !string.IsNullOrEmpty(str3))
                                        {
                                            info.ItemsCommissionScale = flag ? ((decimal.Parse(str) > 0M) ? ((decimal.Parse(str) + num) / 100M) : 0M) : 0M;
                                            info.SecondItemsCommissionScale = flag2 ? ((decimal.Parse(str2) > 0M) ? ((decimal.Parse(str2) + num2) / 100M) : 0M) : 0M;
                                            info.ThirdItemsCommissionScale = flag3 ? ((decimal.Parse(str3) > 0M) ? ((decimal.Parse(str3) + num3) / 100M) : 0M) : 0M;
                                            info.ItemsCommission = flag ? ((decimal.Parse(str) > 0M) ? (info.ItemsCommissionScale * num4) : 0M) : 0M;
                                            info.SecondItemsCommission = flag2 ? ((decimal.Parse(str2) > 0M) ? (info.SecondItemsCommissionScale * num4) : 0M) : 0M;
                                            info.ThirdItemsCommission = flag3 ? ((decimal.Parse(str3) > 0M) ? (info.ThirdItemsCommissionScale * num4) : 0M) : 0M;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                info.ItemsCommission = 0M;
                                info.SecondItemsCommission = 0M;
                                info.ThirdItemsCommission = 0M;
                            }
                        }
                        info.ItemsCommission = ((num4 * info.ItemsCommissionScale) * info.CommissionDiscount) / 100M;
                        info.SecondItemsCommission = ((num4 * info.SecondItemsCommissionScale) * info.CommissionDiscount) / 100M;
                        info.ThirdItemsCommission = ((num4 * info.ThirdItemsCommissionScale) * info.CommissionDiscount) / 100M;
                    }
                    else
                    {
                        info.ItemsCommission = 0M;
                        info.SecondItemsCommission = 0M;
                        info.ThirdItemsCommission = 0M;
                    }
                    info.ItemsCommission = Math.Round(info.ItemsCommission, 2);
                    info.SecondItemsCommission = Math.Round(info.SecondItemsCommission, 2);
                    info.ThirdItemsCommission = Math.Round(info.ThirdItemsCommission, 2);
                }
                return order;
            }
            order.ReferralUserId = 0;
            return order;
        }

        public decimal GetCommossionByOrderId(string orderId, int userId)
        {
            string query = "select CommTotal from Hishop_Commissions WHERE OrderId=@OrderId AND UserId=@UserId";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int16, userId);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if ((obj2 != null) && !(obj2 is DBNull))
            {
                return (decimal) obj2;
            }
            return 0M;
        }

        public int GetCountOrderIDByStatus(OrderStatus? orderstatus, OrderStatus? itemstatus)
        {
            string str = string.Empty;
            if (orderstatus.HasValue && (((((OrderStatus) orderstatus.Value) != OrderStatus.Closed) || !itemstatus.HasValue) || ((((OrderStatus) itemstatus.Value) != OrderStatus.Refunded) && (((OrderStatus) itemstatus.Value) != OrderStatus.Returned))))
            {
                str = " OrderStatus=" + ((int) orderstatus.Value);
            }
            if (itemstatus.HasValue)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    str = str + " and ";
                }
                if (((orderstatus.HasValue && (((OrderStatus) orderstatus.Value) == OrderStatus.Closed)) && (((OrderStatus) itemstatus.Value) == OrderStatus.Refunded)) || (((OrderStatus) itemstatus.Value) == OrderStatus.Returned))
                {
                    object obj2 = str;
                    str = string.Concat(new object[] { obj2, " OrderStatus=", (int) orderstatus.Value, " and OrderId in(SELECT OrderId FROM Hishop_OrderReturns where handlestatus=2)" });
                }
                else
                {
                    object obj3 = str;
                    str = string.Concat(new object[] { obj3, " OrderId in(SELECT OrderId FROM Hishop_OrderItems WHERE OrderItemsStatus=", (int) itemstatus.Value, ")" });
                }
            }
            if (!string.IsNullOrEmpty(str))
            {
                str = " where " + str;
            }
            string query = "select count(0) from Hishop_Orders with (nolock) " + str;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand));
        }

        public int GetCouponId(int ActDid, decimal OrderTotal, int OrderNumber)
        {
            string query = "select top 1 CouponId from Hishop_Activities_Detail where ActivitiesId=@ActivitiesId and  MeetMoney=<@OrderTotal and MeetNumber<=@OrderNumber order by  MeetMoney,MeetNumber DESC";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "ActivitiesId", DbType.Int32, ActDid);
            this.database.AddInParameter(sqlStringCommand, "OrderTotal", DbType.Decimal, OrderTotal);
            this.database.AddInParameter(sqlStringCommand, "OrderNumber", DbType.Int32, OrderNumber);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            int num = 0;
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                num = (int) obj2;
            }
            return num;
        }

        public DbQueryResult GetDeleteOrders(OrderQuery query)
        {
            StringBuilder builder = new StringBuilder("1=1 and userid!=0");
            if (query.Type.HasValue)
            {
                if (((OrderQuery.OrderType) query.Type.Value) == OrderQuery.OrderType.GroupBuy)
                {
                    builder.Append(" And GroupBuyId > 0 ");
                }
                else
                {
                    builder.Append(" And GroupBuyId is null ");
                }
            }
            if ((query.OrderId != string.Empty) && (query.OrderId != null))
            {
                builder.AppendFormat(" AND OrderId = '{0}'", DataHelper.CleanSearchString(query.OrderId));
            }
            if (query.UserId.HasValue)
            {
                builder.AppendFormat(" AND UserId = '{0}'", query.UserId.Value);
            }
            if (query.PaymentType.HasValue)
            {
                builder.AppendFormat(" AND PaymentTypeId = '{0}'", query.PaymentType.Value);
            }
            if (query.GroupBuyId.HasValue)
            {
                builder.AppendFormat(" AND GroupBuyId = {0}", query.GroupBuyId.Value);
            }
            if (!string.IsNullOrEmpty(query.ProductName))
            {
                builder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Hishop_OrderItems WHERE ItemDescription LIKE '%{0}%')", DataHelper.CleanSearchString(query.ProductName));
            }
            if (query.OrderItemsStatus.HasValue)
            {
                if (((OrderStatus) query.OrderItemsStatus.Value) == OrderStatus.ApplyForRefund)
                {
                    builder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Hishop_OrderItems WHERE OrderItemsStatus in({0},{1}))", (int) query.OrderItemsStatus.Value, 7);
                }
                else
                {
                    builder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Hishop_OrderItems WHERE OrderItemsStatus={0})", (int) query.OrderItemsStatus.Value);
                }
            }
            if (!string.IsNullOrEmpty(query.ShipTo))
            {
                builder.AppendFormat(" AND (ShipTo LIKE '%{0}%' or CellPhone='{0}')", DataHelper.CleanSearchString(query.ShipTo));
            }
            if (query.RegionId.HasValue)
            {
                builder.AppendFormat(" AND ShippingRegion like '%{0}%'", DataHelper.CleanSearchString(RegionHelper.GetFullRegion(query.RegionId.Value, "，")));
            }
            if (!string.IsNullOrEmpty(query.UserName))
            {
                builder.AppendFormat(" AND  UserName  = '{0}' ", DataHelper.CleanSearchString(query.UserName));
            }
            builder.AppendFormat(" AND OrderStatus = {0}", 12);
            if (query.StartDate.HasValue)
            {
                builder.AppendFormat(" AND datediff(dd,'{0}',OrderDate)>=0", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
            }
            if (query.EndDate.HasValue)
            {
                builder.AppendFormat(" AND datediff(dd,'{0}',OrderDate)<=0", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
            }
            if (query.ShippingModeId.HasValue)
            {
                builder.AppendFormat(" AND ShippingModeId = {0}", query.ShippingModeId.Value);
            }
            if (query.IsPrinted.HasValue)
            {
                builder.AppendFormat(" AND ISNULL(IsPrinted, 0)={0}", query.IsPrinted.Value);
            }
            if (query.ShippingModeId > 0)
            {
                builder.AppendFormat(" AND ShippingModeId={0}", query.ShippingModeId);
            }
            if (!string.IsNullOrEmpty(query.StoreName))
            {
                builder.AppendFormat(" AND StoreName like '%{0}%' ", DataHelper.CleanSearchString(query.StoreName));
            }
            if (!string.IsNullOrEmpty(query.Gateway))
            {
                builder.AppendFormat(" AND Gateway='{0}' ", DataHelper.CleanSearchString(query.Gateway));
            }
            if (query.DeleteBeforeState > 0)
            {
                builder.AppendFormat(" AND DeleteBeforeState='{0}' ", DataHelper.CleanSearchString(query.DeleteBeforeState.ToString()));
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_Order", "OrderId", builder.ToString(), "*");
        }

        public DataSet GetDistributorOrder(OrderQuery query)
        {
            string str = string.Empty;
            if (query.Status == OrderStatus.Finished)
            {
                str = str + " AND OrderStatus=" + ((int) query.Status);
            }
            string str2 = "SELECT UserID,OrderId, OrderDate,FinishDate, OrderStatus,PaymentTypeId, OrderTotal,Gateway,FirstCommission,SecondCommission,ThirdCommission FROM Hishop_Orders o WHERE ReferralUserId = @UserId";
            str2 = (str2 + str + " ORDER BY OrderDate DESC") + " SELECT ID,OrderId,SkuId, ThumbnailsUrl, ItemDescription, SKUContent, SKU, ProductId,Quantity,ItemListPrice,ItemAdjustedCommssion,OrderItemsStatus,ItemsCommission,Type,ReturnMoney,IsAdminModify,LimitedTimeDiscountId FROM Hishop_OrderItems WHERE OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE ReferralUserId = @UserId" + str + ")";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str2);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, query.UserId);
            DataSet set = this.database.ExecuteDataSet(sqlStringCommand);
            DataColumn parentColumn = set.Tables[0].Columns["OrderId"];
            DataColumn childColumn = set.Tables[1].Columns["OrderId"];
            DataRelation relation = new DataRelation("OrderItems", parentColumn, childColumn);
            set.Relations.Add(relation);
            return set;
        }

        public DataSet GetDistributorOrderByDetials(OrderQuery query)
        {
            string str = string.Empty;
            if (query.Status == OrderStatus.Finished)
            {
                str = str + " AND OrderStatus=" + ((int) query.Status);
            }
            string str2 = "SELECT UserId,OrderId, OrderDate,FinishDate, OrderStatus,PaymentTypeId, OrderTotal,Gateway,FirstCommission,SecondCommission,ThirdCommission FROM Hishop_Orders o WHERE OrderId in (select OrderId from Hishop_Commissions where UserId=@UserId and ReferralUserId=@ReferralUserId) ";
            str2 = (str2 + str) + " ORDER BY OrderDate DESC" + " SELECT OrderId,SkuId, ThumbnailsUrl, ItemDescription, SKUContent, SKU, ProductId,Quantity,ItemListPrice,ItemAdjustedCommssion,OrderItemsStatus,ItemsCommission,Type,ReturnMoney,IsAdminModify,LimitedTimeDiscountId FROM Hishop_OrderItems WHERE OrderId IN (SELECT OrderId FROM Hishop_Commissions WHERE ReferralUserId = @ReferralUserId and  UserId=@UserId ) ORDER BY OrderId DESC";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str2);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, query.UserId);
            this.database.AddInParameter(sqlStringCommand, "ReferralUserId", DbType.Int32, query.ReferralUserId);
            DataSet set = this.database.ExecuteDataSet(sqlStringCommand);
            DataColumn parentColumn = set.Tables[0].Columns["OrderId"];
            DataColumn childColumn = set.Tables[1].Columns["OrderId"];
            DataRelation relation = new DataRelation("OrderItems", parentColumn, childColumn);
            set.Relations.Add(relation);
            return set;
        }

        public DbQueryResult GetDistributorOrderByStatus(OrderQuery query, int userId)
        {
            string filter = string.Empty;
            if (userId > 0)
            {
                filter = filter + string.Format("  UserId={0}", userId);
            }
            if (query.Status == OrderStatus.Finished)
            {
                filter = filter + " AND OrderStatus=" + ((int) query.Status);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_UserOrderByPage", "OrderId", filter, "*");
        }

        public int GetDistributorOrderCount(OrderQuery query)
        {
            string str = string.Empty;
            switch (query.Status)
            {
                case OrderStatus.Finished:
                    str = str + " AND OrderStatus=" + ((int) query.Status);
                    break;

                case OrderStatus.Today:
                {
                    string str2 = DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00";
                    str = str + " AND OrderDate>='" + str2 + "'";
                    break;
                }
            }
            string str3 = "SELECT COUNT(*)  FROM Hishop_Orders o WHERE ReferralUserId = @ReferralUserId";
            str3 = str3 + str;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str3);
            sqlStringCommand.CommandType = CommandType.Text;
            this.database.AddInParameter(sqlStringCommand, "ReferralUserId", DbType.Int32, query.UserId);
            return (int) this.database.ExecuteScalar(sqlStringCommand);
        }

        public string GetexChangeName(int exChangeId)
        {
            string query = "select Name from Hishop_PointExChange_PointExChanges where id=" + exChangeId;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if ((obj2 != null) && !(obj2 is DBNull))
            {
                return obj2.ToString();
            }
            return "";
        }

        public string GetFirstProductName(string OrderId)
        {
            string query = string.Format("select top 1 ItemDescription from Hishop_OrderItems where OrderId= '{0}'", OrderId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return Convert.ToString(this.database.ExecuteScalar(sqlStringCommand));
        }

        public DbQueryResult GetMemberDetailOrders(MemberDetailOrderQuery query)
        {
            StringBuilder builder = new StringBuilder("1=1 and userid!=0");
            if (query.UserId.HasValue)
            {
                builder.AppendFormat(" AND UserId = '{0}'", query.UserId.Value);
            }
            if (query.Status != null)
            {
                builder.AppendFormat(" AND  ((OrderStatus in (" + string.Join(",", (from t in query.Status select ((int) t).ToString()).ToArray<string>()) + ")) ", new object[0]);
                if (query.Status.Contains<OrderStatus>(OrderStatus.BuyerAlreadyPaid))
                {
                    builder.AppendFormat(" OR (OrderStatus = 1 AND Gateway = 'hishop.plugins.payment.podrequest')", new object[0]);
                }
                builder.Append(" )");
            }
            builder.AppendFormat(" AND OrderStatus != {0}", 12);
            if (query.StartDate.HasValue)
            {
                builder.AppendFormat(" AND datediff(dd,'{0}',OrderDate)>=0", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
            }
            if (query.EndDate.HasValue)
            {
                builder.AppendFormat(" AND datediff(dd,'{0}',OrderDate)<=0", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
            }
            if (query.StartFinishDate.HasValue)
            {
                builder.AppendFormat(" AND datediff(dd,'{0}',FinishDate)>=0", DataHelper.GetSafeDateTimeFormat(query.StartFinishDate.Value));
            }
            if (query.EndFinishDate.HasValue)
            {
                builder.AppendFormat(" AND datediff(dd,'{0}',FinishDate)<=0", DataHelper.GetSafeDateTimeFormat(query.EndFinishDate.Value));
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_Order", "OrderId", builder.ToString(), "*");
        }

        public LineItemInfo GetNewLineItemInfo(LineItemInfo info)
        {
            ProductInfo productDetails = new ProductDao().GetProductDetails(info.ProductId);
            if (productDetails != null)
            {
                info.IsSetCommission = productDetails.IsSetCommission;
                info.FirstCommission = productDetails.FirstCommission;
                info.SecondCommission = productDetails.SecondCommission;
                info.ThirdCommission = productDetails.ThirdCommission;
            }
            return info;
        }

        public DataSet GetOrderGoods(string orderIds)
        {
            if (!string.IsNullOrEmpty(orderIds) && !orderIds.Contains("'"))
            {
                orderIds = "'" + orderIds.Replace(",", "','") + "'";
            }
            this.database = DatabaseFactory.CreateDatabase();
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT OrderId, ItemDescription AS ProductName, SKU, SKUContent, ShipmentQuantity,");
            builder.Append(" (SELECT Stock FROM Hishop_SKUs WHERE SkuId = oi.SkuId) + oi.ShipmentQuantity AS Stock, (SELECT Remark FROM Hishop_Orders WHERE OrderId = oi.OrderId) AS Remark");
            builder.Append(" FROM Hishop_OrderItems oi WHERE OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE OrderStatus = 2 or (OrderStatus = 1 AND Gateway='hishop.plugins.payment.podrequest'))");
            builder.Append(" AND (OrderItemsStatus=2 OR OrderItemsStatus=1)");
            builder.AppendFormat(" AND OrderId IN ({0}) ORDER BY OrderId;", orderIds);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand);
        }

        public OrderInfo GetOrderInfo(string orderId)
        {
            OrderInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Orders Where OrderId = @OrderId;  SELECT i.*,o.OrderStatus FROM Hishop_OrderItems i,Hishop_Orders o Where i.OrderId=o.OrderId AND i.OrderId = @OrderId ");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateOrder(reader);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    info.LineItems.Add(reader["Id"].ToString(), DataMapper.PopulateLineItem(reader));
                }
            }
            return info;
        }

        public OrderInfo GetOrderInfoByCode(string code)
        {
            OrderInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Orders Where CouponCode = @CouponCode;");
            this.database.AddInParameter(sqlStringCommand, "CouponCode", DbType.String, code);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateOrder(reader);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    info.LineItems.Add(reader["Id"].ToString(), DataMapper.PopulateLineItem(reader));
                }
            }
            return info;
        }

        public OrderInfo GetOrderInfoForLineItems(string orderId)
        {
            OrderInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select isnull(b.OpenId,'') as  BuyerWXOpenId, isnull(c.OpenId,'') as  SalerWXOpenId  ,a.*\r\n                    from Hishop_Orders a  \r\n                    left join aspnet_Members b on a.UserId= b.UserId\r\n                    left join aspnet_Members c on a.ReferralUserId= c.UserId\r\n                    where a.OrderId = @OrderId;  \r\n               SELECT i.*,o.OrderStatus FROM Hishop_OrderItems i,Hishop_Orders o Where i.OrderId=o.OrderId AND i.OrderId = @OrderId ");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateOrder(reader);
                }
                reader.NextResult();
                info.ItemCount = 0;
                while (reader.Read())
                {
                    info.LineItems.Add(reader["ID"].ToString(), DataMapper.PopulateLineItem(reader));
                    info.ItemCount++;
                }
            }
            return info;
        }

        public DataTable GetOrderMarkingAllOrderID(string OrderMarking, bool isWaitPay)
        {
            string str = string.Empty;
            if (isWaitPay)
            {
                str = " and OrderStatus=1 ";
            }
            string query = "select OrderId from Hishop_Orders where  OrderMarking='" + OrderMarking + "'" + str;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public int GetOrderReferralUserId(string OrderId)
        {
            string query = "select ReferralUserId from Hishop_Orders where OrderId=@OrderId";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, OrderId);
            int num = 0;
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if ((obj2 != null) && (obj2 != DBNull.Value))
            {
                num = (int) obj2;
            }
            return num;
        }

        public DbQueryResult GetOrders(OrderQuery query)
        {
            StringBuilder builder = new StringBuilder("1=1 and userid!=0");
            if (query.Type.HasValue)
            {
                if (((OrderQuery.OrderType) query.Type.Value) == OrderQuery.OrderType.GroupBuy)
                {
                    builder.Append(" And GroupBuyId > 0 ");
                }
                else
                {
                    builder.Append(" And GroupBuyId is null ");
                }
            }
            if ((query.OrderId != string.Empty) && (query.OrderId != null))
            {
                builder.AppendFormat(" AND OrderId = '{0}'", DataHelper.CleanSearchString(query.OrderId));
            }
            if (!string.IsNullOrEmpty(query.ShipId))
            {
                builder.AppendFormat(" AND ShipOrderNumber = '{0}'", DataHelper.CleanSearchString(query.ShipId));
            }
            if (query.UserId.HasValue)
            {
                builder.AppendFormat(" AND UserId = '{0}'", query.UserId.Value);
            }
            if (query.PaymentType.HasValue)
            {
                builder.AppendFormat(" AND PaymentTypeId = '{0}'", query.PaymentType.Value);
            }
            if (query.GroupBuyId.HasValue)
            {
                builder.AppendFormat(" AND GroupBuyId = {0}", query.GroupBuyId.Value);
            }
            if (!string.IsNullOrEmpty(query.ProductName))
            {
                builder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Hishop_OrderItems WHERE ItemDescription LIKE '%{0}%')", DataHelper.CleanSearchString(query.ProductName));
            }
            if (query.OrderItemsStatus.HasValue)
            {
                if (((OrderStatus) query.OrderItemsStatus.Value) == OrderStatus.ApplyForRefund)
                {
                    builder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Hishop_OrderItems WHERE OrderItemsStatus in({0},{1}))", (int) query.OrderItemsStatus.Value, 7);
                }
                else if ((query.Status != OrderStatus.Closed) || ((((OrderStatus) query.OrderItemsStatus.Value) != OrderStatus.Refunded) && (((OrderStatus) query.OrderItemsStatus.Value) != OrderStatus.Returned)))
                {
                    builder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM Hishop_OrderItems WHERE OrderItemsStatus={0})", (int) query.OrderItemsStatus.Value);
                }
            }
            if (!string.IsNullOrEmpty(query.ShipTo))
            {
                builder.AppendFormat(" AND (ShipTo LIKE '%{0}%' or CellPhone='{0}')", DataHelper.CleanSearchString(query.ShipTo));
            }
            if (query.RegionId.HasValue)
            {
                builder.AppendFormat(" AND ShippingRegion like '%{0}%'", DataHelper.CleanSearchString(RegionHelper.GetFullRegion(query.RegionId.Value, "，")));
            }
            if (!string.IsNullOrEmpty(query.UserName))
            {
                builder.AppendFormat(" AND  UserName  = '{0}' ", DataHelper.CleanSearchString(query.UserName));
            }
            if (query.Status == OrderStatus.History)
            {
                builder.AppendFormat(" AND OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2} AND OrderDate < '{3}'", new object[] { 1, 4, 9, DateTime.Now.AddMonths(-3) });
            }
            else if (query.Status == OrderStatus.BuyerAlreadyPaid)
            {
                builder.AppendFormat(" AND (OrderStatus = {0} OR (OrderStatus = 1 AND Gateway = 'hishop.plugins.payment.podrequest'))", (int) query.Status);
            }
            else if (((query.Status == OrderStatus.Closed) && query.OrderItemsStatus.HasValue) && ((((OrderStatus) query.OrderItemsStatus.Value) == OrderStatus.Refunded) || (((OrderStatus) query.OrderItemsStatus.Value) == OrderStatus.Returned)))
            {
                builder.AppendFormat(" AND OrderStatus = {0} and OrderID in(select OrderID from Hishop_OrderReturns where  handlestatus=2)", (int) query.Status);
            }
            else if (query.Status != OrderStatus.All)
            {
                builder.AppendFormat(" AND OrderStatus = {0}", (int) query.Status);
            }
            builder.AppendFormat(" AND OrderStatus != {0}", 12);
            if (query.StartDate.HasValue)
            {
                builder.AppendFormat(" AND datediff(dd,'{0}',OrderDate)>=0", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
            }
            if (query.EndDate.HasValue)
            {
                builder.AppendFormat(" AND datediff(dd,'{0}',OrderDate)<=0", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
            }
            if (query.ShippingModeId.HasValue)
            {
                builder.AppendFormat(" AND ShippingModeId = {0}", query.ShippingModeId.Value);
            }
            if (query.IsPrinted.HasValue)
            {
                builder.AppendFormat(" AND ISNULL(IsPrinted, 0)={0}", query.IsPrinted.Value);
            }
            if (query.ShippingModeId > 0)
            {
                builder.AppendFormat(" AND ShippingModeId={0}", query.ShippingModeId);
            }
            if (!string.IsNullOrEmpty(query.StoreName))
            {
                builder.AppendFormat(" AND StoreName like '%{0}%' ", DataHelper.CleanSearchString(query.StoreName));
            }
            if (!string.IsNullOrEmpty(query.Gateway))
            {
                builder.AppendFormat(" AND Gateway='{0}' ", DataHelper.CleanSearchString(query.Gateway));
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_Order", "OrderId", builder.ToString(), "*");
        }

        public DataSet GetOrdersAndLines(string orderIds)
        {
            if (!string.IsNullOrEmpty(orderIds) && !orderIds.Contains("'"))
            {
                orderIds = "'" + orderIds.Replace(",", "','") + "'";
            }
            this.database = DatabaseFactory.CreateDatabase();
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("SELECT * FROM Hishop_Orders WHERE  OrderId IN ({0}) order by  ShipOrderNumber asc,OrderDate desc ", orderIds);
            builder.AppendFormat(" SELECT * FROM Hishop_OrderItems WHERE OrderId IN ({0});", orderIds);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand);
        }

        public DataSet GetOrdersByOrderIDList(string orderIds)
        {
            if (!string.IsNullOrEmpty(orderIds) && !orderIds.Contains("'"))
            {
                orderIds = "'" + orderIds.Replace(",", "','") + "'";
            }
            this.database = DatabaseFactory.CreateDatabase();
            string query = string.Empty;
            string str2 = " OrderId,ShipTo,RegionId,ExpressCompanyName,ExpressCompanyAbb,ShipOrderNumber,Remark,OrderStatus,ShippingRegion,Address";
            query = string.Format("with v as (SELECT " + str2 + ", row_number() over (partition by ShipTo+CONVERT(VARCHAR(11), RegionId)+[Address]+CellPhone order by  RegionId desc) as rownumber from Hishop_Orders where   OrderId in ({0})) select " + str2 + ",OrderStatus,rownumber from v", orderIds);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return this.database.ExecuteDataSet(sqlStringCommand);
        }

        public bool GetOrderUserAliOpenId(string OrderId, out string BuyerAliOpenId, out string SalerAliOpenId)
        {
            BuyerAliOpenId = "";
            SalerAliOpenId = "";
            string query = string.Format("select top 1 isnull(b.AlipayOpenid,'') as  BuyerWXOpenId, isnull(c.AlipayOpenid,'') as  SalerWXOpenId  from Hishop_Orders a   left join aspnet_Members b on a.UserId= b.UserId  left join aspnet_Members c on a.ReferralUserId= c.UserId  where a.OrderId = '{0}'", OrderId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            DataTable table = this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
            if (table.Rows.Count > 0)
            {
                BuyerAliOpenId = Convert.ToString(table.Rows[0]["BuyerWXOpenId"]);
                SalerAliOpenId = Convert.ToString(table.Rows[0]["SalerWXOpenId"]);
                return true;
            }
            return false;
        }

        public bool GetOrderUserOpenId(string OrderId, out string BuyerWXOpenId, out string SalerWXOpenId)
        {
            BuyerWXOpenId = "";
            SalerWXOpenId = "";
            string query = string.Format("select top 1 isnull(b.OpenId,'') as  BuyerWXOpenId, isnull(c.OpenId,'') as  SalerWXOpenId  from Hishop_Orders a   left join aspnet_Members b on a.UserId= b.UserId  left join aspnet_Members c on a.ReferralUserId= c.UserId  where a.OrderId = '{0}'", OrderId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            DataTable table = this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
            if (table.Rows.Count > 0)
            {
                BuyerWXOpenId = Convert.ToString(table.Rows[0]["BuyerWXOpenId"]);
                SalerWXOpenId = Convert.ToString(table.Rows[0]["SalerWXOpenId"]);
                return true;
            }
            return false;
        }

        public DataSet GetProductGoods(string orderIds)
        {
            if (!string.IsNullOrEmpty(orderIds) && !orderIds.Contains("'"))
            {
                orderIds = "'" + orderIds.Replace(",", "','") + "'";
            }
            this.database = DatabaseFactory.CreateDatabase();
            StringBuilder builder = new StringBuilder();
            builder.Append("SELECT ItemDescription AS ProductName, SKU, SKUContent, sum(ShipmentQuantity) as ShipmentQuantity,");
            builder.Append(" (SELECT Stock FROM Hishop_SKUs WHERE SkuId = oi.SkuId) + sum(ShipmentQuantity) AS Stock FROM Hishop_OrderItems oi");
            builder.Append(" WHERE OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE OrderStatus = 2 or (OrderStatus = 1 AND Gateway='hishop.plugins.payment.podrequest'))");
            builder.Append(" AND OrderItemsStatus=2");
            builder.AppendFormat(" AND OrderId in ({0}) GROUP BY ItemDescription, SkuId, SKU, SKUContent;", orderIds);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand);
        }

        public string GetReplaceComments(string orderId)
        {
            string query = "select Comments from Hishop_OrderReplace where HandleStatus=0 and OrderId='" + orderId + "'";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if ((obj2 != null) && !(obj2 is DBNull))
            {
                return obj2.ToString();
            }
            return "";
        }

        public DataTable GetSendGoodsOrders(string orderIds)
        {
            if (!string.IsNullOrEmpty(orderIds) && !orderIds.Contains("'"))
            {
                orderIds = "'" + orderIds.Replace(",", "','") + "'";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT * FROM Hishop_Orders WHERE (OrderStatus = 2 OR (OrderStatus = 1 AND Gateway = 'hishop.plugins.payment.podrequest')) AND OrderId IN ({0}) order by OrderDate desc", orderIds));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public DataSet GetTradeOrders(OrderQuery query, int distributorUserId, out int records)
        {
            DataSet set = new DataSet();
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_API_Orders_Get");
            this.database.AddInParameter(storedProcCommand, "PageIndex", DbType.Int32, query.PageIndex);
            this.database.AddInParameter(storedProcCommand, "PageSize", DbType.Int32, query.PageSize);
            this.database.AddInParameter(storedProcCommand, "IsCount", DbType.Boolean, query.IsCount);
            this.database.AddInParameter(storedProcCommand, "sqlPopulate", DbType.String, this.BuildOrdersQuery(query, new int?(distributorUserId)));
            this.database.AddOutParameter(storedProcCommand, "TotalOrders", DbType.Int32, 4);
            using (set = this.database.ExecuteDataSet(storedProcCommand))
            {
                set.Relations.Add("OrderRelation", set.Tables[0].Columns["OrderId"], set.Tables[1].Columns["OrderId"]);
            }
            records = (int) this.database.GetParameterValue(storedProcCommand, "TotalOrders");
            return set;
        }

        public OrderInfo GetUserLastOrder(int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT top 1 * FROM Hishop_Orders Where UserId=@UserId and OrderStatus=5 order by orderdate desc");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<OrderInfo>(reader);
            }
        }

        public DataSet GetUserOrder(int userId, OrderQuery query)
        {
            string str = string.Empty;
            if (query.Status == OrderStatus.WaitBuyerPay)
            {
                str = str + " AND OrderStatus = 1 AND Gateway <> 'hishop.plugins.payment.podrequest'";
            }
            else if (query.Status == OrderStatus.BuyerAlreadyPaid)
            {
                str = str + " AND (OrderStatus = 2 OR (OrderStatus = 1 AND Gateway = 'hishop.plugins.payment.podrequest'))";
            }
            else if (query.Status == OrderStatus.SellerAlreadySent)
            {
                str = str + " AND OrderStatus = 3 ";
            }
            string str2 = "SELECT OrderId,OrderMarking, OrderDate, OrderStatus,PaymentTypeId, OrderTotal,   Gateway,(SELECT count(0) FROM vshop_OrderRedPager WHERE OrderId = o.OrderId and ExpiryDays<getdate() and AlreadyGetTimes<MaxGetTimes) as HasRedPage,(SELECT SUM(Quantity) FROM Hishop_OrderItems WHERE OrderId = o.OrderId) as ProductSum FROM Hishop_Orders o WHERE UserId = @UserId";
            str2 = (str2 + str + " ORDER BY OrderDate DESC") + " SELECT OrderId, ThumbnailsUrl, ItemDescription, SKUContent, SKU,OrderItemsStatus, ProductId,Quantity,ReturnMoney,SkuID FROM Hishop_OrderItems WHERE OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE UserId = @UserId" + str + ")";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str2);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            DataSet set = this.database.ExecuteDataSet(sqlStringCommand);
            DataColumn parentColumn = set.Tables[0].Columns["OrderId"];
            DataColumn childColumn = set.Tables[1].Columns["OrderId"];
            DataRelation relation = new DataRelation("OrderItems", parentColumn, childColumn);
            set.Relations.Add(relation);
            return set;
        }

        public DbQueryResult GetUserOrderByPage(int userId, OrderQuery query)
        {
            string str = string.Empty;
            if (query.Status == OrderStatus.WaitBuyerPay)
            {
                str = str + " AND OrderStatus = 1 AND Gateway <> 'hishop.plugins.payment.podrequest'";
            }
            else if (query.Status == OrderStatus.BuyerAlreadyPaid)
            {
                str = str + " AND (OrderStatus = 2 OR (OrderStatus = 1 AND Gateway = 'hishop.plugins.payment.podrequest'))";
            }
            else if (query.Status == OrderStatus.SellerAlreadySent)
            {
                str = str + " AND OrderStatus = 3 ";
            }
            else
            {
                str = str + " AND OrderStatus<>12 ";
            }
            int num = ((query.PageIndex - 1) * query.PageSize) + 1;
            int num2 = (num + query.PageSize) - 1;
            string str2 = "SELECT OrderId FROM Hishop_Orders  WHERE OrderId in ( SELECT orderid from ( SELECT ROW_NUMBER() OVER (ORDER BY OrderDate DESC) AS ordernumber,OrderId  FROM Hishop_Orders o WHERE UserId = @UserId";
            object obj2 = str2 + str;
            str2 = string.Concat(new object[] { obj2, ") AS W where W.ordernumber between ", num, " AND  ", num2, ")" });
            string str3 = "SELECT * from( SELECT ROW_NUMBER() OVER (ORDER BY OrderDate DESC) AS ordernumber,OrderId,PayDate,BalancePayMoneyTotal,OrderMarking,OrderDate,OrderStatus,PaymentTypeId,OrderTotal,Gateway,(SELECT count(0) FROM vshop_OrderRedPager WHERE OrderId = o.OrderId and ExpiryDays<getdate() and AlreadyGetTimes<MaxGetTimes   ) as HasRedPage,(SELECT SUM(Quantity) FROM Hishop_OrderItems WHERE OrderId = o.OrderId) as ProductSum FROM Hishop_Orders o WHERE UserId = @UserId";
            object obj3 = str3 + str;
            str3 = (string.Concat(new object[] { obj3, ") AS W where W.ordernumber between ", num, " AND  ", num2 }) + " SELECT ID, OrderId, ThumbnailsUrl, ItemDescription, SKUContent, SKU,OrderItemsStatus, ProductId,Quantity,ReturnMoney,SkuID,ItemAdjustedPrice,Type,PointNumber,LimitedTimeDiscountId,DiscountAverage FROM Hishop_OrderItems WHERE OrderId IN  (" + str2 + ")   ") + "  SELECT COUNT(*) FROM Hishop_Orders WHERE UserId = @UserId" + str;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str3);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            DataSet set = this.database.ExecuteDataSet(sqlStringCommand);
            DataColumn parentColumn = set.Tables[0].Columns["OrderId"];
            DataColumn childColumn = set.Tables[1].Columns["OrderId"];
            DataRelation relation = new DataRelation("OrderItems", parentColumn, childColumn);
            set.Relations.Add(relation);
            return new DbQueryResult { Data = set, TotalRecords = int.Parse(set.Tables[2].Rows[0][0].ToString()) };
        }

        public int GetUserOrderCount(int userId, OrderQuery query)
        {
            string str = string.Empty;
            if (query.Status == OrderStatus.WaitBuyerPay)
            {
                str = str + " AND OrderStatus = 1 AND Gateway <> 'hishop.plugins.payment.podrequest'";
            }
            else if (query.Status == OrderStatus.SellerAlreadySent)
            {
                str = str + " AND OrderStatus = 3  ";
            }
            else if (query.Status == OrderStatus.BuyerAlreadyPaid)
            {
                str = str + " AND (OrderStatus = 2 OR (OrderStatus = 1 AND Gateway = 'hishop.plugins.payment.podrequest'))";
            }
            string str2 = "SELECT COUNT(1)  FROM Hishop_Orders o WHERE UserId = @UserId";
            str2 = str2 + str;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str2);
            sqlStringCommand.CommandType = CommandType.Text;
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            return (int) this.database.ExecuteScalar(sqlStringCommand);
        }

        public DataTable GetUserOrderPaidWaitFinish(int userId)
        {
            string str = " AND (OrderStatus = 2 or (OrderStatus = 3 and Gateway<>'hishop.plugins.payment.podrequest')) ";
            string query = "SELECT OrderId FROM Hishop_Orders WHERE UserId = @UserId";
            query = query + str + " ORDER BY OrderDate DESC";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public DataSet GetUserOrderReturn(int userId, OrderQuery query)
        {
            string str = string.Empty + " AND (OrderStatus = 2 OR OrderStatus = 3) ";
            string str2 = "SELECT OrderId, OrderDate, OrderStatus,PaymentTypeId, OrderTotal, (SELECT SUM(Quantity) FROM Hishop_OrderItems WHERE OrderId = o.OrderId) as ProductSum FROM Hishop_Orders o WHERE UserId = @UserId";
            str2 = (str2 + str + " ORDER BY OrderDate DESC") + " SELECT OrderId, ThumbnailsUrl,Quantity, ItemDescription,OrderItemsStatus, SKUContent, SKU, ProductId,SkuID FROM Hishop_OrderItems WHERE IsHandled=0 and Type=0 and (OrderItemsStatus=2 OR OrderItemsStatus=3) AND OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE UserId = @UserId " + str + ") ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(str2);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            DataSet set = this.database.ExecuteDataSet(sqlStringCommand);
            DataColumn parentColumn = set.Tables[0].Columns["OrderId"];
            DataColumn childColumn = set.Tables[1].Columns["OrderId"];
            DataRelation relation = new DataRelation("OrderItems", parentColumn, childColumn);
            set.Relations.Add(relation);
            return set;
        }

        public int GetUserOrderReturnCount(int userId)
        {
            object obj2 = string.Empty;
            string str = string.Concat(new object[] { obj2, " AND (OrderItemsStatus = ", 6, " OR OrderItemsStatus =", 7, ")" });
            string query = "SELECT COUNT(*) FROM Hishop_OrderItems WHERE OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE UserId=@UserId)";
            query = query + str;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            return (int) this.database.ExecuteScalar(sqlStringCommand);
        }

        public int GetUserOrders(int userId)
        {
            string query = "select count(OrderId) from Hishop_Orders WHERE UserId=@UserId";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int16, userId);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if ((obj2 != null) && !(obj2 is DBNull))
            {
                return (int) obj2;
            }
            return 0;
        }

        public bool InsertCalculationCommission(ArrayList UserIdList, ArrayList ReferralBlanceList, string orderid, ArrayList OrdersTotalList, string userid)
        {
            string query = "";
            query = query + "begin try  " + "  begin tran TranUpdate";
            for (int i = 0; i < UserIdList.Count; i++)
            {
                object obj2 = query;
                query = string.Concat(new object[] { obj2, " INSERT INTO [Hishop_Commissions]([UserId],[ReferralUserId],[OrderId],[OrderTotal],[CommTotal],[CommType],[State])VALUES(", UserIdList[i], ",", userid, ",'", orderid, "',", OrdersTotalList[i], ",", ReferralBlanceList[i], ",1,0);" });
            }
            query = query + " COMMIT TRAN TranUpdate" + "  end try \r\n                    begin catch \r\n                        ROLLBACK TRAN TranUpdate\r\n                    end catch ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool InsertPointExchange_Changed(PointExchangeChangedInfo info, DbTransaction dbTran, int itemCount = 1)
        {
            if (itemCount < 1)
            {
                return false;
            }
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < itemCount; i++)
            {
                builder.Append("INSERT INTO  Hishop_PointExchange_Changed ([exChangeId],[exChangeName],[ProductId],[PointNumber],[Date],[MemberID],[MemberGrades]) VALUES (@exChangeId,@exChangeName,@ProductId,@PointNumber,@Date,@MemberID,@MemberGrades);");
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "exChangeId", DbType.Int32, info.exChangeId);
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, info.ProductId);
            this.database.AddInParameter(sqlStringCommand, "exChangeName", DbType.String, info.exChangeName);
            this.database.AddInParameter(sqlStringCommand, "PointNumber", DbType.Int32, info.PointNumber);
            this.database.AddInParameter(sqlStringCommand, "Date", DbType.DateTime, DateTime.Now);
            this.database.AddInParameter(sqlStringCommand, "MemberID", DbType.Int32, info.MemberID);
            this.database.AddInParameter(sqlStringCommand, "MemberGrades", DbType.String, info.MemberGrades);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public int RealDeleteOrders(string orderIds)
        {
            if (!string.IsNullOrEmpty(orderIds) && !orderIds.Contains("'"))
            {
                orderIds = "'" + orderIds.Replace(",", "','") + "'";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("DELETE FROM Hishop_OrderItems WHERE OrderId IN({0});DELETE FROM Hishop_OrderReturns WHERE OrderId IN({0});", orderIds));
            this.database.ExecuteNonQuery(sqlStringCommand);
            sqlStringCommand = this.database.GetSqlStringCommand(string.Format("DELETE FROM Hishop_Orders WHERE OrderId IN({0})", orderIds));
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public int RestoreOrders(string orderIds)
        {
            if (!string.IsNullOrEmpty(orderIds) && !orderIds.Contains("'"))
            {
                orderIds = "'" + orderIds.Replace(",", "','") + "'";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("update Hishop_OrderItems set OrderItemsStatus =DeleteBeforeState  WHERE OrderId IN({0}); update Hishop_Orders set OrderStatus= DeleteBeforeState  WHERE OrderId IN({0}) ", orderIds));
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public bool SetOrderExpressComputerpe(string orderIds, string expressCompanyName, string expressCompanyAbb)
        {
            if (!string.IsNullOrEmpty(orderIds) && !orderIds.Contains("'"))
            {
                orderIds = "'" + orderIds.Replace(",", "','") + "'";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Hishop_Orders SET ExpressCompanyName=@ExpressCompanyName,ExpressCompanyAbb=@ExpressCompanyAbb WHERE (OrderStatus = 2 OR (OrderStatus = 1 AND Gateway='hishop.plugins.payment.podrequest')) AND OrderId IN ({0})", orderIds));
            this.database.AddInParameter(sqlStringCommand, "ExpressCompanyName", DbType.String, expressCompanyName);
            this.database.AddInParameter(sqlStringCommand, "ExpressCompanyAbb", DbType.String, expressCompanyAbb);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool SetOrderShippingMode(string orderIds, int realShippingModeId, string realModeName)
        {
            if (!string.IsNullOrEmpty(orderIds) && !orderIds.Contains("'"))
            {
                orderIds = "'" + orderIds.Replace(",", "','") + "'";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Hishop_Orders SET RealShippingModeId=@RealShippingModeId,RealModeName=@RealModeName WHERE (OrderStatus = 2 OR (OrderStatus = 1 AND Gateway='hishop.plugins.payment.podrequest')) AND OrderId IN ({0})", orderIds));
            this.database.AddInParameter(sqlStringCommand, "RealShippingModeId", DbType.Int32, realShippingModeId);
            this.database.AddInParameter(sqlStringCommand, "RealModeName", DbType.String, realModeName);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool SetPrintOrderExpress(string orderId, string expressCompanyName, string expressCompanyAbb, string shipOrderNumber)
        {
            string query = string.Empty;
            if (string.IsNullOrEmpty(shipOrderNumber))
            {
                query = "UPDATE Hishop_Orders SET ExpressCompanyName=@ExpressCompanyName,ExpressCompanyAbb=@ExpressCompanyAbb WHERE  OrderId=@OrderId";
            }
            else
            {
                query = "UPDATE Hishop_Orders SET IsPrinted=1,ShipOrderNumber=@ShipOrderNumber,ExpressCompanyName=@ExpressCompanyName,ExpressCompanyAbb=@ExpressCompanyAbb WHERE  OrderId=@OrderId";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "ShipOrderNumber", DbType.String, shipOrderNumber);
            this.database.AddInParameter(sqlStringCommand, "ExpressCompanyName", DbType.String, expressCompanyName);
            this.database.AddInParameter(sqlStringCommand, "ExpressCompanyAbb", DbType.String, expressCompanyAbb);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateCalculadtionCommission(OrderInfo orderinfo, DbTransaction dbTran = null)
        {
            foreach (LineItemInfo info in orderinfo.LineItems.Values)
            {
                if ((info.OrderItemsStatus == OrderStatus.Refunded) || (info.OrderItemsStatus == OrderStatus.Returned))
                {
                    new LineItemDao().UpdateCommissionItem(info.ID, 0M, 0M, 0M, dbTran);
                }
                else
                {
                    new LineItemDao().UpdateCommissionItem(info.ID, info.ItemsCommission, info.SecondItemsCommission, info.ThirdItemsCommission, dbTran);
                }
            }
            return true;
        }

        public bool UpdateCoupon_MemberCoupons(OrderInfo orderinfo, DbTransaction dbTran)
        {
            string query = "update Hishop_Coupon_MemberCoupons set OrderNo=@OrderNo, Status=@Status,UsedDate=@UsedDate WHERE Id=@Id;\r\n                        update Hishop_Coupon_Coupons set UsedNum=isnull(UsedNum,0)+1 where CouponId=(select top 1 CouponId From Hishop_Coupon_MemberCoupons where Id=@Id);";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "OrderNo", DbType.String, orderinfo.OrderId);
            this.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, orderinfo.RedPagerID);
            this.database.AddInParameter(sqlStringCommand, "Status", DbType.Int32, 1);
            this.database.AddInParameter(sqlStringCommand, "UsedDate", DbType.DateTime, DateTime.Now);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public void UpdateItemsStatus(string orderId, int status, string ItemStr)
        {
            string query = string.Empty;
            if (ItemStr == "all")
            {
                query = "Update Hishop_OrderItems Set OrderItemsStatus=@OrderItemsStatus Where OrderId =@OrderId";
            }
            else
            {
                query = "Update Hishop_OrderItems Set OrderItemsStatus=@OrderItemsStatus Where OrderId =@OrderId and SkuId IN (" + ItemStr + ")";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "OrderItemsStatus", DbType.Int32, status);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public bool UpdateOrder(OrderInfo order, DbTransaction dbTran = null)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Orders SET UpdateDate=getdate(), BalancePayFreightMoneyTotal=@BalancePayFreightMoneyTotal,BalancePayMoneyTotal=@BalancePayMoneyTotal,OrderStatus = @OrderStatus, CloseReason=@CloseReason, PayDate = @PayDate, ShippingDate=@ShippingDate, FinishDate = @FinishDate, RegionId = @RegionId, ShippingRegion = @ShippingRegion, Address = @Address, ZipCode = @ZipCode,ShipTo = @ShipTo, TelPhone = @TelPhone, CellPhone = @CellPhone, ShippingModeId=@ShippingModeId ,ModeName=@ModeName, RealShippingModeId = @RealShippingModeId, RealModeName = @RealModeName, ShipOrderNumber = @ShipOrderNumber,  ExpressCompanyName = @ExpressCompanyName,ExpressCompanyAbb = @ExpressCompanyAbb, PaymentTypeId=@PaymentTypeId,PaymentType=@PaymentType, Gateway = @Gateway, ManagerMark=@ManagerMark,ManagerRemark=@ManagerRemark,IsPrinted=@IsPrinted, OrderTotal = @OrderTotal, OrderProfit=@OrderProfit,Amount=@Amount,OrderCostPrice=@OrderCostPrice, AdjustedFreight = @AdjustedFreight, PayCharge = @PayCharge, AdjustedDiscount=@AdjustedDiscount,OrderPoint=@OrderPoint,GatewayOrderId=@GatewayOrderId,OldAddress=@OldAddress WHERE OrderId = @OrderId");
            decimal total = order.GetTotal();
            decimal balancePayMoneyTotal = order.GetBalancePayMoneyTotal();
            decimal balancePayFreightMoneyTotal = order.BalancePayFreightMoneyTotal;
            decimal point = Globals.GetPoint(total - order.AdjustedFreight);
            this.database.AddInParameter(sqlStringCommand, "BalancePayFreightMoneyTotal", DbType.Decimal, balancePayFreightMoneyTotal);
            this.database.AddInParameter(sqlStringCommand, "BalancePayMoneyTotal", DbType.Decimal, balancePayMoneyTotal);
            this.database.AddInParameter(sqlStringCommand, "OrderStatus", DbType.Int32, (int) order.OrderStatus);
            this.database.AddInParameter(sqlStringCommand, "CloseReason", DbType.String, order.CloseReason);
            this.database.AddInParameter(sqlStringCommand, "PayDate", DbType.DateTime, order.PayDate);
            this.database.AddInParameter(sqlStringCommand, "ShippingDate", DbType.DateTime, order.ShippingDate);
            this.database.AddInParameter(sqlStringCommand, "FinishDate", DbType.DateTime, order.FinishDate);
            this.database.AddInParameter(sqlStringCommand, "RegionId", DbType.String, order.RegionId);
            this.database.AddInParameter(sqlStringCommand, "ShippingRegion", DbType.String, order.ShippingRegion);
            this.database.AddInParameter(sqlStringCommand, "Address", DbType.String, order.Address);
            this.database.AddInParameter(sqlStringCommand, "ZipCode", DbType.String, order.ZipCode);
            this.database.AddInParameter(sqlStringCommand, "ShipTo", DbType.String, order.ShipTo);
            this.database.AddInParameter(sqlStringCommand, "TelPhone", DbType.String, order.TelPhone);
            this.database.AddInParameter(sqlStringCommand, "CellPhone", DbType.String, order.CellPhone);
            this.database.AddInParameter(sqlStringCommand, "ShippingModeId", DbType.Int32, order.ShippingModeId);
            this.database.AddInParameter(sqlStringCommand, "ModeName", DbType.String, order.ModeName);
            this.database.AddInParameter(sqlStringCommand, "RealShippingModeId", DbType.Int32, order.RealShippingModeId);
            this.database.AddInParameter(sqlStringCommand, "RealModeName", DbType.String, order.RealModeName);
            this.database.AddInParameter(sqlStringCommand, "ShipOrderNumber", DbType.String, order.ShipOrderNumber);
            this.database.AddInParameter(sqlStringCommand, "ExpressCompanyName", DbType.String, order.ExpressCompanyName);
            this.database.AddInParameter(sqlStringCommand, "ExpressCompanyAbb", DbType.String, order.ExpressCompanyAbb);
            this.database.AddInParameter(sqlStringCommand, "PaymentTypeId", DbType.Int32, order.PaymentTypeId);
            this.database.AddInParameter(sqlStringCommand, "PaymentType", DbType.String, order.PaymentType);
            this.database.AddInParameter(sqlStringCommand, "Gateway", DbType.String, order.Gateway);
            this.database.AddInParameter(sqlStringCommand, "ManagerMark", DbType.Int32, order.ManagerMark);
            this.database.AddInParameter(sqlStringCommand, "ManagerRemark", DbType.String, order.ManagerRemark);
            this.database.AddInParameter(sqlStringCommand, "IsPrinted", DbType.Boolean, order.IsPrinted);
            this.database.AddInParameter(sqlStringCommand, "OrderTotal", DbType.Currency, total);
            this.database.AddInParameter(sqlStringCommand, "OrderProfit", DbType.Currency, order.GetProfit());
            this.database.AddInParameter(sqlStringCommand, "Amount", DbType.Currency, order.GetAmount());
            this.database.AddInParameter(sqlStringCommand, "OrderCostPrice", DbType.Currency, order.GetCostPrice());
            this.database.AddInParameter(sqlStringCommand, "AdjustedFreight", DbType.Currency, order.AdjustedFreight);
            this.database.AddInParameter(sqlStringCommand, "PayCharge", DbType.Currency, order.PayCharge);
            this.database.AddInParameter(sqlStringCommand, "AdjustedDiscount", DbType.Currency, order.AdjustedDiscount);
            this.database.AddInParameter(sqlStringCommand, "OrderPoint", DbType.Int32, point);
            this.database.AddInParameter(sqlStringCommand, "GatewayOrderId", DbType.String, order.GatewayOrderId);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, order.OrderId);
            this.database.AddInParameter(sqlStringCommand, "OldAddress", DbType.String, order.OldAddress);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateOrderCompany(string orderId, string companycode, string companyname, string shipNumber)
        {
            string query = "UPDATE Hishop_Orders SET ShipOrderNumber=@ShipOrderNumber,ExpressCompanyAbb=@ExpressCompanyAbb,ExpressCompanyName=@ExpressCompanyName WHERE OrderId =@OrderId";
            if (string.IsNullOrEmpty(shipNumber))
            {
                query = "UPDATE Hishop_Orders SET ExpressCompanyAbb=@ExpressCompanyAbb,ExpressCompanyName=@ExpressCompanyName WHERE OrderId =@OrderId";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "ExpressCompanyAbb", DbType.String, companycode);
            this.database.AddInParameter(sqlStringCommand, "ShipOrderNumber", DbType.String, shipNumber);
            this.database.AddInParameter(sqlStringCommand, "ExpressCompanyName", DbType.String, companyname);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public void UpdateOrderItemBalance(string orderid)
        {
            OrderInfo orderInfo = this.GetOrderInfo(orderid);
            if (orderInfo != null)
            {
                decimal balancePayMoneyTotal = orderInfo.BalancePayMoneyTotal;
                orderInfo.BalancePayFreightMoneyTotal = 0M;
                bool flag = false;
                foreach (LineItemInfo info2 in orderInfo.LineItems.Values)
                {
                    if (!flag)
                    {
                        if ((info2.ItemAdjustedPrice - info2.ItemAdjustedCommssion) <= balancePayMoneyTotal)
                        {
                            balancePayMoneyTotal -= info2.ItemAdjustedPrice - info2.ItemAdjustedCommssion;
                            info2.BalancePayMoney = info2.ItemAdjustedPrice - info2.ItemAdjustedCommssion;
                        }
                        else
                        {
                            info2.BalancePayMoney = balancePayMoneyTotal;
                            balancePayMoneyTotal = 0M;
                        }
                        if (balancePayMoneyTotal <= 0M)
                        {
                            flag = true;
                        }
                    }
                    else
                    {
                        info2.BalancePayMoney = 0M;
                    }
                    new LineItemDao().UpdateBalancePayMoney(info2.ID, info2.BalancePayMoney, info2.OrderItemsStatus, null);
                }
                if (balancePayMoneyTotal > 0M)
                {
                    orderInfo.BalancePayFreightMoneyTotal = balancePayMoneyTotal;
                }
            }
            this.UpdateOrder(orderInfo, null);
        }

        public bool UpdateOrderItems(OrderInfo order)
        {
            string query = string.Empty;
            foreach (LineItemInfo info in order.LineItems.Values)
            {
                object obj2 = query;
                query = string.Concat(new object[] { obj2, "Update Hishop_OrderItems Set BalancePayMoney='", info.BalancePayMoney, "' Where Id ='", info.ID, "';" });
            }
            Globals.Debuglog("订单详细！！！" + query, "_DebuglogOrder.txt");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return (this.database.ExecuteNonQuery(sqlStringCommand) >= 1);
        }

        public bool UpdateOrderSplitState(string orderid, int splitstate, DbTransaction dbTran = null)
        {
            string query = "update Hishop_Orders set SplitState=@SplitState where OrderID=@OrderID";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderid);
            this.database.AddInParameter(sqlStringCommand, "SplitState", DbType.Int32, splitstate);
            return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
        }

        public void UpdatePayOrderStock(OrderInfo orderinfo)
        {
            int bargainDetialId = orderinfo.BargainDetialId;
            if (bargainDetialId > 0)
            {
                int quantity = 0;
                foreach (LineItemInfo info in orderinfo.LineItems.Values)
                {
                    quantity = info.Quantity;
                    break;
                }
                if (quantity > 0)
                {
                    string query = "update Hishop_Bargain set TranNumber=TranNumber+@Num where Id=(select BargainId from Hishop_BargainDetial where id=" + bargainDetialId + " AND IsDelete=0)";
                    DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
                    this.database.AddInParameter(sqlStringCommand, "Num", DbType.Int32, quantity);
                    this.database.ExecuteNonQuery(sqlStringCommand);
                }
            }
            this.UpdatePayOrderStock(orderinfo.OrderId);
        }

        public void UpdatePayOrderStock(string orderId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Hishop_SKUs Set Stock = CASE WHEN (Stock - (SELECT SUM(oi.ShipmentQuantity) FROM Hishop_OrderItems oi Where oi.SkuId =Hishop_SKUs.SkuId AND OrderId =@OrderId))<=0 Then 0 ELSE Stock - (SELECT SUM(oi.ShipmentQuantity) FROM Hishop_OrderItems oi  Where oi.SkuId =Hishop_SKUs.SkuId AND OrderId =@OrderId) END WHERE Hishop_SKUs.SkuId  IN (Select SkuId FROM Hishop_OrderItems Where OrderId =@OrderId)");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public bool UpdateRefundOrderStock(string orderId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Hishop_SKUs Set Stock = Stock + (SELECT SUM(oi.ShipmentQuantity) FROM Hishop_OrderItems oi  Where oi.SkuId =Hishop_SKUs.SkuId AND OrderId =@OrderId) WHERE Hishop_SKUs.SkuId  IN (Select SkuId FROM Hishop_OrderItems Where OrderId =@OrderId)");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) >= 1);
        }
    }
}

