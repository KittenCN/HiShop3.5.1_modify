<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wx_SubmitCharge.aspx.cs" Inherits="Hidistro.UI.Web.Pay.wx_SubmitCharge" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
<script type="text/javascript">

    var CheckValue="<%=CheckValue%>";

    if(CheckValue!=""){
        alert(CheckValue);
        //如果出错时，弹出提示
        location.href = "/vshop/MemberCenter.aspx?status=1";
    }
    else
    {
        document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {
            WeixinJSBridge.invoke('getBrandWCPayRequest', <%= pay_json %>, function(res){
                 if(res.err_msg == "get_brand_wcpay_request:ok" ) {
                     //alert("充值成功!点击确认进入我的金额明细");
                     location.href = "/vshop/MemberAmountList.aspx";
            }
            else
            {
                alert("充值取消或者失败");
                location.href = "/vshop/MemberAmountList.aspx";
            }
            });
        });



    }
   
</script>
</body>
</html>

