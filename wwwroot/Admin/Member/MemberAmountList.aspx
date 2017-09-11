<%@ Page Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="MemberAmountList.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Member.MemberAmountList" %>
<%@ Import Namespace="Hidistro.ControlPanel.Members" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register src="../Ascx/ucDateTimePicker.ascx" tagname="DateTimePicker" tagprefix="Hi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>

        $(function () {
            //合并行
            var $temrow = null;
            var tempOrderId = "";


            var $commisTb = $(".td_bg");
           
            if ($commisTb.length > 0) {
                $commisTb.each(function () {
                    var $tds=$(this).find("td");
                    var thisOrderId = $tds.eq(0).text().trim();

                    if (thisOrderId != tempOrderId) {
                        tempOrderId = thisOrderId;
                        $tds.eq(0).css("border-width", "1px 1px 0px 1px");
                        $tds.eq(1).css("border-width", "1px 1px 0px 1px");
                        $tds.eq(2).css("border-width", "1px 1px 0px 1px");
                    } else {
                        $tds.eq(0).text("").css("border-width", "0px 1px 0px 0px");
                        $tds.eq(1).text("").css("border-width", "0px 1px 0px 0px");
                        $tds.eq(2).text("").css("border-width", "0px 1px 0px 0px");
                    }

                });
            }

            $("*[id$=TradeTypeList]").change(function () {
                $("*[id$=hidType]").val($("*[id$=TradeTypeList]").val());
            });

            $("*[id$=TradeWaysList]").change(function () {
                $("*[id$=hidWays]").val($("*[id$=TradeWaysList]").val());
            });

        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
          <div class="page-header">
            <h2>收支明细</h2>
         </div>
    <form runat="server">
    <div class="set-switch">
                    <div class="form-horizontal clearfix">
                       <div class="form-inline mb10">
                            <div class="form-group">
                                <label　 for="sellshop1" style="margin-left:10px">会员用户名：</label>
                                <asp:TextBox ID="txtStoreName" CssClass="form-control resetSize inputw150" runat="server" />
                            </div>
                            <div class="form-group" style="padding-left:1px">
                                <label for="sellshop2">　流水号：</label>
                                <asp:TextBox ID="txtOrderId"  CssClass="form-control  resetSize  inputw150" runat="server"  Width="150" />
                            </div>
                           <div class="form-group" style="padding-left:1px">
                                <label for="sellshop3">　交易类型：</label>
                                <asp:DropDownList ID="TradeTypeList" runat="server" Width="107" >
                                </asp:DropDownList>
                            </div>
                            <div class="form-group" style="padding-left:1px">
                                <label for="sellshop4">　交易方式：</label>
                                <asp:DropDownList ID="TradeWaysList" runat="server" Width="107" >
                                </asp:DropDownList>
                            </div>
                           <asp:HiddenField ID="hidType" runat="server"/>
                           <asp:HiddenField ID="hidWays" runat="server"/>
                        </div>

                        <div class="form-inline  mb10">
                            <label class="col-xs-1 pad control-label resetSize" style="font-weight: 500; margin-left: 10px;" for="setdate">交易时间：</label>
                            <div class="form-inline journal-query">
                                <div class="form-group" style="padding-left:4px">
                                   <Hi:DateTimePicker CalendarType="StartDate" ID="calendarStartDate" runat="server" CssClass="form-control resetSize inputw150" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;至&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                   <Hi:DateTimePicker ID="calendarEndDate" runat="server" CalendarType="EndDate" CssClass="form-control resetSize inputw150" />&nbsp;&nbsp;&nbsp;
                                </div>
                                <asp:Button ID="btnQueryLogs" runat="server" class="btn resetSize btn-primary" Text="查询" OnClick="btnQueryLogs_Click" />&nbsp;&nbsp;
                                <div class="form-group">
                                    <label for="exampleInputName2">快速查看</label>
                                    <asp:Button ID="Button1" runat="server" class="btn resetSize btn-default" Text="最近7天" OnClick="Button1_Click1"  />
                                    <asp:Button ID="Button4" runat="server" class="btn resetSize btn-default" Text="最近一个月" OnClick="Button4_Click1" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

<div class="select-page clearfix">
    <div class="form-horizontal fl">
                       余额总额：<span style="color: red;">￥<%=Math.Round(CurrentTotal,2) %></span>
                    </div>
    <div class="form-horizontal fl" style="margin-left:50px;">
                       可用余额：<span style="color: red;">￥<%=Math.Round(AvailableTotal,2) %></span>
                    </div>
    <div class="form-horizontal fl" style="margin-left:50px;">
                       冻结金额：<span style="color: red;">￥<%=Math.Round(UnliquidatedTotal,2) %></span>
                    </div>
    </div>

    <!--数据列表-->
     <asp:Repeater ID="reCommissions"  runat="server" >
         <HeaderTemplate>
             <div>
             <table class="table table-hover mar table-bordered" style="table-layout:fixed">
                        <thead>
                            <tr>
                                <th width="120">流水号</th>
                                <th width="100">交易金额</th>
                                <th width="100">账户余额</th>
                                <th width="100">交易类型</th>  
                                <th width="100">用户名</th>
                                <th width="100">交易方式</th> 
                                <th width="120">交易时间</th>
                                <th width="120">备注</th> 
                            </tr>
                        </thead>
                        <tbody>
         </HeaderTemplate>
     <ItemTemplate>
      <tr  class="td_bg">
          <td><%# Eval("PayId")%></td>
          <td <%# (decimal)Eval("TradeAmount")>0 ? "style='color:#3bb134'" : "" %> >
              <%# (decimal)Eval("TradeAmount")>0 ? "+ ￥"+Eval("TradeAmount","{0:F2}") : "- ￥"+Eval("TradeAmount","{0:F2}").Replace("-","")%>
          </td>
          <td>￥<%# Eval("AvailableAmount","{0:F2}")%></td>
          <td><%# MemberHelper.StringToTradeType(Eval("TradeType").ToString())%></td>
          <td><a href="MembershipDetails.aspx?userId=<%# Eval("UserId")%>" style="color: #0033ff;text-decoration:underline;"><%# Eval("UserName")%></a></td>
          <td><%# MemberHelper.StringToTradeWays(Eval("TradeWays").ToString())%></td>
          <td><%# Eval("TradeTime")%></td>
          <td><%# Eval("Remark")%></td>      
           </tr>
     </ItemTemplate>
     <FooterTemplate>
         </tbody>
     </table>
     </div>
     </FooterTemplate>
     </asp:Repeater>

         <!--数据列表底部功能区域-->
  <br />
        <div class="select-page clearfix">
                   <%-- <div class="form-horizontal fl">
                       &nbsp;佣金总额：￥<%=Math.Round(CurrentTotal,2) %>
                    </div>--%>
                    <div  class="page fr">
                         <div class="pageNumber">
                        <div class="pagination" style="margin:0px">
                        <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />　
                       </div>
                      </div>
                    </div>
                </div>

        <div class="clearfix" style="height:30px"></div>
        </form>
</asp:Content>
