<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetReset.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Settings.SetReset" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="margin:20px;"><asp:Literal ID="litMsg" runat="server"></asp:Literal></div>
        <div>
            <p>
                <asp:Button ID="btnReset1" runat="server" Text="重置网站的首页模版数据" OnClick="btnReset_Click" />
            </p>
            <p>
                <asp:Button ID="Button1" runat="server" Text="重置管理员为admin" OnClick="Button1_Click" />
            </p>
            <p>
                <asp:Button ID="btnReset2" runat="server" Text="重置页面导航数据" OnClick="btnReset2_Click" />
            </p>
            <p>
                <asp:Button ID="btnReset3" runat="server" Text="重置微信配置信息" OnClick="btnReset3_Click"/>
            </p>
            <p>
                <asp:Button ID="btnReset4" runat="server" Text="重置商品信息" OnClick="btnReset4_Click"/>
            </p>
            <p><a href="/" target="_blank">预览网站</a></p>
        </div>
    </form>
</body>
</html>
