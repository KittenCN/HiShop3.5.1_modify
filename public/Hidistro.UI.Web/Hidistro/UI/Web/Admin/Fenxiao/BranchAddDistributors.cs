namespace Hidistro.UI.Web.Admin.Fenxiao
{
    using Hidistro.ControlPanel.Members;
    using Hidistro.ControlPanel.Store;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class BranchAddDistributors : AdminPage
    {
        protected Button batchCreate;
        protected Button btnExport;
        protected HtmlGenericControl EditTitle;
        private static IList<string> exportdistirbutors;
        protected HtmlInputRadioButton radioaccount;
        protected HtmlInputRadioButton radionumber;
        protected Hidistro.UI.Common.Controls.Style Style1;
        protected HtmlTextArea txtdistributornames;
        protected HtmlInputText txtnumber;
        protected HtmlInputText txtslsdistributors;

        protected BranchAddDistributors() : base("m05", "fxp07")
        {
        }

        private void batchCreate_Click(object sender, EventArgs e)
        {
            try
            {
                string distributorname = this.txtslsdistributors.Value;
                int referruserId = MemberHelper.IsExiteDistributorNames(distributorname);
                if (string.IsNullOrEmpty(distributorname) || (referruserId <= 0))
                {
                    this.ShowMsg("输入的推荐分销商不存在！", false);
                }
                else if (this.radionumber.Checked)
                {
                    if (string.IsNullOrEmpty(this.txtnumber.Value.Trim()))
                    {
                        this.ShowMsg("请输入要生成的账号数量", false);
                    }
                    else
                    {
                        int result = 0;
                        int.TryParse(this.txtnumber.Value, out result);
                        if ((result <= 0) || (result > 0x3e7))
                        {
                            this.ShowMsg("数值必须在1~999之间的正整数", false);
                        }
                        else if (this.CheckDistributorIsCanAuthorization(result))
                        {
                            exportdistirbutors = MemberHelper.BatchCreateMembers(this.CreateDistributros(result), referruserId, "1");
                            this.ShowMsg("批量制作成功", true);
                            if ((exportdistirbutors != null) && (exportdistirbutors.Count > 0))
                            {
                                this.btnExport.Visible = true;
                                this.btnExport.Text = "导出分销商";
                            }
                        }
                    }
                }
                else
                {
                    string str2 = this.txtdistributornames.Value;
                    IList<string> distributornames = new List<string>();
                    if (string.IsNullOrEmpty(str2))
                    {
                        this.ShowMsg("请输入要制作的账号", false);
                    }
                    else
                    {
                        bool flag = false;
                        string[] source = str2.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                        if (this.CheckDistributorIsCanAuthorization(source.Count<string>()))
                        {
                            foreach (string str3 in source)
                            {
                                if ((string.IsNullOrEmpty(str3) || (str3.Length < 6)) || (str3.Length > 50))
                                {
                                    flag = true;
                                    break;
                                }
                                distributornames.Add(str3);
                            }
                            if (flag)
                            {
                                this.ShowMsg("每个账号长度在2~10个字符", false);
                            }
                            else
                            {
                                exportdistirbutors = MemberHelper.BatchCreateMembers(distributornames, referruserId, "2");
                                if ((exportdistirbutors != null) && (exportdistirbutors.Count > 0))
                                {
                                    this.btnExport.Visible = true;
                                    this.btnExport.Text = "导出失败分销商";
                                    this.ShowMsg("部份分销商生成失败，请查看导出文档！", true);
                                }
                                else
                                {
                                    this.btnExport.Visible = false;
                                    this.ShowMsg("生成成功！", true);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            this.Page.Response.Clear();
            this.Page.Response.Buffer = true;
            this.Page.Response.Charset = "GB2312";
            this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=tempdistributors.txt");
            this.Page.Response.ContentEncoding = Encoding.GetEncoding("GB2312");
            base.Response.ContentType = "text/plain";
            this.EnableViewState = false;
            CultureInfo formatProvider = new CultureInfo("ZH-CN", true);
            new StringWriter(formatProvider);
            this.Page.Response.Write(string.Join("\r\n", exportdistirbutors));
            this.Page.Response.End();
        }

        private bool CheckDistributorIsCanAuthorization(int number)
        {
            int leftNumber = 0;
            if (!SystemAuthorizationHelper.CheckDistributorIsCanAuthorization(number, out leftNumber))
            {
                this.ShowMsg(string.Format("对不起，你最多只能再生成{0}个分销商！请确认后重试 ", leftNumber), false);
                return false;
            }
            return true;
        }

        private IList<string> CreateDistributros(int len)
        {
            IList<string> list = new List<string>();
            Random random = new Random(Environment.TickCount);
            for (int i = 0; i < len; i++)
            {
                list.Add(random.Next(0xa98ac7, 0x5f5e0ff).ToString());
            }
            return list;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(base.Request["action"]) && (base.Request["action"] == "SearchKey"))
            {
                string allDistributorsName = string.Empty;
                if (!string.IsNullOrEmpty(base.Request["keyword"]))
                {
                    allDistributorsName = MemberHelper.GetAllDistributorsName(base.Request["keyword"]);
                }
                base.Response.ContentType = "application/json";
                base.Response.Write("{\"data\":[" + allDistributorsName + "]}");
                base.Response.End();
            }
            if (!base.IsPostBack)
            {
                exportdistirbutors = null;
            }
            this.batchCreate.Click += new EventHandler(this.batchCreate_Click);
            this.btnExport.Click += new EventHandler(this.btnExport_Click);
        }
    }
}

