<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="ManageCashBack.aspx.cs" Inherits="Hidistro.UI.Web.Admin.CashBack.ManageCashBack" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        /*.selectthis {border-color:red; color:red; border:1px solid;}*/
        .tdClass {
            text-align: center;
        }

        .labelClass {
            margin-right: 10px;
        }

        .thCss {
            text-align: center;
        }

        .selectthis {
            border: 1px solid;
            border-color: #999999;
            color: #c93027;
            margin-right: 2px;
        }

            .selectthis:hover {
                border: 1px solid;
                border-color: #999999;
                color: #c93027;
                margin-right: 2px;
            }

        .aClass {
            border: 1px solid;
            border-color: #999999;
            color: #999999;
            margin-right: 2px;
        }

            .aClass:hover {
                border: 1px solid;
                border-color: #999999;
                color: #999999;
                margin-right: 2px;
            }

        #datalist td {
            word-break: break-all;
        }

        #ctl00_ContentPlaceHolder1_grdMemberList th {
            margin: 0px;
            border-left: 0px;
            border-right: 0px;
            background-color: #F7F7F7;
            text-align: center;
            vertical-align: middle;
        }

        #ctl00_ContentPlaceHolder1_grdMemberList td {
            margin: 0px;
            border-left: 0px;
            border-right: 0px;
            text-align: center;
            vertical-align: middle;
        }

        .table-bordered > thead > tr > th {
            border: none;
        }
    </style>
    <script type="text/javascript">

        function showCashBackDetails(id) {
            debugger;
            DialogFrame("ManageCashBackDetails.aspx?CashBackId=" + id, "返现明细", 600, 400, function () { });
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">
        <div class="page-header">
            <h2>充值返现管理</h2>
        </div>
        <!--搜索-->

        <!--数据列表区域-->
        <div>
            <div class="form-inline mb10">
                <div class="set-switch">
                    <div class="form-inline  mb10">
                        <div class="form-group mr20" style="margin-left: 0px;">
                            <label for="sellshop1" class="ml10">用户名：</label>
                            <asp:TextBox ID="txtUserName" CssClass="form-control resetSize" runat="server" />
                        </div>
                        <div class="form-group">
                            <label for="sellshop1" class="ml10">手机号码：</label>
                            <asp:TextBox ID="txtPhone" CssClass="form-control resetSize" Width="186px" runat="server" />
                        </div>
                        <div class="form-group mr20" style="margin-left: 30px;">
                            <label for="txtStoreName">返现类型：</label>
                            <Hi:CashBackTypesDropDownList ID="dropCashBackTypes" CssClass="form-control inputw150" runat="server" />
                        </div>
                        <div class="form-group" style="margin-left: 15px">
                            <asp:Button ID="btnSearchButton" runat="server" OnClick="btnSearchButton_Click" CssClass="btn resetSize btn-primary" Text="搜索" />
                        </div>
                    </div>

                </div>
            </div>

            <div class="title-table">
                <div style="margin-bottom: 5px; margin-top: 10px;">
                    <div class="form-inline" id="pagesizeDiv" style="float: left; width: 100%; margin-bottom: 5px;">
                    </div>
                    <div class="page-box">
                        <div class="page fr">
                            <div class="form-group" style="margin-right: 0px; margin-left: 0px; background: #fff;">
                                <label for="exampleInputName2">每页显示数量：</label>
                                <UI:PageSize runat="server" ID="hrefPageSize" />
                            </div>
                        </div>
                    </div>
                    <div class="pageNumber" style="float: right; height: 29px; margin-bottom: 5px; display: none;">
                        <label>每页显示数量：</label>
                        <div class="pagination" style="display: none;">
                            <UI:Pager runat="server" ShowTotalPages="false" ID="pager" />
                        </div>
                    </div>

                    <!--结束-->
                </div>
                <table class="table table-hover mar table-bordered" style="border-bottom: none; display: none;">
                    <thead>
                        <tr>
                            <th style="text-align: center; width: 110px">用户名</th>
                            <th style="text-align: center; width: 112px">手机</th>
                            <th style="text-align: center; width: 106px">充值金额</th>
                            <th style="text-align: center; width: 99px">已返金额</th>
                            <th style="text-align: center; width: 99px">返现类型</th>
                            <th style="text-align: center; width: 99px">状态</th>
                            <th style="text-align: center; width: 99px">是否完成</th>
                            <th style="text-align: center; width: 99px">开始日期</th>
                            <th style="text-align: center; width: 99px">&nbsp;</th>
                        </tr>
                    </thead>
                </table>
            </div>
            <div id="datalist">
                <UI:Grid ID="grdMemberList" runat="server" ShowHeader="true" AutoGenerateColumns="false"
                    DataKeyNames="UserId" HeaderStyle-CssClass="table_title" CssClass="table table-hover mar table-bordered"
                    GridLines="None" Width="100%">
                    <Columns>
                        <asp:TemplateField HeaderText="用户名" ShowHeader="true" ItemStyle-Width="110px">
                            <ItemTemplate>
                                <%# Eval("UserName")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="手机" ItemStyle-Width="112px" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <p><%# Eval("CellPhone").ToString()==""?"<span style='color:gray'>未绑定手机</span>":Eval("CellPhone") %></p>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="充值金额" ItemStyle-Width="106" ShowHeader="true">
                            <ItemTemplate>
                                <%# Eval("RechargeAmount","{0:F2}") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="已返金额" ItemStyle-Width="99">
                            <ItemTemplate>
                                <%# Eval("CashBackAmount","{0:F2}") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="返现比例" ItemStyle-Width="99">
                            <ItemTemplate>
                                <%# Eval("Percentage","{0:F2}") %>%
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="返现类型" ItemStyle-Width="99">
                            <ItemTemplate>
                                <%# ((Hidistro.Entities.CashBack.CashBackTypes) Eval("CashBackType")).ToString() %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="状态" ItemStyle-Width="99">
                            <ItemTemplate>
                                <%# ((bool) Eval("IsValid")?"有效":"失效") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="是否完成" ItemStyle-Width="99">
                            <ItemTemplate>
                                <%# ((bool) Eval("IsFinished")?"已完成":"返现中") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="开始日期" ItemStyle-Width="99">
                            <ItemTemplate>
                                <%#  Eval("CreateDate","{0:yyyy-MM-dd}") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="操作" ItemStyle-Width="99">
                            <ItemTemplate>
                                <%# ((bool) Eval("IsFinished"))?"":"<a href='AddCashBack.aspx?CashBackId=" + Eval("CashBackId") + "'>修改</a>" %>
                                <a href="javascript:void(0);" onclick='javascript:showCashBackDetails(<%#Eval("CashBackId")%>);'>明细</a>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </UI:Grid>

            </div>
        </div>
        <!--数据列表底部功能区域-->
        <div class="bottomPageNumber clearfix">
            <div class="pageNumber">
                <div class="pagination" style="width: auto">
                    <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
                </div>
            </div>
        </div>
    </form>
</asp:Content>
