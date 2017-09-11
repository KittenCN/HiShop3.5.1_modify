<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="BalancePayment.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Settings.BalancePayment" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .lineCss{background-color:green;height:2px;width:auto; margin-left:5px; margin-right:5px;}
    </style>
    <script type="text/javascript">
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
            var operName = "账户余额支付";
            if (savetype === "EnabelBalanceWithdrawal") {
                operName = "账户余额申请提现";
            }
            $.ajax({
                type: "post",
                url: "BalancePayment.aspx",
                data: { type: savetype, enable: enable },
                dataType: "text",
                success: function (data) {
                    if (enable == 'true') {
                        msg(operName + '已开启！');
                    }
                    else {
                        msg(operName + '已关闭！');
                    }
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
            <h2>设置收款账号</h2>
        </div>
        <div>
        <ul class="nav nav-tabs" role="tablist">
            <li role="presentation" class="active"><a href="BalancePayment.aspx">账户余额</a></li>
            <li role="presentation"><a href="WeixinPay.aspx">微信支付</a></li>
            <li role="presentation"><a href="Alipay.aspx">支付宝</a></li>
            <%--<li role="presentation" ><a href="ChinaBank.aspx">网银在线</a></li>--%>
            <li role="presentation"><a href="ShengPay.aspx">盛付通</a></li>
            <li role="presentation"><a href="OfflinePay.aspx">线下支付</a></li>
            <li role="presentation"><a href="COD.aspx">货到付款</a></li>
        </ul>
        <div>
            <div class="set-switch" style="margin-top:5px;">
                <strong>账户余额支付</strong>
                <p>开通账户余额支付，所有会员在店铺的消费可以直接使用余额付款</p>
                <div id="EnableBalancePayment" class="<%=_EnableBalancePayment?"switch-btn":"switch-btn off" %>" onclick="setEnable(this,'EnableBalancePayment')">
                    <%=_EnableBalancePayment?"已开启":"已关闭"%>
                    <i></i>
                </div>
            </div>
        </div>
            
            <div>
            <div class="set-switch" style="margin-top:5px;">
                <strong>账户余额申请提现</strong>
                <p>关闭账户余额申请提现功能，会员账户内的余额只能用于店铺消费</p>
                <div id="EnabelBalanceWithdrawal" class="<%=_EnabelBalanceWithdrawal?"switch-btn":"switch-btn off" %>" onclick="setEnable(this,'EnabelBalanceWithdrawal')">
                    <%=_EnabelBalanceWithdrawal?"已开启":"已关闭"%>
                    <i></i>
                </div>
            </div>
        </div>
    </div>
  </form>
</asp:Content>
