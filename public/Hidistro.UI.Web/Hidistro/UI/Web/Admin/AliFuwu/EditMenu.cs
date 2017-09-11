namespace Hidistro.UI.Web.Admin.AliFuwu
{
    using Hidistro.ControlPanel.Store;
    using Hidistro.Core;
    using Hidistro.Entities.VShop;
    using Hidistro.UI.ControlPanel.Utility;
    using System;
    using System.Text;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Linq;
    using System.Collections.Generic;
    public class EditMenu : AdminPage
    {
        protected Button btnAddMenu;
        protected DropDownList ddlType;
        protected DropDownList ddlValue;
        protected HiddenField hdfIframeHeight;
        private int id;
        protected int iframeHeight;
        protected int iNameByteWidth;
        protected Literal lblParent;
        protected HtmlGenericControl liBind;
        protected HtmlGenericControl liParent;
        protected HtmlGenericControl liUrl;
        protected HtmlGenericControl liValue;
        private int oneLineHeight;
        private int parentid;
        protected TextBox txtMenuName;
        protected TextBox txtUrl;

        protected EditMenu() : base("m11", "fwp02")
        {
            this.iNameByteWidth = 8;
            this.iframeHeight = 240;
            this.oneLineHeight = 0x2c;
            this.id = Globals.RequestQueryNum("MenuID");
            this.parentid = Globals.RequestQueryNum("PID");
        }

        private void btnAddMenu_Click(object sender, EventArgs e)
        {
            if ((this.ddlType.SelectedValue == "1") && (this.ddlValue.Items.Count <= 0))
            {
                this.ShowMsgToTarget("关键字不能为空", false, "parent");
            }
            else
            {
                MenuInfo menu = new MenuInfo();
                if (this.id > 0)
                {
                    menu = VShopHelper.GetFuwuMenu(this.id);
                }
                else
                {
                    menu.ParentMenuId = this.parentid;
                    if (!VShopHelper.CanAddFuwuMenu(menu.ParentMenuId))
                    {
                        this.ShowMsgToTarget("一级菜单不能超过三个，对应二级菜单不能超过五个", false, "parent");
                        return;
                    }
                }
                int num = 0x10;
                string msg = "菜单标题不超过16个字节！";
                if (menu.ParentMenuId > 0)
                {
                    num = 14;
                    msg = "二级菜单不超过14个字节！";
                }
                string text = this.txtMenuName.Text;
                if (string.IsNullOrEmpty(text))
                {
                    this.ShowMsgToTarget("请填写菜单名称！", false, "parent");
                }
                else if (this.GetStrLen(text) > num)
                {
                    this.ShowMsgToTarget(msg, false, "parent");
                }
                else
                {
                    menu.Name = this.txtMenuName.Text;
                    menu.Type = "click";
                    if (menu.ParentMenuId == 0)
                    {
                        menu.Type = "view";
                    }
                    else if (string.IsNullOrEmpty(this.ddlType.SelectedValue) || (this.ddlType.SelectedValue == "0"))
                    {
                        this.ShowMsgToTarget("二级菜单必须绑定一个对象", false, "parent");
                        return;
                    }
                    menu.Bind = Convert.ToInt32(this.ddlType.SelectedValue);
                    BindType bindType = menu.BindType;
                    switch (bindType)
                    {
                        case BindType.Key:
                            menu.ReplyId = Convert.ToInt32(this.ddlValue.SelectedValue);
                            break;

                        case BindType.Topic:
                            menu.Content = this.ddlValue.SelectedValue;
                            break;

                        default:
                            if (bindType == BindType.Url)
                            {
                                menu.Content = this.txtUrl.Text.Trim();
                            }
                            break;
                    }
                    if (this.id > 0)
                    {
                        if (VShopHelper.UpdateFuwuMenu(menu))
                        {
                            this.DoFunction("菜单修改成功！");
                        }
                        else
                        {
                            this.ShowMsgToTarget("菜单添加失败", false, "parent");
                        }
                    }
                    else if (VShopHelper.SaveFuwuMenu(menu))
                    {
                        this.DoFunction("菜单添加成功！");
                    }
                    else
                    {
                        this.ShowMsgToTarget("菜单添加失败", false, "parent");
                    }
                }
            }
        }

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindType bindType = (BindType)Convert.ToInt32(this.ddlType.SelectedValue);
            switch (bindType)
            {
                case BindType.Key:
                case BindType.Topic:
                    this.liUrl.Visible = false;
                    this.liValue.Visible = true;
                    break;
                case BindType.Url:
                    this.liUrl.Visible = true;
                    this.liValue.Visible = false;
                    break;
                default:
                    this.liUrl.Visible = false;
                    this.liValue.Visible = false;
                    break;
            }
            if (bindType != BindType.Key)
                return;
            this.ddlValue.DataSource = (object)Enumerable.Where<ReplyInfo>((IEnumerable<ReplyInfo>)ReplyHelper.GetAllReply(), (Func<ReplyInfo, bool>)(a => !string.IsNullOrWhiteSpace(a.Keys)));
            this.ddlValue.DataTextField = "Keys";
            this.ddlValue.DataValueField = "Id";
            this.ddlValue.DataBind();
        }

        private void DoFunction(string msg)
        {
            string str = "parent.$('#myModal').modal('hide');parent.loadmenu();parent.ShowMsg('" + msg + "',true)";
            if (!this.Page.ClientScript.IsClientScriptBlockRegistered("ServerMessageScript"))
            {
                this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript", "<script language='JavaScript' defer='defer'>setTimeout(function(){" + str + "},300);</script>");
            }
        }

        private int GetStrLen(string strData)
        {
            return Encoding.GetEncoding("GB2312").GetByteCount(strData);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnAddMenu.Click += new EventHandler(this.btnAddMenu_Click);
            if (!this.Page.IsPostBack)
            {
                this.liValue.Visible = false;
                this.liUrl.Visible = false;
                MenuInfo info = new MenuInfo();
                if (this.id <= 0)
                {
                    if (this.parentid > 0)
                    {
                        MenuInfo fuwuMenu = VShopHelper.GetFuwuMenu(this.parentid);
                        if (info != null)
                        {
                            this.iNameByteWidth = 20;
                            this.lblParent.Text = fuwuMenu.Name;
                        }
                        else
                        {
                            this.ShowMsgAndReUrl("参数不正确", false, "ManageMenu.aspx", "parent");
                        }
                    }
                    else
                    {
                        this.iframeHeight -= this.oneLineHeight;
                        this.liParent.Visible = false;
                    }
                }
                else
                {
                    info = VShopHelper.GetFuwuMenu(this.id);
                    if (info == null)
                    {
                        this.ShowMsgAndReUrl("参数不正确", false, "ManageMenu.aspx", "parent");
                    }
                    else
                    {
                        this.txtMenuName.Text = info.Name;
                        if (info.ParentMenuId == 0)
                        {
                            if (VShopHelper.GetFuwuMenusByParentId(this.id).Count > 0)
                            {
                                this.liBind.Visible = false;
                                this.iframeHeight -= this.oneLineHeight;
                            }
                            this.liParent.Visible = false;
                            this.iframeHeight -= this.oneLineHeight;
                        }
                        else
                        {
                            this.iNameByteWidth = 20;
                            this.lblParent.Text = VShopHelper.GetFuwuMenu(info.ParentMenuId).Name;
                        }
                        this.ddlType.SelectedValue = Convert.ToString((int) info.BindType);
                        switch (info.BindType)
                        {
                            case BindType.Key:
                            case BindType.Topic:
                                this.liUrl.Visible = false;
                                this.liValue.Visible = true;
                                break;

                            case BindType.Url:
                                this.liUrl.Visible = true;
                                this.liValue.Visible = false;
                                break;

                            default:
                                this.liUrl.Visible = false;
                                this.liValue.Visible = false;
                                break;
                        }
                        switch (info.BindType)
                        {
                            case BindType.Key:
                                this.ddlValue.DataSource = from a in ReplyHelper.GetAllReply()
                                    where !string.IsNullOrWhiteSpace(a.Keys)
                                    select a;
                                this.ddlValue.DataTextField = "Keys";
                                this.ddlValue.DataValueField = "Id";
                                this.ddlValue.DataBind();
                                this.ddlValue.SelectedValue = info.ReplyId.ToString();
                                break;

                            case BindType.Url:
                                this.txtUrl.Text = info.Content;
                                break;
                        }
                    }
                }
                this.hdfIframeHeight.Value = this.iframeHeight.ToString();
            }
        }
    }
}

