<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SimplePage.Master" AutoEventWireup="true" CodeBehind="MemberSelect.aspx.cs" Inherits="Hidistro.UI.Web.Admin.CashBack.MemberSelect" %>

<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var preSelector = null;
        function select(thiz) {
            var userId = $(thiz).parent().parent().children().eq(0).html();
            var userName = $(thiz).parent().parent().children().eq(1).html();
            window.parent.$DialogFrame_ReturnValue = userId + "$" + userName;
            $(thiz).html("已选");
            $(thiz).parent().parent().css("background", "#C0C0C0");
            if (null == preSelector) {
                //保存上一个选择
                preSelector = thiz;
            }
            else if (preSelector == thiz) {
                $(preSelector).html("选择");
                window.parent.$DialogFrame_ReturnValue = "";
                $(preSelector).parent().parent().css("background", "#fff");
                preSelector = null;
            } else {
                $(preSelector).html("选择");
                $(preSelector).parent().parent().css("background", "#fff");
                preSelector = thiz;
            }


        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server">
        <div class="form-inline mb10">
            <div class="set-switch">
                <div class="form-inline ">
                    <div class="form-group">
                        <label class="ml10">用户名：</label>
                        <asp:TextBox ID="txtUserName" CssClass="form-control resetSize" Width="120px" runat="server" />
                    </div>
                    <div class="form-group">
                        <label for="sellshop1" class="ml10">手机号码：</label>
                        <asp:TextBox ID="txtPhone" CssClass="form-control resetSize" Width="120px" runat="server" />
                    </div>
                    <div class="form-group" style="margin-left: 15px">
                        <asp:Button ID="btnSearchButton" runat="server" CssClass="btn resetSize btn-primary" OnClick="btnSearchButton_Click" Text="搜索" />
                    </div>
                </div>
            </div>
        </div>
        <asp:Repeater ID="rptList" runat="server">
            <HeaderTemplate>
                <table class="table y3-modaltable" style="margin-bottom: 0; width: 100%">
                    <thead>
                        <tr style="background: none;">
                            <th>会员编号</th>
                            <th>会员昵称</th>
                            <th>会员名称</th>
                            <th>手机号码</th>
                            <th>会员等级</th>
                            <th>操作</th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td><%#Eval("UserId")%></td>
                    <td><%#Eval("UserName")%></td>
                    <td><%# Eval("UserBindName")%></td>
                    <td><%#Eval("CellPhone")%></td>
                    <td><%#Eval("GradeName")%></td>
                    <td><a href="javascript:void(0);" onclick='select(this);'>选择</a></td>
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
