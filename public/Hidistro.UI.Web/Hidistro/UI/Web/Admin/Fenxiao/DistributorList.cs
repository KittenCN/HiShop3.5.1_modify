namespace Hidistro.UI.Web.Admin.Fenxiao
{
    using Ajax;
    using ASPNET.WebControls;
    using Hidistro.ControlPanel.Store;
    using Hidistro.ControlPanel.VShop;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Core.Enums;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.StatisticsReport;
    using Hidistro.Entities.Store;
    using Hidistro.Messages;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.ControlPanel.Utility;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data;
    using System.Globalization;
    using System.Threading;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class DistributorList : AdminPage
    {
        protected HyperLink btnDownTaobao;
        protected Button btnSearchButton;
        protected Button CancleCheck;
        private string CellPhone;
        protected DistributorGradeDropDownList DrGrade;
        protected Button EditSave;
        protected TextBox EdittxtCellPhone;
        protected TextBox EdittxtPassword;
        protected TextBox EdittxtQQNum;
        protected TextBox EdittxtRealname;
        protected HiddenField EditUserID;
        protected Button FrozenCheck;
        private string Grade;
        protected Button GradeCheck;
        protected DistributorGradeDropDownList GradeCheckList;
        protected HiddenField hdVery;
        protected PageSize hrefPageSize;
        protected Label lboldCommission;
        protected Literal ListActive;
        protected Literal Listfrozen;
        protected string localUrl;
        private string MicroSignal;
        private UpdateStatistics myEvent;
        private StatisticNotifier myNotifier;
        protected Pager pager;
        protected Button PassCheck;
        private string RealName;
        protected Repeater reDistributor;
        private string Status;
        private string StoreName;
        private string StoreName1;
        protected TextBox txtCellPhone;
        protected HtmlInputText txtCommission;
        protected TextBox txtConformPassword;
        protected TextBox txtMicroSignal;
        protected HtmlInputText txtoldCommission;
        protected TextBox txtPassword;
        protected TextBox txtRealName;
        protected HtmlInputText txtSetCommissionBark;
        protected TextBox txtStoreName;
        protected TextBox txtStoreName1;
        protected HtmlInputText txtUserId;
        protected TextBox txtUserName;
        protected Button UpdateCommission;
        private string UserName;

        protected DistributorList() : base("m05", "fxp03")
        {
            this.StoreName = "";
            this.StoreName1 = "";
            this.Grade = "0";
            this.Status = "0";
            this.RealName = "";
            this.CellPhone = "";
            this.MicroSignal = "";
            this.UserName = "";
            this.localUrl = string.Empty;
            this.myNotifier = new StatisticNotifier();
            this.myEvent = new UpdateStatistics();
        }

        protected void addSession()
        {
            string str = DateTime.Now.ToString("yyyyMMddHHmmss");
            this.hdVery.Value = str;
            this.Session["modifyCommissionVeryCode"] = str;
        }

        private void BindData()
        {
            DistributorsQuery entity = new DistributorsQuery {
                GradeId = int.Parse(this.Grade),
                StoreName = this.StoreName,
                CellPhone = this.CellPhone,
                StoreName1 = this.StoreName1,
                RealName = this.RealName,
                MicroSignal = this.MicroSignal,
                UserName = this.UserName,
                ReferralStatus = int.Parse(this.Status),
                PageIndex = this.pager.PageIndex,
                PageSize = this.pager.PageSize,
                SortOrder = SortAction.Desc,
                SortBy = "userid"
            };
            Globals.EntityCoding(entity, true);
            DbQueryResult result = VShopHelper.GetDistributors(entity, null, null);
            this.reDistributor.DataSource = result.Data;
            this.reDistributor.DataBind();
            this.pager.TotalRecords = result.TotalRecords;
            DataTable distributorsNum = VShopHelper.GetDistributorsNum();
            this.ListActive.Text = "分销商列表(" + distributorsNum.Rows[0]["active"].ToString() + ")";
            this.Listfrozen.Text = "已冻结(" + distributorsNum.Rows[0]["frozen"].ToString() + ")";
        }

        private void btnSearchButton_Click(object sender, EventArgs e)
        {
            this.ReBind(true);
        }

        private void CancleCheck_Click(object sender, EventArgs e)
        {
            string userids = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                userids = base.Request["CheckBoxGroup"];
            }
            if (userids.Length <= 0)
            {
                this.ShowMsg("请先选择要取消资质的分销商", false);
            }
            else
            {
                int num = DistributorsBrower.FrozenCommisionChecks(userids, "9");
                try
                {
                    this.myNotifier.updateAction = UpdateAction.MemberUpdate;
                    this.myNotifier.actionDesc = "批量取消分销商资质";
                    this.myNotifier.RecDateUpdate = DateTime.Today;
                    this.myNotifier.DataUpdated += new StatisticNotifier.DataUpdatedEventHandler(this.myEvent.Update);
                    this.myNotifier.UpdateDB();
                }
                catch (Exception)
                {
                }
                this.ShowMsgAndReUrl(string.Format("成功取消了{0}个分销商的资质", num), true, this.localUrl);
                this.ReBind(true);
            }
        }

        private void EditPasswordSendWeiXinMessage(string userIds, string password)
        {
            try
            {
                List<MemberInfo> memberInfoList = MemberProcessor.GetMemberInfoList(userIds);
                if ((memberInfoList != null) && (memberInfoList.Count > 0))
                {
                    foreach (MemberInfo info in memberInfoList)
                    {
                        if (info != null)
                        {
                            Messenger.SendWeiXinMsg_PasswordReset(info, password);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void EditSave_Click(object sender, EventArgs e)
        {
            string userid = this.EditUserID.Value.Trim();
            if (userid.Length <= 0)
            {
                this.ShowMsg("用户ID为空，参数异常！", false);
            }
            else
            {
                string sourceData = this.EdittxtPassword.Text.Trim();
                if ((sourceData.Length > 0) && ((sourceData.Length > 20) || (sourceData.Length < 6)))
                {
                    this.ShowMsg("用户密码长度在6-20位之间！", false);
                }
                else if (DistributorsBrower.EditDisbutosInfos(userid, this.EdittxtQQNum.Text, this.EdittxtCellPhone.Text, this.EdittxtRealname.Text, HiCryptographer.Md5Encrypt(sourceData)))
                {
                    this.ReBind(true);
                }
                else
                {
                    this.ShowMsg("成功用户信息失败", false);
                }
            }
        }

        private void FrozenCheck_Click(object sender, EventArgs e)
        {
            string userids = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                userids = base.Request["CheckBoxGroup"];
            }
            if (userids.Length <= 0)
            {
                this.ShowMsg("请先选择要冻结的分销商", false);
            }
            else
            {
                int num = DistributorsBrower.FrozenCommisionChecks(userids, "1");
                this.ShowMsgAndReUrl(string.Format("成功冻结了{0}个分销商", num), true, this.localUrl);
                this.ReBind(true);
            }
        }

        private void GradeCheck_Click(object sender, EventArgs e)
        {
            ThreadStart start = null;
            string strIds = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                strIds = base.Request["CheckBoxGroup"];
            }
            string EditGrade = this.GradeCheckList.SelectedValue.ToString();
            if (strIds.Length <= 0)
            {
                this.ShowMsg("请先选择要修改等级的分销商", false);
            }
            else
            {
                int num = DistributorsBrower.EditCommisionsGrade(strIds, EditGrade);
                if (num > 0)
                {
                    if (start == null)
                    {
                        start = delegate {
                            DistributorGradeInfo distributorGradeInfo = DistributorGradeBrower.GetDistributorGradeInfo(int.Parse(EditGrade));
                            if (distributorGradeInfo != null)
                            {
                                foreach (string str in strIds.Split(new char[] { ',' }))
                                {
                                    int result = 0;
                                    if (int.TryParse(str, out result))
                                    {
                                        try
                                        {
                                            MemberInfo member = MemberProcessor.GetMember(result, true);
                                            if (member != null)
                                            {
                                                Messenger.SendWeiXinMsg_DistributorGradeChange(member, distributorGradeInfo.Name);
                                            }
                                        }
                                        catch (Exception exception)
                                        {
                                            Globals.Debuglog("升级变动提醒发送错误：" + exception.Message + "-- " + strIds, "_Debuglog.txt");
                                        }
                                    }
                                }
                            }
                        };
                    }
                    new Thread(start).Start();
                }
                this.ShowMsgAndReUrl(string.Format("成功修改了{0}个分销商的等级", num), true, this.localUrl);
                this.BindData();
            }
        }

        private void LoadParameters()
        {
            if (!this.Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StoreName"]))
                {
                    this.StoreName = base.Server.UrlDecode(this.Page.Request.QueryString["StoreName"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Grade"]))
                {
                    this.Grade = base.Server.UrlDecode(this.Page.Request.QueryString["Grade"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Status"]))
                {
                    this.Status = base.Server.UrlDecode(this.Page.Request.QueryString["Status"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["RealName"]))
                {
                    this.RealName = base.Server.UrlDecode(this.Page.Request.QueryString["RealName"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["CellPhone"]))
                {
                    this.CellPhone = base.Server.UrlDecode(this.Page.Request.QueryString["CellPhone"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["MicroSignal"]))
                {
                    this.MicroSignal = base.Server.UrlDecode(this.Page.Request.QueryString["MicroSignal"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["UserName"]))
                {
                    this.UserName = base.Server.UrlDecode(this.Page.Request.QueryString["UserName"]);
                }
                if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StoreName1"]))
                {
                    this.StoreName1 = base.Server.UrlDecode(this.Page.Request.QueryString["StoreName1"]);
                }
                this.txtStoreName1.Text = this.StoreName1;
                this.txtStoreName.Text = this.StoreName;
                this.DrGrade.SelectedValue = new int?(int.Parse(this.Grade));
                this.txtCellPhone.Text = this.CellPhone;
                this.txtMicroSignal.Text = this.MicroSignal;
                this.txtUserName.Text = this.UserName;
                this.txtRealName.Text = this.RealName;
            }
            else
            {
                this.StoreName1 = this.txtStoreName1.Text;
                this.StoreName = this.txtStoreName.Text;
                this.Grade = this.DrGrade.SelectedValue.ToString();
                this.CellPhone = this.txtCellPhone.Text;
                this.RealName = this.txtRealName.Text;
                this.MicroSignal = this.txtMicroSignal.Text;
                this.UserName = this.txtUserName.Text;
                if (string.IsNullOrEmpty(this.Grade))
                {
                    this.Grade = "0";
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Utility.RegisterTypeForAjax(typeof(DistributorList));
            this.localUrl = base.Request.Url.ToString();
            string str = base.Request.QueryString["task"];
            if (!base.IsPostBack)
            {
                string str2 = DateTime.Now.ToString("yyyyMMddHHmmss");
                this.hdVery.Value = str2;
                this.Session["modifyCommissionVeryCode"] = str2;
            }
            if (!string.IsNullOrEmpty(str))
            {
                string s = "{error:1,msg:'未定义操作！'}";
                if (str == "readinfo")
                {
                    int num;
                    if (int.TryParse(this.Page.Request.QueryString["UserId"], out num))
                    {
                        DistributorsQuery entity = new DistributorsQuery {
                            UserId = int.Parse(this.Page.Request.QueryString["UserId"]),
                            ReferralStatus = -1,
                            PageIndex = 1,
                            PageSize = 1,
                            SortOrder = SortAction.Desc,
                            SortBy = "userid"
                        };
                        Globals.EntityCoding(entity, true);
                        DbQueryResult result = VShopHelper.GetDistributors(entity, null, null);
                        if (result.Data != null)
                        {
                            DataTable data = new DataTable();
                            data = (DataTable) result.Data;
                            s = s = "{error:0,Data:" + JsonConvert.SerializeObject(data) + "}";
                        }
                        else
                        {
                            s = "{error:1,msg:'分销商信息不存在'}";
                        }
                    }
                    else
                    {
                        s = "{error:1,msg:'userid错误'}";
                    }
                }
                base.Response.Write(s);
                base.Response.End();
            }
            this.reDistributor.ItemCommand += new RepeaterCommandEventHandler(this.reDistributor_ItemCommand);
            this.btnSearchButton.Click += new EventHandler(this.btnSearchButton_Click);
            this.FrozenCheck.Click += new EventHandler(this.FrozenCheck_Click);
            this.CancleCheck.Click += new EventHandler(this.CancleCheck_Click);
            this.PassCheck.Click += new EventHandler(this.PassCheck_Click);
            this.GradeCheck.Click += new EventHandler(this.GradeCheck_Click);
            this.EditSave.Click += new EventHandler(this.EditSave_Click);
            this.UpdateCommission.Click += new EventHandler(this.UpdateCommission_Click);
            this.LoadParameters();
            if (!base.IsPostBack)
            {
                this.BindData();
                this.GradeCheckList.DataBind();
                this.DrGrade.DataBind();
                this.DrGrade.SelectedValue = new int?(int.Parse(this.Grade));
            }
        }

        private void PassCheck_Click(object sender, EventArgs e)
        {
            string userids = "";
            if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
            {
                userids = base.Request["CheckBoxGroup"];
            }
            if (userids.Length <= 0)
            {
                this.ShowMsg("请先选择要修改密码的分销商", false);
            }
            else if ((this.txtPassword.Text.Trim().Length < 6) || (this.txtPassword.Text.Trim().Length > 20))
            {
                this.ShowMsg("密码长度在6-20位之间！", false);
            }
            else if (this.txtPassword.Text != this.txtConformPassword.Text)
            {
                this.ShowMsg("两次输入密码不一致！", false);
            }
            else
            {
                int num = MemberProcessor.SetMultiplePwd(userids, HiCryptographer.Md5Encrypt(this.txtPassword.Text.Trim()));
                this.EditPasswordSendWeiXinMessage(userids, this.txtPassword.Text.Trim());
                this.ShowMsgAndReUrl(string.Format("成功修改了{0}个分销商的密码", num), true, this.localUrl);
            }
        }

        private void ReBind(bool isSearch)
        {
            NameValueCollection queryStrings = new NameValueCollection();
            queryStrings.Add("Grade", this.DrGrade.Text);
            queryStrings.Add("StoreName", this.txtStoreName.Text);
            queryStrings.Add("StoreName1", this.txtStoreName1.Text);
            queryStrings.Add("CellPhone", this.txtCellPhone.Text);
            queryStrings.Add("RealName", this.txtRealName.Text);
            queryStrings.Add("MicroSignal", this.txtMicroSignal.Text);
            queryStrings.Add("UserName", this.txtUserName.Text);
            queryStrings.Add("Status", this.Status);
            queryStrings.Add("pageSize", this.pager.PageSize.ToString(CultureInfo.InvariantCulture));
            if (!isSearch)
            {
                queryStrings.Add("pageIndex", this.pager.PageIndex.ToString(CultureInfo.InvariantCulture));
            }
            base.ReloadPage(queryStrings);
        }

        private void reDistributor_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Frozen")
            {
                if (!DistributorsBrower.FrozenCommision(int.Parse(e.CommandArgument.ToString()), "1"))
                {
                    this.ShowMsg("冻结失败", false);
                    return;
                }
                this.ShowMsgAndReUrl("冻结成功", true, this.localUrl);
                this.ReBind(true);
            }
            if (e.CommandName == "Thaw")
            {
                if (!DistributorsBrower.FrozenCommision(int.Parse(e.CommandArgument.ToString()), "0"))
                {
                    this.ShowMsg("解冻失败", false);
                    return;
                }
                this.ShowMsgAndReUrl("解冻成功", true, this.localUrl);
                this.ReBind(true);
            }
            if (e.CommandName == "Forbidden")
            {
                if (DistributorsBrower.FrozenCommision(int.Parse(e.CommandArgument.ToString()), "9"))
                {
                    this.ShowMsgAndReUrl("取消资质成功！", true, this.localUrl);
                    try
                    {
                        this.myNotifier.updateAction = UpdateAction.MemberUpdate;
                        this.myNotifier.actionDesc = "取消分销商资质";
                        this.myNotifier.RecDateUpdate = DateTime.Today;
                        this.myNotifier.DataUpdated += new StatisticNotifier.DataUpdatedEventHandler(this.myEvent.Update);
                        this.myNotifier.UpdateDB();
                    }
                    catch (Exception)
                    {
                    }
                    this.ReBind(true);
                }
                else
                {
                    this.ShowMsg("取消资质失败", false);
                }
            }
        }

        private void UpdateCommission_Click(object sender, EventArgs e)
        {
            if ((this.Session["modifyCommissionVeryCode"] == null) || !(this.Session["modifyCommissionVeryCode"].ToString() == this.hdVery.Value))
            {
                this.ShowMsgAndReUrl("请勿重复提交数据！", false, this.localUrl);
            }
            else
            {
                this.Session["modifyCommissionVeryCode"] = null;
                int userId = Convert.ToInt32(this.txtUserId.Value.Trim());
                ManagerInfo currentManager = ManagerHelper.GetCurrentManager();
                decimal commission = 0M;
                try
                {
                    commission = Convert.ToDecimal(this.txtCommission.Value.Trim());
                }
                catch
                {
                    this.ShowMsg("佣金值应为数字", false);
                    this.addSession();
                    return;
                }
                if ((Convert.ToDecimal(this.txtoldCommission.Value.Trim()) + commission) >= 0M)
                {
                    string str = this.txtSetCommissionBark.Value.Trim();
                    if (string.IsNullOrEmpty(str))
                    {
                        this.ShowMsg("备注不能为空", false);
                    }
                    else if (str.Length > 200)
                    {
                        this.ShowMsg("备注内容过长", false);
                        this.addSession();
                    }
                    else
                    {
                        str = currentManager.UserName + ":手动调整佣金：" + str;
                        if (VShopHelper.UpdateCommission(userId, commission, str))
                        {
                            this.BindData();
                            this.ShowMsgAndReUrl("成功调整佣金", true, this.localUrl);
                            this.txtCommission.Value = "";
                            this.txtSetCommissionBark.Value = "";
                        }
                        else
                        {
                            this.ShowMsg("调整佣金失败", false);
                            this.txtCommission.Value = "";
                            this.txtSetCommissionBark.Value = "";
                        }
                    }
                }
                else
                {
                    this.ShowMsg("减去佣金不能大于当前佣金", false);
                    this.addSession();
                }
            }
        }
    }
}

