<%@ Page Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="MembershipDetails.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Member.MembershipDetails" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register Src="../Ascx/ucDateTimePicker.ascx" TagName="DateTimePicker" TagPrefix="Hi" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.ControlPanel.Store" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .yejiItem {
            text-align: center;
            line-height: 30px;
            float: left;
            margin: 5px;
            padding: 10px 50px;
            border-left: 1px solid #b9caca;
            width: 230px;
        }

            .yejiItem:first-child {
                border-left: 0px;
            }

            .yejiItem .money {
                color: #125acb;
                font-weight: bold;
                font-size: 18px;
            }

            .yejiItem .yejitxt {
                color: #444451;
                font-weight: bold;
                font-size: 18px;
            }

        .infodiv {
            float: left;
        }

        .infosdetail {
            width: 250px;
            margin-left: 50px;
            line-height: 30px;
        }

        .infostitle {
            width: 90px;
            text-align: center;
        }

        .infosdetail ul label {
            width: 90px;
            text-align: right;
            margin-right: 10px;
            font-weight: normal;
        }

        .infosdetailLong {
            width: 500px;
        }
    </style>
    <link rel="stylesheet" type="text/css" href="/admin/css/kkpager_blue.css?20160818" />
    <script src="../js/ZeroClipboard.min.js"></script>
    <script src="../js/kkpager.js?20160818"></script>
    <script>
        var totalPage = 0;
        var totalRecords = 0;
        var pageNo = 0;

        var pagesize = 5;
        $(function () {

            $("#SetUserAmount").formvalidation({
                'submit': '#btnUpdateAmount',
                'txtSetAmountBark': {
                    validators: {
                        notEmpty: {
                            message: '备注不能为空！'
                        }
                    }
                },
                'txtAmount': {
                    validators: {
                        notEmpty: {
                            message: '调整余额不能为空'
                        }
                    }
                }
            });
            //var copy = new ZeroClipboard($("#copybutton")[0], {
            //    moviePath: "../js/ZeroClipboard.swf"
            //});

            //copy.on('complete', function (client, args) {
            //    HiTipsShow("已复制:" + args.text, 'success');
            //});

            $(".infodiv").find("label").css('display', 'inline-block');

            GetTableHtml(0, 1);

            //tab切换
            $("#mytabl .table-page .nav-tabs li").click(function() {

                if ($(this).hasClass("active")) {
                    return;
                } else {
                    $(this).addClass("active").siblings().removeClass("active");
                    $("#hidstart").val("");
                    $("#hidend").val("");
                    $("#ctl00_ContentPlaceHolder1_calendarStartDate_txtDateTimePicker").val("");
                    $("#ctl00_ContentPlaceHolder1_calendarEndDate_txtDateTimePicker").val("");
                    GetTableHtml($(this).index(), 1);
                }

            });

            $("*[id$=btnSearchButton]").click(function () {
                var index = $("#mytabl .table-page .nav-tabs li[class='active']").index();
                $("#hidstart").val($("#ctl00_ContentPlaceHolder1_calendarStartDate_txtDateTimePicker").val());
                $("#hidend").val($("#ctl00_ContentPlaceHolder1_calendarEndDate_txtDateTimePicker").val());
                GetTableHtml(index, 1);
            });

        });

        //function GetData(tabIndex, pageindex) {
        //    var startTime = $("#hidstart").val();
        //    var endTime = $("#hidend").val();
        //    var pageNo = pageindex;
        //    GetTableHtml(tabIndex,  pageindex);
     
        //}


        function GetTableHtml(tabIndex, pageindex) {
            var startTime = $("#hidstart").val();
            var endTime = $("#hidend").val();
            var userid = $("#hiduserid").val();

            var orderthead = " <thead><tr><th width=\"120\">交易号</th>" +
              "<th width=\"120\">订单号</th><th width=\"100\">收货人</th>" +
              "<th width=\"100\">实付金额</th>" +
              "<th width=\"100\">支付方式</th><th width=\"120\">订单完成时间</th>" +
              "<th width=\"120\">备注</th></tr></thead>";

            var amountthead = " <thead><tr><th width=\"120\">流水号</th>" +
                "<th width=\"120\">交易金额</th><th width=\"100\">账户余额</th>" +
                "<th width=\"100\">交易类型</th><th width=\"100\">交易方式</th>" +
                "<th width=\"120\">交易时间</th>" +
                "<th width=\"120\">备注</th></tr></thead>";
            if (tabIndex === 0) {
                $(".table").html(orderthead);
            } else if (tabIndex === 1) {
                $(".table").html(amountthead);
            }
            var html = "";
            var data = "action=GetMemberAmountDetails&pagesize=" + pagesize + "&type=" + tabIndex + "&page=" + pageindex + "&startTime=" + startTime + "&endTime=" + endTime + "&userid="+userid+"&t=" + (new Date()).getTime();
            $.ajax({
                url: "/api/VshopRecharge.ashx",
                type: "post",
                data: data,
                datatype: "json",
                success: function (json) {
                    if (json.success == "false") {
                        $(".page").hide();
                        return;
                    }
                    if (json.success=="true") {
                        if (parseInt(json.total) <= 0) {
                            $(".page").hide();
                            return;
                        } else {
                            $(".page").show();
                        }
                        totalRecords = parseInt(json.total);
                        totalPage = Math.ceil(parseInt(json.total) / pagesize);
                        html = "<tbody>";
                        var jsonData = json.lihtml;
                        for (var i = 0; i < jsonData.length; i++) {

                            if (tabIndex === 0) {
                                var xhtml = "<tr class=\"td_bg\"><td>" + jsonData[i].GatewayOrderId + "</td><td>" + jsonData[i].OrderId + "</td><td>" + jsonData[i].ShipTo + "</td><td>￥" + jsonData[i].OrderTotal + "</td><td>" + jsonData[i].PaymentType + "</td><td>" + jsonData[i].OrderDate + "</td><td>" + jsonData[i].Remark + "</td></tr>";
                                html += xhtml;
                            }

                            if (tabIndex === 1) {
                                var amountShow = "";
                                var temp = parseFloat(jsonData[i].TradeAmount);
                                if (temp > 0) {
                                    amountShow = "<td style='color:#3bb134''>+ ￥" + temp.toFixed(2) + "</td>";
                                } else {
                                    amountShow = "<td>- ￥" + (temp * -1).toFixed(2) + "</td>";
                                }

                                var xhtml = "<tr class=\"td_bg\"><td>" + jsonData[i].PayId + "</td>"+amountShow+"<td>￥" + jsonData[i].AvailableAmount + "</td><td>" + jsonData[i].TradeType + "</td><td>" + jsonData[i].TradeWays + "</td><td>" + jsonData[i].TradeTime + "</td><td>" + jsonData[i].Remark + "</td></tr>";
                                html += xhtml;
                            }                                                                                  
                        }
                        html += "</tbody>";
                        $(".table").append(html);
                        //alert(totalPage);
                        //alert(totalRecords);
                        $("#kkpager").empty();
                        kkpager.total = totalPage;
                        kkpager.totalRecords = totalRecords;
                        //生成分页
                        //有些参数是可选的，比如lang，若不传有默认值
                        kkpager.generPageHtml({
                            pno: pageindex,
                            //总页码
                            total: totalPage,
                            //总数据条数
                            totalRecords: totalRecords,
                            mode: 'click',//默认值是link，可选link或者click
                            click: function (n) {
                                GetTableHtml(tabIndex,  n);
                                //alert(n);
                                // do something
                                //手动选中按钮
                                this.selectPage(n);
                                return false;
                            }
                            /*
                            ,lang				: {
                                firstPageText			: '首页',
                                firstPageTipText		: '首页',
                                lastPageText			: '尾页',
                                lastPageTipText			: '尾页',
                                prePageText				: '上一页',
                                prePageTipText			: '上一页',
                                nextPageText			: '下一页',
                                nextPageTipText			: '下一页',
                                totalPageBeforeText		: '共',
                                totalPageAfterText		: '页',
                                currPageBeforeText		: '当前第',
                                currPageAfterText		: '页',
                                totalInfoSplitStr		: '/',
                                totalRecordsBeforeText	: '共',
                                totalRecordsAfterText	: '条数据',
                                gopageBeforeText		: '&nbsp;转到',
                                gopageButtonOkText		: '确定',
                                gopageAfterText			: '页',
                                buttonTipBeforeText		: '第',
                                buttonTipAfterText		: '页'
                            }*/
                        },true);
                        //kkpager.selectPage(pageindex);

                    }
                }
            });

        }


        //修改余额
        function SetUserAmount() {
            $("#lbNowAmount").text("￥" + $("#ctl00_ContentPlaceHolder1_TotalReferral").text());
            $("#txtTitle").text("调整账户余额-" + $("#ctl00_ContentPlaceHolder1_txtUserName").text());
            $('#SetUserAmount').modal('toggle').children().css({
                width: '450px', top: "170px"
            });
            $("#txtAmount").val("");
            $("#txtSetAmountBark").val("");
            $("#btnUpdateAmount").unbind('click').bind('click', function () {
                var setAmount = $("#txtAmount").val();
                if (!parseFloat(setAmount)) {
                    ShowMsg("调整金额输入不合法", false);
                    return false;
                }
                if (parseFloat(setAmount) == 0) {
                    ShowMsg("调整余额不能为0", false);
                    return false;
                }
                var obj = this;
                $(this).attr("disabled", "disabled");

                var remark = $("#txtSetAmountBark").val();
                var userId = $("#hiduserid").val();
                if (remark.trim() === "") {
                    ShowMsg("调整备注不能为空", false);
                    return false;
                }
                if (parseFloat(setAmount) + parseFloat($("#ctl00_ContentPlaceHolder1_TotalReferral").text()) < 0) {
                    ShowMsg("减去余额不能大于当前余额!", false);
                    return false;
                }
                var tempUrl = location.href;
                var data = "action=SetUserAmountByAdmin&userid=" + userId + "&setAmount=" + setAmount + "&remark=" + remark + "&t=" + (new Date()).getTime();
                //var result = MembershipDetails.SetUserAmount(userId, setAmount, remark);
                $.ajax({
                    url: "/api/VshopRecharge.ashx",
                    type: "post",
                    data: data,
                    datatype: "json",
                    success: function(json) {
                        if (json.success == "true") {
                            //$(this).attr("disabled", false);
                            $(obj).removeAttr("disabled");
                            $('#SetUserAmount').modal('hide');
                            ShowMsgAndReUrl("调整成功", true, tempUrl, null);
                        } else {
                            //$(this).attr("disabled", false);

                            $(obj).removeAttr("disabled");
                            ShowMsg("调整失败", false);
                        }
                    }
                });

            });
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-header">
        <h2>会员详情</h2>
    </div>

    <form runat="server">

        <!--基本信息-->
        <h3 class="templateTitle">基本信息<a href="EditMember.aspx?userId=<%=userid %>" style="margin-left: 50px; color: #0f63ac; font-size: 14px;">编辑用户信息</a></h3>
        <div class="set-switch clearfix">
            <div class="infodiv">
                <div class="qrCode">
                    <Hi:HiImage ID="ListImage1" ImageUrl="/Templates/common/images/user.png" runat="server" Width="90" Height="90" />
                </div>
                <%-- <div class="infostitle">个人头像</div>--%>
            </div>

            <div class="infodiv infosdetail">
                <ul>
                    <li>
                        <label>用户名：</label><span id="txtUserName" runat="server">-</span></li>
                    <li>
                        <label>会员等级：</label><span id="txtGrade" runat="server">-</span></li>
                    <li>
                        <label>手机号码：</label><span id="txtCellPhone" runat="server">-</span></li>
                    <li>
                        <label>注册时间：</label><span id="txtCreateTime" runat="server">-</span></li>
                    <li>
                        <label>微信昵称：</label><span id="txtMicroName" runat="server">-</span></li>
                </ul>
            </div>


            <div class="infodiv infosdetail infosdetailLong">
                <ul>
                    <li>
                        <label>真实姓名：</label><span id="txtRealName" runat="server">-</span>　　　　　　　　　　　　　　　　　</li>
                    <li>
                        <label>上级店铺：</label><span id="txtRefStoreName" runat="server">-</span></li>
                    <li style="overflow: hidden">
                        <label>详细地址：</label><span id="txtAddress" runat="server" style="width: 300px; overflow-x: hidden; margin: 0px">-</span></li>
                    <li>
                        <label>QQ：</label><span id="txtQQ" runat="server">-</span></li>
                    <li>
                        <label>微信OpenId：</label><span id="txtOpenId" runat="server">-</span></li>
                </ul>
            </div>



        </div>


        <!--账户信息-->
        <h3 class="templateTitle">账户信息</h3>
        <div class="set-switch  clearfix" style="height: 120px">

            <div class="yejiItem">
                <div class="money" id="ReferralOrders" runat="server">0</div>
                <div class="yejitxt">成交订单数</div>
            </div>

            <div class="yejiItem">
                <div class="money">￥<span id="OrdersTotal" runat="server">0</span></div>
                <div class="yejitxt">总消费额</div>
            </div>

            <div class="yejiItem">
                <div class="money" style="cursor: pointer;" onclick="SetUserAmount()">￥<span id="TotalReferral" runat="server">0</span>
                    <span style="margin-left:5px;"><img src="../images/edit07.png" height="25" width="20" /></span>
                </div>
                <div class="yejitxt">账户余额</div>
            </div>

            <div class="yejiItem">
                <div class="money">￥<span id="ReferralBlance" runat="server">0</span></div>
                <div class="yejitxt">冻结金额</div>
            </div>

        </div>

        <!--数据列表-->
        <h3 class="templateTitle">消费明细</h3>
        <div id="mytabl">
            <div class="table-page">
                <ul class="nav nav-tabs">
                    <li class="active">
                        <a href="javascript:void(0);">订单消费明细</a></li>
                    <li><a href="javascript:void(0);">余额收支明细</a></li>
                </ul>
            </div>
            <div class="tab-content">
                 <div class="tab-pane active">
                    
                        <div class="form-inline mb10">
                            <div class="form-group mr20">
                                <label for="sellshop1">　交易时间：</label>
                                <Hi:DateTimePicker CalendarType="StartDate" ID="calendarStartDate" runat="server" CssClass="form-control resetSize inputw150" />
                                ~
                                <Hi:DateTimePicker ID="calendarEndDate" runat="server" CalendarType="EndDate" CssClass="form-control resetSize inputw150" />
                                <input type="hidden" id="hidstart" />
                                 <input type="hidden" id="hidend" />
                                <input type="hidden" id="hiduserid" value="<%=userid %>"/>
                                <%--<asp:Button ID="btnSearchButton" runat="server" class="btn resetSize btn-primary" Text="查询"  />--%>
                                <a href="javascript:void(0);" class="btn resetSize btn-primary" id="btnSearchButton">查询</a>
                            </div>
                            </div>
                        
                     </div>
            </div>
        </div>

        <div>
            <table class="table table-hover mar table-bordered" style="table-layout: fixed">
                <%--<thead>
                    <tr>
                        <th width="120">交易号</th>
                        <th width="120">订单号</th>
                        <th width="100">收货人</th>
                        <th width="100">订单金额</th>
                        <th width="100">实付金额</th>
                        <th width="100">支付方式</th>
                        <th width="120">订单完成时间</th>
                        <th width="120">备注</th>
                    </tr>
                </thead>
                <tbody>

                    <asp:Repeater ID="reCommissions" runat="server">
                        <ItemTemplate>
                            <tr class="td_bg">
                                <td width="200">&nbsp; <%# Eval("RequestTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>
                                <td>&nbsp;￥<%# Eval("Amount", "{0:F2}")%></td>
                                <td>&nbsp;<%# VShopHelper.GetCommissionPayType(Eval("RequestType").ToString())%>
                                </td>
                                <td>&nbsp;<%# Eval("MerchantCode") %></td>
                                <td>&nbsp;<%# Eval("AccountName") %></td>
                                <td>&nbsp;<%# Eval("CheckTime", "{0:yyyy-MM-dd HH:mm:ss}")%></td>

                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>--%>
            </table>
        </div>



        <!--数据列表底部功能区域-->
        <br />
        <div class="select-page clearfix">
            <div class="form-horizontal fl">
                <a onclick="javascript:history.go(-1)" class="btn btn-primary">返回</a>
            </div>
            <div class="page fr">
                <div class="pageNumber">
                    <div class="pagination" style="margin-right: 30px;margin-top:0;">
                        <div id="kkpager"></div>
                        <%--<UI:Pager runat="server" ShowTotalPages="true" DefaultPageSize="5" TotalRecords="30" ID="pager" />--%>
                    </div>
                </div>
            </div>
        </div>

        <div class="clearfix" style="height: 130px" id="footer"></div>

        
        <%--  调整余额--%>
         <div class="modal fade" id="SetUserAmount">
          <div class="modal-dialog">
            <div class="modal-content">
              <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <h4 class="modal-title" style="text-align:left" id="txtTitle">调整账户余额</h4>
              </div>
                <div class="modal-body form-horizontal" >
                    <div class="form-group">
                                <label for="inputEmail3" class="col-xs-4 control-label">账户余额：</label>
                                <div class="col-xs-6">
                                    <label id="lbNowAmount" style="margin-top:7px;display:block">0</label>
                                </div>
                        </div>
                     <div class="form-group">
                                <label for="inputEmail3" class="col-xs-4 control-label"><em>*</em>调整金额：</label>
                                <div class="col-xs-6">
                                    <input type="text" name="txtAmount" id="txtAmount" style="width:200px;" maxlength="9" onkeyup="this.value = (this.value.match(/^[\+\-]?\d*?\.?\d{0,2}?$/) || [''])[0]" placeholder="0.00"/>
                                    <span style="color: darkgray;">正数增加余额，负数减少余额</span>
                                </div>
                        </div>
                     <div class="form-group">
                                <label for="inputEmail3" class="col-xs-4 control-label" ><em>*</em>调整备注：</label>
                                <div class="col-xs-6">
                                    <textarea type="text" id="txtSetAmountBark" name="txtSetAmountBark" rows="3" style="width: 200px;" placeholder="余额调整备注说明" maxlength="50"></textarea>
                                </div>
                        </div>
                </div>
              <div class="modal-footer">
                  <input type ="text" id ="txtAmountUserId" value ="" style ="display:none;" />
                  <input type ="button" id ="btnUpdateAmount" name="btnUpdateAmount"  class="btn btn-primary" value="确定修改" />
                  <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
              </div>
            </div>
          </div>
        </div>

    </form>

</asp:Content>

