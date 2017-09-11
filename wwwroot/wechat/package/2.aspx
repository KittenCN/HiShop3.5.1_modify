<%@ Page Language="C#" AutoEventWireup="true" CodeFile="2.aspx.cs" Inherits="wechat_package_2" %>

<!DOCTYPE html>
<html lang="en" style="font-size: 100px;">
<head>
	<meta charset="utf-8">
	<meta http-equiv="X-UA-Compatible" content="IE=edge">
	<title>个性化投保</title>
	<meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
	<meta name="format-detection" content="telephone=no">
	<meta name="description" content="">
	
	<meta name="author" content="">
	<link href="/2.0/css/all.css?v=1.0.2" rel="stylesheet">
	<script src="/2.0/js/ossPost.js"> </script>
	<script src="/2.0/js/jquery-2.1.1.min.js"></script>
	<script src="/2.0/js/common.js?1.0123456"></script>
	<script src="/2.0/js/icheck.js"></script>
	<script src="/2.0/js/jquery-weui.js"></script>
	<script src="/2.0/js/validate.js"></script>
	<link href="/2.0/css/base.css?1.0123456" rel="stylesheet">
	<script src="/2.0/js/address.js?a=1"></script>
	<style> 
#cover{ 
display:none; 
position:fixed; 
z-index:1; 
top:0; 
left:0; 
width:100%; 
height:100%; 
background:rgba(0, 0, 0, 0.44); 
} 
#coverShow{ 
display:none; 
position:fixed; 
z-index:2; 
top:60%; 
left:70%; 
width:150px; 
height:150px; 
margin-left:-150px; 
margin-top:-150px; 
} 
</style>
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
</head>
<body>
<div id="cover"></div> 
<form id="form_insure" method="post" action="/wechat/packageSave">

	<input type="hidden" id="is_sub" value="0" />
			<input type="hidden" name="p_order_city1" value="145">
		<input type="hidden" name="p_order_city2" value="152">
		<input type="hidden" name="p_order_company_id" value="49">
		<input type="hidden" name="p_order_company_name" value="亚太保险公司">
		<input type="hidden" name="p_order_agent" value="4">
		<input type="hidden" name="p_order_is_renewal" value="0">
		<input type="hidden" name="_token" value="4DY4GoSaC2j7kPsVOGD6wHn2bJjP6ktiSHKyiuYM">
	<input type="hidden" name="city_select" value="0">
	<input type="hidden" name="jing_ji" value="2">
	<input type="hidden" name="orders_id" value="">
	<input type="hidden" name="insurance_id" value="">

<div class="individualization">
	<!--温馨提示-->
	<div class="individualization_title">
		<h4>温馨提示</h4>
		1.勾选所需投保险种按钮，取消勾选即为不选择此险种<br/>
		2.新车购车发票及车辆合格证超过3天，需要验车<br/>
		3.续保或转保车辆需要提供上一年商业险保单（未出险，
		未加项，未到期的不需要验车）
	</div>
	<!--温馨提示end-->



			<h2>
					基本险
			</h2>
	<div class="individualization_list bg_ff">
		<table cellpadding="0" cellspacing="0">
						<input type="hidden" id="damage_check_1" value="0">
			<tr>
				<th><input type="checkbox"
											class="ckbox select"
										name="car_insure[]"
					id="car_insure_1" 
					value="1"></th>
				<td class="in">
					<aa id="che_name_1">交强险</aa><span style="display:none;">100%车主投保</span>
					<p>国家强制购买，不购买无法上路</p>
				</td>
				<td class="right">
									</td>
			</tr>
						<input type="hidden" id="damage_check_2" value="0">
			<tr>
				<th><input type="checkbox"
											class="ckbox select"
										name="car_insure[]"
					id="car_insure_2" 
					value="2"></th>
				<td class="in">
					<aa id="che_name_2">商业三者</aa><span style="display:none;">100%车主投保</span>
					<p>赔付需要您承担的他人伤亡及财产损失（含不计免赔）</p>
				</td>
				<td class="right">
					
						<div class="select">
					<select 
					name="car_insure_2_property"
					id="car_insure_2_property">
												<option
						value="5"
						 
						>5万</option>
												<option
						value="10"
						 
						>10万</option>
												<option
						value="15"
						 
						>15万</option>
												<option
						value="20"
						 
						>20万</option>
												<option
						value="30"
						 
						>30万</option>
												<option
						value="50"
													selected
						 
						>50万</option>
												<option
						value="100"
						 
						>100万</option>
												<option
						value="150"
						 
						>150万</option>
												<option
						value="200"
						 
						>200万</option>
											</select>
						</div>
									</td>
			</tr>
						<input type="hidden" id="damage_check_5" value="0">
			<tr>
				<th><input type="checkbox"
											class="ckbox select"
										name="car_insure[]"
					id="car_insure_5" 
					value="5"></th>
				<td class="in">
					<aa id="che_name_5">车上人员险（司机）</aa><span style="display:none;">100%车主投保</span>
					<p>赔付车内驾驶员本人的人身伤亡费用（含不计免赔）</p>
				</td>
				<td class="right">
					
						<div class="select">
					<select 
					name="car_insure_5_property"
					id="car_insure_5_property">
												<option
						value="1"
													selected
						 
						>1万</option>
												<option
						value="2"
						 
						>2万</option>
												<option
						value="3"
						 
						>3万</option>
												<option
						value="4"
						 
						>4万</option>
												<option
						value="5"
						 
						>5万</option>
												<option
						value="10"
						 
						>10万</option>
											</select>
						</div>
									</td>
			</tr>
						<input type="hidden" id="damage_check_6" value="0">
			<tr>
				<th><input type="checkbox"
											class="ckbox select"
										name="car_insure[]"
					id="car_insure_6" 
					value="6"></th>
				<td class="in">
					<aa id="che_name_6">车上人员险（乘客）</aa><span style="display:none;">100%车主投保</span>
					<p>赔付车内乘客的人身伤亡费用（含不计免赔）</p>
				</td>
				<td class="right">
					
						<div class="select">
					<select 
					name="car_insure_6_property"
					id="car_insure_6_property">
												<option
						value="1"
													selected
						 
						>1万</option>
												<option
						value="2"
						 
						>2万</option>
												<option
						value="3"
						 
						>3万</option>
												<option
						value="4"
						 
						>4万</option>
												<option
						value="5"
						 
						>5万</option>
												<option
						value="10"
						 
						>10万</option>
											</select>
						</div>
									</td>
			</tr>
						<input type="hidden" id="damage_check_3" value="2">
			<tr>
				<th><input type="checkbox"
											class="ckbox select"
										name="car_insure[]"
					id="car_insure_3" 
					value="3"></th>
				<td class="in">
					<aa id="che_name_3">车辆损失险</aa><span style="display:none;">100%车主投保</span>
					<p>赔付您爱车的损失（含不计免赔）</p>
				</td>
				<td class="right">
									</td>
			</tr>
						<input type="hidden" id="damage_check_4" value="1">
			<tr>
				<th><input type="checkbox"
											class="ckbox"
										name="car_insure[]"
					id="car_insure_4" 
					value="4"></th>
				<td class="in">
					<aa id="che_name_4">盗抢险</aa><span style="display:none;">100%车主投保</span>
					<p>赔付全车被盗窃、抢劫、抢夺造成的车辆损失（含不计免赔）</p>
				</td>
				<td class="right">
									</td>
			</tr>
					</table>
	</div>
		<h2>
					附加险
			</h2>
	<div class="individualization_list bg_ff">
		<table cellpadding="0" cellspacing="0">
						<input type="hidden" id="damage_check_7" value="1">
			<tr>
				<th><input type="checkbox"
											class="ckbox"
										name="car_insure[]"
					id="car_insure_7" 
					value="7"></th>
				<td class="in">
					<aa id="che_name_7">玻璃破碎险</aa><span style="display:none;">100%车主投保</span>
					<p>赔付挡风玻璃和车窗玻璃单独破碎的损失</p>
				</td>
				<td class="right">
									</td>
			</tr>
						<input type="hidden" id="damage_check_8" value="1">
			<tr>
				<th><input type="checkbox"
											class="ckbox"
										name="car_insure[]"
					id="car_insure_8" 
					value="8"></th>
				<td class="in">
					<aa id="che_name_8">自燃险</aa><span style="display:none;">100%车主投保</span>
					<p>赔付因线路老化等自身原因起火造成车辆本身损失</p>
				</td>
				<td class="right">
									</td>
			</tr>
						<input type="hidden" id="damage_check_9" value="1">
			<tr>
				<th><input type="checkbox"
											class="ckbox"
										name="car_insure[]"
					id="car_insure_9" 
					value="9"></th>
				<td class="in">
					<aa id="che_name_9">涉水险</aa><span style="display:none;">100%车主投保</span>
					<p>赔付车辆因水淹或因涉水行驶造成的发动机损坏</p>
				</td>
				<td class="right">
									</td>
			</tr>
						<input type="hidden" id="damage_check_10" value="1">
			<tr>
				<th><input type="checkbox"
											class="ckbox"
										name="car_insure[]"
					id="car_insure_10" 
					value="10"></th>
				<td class="in">
					<aa id="che_name_10">指定专修特约险</aa><span style="display:none;">100%车主投保</span>
					<p>维修时可以自主选择有资质的4S店</p>
				</td>
				<td class="right">
									</td>
			</tr>
						<input type="hidden" id="damage_check_12" value="1">
			<tr>
				<th><input type="checkbox"
											class="ckbox select"
										name="car_insure[]"
					id="car_insure_12" 
					value="12"></th>
				<td class="in">
					<aa id="che_name_12">无法找到第三方责任险</aa><span style="display:none;">100%车主投保</span>
					<p>出第三方事故后找不到肇事者，如果购买此险种，保险公司全额赔付</p>
				</td>
				<td class="right">
									</td>
			</tr>
						<input type="hidden" id="damage_check_14" value="1">
			<tr>
				<th><input type="checkbox"
											class="ckbox"
										name="car_insure[]"
					id="car_insure_14" 
					value="14"></th>
				<td class="in">
					<aa id="che_name_14">划痕险</aa><span style="display:none;">100%车主投保</span>
					<p>赔付他人恶意行为造成的车辆车身人为划痕</p>
				</td>
				<td class="right">
									</td>
			</tr>
					</table>
	</div>
		




	<h2>车辆基本信息</h2>
	<div class="individualization_table bg_ff">
		<table cellpadding="0" cellspacing="0">
			<tr>
				<th>车辆类型</th>
				<td style="cursor: pointer">
					<div class="ri">
						<select name="car_new_type" id="car_new_type" onchange="carNewType()">
														<option
							value="1"
														>非新购车辆</option>
														<option
							value="2"
														>新购车辆</option>
													</select>
					</div>
				</td>
			</tr>
		</table>
	</div>
	<div class="uplod_img bg_ff">
		<ul class="clearfix">
			<li>
									<img  src="http://wyx.bzb100.com/plugins/weui/dist/image/x1.jpg" onclick="upload(carpic1)" class="carpic1"  width="100%" >
		    			    	<input type="file" class="imgUplod" id="carpic1" name="carpic1" style="display: none"
           		 />
			</li>
			<li>
				           			<img  src="http://wyx.bzb100.com/plugins/weui/dist/image/x2.jpg" onclick="upload(carpic2)" class="carpic2"  width="100%">
              					<input type="file" id="carpic2" class="imgUplod"  name="carpic2" style="display: none"
           		 />
			</li>
		</ul>
	</div>
	<div class="borde"></div>
	<h2>被保人基本信息</h2>
	<div class="individualization_table1 bg_ff">
		<table cellpadding="0" cellspacing="0">
			<tr>
				<th>被保人姓名</th>
				<td>
					<input type="text" class="no_bor_input" 
					id="beneficiary" name="beneficiary" 
						      				value=""
	      								placeholder="请输入被保人姓名">
				</td>
			</tr>
			<tr>
				<th>联系电话</th>
				<td>
					<input type="tel" class="no_bor_input" 
					id="beneficiary_tel" name="beneficiary_tel" 
						      				value="13714137149"
	      								placeholder="请输入被联系电话">
				</td>
			</tr>
		</table>
	</div>
	<div class="uplod_img bg_ff">
		<ul class="clearfix">
			<li>
				              		<img  src="http://wyx.bzb100.com/plugins/weui/dist/image/ss1.jpg" onclick="upload(idpic1)" class="idpic1"   width="100%">
            	            	<input type="file" id="idpic1" name="idpic1" class="imgUplod"  style="display: none"
              	 />
			</li>
			<li>
				             		<img  src="http://wyx.bzb100.com/plugins/weui/dist/image/ss2.jpg"  onclick="upload(idpic2)" class="idpic2" width="100%">
            	          		<input type="file" id="idpic2" name="idpic2" class="imgUplod"  style="display: none"
           		 />
			</li>
		</ul>
	</div>
	<div class="borde"></div>
	<h2>保单邮寄地址</h2>
	<div class="individualization_table1 bg_ff">
				<table cellpadding="0" cellspacing="0">
			<tr>
				<td colspan="2" style="width:94%;padding-left:.3rem;">
					<div class="selt">
						<select id="cmbProvince" name="province"></select>
					</div>
					<div class="selt">
						<select id="cmbCity" name="city"></select>
					</div>
					<div class="selt">
						<select id="cmbArea" name="area"></select>
					</div>
				</td>
			</tr>
			<tr>
				<th>详细地址</th>
				<td>
					<input type="text" class="no_bor_input" 
					name="express_address" id="express_address" 
						      					      					value=""
	      					      								placeholder="请输入详细地址">
				</td>
			</tr>
			<tr>
				<th>收件人</th>
				<td>
					<input type="text" class="no_bor_input" 
					name="express_name" id="express_name" 
						      					      					value=""
	      					      								placeholder="请输入联系姓名">
				</td>
			</tr>
			<tr class="last_bor">
				<th>联系电话</th>
				<td>
					<input type="text" class="no_bor_input" 
					name="express_contact" id="express_contact" 
						      					      					value=""
	      					      								placeholder="请输入联系电话">
				</td>
			</tr>
		</table>
	</div>

	<div class="save_bottom bg_ff">
		<input type="button" class="save_but" onclick="insureSub()" value="立即购买">
	</div>

</div>
</form>
<div id="coverShow"> 
	<div style="padding:5px 15px;background:#3c3c3f;opacity:0.9;border-radius:10px">
       <div style="color:#fff;line-height:20px;font-size:12px;padding-top:10%;padding-bottom:10%">
                    <table width="100%" cellpadding="0" cellspacing="0" border="0">
                        <tbody><tr><td align="center"><img src="/images/loading.gif"></td></tr>
                        <tr><td valign="middle" align="center">提交中，请稍后！</td></tr>
                    </tbody></table>
                    </div>
    </div>
</div> 
<script>

	function insureSub() {


	
		
		var is_sub = $("#is_sub").val();
		if (1 == is_sub) {
			prompt("保单查询已提交");
			return false;
		}

		var insure_flag = 0;
		var check2_flag = 0;
		var check1_flag = 0;
		var check2_name = "";
		var check1_name = "";
		$("input[name='car_insure[]']").each(function(){
			if ($(this).is(':checked')) {
				insure_flag = 1;
				var id_value = $(this).val();
				if (2 == $("#damage_check_"+id_value).val()) {
					check2_flag = 1;
					if (!check2_name) {
						check2_name = $("#che_name_"+id_value).html();
					}
				}
				if (1 == $("#damage_check_"+id_value).val()) {
					check1_flag = 1;
					if (!check1_name) {
						check1_name = $("#che_name_"+id_value).html();
					}
				}
			} else {
				var id_value = $(this).val();
				if (2 == $("#damage_check_"+id_value).val()) {
					if (!check2_name) {
						check2_name = $("#che_name_"+id_value).html();
					}
				}
			}
		});
		if (0 == insure_flag) {
			prompt("险种必选其一");
			return false;
		}

		if (0 == check2_flag && 1 == check1_flag) {
			var error = "购买【"+check1_name+"】必须先购买【"+check2_name+"】";
			prompt(error);
			return false;
		}

		var car_type = $("#car_type").val();
		if (0 == car_type) {
			prompt("车辆类别未选");
			return false;
		}

		var car_ship_tax_price = $("#car_ship_tax_price").val();
		if (0 == car_ship_tax_price) {
			prompt("车型排量未选");
			return false;
		}

		var car_insurance_type = $("#car_insurance_type").val();
		if (0 == car_insurance_type) {
			prompt("出险情况未选");
			return false;
		}

		var city1 = $("#city1").val();
		if (0 == city1) {
			prompt("车辆所在地未选");
			return false;
		}

		var car_new_type = $("#car_new_type").val();
		if (0 == car_new_type) {
			prompt("车辆类型未选");
			return false;
		}
	{
		var carpic1 = $("#carpic1").val();

		if (0 == carpic1.length) {
			var error = "";
			if (1 == car_new_type) {
				error = "行驶本正本不能为空";
			}
			if (2 == car_new_type) {
				error = "购车发票不能为空";
			}
			prompt(error);
			return false;
		}

		var carpic2 = $("#carpic2").val();
		if (0 == carpic2.length) {
			var error = "";
			if (1 == car_new_type) {
				error = "行驶本副本不能为空";
			}
			if (2 == car_new_type) {
				error = "合格证不能为空";
			}
			prompt(error);
			return false;
		}

        var idpic1 = $("#idpic1").val();
		if (0 == idpic1.length) {
			error = "身份证正面不能为空";
			prompt(error);
			return false;
		}

		var idpic2 = $("#idpic2").val();
		if (0 == idpic2.length) {
			error = "身份证反面不能为空";
			prompt(error);
			return false;
		}
			
	}


		var beneficiary = $("#beneficiary").val();
		if (0 == beneficiary.length) {
			prompt("被保人姓名不能为空");
			return false;
		}

		var beneficiary_tel = $("#beneficiary_tel").val();
		if (0 == beneficiary_tel.length) {
			prompt("被保人联系电话不能为空");
			return false;
		}
		var isMobile = isMobileApp(beneficiary_tel);
        if(!isMobile){
            prompt("请正确填写被保人联系电话，例如:13476543210");
            return;
        }


		var cmbProvince = $("#cmbProvince").val();
		if ("请选择" == cmbProvince) {
			prompt("省不能为空");
			return false;
		}
		
		var cmbCity = $("#cmbCity").val();
		if ("请选择" == cmbCity) {
			prompt("市不能为空");
			return false;
		}
		
		var cmbArea = $("#cmbArea").val();
		if ("请选择" == cmbArea) {
			prompt("区不能为空");
			return false;
		}

		var express_address = $("#express_address").val();
		if (0 == express_address.length) {
			prompt("地址不能为空");
			return false;
		}

		var express_name = $("#express_name").val();
		if (0 == express_name.length) {
			prompt("收件人不能为空");
			return false;
		}

		var express_contact = $("#express_contact").val();
		if (0 == express_contact.length) {
			prompt("收件人联系电话不能为空");
			return false;
		}
		var isMobile = isMobileApp(express_contact);
        if(!isMobile){
            prompt("请正确填写收件人联系电话，例如:13476543210");
            return;
         }else{
	
	        var cover = document.getElementById("cover"); 
	        var covershow = document.getElementById("coverShow"); 
	        cover.style.display = 'block'; 
	        covershow.style.display = 'block'; 

        }
 
        var a=$("#is_sub").val(1);
        
		if($("#idpic1")[0].files[0]!=undefined){

	        var idpic1=oss($("#idpic1")[0].files[0]);

	        if(idpic1.status==200){

				$("#idpic1").attr("type","hidden").val(idpic1.img);
				
			}else{

				alert('图片上传失败');
				

			}

			
		}

		if($("#idpic2")[0].files[0]!=undefined){

	        var idpic1=oss($("#idpic2")[0].files[0]);
	        
	        if(idpic1.status==200){

				$("#idpic2").attr("type","hidden").val(idpic1.img);
				
			}else{

				alert('图片上传失败');
				

			}
	        
	
		}

		if($("#carpic1")[0].files[0]!=undefined){

	        var idpic1=oss($("#carpic1")[0].files[0]);

	        if(idpic1.status==200){

				$("#carpic1").attr("type","hidden").val(idpic1.img);
				
			}else{

				alert('图片上传失败');
				

			}
	        
	
		}

		if($("#carpic2")[0].files[0]!=undefined){

	        var idpic1=oss($("#carpic2")[0].files[0]);

	        if(idpic1.status==200){

				$("#carpic2").attr("type","hidden").val(idpic1.img);
				
			}else{

				alert('图片上传失败');
				

			}
	        
	
		}
		
		
		
   		$("#form_insure").submit();		
		
		
        
	}

	$(document).ready(function(){
		$('.select').iCheck('check');
		$('.individualization_list .ckbox').iCheck({
			checkboxClass: 'wj_icheckbox_square-yellow',
			radioClass: 'iradio_square-yellow',
			increaseArea: '20%'
		});
	});
</script>


<script>

	
	addressInit('cmbProvince', 'cmbCity', 'cmbArea');



function upload(way){
	
		way.click();

}

$(".imgUplod").change(function(){

	var img=$(this)[0].files[0];
	var imgSrc=$(this).prev();
	if(!/image\/\w+/.test(img.type)){ 
        alert("看清楚，这个需要图片！");
        return false;  
    }  

	
	var reader=new FileReader();
	reader.readAsDataURL(img);
	reader.onload=function(e){  
          
		imgSrc.attr('src',this.result);
		
    }  
	
})








</script>

</body>
</html>