﻿$(function () {
    Defaults = {
        26: {
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
                        pic: "/PublicMob/images/mob/26banner01.jpg"
                    },
                    {
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "白金早礼",
                        title: "",
                        subtitle: "男装",
                        pic: "/PublicMob/images/mob/26banner01.jpg"
                    },
                    {
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "白金早礼",
                        title: "",
                        subtitle: "童装",
                        pic: "/PublicMob/images/mob/26banner01.jpg"
                    }]
                }
            }],
            LModules: [{
                id: 10,
                type: 8,
                draggable: !0,
                sort: 0,
                content: {
                    dataset: [{
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "首页",
                        title: "店铺主页",
                        pic: "/PublicMob/images/mob/26nev01.png"
                    },
                    {
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "新品",
                        title: "",
                        pic: "/PublicMob/images/mob/26nev02.png"
                    },
                    {
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "热卖",
                        title: "店铺主页",
                        pic: "/PublicMob/images/mob/26nev03.png"
                    },
                    {
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "所有产品",
                        title: "",
                        pic: "/PublicMob/images/mob/26nev04.png"
                    }]
                }
            },
            {
                id: 11,
                type: 13,
                draggable: !0,
                sort: 0,
                content: {
                    layout: "0",
                    dataset: [{
                        linkType: 0,
                        link: "#",
                        title: "导航名称",
                        pic: "/PublicMob/images/mob/26sp01.jpg"
                    },
                    {
                        linkType: 0,
                        link: "#",
                        title: "导航名称",
                        pic: "/PublicMob/images/mob/26sp02.jpg"
                    },
                    {
                        linkType: 0,
                        link: "#",
                        title: "导航名称",
                        pic: "/PublicMob/images/mob/26sp03.jpg"
                    }]
                }
            },
            {
                id: 12,
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
                        pic: "/PublicMob/images/mob/26banner02.jpg"
                    }]
                }
            },
            {
                id: 13,
                type: 13,
                draggable: !0,
                sort: 0,
                content: {
                    layout: "0",
                    dataset: [{
                        linkType: 0,
                        link: "#",
                        title: "导航名称",
                        pic: "/PublicMob/images/mob/26sp04.jpg"
                    },
                    {
                        linkType: 0,
                        link: "#",
                        title: "导航名称",
                        pic: "/PublicMob/images/mob/26sp05.jpg"
                    },
                    {
                        linkType: 0,
                        link: "#",
                        title: "导航名称",
                        pic: "/PublicMob/images/mob/26sp06.jpg"
                    }]
                }
            },
            {
                id: 14,
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
                id: 15,
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
    HiShop.DIY.Unit.event_typeHeader_style26 = function (a, b) {
        var c = b.dom_conitem,
        d = a,
        e = $("#tpl_diy_con_typeHeader_style26").html(),
        f = $("#tpl_diy_ctrl_typeHeader_style26").html(),
        g = function () {
            var a = $(_.template(e, b));
            c.find(".members_head").remove().end().append(a);
            var g = $(_.template(f, b));
            d.empty().append(g),
            HYD.DIY.Unit.event_typeHeader_style26(d, b)
        };
        d.find(".j-modify-bg").click(function () {
            return HYD.popbox.ImgPicker(function (a) {
                b.content.bg = a[0],
                g()
            }),
            !1
        }),
        d.find(".j-modify-photo").click(function () {
            return HYD.popbox.ImgPicker(function (a) {
                b.content.photo = a[0],
                g()
            }),
            !1
        })
    }
});