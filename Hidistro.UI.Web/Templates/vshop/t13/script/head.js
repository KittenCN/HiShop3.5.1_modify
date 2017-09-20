﻿$(function () {
    Defaults = {
        13: {
            page: {
                title: "店铺主页"
            },
            PModules: [{
                id: 13,
                type: "Header_style13",
                draggable: !1,
                sort: 0,
                content: {
                    bg: "/PublicMob/images/indexbg/13.jpg",
                    photo: "/PublicMob/images/header2.jpg",
                    dataset: [{
                        link: "/Shop/index",
                        linkType: 6,
                        subtitle: "新品尝鲜",
                        showtitle: "NEW",
                        title: "店铺主页",
                        pic: "/PublicMob/images/index13-2.jpg",
                        bgColor: "#ffba00",
                        picname: "活动一"
                    },
                    {
                        link: "/Shop/index",
                        linkType: 6,
                        subtitle: "热销商品",
                        showtitle: "HOT",
                        title: "",
                        pic: "/PublicMob/images/index13-3.jpg",
                        bgColor: "#ed5a5a",
                        picname: "活动二"
                    },
                    {
                        link: "/Shop/index",
                        linkType: 6,
                        subtitle: "促销活动",
                        showtitle: "SALE",
                        title: "店铺主页",
                        pic: "/PublicMob/images/index13-4.jpg",
                        bgColor: "#00a5e7",
                        picname: "活动三"
                    },
                    {
                        link: "/Shop/index",
                        linkType: 6,
                        subtitle: "帮助中心",
                        showtitle: "HELP",
                        title: "",
                        pic: "/PublicMob/images/index13-5.jpg",
                        bgColor: "#fe5a00",
                        picname: "活动四"
                    }]
                }
            }],
            LModules: []
        }
    },
    HiShop.DIY.Unit.event_typeHeader_style13 = function (a, b) {
        var c = b.dom_conitem,
        d = a,
        e = $("#tpl_diy_con_typeHeader_style13").html(),
        f = $("#tpl_diy_ctrl_typeHeader_style13").html(),
        g = function () {
            var a = $(_.template(e, b));
            c.find(".members_head").remove().end().append(a);
            var g = $(_.template(f, b));
            d.empty().append(g),
            HiShop.DIY.Unit.event_typeHeader_style13(d, b)
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
        d.find(".ctrl-item-list-add").click(function () {
            var a = {
                link: "/Shop/index",
                linkType: 6,
                subtitle: "新品尝鲜",
                showtitle: "NEW",
                title: "链接到店铺主页",
                pic: "/PublicMob/images/index13-2.jpg",
                bgColor: "#ffba00",
                picname: "活动一"
            };
            b.content.dataset.push(a),
            g()
        }),
        d.find(".j-del").click(function () {
            var a = $(this).parents("li.ctrl-item-list-li").index();
            b.content.dataset.splice(a, 1),
            g()
        }),
        d.find("input[name='showtitle'],input[name='subtitle'],input[name='picname']").change(function () {
            var a = $(this).parents("li.ctrl-item-list-li").index(),
            c = $(this).val(),
            d = $(this).attr("name");
            b.content.dataset[a][d] = c,
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
        }),
        d.find("select[name='navbgColor']").change(function () {
            var a = $(this).parents("li.ctrl-item-list-li").index(),
            c = $(this).val();
            b.content.dataset[a].bgColor = c,
            g()
        })
    }
});