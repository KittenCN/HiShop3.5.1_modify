<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="GuideConcern.aspx.cs" Inherits="Hidistro.UI.Web.Admin.WeiXin.GuideConcern" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function AlertNeedEnabeGuidePage() {
            ShowMsg('请先开启引导关注');
        }
        //开启引导关注;强制关注公众号;自动弹出/跳转
        function setEnable(obj, action) {
            var ob = $("#" + obj.id);
            var cls = ob.attr("class");
            var enable = "false";
            //var guidepageEnable = "true";
            //var mustGuideConcernEnable = "true";
            if (cls == "switch-btn") {
                //关闭操作
                if (obj.id == "guidepageEnable") {
                    var objAutoBtn = $("#EnableAutoGuide");
                    objAutoBtn.unbind("click").bind("click", function () { AlertNeedEnabeGuidePage() });
                    objAutoBtn.empty().append("已关闭 <i></i>").removeClass().addClass("switch-btn off");
                }
                ob.empty();
                ob.append("已关闭 <i></i>");
                ob.removeClass();
                ob.addClass("switch-btn off");
                enable = "false";
            }
            else {
                //开启操作
                if (obj.id == "guidepageEnable") {
                    var objAutoBtn = $("#EnableAutoGuide");
                    objAutoBtn.unbind("click").bind("click", function () { setEnable(this, 'EnableAutoGuide') });
                }
                ob.empty();
                ob.append("已开启 <i></i>");
                ob.removeClass();
                ob.addClass("switch-btn");
                enable = "true";
            }
            var operName = "引导关注";
            switch (action) {
                case "EnableAutoGuide":
                    operName = "自动弹出/跳转";
                    break;
                case "MustGuideConcern":
                    operName = "强制关注公众号";
                    break;
                default:
                    break;
            }
            /*逻辑处理，“强制关注公众号”关闭后，“自动弹出/跳转”要关闭并且禁用；开启后才取消禁用*/
            $.ajax({
                type: "post",
                url: "GuideConcern.aspx",
                data: { enable: enable, action: action },
                dataType: "text",
                success: function (json) {
                    if (enable == 'true') {
                        msg(operName + '已开启！');
                    }
                    else {
                        msg(operName + '已关闭！');
                    }
                }
            });
        }
        //禁用关注方式按钮和禁用自动弹出/跳转
        function EnableAutoGuide() {
            //禁用关注方式按钮
            $("#ConcernType").hide();
            //禁用自动弹出/跳转
            var ob = $("#EnableAutoGuide");
            ob.append("已关闭 <i></i>");
            ob.removeClass();
            ob.addClass("switch-btn off");
            var enable = "true";
            $.ajax({
                type: "post",
                url: "GuideConcern.aspx",
                data: { enable: enable, action: "EnableAutoGuide" },
                dataType: "text",
                success: function (json) {
                    $("#MustGuideConcern").hide();
                }
            });
        }
        //设置关注方式
        function setMsg(action) {
            var operName = "关注方式";
            if (action === "MustGuideConcern") {
                operName = "要求强制关注";
            }
            var concernType = $('input[name="ConcernType"]:checked').val();
            var txt1 = $("#ctl00_ContentPlaceHolder1_txtConcernMsg").val();
            var txt2 = $("#ctl00_ContentPlaceHolder1_txtGuidePageSet").val();
            var mustguide = $('input[name="MustConcern"]:checked').length > 0 ? 1 : 0;
            $.ajax({
                type: "post",
                url: "GuideConcern.aspx",
                data: { action: action, txt1: txt1, txt2: txt2, concernType: concernType, mustguide: mustguide },
                dataType: "text",
                success: function (data) {
                    msg(operName + '已保存！');
                }
            });
        }
        //弹出框信息
        function msg(msg) {
            HiTipsShow(msg, 'success');
        }

        InitTextCounter(10, "#ctl00_ContentPlaceHolder1_txtConcernMsg", null);</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">
        <div class="page-header">
            <h2>引导关注</h2>
        </div>
        <div class="set-switch">
            <strong>强制关注公众号</strong>
            <p>会员必须关注公众号才能进入店铺</p>
            <div id="MustGuideConcern" class="<%=isMustcheckbox?"switch-btn":"switch-btn off" %>" onclick="setEnable(this,'MustGuideConcern')">
                <%=isMustcheckbox?"已开启":"已关闭"%><i></i>
            </div>
        </div>
        <div class="set-switch">
            <strong>开启引导关注</strong>
            <p>开启后，会引导用户关注公众号<%--<a href="../help/yijianguanzhu.html" target="_blank" class="ml20">开启指引</a>--%></p>
            <div id="guidepageEnable" class="<%=EnableGuidePageSet?"switch-btn":"switch-btn off" %>" onclick="setEnable(this,'EnabeGuidePage')">
                <%=EnableGuidePageSet?"已开启":"已关闭"%>
                <i></i>
            </div>
        </div>
        <div class="set-switch">
            <strong>自动弹出/跳转</strong>
            <p>开启后，自动弹出公众号二维码或者跳转到关注引导页面</p>
            <div id="EnableAutoGuide" class="<%=(IsAutoGuide&&EnableGuidePageSet)?"switch-btn":"switch-btn off" %>">
                <%=(IsAutoGuide&&EnableGuidePageSet)?"已开启":"已关闭"%>
                <i></i>
            </div>
        </div>
        <div class="set-switch" style="margin-top: 5px; border: 1px solid #ccc;">
            <strong>关注方式</strong>
            <p>选择"跳转到提醒关注页面"需要制作提醒关注页面</p>
            <div id="GuideConcernType">
                <div class="form-group" style="margin-top: 10px;">
                    <p style="padding-left: 15px;">
                      <label><input name="ConcernType" type="radio" value="0" <%=concernradio==0?"checked":"" %> />弹出公众号二维码</label>
                    </p>
                    <label class="col-xs-2 control-label" for="ctl00_ContentPlaceHolder1_txtConcernMsg">关注引导语：</label>
                    <div class="col-xs-4">
                        <asp:TextBox ID="txtConcernMsg" CssClass="form-control" runat="server" placeholder="长按识别二维码，获取最新优惠资讯" MaxLength="28" />
                    </div>
                </div>
                <div class="form-group">
                    <p style="padding-left: 15px;">
                        <label><input name="ConcernType" type="radio" value="1" <%=concernradio==1?"checked":"" %> />跳转至提醒关注页面</label>
                    </p>
                    <label class="col-xs-2 control-label" for="ctl00_ContentPlaceHolder1_txtGuidePageSet">页面地址：</label>
                    <div class="col-xs-4">
                        <asp:TextBox ID="txtGuidePageSet" CssClass="form-control" runat="server" placeholder="以http://开头" MaxLength="500" />
                        <small class="mt10">未关注公众号时，引导至此页面 <a href="../help/yijianguanzhu.html" target="_blank" class="fr">如何制作引导页面</a></small>
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-xs-2 control-label">
                        <span class="btn btn-success bigsize" id="ConcernType" onclick="setMsg('ConcernType')">保存</span>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script type="text/javascript">
        $("#EnableAutoGuide").bind("click", function () {
             <%if(EnableGuidePageSet){%>setEnable(this, 'EnableAutoGuide')
        <%}else{%>AlertNeedEnabeGuidePage()<%}%>
        })
    </script>
</asp:Content>
