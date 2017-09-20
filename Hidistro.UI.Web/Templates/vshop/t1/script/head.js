﻿$(function () {
    Defaults = {
        30: {
            page: {
                title: "店铺主页"
            },
            PModules: [{
                id: 10,
                type: 9,
                draggable: !1,
                sort: 0,
                content: {
                    showType: 1,
                    dataset: [{
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "VERO",
                        title: "",
                        subtitle: "女装",
                        pic: "/PublicMob/images/mob/30banner01.jpg"
                    },
                    {
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "VERO",
                        title: "",
                        subtitle: "男装",
                        pic: "/PublicMob/images/mob/30banner01.jpg"
                    },
                    {
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "VERO",
                        title: "",
                        subtitle: "童装",
                        pic: "/PublicMob/images/mob/30banner01.jpg"
                    }]
                }
            }],
            LModules: [{
                id: 11,
                type: 8,
                draggable: !0,
                sort: 0,
                content: {
                    dataset: [{
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "",
                        title: "",
                        pic: "/PublicMob/images/mob/30nev01.jpg"
                    },
                    {
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "",
                        title: "",
                        pic: "/PublicMob/images/mob/30nev02.jpg"
                    }]
                }
            },
            {
                id: 12,
                type: 13,
                draggable: !0,
                sort: 0,
                content: {
                    layout: "0",
                    dataset: [{
                        linkType: 0,
                        link: "#",
                        title: "导航名称",
                        pic: "/PublicMob/images/mob/30sp01.jpg"
                    },
                    {
                        linkType: 0,
                        link: "#",
                        title: "导航名称",
                        pic: "/PublicMob/images/mob/30sp02.jpg"
                    },
                    {
                        linkType: 0,
                        link: "#",
                        title: "导航名称",
                        pic: "/PublicMob/images/mob/30sp03.jpg"
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
                        showtitle: "VERO",
                        title: "",
                        subtitle: "女装",
                        pic: "/PublicMob/images/mob/30banner02.jpg"
                    }]
                }
            },
            {
                id: 14,
                type: 8,
                draggable: !0,
                sort: 0,
                content: {
                    dataset: [{
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "",
                        title: "",
                        pic: "/PublicMob/images/mob/30spnev01.jpg"
                    },
                    {
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "",
                        title: "",
                        pic: "/PublicMob/images/mob/30spnev02.jpg"
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
    HiShop.DIY.Unit.event_typeHeader_style30 = function (a, b) {
        b.dom_conitem,
        $("#tpl_diy_con_typeHeader_style30").html(),
        $("#tpl_diy_ctrl_typeHeader_style30").html()
    }
});