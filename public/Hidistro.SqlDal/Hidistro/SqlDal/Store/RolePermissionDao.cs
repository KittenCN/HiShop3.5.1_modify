namespace Hidistro.SqlDal.Store
{
    using Hidistro.Entities.Store;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;
    using System.Text;

    public class RolePermissionDao
    {
        private Database db = DatabaseFactory.CreateDatabase();

        public bool AddRolePermission(IList<RolePermissionInfo> models, int roleId)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("Delete From aspnet_RolePermissions where RoleId={0};", roleId);
            if (models.Count > 0)
            {
                int num = models.Count<RolePermissionInfo>();
                for (int i = 0; i < num; i++)
                {
                    builder.Append("Insert into aspnet_RolePermissions([PermissionId],[RoleId]) values ");
                    builder.AppendFormat("('{0}',{1})", models[i].PermissionId, models[i].RoleId);
                    builder.Append(";");
                }
            }
            DbCommand sqlStringCommand = this.db.GetSqlStringCommand(builder.ToString());
            return (this.db.ExecuteNonQuery(sqlStringCommand) > 0);
        }

        public List<RolePermissionInfo> GetPermissionsByRoleId(int roleId)
        {
            List<RolePermissionInfo> list = new List<RolePermissionInfo>();
            string query = "SELECT [PermissionId],[RoleId] FROM [aspnet_RolePermissions] where RoleId=@RoleId";
            DbCommand sqlStringCommand = this.db.GetSqlStringCommand(query);
            this.db.AddInParameter(sqlStringCommand, "@RoleId", DbType.Int32, roleId);
            using (IDataReader reader = this.db.ExecuteReader(sqlStringCommand))
            {
                while (reader.Read())
                {
                    RolePermissionInfo item = new RolePermissionInfo {
                        PermissionId = reader["PermissionId"].ToString(),
                        RoleId = roleId
                    };
                    list.Add(item);
                }
            }
            return list;
        }
    }
}

