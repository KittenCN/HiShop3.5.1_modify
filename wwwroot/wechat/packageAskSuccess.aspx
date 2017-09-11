<%@ Page Language="C#" AutoEventWireup="true" CodeFile="packageAskSuccess.aspx.cs" Inherits="wechat_packageAskSuccess" %>

<!DOCTYPE html>
<html lang="en" style="font-size: 100px;">
<head>
	<meta charset="utf-8">
	<meta http-equiv="X-UA-Compatible" content="IE=edge">
	<title>订单核算中</title>
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
</head>
<body>
<div class="accounting bg_ff">
	<div class="accounting_img">
		<img src="/2.0/images/accounting.gif"/>
	</div>
	<div class="accounting_text">
		核 算 中...
		<p>报价及积分返还信息稍后将发送到您的微信中<br/>（仅法定工作日提供价格核算服务）</p>
	</div>
	<div class="save_bottom">
		<input type="button" onclick="WeixinJSBridge.call('closeWindow')" class="save_but" value="返回公众号">
	</div>
</div>

<script>

$(document).ready(function(){
	checkDay();
});

function checkDay() {
		if (6 == 5 || 7 == 5) {
		var popupCon = {
			conTitle: '温馨提示', //标题字段
			conContent: '由于保险公司周六日休息，不能进行报价，您的询价请求将在周一上班后处理', //内容字段
			conStyle: 'center', //内容字段样式。left —— 左  center —— 中  right —— right
			conEsc: '关闭', //关闭按钮事件
			conConfirm: '确定', //确认按钮文本内容
			conEscEvent: 'h5_PopupClose()', //关闭事件
			conConfirmEvent: 'h5_PopupClose()' //确认事件
		}
		Popup(popupCon);
	}
}

</script>

</body>
</html>
