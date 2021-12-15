

$(function() {

    //全局初始化
    init();
    // 初始化选择控件
    setYear("bjnf", "请选择开工年份");
    setYear("jgnf", "请选择竣工年份");
    setGclx("gclx");
    setStatistics();
    setMapUrl();
    bindChange();
    setAnnounce();
    showWorkList();
});

//全局初始化
function init() {

    var h = $(window).height();
    //鼠标滚动全屏
    $('.panel').css({
        'height': h
    });
    $("#item").height(h - 70);
    $("#item2").height(h - 110);


    $.scrollify({
        section: 'section',
        scrollbars: false,
        before: function() {
            var sectionName = $.scrollify.current()[0].dataset.sectionName;
            if (sectionName == "section1") {
                page = 1;
            }
            if (sectionName == "section2") {
                page = 2;
            }
            if (sectionName == "section3") {
                page = 3;
            }
        }
    });

    //加载固定块(回到顶部按钮)
    // layui.use('util', function () {
    //     var util = layui.util;
    //     util.fixbar({
    //         // bar1: true,
    //         click: function (type) {
    //             console.log(type);
    //             if (type === 'bar1') {
    //                 alert('点击了bar1')
    //             }
    //         }
    //     });
    // });
}

function bindChange() {
    $("#bjnf").change(function () {
        setStatistics();
        setMapUrl();
    });
    $("#jgnf").change(function () {
        setStatistics();
        setMapUrl();
    });
    $("#gczt").change(function () {
        setStatistics();
        setMapUrl();
    });
    $("#gclx").change(function () {
        setStatistics();
        setMapUrl();
    });
}

window.onresize = function() {
    //高度调整
    setHW();
}

//统计图表宽高调整
function setHW() {

    //每个图表的宽度
    // var bottomwidth = ($("#enginner_chart").width() - 60) / 3;
    // $("#enginner_1").css("width", bottomwidth);
    // $("#enginner_2").css("width", bottomwidth);
    // $("#enginner_3").css("width", bottomwidth);
    // $("#enginner_4").css("width", ($(".panel").width() - 80));

    //每个图表的高度
    // var bottomheight = ($(".panel").height() - 140) / 2;
    // $("#enginner_1").css("height", bottomheight);
    // $("#enginner_2").css("height", bottomheight);
    // $("#enginner_3").css("height", bottomheight);
    // $("#enginner_4").css("height", bottomheight);
}


