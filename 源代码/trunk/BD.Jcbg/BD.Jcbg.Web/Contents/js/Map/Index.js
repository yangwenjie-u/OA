var map;
var wdqy;
$(function () {
    initMap();
    SetLiWidth();
    $(".tiaozhuan_li").on("click", function () {
        for (var i = 0; i < $(".tiaozhuan_mubiao_li").length; i++) {
            if ($(".tiaozhuan_mubiao_li").eq(i).hasClass("active")) {
                $(".tiaozhuan_mubiao_li").eq(i).removeClass("active");
            }
            $($(this).attr('href')).addClass("active");
        }
    });
    //收缩功能
    $(".shouqi").on("click", function () {
        if ($(".shouqi").text() === "收起") {
            $(".fen_show_last").addClass("no_xianshi");
            $(".shouqi").html("展开");
        } else if ($(".shouqi").text() === "展开") {
            $(".fen_show_last").removeClass("no_xianshi");
            $(".shouqi").html("收起");
        }
    });
});

//设置菜单比例
function SetLiWidth() {
    var width;
    wdqy = $("#wdqy").val();
    switch (wdqy) {
        case "0":
            width = "20%";
            break;
        case "1":
            width = "33%";
            $(".xm").css("display", "none");
            break;
        default:
            width = "20%";
            break;
    }
    $(".fen_show_all_top_li").css("width", width);
}

//百度地图API功能
function initMap() {
    var script = document.createElement("script");
    script.type = "text/javascript";
    script.src = "https://api.map.baidu.com/api?v=2.0&ak=Nqv7EOZ2k8lYjVVh8n5E8aFB&callback=init";
    document.body.appendChild(script);
}

//初始化地图
function init() {
    map = new BMap.Map("allmap", { mapType: BMAP_NORMAL_MAP });
    var mapType = new BMap.MapTypeControl({
        anchor: BMAP_ANCHOR_TOP_RIGHT,
        mapTypes: [BMAP_NORMAL_MAP, BMAP_SATELLITE_MAP, BMAP_HYBRID_MAP]
    });
    map.addEventListener("click", function (e) { //点击事件
        if (!e.overlay) {
            $(".fen_show_last").addClass("no_xianshi");
            $(".shouqi").html("展开");
        }
    });
    map.addControl(mapType);
    map.enableScrollWheelZoom(true);
    map.enableKeyboard(); //启用键盘上下左右键移动地图
    addMapControl();
    switch (wdqy) {
        case "1":
            $("#qy").click();
            break;
        default:
            SetLocation("所有工程","");
            break;
    }
}

//设置地图事件
function addMapControl() {
    //向地图中添加缩放控件
    var ctrl_nav = new BMap.NavigationControl({ anchor: BMAP_ANCHOR_TOP_LEFT, type: BMAP_NAVIGATION_CONTROL_LARGE });
    map.addControl(ctrl_nav);
    //向地图中添加缩略图控件
    var ctrl_ove = new BMap.OverviewMapControl({ anchor: BMAP_ANCHOR_BOTTOM_RIGHT, isOpen: 1 });
    map.addControl(ctrl_ove);
    //向地图中添加比例尺控件
    var ctrl_sca = new BMap.ScaleControl({ anchor: BMAP_ANCHOR_BOTTOM_LEFT });
    map.addControl(ctrl_sca);
    var top_right_navigation = new BMap.NavigationControl({
        anchor: BMAP_ANCHOR_BOTTOM_RIGHT,
        type: BMAP_NAVIGATION_CONTROL_SMALL,
        enableGeolocation: true,
        offset: new BMap.Size(10, 230)
    }); //右上角，仅包含平移和缩放按钮
    map.addControl(top_right_navigation);
}

//设置当前位置状态
function SetLocation(gczt, qymc) {
    if (gczt !== "") {
        $("#gczt").html(gczt);
    }
    if (qymc !== "") {
        $("#gcqy").html(qymc);
    }
    $("#qtzt").html("");
    GetGcInfos();
}

//关闭状态
function CloseDiv(div) {
    var id = div.id;
    if ($("#" + id).html() !== "") {
        $("#" + id).html("");
    }
    GetGcInfos();
}

//显示/隐藏div
function ShowDiv() {
    $(".div").each(function(index,element) {
        if ($(element).html() === "") {
            $(element).css("display", "none");
        } else {
            $(element).css("display", "");
        }
    });
}

//获取工程信息
function GetGcInfos() {
    ShowDiv();
    var gczt = $("#gczt").html() === "所有工程" ? "" : $("#gczt").html();
    var qymc = $("#gcqy").html() === "所有区域" ? "" : $("#gcqy").html();
    $("#project").css("display", "");
    //清除原有的覆盖物等
    map.clearOverlays();
    $.ajax({
        url: "/Kqj/GetGcInfos?qymc=" + encodeURIComponent(qymc) + "&gczt=" + encodeURIComponent(gczt),
        type: "get",
        dataType: "json",
        async: true,
        cache: false,
        success: function (json) {
            LoadGcxx(json.gcxx);
            LoadGcjwd(json.gckq);
        }
    });
}

//工程信息列表
function LoadGcxx(gcxx) {
    var allGc = "<div class=\"fen_show_last_ul_div\"><ul class=\"fen_show_last_ul\">",
        zjGc = "<div class=\"fen_show_last_ul_div\"><ul class=\"fen_show_last_ul\">",
        jgGc = "<div class=\"fen_show_last_ul_div\"><ul class=\"fen_show_last_ul\">",
        tgGc = "<div class=\"fen_show_last_ul_div\"><ul class=\"fen_show_last_ul\">",
        allarea = "<div class=\"fen_show_last_ul_div\"><ul class=\"fen_show_last_ul\">",
        ycq = "<div class=\"fen_show_last_ul_div\"><ul class=\"fen_show_last_ul\">",
        kqq = "<div class=\"fen_show_last_ul_div\"><ul class=\"fen_show_last_ul\">",
        syq = "<div class=\"fen_show_last_ul_div\"><ul class=\"fen_show_last_ul\">",
        zjs = "<div class=\"fen_show_last_ul_div\"><ul class=\"fen_show_last_ul\">",
        szs = "<div class=\"fen_show_last_ul_div\"><ul class=\"fen_show_last_ul\">",
        xcx = "<div class=\"fen_show_last_ul_div\"><ul class=\"fen_show_last_ul\">",
        gctp = "/Contents/Image/Home/project.png";
    for (var i = 0; i < gcxx.length; i++) {
        if (gcxx[i].gctp !== "") {
            gctp = gcxx[i].gctp;
        }
        var gc = "<li class=\"fen_show_last_li\" onclick=\"click_fen_show_last_li('" + gcxx[i].Lon + "','" + gcxx[i].Lat + "')\"><div class=\"project_list\"><div class=\"project_list_left\"><div class=\"project_title\">" + gcxx[i].gcmc + "</div><p><span class=\"project_jian_jieshi\">" + gcxx[i].gczt + "</span></p><p><span class=\"project_jian_jieshi1\">" + gcxx[i].qymc + "</span></p></div><div class=\"project_list_right\"><img src=\"" + gctp + "\" /></div></div></li>";
        allGc += gc;
        switch (gcxx[i].gczt) {
            case "在建工程":
                zjGc += gc;
                break;
            case "竣工工程":
                jgGc += gc;
                break;
            case "停工工程":
                tgGc += gc;
                break;
        }
        allarea += gc;
        switch (gcxx[i].qymc) {
            case "越城区":
                ycq += gc;
                break;
            case "柯桥区":
                kqq += gc;
                break;
            case "上虞区":
                syq += gc;
                break;
            case "诸暨市":
                zjs += gc;
                break;
            case "嵊州市":
                szs += gc;
                break;
            case "新昌县":
                xcx += gc;
                break;
        }
    }
    allGc += "</ul></div>";
    zjGc += "</ul></div>";
    jgGc += "</ul></div>";
    tgGc += "</ul></div>";
    allarea += "</ul></div>";
    ycq += "</ul></div>";
    kqq += "</ul></div>";
    syq += "</ul></div>";
    zjs += "</ul></div>";
    szs += "</ul></div>";
    xcx += "</ul></div>";
    $("#all_project").html(allGc);
    $("#building").html(zjGc);
    $("#build_ok").html(jgGc);
    $("#build_borken").html(tgGc);
    $("#all_area").html(allarea);
    $("#ycq").html(ycq);
    $("#kqq").html(kqq);
    $("#syq").html(syq);
    $("#zjs").html(zjs);
    $("#szs").html(szs);
    $("#xcx").html(xcx);
}

//工程经纬度
function LoadGcjwd(gckq) {
    var centerPoint = new Array();
    for (var j = 0; j < gckq.length; j++) {
        var point = new Array();
        var jwd = gckq[j].jwd;
        var center = parseInt(jwd.length / 2);
        for (var n = 0; n < jwd.length; n++) {
            point[n] = new BMap.Point(jwd[n].Lon, jwd[n].Lat);
        }
        if (jwd.length > 0) {
            //在地图上划线
            var polyline = new BMap.Polyline(point,
                {
                    strokeColor: "blue", // 笔画颜色
                    strokeWeight: 6, // 笔画粗细
                    strokeOpacity: 0.5 // 笔画透明度
                });
            map.addOverlay(polyline);
            if (centerPoint.length === 0) {
                centerPoint[0] = new BMap.Point(jwd[center].Lon, jwd[center].Lat);
            }
            //将点标注在地图上，并显示图片
            var img = "/Contents/Image/Home/build.png";
            //考勤率低于0.9时,图片需要闪烁
            for (var i = 0; i < gckq[j].kqxx.length; i++) {
                if (gckq[j].kqxx[i].KQL < 0.9) {
                    img = "/Contents/Image/Home/build3.gif";
                    break;
                }
            }
            var icon = new BMap.Icon(img, new BMap.Size(65, 102),
                { offset: new BMap.Size(32, 102), imageOffset: new BMap.Size(-1 * 2 * 65, 0) });
            switch (gckq[j].gczt) {
                case "在建工程":
                    icon = new BMap.Icon(img, new BMap.Size(65, 102),
                        { offset: new BMap.Size(32, 102), imageOffset: new BMap.Size(-1 * 2 * 65, 0) });
                    break;
                case "竣工工程":
                    icon = new BMap.Icon(img, new BMap.Size(65, 102),
                        { offset: new BMap.Size(32, 102), imageOffset: new BMap.Size(-1 * 0 * 65, 0) });
                    break;
                case "停工工程":
                    icon = new BMap.Icon(img, new BMap.Size(65, 102),
                        { offset: new BMap.Size(32, 102), imageOffset: new BMap.Size(-1 * 1 * 65, 0) });
                    break;
            }
            var label = new BMap.Label(gckq[j].gcmc, { offset: new BMap.Size(50, 0) });
            label.setStyle({
                color: "#ccc",
                fontSize: "12px",
                lineHeight: "20px",
                fontFamily: "微软雅黑",
                border: "2px solid #555",
                padding: "3px 10px 3px 10px",
                backgroundColor: "#89511a",
                opcity: "0.8",
                width: "auto",
                maxWidth: "200px",
                whiteSpace: "nowrap",
                overflow: "hidden",
                textOverflow: "ellipsis"
            });
            //弹出层,显示标段信息,企业考勤信息
            var dwxx = "", sgdw = "", jsdw = "", jldw = "", sjdw = "", kcdw = "", color = "";
            for (var m = 0; m < gckq[j].kqxx.length; m++) {
                if (gckq[j].kqxx[m].KQL < 0.9) {
                    color = "style=\"color:red;\"";
                }
                dwxx = gckq[j].kqxx[m].QYMC +
                    " <span " + color + ">" +
                    gckq[j].kqxx[m].RYKQ +
                    "/" +
                    gckq[j].kqxx[m].RYZS +
                    "</span><br>";
                switch (gckq[j].kqxx[m].QYLX) {
                    case "施工单位":
                        sgdw += dwxx;
                        break;
                    case "建设单位":
                        jsdw += dwxx;
                        break;
                    case "监理单位":
                        jldw += dwxx;
                        break;
                    case "设计单位":
                        sjdw += dwxx;
                        break;
                    case "勘察单位":
                        kcdw += dwxx;
                        break;
                }
            }
            var content = "<div class='info_all'><h4 class='info_h4'>工程名称:" +
                gckq[j].gcmc +
                "</h4>" +
                "<img  src='" +
                "/Contents/Image/Home/project.png" +
                "' />" +
                "<p>建设单位:" +
                jsdw +
                "</p>" +
                "<p>施工单位:" +
                sgdw +
                "</p>" +
                "<p>监理单位:" +
                jldw +
                "</p>" +
                "<p>设计单位:" +
                sjdw +
                "</p>" +
                "<p>勘察单位:" +
                kcdw +
                "</p>" +
                "<p>(考勤人数/总人数)</p>" +
                "</div></div>";
            var marker = LoadInfoWindow(jwd[center].Lon, jwd[center].Lat, icon, content);
            marker.setLabel(label);
            map.addOverlay(marker);
        }
    }
    if (centerPoint.length > 0) {
        map.centerAndZoom(centerPoint[0], 17);
    } else {
        map.centerAndZoom(new BMap.Point(120.605389, 29.972873), 17);
    }
}

//加载工程信息弹出框
function LoadInfoWindow(lon, lat, icon, html) {
    var marker = new BMap.Marker(new BMap.Point(lon, lat), { icon: icon });
    //覆盖物相对于坐标的偏移
    marker.setOffset(new BMap.Size(0, -40));
    marker.addEventListener("click", function () {
        this.openInfoWindow(new BMap.InfoWindow(html));
    });
    return marker;
}

//根据工程定位到地图上显示的位置
function click_fen_show_last_li(lon, lat) {
    var point = new BMap.Point(lon, lat);
    map.centerAndZoom(point, 17);
}

//获取企业信息
function GetSwqyInfos(localtion) {
    $("#project").css("display", "none");
    $("#qtzt").html(localtion);
    $("#gczt").html("");
    $("#gcqy").html("");
    ShowDiv();
    //清除原有的覆盖物等
    map.clearOverlays();
    $.ajax({
        url: "/Kqj/GetSwqyInfos",
        type: "get",
        dataType: "json",
        async: true,
        cache: false,
        success: function (json) {
            LoadQyInfo(json.qy);
        }
    });
}

//企业列表信息
function LoadQyInfo(qy) {
    var swqy = "<div class=\"fen_show_last_ul_div\"><ul class=\"fen_show_last_ul\">",
        gctp = "/Contents/Image/Home/project.png", lon = "", lat = "";
    for (var i = 0; i < qy.length; i++) {
        var kqjxx = "<li class=\"fen_show_last_li\" onclick=\"click_fen_show_last_li('" + qy[i].lon + "','" + qy[i].lat + "')\"><div class=\"project_list\"><div class=\"project_list_left\"><div class=\"project_title\">企业名称:" + qy[i].qymc + "</div><p>企业类型:<span class=\"project_jian_jieshi\">" + qy[i].qylx + "</span></p></div><div class=\"project_list_right\"><img src=\"" + gctp + "\" /></div></div></li>";
        if (lon === "" && lat === "") {
            lon = qy[i].lon;
            lat = qy[i].lat;
        }
        swqy += kqjxx;
        var content = "<div class='info_all'><h4 class='info_h4'>企业名称:" +
            qy[i].qymc +
            "</h4>" +
            "<img  src='" +
            "/Contents/Image/Home/project.png" +
            "' />" +
            "<p>企业类型:" +
            qy[i].qylx +
            "</p>" +
            "<p>应勤人数:" +
            qy[i].ryzs +
            "</p>" +
            "<p>考勤人数:" +
            qy[i].kqs +
            "</p>" +
            "<p>出勤率:<span style='color:red;'>" +
            qy[i].cql +
            "</span></p>" +
            "</div></div>";
        var marker = LoadKqjInfoWindow(qy[i].lon, qy[i].lat, content);
        map.addOverlay(marker);               // 将标注添加到地图中
        if (qy[i].cql <= 80) {
            marker.setAnimation(BMAP_ANIMATION_BOUNCE); //跳动的动画
        }
    }
    swqy += "</ul></div>";
    $("#swqy").html(swqy);
    click_fen_show_last_li(lon, lat);
}

//获取考勤机信息
function GetKqjInfos(kqjzt, localtion) {
    $("#project").css("display", "none");
    $("#qtzt").html(localtion);
    $("#gczt").html("");
    $("#gcqy").html("");
    ShowDiv();
    //清除原有的覆盖物等
    map.clearOverlays();
    $.ajax({
        url: "/Kqj/GetKqjInfos?kqjzt=" + encodeURIComponent(kqjzt),
        type: "get",
        dataType: "json",
        async: true,
        cache: false,
        success: function (json) {
            LoadKqj(json.kqj);
        }
    });
}

//考勤机信息,考勤机是否在线
function LoadKqj(kqj) {
    var allKqj = "<div class=\"fen_show_last_ul_div\"><ul class=\"fen_show_last_ul\">",
        zxKqj = "<div class=\"fen_show_last_ul_div\"><ul class=\"fen_show_last_ul\">",
        lxKqj = "<div class=\"fen_show_last_ul_div\"><ul class=\"fen_show_last_ul\">",
        gctp = "/Contents/Image/Home/project.png";
    for (var i = 0; i < kqj.length; i++) {
        var kqjxx = "<li class=\"fen_show_last_li\" onclick=\"click_fen_show_last_li('" + kqj[i].lon + "','" + kqj[i].lat + "')\"><div class=\"project_list\"><div class=\"project_list_left\"><div class=\"project_title\">考勤机序号:" + kqj[i].kqjbh + "</div><p>考勤机状态:<span class=\"project_jian_jieshi\">" + kqj[i].kqjzt + "</span></p></div><div class=\"project_list_right\"><img src=\"" + gctp + "\" /></div></div></li>";
        allKqj += kqjxx;
        var color = "";
        switch (kqj[i].kqjzt) {
            case "在线":
                zxKqj += kqjxx;
                break;
            case "离线":
                color += "style='color:red;'";
                lxKqj += kqjxx;
                break;
        }
        var content = "<div class='info_all'><h4 class='info_h4'>考勤机序号:" +
            kqj[i].kqjbh +
            "</h4>" +
            "<img  src='" +
            "/Contents/Image/Home/project.png" +
            "' />" +
            "<p>所属企业:" +
            kqj[i].qymc +
            "</p>" +
            "<p>所属工程:" +
            kqj[i].gcmc +
            "</p>" +
            "<p>考勤机状态:<span " + color + ">" +
            kqj[i].kqjzt +
            "</span></p>" +
            "<p>备注:" +
            kqj[i].kqjbz +
            "</p>" +
            "</div></div>";
        var marker = LoadKqjInfoWindow(kqj[i].lon, kqj[i].lat, content);
        map.addOverlay(marker);               // 将标注添加到地图中
        if (kqj[i].kqjzt === "离线") {
            marker.setAnimation(BMAP_ANIMATION_BOUNCE); //跳动的动画
        }
    }
    allKqj += "</ul></div>";
    zxKqj += "</ul></div>";
    lxKqj += "</ul></div>";
    $("#all_kqj").html(allKqj);
    $("#zxkqj").html(zxKqj);
    $("#lxkqj").html(lxKqj);
}

//考勤机信息弹出框
function LoadKqjInfoWindow(lon, lat, html) {
    var point = new BMap.Point(lon, lat);
    var marker = new BMap.Marker(point);  //创建标注
    marker.addEventListener("click", function () {
        this.openInfoWindow(new BMap.InfoWindow(html));
    });
    return marker;
}

//单位类型
function LoadDwlx() {
    $.ajax({
        url: "/Kqj/GetDwlx",
        type: "get",
        dataType: "json",
        async: false,
        cache: false,
        success: function (json) {
            var html = "<option value='' selected='selected'>所有单位</option>";
            for (var i = 0; i < json.length; i++) {
                html += "<option value='" + json[i].lxbh + "'>" + json[i].lxmc + "</option>";
            }
            $("#Dwlx").html(html);
            LoadDwmc($("#Dwmc").val());
        }
    });
}

//单位名称
function LoadDwmc(lxbh) {
    if (lxbh === undefined || lxbh === null) {
        lxbh = "";
    }
    $.ajax({
        url: "/Kqj/GetDwmc?lxbh=" + lxbh,
        type: "get",
        dataType: "json",
        async: false,
        cache: false,
        success: function (json) {
            var html = "<option value='' selected='selected'>所有单位</option>";
            for (var i = 0; i < json.length; i++) {
                html += "<option value='" + json[i].qybh + "'>" + json[i].qymc + "</option>";
            }
            $("#Dwmc").html(html);
        }
    });
}

//获取人员状态信息
function GetRyInfos(localtion) {
    $("#project").css("display", "none");
    $("#qtzt").html(localtion);
    $("#gczt").html("");
    $("#gcqy").html("");
    ShowDiv();
    LoadDwlx();
}

//查询人员信息
function Search() {
    $.ajax({
        url: "/Kqj/GetRyInfos?qylx=" + encodeURIComponent($("#Dwlx").val()) + "&qybh=" + encodeURIComponent($("#Dwmc").val()) + "&ryxm=" + encodeURIComponent($("#UserName").val()),
        type: "get",
        dataType: "json",
        async: true,
        cache: false,
        success: function (json) {
            //清除原有的覆盖物等
            map.clearOverlays();
            LoadRy(json);
        }
    });
}

//人员信息,经纬度
function LoadRy(json) {
    for (var i = 0; i < json.length; i++) {
        var content = "<div class='info_all'><h4 class='info_h4'>人员姓名:" +
            json[i].ryxm +
            "</h4>" +
            "<p>所属企业:" +
            json[i].qymc +
            "</p>" +
            "<p>人员性别:" +
            json[i].xb +
            "</p>" +
            "<p>手机号:" +
            json[i].rybh +
            "</p>" +
            "<p>岗位:" +
            json[i].gw +
            "</p>" +
            "<p>职称:" +
            json[i].zc +
            "</p>" +
            "</div></div>";
        var marker = LoadKqjInfoWindow(json[i].lon, json[i].lat, content);
        map.addOverlay(marker);//将标注添加到地图中
        var point;
        if (json.length > 0) {
            point = new BMap.Point(json[0].lon, json[0].lat);
        } else {
            point = new BMap.Point(120.605389, 29.972873);
        }
        map.centerAndZoom(point, 17);
    }
}