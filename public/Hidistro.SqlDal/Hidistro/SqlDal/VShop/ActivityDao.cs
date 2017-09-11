namespace Hidistro.SqlDal.VShop
{
    using Hidistro.Entities;
    using Hidistro.Entities.Promotions;
    using Hidistro.Entities.VShop;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class ActivityDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public Hidistro.Entities.VShop.ActivityInfo GetActivity(int activityId)
        {
            string query = "SELECT * FROM vshop_Activity WHERE ActivityId=@ActivityId";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "ActivityId", DbType.Int32, activityId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<Hidistro.Entities.VShop.ActivityInfo>(reader);
            }
        }

        public ActivityDetailInfo GetActivityDetailInfo(int Id)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Activities_Detail WHERE ID = @ID");
            this.database.AddInParameter(sqlStringCommand, "ID", DbType.Int32, Id);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<ActivityDetailInfo>(reader);
            }
        }

        public IList<Hidistro.Entities.VShop.ActivityInfo> GetAllActivity()
        {
            string query = "SELECT *, (SELECT Count(ActivityId) FROM vshop_ActivitySignUp WHERE ActivityId = a.ActivityId) AS CurrentValue FROM vshop_Activity a ORDER BY ActivityId DESC";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<Hidistro.Entities.VShop.ActivityInfo>(reader);
            }
        }

        public int SaveActivity(Hidistro.Entities.VShop.ActivityInfo activity)
        {
            int num;
            StringBuilder builder = new StringBuilder();
            builder.Append("INSERT INTO vshop_Activity(").Append("Name,Description,StartDate,EndDate,CloseRemark,Keys").Append(",MaxValue,PicUrl,Item1,Item2,Item3,Item4,Item5)").Append(" VALUES (").Append("@Name,@Description,@StartDate,@EndDate,@CloseRemark,@Keys").Append(",@MaxValue,@PicUrl,@Item1,@Item2,@Item3,@Item4,@Item5)").Append(";select @@IDENTITY");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, activity.Name);
            this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, activity.Description);
            this.database.AddInParameter(sqlStringCommand, "StartDate", DbType.DateTime, activity.StartDate);
            this.database.AddInParameter(sqlStringCommand, "EndDate", DbType.DateTime, activity.EndDate);
            this.database.AddInParameter(sqlStringCommand, "CloseRemark", DbType.String, activity.CloseRemark);
            this.database.AddInParameter(sqlStringCommand, "Keys", DbType.String, activity.Keys);
            this.database.AddInParameter(sqlStringCommand, "MaxValue", DbType.Int32, activity.MaxValue);
            this.database.AddInParameter(sqlStringCommand, "PicUrl", DbType.String, activity.PicUrl);
            this.database.AddInParameter(sqlStringCommand, "Item1", DbType.String, activity.Item1);
            this.database.AddInParameter(sqlStringCommand, "Item2", DbType.String, activity.Item2);
            this.database.AddInParameter(sqlStringCommand, "Item3", DbType.String, activity.Item3);
            this.database.AddInParameter(sqlStringCommand, "Item4", DbType.String, activity.Item4);
            this.database.AddInParameter(sqlStringCommand, "Item5", DbType.String, activity.Item5);
            int.TryParse(this.database.ExecuteScalar(sqlStringCommand).ToString(), out num);
            return num;
        }
    }
}

