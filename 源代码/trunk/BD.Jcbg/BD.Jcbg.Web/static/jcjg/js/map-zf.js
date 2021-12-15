function initMap() {
    //地图相关
    map = new BMap.Map("allMap", {
        enableMapClick: false
    });

    $("#allMap").css("height", $("#panel1").height());

    //地图加载控件
    var mapType1 = new BMap.MapTypeControl({
        // mapTypes: [BMAP_NORMAL_MAP, BMAP_HYBRID_MAP]
    });
    var mapType2 = new BMap.MapTypeControl({
        anchor: BMAP_ANCHOR_TOP_RIGHT,
        offset: new BMap.Size(10, 50)
    });

    // var overView = new BMap.OverviewMapControl();
    var overViewOpen = new BMap.OverviewMapControl({
        isOpen: true,
        anchor: BMAP_ANCHOR_BOTTOM_RIGHT
    });
    //添加地图类型和缩略图
    map.addControl(mapType2); //左上角，默认地图控件

    var top_right_navigation = new BMap.NavigationControl({
        anchor: BMAP_ANCHOR_TOP_RIGHT,
        type: BMAP_NAVIGATION_CONTROL_SMALL,
        offset: new BMap.Size(50, 80)
    }); //右上角，仅包含平移和缩放按钮
    map.addControl(top_right_navigation);

    //地图中心
    getBoundary("浙江省台州市");

    //展开所有点击显示隐藏
    $("#allprograme").click(function() {
        var moresearch = $("#moresearch");
        moresearch.toggle();
        var str = moresearch.is(":hidden") ? "展开所有" :"隐藏列表" ; 
        $(this).html(str);
    });

    //展开所有地图显示在下方
    // var topheight = 40;
    var topheightbottom = 80;
    // $("#allprograme").css({"top", 70);/
    // $("#moresearch").css("top", topheightbottom + 5);
    $("#moresearch").css({
        top:"110px",
        left :"40px"
    });
}

//获取行政区域
function getBoundary(getcity) {
    return;
    if (!getcity) {
        return;
    }
    var bdary = new BMap.Boundary();
    bdary.get(getcity, function(rs) { //获取行政区域
        map.centerAndZoom(getcity, 11); //放大地图
        //map.clearOverlays();
        var count = rs.boundaries.length; //行政区域的点有多少个
        if (count === 0) {
            alert('未能获取当前输入行政区域');
            return;
        }

    });
}

//地图面板初始化
function mapInit() {

    //清空覆盖物
    map.clearOverlays();
    $(".BMap_noprint").hide();
    //增加地图操作按钮
    var mapType = new BMap.MapTypeControl({
        anchor: BMAP_ANCHOR_TOP_RIGHT,
        offset: new BMap.Size(10, 50)
    });
    map.addControl(mapType);
    var top_right_navigation = new BMap.NavigationControl({
        anchor: BMAP_ANCHOR_TOP_RIGHT,
        type: BMAP_NAVIGATION_CONTROL_SMALL,
        offset: new BMap.Size(50, 80)
    }); //右上角，仅包含平移和缩放按钮
    map.addControl(top_right_navigation);

    formChange();

    if (tabId == "GcList") {
        gc_mapstyle();
    }
    if (tabId == "QyList") {
        qy_mapstyle();
    }
    if (tabId == "RyList") {
        ry_mapstyle();
    }
}

//工程图例
function gc_mapstyle() {
    function ShowLegendControl() {
        this.defaultAnchor = BMAP_ANCHOR_BOTTOM_RIGHT;
        this.defaultOffset = new BMap.Size(5, 80); // 默认偏移量
    }
    ShowLegendControl.prototype = new BMap.Control();
    ShowLegendControl.prototype.initialize = function(map) {
        // 创建一个DOM元素
        var ul = document.createElement("ul");
        //console.log(ul1);BMap_noprint anchorBR
        ul.style.background = "White";
        ul.style.padding = "15px";
        ul.style.opacity = " 1";
        var li = $("<li id='construcstyle'>在建工程<img src='/skins/default/welcomezs/images/zj.png'/></li>").appendTo(ul);
        var li = $("<li id='construcstyle'>竣工工程<img src='/skins/default/welcomezs/images/jg.png'/></li>").appendTo(ul);
        var li = $("<li id='construcstyle'>停工工程<img src='/skins/default/welcomezs/images/tg.png'/></li>").appendTo(ul);
        // 添加DOM元素到地图中
        map.getContainer().appendChild(ul);
        // 将DOM元素返回
        return ul;

    }
    // 创建控件
    var showLegendCtrl = new ShowLegendControl();
    // 添加到地图当中
    map.addControl(showLegendCtrl);
}

//单位图例
function qy_mapstyle() {
    function ShowLegendControl() {
        this.defaultAnchor = BMAP_ANCHOR_BOTTOM_RIGHT;
        this.defaultOffset = new BMap.Size(5, 80); // 默认偏移量
    }
    ShowLegendControl.prototype = new BMap.Control();
    ShowLegendControl.prototype.initialize = function(map) {
        // 创建一个DOM元素
        var ul = document.createElement("ul");
        ul.style.background = "White";
        ul.style.padding = "15px";
        ul.style.opacity = " 1";
        var li = $("<li id='construcstyle'>企业<img src='/skins/default/welcomezs/images/qy.png'/></li>").appendTo(ul);
        // 添加DOM元素到地图中
        map.getContainer().appendChild(ul);
        // 将DOM元素返回
        return ul;

    }
    // 创建控件
    var showLegendCtrl = new ShowLegendControl();
    // 添加到地图当中
    map.addControl(showLegendCtrl);
}

//人员图例
function ry_mapstyle() {
    function ShowLegendControl() {
        this.defaultAnchor = BMAP_ANCHOR_BOTTOM_RIGHT;
        this.defaultOffset = new BMap.Size(5, 80); // 默认偏移量
    }
    ShowLegendControl.prototype = new BMap.Control();
    ShowLegendControl.prototype.initialize = function(map) {
        // 创建一个DOM元素
        var ul = document.createElement("ul");
        ul.style.background = "White";
        ul.style.padding = "15px";
        ul.style.opacity = "1";
        var li = $("<li id='construcstyle'>在职<img src='/skins/default/welcomezs/images/zz.png'/></li>").appendTo(ul);
        var li = $("<li id='construcstyle'>离职<img src='/skins/default/welcomezs/images/lz.png'/></li>").appendTo(ul);
        // 添加DOM元素到地图中
        map.getContainer().appendChild(ul);
        // 将DOM元素返回
        return ul;

    }
    // 创建控件
    var showLegendCtrl = new ShowLegendControl();
    // 添加到地图当中
    map.addControl(showLegendCtrl);
}

//获取工程一览
function getGcListForMap() {

    var gcmc = $("#txtGcmc").val();
    var pageindex = $("#gcpageindex").val();

    // var obj = Object.assign(paramsData, {
    //     gcmc: gcmc,
    //     page: pageindex,
    // });
    var obj ={};
    jQuery.extend(obj ,paramsData ,{
        gcmc: gcmc,
        page: pageindex,
    });

    ajaxTpl("/welcome/GetGcList", obj, function(result) {

        var gclist = result.data;

        //全部工程累加
        gclistAll = gclistAll.concat(gclist);

        //展开更多工程一览显示,地图标识
        setGclistForMap(gclist);

        //刷新标志设为false
        refreshmaptabflg1 = false;
    });
}

//展开更多工程一览显示
function setGclistForMap(gclist) {
    var listdata = gclist;
    var singledata = "";
    $.each(listdata, function(index, value) {
        singledata += "<div class='gcsinglelist clearfix'><div class='singlelist_left'><p>工程名称:" + value.gcmc + "</p><p>工程地点:" + value.gcdd + "</p><p>工程状态:" + value.sy_gczt + "</p></div></div>"
    });
    $("#listcontent").append(singledata);
    addMarkerGclist(listdata);
    $(".gcsinglelist").click(function() {
        var singleclick = [];
        var num = $(this).index();
        singleclick.push(gclistAll[num]);
        addMarkerGclist(singleclick);
    });
}

//在地图上表示出来的点
function addMarkerGclist(points) {
    for (var i = 0; i < points.length; i++) {
        var gcbz = points[i].gcbz;
        var jd;
        var wd;
        if (gcbz == "") {
            return;
            //gcbz = "120.709955,28.016725";
        }
        if (gcbz != "") {
            jd = gcbz.split(",")[0];
            wd = gcbz.split(",")[1];

            var point = new BMap.Point(jd, wd);
            if (points[i].sy_gczt == "在建工程") {
                var myIcon = new BMap.Icon("/skins/default/welcomezs/images/zj.png", new BMap.Size(47.5, 47.5));
            } else if (points[i].sy_gczt == "竣工工程") {
                var myIcon = new BMap.Icon("/skins/default/welcomezs/images/jg.png", new BMap.Size(47.5, 47.5));
            } else {
                var myIcon = new BMap.Icon("/skins/default/welcomezs/images/tg.png", new BMap.Size(47.5, 47.5));
            }

            var marker = new BMap.Marker(point, {
                icon: myIcon
            });
            map.addOverlay(marker);

            //单个坐标的话设置为中心,并显示详细
            if (points.length == 1) {
                map.centerAndZoom(point, 12);
                // showInfoGc(marker, points[0]);
            } else if (i == 0) {
                // showInfoGc(marker, points[0]);
            }

            (function() {
                var infopoint = points[i];
                var gcbh = points[i].gcbh;
                marker.addEventListener("onclick", function(e) {
                    showInfoGc(this, infopoint);
                });
            })();
        }

    }
}

//点击不同的图标获取显示不同的值
function showInfoGc(thisMaker, point) {
    var opts = {
        width: 500, // 信息窗口宽度
        height: 250, // 信息窗口高度
    }
    var enginnersingle = " ";
    enginnersingle += "<div id='enginner_bigbox'> ";
    enginnersingle += "<p class='enginner_bigbox_title'>工程名称：" + point.gcmc + "</p> ";
    enginnersingle += "<div id='enginner_bigbox_left'> ";
    enginnersingle += "<p class='enginner_bigbox_all'>建设单位：" + point.sy_jsdwmc + "</p> ";
    enginnersingle += "<p class='enginner_bigbox_all'>施工单位：" + point.sgdwmc + "</p> ";
    enginnersingle += "<p class='enginner_bigbox_all'>计划开工日期：" + point.sy_jhkgrq + "</p> ";
    enginnersingle += "<p class='enginner_bigbox_all'>计划竣工日期：" + point.sy_jhjgrq + "</p> ";
    enginnersingle += "<p class='enginner_bigbox_all'>人员情况：在册：" + point.zcrs + "人&nbsp;&nbsp;在职：" + point.zzrs + "人&nbsp;&nbsp;在岗：" + point.kqrs + "人</p> ";
    enginnersingle += "<p class='enginner_bigbox_all'>工资发放：计划：" + (point.gczj * 0.3) + "亿元&nbsp;&nbsp;到位：" + "0亿元" + "&nbsp;&nbsp;发放：" + "0亿元" + "</p> ";
    enginnersingle += "<span id='pepple_button_left'>查看详细</span> ";
    enginnersingle += "</div> ";
    enginnersingle += "</div> ";
    var infoWindow = new BMap.InfoWindow(enginnersingle, opts);
    var addnumber = point;
    infoWindow.addEventListener("open", function(type, target, point) {
        $("#pepple_button_left").bind('click', function() {
            togczsformap(addnumber.gcbh, addnumber.gcmc);
        });
    })
    thisMaker.openInfoWindow(infoWindow);
}

//获取企业一览
function getQyListForMap() {


    var qymc = $("#txtQymc").val();
    var pageindex = $("#qypageindex").val();


    // var obj = Object.assign(paramsData, {
    //     qymc: qymc,
    //     page: pageindex,
    // });
var obj = {};
    jQuery.extend(obj,paramsData,{
        qymc: qymc,
        page: pageindex,
    });

    ajaxTpl("/welcome/GetQyList", obj, function(result) {
        var qylist = result.data;

        //全部工程累加
        qylistAll = qylistAll.concat(qylist);

        //展开更多企业一览显示,地图标识
        setQylistForMap(qylist);

        //刷新标志设为false
        refreshmaptabflg2 = false;
    });
}

//展开更多企业一览显示
function setQylistForMap(qylist) {
    var listdata = qylist;
    var singledata = "";
    $.each(qylist, function(index, value) {
        singledata += "<div class='qysinglelist clearfix'><div class='singlelist_left'><p>企业名称:" + value.qymc + "</p><p>企业地点:" + (value.szsf + value.szcs + value.szxq) + "</p><p>企业类型:" + value.lxmc + "</p><p>是否黑名单:" + (typeof(value.sfhmd) == "undefined" ? "否" : "是") + "</p></div></div>"
    });
    $("#qycontent").append(singledata);
    addMarkerQylist(qylist);
    $(".qysinglelist").click(function() {
        var singleclick = [];
        var num = $(this).index();
        singleclick.push(qylistAll[num]);
        addMarkerQylist(singleclick);
    });
}

//在地图上表示出来的点
function addMarkerQylist(points) {
    for (var i = 0; i < points.length; i++) {
        if (points[i].lxbh != "") {

            var jd = points[i].lon;;
            var wd = points[i].lat;
            if (jd == "") {
                var jd = "120.709955";
                var wd = "28.016725";
            }

            var point = new BMap.Point(jd, wd);
            var myIcon = new BMap.Icon("/skins/default/welcomezs/images/qy.png", new BMap.Size(47.5, 47.5));
            var marker = new BMap.Marker(point, {
                icon: myIcon
            });
            map.addOverlay(marker);

            //单个坐标的话设置为中心,并显示详细
            if (points.length == 1) {
                map.centerAndZoom(point, 12);
                // showInfoQy(marker, points[0]);
            } else if (i == 0) {
                // showInfoQy(marker, points[0]);
            }

            (function() {
                var infopoint = points[i];
                var gcbh = points[i].gcbh;
                marker.addEventListener("click", function() {
                    showInfoQy(this, infopoint);
                });

            })();
        }
    }
}

//点击不同的图标获取显示不同的值
function showInfoQy(thisMaker, point) {
    var opts = {
        width: 500, // 信息窗口宽度
        height: 200, // 信息窗口高度
    }
    var factorysingle = " ";
    factorysingle += "<div id='enginner_bigbox'><p class='enginner_bigbox_title'>企业名称:" + point.qymc + "</p><div id='enginner_bigbox_left'><p class='enginner_bigbox_all'>企业类型:" + point.lxmc + "</p><p class='enginner_bigbox_all'>企业负责人:" + point.qyfzr + "</p><p class='enginner_bigbox_all'>联系电话:" + point.lxdh + "</p></div></div>";

    var infoWindow = new BMap.InfoWindow(factorysingle, opts);

    thisMaker.openInfoWindow(infoWindow);
}

//获取人员一览
function getRyListForMap() {

    var gcmc = $("#txtGcmcForRy").val();
    var key = $("#txtRymc").val();
    var pageindex = $("#rypageindex").val();

    // var obj = Object.assign(paramsData, {
    //     gcmc: gcmc,
    //     key: key,
    //     page: pageindex,
    // });

    var obj = {};
    jQuery.extend(obj,paramsData,{
        gcmc: gcmc,
        key: key,
        page: pageindex
    });

    ajaxTpl("/welcome/GetRyList", obj, function(result) {
        var rylist = result.data;

        //全部工程累加
        rylistAll = rylistAll.concat(rylist);

        //展开更多工程一览显示
        setRylistForMap(rylist);

        //地图标识
        addMarkerRylist(rylist);

        //刷新标志设为false
        refreshmaptabflg3 = false;
    });
}

//展开更多人员一览显示
function setRylistForMap(rylist) {
    var listdata = rylist;
    var singledata = "";
    $.each(listdata, function(index, value) {
        singledata += "<div class='rysinglelist clearfix'><div class='singlelist_left'><p>姓名:" + value.ryxm + "</p><p>身份证号:" + value.sfzhm + "</p><p>所属工程:" + value.gcmc + "</p><p>状态:" + (value.hasdelete ? "在职" : "离职") + "</p></div><div class='singlelist_right'><img src='data:image/png;base64," + ((value.zp == "" || typeof(value.zp) == "undefined") ? defimg : value.zp) + "'/></div></div>"
    });
    $("#rycontent").append(singledata);
    addMarkerRylist(listdata);
    $(".rysinglelist").click(function() {
        var singleclick = [];
        var num = $(this).index();
        singleclick.push(rylistAll[num]);
        addMarkerRylist(singleclick);
    });
}

//在地图上表示出来的点
function addMarkerRylist(points) {
    for (var i = 0; i < points.length; i++) {
        var gcbz = points[i].gcbz;
        var jd;
        var wd;
        if (gcbz == "") {
            gcbz = "120.699854,28.016724";
        }
        if (gcbz != "") {

            jd = gcbz.split(",")[0];
            wd = gcbz.split(",")[1];

            var point = new BMap.Point(jd, wd);

            if ((points[i].ryzt ? "在职" : "离职") == "在职") {
                var myIcon = new BMap.Icon("/skins/default/welcomezs/images/zz.png", new BMap.Size(47.5, 47.5));
            } else {
                var myIcon = new BMap.Icon("/skins/default/welcomezs/images/lz.png", new BMap.Size(47.5, 47.5));
            }

            var marker = new BMap.Marker(point, {
                icon: myIcon
            });
            map.addOverlay(marker);

            //单个坐标的话设置为中心,并显示详细
            if (points.length == 1) {
                map.centerAndZoom(point, 12);
                // showInfoRy(marker, points[0]);
            } else if (i == 0) {
                // showInfoRy(marker, points[0]);
            }

            (function() {
                var infopoint = points[i];
                var gcbh = points[i].gcbh;
                marker.addEventListener("click", function() {
                    showInfoRy(this, infopoint);
                });

            })();
        }

    }
}

//点击不同的图标获取显示不同的值
function showInfoRy(thisMaker, point) {
    var opts = {
        width: 500, // 信息窗口宽度
        height: 250, // 信息窗口高度
    }
    var peoplesingle = "";
    peoplesingle += "<div id='people_bigbox' class='clearfix'> ";
    peoplesingle += " <p class='people_bigbox_tttle'>姓名:" + point.ryxm + "</p> ";
    peoplesingle += "   <div class='people_bigbox_left'> ";
    peoplesingle += "   <p class='people_bigbox_name'>身份证号:" + point.sfzhm + "</p> ";
    peoplesingle += "   <p class='people_bigbox_name'>所在工程:" + point.gcmc + "</p> ";
    peoplesingle += "    <p class='people_bigbox_name'>工种:" + point.gz + "</p> ";
    peoplesingle += "   <p class='people_bigbox_name'>人员状态:" + (point.hasdelete ? "在职" : "离职") + "</p>";
    peoplesingle += "<span id='pepple_button_left'>查看详细</span> ";
    peoplesingle += "     </div> ";
    peoplesingle += "   <div class='people_bigbox_right'><img src='data:image/png;base64," + ((point.zp == "" || typeof(point.zp) == "undefined") ? defimg : point.zp) + "'/></div></div></div> ";
    var infoWindow = new BMap.InfoWindow(peoplesingle, opts);
    var addnumber = point;
    infoWindow.addEventListener("open", function(type, target, point) {
        $("#pepple_button_left").bind('click', function() {
            toryformap(addnumber.sfzhm, addnumber.gcbh);
        });
    })
    thisMaker.openInfoWindow(infoWindow);
}
function toryformap(sfzhm, gcbh) {

    //每个图表的宽度
    var w = ($(".panel").width() * 0.95).toString() + "px";
    //每个图表的高度
    var h = ($(".panel").height() * 0.9).toString() + "px";

    var index = parent.layer.open({
        type: 2,
        content: '/welcome/welcomeryz?sfzhm=' + sfzhm + '&gcbh=' + gcbh,
        area: [w, h]
    });
}
function togczsformap(gcbh, gcmc) {

    //每个图表的宽度
    var w = ($(".panel").width() * 0.95).toString() + "px";
    //每个图表的高度
    var h = ($(".panel").height() * 0.95).toString() + "px";

    var index = parent.layer.open({
        type: 2,
        content: '/welcome/welcomegcqp?gcbh=' + gcbh + '&gcmc=' + gcmc,
        area: [w, h]
    });

    //layer.full(index);
}

//获取更多工程,企业,人员
function morelist(tabtype) {
    formChange();

    if (tabtype == "1") {
        var pageindex = parseInt($("#gcpageindex").val());
        pageindex = pageindex + 1;
        $("#gcpageindex").val(pageindex)
        getGcListForMap();
    }
    if (tabtype == "2") {
        var pageindex = parseInt($("#qypageindex").val());
        pageindex = pageindex + 1;
        $("#qypageindex").val(pageindex)
        getQyListForMap();
    }
    if (tabtype == "3") {
        var pageindex = parseInt($("#rypageindex").val());
        pageindex = pageindex + 1;
        $("#rypageindex").val(pageindex)
        getRyListForMap();
    }
}

//展开更多检索按钮
function btnSearch(tabtype) {
    mapInit();

    if (tabtype == 1) {
        $("#gcpageindex").val("1");
        $("#listcontent").empty();
        gclistAll = new Array();
        getGcListForMap();
    }

    if (tabtype == 2) {
        $("#qypageindex").val("1");
        $("#qycontent").empty();
        qylistAll = new Array();
        getQyListForMap();
    }

    if (tabtype == 3) {
        $("#rypageindex").val("1");
        $("#rycontent").empty();
        rylistAll = new Array();
        getRyListForMap();
    }

}




//第一图刷新
function refresh1() {
    //地图面板初始化
    mapInit();

    //工程刷新
    if (tabId == "GcList") {
        if (refreshmaptabflg1) {
            // 工程一览获取,地图标注
            getGcListForMap();
        } else {
            // 地图标注
            addMarkerGclist(gclistAll);
        }
        $(".moresearchtitle").css("height", $("#moresearch").height() - 120);
    }

    //企业刷新
    if (tabId == "QyList") {
        if (refreshmaptabflg2) {
            // 工程一览获取,地图标注
            getQyListForMap();
        } else {
            // 地图标注
            addMarkerQylist(qylistAll);
        }
        $(".moresearchtitle").css("height", $("#moresearch").height() - 120);
    }

    //人员刷新
    if (tabId == "RyList") {
        if (refreshmaptabflg3) {
            // 工程一览获取,地图标注
            getRyListForMap();
        } else {
            // 地图标注
            addMarkerRylist(rylistAll);
        }
        $(".moresearchtitle").css("height", $("#moresearch").height() - 170);
    }
}

$("#tabData").on("click", "li", function() {
    var that = $(this);
    var i = that.index();
    that.addClass("active").siblings().removeClass("active");
    that.parent().next().children().eq(i).show().siblings().hide();

    tabId = that.attr("id");

    formChange();

    switch (that.index()) {
        case 0:
            if (!gclistAll.length) {
                getGcListForMap();
            }
            break;
        case 1:
            if (!qylistAll.length) {
                getQyListForMap();
            }
            break;
        case 2:
            if (!rylistAll.length) {
                getRyListForMap();
            }
            break;
        default:
            break;
    }
});