<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectTemplate.aspx.cs" MasterPageFile="~/Admin/AdminNew.Master" Inherits="Hidistro.UI.Web.Admin.Shop.SelectTemplate" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">
        <div class="page-header">
            <h2>自定义页面</h2>
        </div>

        <h3 class="templateTitle">可选用的模板</h3>
        <div class="templateList">
            <ul class="clearfix">
                <asp:Repeater ID="Repeater1" runat="server">
                    <ItemTemplate>
                        <li class="<%# Eval("ThemeName").ToString ()==tempLatePath ?  "active" :""  %>">
                            <div class="img">
                                <div>
                                    <img src="<%# GetImgName(  Eval("ThemeName").ToString ()) %>">
                                </div>
                                <div class="lightBtn">
                                    <a class="btn btn-sm btn-success qiyong" data-value="<%# Eval("ThemeName") %>" href="javascript:void(0);">选择</a>
                                </div>
                                <div class="enableExit"></div>
                            </div>
                            <p class="templateUser"><%#  Eval("Name").ToString () %></p>
                        </li>

                    </ItemTemplate>
                </asp:Repeater>
            </ul>
        </div>
    </form>
    <script type="text/javascript" src="/admin/js/ZeroClipboard.min.js"></script>
    <script type="text/javascript">
        $(function () {
            $(".qiyong").click(function () {
                var tempName = $(this).attr("data-value");
                var id = SelectTemplate.CreateCustomTemplate(tempName).value;
                if (parseInt(id) > 0) {
                    window.location.href = "/admin/shop/CustomPageEdit.aspx?id=" + id;
                }
            });
            init();


            $(".btn-success").click(function () {
                if (ShopIndex.EnableTemp($(this).attr("dataID")).value) {
                    ShowMsgAndReUrl("设置成功", true, "/admin/shop/ShopIndex.aspx", null);
                }
            });

            $("#btn_show").click(function () {
                window.open("/Default.aspx");
            });
            $("#btn_edit").click(function () {
                window.location = $(this).attr("dataID");
            });

            $(".lightBtn .btn-primary").click(function () {
                window.location = $(this).attr("dataID");
            });

            $('.templateList ul li').hover(function () {
                $(this).find('.enableExit').show();
            }, function () {
                $(this).find('.enableExit').hide();
            });


        });

        var copy;
        function init() {
            copy = new ZeroClipboard(document.getElementById("btn_copy"), {
                moviePath: "/admin/js/ZeroClipboard.swf"
            });
            copy.setHandCursor(true); //设置手型  
            copy.addEventListener('mouseDown', function (client) {  //创建监听  
                copyUrl(); //设置需要复制的代码  
            });
            copy.on('complete', function (client, args) {
                HiTipsShow("复制成功，复制内容为：" + args.text, 'success');
            });
        }

        function copyUrl() {
            copy.setText("http://" + window.location.host + "/default.aspx");
        }
    </script>
</asp:Content>
