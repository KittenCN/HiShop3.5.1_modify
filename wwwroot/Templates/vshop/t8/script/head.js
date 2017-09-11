﻿$(function () {
    Defaults = {
        14: {
            page: {
                title: "店铺主页"
            },
            PModules: [{
                id: 14,
                type: "Header_style14",
                draggable: !1,
                sort: 0,
                content: {
                    bg: "/PublicMob/images/indexbg/14.jpg",
                    dataset: [{
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "进入首页",
                        title: "店铺主页"
                    },
                    {
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "新品上架",
                        title: ""
                    },
                    {
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "热卖狂购",
                        title: "店铺主页"
                    },
                    {
                        link: "/Shop/index",
                        linkType: 6,
                        showtitle: "促销甩卖",
                        title: ""
                    }]
                }
            }],
            LModules: []
        }
    },
    HiShop.DIY.Unit.event_typeHeader_style14 = function (a, b) {
        var c = b.dom_conitem,
        d = a,
        e = $("#tpl_diy_con_typeHeader_style14").html(),
        f = $("#tpl_diy_ctrl_typeHeader_style14").html(),
        g = function () {
            var a = $(_.template(e, b));
            c.find(".Header_style_panel").remove().end().append(a);
            var g = $(_.template(f, b));
            d.empty().append(g),
            HiShop.DIY.Unit.event_typeHeader_style14(d, b)
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
        })
    }
});