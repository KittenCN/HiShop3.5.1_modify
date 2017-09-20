﻿$(function () {
    Defaults = {
        24: {
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
                        showtitle: "VERO",
                        title: "",
                        subtitle: "女装",
                        pic: "/PublicMob/images/mob/24banner01.jpg"
                    },
                    {
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "VERO",
                        title: "",
                        subtitle: "男装",
                        pic: "/PublicMob/images/mob/24banner01.jpg"
                    },
                    {
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "VERO",
                        title: "",
                        subtitle: "童装",
                        pic: "/PublicMob/images/mob/24banner01.jpg"
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
                        showtitle: "",
                        title: "",
                        pic: "/PublicMob/images/mob/24nev01.jpg"
                    },
                    {
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "",
                        title: "",
                        pic: "/PublicMob/images/mob/24nev02.jpg"
                    },
                    {
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "",
                        title: "",
                        pic: "/PublicMob/images/mob/24nev03.jpg"
                    },
                    {
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "",
                        title: "",
                        pic: "/PublicMob/images/mob/24nev03.jpg"
                    }]
                }
            },
            {
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
                        pic: "/PublicMob/images/mob/24nev05.jpg"
                    },
                    {
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "",
                        title: "",
                        pic: "/PublicMob/images/mob/24nev06.jpg"
                    },
                    {
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "",
                        title: "",
                        pic: "/PublicMob/images/mob/24nev07.jpg"
                    },
                    {
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "",
                        title: "",
                        pic: "/PublicMob/images/mob/24nev08.jpg"
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
                        showtitle: "VERO",
                        title: "",
                        subtitle: "女装",
                        pic: "/PublicMob/images/mob/24banner02.jpg"
                    }]
                }
            },
            {
                id: 13,
                type: 8,
                draggable: !0,
                sort: 0,
                content: {
                    dataset: [{
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "",
                        title: "",
                        pic: "/PublicMob/images/mob/24cnev01.jpg"
                    },
                    {
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "",
                        title: "",
                        pic: "/PublicMob/images/mob/24cnev02.jpg"
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
                        showtitle: "VERO",
                        title: "",
                        subtitle: "女装",
                        pic: "/PublicMob/images/mob/24banner03.jpg"
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
    HiShop.DIY.Unit.event_typeHeader_style24 = function (a, b) {
        b.dom_conitem,
        $("#tpl_diy_con_typeHeader_style24").html(),
        $("#tpl_diy_ctrl_typeHeader_style24").html()
    }
});