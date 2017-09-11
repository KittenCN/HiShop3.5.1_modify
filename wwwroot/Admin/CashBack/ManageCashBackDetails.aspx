<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SimplePage.Master" AutoEventWireup="true" CodeBehind="ManageCashBackDetails.aspx.cs" Inherits="Hidistro.UI.Web.Admin.CashBack.ManageCashBackDetails" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <form id="form1" runat="server">
        <div class="form-inline mb10">
            <div class="set-switch">
                <div class="form-inline ">
                    <div class="form-group">
                        <label class="ml10">用户名：</label><asp:Literal ID="litUserName" runat="server"></asp:Literal>
                    </div>
                    <div class="form-group">
                        <label for="sellshop1" class="ml10">余额：</label>
                        <asp:Literal ID="litAmount" runat="server"></asp:Literal>
                    </div>
                    <div class="form-group">
                        <label for="sellshop1" class="ml10">积分：</label>
                        <asp:Literal ID="litPoints" runat="server"></asp:Literal>
                    </div>
                    <div class="form-group">
                        <label for="sellshop1" class="ml10">返现类型：</label>
                        <asp:Literal ID="litCashBackType" runat="server"></asp:Literal>
                    </div>
                                        <div class="form-group">
                        <label for="sellshop1" class="ml10">状态：</label>
                        <asp:Literal ID="litIsValid" runat="server"></asp:Literal>
                    </div>
                </div>

                <div class="form-inline ">
                    <div class="form-group">
                        <label class="ml10">充值金额：</label><asp:Literal ID="litRechargeAmount" runat="server"></asp:Literal>
                    </div>
                    <div class="form-group">
                        <label for="sellshop1" class="ml10">已返金额：</label>
                        <asp:Literal ID="litCashBackAmount" runat="server"></asp:Literal>
                    </div>
                    <div class="form-group">
                        <label for="sellshop1" class="ml10">返现比例：</label>
                        <asp:Literal ID="litPercentage" runat="server"></asp:Literal>
                    </div>
                    <div class="form-group">
                        <label for="sellshop1" class="ml10">是否完成：</label>
                        <asp:Literal ID="litFinished" runat="server"></asp:Literal>
                    </div>
                </div>
            </div>
        </div>
        <asp:Repeater ID="rptList" runat="server">
            <HeaderTemplate>
                <table class="table y3-modaltable" style="margin-bottom: 0; width: 100%">
                    <thead>
                        <tr style="background: none;">
                            <th>流水号</th>
                            <th>返现金额</th>
                            <th>返现日期</th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td><%#Eval("CashBackDetailsId")%></td>
                    <td><%#Eval("CashBackAmount","{0:F2}")%></td>
                    <td><%# ((DateTime) Eval("CashBackDate")).ToString("yyyy-MM-dd HH:mm:ss")%></td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody>
                    </table>
            </FooterTemplate>
        </asp:Repeater>
        <asp:Panel ID="divEmpty" runat="server" CssClass="alignc" Visible="false">无相关数据</asp:Panel>
        <div class="page" style="border-top: 1px solid #DDD;">
            <div class="bottomPageNumber clearfix">
                <div class="pageNumber">
                    <div class="pagination">
                        <UI:Pager runat="server" ShowTotalPages="true" ID="pager" DefaultPageSize="5" />
                    </div>
                </div>
            </div>
        </div>
    </form>
</asp:Content>
