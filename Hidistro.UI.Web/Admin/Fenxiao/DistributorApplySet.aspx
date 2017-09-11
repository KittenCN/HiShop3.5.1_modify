<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="DistributorApplySet.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Fenxiao.DistributorApplySet" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register Src="../Ascx/ucDateTimePicker.ascx" TagName="DateTimePicker" TagPrefix="Hi" %>
<%@ Register Src="~/hieditor/ueditor/controls/ucUeditor.ascx" TagName="KindeditorControl" TagPrefix="Kindeditor" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <Hi:Style ID="Style1"  runat="server" Href="/admin/css/bootstrapSwitch.css" />
    <Hi:Script ID="Script4" runat="server" Src="/admin/js/bootstrapSwitch1.js" />
    <style>
        #divProd .list {
            float: left;
            margin-right: 10px;
            position: relative;
        }

            #divProd .list p {
                text-align: center;
            }

            #divProd .list .glyphicon {
                position: absolute;
                right: -5px;
                top: -5px;
                display: none;
                cursor:pointer
            }

            #divProd .list:hover .glyphicon {
                display: block;
            }

        #imgAddProduct {
            float: left;
            display: block;
            margin-top: 8px;
        }
        .set-switch strong em {
    color: red;
    margin-right: 2px;
}
        .set-switch p {
        padding-left:7px;
        }
                .fxBox{ border:1px solid #ddd;}
                .fxBox .top{ background:#F7F7F7;}
                    .fxBox .set-switch {
                        margin-bottom: 0px;
border-radius: 0px;
                    }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-header">
        <h2>分销设置</h2>
    </div>
    <form id="thisForm" runat="server" class="form-horizontal">
        <div class="mate-tabl" id="mytab">
            <ul class="nav nav-tabs" role="tablist">
                <li role="presentation" class="active"><a href="#profile" aria-controls="profile" role="tab" data-toggle="tab" aria-expanded="false">基本设置</a></li>
                <li role="presentation" class=""><a href="#messages" aria-controls="messages" role="tab" data-toggle="tab">分销说明</a></li>
            </ul>
            <div class="tab-content">
                <div role="tabpanel" class="tab-pane active" id="profile">
                    <!--表单-->
            <div class="set-switch" style="margin-top:5px;">
                <strong><em>*</em>开启三级分佣</strong>
                <p>开启后，成交店铺和其上两级分销商都能获得相应的佣金</p>
                <p>关闭后，则只有成交店铺可获得相应的佣金</p>
                <div id="EnableCommission" class= "<%=_EnableCommission?"switch-btn":"switch-btn off" %>" onclick="setEnable(this,'EnableCommission')">
                    <%=_EnableCommission?"已开启":"已关闭"%>
                    <i></i>
                </div>
            </div>
                    
            <div class="set-switch" style="margin-top:5px;">
                <strong><em>*</em>分销商消费是否参与分佣</strong>
                <p>关闭后，分销商自己购买不参与销售分佣，分销商的上三级获得销售佣金</p>
                <div id="IsDistributorBuyCanGetCommission" class="<%=_IsDistributorBuyCanGetCommission?"switch-btn":"switch-btn off" %>" onclick="setEnable(this,'IsDistributorBuyCanGetCommission')">
                    <%=_IsDistributorBuyCanGetCommission?"已开启":"已关闭"%>
                    <i></i>
                </div>
            </div>

                    
            <div class="set-switch" style="margin-top:5px;">
                <strong><em>*</em>启用分销商店中店</strong>
                <p>关闭后，分销商店铺不能编辑及使用店铺LOGO、店铺名称、店铺相关信息，统一使用主站店铺信息;</p>
                <div id="IsShowDistributorSelfStoreName" class="<%=_IsShowDistributorSelfStoreName?"switch-btn":"switch-btn off" %>" onclick="setEnable(this,'IsShowDistributorSelfStoreName')">
                    <%=_IsShowDistributorSelfStoreName?"已开启":"已关闭"%>
                    <i></i>
                </div>
            </div>
                    
                    
            <div class="set-switch" style="margin-top:5px;">
                <strong><em>*</em>佣金自动转入余额</strong>
                <p>开启后，分销商所获得佣金将直接转入账户余额;</p>
                <p>关闭后，分销商需要提交申请佣金转入余额;</p>
                <div id="CommissionAutoToBalance"  class= "<%=_CommissionAutoToBalance?"switch-btn":"switch-btn off" %>" onclick="setEnable(this,'CommissionAutoToBalance')">
                    <%=_CommissionAutoToBalance?"已开启":"已关闭"%>
                    <i></i>
                </div>
            </div>
                    
            <div class="set-switch" style="margin-top:5px;">
                <strong><em>*</em>会员自动成为分销商</strong>
                <p>开启后，会员达到分销条件后自动成为分销商，无需提交申请</p>
                <div id="EnableMemberAutoToDistributor" class="<%=_EnableMemberAutoToDistributor?"switch-btn":"switch-btn off" %>" onclick="setEnable(this,'EnableMemberAutoToDistributor')">
                    <%=_EnableMemberAutoToDistributor?"已开启":"已关闭"%>
                    <i></i>
                </div>
            </div>
            <div class="set-switch" style="margin-top:5px;">
                <strong><em>*</em>开启申请分销提醒</strong>
                <p>开启后，普通用户在交易完成时如果满足有门槛的分销商申请条件，则会提示用户可以申请成为分销商</p>
                <div id="IsRequestDistributor"  class= "<%=_IsRequestDistributor?"switch-btn":"switch-btn off" %>" onclick="setEnable(this,'IsRequestDistributor')">
                    <%=_IsRequestDistributor?"已开启":"已关闭"%>
                    <i></i>
                </div>
            </div>
            <div class="fxBox" style="margin-bottom:120px;">
                <div class="set-switch">
                    <div id="IsRequestDistribdutor"  class= "<%=_DistributorApplicationCondition?"switch-btn":"switch-btn off" %>" onclick="setEnableMemkan(this,'DistributorApplicationCondition')">
                        <%=_DistributorApplicationCondition?"已开启":"已关闭"%>
                        <i></i>
                    </div>
                <div class="top"><strong><em>*</em>分销商门槛条件</strong><small>&nbsp;&nbsp;&nbsp;关闭后，会员无门槛成为分销商</small></div>
               </div>
                    <div class="form-group pt10" id="conditionsGroup"  style="display: none;">
                        <label class="col-xs-1"></label>
                        <div class="col-xs-11">
                              <div style="display:none"><input id="radioDistributorApplicationCondition" type="checkbox" runat="server" /></div>
                            <div class="form-group ">
                                &nbsp;&nbsp;&nbsp;&nbsp;<input type="checkbox" name="conditions" value="false" id="cbRechargeMoneyToDistributor" runat="server"/><label for="ctl00_ContentPlaceHolder1_cbRechargeMoneyToDistributor"> 账户单次充值&nbsp;<input type="text" id="txtRechargeMoneyToDistributor" runat="server" style="width: 100px" />元，可成为分销商</label><br />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;或<br />

                                &nbsp;&nbsp;&nbsp;&nbsp;<input type="checkbox" name="conditions" value="false" id="HasConditions" runat="server" /><label for="ctl00_ContentPlaceHolder1_HasConditions"> 累计消费额达到&nbsp;<input type="text" id="txtrequestmoney" runat="server" style="width: 100px" />元，可成为分销商</label><br />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;或<br />
                                
<div style="display:none;">
                                &nbsp;&nbsp;&nbsp;&nbsp;<input type="checkbox" name="conditions" value="false"  id="HasProduct" runat="server" />
                                <label for="ctl00_ContentPlaceHolder1_HasProduct" style="display:none;">购买指定商品可申请</label>
                                <div id="addProduct" style="width: 100%; background-color: #F0F0F0; margin-left: 30px;padding-bottom:10px">
                                    <br />
                                    <div class="form-inline mb10 ml10">
                                        <div class="form-group ml0">
                                            <label for="sellshop1">生效时间：</label>
                                            <Hi:DateTimePicker CalendarType="StartDate" ID="calendarStartDate" runat="server" CssClass="form-control resetSize inputw150" />
                                            至
                                <Hi:DateTimePicker ID="calendarEndDate" runat="server" CalendarType="EndDate" CssClass="form-control resetSize inputw150" />
                                        </div>
                                    </div>
                                    <div class="clearfix" style="margin-left: 10px;">
                                        <div id="divProd" class="clearfix fl">
                                            <%=productHtml %>
                                        </div>
                                        <img src="../images/addProduct.png" style="display: none" id="imgAddProduct" class="fl" />
                                    </div>
                                </div>
</div>
                            </div>
                            <asp:Button ID="btnSave" runat="server" OnClientClick="return PageValid();" OnClick="btnSave_Click"
                                Text="保存" CssClass="btn btn-success inputw100" />
                            <input type="hidden" runat="server" id="hiddProductId" />
                        </div>
                    </div>
                </div>

                </div>
                <div role="tabpanel" class="tab-pane " id="messages">
                    <div class="edit-text clearfix">
                        <div class="edit-text-left">
                            <div class="mobile-border">
                                <div class="mobile-d">
                                    <div class="mobile-header">
                                        <i></i>
                                        <div class="mobile-title">分销说明</div>
                                    </div>
                                    <div class="upshop-view">
                                        <%--<div class="img-info">
                                            <p>基本信息区</p>
                                            <p>固定样式，显示商品主图、价格等信息</p>
                                        </div>--%>
                                        <div class="exit-shop-info" id="fckDescriptionShow">
                                            内容区
                                        </div>
                                    </div>
                                    <div class="mobile-footer"></div>
                                </div>
                            </div>
                        </div>
                        <div class="edit-text-right">
                            <div class="edit-inner">
                                <Kindeditor:KindeditorControl ID="fckDescription" runat="server" Height="300" Width="570" />
                            </div>
                        </div>
                    </div>
                    <div class="footer-btn navbar-fixed-bottom">
                        <asp:Button runat="server" ID="Button1" Text="保存" OnClientClick="return checkDescription();" OnClick="btnSave_Description" CssClass="btn btn-success inputw100" />

                    </div>
                </div>
            </div>
        </div>
    </form>
    <script>
        function setEnableMemkan(obj, savetype) {
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
            var operName = "分销商门槛条件";
            if (enable != 'true') {
                $("#ctl00_ContentPlaceHolder1_radioDistributorApplicationCondition").removeAttr("checked");
                $.ajax({
                    type: "post",
                    url: "distributorapplyset.aspx",
                    data: { type: savetype, enable: enable },
                    dataType: "text",
                    success: function (data) {
                        $("#conditionsGroup").hide();
                        msg(operName + '已关闭！');
                    }
                });
            } else {
                $("#ctl00_ContentPlaceHolder1_radioDistributorApplicationCondition").prop('checked', 'checked');
                $("#conditionsGroup").show();
            }
        }
        function setEnable(obj, savetype) {
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
            var operName = "开启三级分佣";
            switch (savetype) {
                case "EnableCommission":
                    operName = "开启三级分佣";
                    break;
                case "CommissionAutoToBalance":
                    operName = "佣金自动转入余额";
                    break;
                case "IsRequestDistributor":
                    operName = "申请分销提醒";
                    break;
                case "IsShowDistributorSelfStoreName":
                    operName = "分销商店中店";
                    break;
                case "IsDistributorBuyCanGetCommission":
                    operName = "分销商消费是否参与分佣";
                    break;
                case "EnableMemberAutoToDistributor":
                    operName = "会员自动成为分销商";
                    break;
                default:
                    break;
            }
            $.ajax({
                type: "post",
                url: "distributorapplyset.aspx",
                data: { type: savetype, enable: enable },
                dataType: "text",
                success: function (data) {
                    if (enable == 'true') {
                        msg(operName + '已开启！');
                    }
                    else {
                        msg(operName + '已关闭！');
                    }
                }
            });
        }

        function msg(msg) {
            HiTipsShow(msg, 'success');
        }



        $(function () {
            //分销商申请条件按钮事件
            //$('#DistributorApplicationCondition').on('switch-change', function (e, data) {
            //    if (data.value) {
            //        $("#HasCondition").show();
            //        $("#lbbark").hide();
            //    } else {
            //        $("#HasCondition").hide();
            //        $("#lbbark").show();
            //    };
            //});

            //购买指定商品可申请
            $("#ctl00_ContentPlaceHolder1_HasProduct").on("change", function () {
                if ($(this)[0].checked) {
                    $("#addProduct").show();
                    $("#imgAddProduct").show();
                }
                else {
                    $("#addProduct").hide();
                    $("#imgAddProduct").hide();
                }
            });

            //分销商申请条件显示与隐藏
            if ($("#ctl00_ContentPlaceHolder1_radioDistributorApplicationCondition")[0].checked) {
                $("#conditionsGroup").show();
            } else {
                $("#conditionsGroup").hide();
            };

            if ($("#ctl00_ContentPlaceHolder1_HasProduct")[0].checked) {
                $("#addProduct").show();
                $("#imgAddProduct").show();
            } else {
                $("#addProduct").hide();
                $("#imgAddProduct").hide();
            };

            //购买指定商品可申请
            $(document).on("click", "#imgAddProduct", function () {
                ShowProduct();
            });

            //if ($("#ctl00_ContentPlaceHolder1_radiorequeston")[0].checked) { $("#conditionsGroup").show(); }
            if ($("#ctl00_ContentPlaceHolder1_HasProduct")[0].checked) { $("#addProduct").show(); }
            /*编辑器监听事件*/
            um.addListener('ready', function (editor) {
                $("#fckDescriptionShow").html(um.getContent());
            });
            um.addListener('selectionchange', function () {
                $("#fckDescriptionShow").html(um.getContent());
            });
            var tabnum = "<%= tabnum%>";
            $("#mytab li a").eq(tabnum).tab("show");
        });

        function checkDescription() {
            if (um.getContent() == "") {
                HiTipsShow("分销说明内容不能为空", 'error');
                return false;
            };
        }

        function ShowProduct() {
            var productIds = $("#ctl00_ContentPlaceHolder1_hiddProductId").val();
            $DialogFrame_ReturnValue = "";// 返回值
            DialogFrame("ProductSelect.aspx?productIds=" + productIds, "选择商品", 720, 500, function (rs) {
                LoadProd();
            });
        }

        function LoadProd() {
            var productList = [];
            var productHtml = "";
            $.ajax({
                url: "../VsiteHandler.ashx",
                type: "POST",
                async: false,
                dataType: "json",
                data: { "actionName": "ProdSelect" },
                success: function (result) {
                    if (result != null) {
                        $(result).each(
                            function (index, item) {
                                if (item.ThumbnailUrl60 == "")
                                    item.ThumbnailUrl60 = "/utility/pics/none.gif";

                                var name = item.ProductName;
                                if (name.length > 9)
                                    name = name.substr(0, 7) + "...";

                                productHtml += '<div class="list" id="div' + item.productid + '"><img src="' + item.ThumbnailUrl60 + '" id="product' + item.productid + '" title="' + item.ProductName + '" style="width:60px;height:60px;" /><p style="width:60px;">' + name + '</p><i class="glyphicon glyphicon-remove" onclick="DelProd(' + item.productid + ')"></i></div>';
                                productList.push(item.productid);
                            }
                        );
                    }
                },
                error: function (xmlHttpRequest, error) {
                }
            });

            $("#ctl00_ContentPlaceHolder1_hiddProductId").val(productList.join(","));
            $("#divProd").html("");
            $("#divProd").append(productHtml);
        }
        LoadProd();

        function DelProd(id) {
            $.ajax({
                url: "../VsiteHandler.ashx",
                type: "POST",
                async: false,
                dataType: "json",
                data: { "actionName": "ProdDel", "productid": id },
                success: function (result) {
                    var ids = $("#ctl00_ContentPlaceHolder1_hiddProductId").val().split(',');
                    var newIds = "";
                    for (var i = 0; i < ids.length; i++) {
                        if (ids[i] != id) {
                            newIds = newIds + ids[i] + ",";
                        }
                    }
                    if (newIds.length > 0)
                        newIds = newIds.substr(0, newIds.length - 1);

                    $("#ctl00_ContentPlaceHolder1_hiddProductId").val(newIds);
                    $("#div" + id + "").html("");

                    ShowMsg('删除成功!', true);
                },
                error: function (xmlHttpRequest, error) {
                }
            });
        }

        function PageValid() {
            if ($("#ctl00_ContentPlaceHolder1_radioDistributorApplicationCondition")[0].checked && !$("#ctl00_ContentPlaceHolder1_cbRechargeMoneyToDistributor")[0].checked && !$("#ctl00_ContentPlaceHolder1_HasConditions")[0].checked && !$("#ctl00_ContentPlaceHolder1_HasProduct")[0].checked) {
                ShowMsg('请至少选择一个分销商门槛条件', false);
                return false;
            }
            
            if ($("#ctl00_ContentPlaceHolder1_cbRechargeMoneyToDistributor")[0].checked) {
                var money = $("#ctl00_ContentPlaceHolder1_txtRechargeMoneyToDistributor").val();
                if (money != "" && parseFloat(money) > 0)
                { }
                else {
                    ShowMsg('账户单次充值必须为大于0的金额', false);
                    return false;
                }
            }
            if ($("#ctl00_ContentPlaceHolder1_HasConditions")[0].checked) {
                var money = $("#ctl00_ContentPlaceHolder1_txtrequestmoney").val();
                if (money != "" && parseFloat(money) > 0)
                { }
                else {
                    ShowMsg('累计消费金额必须为大于0的整数金额', false);
                    return false;
                }
            }
            if ($("#ctl00_ContentPlaceHolder1_HasProduct")[0].checked) {
                var startDate = $("#ctl00_ContentPlaceHolder1_calendarStartDate_txtDateTimePicker").val();
                var endDate = $("#ctl00_ContentPlaceHolder1_calendarEndDate_txtDateTimePicker").val();

                if (startDate==""||endDate == "") {
                    ShowMsg('选择了购买指定商品时，必须填写生效开始和结束时间', false);
                    return false;
                }

                if ((endDate != "" && endDate != null) && (startDate != "" || startDate != null)) {
                    var startNum = parseInt(startDate.replace(/-/g, ''), 10);
                    var endNum = parseInt(endDate.replace(/-/g, ''), 10);
                    if (startNum > endNum) {
                        ShowMsg('结束时间不能在开始时间之前', false);
                        return false;
                    }
                    var end = new Date(Date.parse(endDate.replace(/-/g, "/")));
                    if (new Date() > end) {
                        ShowMsg('结束时间不能小于或等于当前时间', false);
                        return false;
                    }
                }
            }
            if ($("#ctl00_ContentPlaceHolder1_HasProduct")[0].checked) {
                var ProductId = $("#ctl00_ContentPlaceHolder1_hiddProductId").val();
                if (ProductId == "") {
                    ShowMsg('请选择指定商品', false);
                    return false;
                }
            }
        }

    </script>
</asp:Content>
