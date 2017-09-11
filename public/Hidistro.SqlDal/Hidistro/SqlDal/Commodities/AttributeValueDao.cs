namespace Hidistro.SqlDal.Commodities
{
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Commodities;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Data;
    using System.Data.Common;

    public class AttributeValueDao
    {
        private Database database = DatabaseFactory.CreateDatabase();

        public int AddAttributeValue(AttributeValueInfo attributeValue)
        {
            int num = 0;
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Hishop_AttributeValues; INSERT INTO Hishop_AttributeValues(AttributeId, DisplaySequence, ValueStr, ImageUrl) VALUES(@AttributeId, @DisplaySequence, @ValueStr, @ImageUrl);SELECT @@IDENTITY");
            this.database.AddInParameter(sqlStringCommand, "AttributeId", DbType.Int32, attributeValue.AttributeId);
            this.database.AddInParameter(sqlStringCommand, "ValueStr", DbType.String, attributeValue.ValueStr);
            this.database.AddInParameter(sqlStringCommand, "ImageUrl", DbType.String, attributeValue.ImageUrl);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            if (obj2 != null)
            {
                num = Convert.ToInt32(obj2.ToString());
            }
            return num;
        }

        public bool ClearAttributeValue(int attributeId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_AttributeValues WHERE AttributeId = @AttributeId AND not exists (SELECT * FROM Hishop_SKUItems WHERE AttributeId = @AttributeId)");
            this.database.AddInParameter(sqlStringCommand, "AttributeId", DbType.Int32, attributeId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public bool DeleteAttributeValue(int attributeValueId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_AttributeValues WHERE ValueId = @ValueId AND not exists (SELECT * FROM Hishop_SKUItems WHERE ValueId = @ValueId) DELETE FROM Hishop_ProductAttributes WHERE ValueId = @ValueId");
            this.database.AddInParameter(sqlStringCommand, "ValueId", DbType.Int32, attributeValueId);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public AttributeValueInfo GetAttributeValueInfo(int valueId)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_AttributeValues WHERE ValueId=@ValueId");
            this.database.AddInParameter(sqlStringCommand, "ValueId", DbType.Int32, valueId);
            using (IDataReader reader = this.database.ExecuteReader(sqlStringCommand))
            {
                return ReaderConvert.ReaderToModel<AttributeValueInfo>(reader);
            }
        }

        public int GetSpecificationValueId(int attributeId, string ValueStr)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ValueId FROM Hishop_AttributeValues WHERE AttributeId = @AttributeId AND ValueStr = @ValueStr");
            this.database.AddInParameter(sqlStringCommand, "AttributeId", DbType.Int32, attributeId);
            this.database.AddInParameter(sqlStringCommand, "ValueStr", DbType.String, ValueStr);
            object obj2 = this.database.ExecuteScalar(sqlStringCommand);
            int num = 0;
            if (obj2 != null)
            {
                num = Convert.ToInt32(obj2);
            }
            return num;
        }

        public void SwapAttributeValueSequence(int attributeValueId, int replaceAttributeValueId, int displaySequence, int replaceDisplaySequence)
        {
            DataHelper.SwapSequence("Hishop_AttributeValues", "ValueId", "DisplaySequence", attributeValueId, replaceAttributeValueId, displaySequence, replaceDisplaySequence);
        }

        public bool UpdateAttributeValue(AttributeValueInfo attributeValue)
        {
            DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_AttributeValues SET  ValueStr=@ValueStr, ImageUrl=@ImageUrl WHERE ValueId=@valueId");
            this.database.AddInParameter(sqlStringCommand, "ValueStr", DbType.String, attributeValue.ValueStr);
            this.database.AddInParameter(sqlStringCommand, "ValueId", DbType.Int32, attributeValue.ValueId);
            this.database.AddInParameter(sqlStringCommand, "ImageUrl", DbType.String, attributeValue.ImageUrl);
            return (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
        }
    }
}

