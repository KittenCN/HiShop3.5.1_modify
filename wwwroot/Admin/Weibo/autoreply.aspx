﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="autoreply.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Shop.AutoReply" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/admin/css/weibo.css">
    <script src="../js/weiboHelper.js"></script>
    <script>
        $(function () {
            $("[data-toggle='popover']").popover({ html: true });

            $("#Homepage").click(function () {
                ReceiverType = "articles";
                // $("#txtContent").val(($(this).attr("gotourl"))).keyup();
                $("#txtContent").css("display", "none");
                $("#txtContent").val('');
                $("#showdiv").css("display", "block");
                $("#showurl").attr("href", $(this).attr("gotourl"));
                $("#showgotourl").attr("href", $(this).attr("gotourl"));
                $("#showmessage").text("店铺主页");
                Url = $(this).attr("gotourl");
            });
            $('#GoodsAndType').click(function () {
                var Rand = Math.random();
                $("#MyGoodsAndTypeIframe").attr("src", "Goods.aspx?Rand=" + Rand);
                $('#myIframeModal').modal('toggle').children().css({
                    width: '800px',
                    height: '700px'
                })
                $("#myIframeModal").modal({ show: true });
            });
            $('#Picture').click(function () {
                var Rand = Math.random();
                $("#myPictureIframeModal").attr("src", "../shop/SelectArtice.aspx?Rand=" + Rand);
                $('#MyPictureIframe').modal('toggle').children().css({
                    width: '800px',
                    height: '700px'
                })
                $("#MyPictureIframe").modal({ show: true });
            });
        });
        function closeModal(modalid, txtContentid, value, title, ShortDescription, ThumbnailUrl40, articleid) {
            if (typeof (articleid) != "undefined") {
                ArticleId = articleid;
            }
            if (value.indexOf("http://") < 0)
                value = "http://" + value;
            ReceiverType = "articles";
            $("#txtContent").css("display", "none");
            $("#txtContent").val('');
            $("#showdiv").css("display", "block");
            $("#showurl").attr("href", value);
            $("#showgotourl").attr("href", value);
            $("#showmessage").text(title);
            Url = value;
            Display_name = title;
            Summary = ShortDescription;
            Image = ThumbnailUrl40;
            $('#' + modalid).modal('hide');

        }
        function eidtandaddkeys(editid, Keys, type) {
            if (type == 1) {
                $("#ctl00_ContentPlaceHolder1_hidid").val(editid);
                $("#ctl00_ContentPlaceHolder1_txtkey").val(Keys);
            }
            else {
                $("#ctl00_ContentPlaceHolder1_hidid").val('');
                $("#ctl00_ContentPlaceHolder1_txtkey").val('');
            }
            $('#myModal').modal('toggle').children().css({
                width: '400px',
                height: '100px'
            })
            $("#myModal").modal({ show: true });
        }
        function SubmitMathching() {
            var Matching = $('input:radio[name="ctl00$ContentPlaceHolder1$Matching"]:checked').val();

            var url = "&ID=" + $("#ctl00_ContentPlaceHolder1_hidid").val() + "&Matching=" + Matching;

            $.getJSON("../../API/WeiboProcess.ashx?action=reply" + url).done(function (d) {
                if (d.status == 1) {

                    $("#myMatchingModal").modal('hide');
                    location.href = "autoreply.aspx";
                }
                else {
                    HiTipsShow("提交失败！", 'fail');
                    $("#myMatchingModal").modal('hide');
                }
            });
        }
        function delreply(id) {

            if (confirm("确定删除吗?")) {
                var url = "&id=" + id;
                $.getJSON("../../API/WeiboProcess.ashx?action=replydel" + url).done(function (d) {
                    if (d.status == 1) {
                        location.href = "autoreply.aspx";
                    }
                    else {
                        HiTipsShow("删除失败！", 'fail');

                    }
                });
            }


        }
        function editreply(id) {
            editid = id;
            var url = "&id=" + id;
            $.getJSON("../../API/WeiboProcess.ashx?action=editmessage" + url).done(function (d) {
                if (d.status == 1) {
                    ReceiverType = d.ReceiverType;
                    Display_name = d.Displayname;
                    Summary = d.Summary;
                    Image = d.Image;
                    Url = d.Url;
                    ArticleId = d.ArticleId;
                    ReplyKeyId = d.ReplyKeyId;
                    if (d.ReceiverType != "text") {
                        $("#txtContent").css("display", "none");
                        $("#txtContent").val('');
                        $("#showdiv").css("display", "block");
                        $("#showurl").attr("href", Url);
                        $("#showgotourl").attr("href", Url);
                        $("#showmessage").text(Display_name);
                    }
                    else {
                        $("#txtContent").css("display", "block");
                        $("#txtContent").val('');
                        $("#showdiv").css("display", "none");
                        ReceiverType = d.ReceiverType;
                        $("#txtContent").val(d.Content);
                    }
                }
                else {
                    HiTipsShow("查询信息失败！", 'fail');

                }
            });
        }
        function txtcontentshow() {
            $("#txtContent").css("display", "block");
            $("#showdiv").css("display", "none");
        }
        function Matching(obj, id) {
            $("#ctl00_ContentPlaceHolder1_hidid").val(id);
            if (obj == "0") {

                $('input:radio:first').attr('checked', 'true');

            }
            else {
                $('input:radio:last').attr('checked', 'true');
            }
            $('#myMatchingModal').modal('toggle').children().css({
                width: '300px',
                height: '100px'
            })
            $("#myMatchingModal").modal({ show: true });
        }
        function addreply(replyKeyId) {
            cleardata();
            ReplyKeyId = replyKeyId;
            addandedittype = "add";
            $("#txtContent").val('');
            $('#myReplyModal').modal('toggle').children().css({
                width: '600px',
                height: '500px'
            })
            $("#myReplyModal").modal({ show: true });
        }
        function editreplywin(id) {
            addandedittype = "edit";
            editreply(id);
            $("#txtContent").val('');
            $('#myReplyModal').modal('toggle').children().css({
                width: '600px',
                height: '500px'
            })
            $("#myReplyModal").modal({ show: true });
        }
        var ReceiverType = "text";
        var ReplyKeyId;
        var Display_name = "";
        var Summary = "";
        var Image = "";
        var Url = "";
        var editid;
        var addandedittype = "add";
        var ArticleId = "0";
        function SubmitReply() {
            if (addandedittype == "add") {
                var Content = $("#txtContent").val();
                var url = "&ArticleId=" + ArticleId + "&Type=1&Content=" + Content + "&ReplyKeyId=" + ReplyKeyId + "&ReceiverType=" + ReceiverType + "&Display_name=" + Display_name + "&Summary=" + Summary + "&Image=" + Image + "&Url=" + Url;
                $.getJSON("../../API/WeiboProcess.ashx?action=addreply" + url).done(function (d) {
                    if (d.status == 1) {
                        location.href = "autoreply.aspx";
                    }
                    else {
                        HiTipsShow("添加失败！", 'fail');

                    }
                })
            }
            else {
                var Content = $("#txtContent").val();
                var url = "&ArticleId=" + ArticleId + "&Type=1&Content=" + Content + "&id=" + editid + "&ReplyKeyId=" + ReplyKeyId + "&ReceiverType=" + ReceiverType + "&Display_name=" + Display_name + "&Summary=" + Summary + "&Image=" + Image + "&Url=" + Url;
                $.getJSON("../../API/WeiboProcess.ashx?action=editreply" + url).done(function (d) {
                    if (d.status == 1) {
                        location.href = "autoreply.aspx";
                    }
                    else {
                        HiTipsShow("修改失败！", 'fail');

                    }
                })
            }
        }
        function showhttp() {
            $("#txthttp").val('');
            $('#myOutHttpModal').modal('toggle').children().css({
                width: '500px',
                height: '100px'
            })
            $("#myOutHttpModal").modal({ show: true });

        }
        function okhttp() {
            var content = "http://";
            if ($.trim($("#txthttp").val()) == "") {
                HiTipsShow("请输入链接地址！", 'warning');
                return;
            }

            if ($("#txthttp").val().indexOf('http://') == -1) {
                $("#txtContent").val($("#txtContent").val() + " " + content + $("#txthttp").val());
            }
            else {
                $("#txtContent").val($("#txtContent").val() + " " + $("#txthttp").val());
            }
            $("#myOutHttpModal").modal('hide');
            ReceiverType = "text";
            txtcontentshow();
        }
        function cleardata() {
            ReceiverType = "text";
            Display_name = '';
            Summary = '';
            Image = '';
            Url = '';
            ArticleId = "0";
            $("#txtContent").css("display", "block");
            $("#txtContent").val('');
            $("#showdiv").css("display", "none");
        }
        function setEnable(obj) {
            var ob = $("#" + obj.id);
            var cls = ob.attr("class");
            var enable = "false";
            if (cls == "switch-btn") {

                ob.empty();
                ob.append("已关闭 <i></i>")
                ob.removeClass();
                ob.addClass("switch-btn off");
                enable = "false";

            }
            else {
                ob.empty();
                ob.append("已开启 <i></i>")
                ob.removeClass();
                ob.addClass("switch-btn");
                enable = "true";
            }
            $.ajax({
                type: "post",
                url: "../../API/WeiboProcess.ashx",
                data: { type: "1", enable: enable, action: "setenable" },
                dataType: "text",
                success: function (data) {
                    if (enable == 'true') {
                        msg('已开启！');
                    }
                    else {
                        msg('已关闭！');
                      
                    }
                }
            });
        }
        function msg(msg) {
            HiTipsShow(msg, 'success');
        }

        function errAlert(msg) {
            HiTipsShow(msg, 'error');
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="page-header">
        <h2>私信自动回复</h2>
        <small></small>

    </div>

    <form runat="server">
        <div class="app-init-container">

            <div id="mytabl">
                <!-- Nav tabs -->
                <ul class="nav nav-tabs">
                    <li class="active"><a href="autoreply.aspx">私信自动回复</a></li>
                    <li><a href="concerned.aspx">订阅后自动回复</a></li>
                    <li><a href="atmessage.aspx">被@后自动回复</a></li>

                </ul>
                <!-- Tab panes -->
                <div class="tab-content">
                    <div class="tab-pane active">

                        <div style="position: relative; margin-bottom: 15px;">
                            <a data-popover-place="bottom" class="btn btn-success js-add-rule-group" href="javascript:;" data-toggle="popover" data-placement="bottom"
                                data-content="" data-container="body" onclick="eidtandaddkeys('','','0')">新建关键词回复</a>

                            <div id="offlineEnable" class="<%=_enable?"switch-btn":"switch-btn off" %>" style="position: absolute; top: 0px; right: 40px; width: 110px; height: 40px;" onclick="setEnable(this)">
                                  <%=_enable?"已开启":"已关闭"%>
                             <i></i>

                            </div>
                        </div>
                        <asp:Repeater ID="repreplykey" runat="server">
                            <ItemTemplate>
                                <div class="app-init">
                                    <div class="app__content setwidth">
                                        <div class="sinaweibo-autoreply clearpadd">
                                            <div id="js-rule-container" class="rule-group-container">
                                                <div class="rule-group">
                                                    <!-- <div class="rule-group"> -->
                                                    <div class="rule-meta">
                                                        <h3 class="setwidth">
                                                            <span class="rule-name"><%#Eval("Keys") %></span>
                                                            <div class="rule-opts">
                                                                <a href="javascript:;" class="js-edit-rule" onclick="eidtandaddkeys('<%#Eval("id") %>','<%#Eval("Keys") %>','1')">编辑</a>
                                                                <span>-</span>
                                                                <Hi:ImageLinkButton ID="btnFrozen" CommandName="del" CommandArgument='<%# Eval("id")%>' runat="server" Text="删除" IsShow="true"
                                                                    DeleteMsg="确定要删除吗？" />
                                                            </div>
                                                        </h3>
                                                    </div>
                                                    <div class="rule-body setbg">
                                                        <div class="rule-keywords">
                                                            <div class="rule-inner">
                                                                <h4>私信关键词：</h4>
                                                                <div class="keyword-container">
                                                                    <div class="keyword input-append">
                                                                        <!-- <div class="keyword input-append"> -->
                                                                        <a href="javascript:;" class="close--circle">×</a>
                                                                        <span class="value"><%#Eval("Keys") %></span>
                                                                        <span class="add-on" style="cursor: pointer" onclick="Matching('<%# Eval("Matching")%>','<%#Eval("id") %>');"><%#Eval("Matching").ToString()=="0"?"全匹配":"模糊" %>
		
                                                                        </span>
                                                                        <!-- </div> -->
                                                                    </div>
                                                                </div>

                                                            </div>
                                                        </div>
                                                        <div class="rule-replies">
                                                            <div class="rule-inner">
                                                                <h4>随机回复列表中的某条内容：</h4>
                                                                <ol class="reply-list">

                                                                    <asp:Repeater ID="repreply" runat="server">
                                                                        <ItemTemplate>
                                                                            <li class="clearfix">
                                                                                <div class="reply-cont">
                                                                                    <div class="reply-summary">
                                                                                        <%#Eval("Content")==null||Eval("Content")==""?Eval("Displayname"):Eval("Content") %>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="reply-opts"><a class="js-edit-it" href="javascript:;" onclick="editreplywin('<%#Eval("id") %>')">编辑</a> - <a class="js-delete-it" href="javascript:;" onclick="delreply('<%#Eval("id") %>')">删除</a></div>
                                                                            </li>
                                                                        </ItemTemplate>
                                                                    </asp:Repeater>
                                                                </ol>
                                                                <a class="js-add-reply add-reply-menu" href="javascript:;" onclick="addreply('<%#Eval("id") %>');">+ 添加一条回复</a>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <!-- </div> -->
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>

                        </asp:Repeater>
                    </div>
                    <div class="tab-pane"></div>
                    <div class="tab-pane"></div>
                </div>
            </div>


        </div>
        <div class="page">
            <div class="bottomPageNumber clearfix">
                <div class="pageNumber">
                    <div class="pagination">
                        <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
                    </div>
                </div>
            </div>
        </div>
        <!-- 模态框（Modal） -->
        <div class="modal fade" id="myModal" tabindex="-1" role="dialog"
            aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close"
                            data-dismiss="modal" aria-hidden="true">
                            &times;
                        </button>
                        <h4 class="modal-title" id="myModalLabel">关键词
                        </h4>
                    </div>
                    <div class="modal-body">
                        <form class="form-inline">
                            <div class="form-group">
                                <label for="exampleInputName2">关键词</label>
                                <input type="text" class="form-control-5" runat="server" id="txtkey" placeholder="输入关键词">
                                <input id="hidid" type="hidden" runat="server" />
                            </div>
                        </form>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default"
                            data-dismiss="modal">
                            关闭
                        </button>
                        <asp:Button ID="BtnKey" CssClass="btn btn-primary" runat="server" Text="提交" />
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal -->
        </div>

        <div class="modal fade" id="myMatchingModal" tabindex="-1" role="dialog"
            aria-labelledby="myMatchingModal" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close"
                            data-dismiss="modal" aria-hidden="true">
                            &times;
                        </button>
                        <h4 class="modal-title" id="">匹配
                        </h4>
                    </div>
                    <div class="modal-body">
                        <form class="form-inline">
                            <div class="form-group">
                                <input id="Matching1" type="radio" runat="server" name="Matching" value="0" />全匹配
                                <input runat="server" id="Matching2" value="1" type="radio" name="Matching" />模糊
                            </div>
                        </form>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default"
                            data-dismiss="modal">
                            关闭
                        </button>
                        <input id="Button1" type="button" value="提交" onclick="SubmitMathching();" class="btn btn-primary" />


                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal -->
        </div>
        <div class="modal fade" id="myReplyModal" tabindex="-1" role="dialog"
            aria-labelledby="myReplyModal" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close"
                            data-dismiss="modal" aria-hidden="true">
                            &times;
                        </button>
                        <h4 class="modal-title" id="">回复
                        </h4>
                    </div>
                    <div class="modal-body">
                        <div class="app-init-container">
                            <div class="sinaweibo-letter-wrap">


                                <div class="wb-sender">
                                    <div class="wb-sender__inner">
                                        <div class="wb-sender__input js-editor-wrap">
                                            <div class="misc top clearfix">
                                                <div class="content-actions clearfix">

                                                    <div class="editor-module insert-emotion">
                                                        <a class="js-open-emotion" data-action-type="emotion" href="javascript:;">表情</a>
                                                        <div class="emotion-wrapper">
                                                            <ul class="emotion-container clearfix">
                                                            </ul>
                                                        </div>
                                                    </div>
                                                    <div class="editor-module insert-article">
                                                        <a class="js-open-articles" id="Picture" href="javascript:;">选择图文</a>
                                                    </div>
                                                    <div class="editor-module insert-article">
                                                        <a class="js-open-articles" href="javascript:;" onclick="showhttp();">自定义外链</a>
                                                    </div>
                                                    <div class="editor-module insert-shortlink">
                                                        <div class="js-reply-menu dropup hover dropdown--right add-reply-menu">
                                                            <a href="javascript:;" class="dropdown-toggle" data-toggle="dropdown">
                                                                <span class="dropdown-current">插入链接</span>
                                                                <i class="caret"></i>
                                                            </a>
                                                            <ul class="dropdown-menu">
                                                                <li><a data-link-type="Goods" id="GoodsAndType" class="js-open-shortlink" href="javascript:;">商品及分类</a></li>

                                                                <li><a data-link-type="Homepage" id="Homepage" class="js-open-shortlink" href="javascript:;" gotourl='http://<%= Request.Url.Host+"/Default.aspx" %>'>店铺主页</a></li>
                                                            </ul>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="content-wrapper">
                                                <textarea id="txtContent" class="js-txta" cols="50" rows="4"></textarea>
                                                <div class="js-picture-container picture-container"></div>

                                                <div class="complex-backdrop" id="showdiv" style="display: none;">
                                                    <div class="js-complex-content complex-content">

                                                        <div class="ng ng-single">
                                                            <a href="javascript:;" class="close--circle js-delete-complex" onclick="cleardata();">×</a>
                                                            <div class="ng-item">
                                                                <a id="showurl" href="" target="_blank" class="new-window"><span class="label label-success" id="showmessage"></span></a>
                                                            </div>
                                                            <div class="ng-item view-more">
                                                                <a id="showgotourl" href="" target="_blank" class="clearfix new-window">
                                                                    <span class="pull-left">阅读全文</span>
                                                                    <span class="pull-right">&gt;</span>
                                                                </a>
                                                            </div>
                                                        </div>

                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default"
                            data-dismiss="modal">
                            关闭
                        </button>
                        <input type="button" value="提交" onclick="SubmitReply();" class="btn btn-primary" />


                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal -->
        </div>
        <div class="modal fade" id="myIframeModal">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">商品及分类</h4>
                    </div>
                    <div class="modal-body">
                        <iframe src="" id="MyGoodsAndTypeIframe" width="750" height="370"></iframe>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>

                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <div class="modal fade" id="MyPictureIframe">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">图文</h4>
                    </div>
                    <div class="modal-body">
                        <iframe src="" id="myPictureIframeModal" width="750" height="370"></iframe>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>

                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <div class="modal fade" id="myOutHttpModal">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">自定义外链</h4>
                    </div>
                    <div class="modal-body">
                        <form class="form-horizontal">
                            <div class="form-group">
                                <label for="title" class="col-xs-3 control-label">链接地址</label>
                                <div class="col-xs-6">
                                    <input type="text" style="width: 300px;" class="form-control" id="txthttp" placeholder="链接地址：http://example.com">
                                </div>
                            </div>

                        </form>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                        <button type="button" class="btn btn-primary" onclick="okhttp();">确定</button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>

    </form>
</asp:Content>
