<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<div class="rollCollar" CouponId="<%# Eval("CouponId") %>" IsAllProduct="<%# Eval("CouponId") %>" >
                <a style="height: 140px;" href="javascript:void(0)">
                    <div class="left" style="display:table;">
                        <div style="display:table-cell; vertical-align:middle; text-align:center;">
                            <span style="font-size:30px">￥<%--<i>--%><%# Eval("CouponValue","{0:F0}") %><%--</i>--%></span>
                            <span><%# Eval("CouponName") %></span>
                        </div>
                      <%--  <span style="font-size:16px"><%# Eval("CouponName") %></span>--%>
                    </div>
                    <div class="pright">
                        <div class="pright_text" style="margin-top:-17px">
                        <h5><%# Eval("useConditon") %></h5>             
                        <%#Convert.ToBoolean(Eval("IsAllProduct"))?"":"<p style='margin-left:-5px;font-size:14px;margin-top:-10px'>（指定商品可用）</p>" %>
                        <p>生效时间：<%# Eval("BeginDate","{0:yyyy-MM-dd HH:mm:ss}") %></p>
                        <p>到期时间：<%# Eval("EndDate","{0:yyyy-MM-dd HH:mm:ss}") %></p>
                        </div>
                    </div>
                </a>
</div>
