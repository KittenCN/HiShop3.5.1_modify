$(function () {
    var a;
    var b = $(pageID).val();
    $.ajax({
        url: HiShop.Config.AjaxUrl.getCustomPage,
        type: 'GET',
        dataType: 'text',
        data: {
            id: b
        },
        success: function (data) {
            a = data.length ? $.parseJSON(data) : Defaults[b];
            //$(".j-pagetitle").text(a.page.title);
            //$(".j-pagetitle-ipt").val(a.page.title);
            //$(".j-pagesubtitle-ipt").val(a.page.subtitle);
            //$(".j-pagecustname").val(a.page.pageurl);
            //$("#isshowmenu").attr('checked', a.page.isshowmenu == true ? 'checked' : "");
            _.each(a.PModules,
             function (a, b) {
                 var c = 0 == b ? !0 : !1;
                 HiShop.DIY.add(a, c);
             });
             _.each(a.LModules,
             function (a) {
                 if (a.type != 5) {
                     HiShop.DIY.add(a);
                 } else {
                     HiShop.Goods.GoodsListBind(a);
                 }
             });
             setTimeout(function () {
                 $('.app_inner_content').css('minHeight', $('.diy-phone-outbox').height());
             }, 500);
             drawg();
             events();
        }
    }),
 
    $("#j-savePage").click(function () {
        //HiShop.Convert.ToHtml();
        if ($.trim($(".j-pagetitle-ipt").val()).length < 1) {
            HiTipsShow("请输入页面名称", "error2")
            return;
        }
        if ($.trim($(".j-pagecustname").val()).length < 1) {
            HiTipsShow("请填写页面地址", "error2")
            return;
        }

        if (!/^[a-zA-Z0-9]{1,100}$/.test($(".j-pagecustname").val()))
        {
            HiTipsShow("页面地址英文或数字！", "error2")
            return;
        }
        var rs = HiShop.DIY.Unit.getData();

        return rs ? ($.ajax({
            url: HiShop.Config.AjaxUrl.saveCustomPage,
            type: "post",
            dataType: "json",
            data: {
                content: JSON.stringify(HiShop.DIY.Unit.getData()),
                id: b,
                type:0,
                getGoodGroupUrl: HiShop.Config.CodeBehind.getGoodGroupUrl,
                getGoodUrl: HiShop.Config.CodeBehind.getGoodUrl,
                is_preview: 0
            },
            beforeSend: function () {
                $.jBox.showloading()
            },
            success: function (a) {
                1 == a.status ? ShowMsgAndReUrl("保存成功", true, "/admin/shop/CustomPageManage.aspx?status=0", null) : ShowMsg("对不起，保存失败：" + a.msg, "false"),
                $.jBox.hideloading()
            }
        }), !1) : void 0
    }),
     $("#j-saveDraftPage").click(function () {
         //HiShop.Convert.ToHtml();
         if ($.trim($(".j-pagetitle-ipt").val()).length < 1 ) {
             HiTipsShow("请输入页面名称", "error2")
             return;
         }
         if ($.trim($(".j-pagecustname").val()).length < 1 ) {
             HiTipsShow("请填写页面地址", "error2")
             return;
         }
         var rs = HiShop.DIY.Unit.getData();

         return rs ? ($.ajax({
             url: HiShop.Config.AjaxUrl.saveCustomPage,
             type: "post",
             dataType: "json",
             data: {
                 content: JSON.stringify(HiShop.DIY.Unit.getData()),
                 id: b,
                 type:1,
                 getGoodGroupUrl: HiShop.Config.CodeBehind.getGoodGroupUrl,
                 getGoodUrl: HiShop.Config.CodeBehind.getGoodUrl,
                 is_preview: 0
             },
             beforeSend: function () {
                 $.jBox.showloading()
             },
             success: function (a) {
                 1 == a.status ? ShowMsgAndReUrl("保存成功", true, "/admin/shop/CustomPageManage.aspx?status=1", null) : ShowMsg("对不起，保存失败：" + a.msg, "false"),
                 $.jBox.hideloading()
             }
         }), !1) : void 0
     })
    ,
    $("#j-saveAndPrvPage").click(function () {
        return HiShop.DIY.Unit.getData() ? ($.ajax({
            url: HiShop.Config.AjaxUrl.saveCustomPage,
            type: "post",
            dataType: "json",
            data: {
                content: JSON.stringify(HiShop.DIY.Unit.getData()),
                id: b,
                is_preview: 1,
                getGoodUrl: HiShop.Config.CodeBehind.getGoodUrl,
                getGoodGroupUrl: HiShop.Config.CodeBehind.getGoodGroupUrl
            },
            beforeSend: function () {
                $.jBox.showloading()
            },
            success: function (a) {
                1 == a.status ? (HiShop.hint("success", "恭喜您，保存成功！"), setTimeout(function () {
                    window.open(a.link)
                },
                1e3)) : HiShop.hint("danger", "对不起，保存失败：" + a.msg),
                $.jBox.hideloading()
            }
        }), !1) : void 0
    }),
    $("#j-homeRecover").click(function () {
        var a = ($(this), $(pageID).val());
        return $.jBox.show({
            title: "还原模板",
            content: "确认还原为初始状态吗？",
            btnOK: {
                onBtnClick: function (b) {
                    $.jBox.close(b),
                    $.ajax({
                        url: "/Shop/home_page_recover",
                        type: "post",
                        dataType: "json",
                        data: {
                            id: a,
                            getGoodGroupUrl:HiShop.Config.CodeBehind.getGoodGroupUrl
                        },
                        beforeSend: function () {
                            $.jBox.showloading()
                        },
                        success: function (a) {
                            1 == a.status ? (HiShop.hint("success", "恭喜您，恢复成功！"), setTimeout(function () {
                                window.location.reload()
                            },
                            1e3)) : HiShop.hint("danger", "对不起，恢复失败：" + a.msg),
                            $.jBox.hideloading()
                        }
                    })
                }
            }
        }),
        !1
    })

});