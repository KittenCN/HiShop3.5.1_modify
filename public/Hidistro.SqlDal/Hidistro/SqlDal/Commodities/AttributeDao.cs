namespace Hidistro.SqlDal.Commodities
{
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Commodities;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;

    public class AttributeDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public bool AddAttribute(AttributeInfo attribute)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Hishop_Attributes; INSERT INTO Hishop_Attributes(AttributeName, DisplaySequence, TypeId, UsageMode, UseAttributeImage) VALUES(@AttributeName, @DisplaySequence, @TypeId, @UsageMode, @UseAttributeImage); SELECT @@IDENTITY");
            this.database.AddInParameter(sqlStringCommand, "AttributeName", DbType.String, attribute.AttributeName);
            this.database.AddInParameter(sqlStringCommand, "TypeId", DbType.Int32, attribute.TypeId);
            this.database.AddInParameter(sqlStringCommand, "UsageMode", DbType.Int32, (int) attribute.UsageMode);
            this.database.AddInParameter(sqlStringCommand, "UseAttributeImage", DbType.Boolean, attribute.UseAttributeImage);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if ((attribute.AttributeValues.Count != 0) && (obj2 != null))
            {
                int num = Convert.ToInt32(obj2);
                foreach (AttributeValueInfo info in attribute.AttributeValues)
                {
                    DbCommand command = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Hishop_AttributeValues; INSERT INTO Hishop_AttributeValues(AttributeId, DisplaySequence, ValueStr, ImageUrl) VALUES(@AttributeId, @DisplaySequence, @ValueStr, @ImageUrl)");
                    this.database.AddInParameter(command, "AttributeId", DbType.Int32, num);
                    this.database.AddInParameter(command, "ValueStr", DbType.String, info.ValueStr);
                    this.database.AddInParameter(command, "ImageUrl", DbType.String, info.ImageUrl);
                    this.database.ExecuteNonQuery(command);
                }
            }
            return (obj2 != null);
        }

        public int AddAttributeName(AttributeInfo attribute)
        {
            int num = 0;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Hishop_Attributes; INSERT INTO Hishop_Attributes(AttributeName, DisplaySequence, TypeId, UsageMode, UseAttributeImage) VALUES(@AttributeName, @DisplaySequence, @TypeId, @UsageMode, @UseAttributeImage); SELECT @@IDENTITY");
            this.database.AddInParameter(sqlStringCommand, "AttributeName", DbType.String, attribute.AttributeName);
            this.database.AddInParameter(sqlStringCommand, "TypeId", DbType.Int32, attribute.TypeId);
            this.database.AddInParameter(sqlStringCommand, "UsageMode", DbType.Int32, (int) attribute.UsageMode);
            this.database.AddInParameter(sqlStringCommand, "UseAttributeImage", DbType.Boolean, attribute.UseAttributeImage);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if (obj2 != null)
            {
                num = Convert.ToInt32(obj2);
            }
            return num;
        }

        public bool DeleteAttribute(int attributeId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_Attributes WHERE AttributeId = @AttributeId AND not exists (SELECT * FROM Hishop_SKUItems WHERE AttributeId = @AttributeId)");
            this.database.AddInParameter(sqlStringCommand, "AttributeId", DbType.Int32, attributeId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public AttributeInfo GetAttribute(int attributeId)
        {
            AttributeInfo info = new AttributeInfo();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_AttributeValues WHERE AttributeId = @AttributeId ORDER BY DisplaySequence DESC; SELECT * FROM Hishop_Attributes WHERE AttributeId = @AttributeId;");
            this.database.AddInParameter(sqlStringCommand, "AttributeId", DbType.Int32, attributeId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                IList<AttributeValueInfo> list = ReaderConvert.ReaderToList<AttributeValueInfo>(reader);
                reader.NextResult();
                info = ReaderConvert.ReaderToModel<AttributeInfo>(reader);
                info.AttributeValues = list;
            }
            return info;
        }

        public IList<AttributeInfo> GetAttributes(int typeId)
        {
            IList<AttributeInfo> list = new List<AttributeInfo>();
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Attributes WHERE TypeId = @TypeId ORDER BY DisplaySequence DESC SELECT * FROM Hishop_AttributeValues WHERE AttributeId IN (SELECT AttributeId FROM Hishop_Attributes WHERE TypeId = @TypeId) ORDER BY DisplaySequence DESC");
            this.database.AddInParameter(sqlStringCommand, "TypeId", DbType.Int32, typeId);
            using (DataSet set = this.database.ExecuteDataSet(sqlStringCommand))
            {
                foreach (DataRow row in set.Tables[0].Rows)
                {
                    AttributeInfo item = new AttributeInfo {
                        AttributeId = (int) row["AttributeId"],
                        AttributeName = (string) row["AttributeName"],
                        DisplaySequence = (int) row["DisplaySequence"],
                        TypeId = (int) row["TypeId"],
                        UsageMode = (AttributeUseageMode) ((int) row["UsageMode"]),
                        UseAttributeImage = (bool) row["UseAttributeImage"]
                    };
                    if (set.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow row2 in set.Tables[1].Select("AttributeId=" + item.AttributeId.ToString()))
                        {
                            AttributeValueInfo info2 = new AttributeValueInfo {
                                ValueId = (int) row2["ValueId"],
                                AttributeId = item.AttributeId,
                                ValueStr = (string) row2["ValueStr"]
                            };
                            item.AttributeValues.Add(info2);
                        }
                    }
                    list.Add(item);
                }
            }
            return list;
        }

        public IList<AttributeInfo> GetAttributes(int typeId, AttributeUseageMode attributeUseageMode)
        {
            string str;
            IList<AttributeInfo> list = new List<AttributeInfo>();
            if (attributeUseageMode == AttributeUseageMode.Choose)
            {
                str = "UsageMode = 2";
            }
            else
            {
                str = "UsageMode <> 2";
            }
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Attributes WHERE TypeId = @TypeId AND " + str + " ORDER BY DisplaySequence Desc SELECT * FROM Hishop_AttributeValues WHERE AttributeId IN (SELECT AttributeId FROM Hishop_Attributes WHERE TypeId = @TypeId AND  " + str + " ) ORDER BY DisplaySequence Desc");
            this.database.AddInParameter(sqlStringCommand, "TypeId", DbType.Int32, typeId);
            using (DataSet set = this.database.ExecuteDataSet(sqlStringCommand))
            {
                foreach (DataRow row in set.Tables[0].Rows)
                {
                    AttributeInfo item = new AttributeInfo {
                        AttributeId = (int) row["AttributeId"],
                        AttributeName = (string) row["AttributeName"],
                        DisplaySequence = (int) row["DisplaySequence"],
                        TypeId = (int) row["TypeId"],
                        UsageMode = (AttributeUseageMode) ((int) row["UsageMode"]),
                        UseAttributeImage = (bool) row["UseAttributeImage"]
                    };
                    if (set.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow row2 in set.Tables[1].Select("AttributeId=" + item.AttributeId.ToString()))
                        {
                            AttributeValueInfo info2 = new AttributeValueInfo {
                                ValueId = (int) row2["ValueId"],
                                AttributeId = item.AttributeId
                            };
                            if (row2["ImageUrl"] != DBNull.Value)
                            {
                                info2.ImageUrl = (string) row2["ImageUrl"];
                            }
                            info2.ValueStr = (string) row2["ValueStr"];
                            item.AttributeValues.Add(info2);
                        }
                    }
                    list.Add(item);
                }
            }
            return list;
        }

        public int GetSpecificationId(int typeId, string specificationName)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT AttributeId FROM Hishop_Attributes WHERE UsageMode = 2 AND TypeId = @TypeId AND AttributeName = @AttributeName");
            this.database.AddInParameter(sqlStringCommand, "TypeId", DbType.Int32, typeId);
            this.database.AddInParameter(sqlStringCommand, "AttributeName", DbType.String, specificationName);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            int num = 0;
            if (obj2 != null)
            {
                num = (int) obj2;
            }
            return num;
        }

        public void SwapAttributeSequence(int attributeId, int replaceAttributeId, int displaySequence, int replaceDisplaySequence)
        {
            DataHelper.SwapSequence("Hishop_Attributes", "AttributeId", "DisplaySequence", attributeId, replaceAttributeId, displaySequence, replaceDisplaySequence);
        }

        public bool UpdateAttribute(AttributeInfo attribute)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Attributes SET AttributeName = @AttributeName, TypeId = @TypeId, UseAttributeImage = @UseAttributeImage WHERE AttributeId = @AttributeId; DELETE FROM Hishop_AttributeValues WHERE AttributeId = @AttributeId;");
            this.database.AddInParameter(sqlStringCommand, "AttributeId", DbType.Int32, attribute.AttributeId);
            this.database.AddInParameter(sqlStringCommand, "AttributeName", DbType.String, attribute.AttributeName);
            this.database.AddInParameter(sqlStringCommand, "TypeId", DbType.Int32, attribute.TypeId);
            this.database.AddInParameter(sqlStringCommand, "UseAttributeImage", DbType.Boolean, attribute.UseAttributeImage);
            bool flag = this.database.ExecuteNonQuery(sqlStringCommand) > 0;
            if (flag && (attribute.AttributeValues.Count != 0))
            {
                foreach (AttributeValueInfo info in attribute.AttributeValues)
                {
                    DbCommand command = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Hishop_AttributeValues; INSERT INTO Hishop_AttributeValues(AttributeId, DisplaySequence, ValueStr, ImageUrl) VALUES(@AttributeId, @DisplaySequence, @ValueStr, @ImageUrl)");
                    this.database.AddInParameter(command, "AttributeId", DbType.Int32, attribute.AttributeId);
                    this.database.AddInParameter(command, "ValueStr", DbType.String, info.ValueStr);
                    this.database.AddInParameter(command, "ImageUrl", DbType.String, info.ImageUrl);
                    this.database.ExecuteNonQuery(command);
                }
            }
            return flag;
        }

        public bool UpdateAttributeName(AttributeInfo attribute)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Attributes SET AttributeName = @AttributeName, UsageMode = @UsageMode WHERE AttributeId = @AttributeId;");
            this.database.AddInParameter(sqlStringCommand, "AttributeId", DbType.Int32, attribute.AttributeId);
            this.database.AddInParameter(sqlStringCommand, "AttributeName", DbType.String, attribute.AttributeName);
            this.database.AddInParameter(sqlStringCommand, "UsageMode", DbType.Int32, (int) attribute.UsageMode);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

