<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/AdminNew.Master" CodeBehind="CustomPageManage.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Shop.CustomPageManage" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">
        <div class="page-header">
            <h2>自定义页面</h2>
             <p class="color9">编辑现有的店铺主页，或新建一个自定义页面，可以使用模板或根据自己的要求，制作适应各种场景的个性化页面</p>
        </div>
        <div class="mb10">
            <a class="btn btn-primary btn-sm" href="/admin/shop/SelectTemplate.aspx">创建新页面</a>
           
        </div>

        <div class="set-switch clearfix">
            <div class="dp_logo fl mr15">
                <img src="<%= GetImgName(tempLatePath) %>" />
            </div>
            <div class="dp_info pt20">
                <h2><%= templateCuName %></h2>
                <p><%= showUrl %></p>
            </div>
            <div class="dp_link">
                <a class="btn btn-default btn-sm mr5" title="点击编辑首页模版" href="<%=   GetTempUrl(tempLatePath) %>">编辑</a>
                <a class="btn btn-default btn-sm mr5" title="点击浏览首页" target="_blank" href="<%= showUrl %>">链接</a>
                <a class="btn btn-default btn-sm mr5" title="点击显示首页二维码" href="#divqrcode" data-toggle="modal" data-target="#divqrcode">二维码</a>
            </div>
        </div>
        <div class="play-tabs">
            <ul class="nav nav-tabs" role="tablist">
                <li role="presentation" class="active presentation"><a href="/admin/shop/CustomPageManage.aspx?status=0" aria-controls="shopclass">店铺页面</a></li>
                <li role="presentation" class="presentation"><a href="/admin/shop/CustomPageManage.aspx?status=1" aria-controls="exitshopinfo">草稿箱</a></li>
            </ul>
            <div class="tab-content">
                <div role="tabpanel" class="tab-pane active" id="shopclass">

                    <table class="table text-center">
                        <asp:Repeater ID="Repeater1" runat="server">
                            <HeaderTemplate>
                                <thead>
                                    <tr>
                                     
                                        <th width="15%">页面名称</th>
                                        <th width="51%" class="text-center">页面地址</th>
                                     

                                        <th width="12%" class="text-center">浏览量</th>
                                        <th width="22%" class="text-center">操作</th>

                                    </tr>
                                </thead>
                            </HeaderTemplate>

                            <ItemTemplate>
                                <tr>
                                 
                                    <td style="text-align: left;">
                                        <p><a href="<%#  Convert.ToInt32( Eval("Status"))==0? GetPageUrl(Eval("PageUrl").ToString()):GetDraftPageUrl(Eval("DraftPageUrl").ToString())  %> " target="_blank"><%# Convert.ToInt32( Eval("Status"))==0? Eval("Name"):Eval("DraftName")  %></a></p>
                                    </td>
                                    <td class="text-left"> <%# Convert.ToInt32( Eval("Status"))==0? GetPageUrl(Eval("PageUrl").ToString()):GetDraftPageUrl(Eval("DraftPageUrl").ToString())  %></td>
                              

                                    <td><%# Eval("PV") %></td>

                                    <td width="22%">
                                        <input type="text" id='urldata<%# Eval("Id") %>' placeholder="" name='urldata<%#  Eval("Id") %>' value='<%# GetPageUrl(Eval("PageUrl").ToString()) %>' disabled="" style="display: none">
                                         <a onclick="winitemqrcode('<%# Convert.ToInt32( Eval("Status"))==0? GetPageUrl(Eval("PageUrl").ToString()):GetDraftPageUrl(Eval("DraftPageUrl").ToString())  %>','<%# Convert.ToInt32( Eval("Status"))==0? Eval("Name"):Eval("DraftName")  %>');" style='display: <%# Convert.ToInt32( Eval("Status"))==0?"":"none;"  %>' href="javascript:void(0)" title="点击查看页面二维码" class="mr20">二维码</a>
                                           
                                        <a class="mr20 copyurl" id="copy<%# Eval("Id") %>" href="javascript:void(0)" style='display: <%# Convert.ToInt32( Eval("Status"))==0?"":"none;"  %>' title="点击复制页面链接" data-clipboard-target="urldata<%# Eval("Id") %>">复制</a>
                                        <a class="mr20" href="/admin/shop/CustomPageEdit.aspx?id=<%# Eval("ID") %>">编辑</a>
                                        <a href="javascript:void(0)" class="deletePage" data-value="<%# Eval("Id") %>">删除</a>
                                    </td>
                                </tr>
                            </ItemTemplate>


                        </asp:Repeater>
                    </table>
                    <div class="select-page clearfix">
                        <div class="form-horizontal fl">
                        </div>
                        <div class="page fr">
                            <div class="pageNumber">
                                <div class="pagination" style="margin: 0px">
                                    <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <br />


            </div>
        </div>
        <div class="modal fade" id="divqrcode">
            <div class="modal-dialog" style="width: 300px; height: 300px;">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">店铺二维码</h4>
                    </div>
                    <div class="modal-body" style="text-align: center">
                        <image id="imagecode" src=""></image>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <div class="modal fade" id="divitemqrcode">
            <div class="modal-dialog" style="width: 300px; height: 300px;">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title" id="itemtitle">店铺二维码</h4>
                    </div>
                    <div class="modal-body" style="text-align: center">
                        <image id="imageitemcode" src=""></image>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
    </form>
    <script src="../js/ZeroClipboard.min.js"></script>
    <script>

        function init() {
            $(".copyurl").each(function () {
                var id = $(this).attr("id");
                var copy = new ZeroClipboard(document.getElementById(id), {
                    moviePath: "../js/ZeroClipboard.swf"
                });
                copy.on('complete', function (client, args) {
                    HiTipsShow("复制成功，复制内容为：" + args.text, 'success');
                });
            })

        }


        $(function () {
            init();
            var status = "<%= status%>";
            $("#imagecode").attr('src', "http://s.jiathis.com/qrcode.php?url=<%= showUrl%>");
            $(".presentation").removeClass("active");

            if (status == "1") {
                $(".presentation").eq(1).addClass("active");
            } else {
                $(".presentation").eq(0).addClass("active");
            }
            binddel();
        })
        function winitemqrcode(url,title) {
            $("#imageitemcode").attr('src', "http://s.jiathis.com/qrcode.php?url=" + url);
            $("#itemtitle").html(title+"二维码");
            $('#divitemqrcode').modal('toggle').children().css({
                width: '300px',
                height: '300px'
            });
            $("#divitemqrcode").modal({ show: true });
        }
        function binddel() {

            $(".deletePage").click(function () {
                var id = $(this).attr("data-value");
                ;
                HiTipsShow("确定删除该页面?", "confirmII", function () {
                    var resuft = CustomPageManage.DeleteCustomPage(id).value;

                    if (resuft) {
                        HiTipsShow("删除成功", "success", function () { location.reload(false) }, null);
                    } else {
                        HiTipsShow("删除失败", "error");
                    }
                })
            });
        }

    </script>
</asp:Content>
