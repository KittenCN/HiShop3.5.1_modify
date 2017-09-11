<%@ Page Language="C#" AutoEventWireup="true" CodeFile="citySelect - 复制.aspx.cs" Inherits="citySelect" %>

<!DOCTYPE html>
<html lang="en" style="font-size: 100px;">
<head>
	<meta charset="utf-8">
	<meta http-equiv="X-UA-Compatible" content="IE=edge">
	<title>所属城市</title>
	<meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no" />
	<meta name="format-detection" content="telephone=no">
	<meta name="_token" content="4DY4GoSaC2j7kPsVOGD6wHn2bJjP6ktiSHKyiuYM"/>
	<meta name="description" content="">
	<meta name="author" content="">
	<link href="/2.0/css/base.css?1.0123456" rel="stylesheet">
	<script src="/2.0/js/jquery-2.1.1.js"></script>
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
<!--搜索-->
<div class="city_search" style="height:.5rem;">
<!--	<i><img src="/2.0/images/icon14.png"/> </i>-->
<!--	<input type="text" class="search_but" placeholder="搜索城市" value=""/>-->
</div>
<!--搜索end-->

<!--定位城市-->
<div class="position_city">
	<h2 class="city_h2">定位城市</h2>
	<div class="city_list clearfix">
		<ul>
			<li onclick="city12Select()">
						</li>
		</ul>
	</div>
</div>
<div class="borde"></div>
<!--定位城市end-->

<!--热门城市-->
<div class="position_city">
	<h2 class="city_h2">热门地省</h2>
	<div class="city_list clearfix">
		<ul>
						<li 			id="city1_2"
			onclick="city2List(2)"
			>黑龙江</li>
						<li 			id="city1_145"
			onclick="city2List(145)"
			>天津市</li>
						<li 			id="city1_153"
			onclick="city2List(153)"
			>浙江省</li>
						<li 			id="city1_154"
			onclick="city2List(154)"
			>辽宁省</li>
						<li 			id="city1_267"
			onclick="city2List(267)"
			>北京市</li>
						<li 			id="city1_285"
			onclick="city2List(285)"
			>山东省</li>
						<li 			id="city1_286"
			onclick="city2List(286)"
			>江苏省</li>
						<li 			id="city1_325"
			onclick="city2List(325)"
			>四川省</li>
						<li 			id="city1_382"
			onclick="city2List(382)"
			>重庆市</li>
						<li 			id="city1_386"
			onclick="city2List(386)"
			>上海市</li>
						<li 			id="city1_400"
			onclick="city2List(400)"
			>甘肃省</li>
						<li 			id="city1_415"
			onclick="city2List(415)"
			>广东省（除粤B牌照）</li>
						<li 			id="city1_437"
			onclick="city2List(437)"
			>深圳市（限粤B牌照）</li>
						<li 			id="city1_439"
			onclick="city2List(439)"
			>湖北省</li>
						<li 			id="city1_457"
			onclick="city2List(457)"
			>大连市</li>
						<li 			id="city1_459"
			onclick="city2List(459)"
			>宁波市</li>
						<li 			id="city1_461"
			onclick="city2List(461)"
			>贵州省</li>
					</ul>
	</div>
	<div class="city_more">更多地省 <img src="/2.0/images/right1.png"/></div>
</div>
<div class="borde"></div>
<!--热门城市end-->

<!--所属城市-->
<div class="position_city">
	<h2 class="city_h2">所属城市</h2>
	<div class="city_list clearfix">
		<ul id="city2_list">
					</ul>
	</div>
	<div class="city_more">更多城市 <img src="/2.0/images/right1.png"/></div>
</div>
<div class="borde"></div>
<!--所属城市end-->

<form method="post" id="form_city" action="/wechat/packageList.aspx">
	<input type="hidden" name="_token" value="4DY4GoSaC2j7kPsVOGD6wHn2bJjP6ktiSHKyiuYM"/>
	<input type="hidden" name="pp" value="{&quot;agent&quot;:4,&quot;agent_name&quot;:&quot;\u8c2d\u6817&quot;}" />
		<input name="city1" id="city1" type="hidden" value="" />
			<input name="city1_name" id="city1_name" type="hidden" value="" />
			<input name="city2" id="city2" type="hidden" value="" />
			<input name="city2_name" id="city2_name" type="hidden" value="" />
	</form>


<script>

function city12Select() {

					$("#form_city").submit();
	
}

function city2Select(city2_id) {
	$("#city2").val(city2_id);
	$("#city2_name").val($("#city2_"+city2_id).html());
	$("#form_city").submit();
}

function city2List(pid) {
	$("#city1").val(pid);
	$("#city1_name").val($("#city1_"+pid).html());
	$("#city2").val(0);
	$("[id^=city1_]").attr("class", "");
	$("#city1_"+pid).attr("class", "open");
	$.ajax({
        type:'POST',
        url: 'http://wyx.bzb100.com/wechat/findCity2ListByPid',
        data:{pid:pid},
        dataType:'json',
        headers:{
            'X-CSRF-TOKEN':$('meta[name="_token"]').attr('content')
        },
        success: function(msg) {
        	if (0 == msg.code) {
        		var str = '';
                jQuery.each(msg.list, function (key, value) {
					str += "<li";
					str += " onclick='city2Select(";
					str += value.id;
					str += ")'";
					str += ' id="city2_'+value.id+'"';
					str += ">"+value.name+"</li>";
                })
                $('#city2_list').html(str);
        	}
        }
	})
}

</script>


</body>
</html>
