<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/AdminNew.Master" CodeBehind="Setting.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Shop.Setting" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .lineCss{background-color:green;height:2px;width:auto; margin-left:5px; margin-right:5px;}
    </style>

     <Hi:Script ID="Script4" runat="server" Src="/admin/js/jquery.formvalidation.js" />
    <script src="/admin/js/bootstrapSwitch.js" type="text/javascript"></script>
    <link href="/admin/css/bootstrapSwitch.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {
            var dom = $("#MainHomePageBottomLink");
        })

        function setEnable(obj, savetype) {
            var ob = $("#" + obj.id);
            var cls = ob.attr("class");
            var enable = "false";
            if (cls == "switch-btn") {
                ob.empty();
                ob.append("已关闭 <i></i>");
                ob.removeClass();
                ob.addClass("switch-btn off");
                enable = "false";
            }
            else {
                ob.empty();
                ob.append("已开启 <i></i>");
                ob.removeClass();
                ob.addClass("switch-btn");
                enable = "true";
            }
            var operName = "底部文字链接";
            switch (savetype) {
                case "EnableHomePageBottomCopyright":
                    operName = "底部版权信息";
                    break;
                case "IsHomeShowFloatMenu":
                    operName = "首页浮动导航";
                    break;
                default:
                    break;
            }
            $.ajax({
                type: "post",
                url: "Setting.aspx",
                data: { type: savetype, enable: enable },
                dataType: "text",
                success: function (data) {
                    if (enable == 'true') {
                        msg(operName + '已开启！');
                        if (savetype == "EnabeHomePageBottomLink") {
                            $("#MainHomePageBottomLink").show();
                            //清除id为fenxiaoName,fenxiaoAddress隐藏控件的缓存,并为其赋上后台保存用户输入的最新值
                            Response.Cookies["ctl00_ContentPlaceHolder1_fenxiaoName"].Expires = DateTime.Now.AddSeconds(-1);
                            Response.Cookies["ctl00_ContentPlaceHolder1_fenxiaoAddress"].Expires = DateTime.Now.AddSeconds(-1);
                            var distributionLinkName = $("#ctl00_ContentPlaceHolder1_fenxiaoName").val();
                            $("#ctl00_ContentPlaceHolder1_txtName").val(distributionLinkName);
                            var distributionLink = $("#ctl00_ContentPlaceHolder1_fenxiaoAddress").val();
                            $("#ctl00_ContentPlaceHolder1_txtDistributionLink").val(distributionLink);
                        } else {
                            $("#MainHomePageBottomCopyright").show();
                            //清除id为fenxiaoCopyright,fenxiaoCopyLink隐藏控件的缓存,并为其赋上后台保存用户输入的最新值
                            Response.Cookies["ctl00_ContentPlaceHolder1_fenxiaoCopyright"].Expires = DateTime.Now.AddSeconds(-1);
                            Response.Cookies["ctl00_ContentPlaceHolder1_fenxiaoCopyLink"].Expires = DateTime.Now.AddSeconds(-1);
                            var distributionCopyright = $("#ctl00_ContentPlaceHolder1_fenxiaoCopyright").val();
                            $("#ctl00_ContentPlaceHolder1_txtCopyName").val(distributionCopyright);
                            var distributionCopyLink = $("#ctl00_ContentPlaceHolder1_fenxiaoCopyLink").val();
                            $("#ctl00_ContentPlaceHolder1_txtCopyLink").val(distributionCopyLink);
                        }
                    }
                    else {
                        msg(operName + '已关闭！');
                        if (savetype == "EnabeHomePageBottomLink") {
                            $("#MainHomePageBottomLink").hide();
                        } else if(savetype=="EnableHomePageBottomCopyright"){
                            $("#MainHomePageBottomCopyright").hide();
                        }
                    }
                }
            });
        }

        function setMsg(savetype) {
            var operName = "分销商申请栏目";
            if (savetype === "CopyrightLink") {
                operName = "版权信息";
            }
            var txt1 = $("#ctl00_ContentPlaceHolder1_txtName").val();
            var txt2 = $("#ctl00_ContentPlaceHolder1_txtDistributionLink").val();
            var txt3 = $("#ctl00_ContentPlaceHolder1_txtCopyName").val();
            var txt4 = $("#ctl00_ContentPlaceHolder1_txtCopyLink").val();

                txt4 = (txt4.substr(0, 7).toLowerCase() == "http://" || txt4 == "") || txt4.substr(0, 8).toLowerCase()== "https://" ? txt4 : "http://" + txt4;
                $("#ctl00_ContentPlaceHolder1_txtCopyLink").val(txt4);
                $.ajax({
                    type: "post",
                    url: "Setting.aspx",
                    data: { type: savetype, txt1: txt1, txt2: txt2, txt3: txt3, txt4: txt4 },
                    dataType: "text",
                    success: function (data) {
                        msg(operName + '已保存！');
                    }
                });
            }

        function msg(msg) {
            HiTipsShow(msg, 'success');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">
        <div class="page-header">
            <h2>店铺设置</h2>
        </div>
        
            <div class="tab-content">
                <div role="tabpanel" class="tab-pane active" id="profile">
                    <!--表单-->
            <div class="set-switch" style="margin-top:5px;border: 1px solid #ccc;">
                <strong>开启首页快捷菜单</strong>
                <p>开启后，首页左侧会显示快捷菜单</p>
                <div id="IsHomeShowFloatMenu" class= "<%=_IsHomeShowFloatMenu?"switch-btn":"switch-btn off" %>" onclick="setEnable(this,'IsHomeShowFloatMenu')">
                    <%=_IsHomeShowFloatMenu?"已开启":"已关闭"%>
                    <i></i>
                </div>
            </div>
                    </div>
                </div>

        <div>
        <div>
            <input type="text" id="fenxiaoName" hidden="hidden" runat="server" />
            <input type="text" id="fenxiaoAddress" hidden="hidden" runat="server" />
            <div class="set-switch" style="margin-top:5px; border: 1px solid #ccc;">
                <strong>底部文字链接</strong>
                <p>关闭后，底部将不显示文字链接</p>
                <div id="EnabeHomePageBottomLink" class="<%=_EnabeHomePageBottomLink?"switch-btn":"switch-btn off" %>" onclick="setEnable(this,'EnabeHomePageBottomLink')">
                    <%=_EnabeHomePageBottomLink?"已开启":"已关闭"%>
                    <i></i>
                </div>
                
             <div id="MainHomePageBottomLink" style="<%=_EnabeHomePageBottomLink?"": "display:none" %>">
                <div class="form-group" style="margin-top:10px;">
                <label class="col-xs-2 control-label">分销申请栏目名称：</label>
                <div class="col-xs-4" style="width:13%;">
                    <asp:TextBox ID="txtName" CssClass="form-control" runat="server"  placeholder="申请分销" MaxLength="9" />
                </div>
            </div>
            <div class="form-group">
                <label class="col-xs-2 control-label">栏目链接地址：</label>
                <div class="col-xs-4">
                    <asp:TextBox ID="txtDistributionLink" CssClass="form-control" runat="server" placeholder="栏目链接地址" />
                </div>
            </div>
                <div class="form-group"> 
                    <div class="col-xs-4 control-label" style="padding-right: 13px;">
                       <%-- <asp:Button runat="server" class="btn btn-success bigsize" Text="保存" OnClick="SetDistributionLink" />--%>
                        <span class="btn btn-success bigsize" onclick="setMsg('DistributionLink')">保存</span>
                        <asp:Button ID="btnResetHomePageBottomLink" runat="server" Text="恢复默认" CssClass="btnLink ml15" OnClick="btnResetHomePageBottomLink_Click" OnClientClick="return HiConform('<strong></strong><p>确定要将底部文字链接恢复默认吗？</p>',this)" />
                    </div>
                </div>
       </div>

            </div>
        </div>
            
            <div>
            <input type="text" id="fenxiaoCopyright" hidden="hidden" runat="server" />
            <input type="text" id="fenxiaoCopyLink" hidden="hidden" runat="server" />
            <div class="set-switch" style="margin-top:5px;border: 1px solid #ccc;">
                <strong>底部版权信息</strong>
                <p>关闭后，底部将不显示版权信息文字</p>
                <div id="EnableHomePageBottomCopyright" class="<%=_EnableHomePageBottomCopyright?"switch-btn":"switch-btn off" %>" onclick="setEnable(this,'EnableHomePageBottomCopyright')">
                    <%=_EnableHomePageBottomCopyright?"已开启":"已关闭"%>
                    <i></i>
                </div>
                 <div id="MainHomePageBottomCopyright" style="<%=_EnableHomePageBottomCopyright?"": "display:none" %>">
                <div class="form-group" style="margin-top:10px;">
                <label class="col-xs-2 control-label">版本信息文字：</label>
                <div class="col-xs-4">
                    <asp:TextBox ID="txtCopyName" CssClass="form-control" runat="server"  placeholder="Hishop技术支持" />
                </div>
            </div>
            <div class="form-group">
                <label class="col-xs-2 control-label">文字对应跳转链接：</label>
                <div class="col-xs-4">
                    <asp:TextBox ID="txtCopyLink" CssClass="form-control" runat="server" placeholder="由http://开头，为空则不进行跳转" />
                </div>
            </div>
                <div class="form-group"> 
                    <div class="col-xs-4 control-label" style="padding-right: 13px;">
                        <span class="btn btn-success bigsize" onclick="setMsg('CopyrightLink')">保存</span>
                        <asp:Button ID="Button1" runat="server" Text="恢复默认" CssClass="btnLink ml15" OnClick="Button1_Click" OnClientClick="return HiConform('<strong></strong><p>确定要将底部版权信息恢复默认吗？</p>',this)"/>
                       <%-- <asp:Button runat="server" class="btn btn-success bigsize" Text="保存" />--%>
                    </div>
                </div>
                     </div>
            </div>
        </div>
    </div>
        <script>
            InitTextCounter(7, "#ctl00_ContentPlaceHolder1_txtName", null);</script>
  </form>
</asp:Content>
