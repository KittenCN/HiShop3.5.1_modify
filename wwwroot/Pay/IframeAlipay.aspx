<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IframeAlipay.aspx.cs" Inherits="Hidistro.UI.Web.Pay.IframeAlipay" %>

<meta name="viewport" content="width=device-width" />
<meta charset='utf-8'>
<meta http-equiv='X-UA-Compatible' content='IE=edge'>
<meta name='viewport' content='width=device-width, initial-scale=1'>
<title>支付宝支付</title>
<style>
    * {
        margin: 0;
        padding: 0;
    }
    iframe {
        border: 0;
    }
</style>
<iframe id="ifmPayUrl" src='<%=IframeUrl %>' width='100%' height='100%'></iframe>