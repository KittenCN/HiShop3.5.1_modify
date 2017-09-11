namespace Hidistro.SqlDal.Commodities
{
    using Hidistro.Core;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class TagDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool AddProductTags(int productId, IList<int> tagIds, DbTransaction tran)
        {
            bool flag = false;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_ProductTag VALUES(@TagId,@ProductId)");
            this.database.AddInParameter(sqlStringCommand, "TagId", DbType.Int32);
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32);
            foreach (int num in tagIds)
            {
                this.database.SetParameterValue(sqlStringCommand, "ProductId", productId);
                this.database.SetParameterValue(sqlStringCommand, "TagId", num);
                if (tran != null)
                {
                    flag = this.database.ExecuteNonQuery(sqlStringCommand, tran) > 0;
                }
                else
                {
                    flag = this.database.ExecuteNonQuery(sqlStringCommand) > 0;
                }
                if (!flag)
                {
                    return flag;
                }
            }
            return flag;
        }

        public int AddTags(string tagname)
        {
            int num = 0;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_Tags VALUES(@TagName);SELECT @@IDENTITY");
            this.database.AddInParameter(sqlStringCommand, "TagName", DbType.String, Globals.SubStr(tagname, 8, ""));
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if (obj2 != null)
            {
                num = Convert.ToInt32(obj2.ToString());
            }
            return num;
        }

        public int AddInsuranceCompany(string insurancecompanyname)
        {
            int num = 0;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_InsuranceCompany VALUES(@InsuranceCompanyName);SELECT @@IDENTITY");
            this.database.AddInParameter(sqlStringCommand, "InsuranceCompanyName", DbType.String, Globals.SubStr(insurancecompanyname, 8, ""));
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if (obj2 != null)
            {
                num = Convert.ToInt32(obj2.ToString());
            }
            return num;
        }


        public int AddInsuranceCompanyArea(string insuranceAreaCiteId, string insuranceAreaCiteName, string insuranceAreaProvinceId, string insuranceAreaName, string insuranceCompanyTypes,string insuranceCompanyTypesIds)
        {
            int num = 0; 
             DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_InsuranceArea VALUES(@insuranceAreaCiteId,@insuranceAreaCiteName,@insuranceAreaProvinceId,@insuranceAreaName,@insuranceCompanyTypes,@insuranceCompanyTypesIds);SELECT @@IDENTITY");
            this.database.AddInParameter(sqlStringCommand, "insuranceAreaCiteId", DbType.String, insuranceAreaName);
            this.database.AddInParameter(sqlStringCommand, "insuranceAreaCiteName", DbType.Int32,int.Parse(insuranceAreaCiteId));
            this.database.AddInParameter(sqlStringCommand, "insuranceAreaProvinceId", DbType.Int32, int.Parse(insuranceAreaProvinceId));
            this.database.AddInParameter(sqlStringCommand, "insuranceAreaName", DbType.String, insuranceAreaCiteName);
            this.database.AddInParameter(sqlStringCommand, "insuranceCompanyTypes", DbType.String, insuranceCompanyTypes);
            this.database.AddInParameter(sqlStringCommand, "insuranceCompanyTypesIds", DbType.String, insuranceCompanyTypesIds);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if (obj2 != null)
            {
                num = Convert.ToInt32(obj2.ToString());
            }
            return num;
        }

        public bool DeleteProductTags(int productId, DbTransaction tran)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ProductTag WHERE ProductId=@ProductId");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            if (tran != null)
            {
                return (this.database.ExecuteNonQuery(sqlStringCommand, tran) >= 0);
            }
            return (this.database.ExecuteNonQuery(sqlStringCommand) >= 0);
        }

        public bool DeleteTags(int tagId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ProductTag WHERE TagID=@TagID;DELETE FROM Hishop_Tags WHERE TagID=@TagID;");
            this.database.AddInParameter(sqlStringCommand, "TagID", DbType.Int32, tagId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool DeleteInsuranceCompany(int insuranceCompanyId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_InsuranceCompany WHERE InsuranceCompanyID=@InsuranceCompanyID;");
            this.database.AddInParameter(sqlStringCommand, "InsuranceCompanyID", DbType.Int32, insuranceCompanyId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool DeleteInsuranceArea(int insuranceAreaId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_InsuranceArea WHERE insuranceAreaId=@insuranceAreaId;");
            this.database.AddInParameter(sqlStringCommand, "insuranceAreaId", DbType.Int32, insuranceAreaId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public string GetProductTagName(int productId)
        {
            string str = string.Empty;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select top 1 b.TagName from Hishop_ProductTag a inner join Hishop_Tags b on a.tagid=b.tagid where ProductId=@ProductId");
            this.database.AddInParameter(sqlStringCommand, "ProductId", DbType.Int32, productId);
            DataTable table = this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
            if (table.Rows.Count > 0)
            {
                str = table.Rows[0]["TagName"].ToString();
            }
            return str;
        }

        public string GetTagName(int tagId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT TagName  FROM  Hishop_Tags WHERE TagID = {0}", tagId));
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if (obj2 != null)
            {
                return obj2.ToString();
            }
            return string.Empty;
        }

        public DataTable GetTags()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT *  FROM  Hishop_Tags");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public DataTable GetInsuranceCompany()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT *  FROM  Hishop_InsuranceCompany");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }


        public DataTable GetInsuranceArea()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT *  FROM  Hishop_InsuranceArea order by InsuranceAreaProvinceId");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public DataTable GetInsuranceAreaGroupProvinceId()
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select InsuranceAreaProvinceId,InsuranceAreaName from Hishop_InsuranceArea where  InsuranceAreaId in(SELECT max(InsuranceAreaId)  FROM  Hishop_InsuranceArea group by InsuranceAreaProvinceId)");
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }

        public DataTable GetInsuranceCompanyByCityId(string city1,string city2)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select InsuranceCompanyID,InsuranceCompanyName from Hishop_InsuranceCompany where(select  InsuranceCompanyTypesIds from Hishop_InsuranceArea where InsuranceAreaProvinceId = @InsuranceAreaProvinceId and InsuranceAreaCiteId = @InsuranceAreaCiteId)  LIKE   '%,' + cast(InsuranceCompanyID as varchar(8)) + ',%'");
            this.database.AddInParameter(sqlStringCommand, "InsuranceAreaProvinceId", DbType.String, city1);
            this.database.AddInParameter(sqlStringCommand, "InsuranceAreaCiteId", DbType.String, city2);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }



        public DataTable GetInsuranceAreaByProvinceId(int InsuranceAreaProvinceId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select InsuranceAreaCiteId,InsuranceAreaCiteName from Hishop_InsuranceArea where  InsuranceAreaProvinceId =@InsuranceAreaProvinceId");
            this.database.AddInParameter(sqlStringCommand, "InsuranceAreaProvinceId", DbType.Int32, InsuranceAreaProvinceId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return DataHelper.ConverDataReaderToDataTable(reader);
            }
        }


        public int GetTags(string tagName)
        {
            int num = 0;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT TagID  FROM  Hishop_Tags WHERE TagName=@TagName");
            this.database.AddInParameter(sqlStringCommand, "TagName", DbType.String, tagName);
            IDataReader reader = this.database.ExecuteReader(sqlStringCommand);
            if (reader.Read())
            {
                num = Convert.ToInt32(reader["TagID"].ToString());
            }
            return num;
        }

        public int GetInsuranceCompany(string insuranceCompanyName)
        {
            int num = 0;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT InsuranceCompanyID  FROM  Hishop_InsuranceCompany WHERE InsuranceCompanyName=@InsuranceCompanyName");
            this.database.AddInParameter(sqlStringCommand, "InsuranceCompanyName", DbType.String, insuranceCompanyName);
            IDataReader reader = this.database.ExecuteReader(sqlStringCommand);
            if (reader.Read())
            {
                num = Convert.ToInt32(reader["InsuranceCompanyID"].ToString());
            }
            return num;
        }


        public int GetInsuranceArea(string InsuranceAreaCiteId)
        {
            int num = 0;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT InsuranceAreaId  FROM  Hishop_InsuranceArea WHERE InsuranceAreaCiteId=@InsuranceAreaCiteId");
            this.database.AddInParameter(sqlStringCommand, "InsuranceAreaCiteId", DbType.String, InsuranceAreaCiteId);
            IDataReader reader = this.database.ExecuteReader(sqlStringCommand);
            if (reader.Read())
            {
                num = Convert.ToInt32(reader["InsuranceAreaId"].ToString());
            }
            return num;
        }


        public int GetInsuranceCompanyArea(string insuranceAreaCiteId)
        {
            int num = 0;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT InsuranceAreaCiteId  FROM  Hishop_InsuranceArea WHERE InsuranceAreaCiteId=@insuranceAreaCiteId");
            this.database.AddInParameter(sqlStringCommand, "InsuranceAreaCiteId", DbType.String, insuranceAreaCiteId);
            IDataReader reader = this.database.ExecuteReader(sqlStringCommand);
            if (reader.Read())
            {
                num = Convert.ToInt32(reader["InsuranceAreaCiteId"].ToString());
            }
            return num;
        }


        public bool UpdateTags(int tagId, string tagname)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Tags SET TagName=@TagName WHERE TagID=@TagID");
            this.database.AddInParameter(sqlStringCommand, "TagName", DbType.String, Globals.SubStr(tagname, 8, ""));
            this.database.AddInParameter(sqlStringCommand, "TagID", DbType.Int32, tagId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }


        public bool UpdateInsuranceCompany(int insuranceCompanyId, string insuranceCompanyname)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_InsuranceCompany SET InsuranceCompanyName=@InsuranceCompanyName WHERE InsuranceCompanyID=@InsuranceCompanyID");
            this.database.AddInParameter(sqlStringCommand, "InsuranceCompanyName", DbType.String, Globals.SubStr(insuranceCompanyname, 8, ""));
            this.database.AddInParameter(sqlStringCommand, "InsuranceCompanyID", DbType.Int32, insuranceCompanyId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool UpdateInsuranceArea(int InsuranceAreaId, string insuranceAreaCiteId, string insuranceAreaCiteName, string insuranceAreaProvinceId, string insuranceAreaName, string insuranceCompanyTypes,string insuranceCompanyTypesIds)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_InsuranceArea SET insuranceAreaCiteId=@insuranceAreaCiteId,insuranceAreaCiteName=@insuranceAreaCiteName,insuranceAreaProvinceId=@insuranceAreaProvinceId,insuranceAreaName=@insuranceAreaName,insuranceCompanyTypes=@insuranceCompanyTypes,insuranceCompanyTypesIds=@insuranceCompanyTypesIds WHERE InsuranceAreaId=@InsuranceAreaId");
            this.database.AddInParameter(sqlStringCommand, "insuranceAreaCiteId", DbType.Int32, insuranceAreaCiteId);
            this.database.AddInParameter(sqlStringCommand, "insuranceAreaCiteName", DbType.String, insuranceAreaCiteName);
            this.database.AddInParameter(sqlStringCommand, "insuranceAreaProvinceId", DbType.Int32, insuranceAreaProvinceId);
            this.database.AddInParameter(sqlStringCommand, "insuranceAreaName", DbType.String, insuranceAreaName);
            this.database.AddInParameter(sqlStringCommand, "insuranceCompanyTypes", DbType.String, insuranceCompanyTypes);
            this.database.AddInParameter(sqlStringCommand, "insuranceCompanyTypesIds", DbType.String, insuranceCompanyTypesIds);
            this.database.AddInParameter(sqlStringCommand, "InsuranceAreaId", DbType.Int32, InsuranceAreaId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

