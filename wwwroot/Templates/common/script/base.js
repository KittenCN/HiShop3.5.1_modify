/**
 * Created by ChengGL on 2016/4/6.
 */
(function () {
    var $html = $('html');
    var $window = $(window);
    var i = $window.width();
    if (i > 720) { i = 720 }
    var htmlfont = i / 720 * 100 + 'px';
    function size() {
        $html.css('font-size', htmlfont);
    }
    $(window).resize(function () {
        size();
    });
    size();
})();

$(".subtext").each(function () {
    if($(this).text().indexOf("-")!=-1){
        $(this).addClass("subgreen");
    }
})
$(".head_nav a").unbind("click").bind("click",function(){
    history.back();
})
$(".input").bind("input propertychange",function(){
    $(".input-wrapper").find(".clear").show();
});
$(".input-wrapper").find(".clear").unbind("click").bind("click",function(){
    $(".input-wrapper").find(".input").val("");
    $(this).hide();
});
$(".submit").unbind("click").bind("click",function(){
    var search=$(".input").val();
    if(search==""){
        $(".search_tips").show();
    }
    else {
        $(".search_tips").hide();
    }
});

$(".transfer-text").change(function(){
    alert('Handler for .blur() called.');
});
