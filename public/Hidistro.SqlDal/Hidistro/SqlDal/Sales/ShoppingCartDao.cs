namespace Hidistro.SqlDal.Sales
{
    using Hidistro.Core;
    using Hidistro.Entities.Bargain;
    using Hidistro.Entities.Commodities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.Sales;
    using Hidistro.SqlDal.Bargain;
    using Hidistro.SqlDal.Commodities;
    using Hidistro.SqlDal.Promotions;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;
    using System.Text;

    public class ShoppingCartDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public void AddLineItem(MemberInfo member, string skuId, int quantity, int categoryid, int Templateid, int type, int exchangeId, int limitedTimeDiscountId)
        {
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_ShoppingCart_AddLineItem");
            this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, member.UserId);
            this.database.AddInParameter(storedProcCommand, "SkuId", DbType.String, skuId);
            this.database.AddInParameter(storedProcCommand, "Quantity", DbType.Int32, quantity);
            this.database.AddInParameter(storedProcCommand, "CategoryId", DbType.Int32, categoryid);
            this.database.AddInParameter(storedProcCommand, "Templateid", DbType.Int32, Templateid);
            this.database.AddInParameter(storedProcCommand, "Type", DbType.Int32, type);
            this.database.AddInParameter(storedProcCommand, "ExchangeId", DbType.Int32, exchangeId);
            this.database.AddInParameter(storedProcCommand, "LimitedTimeDiscountId", DbType.Int32, limitedTimeDiscountId);
            this.database.ExecuteNonQuery(storedProcCommand);
        }

        public void ClearShoppingCart(int userId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ShoppingCarts WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public DataTable GetAllFull(int ActivitiesType)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select ReductionMoney,ActivitiesId,ActivitiesName,MeetMoney,ActivitiesType from Hishop_Activities where datediff(dd,GETDATE(),StartTime)<=0 and datediff(dd,GETDATE(),EndTIme)>=0 and (ActivitiesType=0 or ActivitiesType=" + ActivitiesType + ")  order by MeetMoney asc");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public ShoppingCartItemInfo GetCartItemInfo(MemberInfo member, string skuId, int quantity, int type = 0, int bargainDetialId = 0, int limitedTimeDiscountId = 0)
        {
            ShoppingCartItemInfo info = null;
            DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_ShoppingCart_GetItemInfo");
            this.database.AddInParameter(storedProcCommand, "Quantity", DbType.Int32, quantity);
            this.database.AddInParameter(storedProcCommand, "UserId", DbType.Int32, (member != null) ? member.UserId : 0);
            this.database.AddInParameter(storedProcCommand, "SkuId", DbType.String, skuId);
            this.database.AddInParameter(storedProcCommand, "GradeId", DbType.Int32, (member != null) ? member.GradeId : 0);
            this.database.AddInParameter(storedProcCommand, "Type", DbType.Int32, type);
            using (IDataReader reader = this.database.ExecuteReader(storedProcCommand))
            {
                if (!reader.Read())
                {
                    return info;
                }
                info = new ShoppingCartItemInfo {
                    SkuId = skuId,
                    Quantity = quantity,
                    MainCategoryPath = reader["MainCategoryPath"].ToString(),
                    ProductId = (int) reader["ProductId"]
                };
                if (DBNull.Value != reader["CubicMeter"])
                {
                    info.CubicMeter = (decimal) reader["CubicMeter"];
                }
                if (DBNull.Value != reader["FreightWeight"])
                {
                    info.FreightWeight = (decimal) reader["FreightWeight"];
                }
                if (reader["SKU"] != DBNull.Value)
                {
                    info.SKU = (string) reader["SKU"];
                }
                info.Name = (string) reader["ProductName"];
                if (DBNull.Value != reader["Weight"])
                {
                    info.Weight = (int) reader["Weight"];
                }
                if (DBNull.Value != reader["FreightTemplateId"])
                {
                    info.FreightTemplateId = (int) reader["FreightTemplateId"];
                }
                else
                {
                    info.FreightTemplateId = 0;
                }
                if (DBNull.Value != reader["ThirdCommission"])
                {
                    info.ThirdCommission = (decimal) reader["ThirdCommission"];
                }
                else
                {
                    info.ThirdCommission = 0M;
                }
                if (DBNull.Value != reader["SecondCommission"])
                {
                    info.SecondCommission = (decimal) reader["SecondCommission"];
                }
                else
                {
                    info.SecondCommission = 0M;
                }
                if (DBNull.Value != reader["FirstCommission"])
                {
                    info.FirstCommission = (decimal) reader["FirstCommission"];
                }
                else
                {
                    info.FirstCommission = 0M;
                }
                if (DBNull.Value != reader["IsSetCommission"])
                {
                    info.IsSetCommission = (bool) reader["IsSetCommission"];
                }
                else
                {
                    info.IsSetCommission = false;
                }
                BargainDetialInfo bargainDetialInfo = null;
                if (bargainDetialId > 0)
                {
                    bargainDetialInfo = new BargainDao().GetBargainDetialInfo(bargainDetialId);
                }
                if ((bargainDetialId > 0) && (bargainDetialInfo != null))
                {
                    info.MemberPrice = info.AdjustedPrice = bargainDetialInfo.Price;
                }
                else
                {
                    info.MemberPrice = info.AdjustedPrice = decimal.Round((decimal) reader["SalePrice"], 2);
                }
                if (limitedTimeDiscountId > 0)
                {
                    LimitedTimeDiscountProductInfo info3 = new LimitedTimeDiscountDao().GetLimitedTimeDiscountProductByLimitIdAndProductIdAndUserId(limitedTimeDiscountId, info.ProductId, (member != null) ? member.UserId : 0);
                    if (((info3 != null) && (info3.BeginTime <= DateTime.Now)) && (DateTime.Now < info3.EndTime))
                    {
                        info.MemberPrice = info.AdjustedPrice = decimal.Round(info3.FinalPrice, 2);
                        info.LimitedTimeDiscountId = limitedTimeDiscountId;
                    }
                    else
                    {
                        info.LimitedTimeDiscountId = 0;
                    }
                }
                else
                {
                    info.LimitedTimeDiscountId = 0;
                }
                if (DBNull.Value != reader["ThumbnailUrl40"])
                {
                    info.ThumbnailUrl40 = reader["ThumbnailUrl40"].ToString();
                }
                if (DBNull.Value != reader["ThumbnailUrl60"])
                {
                    info.ThumbnailUrl60 = reader["ThumbnailUrl60"].ToString();
                }
                if (DBNull.Value != reader["ThumbnailUrl100"])
                {
                    info.ThumbnailUrl100 = reader["ThumbnailUrl100"].ToString();
                }
                if (DBNull.Value != reader["IsfreeShipping"])
                {
                    info.IsfreeShipping = Convert.ToBoolean(reader["IsfreeShipping"]);
                }
                string str = string.Empty;
                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        if (((reader["AttributeName"] != DBNull.Value) && !string.IsNullOrEmpty((string) reader["AttributeName"])) && ((reader["ValueStr"] != DBNull.Value) && !string.IsNullOrEmpty((string) reader["ValueStr"])))
                        {
                            object obj2 = str;
                            str = string.Concat(new object[] { obj2, reader["AttributeName"], "：", reader["ValueStr"], "; " });
                        }
                    }
                }
                info.SkuContent = str;
                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        info.Type = 1;
                        if (DBNull.Value != reader["ProductNumber"])
                        {
                            info.ProductNumber = Convert.ToInt32(reader["ProductNumber"]);
                        }
                        if (DBNull.Value != reader["PointNumber"])
                        {
                            info.PointNumber = Convert.ToInt32(reader["PointNumber"]);
                        }
                        if (DBNull.Value != reader["status"])
                        {
                            info.Status = Convert.ToInt32(reader["status"]);
                        }
                        if (DBNull.Value != reader["exChangeId"])
                        {
                            info.ExchangeId = Convert.ToInt32(reader["exChangeId"]);
                        }
                    }
                    return info;
                }
                info.Type = 0;
            }
            return info;
        }

        public int GetLimitedTimeDiscountUsedNum(int limitedTimeDiscountId, string skuId, int productId, int userid, bool isContainsShippingCart)
        {
            int num = 0;
            if (userid == -1)
            {
                userid = Globals.GetCurrentMemberUserId(false);
            }
            if (productId <= 0)
            {
                SKUItem skuItem = new SkuDao().GetSkuItem(skuId);
                if (skuItem != null)
                {
                    productId = skuItem.ProductId;
                }
            }
            if (((productId > 0) && (limitedTimeDiscountId > 0)) && (userid > 0))
            {
                string query = string.Concat(new object[] { "select isnull(sum(Quantity),0) from Hishop_OrderItems a inner join Hishop_Orders b on a.OrderId=b.OrderId where a.LimitedTimeDiscountId=", limitedTimeDiscountId, " and a.ProductId=", productId, " and b.UserID=", userid, " and a.OrderItemsStatus<>4" });
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
                num = Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand));
                if (isContainsShippingCart)
                {
                    query = string.Concat(new object[] { "select isnull(sum(Quantity),0) from Hishop_ShoppingCarts where UserId=", userid, " and SkuId in(select SkuId from Hishop_SKUs where ProductId=", productId, ") and LimitedTimeDiscountId=", limitedTimeDiscountId });
                    sqlStringCommand = this.database.GetSqlStringCommand(query);
                    this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
                    num += Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand));
                }
            }
            return num;
        }

        public List<ShoppingCartInfo> GetOrderSummitCart(MemberInfo member)
        {
            List<ShoppingCartInfo> list = new List<ShoppingCartInfo>();
            DataTable template = this.GetTemplate(member);
            DataTable shoppingTemplateid = new DataTable();
            for (int i = 0; i < template.Rows.Count; i++)
            {
                decimal num2 = 0M;
                int num3 = 0;
                ShoppingCartInfo item = new ShoppingCartInfo {
                    TemplateId = template.Rows[i]["TemplateId"].ToString()
                };
                shoppingTemplateid = this.GetShoppingTemplateid(item.TemplateId, member);
                for (int j = 0; j < shoppingTemplateid.Rows.Count; j++)
                {
                    ShoppingCartItemInfo info2 = this.GetCartItemInfo(member, shoppingTemplateid.Rows[j]["SkuId"].ToString(), int.Parse(shoppingTemplateid.Rows[j]["Quantity"].ToString()), int.Parse(shoppingTemplateid.Rows[j]["Type"].ToString()), 0, Globals.ToNum(shoppingTemplateid.Rows[j]["LimitedTimeDiscountId"].ToString()));
                    if (info2 != null)
                    {
                        if (info2.Type == 0)
                        {
                            item.Amount = decimal.Round(info2.SubTotal, 2);
                            num2 += item.Amount;
                            item.Exemption = 0M;
                            item.ShipCost = 0M;
                            info2.ExchangeId = 0;
                        }
                        else
                        {
                            num3 += info2.PointNumber * info2.Quantity;
                            info2.ExchangeId = int.Parse(shoppingTemplateid.Rows[j]["ExchangeId"].ToString());
                        }
                        item.LineItems.Add(info2);
                    }
                }
                item.Total = num2;
                item.GetPointNumber = num3;
                item.MemberPointNumber = member.Points;
                list.Add(item);
            }
            return list;
        }

        public DataTable GetShopping(string CategoryId, MemberInfo member, int type)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(string.Concat(new object[] { "select * from Hishop_ShoppingCarts where CategoryId=", CategoryId, " and UserId = ", member.UserId, " and [Type]=", type }));
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public ShoppingCartInfo GetShoppingCart(MemberInfo member)
        {
            ShoppingCartInfo info = new ShoppingCartInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ShoppingCarts WHERE UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, member.UserId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    ShoppingCartItemInfo item = this.GetCartItemInfo(member, (string) reader["SkuId"], (int) reader["Quantity"], 0, 0, 0);
                    if (item != null)
                    {
                        info.LineItems.Add(item);
                    }
                }
            }
            return info;
        }

        public ShoppingCartInfo GetShoppingCart(MemberInfo member, int Templateid)
        {
            ShoppingCartInfo info = new ShoppingCartInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ShoppingCarts WHERE UserId = @UserId and Templateid=@Templateid");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, member.UserId);
            this.database.AddInParameter(sqlStringCommand, "Templateid", DbType.Int32, Templateid);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    ShoppingCartItemInfo item = this.GetCartItemInfo(member, (string) reader["SkuId"], (int) reader["Quantity"], 0, 0, 0);
                    if (item != null)
                    {
                        info.LineItems.Add(item);
                    }
                }
            }
            return info;
        }

        public List<ShoppingCartInfo> GetShoppingCartAviti(MemberInfo member, int type)
        {
            List<ShoppingCartInfo> list = new List<ShoppingCartInfo>();
            DataTable shoppingCategoryId = this.GetShoppingCategoryId(member, type);
            DataTable table2 = new DataTable();
            for (int i = 0; i < shoppingCategoryId.Rows.Count; i++)
            {
                ShoppingCartInfo item = new ShoppingCartInfo {
                    CategoryId = int.Parse(shoppingCategoryId.Rows[i]["CategoryId"].ToString())
                };
                table2 = this.GetShopping(item.CategoryId.ToString(), member, type);
                for (int j = 0; j < table2.Rows.Count; j++)
                {
                    ShoppingCartItemInfo info2 = this.GetCartItemInfo(member, table2.Rows[j]["SkuId"].ToString(), int.Parse(table2.Rows[j]["Quantity"].ToString()), type, 0, Globals.ToNum(table2.Rows[j]["limitedTimeDiscountId"].ToString()));
                    if (info2 != null)
                    {
                        item.LineItems.Add(info2);
                    }
                }
                list.Add(item);
            }
            return list;
        }

        public DataTable GetShoppingCategoryId(MemberInfo member, int type)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(string.Concat(new object[] { "select distinct CategoryId from Hishop_ShoppingCarts where userid=", member.UserId, " and type=", type }));
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public DataTable GetShoppingTemplateid(string TemplateId, MemberInfo member)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(string.Concat(new object[] { "select * from Hishop_ShoppingCarts where TemplateId=", TemplateId, " and UserId = ", member.UserId, " order by Type" }));
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public DataTable GetTemplate(MemberInfo member)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select distinct TemplateId from Hishop_ShoppingCarts where userid=" + member.UserId);
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public void RemoveLineItem(int userId, string skuId, int type, int limitedTimeDiscountId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ShoppingCarts WHERE UserId = @UserId AND SkuId = @SkuId And [Type]=@Type And LimitedTimeDiscountId=" + limitedTimeDiscountId);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            this.database.AddInParameter(sqlStringCommand, "Type", DbType.Int32, type);
            if (this.database.ExecuteNonQuery(sqlStringCommand) == 0)
            {
                sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ShoppingCarts WHERE UserId = @UserId AND SkuId = @SkuId And [Type]=@Type");
                this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, userId);
                this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
                this.database.AddInParameter(sqlStringCommand, "Type", DbType.Int32, type);
                this.database.ExecuteNonQuery(sqlStringCommand);
            }
        }

        public void UpdateLineItemQuantity(MemberInfo member, string skuId, int quantity, int type, int limitedTimeDiscountId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_ShoppingCarts SET Quantity = @Quantity WHERE UserId = @UserId AND SkuId = @SkuId And [Type]=@Type AND LimitedTimeDiscountId=" + limitedTimeDiscountId);
            this.database.AddInParameter(sqlStringCommand, "Quantity", DbType.Int32, quantity);
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int32, member.UserId);
            this.database.AddInParameter(sqlStringCommand, "SkuId", DbType.String, skuId);
            this.database.AddInParameter(sqlStringCommand, "Type", DbType.Int32, type);
            this.database.ExecuteNonQuery(sqlStringCommand);
        }
    }
}

