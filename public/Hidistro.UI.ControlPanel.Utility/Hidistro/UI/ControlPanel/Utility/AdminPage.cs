namespace Hidistro.UI.ControlPanel.Utility
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.UI.Common.Controls;
    using Hishop.Components.Validation;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Data;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Web.UI;

    public class AdminPage : Page
    {
        private AdminPage()
        {
        }

        protected AdminPage(string moduleId, string pageId) : this(moduleId, pageId, null)
        {
        }

        protected AdminPage(string moduleId, string pageId, string subPageId)
        {
            this.ModuleId = moduleId;
            this.PageId = pageId;
            this.SubPageId = subPageId;
        }

        private void CheckPageAccess()
        {
            int roleId = 0;
            bool isDefault = false;
            if (Globals.GetCurrentManagerUserId(out roleId, out isDefault) == 0)
            {
                this.Page.Response.Redirect(Globals.ApplicationPath + "/admin/Login.aspx", true);
            }
            if (!isDefault && !ManagerHelper.IsHavePermission(this.ModuleId, this.PageId, roleId))
            {
                this.NotPremissonRedirect(this.ModuleId, roleId, null);
            }
        }

        protected string CutWords(object obj, int length)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            string str = obj.ToString();
            if (str.Length > length)
            {
                return (str.Substring(0, length) + "......");
            }
            return str;
        }

        private string GenericReloadUrl(NameValueCollection queryStrings)
        {
            if ((queryStrings == null) || (queryStrings.Count == 0))
            {
                return base.Request.Url.AbsolutePath;
            }
            StringBuilder builder = new StringBuilder();
            builder.Append(base.Request.Url.AbsolutePath).Append("?");
            foreach (string str2 in queryStrings.Keys)
            {
                string str = queryStrings[str2].Trim().Replace("'", "");
                if (!string.IsNullOrEmpty(str) && (str.Length > 0))
                {
                    builder.Append(str2).Append("=").Append(base.Server.UrlEncode(str)).Append("&");
                }
            }
            queryStrings.Clear();
            builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }

        public decimal GetFieldDecimalValue(DataRow drOne, string FieldName)
        {
            if (((drOne == null) || drOne.Table.Columns.Contains(FieldName)) && (((drOne != null) && drOne.Table.Columns.Contains(FieldName)) && !string.IsNullOrEmpty(drOne[FieldName].ToString())))
            {
                return Convert.ToDecimal(drOne[FieldName].ToString());
            }
            return 0M;
        }

        public int GetFieldIntValue(DataRow drOne, string FieldName)
        {
            if (((drOne == null) || drOne.Table.Columns.Contains(FieldName)) && ((drOne != null) && !string.IsNullOrEmpty(drOne[FieldName].ToString())))
            {
                return int.Parse(drOne[FieldName].ToString());
            }
            return 0;
        }

        public string GetFieldValue(DataRow drOne, string FieldName)
        {
            if (((drOne == null) || drOne.Table.Columns.Contains(FieldName)) && ((drOne != null) && (drOne[FieldName] != null)))
            {
                return drOne[FieldName].ToString();
            }
            return "";
        }

        protected int GetFormIntParam(string name)
        {
            string str = base.Request.Form.Get(name);
            if (string.IsNullOrEmpty(str))
            {
                return 0;
            }
            try
            {
                return Convert.ToInt32(str);
            }
            catch (FormatException)
            {
                return 0;
            }
        }

        protected bool GetUrlBoolParam(string name)
        {
            string str = base.Request.QueryString.Get(name);
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            try
            {
                return Convert.ToBoolean(str);
            }
            catch (FormatException)
            {
                return false;
            }
        }

        protected int GetUrlIntParam(string name)
        {
            string str = base.Request.QueryString.Get(name);
            if (string.IsNullOrEmpty(str))
            {
                return 0;
            }
            try
            {
                return Convert.ToInt32(str);
            }
            catch (FormatException)
            {
                return 0;
            }
        }

        protected string GetUrlParam(string name)
        {
            string str = base.Request.QueryString.Get(name);
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            return str;
        }

        protected void GotoResourceNotFound()
        {
            base.Response.Redirect(Globals.ApplicationPath + "/Admin/NotPermisson.aspx?type=1", true);
        }

        private void NotPremissonRedirect(string moduleId, int roleId, string msg = null)
        {
            Navigation navigation = Navigation.GetNavigation(true);
            string link = string.Empty;
            if (!string.IsNullOrEmpty(base.Request.QueryString["firstPage"]))
            {
                foreach (KeyValuePair<string, NavModule> pair in navigation.ModuleList)
                {
                    if (string.Equals(pair.Value.ID, moduleId, StringComparison.CurrentCultureIgnoreCase))
                    {
                        foreach (KeyValuePair<string, NavItem> pair2 in pair.Value.ItemList)
                        {
                            foreach (KeyValuePair<string, NavPageLink> pair3 in pair2.Value.PageLinks)
                            {
                                if (ManagerHelper.IsHavePermission(pair.Value.ID, pair3.Value.ID, roleId))
                                {
                                    link = pair3.Value.Link;
                                    break;
                                }
                            }
                            if (!string.IsNullOrEmpty(link))
                            {
                                break;
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(link))
                    {
                        break;
                    }
                }
            }
            if (!string.IsNullOrEmpty(link))
            {
                base.Response.Redirect(string.Format("{0}{1}", Globals.ApplicationPath, link), false);
            }
            else
            {
                string str2 = "&m=" + this.ModuleId + "&p=" + this.PageId;
                base.Server.Transfer("/Admin/NoPermissionShow.aspx?type=2" + str2);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (ConfigurationManager.AppSettings["Installer"] != null)
            {
                base.Response.Redirect(Globals.ApplicationPath + "/installer/default.aspx", false);
            }
            else
            {
                this.CheckPageAccess();
                base.OnInit(e);
            }
        }

        protected void ReloadPage(NameValueCollection queryStrings)
        {
            base.Response.Redirect(this.GenericReloadUrl(queryStrings));
        }

        protected void ReloadPage(NameValueCollection queryStrings, bool endResponse)
        {
            base.Response.Redirect(this.GenericReloadUrl(queryStrings), endResponse);
        }

        protected virtual void ShowMsg(ValidationResults validateResults)
        {
            StringBuilder builder = new StringBuilder();
            foreach (ValidationResult result in (IEnumerable<ValidationResult>) validateResults)
            {
                builder.Append(Formatter.FormatErrorMessage(result.Message));
            }
            this.ShowMsg(builder.ToString(), false);
        }

        protected virtual void ShowMsg(string msg, bool success)
        {
            string str = string.Format("HiTipsShow(\"{0}\", {1})", msg, success ? "'success'" : "'error'");
            if (!this.Page.ClientScript.IsClientScriptBlockRegistered("ServerMessageScript"))
            {
                this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript", "<script language='JavaScript' defer='defer'>setTimeout(function(){" + str + "},300);</script>");
            }
        }

        protected virtual void ShowMsgAndReUrl(string msg, bool success, string url)
        {
            string str = string.Format("ShowMsgAndReUrl(\"{0}\", {1}, \"{2}\")", msg, success ? "true" : "false", url);
            if (!this.Page.ClientScript.IsClientScriptBlockRegistered("ServerMessageScript"))
            {
                this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript", "<script language='JavaScript' defer='defer'>setTimeout(function(){" + str + "},300);</script>");
            }
        }

        protected virtual void ShowMsgAndReUrl(string msg, bool success, string url, string target)
        {
            string str = string.Format("ShowMsgAndReUrl(\"{0}\", {1}, \"{2}\",\"{3}\")", new object[] { msg, success ? "true" : "false", url, target });
            if (!this.Page.ClientScript.IsClientScriptBlockRegistered("ServerMessageScript"))
            {
                this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript", "<script language='JavaScript' defer='defer'>setTimeout(function(){" + str + "},300);</script>");
            }
        }

        protected virtual void ShowMsgToTarget(string msg, bool success, string targentname)
        {
            string str3;
            string str = string.Empty;
            if (((str3 = targentname.ToLower()) != null) && ((str3 == "parent") || (str3 == "top")))
            {
                str = targentname + ".";
            }
            string str2 = string.Format("{2}HiTipsShow(\"{0}\", {1})", msg, success ? "'success'" : "'error'", str);
            if (!this.Page.ClientScript.IsClientScriptBlockRegistered("ServerMessageScript"))
            {
                this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript", "<script language='JavaScript' defer='defer'>setTimeout(function(){" + str2 + "},300);</script>");
            }
        }

        public string ModuleId { get; private set; }

        public string PageId { get; private set; }

        public string SubPageId { get; private set; }
    }
}

