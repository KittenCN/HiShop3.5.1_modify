﻿<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
                 <li>
					<h3><%# Eval("StoreName")%>【<%# Eval("GradeName")%>】</h3>
					<div class="userinfobox">
						<div class="userimg">
							<img src="<%# Eval("Logo")%>">
						</div>
						<div class="usertextinfo clearfix">
							<div class="left">
								<p><span class="colorc">用户呢称：</span><%# Eval("UserName")%></p>
								<p><span class="colorc">申请时间：</span><%# Eval("CreateTime","{0:yyyy-MM-dd}")%></p>
								<p><a href='ChirldrenDistributorDetials.aspx?distributorId=<%# Eval("UserId")%>'><span class="colorc">销售总额：</span><span class="colorg">￥<%# Eval("OrderTotal", "{0:F2}")%></span></a></p>
							</div>
							<div class="right1">
								<p><a href='ChirldrenDistributorStores.aspx?UserId=<%# Eval("UserId")%>&gradeId=<%#Eval("GradeId") %>'><span class="colorc">下级分店：</span><span class="colorg"><%# Eval("disTotal")%></span> 家</a></p>
								<p><span class="colorc">下级会员：</span><span><%# Eval("MemberTotal")%></span> 位</p>
								<p><a href="ChirldrenDistributorDetials.aspx?distributorId=<%# Eval("UserId")%>"><span class="colorc">贡献佣金：</span><span class="colorg">￥<%# Eval("commtotal", "{0:F2}")%></span></a></p>
							</div>
						</div>
					</div>
					<span class="left radius"></span>
					<span class="right1 radius"></span>
				</li>
