function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + "; " + expires;
}
//获取cookie
function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1);
        if (c.indexOf(name) != -1) return c.substring(name.length, c.length);
    }
    return "";
}

/// 开启/关闭一键关注
var enableGuidePageSet = "";
/// 是否自动弹出/跳转引导关注
var isAutoGuide = "";
/// 是否要求强制关注
var isMustConcern = "";
/// 关注方式  0:弹出公众号二维码 ;  1:跳转至提醒关注页面
var guideConcernType = "";
/// 引导关注公众号
var guidePageSet = "";
var concernMsg = "";

$(function () {
    /*是否在微信端打开*/
    var followtype = "wx";
    if (/micromessenger/i.test(navigator.userAgent.toLowerCase())) {
        followtype = "wx"; 
    }
    else {
         return;
    }
    //是否已经关闭过
    if (getCookie(followtype + "follow")!="") {
        return;
    }

    //加载css文件
    LoadForceFollowCss();

    ///获取配置文件
    $.post("/api/VshopProcess.ashx",
                   { action: "GetSiteSettings" }, function (msg) {
                       enableGuidePageSet = msg.EnableGuidePageSet;
                       isAutoGuide = msg.IsAutoGuide;
                       isMustConcern = msg.IsMustConcern;
                       guideConcernType = msg.GuideConcernType;
                       guidePageSet = msg.GuidePageSet;
                       concernMsg = msg.ConcernMsg;
                       console.log("关注信息：" + msg.FollowInfo);
                       //alert(msg.FollowInfo);
                       if (msg.FollowInfo != 4) {
                           //加载强制关注层
                           if (enableGuidePageSet) {
                               LoadFollowtx(isMustConcern, enableGuidePageSet);
                               if (isAutoGuide) {
                                   if (guideConcernType == 0) {
                                       OpenFollowMe(true);
                                   }
                                   else {
                                       window.location.href = guidePageSet;
                                   }
                               }
                           } else {
                               if (isMustConcern) {
                                   if (guideConcernType == 0) {
                                       OpenFollowMe(true);
                                   }
                                   else {
                                       window.location.href = guidePageSet;
                                   }
                               }
                           }
                       }
                   });

    $(document).on('click', '#FollowMe', function () {
        OpenFollowMe(false);
    })

    $(document).on('click', ".closeFlow", function () {
        setCookie("closefollow", "true", 1);/*24小时提醒一次*/
        $("#followtx").hide(300);
        setCookie(followtype + "follow", "true", 1);/*24小时提醒一次*/
    });

    //关闭弹出层
    //$(document).on('click', '.closeBtn', function () {
    //    $(".closeFlow").click();
    //    if (isMustConcern != "true") {
    //        $('.popup').remove();
    //        $('.popup-bg').remove();
    //    }
    //})
});

//加载css文件
function LoadForceFollowCss() {
    var link = document.createElement('link');
    link.type = 'text/css';
    link.rel = 'stylesheet';
    link.href = '../../Admin/Shop/PublicMob/css/Followtx.css?20160818';
    document.getElementsByTagName("head")[0].appendChild(link);
}

//弹出二维码窗口
function OpenFollowMe(checkfollow) {
    if (checkfollow) {
        if (getCookie("closefollow")!="") {
            return;
        }
    }

    var isMustConcernHtml = "";
    if (!isMustConcern) {
        isMustConcernHtml = '<div class="closeBtn" onclick="closeConcernLayer()">X</div>';
    }
    concernMsg == '' ? "随时获取优惠资讯" : concernMsg;
    var html = '<div class="popup">' +
                    '<div class="img"><img src="/api/qrcode/" style="width:100%;height:100%"></div>' +
                        '<p style="margin-top:10px">长按图片识别二维码</p>' +
                        '<p style="text-align:center; margin:0 -20px;">' + concernMsg + '</p>' +
                          isMustConcernHtml +
                    '</div>' +
                '<div class="popup-bg" ></div>';
    $('body').append(html);
}

//获取分销商信息
function LoadFollowtx(isMustConcern, enableGuidePageSet) {
   
    var isFollowTw = "";
    if (enableGuidePageSet ) {
        isFollowTw = "block";
    }
    else {
        isFollowTw = "none";
    }

    var closeFlowHtml = "";
    if (!isMustConcern) {
        closeFlowHtml = '<div class="closeFlow" ">×</div>';
    }
    var followtxHtml = '<div id="followtx" style="display:' + isFollowTw + '">'
    + closeFlowHtml
    + '<div class="little-logo">'
    + '<img  id="imgSiteLogo" width="30" height="30" src="' + followWtImgUrl + '" style="border-radius: 50%;" />'
    + '</div>'
    + '<input type="button" id="FollowMe" value="一键关注" />'
    + '<div class="textBox">'
    + '<em>欢迎您加入【' + followWtSiteName + '】</em>'
    + '</div>'
    + '</div>'
    $('body').append(followtxHtml);
}

function closeConcernLayer() {
    $('.popup').remove();
    $('.popup-bg').remove();
    setCookie("closefollow", "true", 1);/*24小时提醒一次*/
}

function closepopupbg() {
    if (!isMustConcern) {
        $('.popup').remove();
        $('.popup-bg').remove();
    }
}


