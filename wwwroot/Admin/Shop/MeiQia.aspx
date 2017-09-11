<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="MeiQia.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Shop.MeiQia" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../css/bootstrapSwitch.css" />
    <script type="text/javascript" src="../js/bootstrapSwitch.js"></script>
    <style type="text/css">
        #ctl00_ContentPlaceHolder1_OpenAccount {
            margin-left: 16px;
        }

        #ctl00_ContentPlaceHolder1_ChangePwd {
            margin-left: 16px;
        }

        #custom tbody td {
            vertical-align: middle;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            $('#mySwitch').on('switch-change', function (e, data) {
                var type = "0";
                var enable = data.value;
                $.ajax({
                    type: "post",
                    url: "ShopConfigHandler.ashx",
                    data: { type: type, enable: enable },
                    dataType: "json",
                    success: function (data) {
                        if (data.type == "success") {
                            if (enable == true) {
                                $('.maind').css('display', '');
                            }
                            else {
                                $('.maind').css('display', 'none');
                            }
                        }
                        else {
                            ShowMsg("修改失败（" + data.data + ")");
                        }
                    }
                });
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-header">
        <h2>美洽新接口配置</h2>
    </div>
    <form runat="server" id="thisForm" class="form-horizontal">
        <div class="tab-content">
            <div role="tabpanel" class="exitshopinfo" style="background-color: #fff;" id="setting">
                <div class="form-group">
                    <label class="col-xs-2 control-label">开启在线客服：</label>
                    <div class="col-xs-4">
                        <div class="switch" id="mySwitch">
                            <input type="checkbox" <%=enable ? "checked" : ""%> />
                        </div>
                    </div>
                </div>
                <div class="maind" style="<%=enable ? "": "display:none" %>">
                    <div class="form-group">
                        <label for="ctl00_ContentPlaceHolder1_txtKey" class="col-xs-2 control-label">企业ID：</label>
                        <div class="col-xs-10">
                            <asp:TextBox ID="txtKey" runat="server" CssClass="form-control resetSize inputw350" MaxLength="10" Style="display: inline-block;" Text=""></asp:TextBox>
                            <a href="https://app.meiqia.com/signup" style="margin: inherit" target="_blank">去美洽注册帐号</a>
                            <p>提供企业ID获取最佳美洽客服使用体验. <a href="../help/meiqia.html" target="_blank">如何获取企业ID &gt;&gt;</a> </p>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-xs-offset-2 col-xs-10 marginl">
                            <asp:Button runat="server" ID="btnSave" class="btn btn-success inputw100" Text="保存" OnClick="btnSave_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script>$("#ctl00_ContentPlaceHolder1_txtKey").keyup(function () {
    var temp = $(this).val();
    if ('' != temp.replace(/\d{1,}/, '')) {
        temp = temp.match(/\d{1,}/) == null ? '' : temp.match(/\d{1,}/);
    }
    $(this).val(temp);
})</script>
</asp:Content>
