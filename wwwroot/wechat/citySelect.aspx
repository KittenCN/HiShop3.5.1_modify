<%@ Page Language="C#" AutoEventWireup="true" CodeFile="citySelect.aspx.cs" Inherits="citySelect" %>

<!DOCTYPE html>
<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
<meta charset="UTF-8">
<meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0" name="viewport">
<meta content="yes" name="apple-mobile-web-app-capable">
<meta content="black" name="apple-mobile-web-app-status-bar-style">
<meta content="telephone=no" name="format-detection">
<title>所属城市</title>

<link rel="stylesheet" href="/wechat/citySelect/cityPicker.css">
<script type="text/javascript">
function stops(){
   return false;
}
document.oncontextmenu=stops;
</script>
</head>
<body>
<input type="text" class="city" readonly>
<script src="/wechat/citySelect/jquery-2.1.4.min.js"></script>
<script src="/wechat/citySelect/cityPicker.js"></script> 
<script>
    $(".city").CityPicker();
</script>
</body>
</html> 
