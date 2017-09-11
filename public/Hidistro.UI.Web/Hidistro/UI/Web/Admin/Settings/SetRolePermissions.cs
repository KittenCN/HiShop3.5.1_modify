namespace Hidistro.UI.Web.Admin.Settings
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Entities.Store;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class SetRolePermissions : AdminPage
    {
        protected Literal ltHtml;
        protected Literal ltRoleName;
        private IList<RolePermissionInfo> oldPermissions;
        private int roleId;
        protected HtmlForm thisForm;

        protected SetRolePermissions() : base("m09", "szp10")
        {
        }

        private void BindDate()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("<input type=\"hidden\" id=\"txtRoleId\" value=\"{0}\" />", this.roleId);
            Dictionary<string, NavModule> moduleList = Navigation.GetNavigation(true).ModuleList;
            bool isCheck = true;
            foreach (KeyValuePair<string, NavModule> pair in moduleList)
            {
                bool isAllCheck = true;
                string nodeKey = string.Empty;
                string str2 = this.GetFirstNoHeaderNode(pair.Value.ItemList, pair.Value.ID, out isAllCheck, out nodeKey);
                bool flag3 = true;
                string str3 = this.GetPageLink(pair.Value.ItemList, pair.Value.ID, nodeKey, out flag3);
                if (!flag3)
                {
                    isCheck = false;
                }
                if (!isAllCheck)
                {
                    isCheck = false;
                }
                builder.Append(" <div class=\"checkboxlist\">");
                builder.Append(" <div class=\"boxrow clearfix\">");
                builder.Append(" <div class=\"classa fl\">");
                builder.Append(" <label class=\"setalign\">");
                builder.AppendFormat(" <input type=\"checkbox\" {1}><strong>{0}</strong>", pair.Value.Title, this.GetInputCheck(isCheck));
                builder.Append(" </label>");
                builder.Append(" </div>");
                builder.Append(str2);
                builder.Append(" </div>");
                builder.Append(str3);
                builder.Append(" </div>");
            }
            this.ltHtml.Text = builder.ToString();
        }

        private void BindRoleInfo()
        {
            RoleInfo role = ManagerHelper.GetRole(this.roleId);
            if (role != null)
            {
                this.ltRoleName.Text = role.RoleName;
            }
        }

        private string GetFirstNoHeaderNode(Dictionary<string, NavItem> navItems, string id, out bool isAllCheck, out string nodeKey)
        {
            nodeKey = string.Empty;
            isAllCheck = true;
            StringBuilder builder = new StringBuilder();
            builder.Append("<div class=\"two fl clearfix\">");
            if (navItems.Values.Count > 0)
            {
                KeyValuePair<string, NavItem> pair = navItems.First<KeyValuePair<string, NavItem>>();
                if (string.IsNullOrEmpty(pair.Value.SpanName))
                {
                    nodeKey = pair.Key;
                    int num = 0;
                    foreach (KeyValuePair<string, NavPageLink> pair2 in pair.Value.PageLinks)
                    {
                        string permissionId = RolePermissionInfo.GetPermissionId(id, pair2.Value.ID);
                        bool isCheck = this.oldPermissions.FirstOrDefault<RolePermissionInfo>(p => (p.PermissionId == permissionId)) != null;
                        if (!isCheck)
                        {
                            isAllCheck = false;
                        }
                        if (num == 0)
                        {
                            builder.Append(" <div class=\"titlecheck fl\">");
                            builder.AppendFormat(" <label class=\"setalign\"> <input type=\"checkbox\" name=\"permissions\" value=\"{0}\" {1}>{2}</label>", permissionId, this.GetInputCheck(isCheck), pair2.Value.Title);
                            builder.Append("</div>");
                        }
                        else
                        {
                            builder.Append("  <div class=\"twoinerlist fl\">");
                            builder.AppendFormat(" <label class=\"setalign\"> <input type=\"checkbox\" name=\"permissions\" value=\"{0}\" {1}>{2}</label>", permissionId, this.GetInputCheck(isCheck), pair2.Value.Title);
                            builder.Append("</div>");
                        }
                        num++;
                    }
                }
                else
                {
                    builder.Append(" <div class=\"titlecheck fl\">");
                    builder.Append("  &nbsp;");
                    builder.Append("</div>");
                }
            }
            builder.Append("</div>");
            return builder.ToString();
        }

        private string GetInputCheck(bool isCheck)
        {
            if (!isCheck)
            {
                return "";
            }
            return "checked=\"checked\"";
        }

        private string GetNodeString(Dictionary<string, NavPageLink> pageLinks, string id, out bool isAllCheck)
        {
            isAllCheck = true;
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, NavPageLink> pair in pageLinks)
            {
                string permissionId = RolePermissionInfo.GetPermissionId(id, pair.Value.ID);
                bool isCheck = this.oldPermissions.FirstOrDefault<RolePermissionInfo>(p => (p.PermissionId == permissionId)) != null;
                if (!isCheck)
                {
                    isAllCheck = false;
                }
                builder.Append(" <label class=\"setalign\">");
                builder.AppendFormat(" <input name=\"permissions\" value=\"{0}\" type=\"checkbox\" {1}>{2}", permissionId, this.GetInputCheck(isCheck), pair.Value.Title);
                builder.Append(" </label>");
            }
            return builder.ToString();
        }

        private string GetPageLink(Dictionary<string, NavItem> navItems, string id, string nodeKey, out bool isAllCheck)
        {
            isAllCheck = true;
            StringBuilder builder = new StringBuilder();
            List<string> list = new List<string>(navItems.Keys);
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] != nodeKey)
                {
                    string str = this.GetNodeString(navItems[list[i]].PageLinks, id, out isAllCheck);
                    if (!string.IsNullOrEmpty(navItems[list[i]].SpanName))
                    {
                        builder.Append(this.TwoHeaderBegin(navItems[list[i]].SpanName, isAllCheck));
                    }
                    builder.Append(str);
                    if (i < (list.Count - 1))
                    {
                        if (!string.IsNullOrEmpty(navItems[list[i + 1]].SpanName))
                        {
                            builder.Append(this.TwoHeaderEnd());
                        }
                    }
                    else
                    {
                        builder.Append(this.TwoHeaderEnd());
                    }
                }
            }
            return builder.ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.roleId = int.Parse(base.Request.QueryString["roleId"]);
                this.oldPermissions = ManagerHelper.GetRolePremissonsByRoleId(this.roleId);
            }
            catch (Exception)
            {
                base.GotoResourceNotFound();
            }
            if (!this.Page.IsPostBack)
            {
                this.BindRoleInfo();
                this.BindDate();
            }
        }

        private string TwoHeaderBegin(string titleName, bool isCheck)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" <div class=\"boxrow clearfix\">");
            builder.Append(" <div class=\"classa fl\">&nbsp;</div>");
            builder.Append("<div class=\"two fl clearfix\">");
            builder.Append("<div class=\"titlecheck fl\">");
            builder.Append(" <label class=\"setalign\">");
            builder.AppendFormat(" <input type=\"checkbox\" {1}><strong>{0}</strong>", titleName, this.GetInputCheck(isCheck));
            builder.Append(" </label>");
            builder.Append(" </div>");
            builder.Append(" <div class=\"twoinerlist fl\">");
            return builder.ToString();
        }

        private string TwoHeaderEnd()
        {
            return " </div> </div> </div>  ";
        }
    }
}

