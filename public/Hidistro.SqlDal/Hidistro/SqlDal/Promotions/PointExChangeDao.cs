namespace Hidistro.SqlDal.Promotions
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities;
    using Hidistro.Entities.Promotions;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Runtime.InteropServices;
    using System.Text;

    public class PointExChangeDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool AddProducts(int exchangeId, string productIds, string pNumbers, string points, string eachNumbers)
        {
            try
            {
                int num = 0;
                string[] strArray = productIds.Split(new char[] { ',' });
                string[] strArray2 = pNumbers.Split(new char[] { ',' });
                string[] strArray3 = points.Split(new char[] { ',' });
                string[] strArray4 = eachNumbers.Split(new char[] { ',' });
                for (int i = 0; i < strArray.Length; i++)
                {
                    DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT count(1) FROM Hishop_PointExChange_Products WHERE ProductId = @ProductId and exChangeId=@exChangeId");
                    this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, int.Parse(strArray[i]));
                    this.database.AddInParameter(sqlStringCommand, "exChangeId", DbType.Int32, exchangeId);
                    if (((int) this.database.ExecuteScalar(sqlStringCommand)) == 0)
                    {
                        DbCommand command = this.database.GetSqlStringCommand("INSERT INTO [Hishop_PointExChange_Products]([exChangeId],[ProductId],[status],[ProductNumber],[PointNumber],[ChangedNumber],eachMaxNumber) VALUES (@exChangeId,@ProductId,@status,@ProductNumber,@PointNumber,@ChangedNumber,@eachMaxNumber) update Hishop_PointExChange_PointExChanges set ProductNumber=ProductNumber+1 where Id=@exChangeId ");
                        this.database.AddInParameter(command, "exChangeId", DbType.Int32, exchangeId);
                        this.database.AddInParameter(command, "ProductId", DbType.Int32, int.Parse(strArray[i]));
                        this.database.AddInParameter(command, "status", DbType.Int32, 0);
                        this.database.AddInParameter(command, "ChangedNumber", DbType.Int32, 0);
                        this.database.AddInParameter(command, "ProductNumber", DbType.Int32, int.Parse(strArray2[i]));
                        this.database.AddInParameter(command, "PointNumber", DbType.Int32, int.Parse(strArray3[i]));
                        this.database.AddInParameter(command, "EachMaxNumber", DbType.Int32, int.Parse(strArray4[i]));
                        num += this.database.ExecuteNonQuery(command);
                    }
                }
                return (num > 0);
            }
            catch
            {
                return false;
            }
        }

        public int Create(PointExChangeInfo exchange, ref string msg)
        {
            msg = "未知错误";
            try
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Id  FROM Hishop_PointExChange_PointExChanges WHERE Name=@Name");
                this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, exchange.Name);
                if (Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) >= 1)
                {
                    msg = "积分兑换活动重名";
                    return 0;
                }
                sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO [Hishop_PointExChange_PointExChanges]([Name] ,[MemberGrades],[DefualtGroup],[CustomGroup],[BeginDate],[EndDate],[ProductNumber],[ImgUrl]) VALUES(@Name  ,@MemberGrades  ,@DefualtGroup  ,@CustomGroup  ,@BeginDate  ,@EndDate  ,@ProductNumber,@ImgUrl); SELECT CAST(scope_identity() AS int);");
                this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, exchange.Name);
                this.database.AddInParameter(sqlStringCommand, "MemberGrades", DbType.String, exchange.MemberGrades);
                this.database.AddInParameter(sqlStringCommand, "DefualtGroup", DbType.String, exchange.DefualtGroup);
                this.database.AddInParameter(sqlStringCommand, "CustomGroup", DbType.String, exchange.CustomGroup);
                this.database.AddInParameter(sqlStringCommand, "BeginDate", DbType.DateTime, exchange.BeginDate);
                this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, exchange.EndDate);
                this.database.AddInParameter(sqlStringCommand, "ProductNumber", DbType.Int32, exchange.ProductNumber);
                this.database.AddInParameter(sqlStringCommand, "ImgUrl", DbType.String, exchange.ImgUrl);
                int num = (int) this.database.ExecuteScalar(sqlStringCommand);
                msg = "";
                return num;
            }
            catch (Exception exception)
            {
                msg = exception.Message;
                return 0;
            }
        }

        public bool Delete(int Id)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_PointExChange_PointExChanges WHERE Id = @Id");
            this.database.AddInParameter(sqlStringCommand, "Id", DbType.Int32, Id);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool DeleteProduct(int exchangeId, int productId)
        {
            using (DbConnection connection = this.database.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("delete from Hishop_PointExChange_Products where [exChangeId]= @exChangeId and [ProductId] = @ProductId ");
                this.database.AddInParameter(sqlStringCommand, "exChangeId", DbType.Int32, exchangeId);
                this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
                DbCommand command = this.database.GetSqlStringCommand("update Hishop_PointExChange_PointExChanges set ProductNumber=ProductNumber-1 where Id=@Id");
                this.database.AddInParameter(command, "Id", DbType.Int32, exchangeId);
                try
                {
                    this.database.ExecuteNonQuery(sqlStringCommand, transaction);
                    this.database.ExecuteNonQuery(command, transaction);
                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
                finally
                {
                    if (transaction.Connection != null)
                    {
                        connection.Close();
                    }
                }
            }
            return false;
        }

        public bool DeleteProducts(int actId, string ProductIds)
        {
            using (DbConnection connection = this.database.CreateConnection())
            {
                connection.Open();
                DbTransaction transaction = connection.BeginTransaction();
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("Delete from Hishop_PointExChange_Products WHERE exChangeId ={0} and ProductId in ( {1} )", actId, ProductIds.ReplaceSingleQuoteMark()));
                DbCommand command = this.database.GetSqlStringCommand("update Hishop_PointExChange_PointExChanges set ProductNumber=ProductNumber-1 where Id=@Id");
                this.database.AddInParameter(command, "Id", DbType.Int32, actId);
                try
                {
                    this.database.ExecuteNonQuery(sqlStringCommand, transaction);
                    this.database.ExecuteNonQuery(command, transaction);
                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
                finally
                {
                    if (transaction.Connection != null)
                    {
                        connection.Close();
                    }
                }
            }
            return false;
        }

        public bool EditProducts(int exchangeId, string productIds, string pNumbers, string points, string eachNumbers)
        {
            try
            {
                string[] strArray = productIds.Split(new char[] { ',' });
                string[] strArray2 = pNumbers.Split(new char[] { ',' });
                string[] strArray3 = points.Split(new char[] { ',' });
                string[] strArray4 = eachNumbers.Split(new char[] { ',' });
                int num = 0;
                for (int i = 0; i < strArray.Length; i++)
                {
                    DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE [Hishop_PointExChange_Products] SET  [ProductNumber] = @ProductNumber  ,[PointNumber] = @PointNumber  ,[eachMaxNumber]=@eachMaxNumber where [exChangeId]= @exChangeId and [ProductId] = @ProductId");
                    this.database.AddInParameter(sqlStringCommand, "exChangeId", DbType.Int32, exchangeId);
                    this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, int.Parse(strArray[i]));
                    this.database.AddInParameter(sqlStringCommand, "ProductNumber", DbType.Int32, int.Parse(strArray2[i]));
                    this.database.AddInParameter(sqlStringCommand, "PointNumber", DbType.Int32, int.Parse(strArray3[i]));
                    this.database.AddInParameter(sqlStringCommand, "EachMaxNumber", DbType.Int32, int.Parse(strArray4[i]));
                    num += this.database.ExecuteNonQuery(sqlStringCommand);
                }
                return (num > 0);
            }
            catch
            {
                return false;
            }
        }

        public PointExChangeInfo GetExChange(int Id)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_PointExChange_PointExChanges WHERE ID = @ID");
            this.database.AddInParameter(sqlStringCommand, "ID", DbType.Int32, Id);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<PointExChangeInfo>(reader);
            }
        }

        public PointExChangeInfo GetExChange(string Name)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_PointExChange_PointExChanges WHERE Name = @Name");
            this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, Name);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<PointExChangeInfo>(reader);
            }
        }

        public int GetProductExchangedCount(int exchangeId, int productId)
        {
            try
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT count(id) FROM Hishop_PointExchange_Changed WHERE exChangeId = @exChangeId and ProductId=@ProductId");
                this.database.AddInParameter(sqlStringCommand, "exChangeId", DbType.Int32, exchangeId);
                this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
                return (int) this.database.ExecuteScalar(sqlStringCommand);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public PointExchangeProductInfo GetProductInfo(int exchangeId, int productId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_PointExChange_Products  where [exChangeId]= @exChangeId and [ProductId] = @ProductId");
            this.database.AddInParameter(sqlStringCommand, "exChangeId", DbType.Int32, exchangeId);
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<PointExchangeProductInfo>(reader);
            }
        }

        public DataTable GetProducts(int exchangeId)
        {
            DataTable table2;
            try
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * from Hishop_PointExChange_Products where [exChangeId]=@exChangeId");
                this.database.AddInParameter(sqlStringCommand, "exChangeId", DbType.Int32, exchangeId);
                using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
                {
                    table2 = DataHelper.ConverDataReaderToDataTable(reader);
                }
            }
            catch (Exception)
            {
                table2 = null;
            }
            return table2;
        }

        public DataTable GetProducts(int exchangeId, int pageNumber, int maxNum, out int total, string sort, bool order = false)
        {
            string filter = " status=0 and exChangeId=" + exchangeId;
            string selectFields = "ProductName,exChangeId,ProductId,status,ProductNumber,PointNumber,ChangedNumber,eachMaxNumber,ThumbnailUrl100,MemberGrades";
            DbQueryResult result = DataHelper.PagingByRownumber(pageNumber, maxNum, sort, order ? SortAction.Asc : SortAction.Desc, true, "vw_Hishop_PointExchange_Products", "ProductId", filter, selectFields);
            DataTable data = (DataTable) result.Data;
            total = result.TotalRecords;
            return data;
        }

        public int GetUserProductExchangedCount(int exchangeId, int productId, int userId)
        {
            try
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT count(id) FROM Hishop_PointExchange_Changed WHERE exChangeId = @exChangeId and ProductId=@ProductId and MemberID=@MemberID");
                this.database.AddInParameter(sqlStringCommand, "exChangeId", DbType.Int32, exchangeId);
                this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
                this.database.AddInParameter(sqlStringCommand, "MemberID", DbType.Int32, userId);
                return (int) this.database.ExecuteScalar(sqlStringCommand);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public bool InsertProduct(PointExchangeProductInfo product)
        {
            try
            {
                bool flag = false;
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT count(1) FROM Hishop_PointExChange_Products WHERE ProductId = @ProductId and exChangeId=@exChangeId");
                this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, product.ProductId);
                this.database.AddInParameter(sqlStringCommand, "exChangeId", DbType.Int32, product.exChangeId);
                if (((int) this.database.ExecuteScalar(sqlStringCommand)) == 0)
                {
                    DbCommand command = this.database.GetSqlStringCommand("INSERT INTO [Hishop_PointExChange_Products]([exChangeId],[ProductId],[status],[ProductNumber],[PointNumber],[ChangedNumber],eachMaxNumber) VALUES (@exChangeId,@ProductId,@status,@ProductNumber,@PointNumber,@ChangedNumber,@eachMaxNumber) update Hishop_PointExChange_PointExChanges set ProductNumber=ProductNumber+1 where Id=@exChangeId ");
                    this.database.AddInParameter(command, "exChangeId", DbType.Int32, product.exChangeId);
                    this.database.AddInParameter(command, "ProductId", DbType.Int32, product.ProductId);
                    this.database.AddInParameter(command, "status", DbType.Int32, product.status);
                    this.database.AddInParameter(command, "ProductNumber", DbType.Int32, product.ProductNumber);
                    this.database.AddInParameter(command, "PointNumber", DbType.Int32, product.PointNumber);
                    this.database.AddInParameter(command, "ChangedNumber", DbType.Int32, product.ChangedNumber);
                    this.database.AddInParameter(command, "EachMaxNumber", DbType.Int32, product.EachMaxNumber);
                    flag = this.database.ExecuteNonQuery(command) > 0;
                }
                return flag;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public DataTable Query(ExChangeSearch search, ref int total)
        {
            DataTable table3;
            try
            {
                StringBuilder builder = new StringBuilder(" 1=1 ");
                total = 0;
                if (!string.IsNullOrEmpty(search.ProductName))
                {
                    string str = string.Format(" and id in (select exchangeID from Hishop_PointExChange_Products a join [Hishop_Products] b on a.ProductId=b.ProductId where b.ProductName like '%{0}%' )", search.ProductName.ReplaceSingleQuoteMark());
                    builder.Append(str);
                }
                if (search.status != ExchangeStatus.All)
                {
                    if (search.status == ExchangeStatus.In)
                    {
                        builder.AppendFormat("and [BeginDate] <= '{0}' and [EndDate] >= '{0}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    else if (search.status == ExchangeStatus.End)
                    {
                        builder.AppendFormat("and [EndDate] < '{0}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    else if (search.status == ExchangeStatus.unBegin)
                    {
                        builder.AppendFormat("and [BeginDate] > '{0}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                }
                if (search.bFinished.HasValue)
                {
                    builder.AppendFormat("and [EndDate] < '{0}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                string query = "select count(id) as total from Hishop_PointExChange_PointExChanges where " + builder.ToString();
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
                using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
                {
                    DataTable table = DataHelper.ConverDataReaderToDataTable(reader);
                    total = int.Parse(table.Rows[0][0].ToString());
                }
                if (total <= 0)
                {
                    return null;
                }
                int pageSize = 0;
                int num2 = search.PageIndex * search.PageSize;
                if ((search.PageIndex != 0) && (search.PageSize != 0))
                {
                    pageSize = search.PageSize;
                    if (search.PageIndex >= Math.Ceiling((double) (((double) total) / ((double) search.PageSize))))
                    {
                        search.PageIndex = int.Parse(Math.Ceiling((double) (((double) total) / ((double) search.PageSize))).ToString());
                    }
                    int num3 = search.PageIndex * search.PageSize;
                    if (num3 > total)
                    {
                        pageSize = search.PageSize - (num3 - total);
                    }
                }
                string str3 = "(select sum(p.ProductNumber)  from Hishop_PointExChange_Products p where exChangeId=m.Id ) as TotalNumber";
                string str4 = "(select count(c.ProductId)  from Hishop_PointExchange_Changed c where exChangeId=m.Id ) as ExChangedNumber";
                string str5 = "order by m.ID desc ";
                string str6 = "order by ID ";
                string str7 = "order by ID desc ";
                string str8 = string.Format("select top {0} m.*  , {1},{2} from Hishop_PointExChange_PointExChanges m where ", num2, str3, str4);
                string str9 = string.Format("select * from ( select top {0} * from ( {1} ) as t1 {2} ) as t2 {3} ", new object[] { pageSize, str8 + builder.ToString() + str5, str6, str7 });
                sqlStringCommand = this.database.GetSqlStringCommand(str9);
                using (IDataReader reader2 = this.database.ExecuteReader(sqlStringCommand))
                {
                    table3 = DataHelper.ConverDataReaderToDataTable(reader2);
                }
            }
            catch (Exception)
            {
                table3 = null;
            }
            return table3;
        }

        public DataTable QueryExChanged(ExChangedProductSearch search)
        {
            DataTable table3;
            try
            {
                StringBuilder builder = new StringBuilder(" 1=1 ");
                int num = 0;
                if (search.exChangeId.HasValue)
                {
                    builder.Append(string.Format(" and exChangeID={0} ", search.exChangeId.Value));
                }
                string query = "select count(ProductId) as total from Hishop_PointExchange_Changed where " + builder.ToString();
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
                using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
                {
                    num = int.Parse(DataHelper.ConverDataReaderToDataTable(reader).Rows[0][0].ToString());
                }
                if (num <= 0)
                {
                    return null;
                }
                int pageSize = 0;
                int num3 = search.PageIndex * search.PageSize;
                if ((search.PageIndex != 0) && (search.PageSize != 0))
                {
                    pageSize = search.PageSize;
                    if (search.PageIndex >= Math.Ceiling((double) (((double) num) / ((double) search.PageSize))))
                    {
                        search.PageIndex = int.Parse(Math.Ceiling((double) (((double) num) / ((double) search.PageSize))).ToString());
                    }
                    int num4 = search.PageIndex * search.PageSize;
                    if (num4 > num)
                    {
                        pageSize = search.PageSize - (num4 - num);
                    }
                }
                string str2 = "order by a.ExChangeID desc , a.Date desc ";
                string str3 = "order by ExChangeID , Date  ";
                string str4 = "order by ExChangeID desc , Date desc ";
                string str5 = string.Format("select top {0} a.* , b.ProductName,b.ThumbnailUrl40 from Hishop_PointExchange_Changed a left join [Hishop_Products] b on a.ProductID=b.ProductID  where ", num3);
                string str6 = string.Format("select * from ( select top {0} * from ( {1} ) as t1 {2} ) as t2 {3} ", new object[] { pageSize, str5 + builder.ToString() + str2, str4, str3 });
                sqlStringCommand = this.database.GetSqlStringCommand(str6);
                using (IDataReader reader2 = this.database.ExecuteReader(sqlStringCommand))
                {
                    table3 = DataHelper.ConverDataReaderToDataTable(reader2);
                }
            }
            catch (Exception)
            {
                table3 = null;
            }
            return table3;
        }

        public bool SetProductsStatus(int actId, int status, string ProductIds)
        {
            try
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("Update Hishop_PointExChange_Products set status={0}  WHERE exChangeId ={1} and ProductId in ({2})", status, actId, ProductIds.ReplaceSingleQuoteMark()));
                this.database.ExecuteNonQuery(sqlStringCommand);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(PointExChangeInfo exchange, ref string msg)
        {
            msg = "未知错误";
            try
            {
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Id  FROM Hishop_PointExChange_PointExChanges WHERE Name=@Name and Id<>@ID");
                this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, exchange.Name);
                this.database.AddInParameter(sqlStringCommand, "ID", DbType.Int32, exchange.Id);
                if (Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) >= 1)
                {
                    msg = "积分兑换活动重名";
                    return false;
                }
                sqlStringCommand = this.database.GetSqlStringCommand("UPDATE [Hishop_PointExChange_PointExChanges] SET [Name] = @Name ,[MemberGrades] = @MemberGrades ,[DefualtGroup] = @DefualtGroup ,[CustomGroup] = @CustomGroup ,  [BeginDate] = @BeginDate ,[EndDate] = @EndDate, [ProductNumber] = @ProductNumber , [ImgUrl]=@ImgUrl where Id=@ID");
                this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, exchange.Name);
                this.database.AddInParameter(sqlStringCommand, "MemberGrades", DbType.String, exchange.MemberGrades);
                this.database.AddInParameter(sqlStringCommand, "DefualtGroup", DbType.String, exchange.DefualtGroup);
                this.database.AddInParameter(sqlStringCommand, "CustomGroup", DbType.String, exchange.CustomGroup);
                this.database.AddInParameter(sqlStringCommand, "BeginDate", DbType.DateTime, exchange.BeginDate);
                this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, exchange.EndDate);
                this.database.AddInParameter(sqlStringCommand, "ProductNumber", DbType.Int32, exchange.ProductNumber);
                this.database.AddInParameter(sqlStringCommand, "ID", DbType.String, exchange.Id);
                this.database.AddInParameter(sqlStringCommand, "ImgUrl", DbType.String, exchange.ImgUrl);
                this.database.ExecuteScalar(sqlStringCommand);
                msg = "";
                return true;
            }
            catch (Exception exception)
            {
                msg = exception.Message;
                return false;
            }
        }

        public bool UpdateProduct(PointExchangeProductInfo product)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE [Hishop_PointExChange_Products] SET  [exChangeId] = @exChangeId  ,[ProductId] = @ProductId  ,[status] = @status  ,[ProductNumber] = @ProductNumber  ,[PointNumber] = @PointNumber  ,[ChangedNumber] = @ChangedNumber ,[eachMaxNumber]=@eachMaxNumber where [exChangeId]= @exChangeId and [ProductId] = @ProductId");
            this.database.AddInParameter(sqlStringCommand, "exChangeId", DbType.Int32, product.exChangeId);
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, product.ProductId);
            this.database.AddInParameter(sqlStringCommand, "status", DbType.Int32, product.status);
            this.database.AddInParameter(sqlStringCommand, "ProductNumber", DbType.Int32, product.ProductNumber);
            this.database.AddInParameter(sqlStringCommand, "PointNumber", DbType.Int32, product.PointNumber);
            this.database.AddInParameter(sqlStringCommand, "ChangedNumber", DbType.Int32, product.ChangedNumber);
            this.database.AddInParameter(sqlStringCommand, "EachMaxNumber", DbType.Int32, product.EachMaxNumber);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

