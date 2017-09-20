<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DraftCustom.aspx.cs" Inherits="Hidistro.UI.Web.DraftCustom" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.SaleSystem.CodeBehind" Assembly="Hidistro.UI.SaleSystem.CodeBehind" %>
<%@ Register TagPrefix="H2" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %><!DOCTYPE html>
<html lang="en-US">
<head>
    <meta charset="UTF-8" />
    <meta content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=1" name="viewport" />
    <meta http-equiv="content-script-type" content="text/javascript">
    <meta name="format-detection" content="telephone=no" />
    <!-- uc强制竖屏 -->
    <meta name="screen-orientation" content="portrait">
    <!-- QQ强制竖屏 -->
    <meta name="x5-orientation" content="portrait">
    <title><%--店铺主页 - --%><%=htmlTitleName %></title>
    <meta name="description" content="<%=Server.HtmlEncode(Desc) %>" />
    <link rel="stylesheet" href="/Admin/Shop/PublicMob/css/dist/style.css?20160816">
    <link rel="stylesheet" href="/Admin/Shop/PublicMob/css/SpsBtn.css">
    <link rel="stylesheet" href="/Admin/Shop/PublicMob/css/Menu.css">
    <link rel="stylesheet" href="<%=cssSrc %>">
    <style type="text/css">/*隐藏美洽客服手机端右下角层*/
         .mc_button {display:none}
         #followtx{width:100%;height:36px; padding:2px 0;max-width:640px;background:#333;position:absolute;line-height:18px;display:none;z-index:999;color:#fff;opacity: 0.9; left:50%; transform:translateX(-50%); -webkit-transform:translateX(-50%); -moz-transform:translateX(-50%);}
         #followtx input{padding:2px 6px; float:right;margin:6px 10px 0 0px;display:block;background:#02b8b1;color:#fff;border:none;}
         #followtx .closeFlow{cursor:pointer;position:absolute;top:8px;left:10px;color:#fff; font-size:24px;}
		 #followtx .little-logo{ width:30px; height:30px; border-radius:50%; border:1px solid #fff; float:left; margin: 2px 10px 0 40px;}
        .icon_buy{display:none!important}
		.textBox{ width:54%; float:left; display:table; height:100%;}
		.textBox em{ display:table-cell; vertical-align:middle;}
		.popup{ width:200px; height:248px; padding:20px; position:fixed; left:50%; top:50%; transform:translate(-50%,-50%); -webkit-transform:translate(-50%,-50%); -moz-transform:translate(-50%,-50%); z-index:9999; background:url(/Utility/pics/popup.png) no-repeat left top / contain; text-align:center;}
		.popup .img{ width:200px; height:200px; overflow:hidden;}
		.popup p{ line-height:20px; font-size:14px; color:#fff;}
		.popup-bg{ width:100%; max-width:640px; height:100%; position:fixed; z-index:9998; left:50%; top:0; background:#000; opacity:0.7; transform:translateX(-50%); -moz-transform:translateX(-50%); -webkit-transform:translateX(-50%);}
		.closeBtn{ width:20px; height:20px; line-height:20px; text-align:center; border:1px solid #fff; border-radius:50%; position:absolute; top:-35px; right:-10px; color:#fff;}
    </style>
    <script src="/Utility/bootflat/js/jquery-1.10.1.min.js"></script>
    <script src="/Utility/WeixinApi.js?v=3.7"></script>
    <script src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
    <H2:weixinset ID ="weixin" runat="server"></H2:weixinset>
    <script type="text/javascript">
        var wxinshare_title = '<%=Server.HtmlEncode(siteName)%>'; var wxinshare_desc = '<%=Server.HtmlEncode(Desc.Replace("\n","").Replace("\r",""))%>';
        var wxinshare_link = window.location.href; var fxShopName = '<%=Server.HtmlEncode(siteName)%>'; var wxinshare_imgurl = '<%=imgUrl%>';
    </script>
<script>
    if (window.top.location != window.location) {
        window.top.location.href = window.location.href;
    }

</script>
</head>
<body style=" ">
    <div class="membersbox pad50 " id ="divCommon" style="max-width:640px; margin:0 auto;position:relative;">
        <Hi:CustomDraftHomePage runat ="server" ID="H_Page" ></Hi:CustomDraftHomePage>
        
    </div>
    <div id="mmexport"><img src="/Admin/Shop/PublicMob/images/mmexport.png" width="100%" alt=""></div>
    <!--关注-->
    <!-- 悬浮按钮 -->
    <div class="mask_menu" id="menubtn" style="display: none;"></div>
    <div class="menu" id="menufloat"><i class="icon-menu"></i></div>
    <div class="menu-c notel nofocus" id="menufloat-c" style="">
        <div class="menu-c-out">
            <div class="menu-c-inner" id ="menuIn">            
            </div>
        </div>
    </div>    
    <!-- 收藏 -->
    <div class="collectbg"><img src="/Admin/Shop/PublicMob/images/collectbg.png" width="100%" alt=""><a href="javascript:;" class="a-know">我知道了</a></div>
    <!-- 分享 -->
    <div class="sharebg"><img src="/Admin/Shop/PublicMob/images/mmexport.png" width="100%" alt=""><a href="javascript:;" class="a-know">我知道了</a></div>   
    <script src="/Admin/Shop/PublicMob/js/dist/lib-min.js"></script>
    <script src="/Admin/Shop/PublicMob/js/dist/main.js?20160420"></script>
    <H2:MeiQiaSet ID ="MeiQiaSet" runat="server"></H2:MeiQiaSet>
    <script src="/templates/common/script/WeiXinShare.js?2016"></script>
    <script>WinXinShareMessage(wxinshare_title, wxinshare_desc, wxinshare_link, wxinshare_imgurl);</script>
    <script src="/Admin/shop/Public/js/dist/underscore.js"></script>
    <% if (DraftIsShowMenu)
       { %>
    <script src="/Utility/ShowIndex.js?20160818"></script>
    <% } %>
    <script src="/Utility/IndexSuspendMenu.js"></script>
    <script src="/Utility/jquery.cookie.js"></script>
    <script src="/Utility/lazyload.js"></script>
    <script>
        $(function () {
            $(".biggoods a.goodsimg img,.members_imgad a.goodsimg img").each(function () {
                var obj = this;
                if ($(obj).attr("data-original")) {
                    $(obj).attr("data-original", $(obj).attr("data-original").replace("thumbs310/310_", "images/"));
                }

            })
            $("img.imgLazyLoading").lazyload();
            /*检查视频参数是否出错*/
            if ($(".diyShowVideo").length > 0) {
                $(".diyShowVideo").each(function () {
                    $(this).find(".diy-video").each(function () {
                        if ($(this).attr("src") != null && $(this).attr("src").indexOf("false") == 0) {
                            $(this).hide();/*如果为false 则隐藏*/
                        }
                    });
                });
            }
            /*点击我要分销功能*/
            $(document).on('click', '#fxBtn', function () {
                $("#mmexport").show();
            });
            $(document).on('touchend', '#fxBtn', function () {
                $('#fxBtn').click();
            })
            $(document).on('touchend', "#mmexport", function () {
                $(this).hide();
            });
            (function () {
                $(".mingoods img").each(function () {
                    if ($(this).attr("src") == "") {
                        $(this).attr("src", "/Utility/pics/none.gif");
                    }
                });

                /*关注检查，没有关注，就引出提示，每天弹一次*/
                //$("#followtx").hide();
                var followtype = "";
                var WeixinfollowUrl = "<%=WeixinfollowUrl%>";
                var AlinfollowUrl = "<%=AlinfollowUrl%>";
                var followurl = "";
                if (/micromessenger/i.test(navigator.userAgent.toLowerCase())) {
                    followtype = "wx"; /*微信端打开*/
                    followurl = WeixinfollowUrl;
                }
                else if (/alipay/i.test(navigator.userAgent.toLowerCase())) {
                    followtype = "fw"; /*服务窗口打开*/
                    followurl = AlinfollowUrl;
                }
                //if (followtype != "" && followurl != "") {
                //setTimeout(function() {
                <%--if ($.cookie(followtype + "follow") == null) {
                    $.post("/api/VshopProcess.ashx",
                    { followtype: followtype, action: "followCheck" }, function (msg) {
                        if (msg.type != 3) {

                            //未关注 开启一键关注 未自动弹出跳转
                            var EnableGuidePageSet = '<%=EnableGuidePageSet%>'.toLowerCase();
                            var IsAutoGuide = '<%=IsAutoGuide%>'.toLowerCase();
                            var IsMustConcern = '<%=IsMustConcern%>'.toLowerCase();
                            if (EnableGuidePageSet == 'true' && IsAutoGuide == 'false') {
                                $("#followtx").show();
                            } else {
                                $("#followtx").hide();
                                if (IsAutoGuide == 'true') {
                                    if (IsMustConcern == 'true') {
                                        $("#FollowMe").trigger("click");
                                        $(".closeBtn").hide();
                                    } else if (EnableGuidePageSet == 'true') {
                                        $("#FollowMe").trigger("click");
                                    }
                                }
                                //if (EnableGuidePageSet == 'true' && IsAutoGuide == 'true') {
                                //    $("#FollowMe").trigger("click");
                                //}                                        
                                //if (IsMustConcern == 'true' && IsAutoGuide == 'true') {

                                //}
                            }

                            //$("#followtx").width($(window).width());
                            //$("#followtx").css("left", $("#divCommon").offset().left + "px");
                            //$(document).on('scroll', function (evt) {
                            //    $("#followtx").css("top", $(document).scrollTop() + "px");
                            //});
                            //$("#followtx").show(300);
                        } else {
                            $("#followtx").hide();
                            $.cookie(followtype + "follow", "true", { expires: 7 }); /*7天再检查提醒一次*/
                        };
                    });
                }--%>
                //}, 1000);
                //}

                //$(".closeFlow").click(function () {
                //    $("#followtx").hide(300);
                //    $.cookie(followtype + "follow", "true"); //每次会话提醒一次
                //    //$.cookie(followtype + "follow", "true", { expires:0.5});/*12小时提醒一次*/
                //});

                /*点击转到相应关注页面*/
                /*$("#FollowMe").click(function () {
                    window.location.href = followurl; 
                });*/

                /*公众号关注检查END
                控制添加商品的图片显示高度，确保商品布局正常*/
                $('.b_mingoods,.mingoods').each(function (index, el) {
                    var me = $(this),
                        imgHeight = me.find('img').width();
                    me.find('img').closest('a').height(imgHeight);
                });
                $('.board3').each(function (index, el) {
                    var me = $(this);
                    var bwidth = me.width();
                    if (me.hasClass('small_board') || !me.hasClass('big_board')) {
                        me.children('span').attr('style', 'height:' + bwidth + 'px !important;overflow:hidden;');
                    }
                    if (me.hasClass('big_board')) {
                        me.children('span').attr('style', 'height:' + (bwidth * 2 + 10) + 'px !important;overflow:hidden;');
                    }
                });
                $('.mdetail_goodsimg ul li').each(function (index, el) {
                    var me = $(this);
                    var imgWidth = me.width();
                    me.height(imgWidth);
                });
                /*悬浮按钮*/
                var SpsBtn = {
                    config: {
                        menu: document.getElementById('menufloat'),
                        menusub: document.getElementById('menufloat-c'),
                        menubtn: document.getElementById('menubtn')
                    },
                    init: function () {
                        this.SpsbtnClick();
                        this.touchMove();
                    },
                    touchMove: function () {
                        var _this = this;
                        _this.config.menu.addEventListener('touchmove', function (e) {
                            e.preventDefault();
                            var touch = e.touches[0],
                                moveX, moveY,
                                winWh = window.innerWidth - 50,
                                winHt = window.innerHeight - 50;
                            moveX = touch.clientX - 25;
                            moveY = touch.clientY - 25;
                            moveY = moveY < 0 ? 0 : moveY;
                            moveX = moveX < 0 ? 0 : moveX;
                            moveY = moveY > winHt ? winHt : moveY;
                            moveX = moveX > winWh ? winWh : moveX;
                            _this.config.menu.style.left = moveX + 'px';
                            _this.config.menu.style.top = moveY + 'px';
                            _this.config.menusub.style.left = moveX + 10 + 'px';
                            _this.config.menusub.style.top = moveY - 190 + 'px';
                        }, false)
                    },
                    SpsbtnClick: function () {
                        var _this = this;
                        this.config.menu.addEventListener('click', function () {
                            var me = $(this);
                            if (!me.hasClass('show')) {
                                me.addClass('show');
                                me.siblings('.mask_menu,#menufloat-c').show();
                                me.siblings('#menufloat-c').find('.menu-c-inner').addClass('in').removeClass('outer')
                            } else {
                                me.removeClass('show');
                                me.siblings('.mask_menu,#menufloat-c').hide();
                                me.siblings('#menufloat-c').find('.menu-c-inner').removeClass('in').addClass('outer')
                            }
                        }, false);
                        this.config.menubtn.addEventListener('click', function () {
                            $(_this.config.menu).removeClass('show');
                            $(_this.config.menu).siblings('.mask_menu,#menufloat-c').hide();
                            $(_this.config.menu).siblings('#menufloat-c').find('.menu-c-inner').removeClass('in').addClass('outer')
                        })
                    }
                }
                SpsBtn.init();
                $('.a-know').click(function (event) {
                    $(this).parent('.collectbg,.sharebg').hide();
                });
            })();
        });
    </script>
    <script type="text/javascript" src="/Admin/Shop/PublicMob/plugins/swipe/swipe.js"></script>
    <script>
        $(function () {
            var t = new Date().getMinutes();
            $.getJSON("/api/Hi_Ajax_GetProductsCount.ashx?t=" + t, function (data) {
                $("#goodsCount").html(data.count);
                if ($("#StoreName").length > 0) {
                    $("#StoreName").html(data.storeName);
                    if (data.logoUrl != "") {
                        $("#imglogo").attr("src", data.logoUrl);
                    }
                }
            })
            $('.j-swipe').each(function (index, el) {
                var me = $(this);
                me.attr('id', 'Swiper' + index);
                var id = me.attr('id');
                var elem = document.getElementById(id);
                window.mySwipe = Swipe(elem, {
                    startSlide: 0,
                    auto: 3000,
                    callback: function (m) {
                        $(elem).find('.members_flash_time').children('span').eq(m).addClass('cur').siblings().removeClass('cur')
                    },
                });
            });
        });
    </script>
    <script type="text/j-template" id="menu">
      <div class="menuNav" id="menuNav">
          <ul id="ul">
                    <# _.each(menuList,function(item){ #>
                    <li>
                   
                          <# if(item.SubMenus.length > 0){ #>
                            <div class="navcontent" data="1">
                            <# if(item.ShopMenuPic.length > 0){ #>
                                <img src="<#= item.ShopMenuPic#>" style="width :20px;height:20px;" />
                            <# } #>
                             <#= item.Name #>
                        
    	                    <div class="childNav">
                            <ul>
                                 <# _.each(item.SubMenus,function(chlitem){ #>
                                    <li><a href="<#= chlitem.Content #>"><#= chlitem.Name #></a></li>
                                 <# }) #>
                           </ul>
                           </div>
                
                        <# } else{ #>
                               <div class="navcontent" data="0">
                                 <a href="<#= item.Content #>">
                               <# if(item.ShopMenuPic.length > 0){ #>
                                <img src="<#= item.ShopMenuPic#>" style="width :20px;height:20px;" />
                                <# } #>
                               <#= item.Name #></a>
                         <# } #>
                        </div>
                    </li>
                    <# }) #>
            </ul>
    </div>
    </script>
</body>
</html>