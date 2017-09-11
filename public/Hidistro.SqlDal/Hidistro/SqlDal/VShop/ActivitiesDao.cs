namespace Hidistro.SqlDal.VShop
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.VShop;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class ActivitiesDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public int AddActivities(ActivitiesInfo activity)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("INSERT INTO Hishop_Activities(").Append("ActivitiesName,ActivitiesType,MeetMoney,ReductionMoney,StartTime,EndTIme,ActivitiesDescription,Type)").Append(" VALUES (").Append("@ActivitiesName,@ActivitiesType,@MeetMoney,@ReductionMoney,@StartTime,@EndTime,@ActivitiesDescription,@Type)");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "ActivitiesName", DbType.String, activity.ActivitiesName);
            this.database.AddInParameter(sqlStringCommand, "ActivitiesType", DbType.Int32, activity.ActivitiesType);
            this.database.AddInParameter(sqlStringCommand, "MeetMoney", DbType.Decimal, activity.MeetMoney);
            this.database.AddInParameter(sqlStringCommand, "ReductionMoney", DbType.Decimal, activity.ReductionMoney);
            this.database.AddInParameter(sqlStringCommand, "StartTime", DbType.DateTime, activity.StartTime);
            this.database.AddInParameter(sqlStringCommand, "EndTIme", DbType.DateTime, activity.EndTIme);
            this.database.AddInParameter(sqlStringCommand, "ActivitiesDescription", DbType.String, activity.ActivitiesDescription);
            this.database.AddInParameter(sqlStringCommand, "Type", DbType.Int32, activity.Type);
            return this.database.ExecuteNonQuery(sqlStringCommand);
        }

        public bool DeleteActivities(int ActivitiesId)
        {
            string query = "DELETE FROM Hishop_Activities WHERE ActivitiesId=@ActivitiesId";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "ActivitiesId", DbType.Int32, ActivitiesId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public IList<ActivitiesInfo> GetActivitiesInfo(string ActivitiesId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT * FROM Hishop_Activities WHERE ActivitiesId={0}", ActivitiesId));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<ActivitiesInfo>(reader);
            }
        }

        public DbQueryResult GetActivitiesList(ActivitiesQuery query)
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(query.ActivitiesName))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" ActivitiesName LIKE '%{0}%'", DataHelper.CleanSearchString(query.ActivitiesName));
            }
            if (!string.IsNullOrEmpty(query.State.ToString()))
            {
                if (query.State == "1")
                {
                    if (builder.Length > 0)
                    {
                        builder.Append(" AND ");
                    }
                    builder.AppendFormat(" datediff(dd,'{0}',StartTime)<=0 and datediff(dd,'{0}',EndTIme)>=0", DateTime.Now.ToShortDateString());
                }
                else if (query.State == "2")
                {
                    if (builder.Length > 0)
                    {
                        builder.Append(" AND ");
                    }
                    builder.AppendFormat(" datediff(dd,'{0}',StartTime)>0 ", DateTime.Now.ToShortDateString());
                }
                else
                {
                    if (builder.Length > 0)
                    {
                        builder.Append(" AND ");
                    }
                    builder.AppendFormat(" datediff(dd,'{0}',EndTIme)<0 ", DateTime.Now.ToShortDateString());
                }
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_Activities ", "ActivitiesId", (builder.Length > 0) ? builder.ToString() : null, "*, (SELECT Name FROM Hishop_Categories WHERE CategoryId = Hishop_Activities.ActivitiesType) AS CategoriesName");
        }

        public DataTable GetType(int Types)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("select ReductionMoney,ActivitiesId,ActivitiesName,MeetMoney,ActivitiesType from Hishop_Activities where datediff(dd,GETDATE(),StartTime)<=0 and datediff(dd,GETDATE(),EndTIme)>=0 and Type=" + Types + " order by MeetMoney asc");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public bool UpdateActivities(ActivitiesInfo activity)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("UPDATE Hishop_Activities SET ").Append("ActivitiesName=@ActivitiesName,").Append("ActivitiesType=@ActivitiesType,").Append("MeetMoney=@MeetMoney,").Append("ReductionMoney=@ReductionMoney,").Append("StartTime=@StartTime,").Append("EndTIme=@EndTIme,").Append("ActivitiesDescription=@ActivitiesDescription").Append(" WHERE ActivitiesId=@ActivitiesId");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "ActivitiesName", DbType.String, activity.ActivitiesName);
            this.database.AddInParameter(sqlStringCommand, "ActivitiesType", DbType.Int32, activity.ActivitiesType);
            this.database.AddInParameter(sqlStringCommand, "MeetMoney", DbType.Decimal, activity.MeetMoney);
            this.database.AddInParameter(sqlStringCommand, "ReductionMoney", DbType.Decimal, activity.ReductionMoney);
            this.database.AddInParameter(sqlStringCommand, "StartTime", DbType.DateTime, activity.StartTime);
            this.database.AddInParameter(sqlStringCommand, "EndTIme", DbType.DateTime, activity.EndTIme);
            this.database.AddInParameter(sqlStringCommand, "ActivitiesDescription", DbType.String, activity.ActivitiesDescription);
            this.database.AddInParameter(sqlStringCommand, "ActivitiesId", DbType.Int32, activity.ActivitiesId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateActivitiesTakeEffect(string activity)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("UPDATE Hishop_Activities SET ").Append("TakeEffect=TakeEffect+1").Append(" WHERE ActivitiesId IN (" + activity + ")");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

