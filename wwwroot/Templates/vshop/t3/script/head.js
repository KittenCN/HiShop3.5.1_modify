﻿$(function () {
    Defaults = {
        1: {
            page: {
                title: "店铺主页"
            },
            PModules: [{
                id: 1,
                type: "Header_style1",
                draggable: !1,
                sort: 0,
                content: {
                    bg: "/Admin/shop/PublicMob/images/indexbg/01.jpg",
                    photo: "/Admin/shop/PublicMob/images/header2.jpg"
                }
            }],
            LModules: [{
                id: 9,
                type: 6,
                draggable: !0,
                sort: 0,
                content: {
                    layout: 1,
                    showPrice: !0,
                    showIco: !0,
                    showName: !0,
                    goodslist: []
                }
            },
            {
                id: 10,
                type: 4,
                draggable: !0,
                sort: 0,
                content: {
                    layout: 1,
                    showPrice: !0,
                    showIco: !0,
                    showName: !0,
                    goodslist: []
                }
            },
            {
                id: 11,
                type: 4,
                draggable: !0,
                sort: 0,
                content: {
                    layout: 1,
                    showPrice: !0,
                    showIco: !0,
                    showName: !0,
                    goodslist: []
                }
            }]
        }
    },
    HiShop.DIY.Unit.event_typeHeader_style1 = function (a, b) {
        var c = b.dom_conitem,
        d = a,
        e = $("#tpl_diy_con_typeHeader_style1").html(),
        f = $("#tpl_diy_ctrl_typeHeader_style1").html(),
        g = function () {
            var a = $(_.template(e, b));
            c.find(".members_head").remove().end().append(a);
            var g = $(_.template(f, b));
            d.empty().append(g),
            HiShop.DIY.Unit.event_typeHeader_style1(d, b)
        };
        d.find(".j-modify-bg").click(function () {
            return HiShop.popbox.ImgPicker(function (a) {
                b.content.bg = a[0],
                g()
            }),
            !1
        }),
        d.find(".j-modify-photo").click(function () {
            return HiShop.popbox.ImgPicker(function (a) {
                b.content.photo = a[0],
                g()
            }),
            !1
        })
    }
});