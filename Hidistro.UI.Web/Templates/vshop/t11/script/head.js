﻿$(function () {
    Defaults = {
        9: {
            page: {
                title: "店铺主页"
            },
            PModules: [{
                id: 9,
                type: "Header_style9",
                draggable: !1,
                sort: 0,
                content: {
                    bg: "/PublicMob/images/indexbg/09.jpg",
                    photo: "/PublicMob/images/index9-4.png",
                    dataset: [{
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "首页",
                        title: "店铺主页",
                        pic: "/PublicMob/images/index5-2.png"
                    },
                    {
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "新品",
                        title: "",
                        pic: "/PublicMob/images/index5-3.png"
                    },
                    {
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "热卖",
                        title: "店铺主页",
                        pic: "/PublicMob/images/index5-4.png"
                    },
                    {
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "联系我们",
                        title: "",
                        pic: "/PublicMob/images/index5-5.png"
                    }]
                }
            }],
            LModules: [{
                id: 8,
                type: 9,
                draggable: !0,
                sort: 0,
                content: {
                    showType: 1,
                    dataset: [{
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "VERO",
                        title: "",
                        subtitle: "女装",
                        pic: "/PublicMob/images/mob/09banner02.jpg"
                    },
                    {
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "VERO",
                        title: "",
                        subtitle: "男装",
                        pic: "/PublicMob/images/mob/09banner02.jpg"
                    },
                    {
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "VERO",
                        title: "",
                        subtitle: "童装",
                        pic: "/PublicMob/images/mob/09banner02.jpg"
                    }]
                }
            },
            {
                id: 9,
                type: 8,
                draggable: !0,
                sort: 0,
                content: {
                    dataset: [{
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "",
                        title: "",
                        pic: "/PublicMob/images/mob/09banner03.jpg"
                    }]
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
    HiShop.DIY.Unit.event_typeHeader_style9 = function (a, b) {
        var c = b.dom_conitem,
        d = a,
        e = $("#tpl_diy_con_typeHeader_style9").html(),
        f = $("#tpl_diy_ctrl_typeHeader_style9").html(),
        g = function () {
            var a = $(_.template(e, b));
            c.find(".membersbox").remove().end().append(a);
            var g = $(_.template(f, b));
            d.empty().append(g),
            HiShop.DIY.Unit.event_typeHeader_style9(d, b)
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