<%@ Control Language="C#"%>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="Hidistro.Entities" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>

<%--<li onclick="getFreightTemplate('<%#RegionHelper.GetCity((int)Eval("RegionId"))%>')" id='<%#RegionHelper.GetCity((int)Eval("RegionId"))%>'><a href="#" shippingId="<%# Eval("ShippingId")%>" name="<%# Eval("RegionId")%>" briefAddress="<%# Eval("ShipTo")%> &nbsp;<%# Eval("CellPhone")%> &nbsp; <%# Eval("Address")%>" > 

<%#Eval("ShipTo")+" "+ Eval("CellPhone")+" "+RegionHelper.GetFullRegion((int)Eval("RegionId")," ")+" "+ Eval("Address") %>
</a>

</li>--%>

<ul class="dropdown-menu" role="menu">
            <li><a href="javascript:void(0)" name="时间不限">时间不限</a></li>
            <li><a href="javascript:void(0)" name="周一至周五">周一至周五</a></li>
            <li><a href="javascript:void(0)" name="周六、周日及公众假期">周六、周日及公众假期</a></li>
        </ul>
