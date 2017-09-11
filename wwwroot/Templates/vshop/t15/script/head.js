﻿$(function () {
    Defaults = {
        22: {
            page: {
                title: "店铺主页"
            },
            PModules: [{
                id: 9,
                type: 9,
                draggable: !1,
                sort: 0,
                content: {
                    showType: 1,
                    dataset: [{
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "白金早礼",
                        title: "",
                        subtitle: "女装",
                        pic: "/PublicMob/images/mob/22banner01.jpg"
                    },
                    {
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "白金早礼",
                        title: "",
                        subtitle: "男装",
                        pic: "/PublicMob/images/mob/22banner01.jpg"
                    },
                    {
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "白金早礼",
                        title: "",
                        subtitle: "童装",
                        pic: "/PublicMob/images/mob/22banner01.jpg"
                    }]
                }
            }],
            LModules: [{
                id: 11,
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
                id: 12,
                type: 8,
                draggable: !0,
                sort: 0,
                content: {
                    dataset: [{
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "首页",
                        title: "店铺主页",
                        pic: "/PublicMob/images/mob/22nev01.png"
                    },
                    {
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "新品",
                        title: "",
                        pic: "/PublicMob/images/mob/22nev02.png"
                    },
                    {
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "热卖",
                        title: "店铺主页",
                        pic: "/PublicMob/images/mob/22nev03.png"
                    },
                    {
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "我的",
                        title: "",
                        pic: "/PublicMob/images/mob/22nev04.png"
                    }]
                }
            },
            {
                id: 13,
                type: 9,
                draggable: !0,
                sort: 0,
                content: {
                    showType: 2,
                    dataset: [{
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "白金早礼",
                        title: "",
                        subtitle: "女装",
                        pic: "/PublicMob/images/mob/22banner02.jpg"
                    }]
                }
            },
            {
                id: 14,
                type: 9,
                draggable: !0,
                sort: 0,
                content: {
                    showType: 2,
                    dataset: [{
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "白金早礼",
                        title: "",
                        subtitle: "女装",
                        pic: "/PublicMob/images/mob/22banner03.jpg"
                    }]
                }
            },
            {
                id: 15,
                type: 9,
                draggable: !0,
                sort: 0,
                content: {
                    showType: 2,
                    dataset: [{
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "白金早礼",
                        title: "",
                        subtitle: "女装",
                        pic: "/PublicMob/images/mob/22banner04.jpg"
                    }]
                }
            },
            {
                id: 16,
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
                id: 17,
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
    HiShop.DIY.Unit.event_typeHeader_style22 = function (a, b) {
        var c = b.dom_conitem,
        d = a,
        e = $("#tpl_diy_con_typeHeader_style22").html(),
        f = $("#tpl_diy_ctrl_typeHeader_style22").html(),
        g = function () {
            var a = $(_.template(e, b));
            c.find(".members_head").remove().end().append(a);
            var g = $(_.template(f, b));
            d.empty().append(g),
            HiShop.DIY.Unit.event_typeHeader_style22(d, b)
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