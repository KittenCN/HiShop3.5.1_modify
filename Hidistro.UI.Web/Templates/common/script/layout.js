/**
 * Created by Administrator on 2016/5/31.
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
