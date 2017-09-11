namespace Hidistro.UI.Web.Admin.WeiXin
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Store;
    using Hidistro.Entities.VShop;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.Weixin.MP.Api;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    [PrivilegeCheck(Privilege.ProductCategory)]
    public class MessageTemplets : AdminPage
    {
        protected Button btnSaveRole;
        protected Button btnSaveTemplatesList;
        protected Button btnSaveUserMsgDetail;
        protected CheckBox cbMsg1;
        protected CheckBox cbMsg2;
        protected CheckBox cbMsg3;
        protected CheckBox cbMsg4;
        protected CheckBox cbMsg5;
        protected CheckBox cbMsg6;
        protected CheckBoxList cbPowerListDistributors;
        protected CheckBoxList cbPowerListMember;
        protected string CodeTicket;
        protected HiddenField hfAppID;
        protected HiddenField hfWeiXinAccessToken;
        protected Image imgHeadImage;
        protected Image imgQRCode;
        protected Repeater rptAdminUserList;
        protected Repeater rptList;
        protected Script Script5;
        protected Script Script6;
        private SiteSettings siteSettings;
        protected TextBox txtAdminName;
        protected TextBox txtAdminRole;
        protected TextBox txtScanOpenID;
        public string WeixinAppId;

        protected MessageTemplets() : base("m06", "wxp06")
        {
            this.WeixinAppId = "";
            this.siteSettings = SettingsManager.GetMasterSettings(false);
            this.CodeTicket = "";
        }

        private void BindData()
        {
            this.rptAdminUserList.DataSource = VShopHelper.GetAdminUserMsgList(0);
            this.rptAdminUserList.DataBind();
            this.rptList.DataSource = VShopHelper.GetMessageTemplates();
            this.rptList.DataBind();
            int num = 0;
            this.cbPowerListDistributors.Items.Clear();
            foreach (DataRow row in VShopHelper.GetAdminUserMsgDetail(true).Rows)
            {
                ListItem item = new ListItem(row["DetailName"].ToString(), row["DetailType"].ToString());
                this.cbPowerListDistributors.Items.Add(item);
                this.cbPowerListDistributors.Items[num].Selected = row["IsSelected"].ToString() == "1";
                num++;
            }
            num = 0;
            this.cbPowerListMember.Items.Clear();
            foreach (DataRow row2 in VShopHelper.GetAdminUserMsgDetail(false).Rows)
            {
                ListItem item2 = new ListItem(row2["DetailName"].ToString(), row2["DetailType"].ToString());
                this.cbPowerListMember.Items.Add(item2);
                this.cbPowerListMember.Items[num].Selected = row2["IsSelected"].ToString() == "1";
                num++;
            }
            this.WeixinAppId = this.siteSettings.WeixinAppId;
            this.hfAppID.Value = this.siteSettings.WeixinAppId;
            string str = TokenApi.GetToken_Message(this.siteSettings.WeixinAppId, this.siteSettings.WeixinAppSecret);
            this.hfWeiXinAccessToken.Value = str;
            this.ShowQRImage();
        }

        protected void btnGetCreateTicket_Click(object sender, EventArgs e)
        {
            this.ShowQRImage();
        }

        protected void btnSaveRole_Click(object sender, EventArgs e)
        {
            MsgList myList = new MsgList {
                UserOpenId = this.txtScanOpenID.Text.Trim(),
                RoleName = this.txtAdminRole.Text.Trim(),
                RealName = this.txtAdminName.Text.Trim(),
                Msg1 = Convert.ToInt32(this.cbMsg1.Checked),
                Msg2 = Convert.ToInt32(this.cbMsg2.Checked),
                Msg3 = Convert.ToInt32(this.cbMsg3.Checked),
                Msg4 = Convert.ToInt32(this.cbMsg4.Checked),
                Msg5 = Convert.ToInt32(this.cbMsg5.Checked),
                Msg6 = Convert.ToInt32(this.cbMsg6.Checked),
                Type = 0
            };
            string retInfo = "";
            bool success = false;
            success = VShopHelper.SaveAdminUserMsgList(true, myList, myList.UserOpenId, out retInfo);
            this.BindData();
            this.ShowMsg(retInfo, success);
        }

        protected void btnSaveTemplatesList_Click(object sender, EventArgs e)
        {
            List<MessageTemplate> templates = new List<MessageTemplate>();
            for (int i = 0; i < this.rptList.Items.Count; i++)
            {
                MessageTemplate item = new MessageTemplate {
                    MessageType = ((HiddenField) this.rptList.Items[i].FindControl("hdfMessageType")).Value,
                    SendWeixin = true,
                    WeixinTemplateId = ((TextBox) this.rptList.Items[i].FindControl("txtTemplateId")).Text.Trim()
                };
                templates.Add(item);
            }
            VShopHelper.UpdateSettings(templates);
            this.ShowMsg("保存设置成功", true);
        }

        protected void btnSaveUserMsgDetail_Click(object sender, EventArgs e)
        {
            List<MsgDetail> templates = new List<MsgDetail>();
            for (int i = 0; i < this.cbPowerListDistributors.Items.Count; i++)
            {
                MsgDetail item = new MsgDetail {
                    DetailType = this.cbPowerListDistributors.Items[i].Value,
                    IsSelectedByDistributor = this.cbPowerListDistributors.Items[i].Selected ? 1 : 0
                };
                templates.Add(item);
            }
            VShopHelper.UpdateWeiXinMsgDetail(true, templates);
            templates.Clear();
            for (int j = 0; j < this.cbPowerListMember.Items.Count; j++)
            {
                MsgDetail detail2 = new MsgDetail {
                    DetailType = this.cbPowerListMember.Items[j].Value,
                    IsSelectedByMember = this.cbPowerListMember.Items[j].Selected ? 1 : 0
                };
                templates.Add(detail2);
            }
            VShopHelper.UpdateWeiXinMsgDetail(false, templates);
            this.ShowMsg("保存成功！", true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                if (this.siteSettings.WeixinAppId.Length < 0x10)
                {
                    this.ShowMsgAndReUrl("请先绑定微信公众号", false, "wxconfig.aspx");
                }
                else
                {
                    this.BindData();
                }
            }
        }

        protected void rptAdminUserList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int result = 0;
            int.TryParse(e.CommandArgument.ToString(), out result);
            MsgList myList = new MsgList();
            TextBox box = e.Item.FindControl("txtUserOpenId") as TextBox;
            myList.UserOpenId = box.Text.Trim();
            TextBox box2 = e.Item.FindControl("txtRoleName") as TextBox;
            myList.RoleName = box2.Text.Trim();
            TextBox box3 = e.Item.FindControl("txtRealName") as TextBox;
            myList.RealName = box3.Text.Trim();
            myList.Msg1 = Convert.ToInt32((e.Item.FindControl("cbMsg1") as CheckBox).Checked);
            myList.Msg2 = Convert.ToInt32((e.Item.FindControl("cbMsg2") as CheckBox).Checked);
            myList.Msg3 = Convert.ToInt32((e.Item.FindControl("cbMsg3") as CheckBox).Checked);
            myList.Msg4 = Convert.ToInt32((e.Item.FindControl("cbMsg4") as CheckBox).Checked);
            myList.Msg5 = Convert.ToInt32((e.Item.FindControl("cbMsg5") as CheckBox).Checked);
            myList.Msg6 = Convert.ToInt32((e.Item.FindControl("cbMsg6") as CheckBox).Checked);
            string retInfo = "";
            bool success = false;
            string commandName = e.CommandName;
            if (commandName != null)
            {
                if (!(commandName == "Save"))
                {
                    if (!(commandName == "Delete"))
                    {
                        return;
                    }
                }
                else
                {
                    if ((((((myList.Msg1 + myList.Msg2) + myList.Msg3) + myList.Msg4) + myList.Msg5) + myList.Msg6) == 0)
                    {
                        this.ShowMsg("当前用户未选择任何消息提醒，无法保存。", false);
                        return;
                    }
                    success = VShopHelper.SaveAdminUserMsgList(false, myList, myList.UserOpenId, out retInfo);
                    this.BindData();
                    this.ShowMsg(retInfo, success);
                    return;
                }
                success = VShopHelper.DeleteAdminUserMsgList(myList, out retInfo);
                if (success)
                {
                    this.rptAdminUserList.Items[0].Visible = false;
                }
                this.ShowMsg(retInfo, success);
            }
        }

        protected void rptAdminUserList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                HiddenField field = e.Item.FindControl("hdfAutoID") as HiddenField;
                field.Value = DataBinder.Eval(e.Item.DataItem, "AutoID").ToString();
                e.Item.FindControl("lbMsgList");
                CheckBox box = e.Item.FindControl("cbMsg1") as CheckBox;
                CheckBox box2 = e.Item.FindControl("cbMsg2") as CheckBox;
                CheckBox box3 = e.Item.FindControl("cbMsg3") as CheckBox;
                CheckBox box4 = e.Item.FindControl("cbMsg4") as CheckBox;
                CheckBox box5 = e.Item.FindControl("cbMsg5") as CheckBox;
                CheckBox box6 = e.Item.FindControl("cbMsg6") as CheckBox;
                box.Checked = DataBinder.Eval(e.Item.DataItem, "Msg1").ToString() == "1";
                box2.Checked = DataBinder.Eval(e.Item.DataItem, "Msg2").ToString() == "1";
                box3.Checked = DataBinder.Eval(e.Item.DataItem, "Msg3").ToString() == "1";
                box4.Checked = DataBinder.Eval(e.Item.DataItem, "Msg4").ToString() == "1";
                box5.Checked = DataBinder.Eval(e.Item.DataItem, "Msg5").ToString() == "1";
                box6.Checked = DataBinder.Eval(e.Item.DataItem, "Msg6").ToString() == "1";
            }
        }

        protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                HiddenField field = e.Item.FindControl("hdfMessageType") as HiddenField;
                field.Value = DataBinder.Eval(e.Item.DataItem, "MessageType").ToString();
            }
        }

        private void ShowQRImage()
        {
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            string tICKET = BarCodeApi.CreateTicket(TokenApi.GetToken_Message(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret), "1", "QR_SCENE", "604800");
            this.CodeTicket = tICKET;
            this.imgQRCode.ImageUrl = BarCodeApi.GetQRImageUrlByTicket(tICKET);
        }
    }
}

