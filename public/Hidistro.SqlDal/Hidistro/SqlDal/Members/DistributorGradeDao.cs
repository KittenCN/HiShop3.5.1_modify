namespace Hidistro.SqlDal.Members
{
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities;
    using Hidistro.Entities.Members;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Text;

    public class DistributorGradeDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool ClearAddCommission()
        {
            string query = "UPDATE aspnet_DistributorGrade set AddCommission=0";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool CreateDistributorGrade(DistributorGradeInfo distributorgrade)
        {
            StringBuilder builder = new StringBuilder();
            if (distributorgrade.IsDefault)
            {
                builder.AppendLine("UPDATE aspnet_DistributorGrade set IsDefault=0 where IsDefault=1;");
            }
            builder.AppendLine("INSERT INTO aspnet_DistributorGrade").AppendLine("(Name,Description,CommissionsLimit,FirstCommissionRise,SecondCommissionRise,ThirdCommissionRise,IsDefault,Ico)").AppendLine("VALUES(@Name,@Description,@CommissionsLimit,@FirstCommissionRise,@SecondCommissionRise,@ThirdCommissionRise,@IsDefault,@Ico);select @@identity");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, distributorgrade.Name);
            this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, distributorgrade.Description);
            this.database.AddInParameter(sqlStringCommand, "CommissionsLimit", DbType.Decimal, distributorgrade.CommissionsLimit);
            this.database.AddInParameter(sqlStringCommand, "FirstCommissionRise", DbType.Decimal, distributorgrade.FirstCommissionRise);
            this.database.AddInParameter(sqlStringCommand, "SecondCommissionRise", DbType.Decimal, distributorgrade.SecondCommissionRise);
            this.database.AddInParameter(sqlStringCommand, "ThirdCommissionRise", DbType.Decimal, distributorgrade.ThirdCommissionRise);
            this.database.AddInParameter(sqlStringCommand, "IsDefault", DbType.Boolean, distributorgrade.IsDefault);
            this.database.AddInParameter(sqlStringCommand, "Ico", DbType.String, distributorgrade.Ico);
            int gradeid = int.Parse(this.database.ExecuteScalar(sqlStringCommand).ToString());
            bool flag = gradeid > 0;
            if (flag && distributorgrade.IsDefault)
            {
                this.SetGradeDefault(gradeid);
            }
            return flag;
        }

        public string DelOneGrade(int gradeid)
        {
            if (this.HasDistributor(gradeid))
            {
                return "-1";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("delete  aspnet_DistributorGrade  where GradeId=@GradeId and IsDefault=0");
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, gradeid);
            return ((this.database.ExecuteNonQuery(sqlStringCommand) > 0) ? "1" : "0");
        }

        public DataTable GetAllDistributorGrade()
        {
            string query = "select * from aspnet_DistributorGrade order by CommissionsLimit asc";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
        }

        public DbQueryResult GetDistributorGrade(DistributorGradeQuery query)
        {
            StringBuilder builder = new StringBuilder();
            if (query.GradeId > 0)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" GradeId = {0}", query.GradeId);
            }
            if (!string.IsNullOrEmpty(query.Name))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" Name LIKE '%{0}%'", DataHelper.CleanSearchString(query.Name));
            }
            if (!string.IsNullOrEmpty(query.Description))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" Description LIKE '%{0}%'", DataHelper.CleanSearchString(query.Description));
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "aspnet_DistributorGrade", "GradeID", (builder.Length > 0) ? builder.ToString() : null, "*");
        }

        public DistributorGradeInfo GetDistributorGradeInfo(int gradeid)
        {
            if (gradeid <= 0)
            {
                return null;
            }
            DistributorGradeInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT * FROM aspnet_DistributorGrade where gradeid={0}", gradeid));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateDistributorGradeInfo(reader);
                }
            }
            return info;
        }

        public IList<DistributorGradeInfo> GetDistributorGradeInfos()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_DistributorGrade");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToList<DistributorGradeInfo>(reader);
            }
        }

        public DbQueryResult GetDistributorGradeRequest(DistributorGradeQuery query)
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(query.Name))
            {
                if (builder.Length > 0)
                {
                    builder.Append(" AND ");
                }
                builder.AppendFormat(" Name LIKE '%{0}%'", DataHelper.CleanSearchString(query.Name));
            }
            return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "aspnet_DistributorGrade ", "GradeID", (builder.Length > 0) ? builder.ToString() : null, "*");
        }

        public Dictionary<int, int> GetGradeCount(string ReferralStatus)
        {
            DataTable table;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT DistributorGradeId g, COUNT(UserId) as c FROM aspnet_Distributors where  ReferralStatus=@ReferralStatus group by DistributorGradeId");
            this.database.AddInParameter(sqlStringCommand, "ReferralStatus", DbType.String, ReferralStatus);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                table = DataHelper.ConverDataReaderToDataTable(reader);
            }
            Dictionary<int, int> dictionary = new Dictionary<int, int>();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                dictionary.Add((int) table.Rows[i]["g"], (int) table.Rows[i]["c"]);
            }
            return dictionary;
        }

        public DistributorGradeInfo GetIsDefaultDistributorGradeInfo()
        {
            DistributorGradeInfo info = null;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT * FROM aspnet_DistributorGrade where IsDefault=1 order by CommissionsLimit asc", new object[0]));
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                if (reader.Read())
                {
                    info = DataMapper.PopulateDistributorGradeInfo(reader);
                }
            }
            return info;
        }

        public bool HasDistributor(int greadeid)
        {
            string query = "select * from aspnet_Distributors where DistributorGradeId=@DistributorGradeId";
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "DistributorGradeId", DbType.Int32, greadeid);
            return (this.database.ExecuteDataSet(sqlStringCommand).Tables[0].Rows.Count > 0);
        }

        public bool IsExistsMinAmount(int gradeid, decimal minorderamount)
        {
            bool flag = false;
            string query = "select top 1 GradeId from aspnet_DistributorGrade where CommissionsLimit=" + minorderamount;
            if (gradeid > 0)
            {
                query = query + " and GradeId<>" + gradeid;
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            IDataReader reader = this.database.ExecuteReader(sqlStringCommand);
            if (reader.Read())
            {
                flag = true;
            }
            reader.Close();
            return flag;
        }

        public bool SetAddCommission(int gradeid, decimal addcommission)
        {
            string query = "UPDATE aspnet_DistributorGrade set AddCommission=@AddCommission where GradeId=" + gradeid;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
            this.database.AddInParameter(sqlStringCommand, "AddCommission", DbType.Decimal, addcommission);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool SetGradeDefault(int gradeid)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_DistributorGrade set IsDefault=0 where GradeId<>@GradeId;UPDATE aspnet_DistributorGrade set IsDefault=1 where GradeId=@GradeId");
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, gradeid);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateDistributor(DistributorGradeInfo distributorgrade)
        {
            StringBuilder builder = new StringBuilder();
            if (distributorgrade.IsDefault)
            {
                builder.AppendLine("UPDATE aspnet_DistributorGrade set IsDefault=0 where IsDefault=1;");
            }
            builder.AppendLine("UPDATE aspnet_DistributorGrade SET Name=@Name,Description=@Description,CommissionsLimit=@CommissionsLimit,");
            builder.AppendLine("FirstCommissionRise=@FirstCommissionRise,SecondCommissionRise=@SecondCommissionRise,ThirdCommissionRise=@ThirdCommissionRise,Ico=@Ico,IsDefault=@IsDefault WHERE GradeId=@GradeId");
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(builder.ToString());
            this.database.AddInParameter(sqlStringCommand, "GradeId", DbType.Int32, distributorgrade.GradeId);
            this.database.AddInParameter(sqlStringCommand, "Name", DbType.String, distributorgrade.Name);
            this.database.AddInParameter(sqlStringCommand, "Description", DbType.String, distributorgrade.Description);
            this.database.AddInParameter(sqlStringCommand, "CommissionsLimit", DbType.Decimal, distributorgrade.CommissionsLimit);
            this.database.AddInParameter(sqlStringCommand, "FirstCommissionRise", DbType.Decimal, distributorgrade.FirstCommissionRise);
            this.database.AddInParameter(sqlStringCommand, "SecondCommissionRise", DbType.Decimal, distributorgrade.SecondCommissionRise);
            this.database.AddInParameter(sqlStringCommand, "ThirdCommissionRise", DbType.Decimal, distributorgrade.ThirdCommissionRise);
            this.database.AddInParameter(sqlStringCommand, "IsDefault", DbType.Boolean, distributorgrade.IsDefault);
            this.database.AddInParameter(sqlStringCommand, "Ico", DbType.String, distributorgrade.Ico);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

