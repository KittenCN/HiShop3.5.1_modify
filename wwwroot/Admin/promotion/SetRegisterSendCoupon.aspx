﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="SetRegisterSendCoupon.aspx.cs" Inherits="Hidistro.UI.Web.Admin.promotion.SetRegisterSendCoupon" %>
<%@ Register Src="~/Admin/Ascx/ucDateTimePicker.ascx" TagPrefix="uc1" TagName="ucDateTimePicker" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../css/bootstrapSwitch.css" />
    <script type="text/javascript" src="../js/bootstrapSwitch1.js"></script>
    <script>
        $(function () {
           // $("#mySwitch").click(function () {
                $('#mySwitch').on('switch-change', function (e, data) {
                if (!$(this).find("input")[0].checked) {
                    $("#signmainDiv").hide();
                    $("#txtIsEnble").val("false");
                    $.ajax({
                        type: 'get',
                        dataType: 'json',
                        url: 'GetMemberGradesHandler.ashx?action=setregisternosendcoupon',
                        success: function (data)
                        {
                            if (data.Status == "ok") {
                                HiTipsShow("保存成功！", 'success');
                            } else {
                                HiTipsShow("保存失败！", "error");
                            }
                        }
                    });
                } else {
                    $("#signmainDiv").show();
                    $("#txtIsEnble").val("true");
                }
            });
            GetCouponInfo();
            $("[id$='ddlCouponList']").change(function () { GetCouponInfo(); });
        })
        function CheckForm() {
            var time_1_str=$("#ctl00_ContentPlaceHolder1_ucDateBegin_txtDateTimePicker").val();
            var time_2_str=$("#ctl00_ContentPlaceHolder1_ucDateEnd_txtDateTimePicker").val();
            if (time_1_str == "" || time_2_str == "") {
                ShowMsg("请输入活动期限！");
                return false;
            }
            var time_1 = new Date(time_1_str.replace(/-/g, "/")).getTime();
            var time_2 = new Date(time_2_str.replace(/-/g, "/")).getTime();
            if (time_1 > time_2) {
                ShowMsg("活动的开始时间不能大于结束时间！");
                return false;
            }
            //alert(time_1 > time_2); return false;
            var couponId = $("[id$='ddlCouponList']").val();
            if (couponId > 0) {
                $.ajax({
                    type: 'get',
                    dataType: 'json',
                    data: { "action": "getcouponinfo", "id": couponId },
                    url: 'GetMemberGradesHandler.ashx',
                    success: function (data) {
                        if (data.Status == 2) {
                            var count = data.Count;
                            var beginTime = new Date(data.BeginTime.replace(/-/g, "/")).getTime();
                            var endTime = new Date(data.EndTime.replace(/-/g, "/")).getTime();
                            //alert(beginTime)
                            if (endTime < time_2) {
                                ShowMsg("活动期限截止时间不能大于优惠券过期时间！");
                                return false;
                            } else {
                                $("#ctl00_ContentPlaceHolder1_btnSave").click();
                            }
                        }
                    }
                });
            } else {
                ShowMsg("请先选择可用优惠券！");
                return false;
            }
            return false;
        }
        function GetCouponInfo()
        {
            var couponId = $("[id$='ddlCouponList']").val();
            if (couponId > 0) {
                $.ajax({
                    type: 'get',
                    dataType: 'json',
                    data: { "action": "getcouponinfo", "id": couponId },
                    url: 'GetMemberGradesHandler.ashx',
                    success: function (data) {
                        if (data.Status == 2) {
                            $("#couponCount").html('还剩 ' + data.Count + ' 张');
                            $("#couponDate").html('有效期：' + data.BeginTime + ' 到 ' + data.EndTime);
                        }
                    }
                });
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">
    <input type="hidden" name="txtIsEnble" id="txtIsEnble" value="<%=IsEnble?"true":"false" %>" />
    <div class="page-header">
        <h2>注册送优惠券设置</h2>
    </div>
    <div class="tab-content">
        <div role="tabpanel" class="tab-pane active" id="signScoreDiv">
            <div class="form-group">
                <label class="col-xs-3 pad resetSize control-label" for="ddlCouponType">&nbsp;&nbsp;开启注册送优惠券：</label>
                <div class="col-xs-9">
                <div class="switch1" id="mySwitch">
                        <input type="checkbox" <%= IsEnble?"checked=\"checked\"":"" %>  />
                    </div>
                </div>
            </div>
            <div id="signmainDiv" style="<%=IsEnble?"": "display:none" %>">
                <div class="form-group">
                    <label class="col-xs-3 pad resetSize control-label" for="ddlCouponType">&nbsp;&nbsp;送优惠券：</label>
                    <div class="form-inline  col-xs-9">
                            <asp:DropDownList ID="ddlCouponList" runat="server" Style="width: 200px;" CssClass="form-control resetSize">
                            </asp:DropDownList><a href="NewCoupon.aspx" style="margin-left:15px;">新建</a>
                        <small id="couponCount"></small>
                        <small id="couponDate"></small>
                    </div>
                </div>
                <div class="form-group">
                    <label class="col-xs-3 pad resetSize control-label" >&nbsp;&nbsp;活动期限：</label>
                     <div class="form-inline col-xs-9">
                         <uc1:ucDateTimePicker runat="server" ID="ucDateBegin" name="canTest" DateFormat="yyyy-MM-dd HH:mm:ss"
                             style="width:200px;" CssClass="form-control resetSize" />&nbsp;至&nbsp;
                         <uc1:ucDateTimePicker runat="server" ID="ucDateEnd" name="canTest" IsEnd="true" DateFormat="yyyy-MM-dd HH:mm:ss"
                             Style="width: 200px;" CssClass="form-control resetSize" />
                         <small>活动的起始期限，只有在设置的期限所在时间内活动才有效</small>
                     </div>
                </div>

                <div class="form-group" style="width: 110%;">
                    <div class="col-xs-offset-2 marginl">
                        <input type="button" onclick="return CheckForm()" value="保存" class="btn btn-success inputw100" />
                        <asp:Button runat="server" ID="btnSave"  CssClass="btn btn-success inputw100 hide" Text="保存" />
                    </div>
                </div>
            </div>
        </div>

    </div>
        </form>
</asp:Content>
