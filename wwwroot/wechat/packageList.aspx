<%@ Page Language="C#" AutoEventWireup="true" CodeFile="packageList.aspx.cs" Inherits="chefrom" %>
<!DOCTYPE html>
<html lang="en" style="font-size: 100px;">
<head>
	<meta charset="utf-8">
	<meta http-equiv="X-UA-Compatible" content="IE=edge">
	<title>车险</title>
	<meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
	<meta name="format-detection" content="telephone=no">
	<meta name="description" content="">
	<meta name="author" content="">
	<link href="/2.0/css/base.css?1.0123456" rel="stylesheet">
	<script src="/2.0/js/jquery-2.1.1.js"></script>
	<script src="/2.0/js/common.js?1.0123456"></script>
	<script>
		//默认字体大小设置
		(function(win) {
			initPage();
			window.onresize = function () {
				initPage()
			};
			function initPage() {
				document=window.document,docElem=document.documentElement;
				var htmlWidth = docElem.getBoundingClientRect().width;
				htmlWidth > 750 && (htmlWidth = 750 * 1);
				var rem = htmlWidth / 7.50;
				docElem.style.fontSize = rem + "px"
			}
		})(window);
	</script>

    <link rel="stylesheet" href="/wechat/citySelect/cityPicker.css">
    <script src="/wechat/citySelect/cityPicker.js"></script> 
</head>
<body class="bg_ff">

<!--题图开始-->
<div class="banner">
	<img src="/2.0/images/bann2.jpg"/>
</div>
<!--题图end-->

<!--城市、代理人、保险公司、上年是否同家开始-->
<form method="post" id="form_package" action="">
<input type="hidden" name="_token" value="4DY4GoSaC2j7kPsVOGD6wHn2bJjP6ktiSHKyiuYM"/>
<input type="hidden" name="is_ask" id="is_ask" value=""/>
 
<input type="hidden" name="pp" value="{&quot;agent&quot;:3,&quot;agent_name&quot;:&quot;\u8d75\u5a9b&quot;}" />
<input type="hidden" name="city1" id="city1" value="" />
<input type="hidden" name="city2" id="city2" value="" />
<input type="hidden" name="agent" id="agent" value="3" />
<input type="hidden" name="company" id="company" value="" />
<input type="hidden" name="is_renewal" id="is_renewal" value="" />
<div class="cars_index_top bg_ff" style='border:0'>
	<ul >
		<li>
			<i class="city"></i>
			<p>所属城市</p>
			
                <input type="text"  class="select" value="请选择" id="city" readonly>
							
					
		</li>
		<li>
			<i class="dlr"></i>
			<p>保险代理人</p>
			<a class="select" onclick="agentsSelect()">
						赵媛
						</a>
		</li>
		<li>
			<i class="gs"></i>
			<p>保险公司</p>
			<a class="select" onclick="insuranceCompany()">
						请选择
						</a>
		</li>
		<li class="ckbox">
			<i class="ts"></i>
			<p>上年是同一保险公司</p>
			<a class="select">否</a>
		</li>
	</ul>
</div>
</form>
<!--城市、代理人、保险公司、上年是否同家end-->
<script>
    $("#city").CityPicker();
</script>
<!--自动报价、个性化投保-->
<div class="cars_index_bottom bg_ff" style='margin:0;border:0'>
	<div class="insure insure1" onclick="packageSub(2)">
		<h2 style="border-bottom-left-radius: .2rem;border-bottom-right-radius: .2rem;">立即投保</h2>
<!--		<p>-->
<!--			<span>个性化投保&nbsp;&nbsp;各险种自由选择</span>-->
<!--			用户可自由选择投保险种及附加险。<br/><br/>-->
<!--		</p>-->
	</div>
	<div style="display: none;" class="insure" style="margin-top:.5rem"  onclick="packageSub(1)">
		<h2>模拟自动报价</h2>
		<p>
			<span>交强险&nbsp;&nbsp;商业三者责任险</span>
			包括交强险、商业三者、商业三者不计免赔、车船税。
		</p>
	</div>
	<div class="kf">
		客服电话：<a href="javascript:;">400-9677-966</a>
	</div>
</div>
<!--自动报价、个性化投保end-->
<script>

function packageSub(id) {

	var city1 = $("#city").val();
	if (city1 == "请选择") {
		prompt("车辆所在地未选");
		return false;
	}

	//var city2 = $("#city2").val();
	//if (0 == city2.length) {
	//	prompt("车辆所在地市未选");
	//	return false;
	//}

	var agent = $("#agent").val();
	if (0 == agent.length) {
		prompt("代理人员未选");
		return false;
	}
	
	var company = $("#company").val();
	if (0 == company.length) {
		prompt("保险公司未选");
		return false;
	}

	if (1 == id) {
		$("#is_ask").val(1);
		$("#form_package").attr("action", "/wechat/package/1");
	} else if (2 == id) {
		$("#is_ask").val(0);
		$("#form_package").attr("action", "/wechat/package/2");
	} else if (3 == id) {
		$("#is_ask").val(1);
		$("#form_package").attr("action", "/wechat/package/1");
	}

	$("#form_package").submit();

}

	function citySelect() {
		$("#form_package").attr("action", "/wechat/citySelect.aspx");
		$("#form_package").submit();
	}
	function agentsSelect() {
		$("#form_package").attr("action", "/wechat/agents");
		$("#form_package").submit();
	}
	function insuranceCompany() {

		var city1 = $("#city1").val();
		if (0 == city1.length) {
			prompt("先选择省份，才能选保险公司");
			return false;
		}
		
		$("#form_package").attr("action", "/wechat/insuranceCompany");
		$("#form_package").submit();
	}

	$('.ckbox').click(function(){
		var ck=$(this).find('i').attr('class');
		if(ck=='ts'){
			$(this).find('i').addClass('opents');
			$(this).find('.select').html('是');
			$("#is_renewal").val(1);
		}else{
			$(this).find('.select').html('否');
			$(this).find('i').removeClass('opents');
			$("#is_renewal").val(0);
		}
	});
</script>
</body>
</html>