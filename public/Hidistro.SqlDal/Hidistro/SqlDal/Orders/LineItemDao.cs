namespace Hidistro.SqlDal.Orders
{
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Orders;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;
    using System.Text;

    public class LineItemDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool AddOrderLineItems(string orderId, ICollection lineItems, DbTransaction dbTran)
        {
            if ((lineItems == null) || (lineItems.Count == 0))
            {
                return false;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            int num = 0;
            StringBuilder builder = new StringBuilder();
            foreach (LineItemInfo info in lineItems)
            {
                string str = num.ToString();
                builder.Append("INSERT INTO Hishop_OrderItems(OrderId, SkuId, ProductId, SKU, Quantity, ShipmentQuantity, CostPrice").Append(",ItemListPrice, ItemAdjustedPrice, ItemDescription, ThumbnailsUrl, Weight, SKUContent, PromotionId, PromotionName,OrderItemsStatus,ItemsCommission,SecondItemsCommission,ThirdItemsCommission,ItemsCommissionScale,SecondItemsCommissionScale,ThirdItemsCommissionScale,PointNumber,Type,DiscountAverage,LimitedTimeDiscountId,CommissionDiscount) VALUES(@OrderId").Append(",@SkuId").Append(str).Append(",@ProductId").Append(str).Append(",@SKU").Append(str).Append(",@Quantity").Append(str).Append(",@ShipmentQuantity").Append(str).Append(",@CostPrice").Append(str).Append(",@ItemListPrice").Append(str).Append(",@ItemAdjustedPrice").Append(str).Append(",@ItemDescription").Append(str).Append(",@ThumbnailsUrl").Append(str).Append(",@Weight").Append(str).Append(",@SKUContent").Append(str).Append(",@PromotionId").Append(str).Append(",@PromotionName").Append(str).Append(",@OrderItemsStatus").Append(str).Append(",@ItemsCommission").Append(str).Append(",@SecondItemsCommission").Append(str).Append(",@ThirdItemsCommission").Append(str).Append(",@ItemsCommissionScale").Append(str).Append(",@SecondItemsCommissionScale").Append(str).Append(",@ThirdItemsCommissionScale").Append(str).Append(",@PointNumber").Append(str).Append(",@Type").Append(str).Append(",@DiscountAverage").Append(str).Append(",@LimitedTimeDiscountId").Append(str).Append(",@CommissionDiscount").Append(str).Append(");");
                this.database.AddInParameter(sqlStringCommand, "SkuId" + str, DbType.String, info.SkuId);
                this.database.AddInParameter(sqlStringCommand, "ProductId" + str, DbType.Int32, info.ProductId);
                this.database.AddInParameter(sqlStringCommand, "SKU" + str, DbType.String, info.SKU);
                this.database.AddInParameter(sqlStringCommand, "Quantity" + str, DbType.Int32, info.Quantity);
                this.database.AddInParameter(sqlStringCommand, "ShipmentQuantity" + str, DbType.Int32, info.ShipmentQuantity);
                this.database.AddInParameter(sqlStringCommand, "CostPrice" + str, DbType.Currency, info.ItemCostPrice);
                this.database.AddInParameter(sqlStringCommand, "ItemListPrice" + str, DbType.Currency, info.ItemListPrice);
                this.database.AddInParameter(sqlStringCommand, "ItemAdjustedPrice" + str, DbType.Currency, info.ItemAdjustedPrice);
                this.database.AddInParameter(sqlStringCommand, "ItemDescription" + str, DbType.String, info.ItemDescription);
                this.database.AddInParameter(sqlStringCommand, "ThumbnailsUrl" + str, DbType.String, info.ThumbnailsUrl);
                this.database.AddInParameter(sqlStringCommand, "Weight" + str, DbType.Int32, info.ItemWeight);
                this.database.AddInParameter(sqlStringCommand, "SKUContent" + str, DbType.String, info.SKUContent);
                this.database.AddInParameter(sqlStringCommand, "PromotionId" + str, DbType.Int32, info.PromotionId);
                this.database.AddInParameter(sqlStringCommand, "PromotionName" + str, DbType.String, info.PromotionName);
                this.database.AddInParameter(sqlStringCommand, "OrderItemsStatus" + str, DbType.Int32, (int) info.OrderItemsStatus);
                this.database.AddInParameter(sqlStringCommand, "ItemsCommission" + str, DbType.Decimal, info.ItemsCommission);
                this.database.AddInParameter(sqlStringCommand, "SecondItemsCommission" + str, DbType.Decimal, info.SecondItemsCommission);
                this.database.AddInParameter(sqlStringCommand, "ThirdItemsCommission" + str, DbType.Decimal, info.ThirdItemsCommission);
                this.database.AddInParameter(sqlStringCommand, "ItemsCommissionScale" + str, DbType.Decimal, info.ItemsCommissionScale);
                this.database.AddInParameter(sqlStringCommand, "SecondItemsCommissionScale" + str, DbType.Decimal, info.SecondItemsCommissionScale);
                this.database.AddInParameter(sqlStringCommand, "ThirdItemsCommissionScale" + str, DbType.Decimal, info.ThirdItemsCommissionScale);
                this.database.AddInParameter(sqlStringCommand, "PointNumber" + str, DbType.Int32, info.PointNumber);
                this.database.AddInParameter(sqlStringCommand, "Type" + str, DbType.Int32, info.Type);
                this.database.AddInParameter(sqlStringCommand, "DiscountAverage" + str, DbType.Decimal, info.DiscountAverage);
                this.database.AddInParameter(sqlStringCommand, "LimitedTimeDiscountId" + str, DbType.Decimal, info.LimitedTimeDiscountId);
                this.database.AddInParameter(sqlStringCommand, "CommissionDiscount" + str, DbType.Int32, info.CommissionDiscount);
                num++;
                if (num == 50)
                {
                    int num2;
                    sqlStringCommand.CommandText = builder.ToString();
                    if (dbTran != null)
                    {
                        num2 = this.database.ExecuteNonQuery(sqlStringCommand, dbTran);
                    }
                    else
                    {
                        num2 = this.database.ExecuteNonQuery(sqlStringCommand);
                    }
                    if (num2 <= 0)
                    {
                        return false;
                    }
                    builder.Remove(0, builder.Length);
                    sqlStringCommand.Parameters.Clear();
                    this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
                    num = 0;
                }
            }
            if (builder.ToString().Length <= 0)
            {
                return true;
            }
            sqlStringCommand.CommandText = builder.ToString();
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool DeleteLineItem(string skuId, string orderId, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_OrderItems WHERE OrderId=@OrderId AND SkuId=@SkuId ");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) == 1);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public int GetItemNumByOrderID(string orderid)
        {
            string query = "select count(0) from Hishop_OrderItems where OrderId=@OrderId ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderid);
            return Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand));
        }

        public LineItemInfo GetLineItemInfo(int id, string orderid)
        {
            string.IsNullOrEmpty(orderid);
            string query = "select top 1 * from Hishop_OrderItems where Id=@Id ";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, id);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderid);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<LineItemInfo>(reader);
            }
        }

        public LineItemInfo GetReturnMoneyByOrderIDAndProductID(string orderId, string skuid, int itemid)
        {
            string query = "select top 1 * from Hishop_OrderItems where OrderId=@OrderId and ID=@ID";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuid);
            this.database.AddInParameter(sqlStringCommand, "ID", DbType.Int32, itemid);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<LineItemInfo>(reader);
            }
        }

        public bool UpdateBalancePayMoney(int itemid, decimal balancePayMoney, OrderStatus orderStatus, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_OrderItems SET BalancePayMoney=@BalancePayMoney,OrderItemsStatus=@OrderItemsStatus WHERE Id=" + itemid);
            this.database.AddInParameter(sqlStringCommand, "BalancePayMoney", DbType.Decimal, balancePayMoney);
            this.database.AddInParameter(sqlStringCommand, "OrderItemsStatus", DbType.Int32, (int) orderStatus);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateCommissionItem(int id, decimal itemsCommission, decimal secondItemsCommission, decimal thirdItemsCommission, DbTransaction dbTran = null)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Hishop_OrderItems Set ItemsCommission=@ItemsCommission,SecondItemsCommission=@SecondItemsCommission,ThirdItemsCommission=@ThirdItemsCommission,IsAdminModify=1 Where ID=@ID");
            this.database.AddInParameter(sqlStringCommand, "ID", DbType.Int32, id);
            this.database.AddInParameter(sqlStringCommand, "ItemsCommission", DbType.Currency, itemsCommission);
            this.database.AddInParameter(sqlStringCommand, "SecondItemsCommission", DbType.Currency, secondItemsCommission);
            this.database.AddInParameter(sqlStringCommand, "ThirdItemsCommission", DbType.Currency, thirdItemsCommission);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateLineItem(string orderId, LineItemInfo lineItem, DbTransaction dbTran)
        {
            string str = string.Empty;
            if (!lineItem.IsAdminModify)
            {
                str = "IsAdminModify=0,";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Concat(new object[] { "UPDATE Hishop_OrderItems SET ", str, "ShipmentQuantity=@ShipmentQuantity,ItemAdjustedPrice=@ItemAdjustedPrice,ItemAdjustedCommssion=@ItemAdjustedCommssion,OrderItemsStatus=@OrderItemsStatus,ItemsCommission=@ItemsCommission,Quantity=@Quantity, PromotionId = NULL, PromotionName = NULL WHERE OrderId=@OrderId AND SkuId=@SkuId And ID=", lineItem.ID }));
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderId);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, lineItem.SkuId);
            this.database.AddInParameter(sqlStringCommand, "ShipmentQuantity", DbType.Int32, lineItem.ShipmentQuantity);
            this.database.AddInParameter(sqlStringCommand, "ItemAdjustedPrice", DbType.Currency, lineItem.ItemAdjustedPrice);
            this.database.AddInParameter(sqlStringCommand, "Quantity", DbType.Int32, lineItem.Quantity);
            this.database.AddInParameter(sqlStringCommand, "ItemAdjustedCommssion", DbType.Currency, lineItem.ItemAdjustedCommssion);
            this.database.AddInParameter(sqlStringCommand, "OrderItemsStatus", DbType.Int16, (int) lineItem.OrderItemsStatus);
            this.database.AddInParameter(sqlStringCommand, "ItemsCommission", DbType.Currency, lineItem.ItemsCommission);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) == 1);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
        }

        public bool UpdateLineItemOrderID(string itemIDList, string orderid, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_OrderItems SET OrderId=@OrderId WHERE Id in(" + itemIDList + ")");
            this.database.AddInParameter(sqlStringCommand, "OrderId", DbType.String, orderid);
            if (dbTran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

