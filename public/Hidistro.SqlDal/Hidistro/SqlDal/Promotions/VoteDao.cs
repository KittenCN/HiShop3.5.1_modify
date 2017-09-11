namespace Hidistro.SqlDal.Promotions
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Promotions;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;

    public class VoteDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public long CreateVote(VoteInfo vote)
        {
            try
            {
                string query = "INSERT INTO [Hishop_Votes]([VoteName],[IsBackup] ,[MaxCheck],[ImageUrl],[StartDate],[EndDate],[Description],[MemberGrades],[DefualtGroup],[CustomGroup],[IsMultiCheck])VALUES (@VoteName,@IsBackup,@MaxCheck,@ImageUrl,@StartDate,@EndDate,@Description,@MemberGrades,@DefualtGroup,@CustomGroup,@IsMultiCheck); SELECT CAST(scope_identity() AS int);";
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
                this.database.AddInParameter(sqlStringCommand, "VoteName", DbType.String, vote.VoteName);
                this.database.AddInParameter(sqlStringCommand, "IsBackup", DbType.Boolean, false);
                this.database.AddInParameter(sqlStringCommand, "MaxCheck", DbType.Int32, vote.MaxCheck);
                this.database.AddInParameter(sqlStringCommand, "ImageUrl", DbType.String, vote.ImageUrl);
                this.database.AddInParameter(sqlStringCommand, "StartDate", DbType.DateTime, vote.StartDate);
                this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, vote.EndDate);
                this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, vote.Description);
                this.database.AddInParameter(sqlStringCommand, "MemberGrades", DbType.String, vote.MemberGrades);
                this.database.AddInParameter(sqlStringCommand, "DefualtGroup", DbType.String, vote.DefualtGroup);
                this.database.AddInParameter(sqlStringCommand, "CustomGroup", DbType.String, vote.CustomGroup);
                this.database.AddInParameter(sqlStringCommand, "IsMultiCheck", DbType.Boolean, vote.IsMultiCheck);
                int num = (int) this.database.ExecuteScalar(sqlStringCommand);
                vote.VoteId = num;
                if ((vote.VoteItems != null) && (vote.VoteItems.Count > 0))
                {
                    foreach (VoteItemInfo info in vote.VoteItems)
                    {
                        string str2 = "INSERT INTO [Hishop_VoteItems]([VoteId],[VoteItemName],[ItemCount]) VALUES (@VoteId,@VoteItemName,@ItemCount)";
                        sqlStringCommand = this.database.GetSqlStringCommand(str2);
                        this.database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, num);
                        this.database.AddInParameter(sqlStringCommand, "VoteItemName", DbType.String, info.VoteItemName);
                        this.database.AddInParameter(sqlStringCommand, "ItemCount", DbType.Int32, 0);
                        this.database.ExecuteNonQuery(sqlStringCommand);
                    }
                }
                return num;
            }
            catch (Exception)
            {
                return 0L;
            }
        }

        public bool CreateVoteItem(VoteItemInfo item)
        {
            try
            {
                string query = string.Format("INSERT INTO [Hishop_VoteItems] ([VoteId],[VoteItemName],[ItemCount]) VALUES ({0},'{1}',{2})", item.VoteId, item.VoteItemName, item.ItemCount);
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
                return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int CreateVoteItem(VoteItemInfo voteItem, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_VoteItems(VoteId, VoteItemName, ItemCount) Values(@VoteId, @VoteItemName, @ItemCount)");
            this.database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, voteItem.VoteId);
            this.database.AddInParameter(sqlStringCommand, "VoteItemName", DbType.String, voteItem.VoteItemName);
            this.database.AddInParameter(sqlStringCommand, "ItemCount", DbType.Int32, voteItem.ItemCount);
            if (dbTran == null)
            {
                return this.database.ExecuteNonQuery(sqlStringCommand);
            }
            return this.database.ExecuteNonQuery(sqlStringCommand, dbTran);
        }

        public bool DeleteItem(long itemId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("delete from Hishop_VoteItems where VoteItemId=@ItemId");
            this.database.AddInParameter(sqlStringCommand, "ItemId", DbType.Int64, itemId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public int DeleteVote(long voteId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_Votes WHERE VoteId = @VoteId; DELETE FROM Hishop_VoteItems WHERE VoteId = @VoteId;");
            this.database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, voteId);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public bool DeleteVoteItem(long voteId, DbTransaction dbTran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_VoteItems WHERE VoteId = @VoteId");
            this.database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, voteId);
            return (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) >= 0);
        }

        public int GetVoteAttends(long voteId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Count(*) as Num FROM Hishop_VoteRecord WHERE VoteId = @VoteId");
            this.database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, voteId);
            return (int) this.database.ExecuteScalar(sqlStringCommand);
        }

        public VoteInfo GetVoteById(long voteId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Votes WHERE VoteId = @VoteId");
            this.database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, voteId);
            VoteInfo info = null;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = this.PopulateVote(reader);
                    info.VoteCounts = this.GetVoteCounts(voteId);
                    info.VoteAttends = this.GetVoteAttends(voteId);
                }
            }
            if (info != null)
            {
                string query = "select * from Hishop_VoteItems where VoteId=@VoteId";
                sqlStringCommand = this.database.GetSqlStringCommand(query);
                this.database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, voteId);
                using (IDataReader reader2 = this.database.ExecuteReader(sqlStringCommand))
                {
                    IList<VoteItemInfo> list = ReaderConvert.ReaderToList<VoteItemInfo>(reader2);
                    int voteCounts = info.VoteCounts;
                    if (voteCounts > 0)
                    {
                        foreach (VoteItemInfo info2 in list)
                        {
                            info2.Percentage = (info2.ItemCount * 100) / voteCounts;
                        }
                    }
                    info.VoteItems = list;
                }
            }
            return info;
        }

        public int GetVoteCounts(long voteId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ISNULL(SUM(ItemCount),0) FROM Hishop_VoteItems WHERE VoteId = @VoteId");
            this.database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, voteId);
            return (int) this.database.ExecuteScalar(sqlStringCommand);
        }

        public IList<VoteItemInfo> GetVoteItems(long voteId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_VoteItems WHERE VoteId = @VoteId");
            this.database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, voteId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<VoteItemInfo>(reader);
            }
        }

        public IList<VoteInfo> GetVoteList()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Votes");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<VoteInfo>(reader);
            }
        }

        public bool IsVote(int voteId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM Hishop_VoteRecord WHERE VoteId = @VoteId AND UserId = @UserId");
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int64, Globals.GetCurrentMemberUserId(false));
            this.database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, voteId);
            return (((int) this.database.ExecuteScalar(sqlStringCommand)) > 0);
        }

        public DataTable LoadVote(int voteId, out string voteName, out int checkNum, out int voteNum)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT VoteName, MaxCheck, (SELECT SUM(ItemCount) FROM Hishop_VoteItems WHERE VoteId = @VoteId) AS VoteNum FROM Hishop_Votes WHERE VoteId = @VoteId; SELECT * FROM Hishop_VoteItems WHERE VoteId = @VoteId");
            this.database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, voteId);
            voteName = string.Empty;
            checkNum = 1;
            voteNum = 0;
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    voteName = (string) reader["VoteName"];
                    checkNum = (int) reader["MaxCheck"];
                    voteNum = (int) reader["VoteNum"];
                }
                reader.NextResult();
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public VoteInfo PopulateVote(IDataRecord reader)
        {
            VoteInfo info = new VoteInfo();
            Type type = info.GetType();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                string name = reader.GetName(i);
                PropertyInfo property = type.GetProperty(name);
                if (property != null)
                {
                    property.SetValue(info, reader[i], null);
                }
            }
            return info;
        }

        public DbQueryResult Query(VoteSearch query)
        {
            StringBuilder builder = new StringBuilder("1=1 ");
            if (query.status != VoteStatus.All)
            {
                if (query.status == VoteStatus.In)
                {
                    builder.AppendFormat("and [StartDate] <= '{0}' and  [EndDate] >= '{0}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else if (query.status == VoteStatus.End)
                {
                    builder.AppendFormat("and [EndDate] < '{0}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else if (query.status == VoteStatus.unBegin)
                {
                    builder.AppendFormat("and [StartDate] > '{0}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                }
            }
            if (!string.IsNullOrEmpty(query.Name))
            {
                builder.AppendFormat("and VoteName like '%{0}%'  ", query.Name);
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_Votes", "VoteId", builder.ToString(), "*");
        }

        public bool UpdateVote(VoteInfo vote)
        {
            if (vote.StartDate > vote.EndDate)
            {
                DateTime startDate = vote.StartDate;
                vote.StartDate = vote.EndDate;
                vote.EndDate = startDate;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE [Hishop_Votes] SET [VoteName] = @VoteName,[MaxCheck] = @MaxCheck,[ImageUrl] = @ImageUrl ,[StartDate] = @StartDate,[EndDate] = @EndDate,[Description] = @Description,[MemberGrades] = @MemberGrades,[DefualtGroup] = @DefualtGroup,[CustomGroup] = @CustomGroup,[IsMultiCheck] = @IsMultiCheck WHERE VoteId = @VoteId ;");
            this.database.AddInParameter(sqlStringCommand, "VoteName", DbType.String, vote.VoteName);
            this.database.AddInParameter(sqlStringCommand, "MaxCheck", DbType.Int32, vote.MaxCheck);
            this.database.AddInParameter(sqlStringCommand, "ImageUrl", DbType.String, vote.ImageUrl);
            this.database.AddInParameter(sqlStringCommand, "StartDate", DbType.DateTime, vote.StartDate);
            this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, vote.EndDate);
            this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, vote.Description);
            this.database.AddInParameter(sqlStringCommand, "MemberGrades", DbType.String, vote.MemberGrades);
            this.database.AddInParameter(sqlStringCommand, "DefualtGroup", DbType.String, vote.DefualtGroup);
            this.database.AddInParameter(sqlStringCommand, "CustomGroup", DbType.String, vote.CustomGroup);
            this.database.AddInParameter(sqlStringCommand, "IsMultiCheck", DbType.Boolean, vote.IsMultiCheck);
            this.database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, vote.VoteId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateVote(VoteInfo vote, DbTransaction trans)
        {
            if (vote.StartDate > vote.EndDate)
            {
                DateTime startDate = vote.StartDate;
                vote.StartDate = vote.EndDate;
                vote.EndDate = startDate;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE [Hishop_Votes] SET [VoteName] = @VoteName,[MaxCheck] = @MaxCheck,[ImageUrl] = @ImageUrl ,[StartDate] = @StartDate,[EndDate] = @EndDate,[Description] = @Description,[MemberGrades] = @MemberGrades,[DefualtGroup] = @DefualtGroup,[CustomGroup] = @CustomGroup,[IsMultiCheck] = @IsMultiCheck WHERE VoteId = @VoteId ;");
            this.database.AddInParameter(sqlStringCommand, "VoteName", DbType.String, vote.VoteName);
            this.database.AddInParameter(sqlStringCommand, "MaxCheck", DbType.Int32, vote.MaxCheck);
            this.database.AddInParameter(sqlStringCommand, "ImageUrl", DbType.String, vote.ImageUrl);
            this.database.AddInParameter(sqlStringCommand, "StartDate", DbType.DateTime, vote.StartDate);
            this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, vote.EndDate);
            this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, vote.Description);
            this.database.AddInParameter(sqlStringCommand, "MemberGrades", DbType.String, vote.MemberGrades);
            this.database.AddInParameter(sqlStringCommand, "DefualtGroup", DbType.String, vote.DefualtGroup);
            this.database.AddInParameter(sqlStringCommand, "CustomGroup", DbType.String, vote.CustomGroup);
            this.database.AddInParameter(sqlStringCommand, "IsMultiCheck", DbType.Boolean, vote.IsMultiCheck);
            this.database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, vote.VoteId);
            return (this.database.ExecuteNonQuery(sqlStringCommand, trans) > 0);
        }

        public bool UpdateVoteAll(VoteInfo vote)
        {
            try
            {
                VoteInfo voteById = this.GetVoteById(vote.VoteId);
                if ((voteById != null) && (vote.VoteItems.Count > 0))
                {
                    using (IEnumerator<VoteItemInfo> enumerator = vote.VoteItems.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            Func<VoteItemInfo, bool> predicate = null;
                            VoteItemInfo item = enumerator.Current;
                            if (voteById.VoteItems.Count == 0)
                            {
                                this.CreateVoteItem(item);
                            }
                            else
                            {
                                if (predicate == null)
                                {
                                    predicate = x => x.VoteItemName.Equals(item.VoteItemName);
                                }
                                if (voteById.VoteItems.Where<VoteItemInfo>(predicate).ToList<VoteItemInfo>().Count == 0)
                                {
                                    this.CreateVoteItem(item);
                                }
                            }
                        }
                    }
                    List<string> nowId = (from q in vote.VoteItems select q.VoteItemName).ToList<string>();
                    IEnumerable<VoteItemInfo> source = from x in voteById.VoteItems
                        where !nowId.Contains(x.VoteItemName)
                        select x;
                    if (source.Count<VoteItemInfo>() > 0)
                    {
                        foreach (VoteItemInfo info2 in source)
                        {
                            this.DeleteItem(info2.VoteItemId);
                        }
                    }
                }
                else
                {
                    return false;
                }
                return this.UpdateVote(vote);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateVoteItem(VoteItemInfo item)
        {
            try
            {
                string query = "UPDATE [Hishop_VoteItems] SET [VoteItemName] = @VoteItemName where VoteItemId=@VoteItemId";
                DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
                this.database.AddInParameter(sqlStringCommand, "VoteItemName", DbType.String, item.VoteItemName);
                this.database.AddInParameter(sqlStringCommand, "VoteItemId", DbType.Int64, item.VoteItemId);
                return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Vote(int voteId, string itemIds)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("IF EXISTS (SELECT 1 FROM Hishop_Votes WHERE VoteId=@VoteId AND (GETDATE() < StartDate OR GETDATE() > EndDate) ) return;INSERT INTO Hishop_VoteRecord (UserId, VoteId) VALUES (@UserId, @VoteId);" + string.Format(" UPDATE Hishop_VoteItems SET ItemCount = ItemCount + 1 WHERE VoteId = @VoteId AND VoteItemId IN ({0})", itemIds));
            this.database.AddInParameter(sqlStringCommand, "UserId", DbType.Int64, Globals.GetCurrentMemberUserId(false));
            this.database.AddInParameter(sqlStringCommand, "VoteId", DbType.Int64, voteId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

