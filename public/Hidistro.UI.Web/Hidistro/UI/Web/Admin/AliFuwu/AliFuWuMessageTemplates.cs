namespace Hidistro.UI.Web.Admin.AliFuwu
{
    using Aop.Api.Response;
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Core.Entities;
    using Hidistro.Entities.VShop;
    using Hidistro.UI.Common.Controls;
    using Hidistro.UI.ControlPanel.Utility;
    using Hishop.AlipayFuwu.Api.Model;
    using Hishop.AlipayFuwu.Api.Util;
    using Hishop.Weixin.MP.Api;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class AliFuWuMessageTemplates : AdminPage
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
        protected HiddenField hfAppID;
        protected HiddenField hfWeiXinAccessToken;
        protected HiddenField hiddSceneId;
        protected Image imgHeadImage;
        protected Image imgQRCode;
        protected Repeater rptAdminUserList;
        protected Repeater rptAliFuWuMessageTemplateList;
        protected Script Script5;
        protected Script Script6;
        protected TextBox txtAdminName;
        protected TextBox txtAdminRole;
        protected TextBox txtScanOpenID;
        public string WeixinAppId;

        protected AliFuWuMessageTemplates() : base("m11", "fwp05")
        {
            this.WeixinAppId = "";
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
                Type = 1
            };
            string retInfo = "";
            bool success = false;
            success = VShopHelper.SaveAdminUserMsgList(true, myList, myList.UserOpenId, out retInfo);
            this.DataListBind();
            this.ShowMsg(retInfo, success);
        }

        protected void btnSaveTemplatesList_Click(object sender, EventArgs e)
        {
            List<MessageTemplate> templates = new List<MessageTemplate>();
            for (int i = 0; i < this.rptAliFuWuMessageTemplateList.Items.Count; i++)
            {
                MessageTemplate item = new MessageTemplate {
                    MessageType = ((HiddenField) this.rptAliFuWuMessageTemplateList.Items[i].FindControl("hdfMessageType")).Value,
                    SendWeixin = true,
                    WeixinTemplateId = ((TextBox) this.rptAliFuWuMessageTemplateList.Items[i].FindControl("txtTemplateId")).Text.Trim()
                };
                templates.Add(item);
            }
            VShopHelper.UpdateAliFuWuSettings(templates);
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

        private void DataListBind()
        {
            this.rptAdminUserList.DataSource = VShopHelper.GetAdminUserMsgList(1);
            this.rptAdminUserList.DataBind();
            IList<MessageTemplate> aliFuWuMessageTemplates = VShopHelper.GetAliFuWuMessageTemplates();
            this.rptAliFuWuMessageTemplateList.DataSource = aliFuWuMessageTemplates;
            this.rptAliFuWuMessageTemplateList.DataBind();
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
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            this.WeixinAppId = masterSettings.WeixinAppId;
            this.hfAppID.Value = masterSettings.WeixinAppId;
            string str = TokenApi.GetToken_Message(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret);
            this.hfWeiXinAccessToken.Value = str;
            this.ShowQRImage();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                if (AlipayFuwuConfig.appId.Length < 0x10)
                {
                    this.ShowMsgAndReUrl("请先绑定服务窗", false, "AliFuwuConfig.aspx");
                }
                else
                {
                    this.DataListBind();
                }
            }
        }

        protected void rptAdminUserList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
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
                    this.DataListBind();
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
            string str = "bind" + DateTime.Now.ToString("yyyyMMddHHmmss");
            try
            {
                QrcodeInfo info2 = new QrcodeInfo {
                    codeType = "TEMP",
                    showLogo = "Y",
                    expireSecond = 600
                };
                codeInfo info3 = new codeInfo();
                scene scene = new scene {
                    sceneId = str
                };
                info3.scene = scene;
                info2.codeInfo = info3;
                QrcodeInfo codeInfo = info2;
                this.hiddSceneId.Value = str;
                AlipayMobilePublicQrcodeCreateResponse response = AliOHHelper.QrcodeSend(codeInfo);
                if ((response != null) && (response.Code == 200L))
                {
                    this.imgQRCode.ImageUrl = response.CodeImg;
                }
                else
                {
                    this.imgQRCode.AlternateText = "未成功获取服务窗授权";
                }
            }
            catch (Exception exception)
            {
                AliOHHelper.log(exception.Message);
            }
        }
    }
}

