<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<tr>
    <td width="80%;" style="text-align:left;"> 
        <p><%#Eval("GoToUrl").ToString()==""?Eval("IntegralSource"):"<a href=\""+Eval("GoToUrl")+"\"><em class=\"blue\">"+Eval("IntegralSource")+"</em></a>" %></p>
        <p class="ccc"><%#Eval("TrateTime") %></p>
    </td>
    <td width="20%" title="<%#Server.HtmlEncode(Eval("Remark").ToString()) %>"><%#Convert.ToDecimal(Eval("IntegralChange"))>0?"<em class=\"colorg\">+ "+Eval("IntegralChange","{0:f0}")+"</em>":"<em class=\"\">- "+(Convert.ToDecimal(Eval("IntegralChange").ToString())*-1).ToString("F0")+"</em>" %></td>
</tr>
