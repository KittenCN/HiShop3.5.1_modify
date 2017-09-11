/**
 * Created by weijin on 2016/9/24.
 */

//提示加载
function prompt(promptCon1) {
    $('body').append('<div class="prompt"><p>' + promptCon1 + '</p></div>');
    if (promptCon1) {

    } else {
        $('.prompt').empty();
    };
    setTimeout("promptClose()", 3000);
}

function promptClose() {
    $('.prompt').remove();
}


//弹窗加载
function Popup(popupCon1) {
    var popupCon = {
        conTitle: '111', //标题字段
        conContent: '400-010-6767', //内容字段
        conStyle: 'left', //内容字段样式。left —— 左  center —— 中  right —— right
        conEsc: '111', //关闭按钮事件
        conConfirm: '222', //确认按钮文本内容
        conEscEvent: 'PopupClose()', //关闭事件
        conConfirmEvent: 'PopupClose()' //确认事件
    }
    popupCon = popupCon1;
    $('body').append(
        '<div class="popup_bg">' +
        '<div class="poput_box">' +
        '<h3 class="poput_title">温馨提示</h3>'+
        '<div class="poput_con">' +
        '<p  style="text-align: ' + popupCon.conStyle + ';">' + popupCon.conContent + '</p>' +
        '<div class="poput_bottom"><input type="button" class="pop_but" value="确 认" onclick="PopupClose()" /></div>'+
        '</div>' +
        '</div>' +
        '</div>');

    var pop_top=parseInt($(document).scrollTop())+parseInt($('.poput_box').height());
    $('.poput_box').attr('style','top:'+pop_top+'px');
}

function PopupClose() {
    $('.popup_bg').remove();
}


//弹窗加载
function h5_Popup(popupCon1) {
    var popupCon = {
        conTitle: '111', //标题字段
        conContent: '400-010-6767', //内容字段
        conStyle: 'left', //内容字段样式。left —— 左  center —— 中  right —— right
        conEsc: '111', //关闭按钮事件
        conConfirm: '222', //确认按钮文本内容
        conEscEvent: 'PopupClose()', //关闭事件
        conConfirmEvent: 'PopupClose()' //确认事件
    }
    popupCon = popupCon1;
    $('body').append(
        '<div class="h5_popup_bg">' +
        '<div class="h5_poput_box">' +
        '<div class="h5_poput_con">' +
        '<h3>' + popupCon.conTitle + '</h3>' +
        '<p style="text-align: ' + popupCon.conStyle + ';">' + popupCon.conContent + '</p>' +
        '<a class="h5_poput_esc" onclick="' + popupCon.conEscEvent + ';">' + popupCon.conEsc + '</a>' +
        '<a class="h5_poput_confirm" onclick="' + popupCon.conConfirmEvent + '">' + popupCon.conConfirm + '</a>' +
        '</div>' +
        '</div>' +
        '</div>');

    var pop_top=parseInt($(document).scrollTop())+parseInt($('.h5_poput_box').height());
    $('.h5_poput_box').attr('style','top:'+pop_top+'px');
}

function h5_PopupClose() {
    $('.h5_popup_bg').remove();
}

//loading
function loading(text) {
    if(text=="") {
        text = '加载中...';
    }
    $('body').append('<div class="loading_bg">' +
        '<div class="loading_box">' +
        '<div class="box">' +
        '<i></i> '+
        '<span class="text">'+text+'</span>'+
        '</div> ' +
        '</div>' +
        '</div>');

    var pop_top=parseInt($(document).scrollTop())+parseInt($('.loading_box').height());
    $('.loading_bg .box').attr('style','top:'+pop_top+'px');
}

function loadingClose() {
    $('.loading_bg').remove();
}

//弹窗显示大图
function img_Popup(url) {
    $('body').append(
        '<div class="img_popup_bg" onclick="img_PopupClose()">' +
        '<div class="img_poput_box">' +
        '<div class="img_poput_con">' +
        '<p><img onclick="img_PopupClose()" src="' + url + '" /></p>' +
        '</div>' +
        '</div>' +
        '</div>');

    var pop_top=parseInt($(document).scrollTop())+parseInt($('.img_poput_box').height());
    $('.img_poput_box').attr('style','top:'+pop_top+'px');
}

function img_PopupClose() {
    $('.img_popup_bg').remove();
}