namespace Hidistro.UI.Web.Admin.Settings
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.Store;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Web;

    public class SaveRolePermissionData : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            if (Globals.GetCurrentManagerUserId() <= 0)
            {
                context.Response.Write("{\"type\":\"error\",\"data\":\"请先登录\"}");
                context.Response.End();
            }
            this.SaveRolePermission(context);
        }

        private void SaveRolePermission(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            StringBuilder builder = new StringBuilder("{");
            int roleId = 0;
            try
            {
                roleId = int.Parse(context.Request.Form["roleId"]);
            }
            catch (Exception)
            {
                builder.Append("\"status\":\"0\",\"Desciption\":\"参与错误!\"}");
                context.Response.Write(builder.ToString());
                return;
            }
            string str = context.Request.Form["rolePermissions"];
            List<RolePermissionInfo> models = new List<RolePermissionInfo>();
            if (!string.IsNullOrEmpty(str))
            {
                foreach (string str2 in str.Split(new char[] { ',' }))
                {
                    RolePermissionInfo item = new RolePermissionInfo {
                        RoleId = roleId,
                        PermissionId = str2
                    };
                    models.Add(item);
                }
            }
            if (ManagerHelper.AddRolePermission(models, roleId))
            {
                builder.Append("\"status\":\"1\",\"Desciption\":\"操作成功!\"}");
                context.Response.Write(builder.ToString());
            }
            else
            {
                builder.Append("\"status\":\"1\",\"Desciption\":\"操作成功,该部门未设置任何权限!\"}");
                context.Response.Write(builder.ToString());
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}

