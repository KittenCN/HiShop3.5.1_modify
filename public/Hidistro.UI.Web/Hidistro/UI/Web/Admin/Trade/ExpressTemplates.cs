namespace Hidistro.UI.Web.Admin.Trade
{
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Sales;
    using Hidistro.ControlPanel.Settings;
    using Hidistro.Core;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.IO;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Xml;

    public class ExpressTemplates : AdminPage
    {
        protected Grid grdExpressTemplates;
        protected Button lkbDeleteCheck;
        protected Pager pager;
        protected Pager pager1;
        protected HtmlForm thisForm;

        protected ExpressTemplates() : base("m03", "ddp11")
        {
        }

        private void BindExpressTemplates()
        {
            this.grdExpressTemplates.DataSource = SalesHelper.GetExpressTemplates();
            this.grdExpressTemplates.DataBind();
        }

        private void DeleteCheck()
        {
            string expressIds = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                expressIds = base.Request["CheckBoxGroup"];
            }
            if (expressIds.Length <= 0)
            {
                this.ShowMsg("请先选择要删除的快递单模板", false);
            }
            else
            {
                int num = SettingsHelper.DeleteExpressTemplates(expressIds);
                if (num > 0)
                {
                    foreach (string str2 in expressIds.Split(new char[] { ',' }))
                    {
                        for (int i = 0; i < this.grdExpressTemplates.Rows.Count; i++)
                        {
                            if (this.grdExpressTemplates.DataKeys[i].Value.ToString() == str2.ToString().Trim())
                            {
                                Literal literal = this.grdExpressTemplates.Rows[i].FindControl("litXmlFile") as Literal;
                                this.DeleteXmlFile(literal.Text);
                                break;
                            }
                        }
                    }
                }
                this.BindExpressTemplates();
                this.ShowMsg(string.Format("成功删除了{0}个快递单模板", num), true);
            }
        }

        private void DeleteXmlFile(string xmlfile)
        {
            string path = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Storage/master/flex/{0}", xmlfile));
            if (File.Exists(path))
            {
                XmlDocument document = new XmlDocument();
                document.Load(path);
                XmlNode node = document.SelectSingleNode("printer/pic");
                Globals.DelImgByFilePath(HttpContext.Current.Request.MapPath(Globals.ApplicationPath + string.Format("/Storage/master/flex/{0}", node.InnerText)));
                File.Delete(path);
            }
        }

        private void grdExpressTemplates_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow namingContainer = (GridViewRow) ((Control) e.CommandSource).NamingContainer;
            int expressId = (int) this.grdExpressTemplates.DataKeys[namingContainer.RowIndex].Value;
            if (e.CommandName == "SetYesOrNo")
            {
                SalesHelper.SetExpressIsUse(expressId);
                this.BindExpressTemplates();
            }
            else if (e.CommandName == "DeleteRow")
            {
                if (SalesHelper.DeleteExpressTemplate(expressId))
                {
                    Literal literal = this.grdExpressTemplates.Rows[namingContainer.RowIndex].FindControl("litXmlFile") as Literal;
                    this.DeleteXmlFile(literal.Text);
                    this.BindExpressTemplates();
                    this.ShowMsg("已经成功删除选择的快递单模板", true);
                }
                else
                {
                    this.ShowMsg("删除快递单模板失败", false);
                }
            }
            else if (e.CommandName == "IsDefault")
            {
                SettingsHelper.SetExpressIsDefault(expressId);
                this.BindExpressTemplates();
            }
        }

        private void lkbDeleteCheck_Click(object sender, EventArgs e)
        {
            this.DeleteCheck();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.grdExpressTemplates.RowCommand += new GridViewCommandEventHandler(this.grdExpressTemplates_RowCommand);
            this.lkbDeleteCheck.Click += new EventHandler(this.lkbDeleteCheck_Click);
            if (!this.Page.IsPostBack)
            {
                this.BindExpressTemplates();
            }
        }
    }
}

