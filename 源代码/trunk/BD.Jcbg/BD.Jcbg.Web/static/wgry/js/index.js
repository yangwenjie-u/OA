var page = 1;
var map;
var gclist;
var qylist;
var rylist;
var gclistAll = new Array()
var qylistAll = new Array()
var rylistAll = new Array()
var tempCity = "浙江省温州市";
var millisec = 15000;
var defimg = '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';
var tabId = "GcList";
var firstflg = true;
var loop;
$(function () {

    $("#province10").prop('disabled', true);
    $("#city10").prop('disabled', true);

    //fullscreen();

	//全局初始化,地图初始化，用户名
	init();

	//获取企业一览,成功后内容初始化，各选项changes事件绑定
	pageLoad();

    //各选项changes事件绑定
	bindChange();

});

//刷新
function Refresh() {
    alert(1111);
}

//全局初始化
function init() {
	//鼠标滚动全屏
	$('.panel').css({
		'height': $(window).height()
	});
	$.scrollify({
	    section: 'section',
	    scrollbars: false,

		before: function () {
		    alert($.scrollify.current()[0].dataset.sectionName);
		},
	    after: function () {
	        //alert('滚动完成');
	    }
	});

	//加载固定块(回到顶部按钮)
	layui.use('util', function() {
		var util = layui.util;
		util.fixbar({
			// bar1: true,
			click: function(type) {
				console.log(type);
				if(type === 'bar1') {
					alert('点击了bar1')
				}
			}
		});
	});

	//加载地图 
	map = new BMap.Map("allMap", {
		enableMapClick: false
	});
	$("#allMap").css("height", document.documentElement.clientHeight - 40);

	//地图加载控件
	var mapType1 = new BMap.MapTypeControl({
		mapTypes: [BMAP_NORMAL_MAP, BMAP_HYBRID_MAP]
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
	//map.addControl(mapType1);          //2D图，卫星图
	map.addControl(mapType2); //左上角，默认地图控件
	// map.setCurrentCity("温州");        //由于有3D图，需要设置城市哦
	// map.addControl(overView);          //添加默认缩略地图控件
	//map.addControl(overViewOpen);      //右下角，打开
	var top_right_navigation = new BMap.NavigationControl({
		anchor: BMAP_ANCHOR_TOP_RIGHT,
		type: BMAP_NAVIGATION_CONTROL_SMALL,
		offset: new BMap.Size(50, 80)
	}); //右上角，仅包含平移和缩放按钮
	map.addControl(top_right_navigation);

	//展开所有点击显示隐藏
	$("#allprograme").click(function() {
		$("#moresearch").toggle();
	});

	//展开所有地图显示在下方
	var topheight = 40;
	var topheightbottom = 80;
	$("#allprograme").css("top", topheight);
	$("#moresearch").css("top", topheightbottom + 5);
	$(".moresearchtitle").css("height", $("#moresearch").height() - 120);
}

//获取企业一览,成功后内容初始化，各选项changes事件绑定
function pageLoad() {

    clearInterval(loop);

    var province = encodeURIComponent($("#province10").val());
    var city = encodeURIComponent($("#city10").val());
    var district = encodeURIComponent($("#district10").val());
    var qylx = $("#selectQylx").val();

  

	//获取企业一览
    $.ajax({
        type: "POST",
        url: "/welcome/GetSelectQyList?province=" + province + "&city=" + city + "&district=" + district + "&qylx=" + qylx,
        dataType: "json",
        async: false,
		success: function(result) {
		    $("#selectQy").empty();
		    $("#selectQy").append("<option value=''>==请选择==</option>");
			var d = result.rows;
			for(var i = 0; i < d.length; i++) {
				var id = d[i].qybh;
				var name = d[i].qymc;
				var opt = "<option value='" + id + "'>" + name + "</option>";
				$("#selectQy").append(opt);
			}

		    //内容初始化
			intMap(); //地图

		    //八块内容
			initStatistics(); //展示

		    //三个一览
			initGcTable();
			initQyTable();
			initRyTable();
          
		    //八块内容定时刷新，当有变更时，其他的也刷新
			loop = setInterval(function () {
	
			    statistics0(); 

			    //各选项changes事件绑定
			    //bindChange();

			}, millisec);

			
		}
	});
}

//省变更 市变更 区变更
function cityChange() {
	//移除各选项changes事件绑定
    unbindChange();

    $("#gcpageindex").val("1");
    $("#qypageindex").val("1");
    $("#rypageindex").val("1");
    $("#listcontent").empty();
    $("#qycontent").empty();
    $("#rycontent").empty();
    gclistAll = new Array();
    qylistAll = new Array();
    rylistAll = new Array();

    $("#txtGcmc").val("");
    $("#txtQymc").val("");
    $("#txtGcmcForRy").val("");
    $("#txtRymc").val("");
    $("#txtLableGcmc").val("");
    $("#txtLableQymc").val("");
    $("#txtLableGcmcForRy").val("");
    $("#txtLableRymc").val("");

	//地图重载
	getBoundary($("#province10").val() + $("#city10").val() + $("#district10").val());

	//企业类型 初始化
	$('#selectQylx').prop('selectedIndex', 0);

	//工程状态 初始化
	$('#selectGczt').prop('selectedIndex', 0);

	remove_overlay();
	mapoverlay();

	//企业一览 初始化
	pageLoad();
}

//企业类型变更
function qylxChange() {

	//移除各选项changes事件绑定
    unbindChange();

    $("#gcpageindex").val("1");
    $("#qypageindex").val("1");
    $("#rypageindex").val("1");
    $("#listcontent").empty();
    $("#qycontent").empty();
    $("#rycontent").empty();
    gclistAll = new Array();
    qylistAll = new Array();
    rylistAll = new Array();

    $("#txtGcmc").val("");
    $("#txtQymc").val("");
    $("#txtGcmcForRy").val("");
    $("#txtRymc").val("");
    $("#txtLableGcmc").val("");
    $("#txtLableQymc").val("");
    $("#txtLableGcmcForRy").val("");
    $("#txtLableRymc").val("");

	//工程状态 初始化
	$('#selectGczt').prop('selectedIndex', 0);

	//企业一览 初始化
	pageLoad();
}

//企业一览变更
function qyChange() {

	//移除各选项changes事件绑定
    unbindChange();

    $("#gcpageindex").val("1");
    $("#qypageindex").val("1");
    $("#rypageindex").val("1");
    $("#listcontent").empty();
    $("#qycontent").empty();
    $("#rycontent").empty();
    gclistAll = new Array();
    qylistAll = new Array();
    rylistAll = new Array();

    $("#txtGcmc").val("");
    $("#txtQymc").val("");
    $("#txtGcmcForRy").val("");
    $("#txtRymc").val("");
    $("#txtLableGcmc").val("");
    $("#txtLableQymc").val("");
    $("#txtLableGcmcForRy").val("");
    $("#txtLableRymc").val("");

	//工程状态 初始化
	$('#selectGczt').prop('selectedIndex', 0);

	//内容初始化
	intMap(); //地图
	initStatistics(); //展示

    //三个一览
	initGcTable();
	initQyTable();
	initRyTable();

	//各选项changes事件绑定
	//bindChange();
}

//工程状态变更
function gcztChange() {

 	//移除各选项changes事件绑定
    unbindChange();

    $("#gcpageindex").val("1");
    $("#qypageindex").val("1");
    $("#rypageindex").val("1");
    $("#listcontent").empty();
    $("#qycontent").empty();
    $("#rycontent").empty();
    gclistAll = new Array();
    qylistAll = new Array();
    rylistAll = new Array();

    $("#txtGcmc").val("");
    $("#txtQymc").val("");
    $("#txtGcmcForRy").val("");
    $("#txtRymc").val("");
    $("#txtLableGcmc").val("");
    $("#txtLableQymc").val("");
    $("#txtLableGcmcForRy").val("");
    $("#txtLableRymc").val("");

	//内容初始化
	intMap(); //地图
	initStatistics(); //展示

    //三个一览
	initGcTable();
	initQyTable();
	initRyTable();

	//各选项changes事件绑定
	//bindChange();
}

//地图初始化
function intMap() {

    //获取工程一览
    getGcList();

    //获取企业一览
    getQyList();

    //获取人员一览
    getRyList();

}

//展开更多检索按钮
function btnSearch(flg) {
 
    remove_overlay();

    if (flg == 1) {
        $("#gcpageindex").val("1");
        $("#listcontent").empty();
        gclistAll = new Array();
        getGcList();
        addMarker(gclistAll);
    }

    if (flg == 2) {
        $("#qypageindex").val("1");
        $("#qycontent").empty();
        qylistAll = new Array();
        getQyList();
        addMarker2(qylistAll);
    }

    if (flg == 3) {
        $("#rypageindex").val("1");
        $("#rycontent").empty();
        rylistAll = new Array();
        getRyList();
        addMarker4(rylistAll);
    }

}

//获取更多工程,企业,人员
function morelist(type) {
    if (type=="1") {
        var pageindex = parseInt($("#gcpageindex").val());
        pageindex = pageindex + 1;
        $("#gcpageindex").val(pageindex)
        getGcList();
    }
    if (type == "2") {
        var pageindex = parseInt($("#qypageindex").val());
        pageindex = pageindex + 1;
        $("#qypageindex").val(pageindex)
        getQyList();
    }
    if (type == "3") {
        var pageindex = parseInt($("#rypageindex").val());
        pageindex = pageindex + 1;
        $("#rypageindex").val(pageindex)
        getRyList();
    }
}

//获取工程一览
function getGcList() {

    var province = encodeURIComponent($("#province10").val());
    var city = encodeURIComponent($("#city10").val());
    var district = encodeURIComponent($("#district10").val());
    var qylx = $("#selectQylx").val();
    var qybh = $("#selectQy").val();
    var gczt = $("#selectGczt").val();
    var gcmc = encodeURIComponent($("#txtGcmc").val());
    var pageindex = $("#gcpageindex").val();

    //相关数据取得
    $.ajax({
        type: "POST",
        url: "/welcome/GetGcList?province=" + province + "&city=" + city + "&district=" + district + "&qylx=" + qylx + "&qybh=" + qybh + "&gczt=" + gczt + "&gcmc=" + gcmc + "&page=" + pageindex,
        dataType: "json",
        success: function (result) {
            gclist = result.data;

            setGclist();

            if (tabId == "GcList") {
                addMarker(gclist);
                gc_mapstyle();
            }

            //if (tabId == "GcList") {
            //    //地图块
            //    //remove_overlay();
            //    $(".BMap_noprint").hide();
            //    gc_mapstyle()
            //    setGclist();
            //}

            ////一览块
            //initGcTable();
        }
    });
}

//获取企业一览
function getQyList() {

    var province = encodeURIComponent($("#province10").val());
    var city = encodeURIComponent($("#city10").val());
    var district = encodeURIComponent($("#district10").val());
    var qylx = $("#selectQylx").val();
    var qybh = $("#selectQy").val();
    var qymc = encodeURIComponent($("#txtQymc").val());
    var pageindex = $("#qypageindex").val();

    //相关数据取得
    $.ajax({
        type: "POST",
        url: "/welcome/GetQyList?province=" + province + "&city=" + city + "&district=" + district + "&qylx=" + qylx + "&qybh=" + qybh + "&qymc=" + qymc + "&page=" + pageindex,
        dataType: "json",
        success: function (result) {
            qylist = result.data;

            setQylist();

            if (tabId == "QyList") {
                addMarker2(qylist);
                qy_mapstyle();
            }

            //if (tabId == "QyList") {
            //    $(".BMap_noprint").hide();
            //    qy_mapstyle()
            //    setQylist();
            //}

            //一览块
            //initQyTable();
        }
    });
}

//获取人员一览
function getRyList() {

    var province = encodeURIComponent($("#province10").val());
    var city = encodeURIComponent($("#city10").val());
    var district = encodeURIComponent($("#district10").val());
    var qylx = $("#selectQylx").val();
    var qybh = $("#selectQy").val();
    var gczt = $("#selectGczt").val();
    var gcmc = encodeURIComponent($("#txtGcmcForRy").val());
    var key = encodeURIComponent($("#txtRymc").val());
    var pageindex = $("#rypageindex").val();

    //相关数据取得
    $.ajax({
        type: "POST",
        url: "/welcome/GetRyList?province=" + province + "&city=" + city + "&district=" + district + "&qylx=" + qylx + "&qybh=" + qybh + "&gczt=" + gczt + "&gcmc=" + gcmc + "&key=" + key + "&page=" + pageindex,
        dataType: "json",
        success: function (result) {
            rylist = result.data;

            setRylist();

            if (tabId == "RyList") {
                addMarker4(rylist);
                ry_mapstyle();
            }

            //if (tabId == "RyList") {
            //    $(".BMap_noprint").hide();
            //    ry_mapstyle()
            //    setRylist();
            //}

            ////一览块
            //initRyTable();

            //tab切换事件
            tabChange();
        }
    });
}

//tab切换
function tabChange() {
    //tab切换事件
    layui.use('element', function () {
        var element = layui.element;
        //切换模块是加载不同的内容
        element.on('tab(docDemoTabBrief)', function () {
            //切换时的数据
            var layid = this.getAttribute('id');
            if (layid == "GcList") {
                tabId = "GcList";
                remove_overlay();
                $(".BMap_noprint").hide();
                gc_mapstyle()
                addMarker(gclistAll);
                mapoverlay();
            } else if (layid == "QyList") {
                tabId = "QyList";
                remove_overlay();
                $(".BMap_noprint").hide();
                qy_mapstyle();
                addMarker2(qylistAll);
                mapoverlay();
            } else {
                tabId = "RyList";
                remove_overlay();
                $(".BMap_noprint").hide();
                ry_mapstyle();
                addMarker4(rylistAll);
                mapoverlay();
            }

        });
    });
}

function mapoverlay() {
    var mapType2 = new BMap.MapTypeControl({
        anchor: BMAP_ANCHOR_TOP_RIGHT,
        offset: new BMap.Size(10, 50)
    });

    map.addControl(mapType2);
    var top_right_navigation = new BMap.NavigationControl({
        anchor: BMAP_ANCHOR_TOP_RIGHT,
        type: BMAP_NAVIGATION_CONTROL_SMALL,
        offset: new BMap.Size(50, 80)
    }); //右上角，仅包含平移和缩放按钮
    map.addControl(top_right_navigation);
}

//清除覆盖物
function remove_overlay() {
	map.clearOverlays();
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
}

//获取行政区域
function getBoundary(getcity) {
	var bdary = new BMap.Boundary();
	bdary.get(getcity, function(rs) { //获取行政区域
		map.centerAndZoom(getcity, 11); //放大地图
		//map.clearOverlays();
		var count = rs.boundaries.length; //行政区域的点有多少个
		if(count === 0) {
			alert('未能获取当前输入行政区域');
			return;
		}
		//var pointArray = [];
		//for(var i = 0; i < count; i++) {
		//	var ply = new BMap.Polygon(rs.boundaries[i], {
		//		strokeWeight: 2,
		//		strokeColor: "#ff0000"
		//	}); //建立多边形覆盖物
		//map.addOverlay(ply); //添加覆盖物
		//	pointArray = pointArray.concat(ply.getPath());
		//}
		//map.setViewport(pointArray); //调整视野
		// addlabel();
	});
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

//工程list
function setGclist() {
    gclistAll = gclistAll.concat(gclist)
	var listdata = gclist;
	var singledata = "";
	$.each(listdata, function(index, value) {
	    singledata += "<div class='gcsinglelist clearfix'><div class='singlelist_left'><p>工程名称:" + value.gcmc + "</p><p>工程地点:" + value.gcdd + "</p><p>工程状态:" + value.sy_gczt + "</p></div></div>"
	});
	$("#listcontent").append(singledata);
	//remove_overlay();
	//addMarker(listdata);
	$(".gcsinglelist").click(function () {
		//remove_overlay();
		var singleclick = [];
		var num = $(this).index();
		singleclick.push(gclistAll[num]);
		addMarker(singleclick);
	});
}

//在地图上表示出来的点
function addMarker(points) {
    for (var i = 0; i < points.length; i++) {
        var gcbz = points[i].gcbz;
        var jd;
        var wd;
        if (gcbz == "") {
            gcbz = "120.709955,28.016725";
        }
        if (gcbz != "") {
            jd = gcbz.split(",")[0];
            wd = gcbz.split(",")[1];

            var point = new BMap.Point(jd, wd);
            if (points[i].sy_gczt == "在建工程") {
                var myIcon = new BMap.Icon("/skins/default/welcomezs/images/zj.png", new BMap.Size(95, 95));
            } else if (points[i].sy_gczt == "竣工工程") {
                var myIcon = new BMap.Icon("/skins/default/welcomezs/images/jg.png", new BMap.Size(95, 95));
            } else {
                var myIcon = new BMap.Icon("/skins/default/welcomezs/images/tg.png", new BMap.Size(95, 95));
            }

            var marker = new BMap.Marker(point, {
                icon: myIcon
            });
            map.addOverlay(marker);

            //单个坐标的话设置为中心,并显示详细
            if (points.length == 1) {
                map.centerAndZoom(point, 12);
                showInfo(marker, points[0]);
            } else if (i == 0) {
                showInfo(marker, points[0]);
            }

            (function () {
                var infopoint = points[i];
                var gcbh = points[i].gcbh;
                marker.addEventListener("onclick", function (e) {
                    showInfo(this, infopoint);
                });
            })();
        }

    }
}

//点击不同的图标获取显示不同的值
function showInfo(thisMaker, point) {
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
    //var addnumber = point;
    //infoWindow.addEventListener("open", function(type, target, point) {
    //	$(".bigbox_bottom_left").bind('click', function() {
    //		assignment(addnumber.gcbh);
    //	});
    //	$(".bigbox_bottom_right").click(function() {
    //		$("#enginner_bigbox_left").html("");
    //		dosearch(addnumber.latitude, addnumber.longitude);
    //	});
    //})
    thisMaker.openInfoWindow(infoWindow);
}

//企业list
function setQylist() {
    qylistAll = qylistAll.concat(qylist)
	var qydata = qylist;
	var qysingle = "";
	$.each(qydata, function(index, value) {
	    qysingle += "<div class='qysinglelist clearfix'><div class='singlelist_left'><p>企业名称:" + value.qymc + "</p><p>企业地点:" + (value.szsf + value.szcs + value.szxq) + "</p><p>企业类型:" + value.lxmc + "</p><p>是否黑名单:" + (typeof (value.sfhmd) == "undefined" ? "否" : "是") + "</p></div></div>"
	});
	$("#qycontent").append(qysingle);
	//addMarker2(qydata);
	$(".qysinglelist").click(function () {
		remove_overlay();
		var singleclick = [];
		var num = $(this).index();
		singleclick.push(qylistAll[num]);
		addMarker2(singleclick);
	});
}

function addMarker2(points) {
    for (var i = 0; i < points.length; i++) {
        if (points[i].lxbh != "") {

            var jd = points[i].lon;;
            var wd = points[i].lat;
            if (jd == "") {
                var jd = "120.709955";
                var wd = "28.016725";
            }

            var point = new BMap.Point(jd, wd);
            var myIcon = new BMap.Icon("/skins/default/welcomezs/images/qy.png", new BMap.Size(95, 95));
            var marker = new BMap.Marker(point, {
                icon: myIcon
            });
            map.addOverlay(marker);


            //单个坐标的话设置为中心
            if (points.length == 1) {
                map.centerAndZoom(point, 12);
            }

            (function () {
                var infopoint = points[i];
                var gcbh = points[i].gcbh;
                marker.addEventListener("click", function () {
                    showInfo2(this, infopoint);
                });

            })();
        }

    }
}

function showInfo2(thisMaker, point) {
    var opts = {
        width: 500, // 信息窗口宽度
        height: 200, // 信息窗口高度
    }
    var factorysingle = " ";
    //factorysingle += "<div id='factory_bigbox' class='clearfix'><P class='factory_bigbox_title'>企业名称:" + point.qymc + "</p><div id='factory_bigbox_left'><p class='factory_bigbox_number'><p class='enginner_bigbox_all'>单位下所有工程:</p><p>" + point.gcmc1 + "</p><p>" + point.gcmc2 + "</p><p>" + point.gcmc3 + "</p><p>" + point.gcmc4 + "</p></div><div id='factory_bigbox_right'><img src='data:image/png;base64," + point.qyzp + "'/></div></div>"
    factorysingle += "<div id='enginner_bigbox'><p class='enginner_bigbox_title'>企业名称:" + point.qymc + "</p><div id='enginner_bigbox_left'><p class='enginner_bigbox_all'>企业类型:" + point.lxmc + "</p><p class='enginner_bigbox_all'>企业负责人:" + point.qyfzr + "</p><p class='enginner_bigbox_all'>联系电话:" + point.lxdh + "</p></div></div>";

    var infoWindow = new BMap.InfoWindow(factorysingle, opts);
    //var addnumber = point;
    //infoWindow.addEventListener("open", function(type, target, point) {
    //	$(".factory_button_left").bind('click', function() {
    //		assignment(addnumber.qybh);
    //	});
    //	$(".factory_button_right").click(function() {
    //		$("#factory_bigbox_left").html("");
    //		dosearch(addnumber.latitude, addnumber.longitude);
    //	});
    //});
    thisMaker.openInfoWindow(infoWindow);
}

//人员list
function setRylist() {
    rylistAll = rylistAll.concat(rylist)
	var rydata = rylist;
	var rysingle = "";
	$.each(rydata, function(index, value) {
	    rysingle += "<div class='rysinglelist clearfix'><div class='singlelist_left'><p>姓名:" + value.ryxm + "</p><p>身份证号:" + value.sfzhm + "</p><p>所属工程:" + value.gcmc + "</p><p>状态:" + (value.hasdelete ? "在职" : "离职") + "</p></div><div class='singlelist_right'><img src='data:image/png;base64," + ((value.zp == "" || typeof (value.zp) == "undefined") ? defimg : value.zp) + "'/></div></div>"
	});
	$("#rycontent").append(rysingle);
	//addMarker4(rydata);
	$(".rysinglelist").click(function () {
		remove_overlay();
		var singleclick = [];
		var num = $(this).index();
		singleclick.push(rylistAll[num]);
		addMarker4(singleclick);
	});
}

function tolskq(sfzh,sfbzfzr) {

    var index = parent.layer.open({
        type: 2,
        content: '/welcome/rylskq?&sfzh' + sfzh + '&sfbzfzr' + sfbzfzr,
        closeBtn: 0
    });

    layer.full(index);
}

function addMarker4(points) {
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
                var myIcon = new BMap.Icon("/skins/default/welcomezs/images/zz.png", new BMap.Size(95, 95));
            } else {
                var myIcon = new BMap.Icon("/skins/default/welcomezs/images/lz.png", new BMap.Size(95, 95));
            }

            var marker = new BMap.Marker(point, {
                icon: myIcon
            });
            map.addOverlay(marker);

            //单个坐标的话设置为中心
            if (points.length == 1) {
                map.centerAndZoom(point, 12);
            }

            (function () {
                var infopoint = points[i];
                var gcbh = points[i].gcbh;
                marker.addEventListener("click", function () {
                    showInfo4(this, infopoint);
                });

            })();
        }

    }
}

function showInfo4(thisMaker, point) {
    var opts = {
        width: 500, // 信息窗口宽度
        height: 250, // 信息窗口高度
    }
    var peoplesingle = "";
    peoplesingle += "<div id='people_bigbox' class='clearfix'><p class='people_bigbox_tttle'>姓名:" + point.ryxm + "</p><div class='people_bigbox_left'><p class='people_bigbox_name'>身份证号:" + point.sfzhm + "</p><p class='people_bigbox_name'>所在工程:" + point.gcmc + "</p><p class='people_bigbox_name'>工种:" + point.gz + "</p><p class='people_bigbox_name'>人员状态:" + (point.hasdelete ? "在职" : "离职") + "</p></div><div class='people_bigbox_right'><img src='data:image/png;base64," + ((point.zp == "" || typeof (point.zp) == "undefined") ? defimg : point.zp) + "'/></div></div></div>";
    var infoWindow = new BMap.InfoWindow(peoplesingle, opts);
    var addnumber = point;
    thisMaker.openInfoWindow(infoWindow);
}

//各选项changes事件绑定
function bindChange() {
	$("#province10").change(function() {
		var currentCity = $("#province10").val() + $("#city10").val() + $("#district10").val();
		if(currentCity == tempCity) {
			return;
		}
		tempCity = currentCity;
		cityChange();
	});
	$("#city10").change(function() {
		var currentCity = $("#province10").val() + $("#city10").val() + $("#district10").val();
		if(currentCity == tempCity) {
			return;
		}
		tempCity = currentCity;
		cityChange();
	});
	$("#district10").change(function() {
		var currentCity = $("#province10").val() + $("#city10").val() + $("#district10").val();
		if(currentCity == tempCity) {
			return;
		}
		tempCity = currentCity;
		cityChange();
	});

	$("#selectQylx").change(function() {
		qylxChange();
	});
	$("#selectQy").change(function() {
		qyChange();
	});
	$("#selectGczt").change(function() {
		gcztChange();
	});
}

//移除各选项changes事件绑定
function unbindChange() {
    return;
	//$("#province10").unbind();
	//$("#city10").unbind();
	//$("#district10").unbind();
	$("#selectQylx").unbind();
	$("#selectQy").unbind();
	$("#selectGczt").unbind();
}

//统计块初始化
function initStatistics() {
    //高度调整
    setHW();

    statistics0();
	statistics1();
	statistics2();
	statistics3();
	statistics4();
}

//统计图表宽高调整
function setHW(){

    //每个图表的宽度
    var bottomwidth = ($(".panel").width() - 60) / 3;
    $("#enginner_1").css("width", bottomwidth);
    $("#enginner_2").css("width", bottomwidth);
    $("#enginner_3").css("width", bottomwidth);
    $("#enginner_4").css("width", ($(".panel").width() - 20));

    //每个图表的高度
    var bottomheight = ($(".panel").height() - 140) / 2;
    $("#enginner_1").css("height", bottomheight);
    $("#enginner_2").css("height", bottomheight);
    $("#enginner_3").css("height", bottomheight);
    $("#enginner_4").css("height", bottomheight);
}

//八块内容
function statistics0() {

    var province = encodeURIComponent($("#province10").val());
    var city = encodeURIComponent($("#city10").val());
    var district = encodeURIComponent($("#district10").val());
    var qylx = $("#selectQylx").val();
    var qybh = $("#selectQy").val();
    var gczt = $("#selectGczt").val();

    //相关数据取得
    $.ajax({
        type: "POST",
        url: "/welcome/GetStatistics?province=" + province + "&city=" + city + "&district=" + district + "&qylx=" + qylx + "&qybh=" + qybh + "&gczt=" + gczt,
        dataType: "json",
        success: function (result) {

            var i = 0;
            var ids = new Array();

            var zgcs = result.rows[0].zgcs;
            var oldzgcs = $("#enginner_zgcs").html();
            oldzgcs = oldzgcs.replace("个", "");
            if (zgcs != oldzgcs) {
                $("#enginner_zgcs").empty();
                $("#enginner_zgcs").append(zgcs + "个");
                ids[i] = "enginner_zgcs";
                i = i + 1;
            } else {
                $('#enginner_zgcs').css('color', 'white');
            }

            var zjgcs = result.rows[0].zjgcs;
            var oldzjgcs = $("#enginner_zjgcs").html();
            oldzjgcs = oldzjgcs.replace("个", "");
            if (zjgcs != oldzjgcs) {
                $("#enginner_zjgcs").empty();
                $("#enginner_zjgcs").append(zjgcs + "个");
                ids[i] = "enginner_zjgcs";
                i = i + 1;
            } else {
                $('#enginner_zjgcs').css('color', 'white');
            }

            var zcry = result.rows[0].zcry;
            var oldzcry = $("#enginner_zcry").html();
            oldzcry = oldzcry.replace("个", "");
            if (zcry != oldzcry) {
                $("#enginner_zcry").empty();
                $("#enginner_zcry").append(zcry + "个");
                ids[i] = "enginner_zcry";
                i = i + 1;
            } else {
                $('#enginner_zcry').css('color', 'white');
            }

            var zgry = result.rows[0].zgry;
            var oldzgry = $("#enginner_zgry").html();
            oldzgry = oldzgry.replace("个", "");
            if (zgry != oldzgry) {
                $("#enginner_zgry").empty();
                $("#enginner_zgry").append(zgry + "个");
                ids[i] = "enginner_zgry";
                i = i + 1;
            } else {
                $('#enginner_zgry').css('color', 'white');
            }

            var dqry = result.rows[0].dqry;
            var olddqry = $("#enginner_dqry").html();
            olddqry = olddqry.replace("个", "");
            if (dqry != olddqry) {
                $("#enginner_dqry").empty();
                $("#enginner_dqry").append(dqry + "个");
                ids[i] = "enginner_dqry";
                i = i + 1;
            } else {
                $('#enginner_dqry').css('color', 'white');
            }

            var jhje = result.rows[0].jhje;
            var oldjhje = $("#enginner_jhje").html();
            oldjhje = oldjhje.replace("亿元", "");
            if (jhje != oldjhje) {
                $("#enginner_jhje").empty();
                $("#enginner_jhje").append(jhje + "亿元");
                ids[i] = "enginner_jhje";
                i = i + 1;
            } else {
                $('#enginner_jhje').css('color', 'white');
            }

            var dwje = result.rows[0].dwje;
            var olddwje = $("#enginner_dwje").html();
            olddwje = olddwje.replace("亿元", "");
            if (dwje != olddwje) {
                $("#enginner_dwje").empty();
                $("#enginner_dwje").append(dwje + "亿元");
                ids[i] = "enginner_dwje";
                i = i + 1;
            } else {
                $('#enginner_dwje').css('color', 'white');
            }

            var ffje = result.rows[0].ffje;
            var oldffje = $("#enginner_ffje").html();
            oldffje = oldffje.replace("亿元", "");
            if (ffje != oldffje) {
                $("#enginner_ffje").empty();
                $("#enginner_ffje").append(ffje + "亿元");
                ids[i] = "enginner_ffje";
                i = i + 1;
            } else {
                $('#enginner_ffje').css('color', 'white');
            }

            if (!firstflg) {
                spangled(ids);
            }


            //八块内容定时刷新，当有变更时，其他的也刷新
            if (ids.length > 0 && !firstflg) {
                // pageLoad();
            }
            firstflg = false;
        }
    });
}

var nLeft = 0;
//文字闪烁
function spangled(ids) {
    nLeft = 0;
    var timer = setInterval(function () {    //开启定时器
        nLeft++;

        for (var i = 0; i < ids.length; i++) {
            setTimeout(" $('#" + ids[i] + "').css('color','#fF0000')", 100); //第一次闪烁
            setTimeout("$('#" + ids[i] + "').css('color','#ccc')", 200); //第二次闪烁
        }

        if (nLeft == 5) {
            clearInterval(timer);    //清除定时器
        }

    }, 400);
}

//第一图
function statistics1() {

    var province = encodeURIComponent($("#province10").val());
    var city = encodeURIComponent($("#city10").val());
    var district = encodeURIComponent($("#district10").val());
    var qylx = $("#selectQylx").val();
    var qybh = $("#selectQy").val();

    //相关数据取得
    $.ajax({
        type: "POST",
        url: "/welcome/GetStatisticsQylb?province=" + province + "&city=" + city + "&district=" + district + "&qylx=" + qylx + "&qybh=" + qybh,
        dataType: "json",
        success: function (result) {
            var listdata = result.rows;
            var legenddata = [];
            var seriesdata = [];

            $.each(listdata, function (index, value) {
                legenddata.push(value.name);
                seriesdata.push(value);
            });

            var enginner_1 = document.getElementById("enginner_1");
            var myChart1 = echarts.init(enginner_1);
            var option1 = null;
            option1 = {
                backgroundColor: '#fff',
                title: {
                    text: '企业类别',
                    subtext: '共' + listdata.length + "种企业",
                    x: 'center'
                },
                tooltip: {
                    trigger: 'item',
                    formatter: "{a} <br/>{b} : {c} ({d}%)"
                },
                legend: {
                    orient: 'vertical',
                    left: 'left',
                    data: legenddata
                },
                series: [{
                    name: '企业类别',
                    type: 'pie',
                    radius: '55%',
                    center: ['50%', '60%'],
                    data: seriesdata,
                    itemStyle: {
                        emphasis: {
                            shadowBlur: 10,
                            shadowOffsetX: 0,
                            shadowColor: 'rgba(0, 0, 0, 0.5)'
                        }
                    }
                }]
            };;
            if (option1 && typeof option1 === "object") {
                myChart1.setOption(option1, true);
            }
            //点击图表跳转
            myChart1.on('click', function (param) {
                var name = param.name;
            });

        }
    });
}

//第二图
function statistics2() {

    var province = encodeURIComponent($("#province10").val());
    var city = encodeURIComponent($("#city10").val());
    var district = encodeURIComponent($("#district10").val());
    var qylx = $("#selectQylx").val();
    var qybh = $("#selectQy").val();
    var gczt = $("#selectGczt").val();


    //相关数据取得
    $.ajax({
        type: "POST",
        url: "/welcome/GetStatisticsGclb?province=" + province + "&city=" + city + "&district=" + district + "&qylx=" + qylx + "&qybh=" + qybh + "&gczt=" + gczt,
        dataType: "json",
        success: function (result) {
            var listdata = result.rows;
            var xAxisdata = [];
            var seriesdata = [];

            $.each(listdata, function (index, value) {
                xAxisdata.push(value.name);
                seriesdata.push(value.value);
            });

            var enginner_2 = document.getElementById("enginner_2");
            var myChart2 = echarts.init(enginner_2);
            var option2 = null;
            option2 = {
                backgroundColor: '#fff',
                title: {
                    text: '工程类别',
                    subtext: '共' + listdata.length + "类工程",
                    x: 'center'
                },
                color: ['#3398DB'],
                tooltip: {
                    trigger: 'axis',
                    axisPointer: { // 坐标轴指示器，坐标轴触发有效
                        type: 'shadow' // 默认为直线，可选为：'line' | 'shadow'
                    }
                },
                grid: {
                    left: '3%',
                    right: '4%',
                    bottom: '3%',
                    containLabel: true
                },
                xAxis: [{
                    type: 'category',
                    data: xAxisdata,
                    axisTick: {
                        alignWithLabel: true
                    }
                }],
                yAxis: [{
                    type: 'value'
                }],
                series: [{
                    name: '',
                    type: 'bar',
                    barWidth: '60%',
                    data: seriesdata
                }]
            };;

            if (option2 && typeof option2 === "object") {
                myChart2.setOption(option2, true);
            }

            //点击图表跳转
            myChart2.on('click', function (param) {
                var name = param.name;
            });

        }
    });
}

//第三图
function statistics3() {

    var province = encodeURIComponent($("#province10").val());
    var city = encodeURIComponent($("#city10").val());
    var district = encodeURIComponent($("#district10").val());
    var qylx = $("#selectQylx").val();
    var qybh = $("#selectQy").val();
    var gczt = $("#selectGczt").val();

    //相关数据取得
    $.ajax({
        type: "POST",
        url: "/welcome/GetStatisticsJg?province=" + province + "&city=" + city + "&district=" + district + "&qylx=" + qylx + "&qybh=" + qybh + "&gczt=" + gczt,
        dataType: "json",
        success: function (result) {

            var listdata = result.rows;
            var legenddata = [];

            var seriesname0 = listdata[0].name;
            var seriesdata0 = [];


            var seriesname1 = listdata[1].name;
            var seriesdata1= [];

            var seriesname2 = listdata[2].name;
            var seriesdata2 = [];

            var seriesname3 = listdata[3].name;
            var seriesdata3 = [];

            $.each(listdata, function (index, value) {
                legenddata.push(value.name);
            });
            seriesdata0.push(eval('(' + listdata[0].data + ')'));
            seriesdata1.push(eval('(' + listdata[1].data + ')'));
            seriesdata2.push(eval('(' + listdata[2].data + ')'));
            seriesdata3.push(eval('(' + listdata[3].data + ')'));

            if (null == listdata[3].data)
                var maxValue = 0
            else
                var maxValue = eval('(' + listdata[3].data + ')')[eval('(' + listdata[3].data + ')').length - 1].value;

            var enginner_3 = document.getElementById("enginner_3");
            var myChart3 = echarts.init(enginner_3);
            var option3 = null;

            option3 = {
                backgroundColor: '#fff',
                title: {
                    text: '务工人员籍贯分布',
                    left: 'center'
                },
                tooltip: {
                    trigger: 'item'
                },
                legend: {
                    orient: 'vertical',
                    left: 'left',
                    data: legenddata
                },
                visualMap: {
                    min: 0,
                    max: maxValue,
                    left: 'left',
                    top: 'bottom',
                    text: ['高', '低'], // 文本，默认为数值文本
                    calculable: true
                },
                toolbox: {
                    show: true,
                    orient: 'vertical',
                    left: 'right',
                    top: 'center',
                    feature: {
                        dataView: {
                            readOnly: false
                        },
                        restore: {},
                        saveAsImage: {}
                    }
                },
                series: [{
                    name: seriesname0,
                    type: 'map',
                    mapType: 'china',
                    roam: false,
                    label: {
                        normal: {
                            show: true
                        },
                        emphasis: {
                            show: true
                        }
                    },
                    data: seriesdata0[0]
                }, {
                    name: seriesname1,
                    type: 'map',
                    mapType: 'china',
                    label: {
                        normal: {
                            show: true
                        },
                        emphasis: {
                            show: true
                        }
                    },
                    data: seriesdata1[0]
                }, {
                    name: seriesname2,
                    type: 'map',
                    mapType: 'china',
                    label: {
                        normal: {
                            show: true
                        },
                        emphasis: {
                            show: true
                        }
                    },
                    data: seriesdata2[0]
                }, {
                    name: seriesname3,
                    type: 'map',
                    mapType: 'china',
                    label: {
                        normal: {
                            show: true
                        },
                        emphasis: {
                            show: true
                        }
                    },
                    data: seriesdata3[0]
                }
                ]
            };;
            if (option3 && typeof option3 === "object") {
                myChart3.setOption(option3, true);
            }

            //点击图表跳转
            myChart3.on('click', function (param) {
                var name = param.name;
                alert(name);
            });
        }
    });
}

//第四图
function statistics4() {
    var province = encodeURIComponent($("#province10").val());
    var city = encodeURIComponent($("#city10").val());
    var district = encodeURIComponent($("#district10").val());
    var qylx = $("#selectQylx").val();
    var qybh = $("#selectQy").val();
    var gczt = $("#selectGczt").val();

    //相关数据取得
    $.ajax({
        type: "POST",
        url: "/welcome/GetStatisticsGz?province=" + province + "&city=" + city + "&district=" + district + "&qylx=" + qylx + "&qybh=" + qybh + "&gczt=" + gczt,
        dataType: "json",
        success: function (result) {
            var listdata = result.rows;
            var xAxisdata = [];
            var seriesdata = [];

            $.each(listdata, function (index, value) {
                xAxisdata.push(value.name);
                seriesdata.push(value.value);
            });

            var enginner_4 = document.getElementById("enginner_4");
            var myChart4 = echarts.init(enginner_4);
            var option4 = null;
            option4 = {
                backgroundColor: '#fff',
                title: {
                    text: '务工人员工种分布',
                    subtext: '共' + listdata.length + "类工种",
                    x: 'center'
                },
                color: ['#3398DB'],
                tooltip: {
                    trigger: 'axis',
                    axisPointer: { // 坐标轴指示器，坐标轴触发有效
                        type: 'shadow' // 默认为直线，可选为：'line' | 'shadow'
                    }
                },
                grid: {
                    left: '3%',
                    right: '4%',
                    bottom: '3%',
                    containLabel: true
                },
                xAxis: [{
                    type: 'category',
                    data: xAxisdata,

                    axisTick: {
                        alignWithLabel: true
                    }
                }],
                yAxis: [{
                    type: 'value'
                }],
                series: [{
                    name: '',
                    type: 'bar',
                    barWidth: '60%',
                    data: seriesdata
                }]
            };;

            if (option4 && typeof option4 === "object") {
                myChart4.setOption(option4, true);
            }

            //点击图表跳转
            myChart4.on('click', function (param) {
                var name = param.name;
                alert(name);
            });
        }
    });
}

//工程一览初始化
function initGcTable() {

    var province = encodeURIComponent($("#province10").val());
    var city = encodeURIComponent($("#city10").val());
    var district = encodeURIComponent($("#district10").val());
    var qylx = $("#selectQylx").val();
    var qybh = $("#selectQy").val();
    var gczt = $("#selectGczt").val();
    var gcmc = encodeURIComponent($("#txtLableGcmc").val());

    layui.use('table', function () {
        var table = layui.table;

        table.render({
            elem: '#gcTable'
          , url: "/welcome/GetGcList?province=" + province + "&city=" + city + "&district=" + district + "&qylx=" + qylx + "&qybh=" + qybh + "&gczt=" + gczt + "&gcmc=" + gcmc
          , page: { //支持传入 laypage 组件的所有参数（某些参数除外，如：jump/elem） - 详见文档
              layout: ['limit', 'count', 'prev', 'page', 'next', 'skip'] //自定义分页布局
              //,curr: 5 //设定初始在第 5 页
            , groups: 1 //只显示 1 个连续页码
            , first: false //不显示首页
            , last: false //不显示尾页

          }
          , cols: [[
            { fixed: 'left', width: 60, align: 'center', templet: '#togczsTbl' },
            { field: 'gcmc', width: 300, title: '工程名称', fixed: 'left' },
            { field: 'cql', width: 300, title: '出勤情况', templet: '#cqlTpl' },
            { field: 'ffl', width: 300, title: '工资发放情况', templet: '#fflTpl' },
            { field: 'gclxbh', width: 100, title: '工程类型' },
            { field: 'sy_jsdwmc', width: 300, title: '建设单位' },
            { field: 'sgdwmc', width: 300, title: '施工单位' },
            { field: 'jldwmc', width: 300, title: '监理单位' },
            { field: 'kcdwmc', width: 300, title: '勘察单位' },
            { field: 'sjdwmc', width: 300, title: '设计单位' },
            { field: 'gczj', width: 120, title: '造价（万元）' },
            { field: 'jzmj', width: 160, title: '建筑面积（平方米）' },
            { field: 'sy_jhkgrq', width: 120, title: '计划开工日期' },
            { field: 'sy_jhjgrq', width: 120, title: '计划竣工日期' }
          ]]

        });

        //高度调整
        var tableheight = $(".panel").height() - 240;
        $(".layui-table-body").css("height", tableheight);
        $(".layui-tab-content").css("padding-top", "0px");
       
    });
}

//企业一览初始化
function initQyTable() {

    var province = encodeURIComponent($("#province10").val());
    var city = encodeURIComponent($("#city10").val());
    var district = encodeURIComponent($("#district10").val());
    var qylx = $("#selectQylx").val();
    var qybh = $("#selectQy").val();
    var gczt = $("#selectGczt").val();
    var qymc = encodeURIComponent($("#txtLableQymc").val());

    layui.use('table', function () {
        var table = layui.table;

        table.render({
            elem: '#qyTable'
          , url: "/welcome/GetQyList?province=" + province + "&city=" + city + "&district=" + district + "&qylx=" + qylx + "&qybh=" + qybh + "&gczt=" + gczt + "&qymc=" + qymc
          , page: { //支持传入 laypage 组件的所有参数（某些参数除外，如：jump/elem） - 详见文档
              layout: ['limit', 'count', 'prev', 'page', 'next', 'skip'] //自定义分页布局
              //,curr: 5 //设定初始在第 5 页
            , groups: 1 //只显示 1 个连续页码
            , first: false //不显示首页
            , last: false //不显示尾页

          }
          , cols: [[
            { field: 'qymc', width: 350, title: '企业名称', templet: '#qymcTpl' },
            { field: 'qybh', width: 100, title: '企业编号' },
            { field: 'lxmc', width: 100, title: '企业类型' },
            { field: 'qyfzr', width: 100, title: '法人代表' },
            { field: 'lxdh', width: 100, title: '资质等级' },
            { field: 'lrsj', width: 350, title: '录入时间' }
          ]]

        });

        //高度调整
        var tableheight = $(".panel").height() - 240;
        $(".layui-table-body").css("height", tableheight);
    });
}

//人员一览初始化
function initRyTable() {

    var province = encodeURIComponent($("#province10").val());
    var city = encodeURIComponent($("#city10").val());
    var district = encodeURIComponent($("#district10").val());
    var qylx = $("#selectQylx").val();
    var qybh = $("#selectQy").val();
    var gczt = $("#selectGczt").val();
    var gcmc = encodeURIComponent($("#txtLableGcmcForRy").val());
    var key = encodeURIComponent($("#txtLableRymc").val());

    layui.use('table', function () {
        var table = layui.table;

        table.render({
            elem: '#ryTable'
          , url: "/welcome/GetRyList?province=" + province + "&city=" + city + "&district=" + district + "&qylx=" + qylx + "&qybh=" + qybh + "&gczt=" + gczt + "&gcmc=" + gcmc + "&key=" + key
          , page: { //支持传入 laypage 组件的所有参数（某些参数除外，如：jump/elem） - 详见文档
              layout: ['limit', 'count', 'prev', 'page', 'next', 'skip'] //自定义分页布局
              //,curr: 5 //设定初始在第 5 页
            , groups: 1 //只显示 1 个连续页码
            , first: false //不显示首页
            , last: false //不显示尾页

          }
          , cols: [[
            { field: 'ryxm', width: 120, title: '姓名' },
            { field: 'xb', width: 100, title: '性别' },
            { field: 'sfzhm', width: 200, title: '身份证号' },
            { field: 'gcmc', width: 350, title: '所属工程' },
            { field: 'gz', width: 100, title: '工种' },
            { field: 'gw', width: 100, title: '岗位' },
            { field: 'bzfzrxm', width: 150, title: '班组负责人' }
          ]]

        });

        //高度调整
        var tableheight = $(".panel").height() - 240;
        $(".layui-table-body").css("height", tableheight);
    });
}

//展开更多检索按钮
function btnLableSearch(flg) {

    if (flg == 1) {
        initGcTable();
    }

    if (flg == 2) {
        initQyTable();
    }

    if (flg == 3) {
        initRyTable();
    }
}

function togczs(gcbh, gcmc) {

    //每个图表的宽度
    var w = ($(".panel").width()).toString() + "px";
    //每个图表的高度
    var h = ($(".panel").height()).toString() + "px";

    var index = layer.open({
        type: 2,
        content: '/welcome/welcomegcqp?gcbh=' + gcbh + '&gcmc=' + gcmc,
        area: [w, h]
    });

    layer.full(index);
}

function tolskq(gcbh, gcmc) {

}


function fullscreen() {

    //每个图表的宽度
    var w = ($(".panel").width() + 270).toString() + "px";
    //每个图表的高度
    var h = ($(".panel").height() + 50).toString() + "px";


    var index = parent.layer.open({
        type: 2,
        content: '/welcome/welcomezfqp',
        area: [w, h],
        closeBtn: 0
    });

    parent.layer.full(index);

    parent.$(".layui-layer-title").css("height", "0");
}

function unfullscreen() {
    parent.layer.closeAll();
}

//window.onresize = function () {
//    高度调整
//    setHW();
//    statistics1();
//    statistics2();
//    statistics3();
//    statistics4();
//}
