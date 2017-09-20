﻿$(function () {
    Defaults = {
        11: {
            page: {
                title: "店铺主页"
            },
            PModules: [{
                id: 11,
                type: "Header_style11",
                draggable: !1,
                sort: 0,
                content: {
                    bg: "/PublicMob/images/indexbg/11.jpg",
                    dataset: [{
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "首页",
                        title: "店铺主页",
                        pic: "/PublicMob/images/index10-2.png"
                    },
                    {
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "所有商品",
                        title: "",
                        pic: "/PublicMob/images/index10-3.png"
                    },
                    {
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "新品",
                        title: "店铺主页",
                        pic: "/PublicMob/images/index10-4.png"
                    },
                    {
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "热卖",
                        title: "",
                        pic: "/PublicMob/images/index10-5.png"
                    },
                    {
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "微杂志",
                        title: "",
                        pic: "/PublicMob/images/index10-6.png"
                    },
                    {
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "帮助",
                        title: "",
                        pic: "/PublicMob/images/index10-7.png"
                    }]
                }
            }],
            LModules: []
        }
    },
    HiShop.DIY.Unit.event_typeHeader_style11 = function (a, b) {
        var c = b.dom_conitem,
        d = a,
        e = $("#tpl_diy_con_typeHeader_style11").html(),
        f = $("#tpl_diy_ctrl_typeHeader_style11").html(),
        g = function () {
            var a = $(_.template(e, b));
            c.find(".Header_style_panel").remove().end().append(a);
            var g = $(_.template(f, b));
            d.empty().append(g),
            HiShop.DIY.Unit.event_typeHeader_style11(d, b)
        };
        d.find(".j-modify-bg").click(function () {
            HiShop.popbox.ImgPicker(function (a) {
                b.content.bg = a[0],
                g()
            })
        }),
        d.find(".j-moveup").click(function () {
            var a = $(this).parents("li.ctrl-item-list-li").index();
            if (0 != a) {
                var c = b.content.dataset.slice(a, a + 1)[0];
                b.content.dataset.splice(a, 1),
                b.content.dataset.splice(a - 1, 0, c),
                g()
            }
        }),
        d.find(".j-movedown").click(function () {
            var a = $(this).parents("li.ctrl-item-list-li").index(),
            c = b.content.dataset.length;
            if (a != c - 1) {
                var d = b.content.dataset.slice(a, a + 1)[0];
                b.content.dataset.splice(a, 1),
                b.content.dataset.splice(a + 1, 0, d),
                g()
            }
        }),
        d.find("input[name='navtitle']").change(function () {
            var a = $(this).parents("li.ctrl-item-list-li").index(),
            c = $(this).val();
            b.content.dataset[a].showtitle = c,
            g()
        }),
        d.find(".droplist li").click(function () {
            var a = $(this).parents("li.ctrl-item-list-li").index();
            HiShop.popbox.dplPickerColletion({
                linkType: $(this).data("val"),
                callback: function (c, d) {
                    b.content.dataset[a].title = c.title,
                    b.content.dataset[a].link = c.link,
                    b.content.dataset[a].linkType = d,
                    g()
                }
            })
        }),
        d.find("input[name='customlink']").change(function () {
            var a = $(this).parents("li.ctrl-item-list-li").index();
            b.content.dataset[a].link = $(this).val()
        }),
        d.find(".j-navModifyIcon").click(function () {
            var a = $(this).parents("li.ctrl-item-list-li").index();
            HiShop.popbox.ImgPicker(function (c) {
                b.content.dataset[a].pic = c[0],
                g()
            })
        })
    }
});