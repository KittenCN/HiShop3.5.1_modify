<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="AddCashBack.aspx.cs" Inherits="Hidistro.UI.Web.Admin.CashBack.AddCashBack" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <Hi:Script ID="Script4" runat="server" Src="/admin/js/jquery.formvalidation.js" />
    <script src="/admin/js/bootstrapSwitch.js" type="text/javascript"></script>
    <link href="/admin/css/bootstrapSwitch.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        var selUserId = 0, selUserName = "";
        $(document).ready(function () {
            //page load 
            $("#<%=txtUserName.ClientID%>").dblclick(function () {
                DialogFrame("MemberSelect.aspx", "请选择会员", 600, 400, function (k, v) {
                    //取到返回的数据
                    if (k == "OK") {
                        //alert($DialogFrame_ReturnValue);
                        selUserId = v.split('$')[0];
                        selUserName = v.split('$')[1];
                        $("#<%=txtUserName.ClientID%>").val(selUserName).focus(); 
                        $("#<%=txtUserId.ClientID%>").val(selUserId);
                    }
                }, true);
            });
            $('#aspnetForm').formvalidation({
                'ctl00$ContentPlaceHolder1$txtUserName': {
                    validators: {
                        notEmpty: {
                            message: "请双击选择会员"
                        }
                    }
                },
                'ctl00$ContentPlaceHolder1$txtRechargeAmount': {
                    validators: {
                        notEmpty: {
                            message: "请输入充值金额"
                        },
                        regexp: {
                            regexp: /^[0-9]+\.{0,1}[0-9]{0,2}$/,
                            message: '请输入数字'
                        }
                    }
                },'ctl00$ContentPlaceHolder1$txtPercentage': {
                    validators: {
                        notEmpty: {
                            message: "请输入每期返比例"
                        },
                        regexp: {
                            regexp: /^[0-9]+\.{0,1}[0-9]{0,2}$/,
                            message: '请输入数字'
                        }
                    }
                }, 'ctl00$ContentPlaceHolder1$dropCashBackTypes': {
                validators: {
                        notEmpty: {
                            message: "请选择返现类型"
                        }
                }
            }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">
        <div class="page-header">
            <h2><%=opt%>充值返现</h2>
            <asp:HiddenField ID="hidAction" Value="ADD" runat="server" />
        </div>
        <div>
            <div class="form-group" style="margin-left: 5px; margin-bottom: 30px;">
                <div style="font-size: 16px;"><span style="color: red; margin-right: 10px; font-weight: 900;">|</span>基本信息 </div>
                <div class="splitLine"></div>
            </div>
            <div class="form-group">
                <label for="inputEmail1" class="col-xs-2 control-label"><em>*</em>会员名称：</label>
                <div class="col-xs-4">
                    <asp:HiddenField ID="txtUserId" Value="" runat="server" />
                    <asp:TextBox ID="txtUserName" ReadOnly="true" CssClass="form-control inputw150" runat="server" />
                     <small>双击选择会员.</small>
                </div>
            </div>

            <div class="form-group">
                <label for="inputEmail1" class="col-xs-2 control-label"><em>*</em>返现类型：</label>
                <div class="col-xs-4">
                    <Hi:CashBackTypesDropDownList ID="dropCashBackTypes" CssClass="form-control inputw150" runat="server" />
                </div>
            </div>

            <div class="form-group">
                <label for="inputEmail1" class="col-xs-2 control-label"><em>*</em>充值金额：</label>
                <div class="col-xs-4">
                    <asp:TextBox ID="txtRechargeAmount" CssClass="form-control inputw150 inl" runat="server" />&nbsp;元
                    <br />
                    <small>输入需要充值的金额.</small>
                </div>
            </div>
            <div class="form-group">
                <label for="inputEmail1" class="col-xs-2 control-label"><em>*</em>每期返比例：</label>
                <div class="col-xs-4">
                    充值金额×<asp:TextBox ID="txtPercentage" CssClass="form-control inl ml5" Width="90px" runat="server" />
                    %
                <br />
                    <small>输入1表示返1%.</small>
                </div>
            </div>
            <div class="form-group">
                <label for="inputEmail1" class="col-xs-2 control-label"><em>*</em>是否有效：</label>
                <div id="radioDiv" class="col-xs-4">
                    <div class="switch" id="mySwitch">
                        <input type="checkbox" id="cbIsDefault" checked="checked" runat="server" />
                    </div>
                </div>
            </div>

            <div class="form-group">
                <div class="col-xs-offset-2">
                    <asp:Button runat="server" ID="btnAddCashBack" OnClick="btnAddCashBack_Click" class="btn btn-success inputw100 ml20"
                        Text="确定" />
                </div>
            </div>
        </div>
    </form>
</asp:Content>
