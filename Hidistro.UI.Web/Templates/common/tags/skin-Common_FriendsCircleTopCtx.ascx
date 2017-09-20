<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="Hidistro.UI.SaleSystem.CodeBehind" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<div class="title"><a href='<%#Eval("Url")%>'><%# Eval("Title")  %></a></div>
<span class="share_title"><%# Eval("PubTime", "{0:yyyy-MM-dd HH:mm:ss}")%></span>
<a  href='<%# Eval("Url")  %>'>
<img src='<%# Eval("ImageUrl")%>' style="width: 100%" class="img-responsive">
</a>
<div class="mate-ctx clear">
    <%# Eval("Content")%>
</div>
<hr />
