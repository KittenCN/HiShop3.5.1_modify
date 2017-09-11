<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="distributorcenter.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Fenxiao.distributorcenter" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style>
    h2,h3,p,ul,li{ margin:0;}
    .width90{ width:90px !important;}
    .width120{ width:120px !important;}
    .phonebox{ width:345px; height:770px; float:left; background:url(images/phonebg.jpg) no-repeat; padding:98px 17px 148px; margin-left:50px;}
    .phonebox label{ font-weight:normal; margin:0;}
    .phonebox h2{ height:38px; line-height:38px; text-align:center; font-size:16px; color:#fff;}
    .phonebox .dpinfo{ height:87px; padding-left:78px; padding-top:20px;}
    .phonebox .dpinfo p{ color:#fff; padding-left:10px;}
    .phonebox .dpinfo small{ padding-left:6px; margin-left:-6px;}
    .phonebox .dpinfo small span{ color:#ffa200;}
    .phonebox .change span{ line-height:24px; border:1px solid #transparent; display:inline-block; margin-left:-6px; padding:0 10px 0 5px;}
    .phonebox .title{ width:100%; height:32px; line-height:32px; text-align:center;}
    .phonebox .title li{ width:50%; color:#fff; float:left;}
    .phonebox .summary{ width:100%; height:54px; text-align:center; padding-top:7px;}
    .phonebox .summary li{ width:33.33%; float:left; color:#fff;}
    .phonebox .dptier{ margin-top:38px; background:#f5f5f5;}
    .phonebox .dptier .dplist{ height:34px; line-height:34px; border-bottom:1px solid #d9d9d9; padding-left:50px; margin-bottom:8px; background:#fff; border-top:1px solid #e3e3e3; position:relative; border-left:1px solid #fff; border-right:1px solid #fff;}
    .phonebox .dptier .dplist.part{ margin-top:-8px; border-top:0;}
    .phonebox .dptier .dplist:after{ width:9px; height:15px; content:""; display:block; background:url(images/right.png) no-repeat; position:absolute; right:10px; top:8px;}
    .phonebox .dptier .dplist i{ width:18px; height:18px; display:block; position:absolute; left:20px; top:8px;}
    .phonebox .dptier .dplist i.icon1{ background:url(images/icon1.jpg) center no-repeat;}
    .phonebox .dptier .dplist i.icon2{ background:url(images/icon2.jpg) center no-repeat;}
    .phonebox .dptier .dplist i.icon3{ background:url(images/icon3.jpg) center no-repeat;}
    .phonebox .dptier .dplist i.icon4{ background:url(images/icon4.jpg) center no-repeat;}
    .phonebox .dptier .dplist .num{ width:28px; height:18px; line-height:18px; border-radius:10px; border:1px solid #c82100; display:inline-block; position:absolute; top:6px; right:30px; color:#c82100; font-size:12px; text-align:center;}
	
    .editinfo{ width:464px; margin-left:40px; margin-top:108px; border:1px solid #d3d3d3; background:#f4f4f4; border-radius:10px; padding:10px 30px; float:left; position:relative;}
    .editinfo:before{ content:""; display:block; border-right:10px solid #d3d3d3; border-top:8px solid transparent; border-bottom:8px solid transparent; position:absolute; left:-10px; top:25px;}
    .editinfo:after{ content:""; display:block; border-right:10px solid #f4f4f4; border-top:8px solid transparent; border-bottom:8px solid transparent; position:absolute; left:-8px; top:25px;}
    .editinfo h3{ line-height:30px; font-size:14px; font-weight:600; border-bottom:1px solid #d3d3d3; margin-bottom:20px;}
    .editinfo label em{ font-style:normal; margin-right:5px; color:#ff0000;}
    .editinfo .explain{ padding:15px 0; margin-top:30px; border-top:1px solid #d3d3d3;}
    .editinfo .explain p{ color:#999999; line-height:20px; font-size:12px;}
    .editinfo .explain p.indent3{ text-indent:3em;}
</style>
<script>
    $(function(){
        $('.editinfo').find('input[type="text"]').keyup(function(){
            var id = $(this).prop('id');
            $('[data-target="'+id+'"]').html($(this).val())
            if($(this).val()==''){
                $('[data-target="'+id+'"]').html($(this).prop('placeholder'))
            }
        }).focus(function(){
            var id = $(this).prop('id');
            $(this).css('border','1px solid #ffa200');
            $('[data-target="'+id+'"]').parent().css('border','2px dashed #ffa200')	
        }).blur(function(){
            var id = $(this).prop('id');
            $(this).css('border','1px solid #c3c3c3');
            $('[data-target="'+id+'"]').parent().removeAttr('style');	
        })
    })
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-header">
        <h2>分销中心</h2>
    </div>
    <div class="clearfix mt50">
	<div class="phonebox">
    	<h2><label data-target="fxCenter">分销中心</label></h2>
        <div class="dpinfo">
        	<p>天天的小店【初级】</p>
            <p class="change"><span><label data-target="commissionName">佣金</label>金额：¥20.00</span></p>
            <p><small>再获得<span>180.00</span>元<label data-target="commissionName">佣金</label>升级为<span>中级用户</span></small></p>
        </div>
        <ul class="title">
            <li>会员中心</li>
            <li><label data-target="fxCenter">分销中心</label></li>
        </ul>
        <ul class="summary">
        	<li>
            	<p>¥0.00</p>
                <p>销售额</p>
            </li>
            <li>
            	<p>¥0.00</p>
                <p>今日<label data-target="commissionName">佣金</label></p>
            </li>
            <li>
            	<p>0</p>
                <p>今日订单</p>
            </li>
        </ul>
        <div class="dptier">
        	<div class="dplist"><i class="icon1"></i>店铺订单<p class="num">0</p></div>
            <div class="dplist"><i class="icon2"></i><label data-target="fxTeamName">我的下属</label></div>
            <div class="dplist part"><label data-target="shopName">店铺会员</label><p class="num">0</p></div>
            <div class="dplist part"><label data-target="firstShop">一级会员</label><p class="num">0</p></div>
            <div class="dplist part"><label data-target="secondShop">二级会员</label><p class="num">0</p></div>
            <div class="dplist"><i class="icon3"></i><label data-target="myCommission">我的佣金</label></div>
            <div class="dplist"><i class="icon4"></i><label data-target="fxExplain">店铺说明</label></div>
        </div>
    </div>
    <div class="editinfo">
    	<h3>分销名称编辑</h3>
        <form class="form-horizontal">
            <div class="form-group">
                <label class="control-label col-xs-5"><em>*</em>分销中心名称</label>
                <div class="col-xs-7"><input type="text" id="fxCenter" class="form-control width120" placeholder="分销中心" maxlength="10" value="<%=DistributorCenterName %>" /></div>
            </div>
            <div class="form-group">
                <label class="control-label col-xs-5"><em>*</em>佣金名称</label>
                <div class="col-xs-7"><input type="text" id="commissionName" class="form-control width120" placeholder="佣金" maxlength="4" value="<%=CommissionName %>" /></div>
            </div>
            <div class="form-group">
                <label class="control-label col-xs-5"><em>*</em>分销团队名称</label>
                <div class="col-xs-7"><input type="text" id="fxTeamName" class="form-control width120" placeholder="我的下属" maxlength="16" value="<%=DistributionTeamName %>" /></div>
            </div>
            <div class="form-group">
                <label class="control-label col-xs-5"><em>*</em>我的店铺名称</label>
                <div class="col-xs-7"><input type="text" id="shopName" class="form-control width120" placeholder="店铺会员" maxlength="10" value="<%=MyShopName %>" /></div>
            </div>
            <div class="form-group">
                <label class="control-label col-xs-5"><em>*</em>一级分店名称</label>
                <div class="col-xs-7"><input type="text" id="firstShop" class="form-control width120" placeholder="一级分店" maxlength="10" value="<%=FirstShopName %>" /></div>
            </div>
            <div class="form-group">
                <label class="control-label col-xs-5"><em>*</em>二级分店名称</label>
                <div class="col-xs-7"><input type="text" id="secondShop" class="form-control width120" placeholder="二级分店" maxlength="10" value="<%=SecondShopName %>" /></div>
            </div>
            <div class="form-group">
                <label class="control-label col-xs-5"><em>*</em>我的佣金名称</label>
                <div class="col-xs-7"><input type="text" id="myCommission" class="form-control width120" placeholder="我的佣金" maxlength="16" value="<%=MyCommissionName %>" /></div>
            </div>
            <div class="form-group">
                <label class="control-label col-xs-5"><em>*</em>分销说明名称</label>
                <div class="col-xs-7"><input type="text" id="fxExplain" class="form-control width120" placeholder="分销说明"  maxlength="16" value="<%=DistributionDescriptionName %>" /></div>
            </div>
            <div class="form-group mt30">
                <div class="col-xs-offset-5">
                    <button type="button" class="btn btn-primary width90 fl ml15"  onclick="SavefxCenter()">保存</button>
                    <a class="fl" href="javascript:;" style="margin-top:14px; margin-left:10px;"   onclick="HuifufxCenter()">恢复默认</a>
                    <input type="button" id="huifu" style="display:none;" />               
                </div>
            </div>
        </form>
        <div class="explain">
        	<p>说明：1. 为了商城运营更好，分销设置内容必须符合政策运营范围；</p>
            <p class="indent3">2. 由分销设置内容引发的政策风险，由商家自行承担。</p>
        </div>
    </div>
</div>
    
    <script>
        $(function() {
            $('.editinfo').find('input[type="text"]').each(function() {
                var id = $(this).prop('id');
                $('[data-target="' + id + '"]').html($(this).val());
                if ($(this).val() == '') {
                    $('[data-target="' + id + '"]').html($(this).prop('placeholder'));
                }
            });

            $("#huifu").click(function() {
                if (!isconfirmOK) {
                    return HiConform('<strong></strong><p>确定要将分销中心恢复默认设置吗？</p>', this);
                }
                var array = new Array();
                $('.editinfo').find('input[type="text"]').each(function () {
                    array.push($(this).prop('placeholder'));
                });
                SetDistributorCenter(array, "Huifu");
            });

        });

        function SavefxCenter() {
            var array = new Array();
            $('.editinfo').find('input[type="text"]').each(function() {
                array.push($(this).val() == "" ? $(this).prop('placeholder') : $(this).val());
            });
            SetDistributorCenter(array,"Save");
        }

        function HuifufxCenter() {
            $("#huifu").trigger("click");          
        };

        function SetDistributorCenter(array, action) {
            if (array.length !== 8) {
                HiTipsShow("数据出现错误", 'fail');
                return;
            }
            $.ajax({
                type: "post",
                url: "distributorcenter.aspx",
                data: { fx0: array[0], fx1: array[1], fx2: array[2], fx3: array[3], fx4: array[4], fx5: array[5], fx6: array[6], fx7: array[7], action: action },
                dataType: "text",
                success: function (data) {
                    if (action == "Huifu") {
                        $('.editinfo').find('input[type="text"]').each(function() {
                            var id = $(this).prop('id');
                            $(this).val($(this).prop('placeholder'));
                            $('[data-target="' + id + '"]').html($(this).prop('placeholder'));
                        });
                    }
                    HiTipsShow("操作成功", 'success');
                }
            });
        }
    </script>

</asp:Content>
