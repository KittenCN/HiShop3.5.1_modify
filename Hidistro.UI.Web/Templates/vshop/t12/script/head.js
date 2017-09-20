﻿$(function () {
    Defaults = {
        27: {
            page: {
                title: "店铺主页"
            },
            PModules: [],
            LModules: [{
                id: 9,
                type: 9,
                draggable: !0,
                sort: 0,
                content: {
                    showType: 1,
                    dataset: [{
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "白金早礼",
                        title: "",
                        subtitle: "女装",
                        pic: "/PublicMob/images/mob/27banner01.jpg"
                    },
                    {
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "白金早礼",
                        title: "",
                        subtitle: "男装",
                        pic: "/PublicMob/images/mob/27banner01.jpg"
                    },
                    {
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "白金早礼",
                        title: "",
                        subtitle: "童装",
                        pic: "/PublicMob/images/mob/27banner01.jpg"
                    }]
                }
            },
            {
                id: 10,
                type: 12,
                draggable: !0,
                sort: 0,
                content: {
                    layout: "0",
                    marginstyle: "0",
                    dataset: [{
                        linkType: 0,
                        link: "#",
                        title: "导航名称",
                        showtitle: "商城首页",
                        bgColor: "#d54963",
                        fotColor: "#FFF",
                        pic: "/PublicMob/images/ind3_1.png"
                    },
                    {
                        linkType: 0,
                        link: "#",
                        title: "导航名称",
                        showtitle: "正在热卖",
                        bgColor: "#d54963",
                        fotColor: "#FFF",
                        pic: "/PublicMob/images/ind3_2.png"
                    },
                    {
                        linkType: 0,
                        link: "#",
                        title: "导航名称",
                        showtitle: "所有商品",
                        bgColor: "#d54963",
                        fotColor: "#FFF",
                        pic: "/PublicMob/images/ind3_3.png"
                    },
                    {
                        linkType: 0,
                        link: "#",
                        title: "导航名称",
                        showtitle: "我要分销",
                        bgColor: "#d54963",
                        fotColor: "#FFF",
                        pic: "/PublicMob/images/ind3_4.png"
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
                        pic: "/PublicMob/images/mob/27banner02.jpg"
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
                        pic: "/PublicMob/images/mob/27banner03.jpg"
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
                        pic: "/PublicMob/images/mob/27banner04.jpg"
                    }]
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
            }]
        }
    },
    HiShop.DIY.Unit.event_typeHeader_style27 = function (a, b) {
        var c = b.dom_conitem,
        d = a,
        e = $("#tpl_diy_con_typeHeader_style27").html(),
        f = $("#tpl_diy_ctrl_typeHeader_style27").html(),
        g = function () {
            var a = $(_.template(e, b));
            c.find(".members_head").remove().end().append(a);
            var g = $(_.template(f, b));
            d.empty().append(g),
            HiShop.DIY.Unit.event_typeHeader_style27(d, b)
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