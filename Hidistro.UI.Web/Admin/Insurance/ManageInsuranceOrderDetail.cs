namespace Hidistro.UI.Web.Admin.Insurance
{
    using Entities.Insurance;
    using Entities.Orders;
    using Hidistro.ControlPanel.Members;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities;
    using Hidistro.Entities.Members;
    using Hidistro.Entities.Store;
    using Hidistro.Messages;
    using Hidistro.SaleSystem.Vshop;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Components.Validation;
    using SqlDal.Insurance;
    using SqlDal.Orders;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;


    public class ManageInsuranceOrderDetail : AdminPage
    {
        protected Button BindCheck;
        protected Button btnEditUser;
        private int currentUserId;
        protected MemberGradeDropDownList drpMemberRankList;
        protected Literal lblLoginNameValue;
        protected FormatedTimeLabel lblRegsTimeValue;
        protected Literal lblTotalAmountValue;
        protected Literal LitUserBindName;
        protected HtmlInputHidden PSWUserIds;
        protected RegionSelector rsddlRegion;
        protected HtmlForm thisForm;
        protected TextBox txtAddress;
        protected TextBox txtBindName;
        protected TextBox txtCardID;
        protected TextBox txtCellPhone;
        protected TextBox txtprivateEmail;
        protected TextBox txtQQ;
        protected TextBox txtRealName;
        protected TextBox txtUserPassword;
        InsuranceDao dao = new InsuranceDao();
        protected InsuranceOrderTypesDropDownList dropCashBackTypes;
        public InsuranceOrderInfo insurance;
        public string ifsuc = "0";
        protected ManageInsuranceOrderDetail() : base("m108", "bxp01")
        {
        }

        protected void BindCheck_Click(object sender, EventArgs e)
        {
            int result = 0;
            if (int.TryParse(this.BindCheck.CommandName, out result))
            {
                string text = this.txtBindName.Text;
                string sourceData = this.txtUserPassword.Text;
                MemberInfo bindusernameMember = MemberProcessor.GetBindusernameMember(text);
                if ((bindusernameMember != null) && (bindusernameMember.UserId != result))
                {
                    this.ShowMsg("该用户名已经被绑定", false);
                }
                else if (bindusernameMember != null)
                {
                    this.ShowMsg("该用户已经绑定系统帐号", false);
                    this.LoadMemberInfo();
                }
                else if (MemberProcessor.BindUserName(result, text, HiCryptographer.Md5Encrypt(sourceData)))
                {
                    this.ShowMsg("用户绑定成功!", true);
                    MemberInfo member = MemberProcessor.GetMember(result, false);
                    try
                    {
                        Messenger.SendWeiXinMsg_SysBindUserName(member, sourceData);
                    }
                    catch
                    {
                    }
                    this.LoadMemberInfo();
                }
                else
                {
                    this.ShowMsg("用户绑定失败!", false);
                }
            }
            else
            {
                this.ShowMsg("用户不存在！", false);
            }
        }

        protected void btnEditUser_Click(object sender, EventArgs e)
        {
            MemberInfo member = MemberHelper.GetMember(this.currentUserId);
            int gradeId = member.GradeId;
            member.GradeId = this.drpMemberRankList.SelectedValue.Value;
            member.RealName = this.txtRealName.Text.Trim();
            if (this.rsddlRegion.GetSelectedRegionId().HasValue)
            {
                member.RegionId = this.rsddlRegion.GetSelectedRegionId().Value;
                member.TopRegionId = RegionHelper.GetTopRegionId(member.RegionId);
            }
            member.Address = Globals.HtmlEncode(this.txtAddress.Text);
            member.QQ = this.txtQQ.Text;
            member.Email = member.QQ + "@qq.com";
            member.CellPhone = this.txtCellPhone.Text;
            member.Email = this.txtprivateEmail.Text;
            member.CardID = this.txtCardID.Text;
            if (this.ValidationMember(member))
            {
                if (gradeId != this.drpMemberRankList.SelectedValue.Value)
                {
                    try
                    {
                        Messenger.SendWeiXinMsg_MemberGradeChange(member);
                    }
                    catch
                    {
                    }
                }
                if (MemberHelper.Update(member))
                {
                    this.ShowMsgAndReUrl("成功修改了当前会员的个人资料", true, "/Admin/member/managemembers.aspx");
                }
                else
                {
                    this.ShowMsg("当前会员的个人信息修改失败", false);
                }
            }
        }

        private void LoadMemberInfo()
        {


            insurance = dao.GetModel(this.currentUserId);

            this.dropCashBackTypes.SelectedValue = insurance.InsuranceOrderStatu;
            if (insurance == null)
            {
                base.GotoResourceNotFound();
            }
            else
            {
               if(Request.Params["ctl00$ContentPlaceHolder1$dropCashBackTypes"]!=null)
                {
                    if(insurance.InsuranceOrderStatu!=int.Parse(Request.Params["ctl00$ContentPlaceHolder1$dropCashBackTypes"]))
                    {
                        int tmpstatus= int.Parse(Request.Params["ctl00$ContentPlaceHolder1$dropCashBackTypes"]);
                        insurance.InsuranceOrderStatu = tmpstatus;


                        if (tmpstatus == 2)
                        {
                            int JiaoQiangXian = 0, ShangYeSanXian = 0, SiJiXian = 0, ChengKeXian = 0, SunShiXian = 0, DaoQiangXian = 0, BoliXian = 0, ZiRanXian = 0, SheShuiXian = 0, TeYueXian = 0, SanFangZenRenXian = 0, HuaHenXian = 0;
                            if (Request.Params["JiaoQiangXian"] != null)
                            {
                                JiaoQiangXian = int.Parse(Request.Params["JiaoQiangXian"]);
                                insurance.InsuranceOrderJiaoQiangXian = "1" + "|" + int.Parse(Request.Params["JiaoQiangXian"]).ToString();
                            }
                            if (Request.Params["ShangYeSanXian"] != null)
                            {
                                ShangYeSanXian = int.Parse(Request.Params["ShangYeSanXian"]);
                                insurance.InsuranceOrderShangYeSanXian = "1" + "|" + int.Parse(Request.Params["ShangYeSanXian"]).ToString();
                            }
                           

                            if (Request.Params["SiJiXian"] != null)
                            {
                                SiJiXian = int.Parse(Request.Params["SiJiXian"]);
                                insurance.InsuranceOrderCheShangRenYuanSiJiXian = "1" + "|" + int.Parse(Request.Params["SiJiXian"]).ToString();
                            }

                            if (Request.Params["ChengKeXian"] != null)
                            {
                                ChengKeXian = int.Parse(Request.Params["ChengKeXian"]);
                                insurance.InsuranceOrderCheShangRenYuanChengKeXian = "1" + "|" + int.Parse(Request.Params["ChengKeXian"]).ToString();
                            }

                            if (Request.Params["SunShiXian"] != null)
                            {
                                SunShiXian = int.Parse(Request.Params["SunShiXian"]);
                                insurance.InsuranceOrderCheLiangSunShiXian = "1" + "|" + int.Parse(Request.Params["SunShiXian"]).ToString();
                            }

                            if (Request.Params["DaoQiangXian"] != null)
                            {
                                DaoQiangXian = int.Parse(Request.Params["DaoQiangXian"]);
                                insurance.InsuranceOrderDaoQiangXian = "1" + "|" + int.Parse(Request.Params["DaoQiangXian"]).ToString();
                            }

                            if (Request.Params["BoliXian"] != null)
                            {
                                BoliXian = int.Parse(Request.Params["BoliXian"]);
                                insurance.InsuranceOrderBoliXian = "1" + "|" + int.Parse(Request.Params["BoliXian"]).ToString();
                            }

                            if (Request.Params["ZiRanXian"] != null)
                            {
                                ZiRanXian = int.Parse(Request.Params["ZiRanXian"]);
                                insurance.InsuranceOrderZiRanXian = "1" + "|" + int.Parse(Request.Params["ZiRanXian"]).ToString();
                            }

                            if (Request.Params["SheShuiXian"] != null)
                            {
                                SheShuiXian = int.Parse(Request.Params["SheShuiXian"]);
                                insurance.InsuranceOrderSheShuiXian = "1" + "|" + int.Parse(Request.Params["SheShuiXian"]).ToString();
                            }

                            if (Request.Params["TeYueXian"] != null)
                            {
                                TeYueXian = int.Parse(Request.Params["TeYueXian"]);
                                insurance.InsuranceOrderTeYueXian = "1" + "|" + int.Parse(Request.Params["TeYueXian"]).ToString();
                            }

                            if (Request.Params["SanFangZenRenXian"] != null)
                            {
                                SanFangZenRenXian = int.Parse(Request.Params["SanFangZenRenXian"]);
                                insurance.InsuranceOrderSanFangZenRenXian = "1" + "|" + int.Parse(Request.Params["SanFangZenRenXian"]).ToString();
                            }

                            if (Request.Params["HuaHenXian"] != null)
                            {
                                HuaHenXian = int.Parse(Request.Params["HuaHenXian"]);
                                insurance.InsuranceOrderHuaHenXian = "1" + "|" + int.Parse(Request.Params["HuaHenXian"]).ToString();
                            }
                            insurance.InsuranceOrderAmount = JiaoQiangXian + ShangYeSanXian + SiJiXian + ChengKeXian + SunShiXian + DaoQiangXian + BoliXian + ZiRanXian + SheShuiXian + TeYueXian + SanFangZenRenXian + HuaHenXian;

                            //新增一个订单表
                            OrderTmpInfo order = new OrderTmpInfo();
                            order.OrderId = this.GenerateOrderId();
                            order.OrderMarking= this.GenerateOrderId();
                            order.OrderDate = DateTime.Now;
                            order.UserId = 0; order.Username = "";
                           

                            order.OrderStatus =1;
                            order.PointToCash = 0;
                            order.PointExchange = 0;
                            order.SplitState = 0;
                            order.DeleteBeforeState = 0;
                            order.ClientShortType = 1;
                            order.BargainDetialId = 0;
                            order.BalancePayMoneyTotal = 0;
                            order.BalancePayFreightMoneyTotal = 0;
                            order.CouponFreightMoneyTotal = 0;
                            order.UpdateDate = DateTime.Now;
                            order.LogisticsTools = 1;

                            order.Amount =order.OrderTotal= insurance.InsuranceOrderAmount;
                            order.Gateway = "hishop.plugins.payment.weixinrequest";
                            order.CouponCode = insurance.InsuranceOrderId.ToString();
                            order.PaymentTypeId = 88;
                            
                            bool num17 = new OrderDao().Add(order);
                        }

                        if (dao.Update(insurance))
                        {
                            string content = Request.Params["msgcontent"];
                            string ordernum = insurance.InsuranceOrderCreatDate.Value.ToString("yyyyMMddHHmmss") + insurance.InsuranceOrderId;
                            string orderstatus = ((Hidistro.Entities.Insurance.InsuranceOrderTypes)insurance.InsuranceOrderStatu.Value).ToString();
                            Messenger.SendWeiXinMsg_InsuranceOrder(insurance.InsuranceOrderOpenId, ordernum, orderstatus, content);
                            ifsuc = "1";
                        }
                        else
                        {
                            ifsuc = "2";
                        }


                    }
                }
            }
        }
        public string GenerateOrderId()
        {
            return Globals.GetGenerateId();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(this.Page.Request.QueryString["InsuranceOrderId"], out this.currentUserId))
            {
                base.GotoResourceNotFound();
            }
            else
            {
                this.Form.Action = "ManageInsuranceOrderDetail.aspx?InsuranceOrderId="+ this.currentUserId + "";
                this.dropCashBackTypes.DataBind();

                this.LoadMemberInfo();
                dropCashBackTypes.SelectedValue = insurance.InsuranceOrderStatu;
            }
        }

        private bool ValidationMember(MemberInfo member)
        {
            ValidationResults results = Hishop.Components.Validation.Validation.Validate<MemberInfo>(member, new string[] { "ValMember" });
            string msg = string.Empty;
            if (!results.IsValid)
            {
                foreach (ValidationResult result in (IEnumerable<ValidationResult>) results)
                {
                    msg = msg + Formatter.FormatErrorMessage(result.Message);
                }
                this.ShowMsg(msg, false);
            }
            return results.IsValid;
        }
    }
}

