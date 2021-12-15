var page = 1;

var gcbh = "";
$(function() {
    init();
    Refresh();


})
//全局初始化
function init() {
    var obj = document.getElementById("gcbh");
    gcbh = obj.innerHTML;
    //鼠标滚动全屏
    var height = $(window).height();

    $('.panel').height(height);

    $("#enginner_1").height(height - 250);
    $("#enginner_2").height(height - 250);
    $("#enginner_3").height(height - 250);
    $("#enginner_4").height(height - 250);

    $("#item").height(height - 100);

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
            Refresh();
        }
    });
}

$("#enginner_dqry").click(function(event) {
    try {
        var url = "/kqj/currentworker?gcbh=" + gcbh;
        //window.parent.openPage("/kqj/currentworker?gcbh="+gcbh, "GCGL_DQRY", "当前在岗人员");
        parent.parent.openPage("/kqj/currentworker?gcbh=" + gcbh, "GCGL_DQRY", "当前在岗人员");
      
    } catch (e) {
        alert(e);
    }
});
$("#enginner_QYGL").click(function(event) {
    try {

       // parent.parent.openPage("/WebList/EasyUiIndex?FormDm=QYGL_GCRYBA&FormStatus=0&FormParam=PARAM--"+gcbh, "QYGL_GCRYBA", "实名制登记");
    } catch (e) {
        alert(e);
    }
});
$("#enginner_sbyc").click(function (event) {
    try {

        parent.parent.openPage("/WebList/EasyUiIndex?FormDm=I_M_KQJ_YC&FormStatus=0&FormParam=PARAM--" + gcbh, "QYGL_KQJYC", "异常设备查看");
    } catch (e) {
        console.log(e);
    }
});
$("#enginner_kqyc").click(function (event) {
    try {

        parent.parent.openPage("/WebList/EasyUiIndex?FormDm=KQYC_RY&FormStatus=0&FormParam=PARAM--" + gcbh, "KQYC_RY", "考勤异常查看");
    } catch (e) {
        console.log(e);
    }
});


function Refresh() {

    if (page == 1) {
        $("#top-fixed").show();
        RefreshPage1();
    } else if (page == 2) {
        $("#top-fixed").show();
        RefreshPage2();
    } else if (page == 3) {
        $("#top-fixed").hide();
    }
}


function ajaxTpl(url, params, handle) {
    $.ajax({
        url: url,
        type: 'post',
        data: params,
        dataType: 'json',
        success: function(data) {
            handle(data);
        },
        fail: function(err) {
            console.log(err);
        }
    });
}

function RefreshPage1() {
    getInfo_GC();
    GetGzRynum();
}

function RefreshPage2() {

}

// 工种数
function GetGzRynum() {
    ajaxTpl("/wzwgry/GetGzRynum?gcbh=" + gcbh, "", function (data) {
        enginner1(data);
        enginner2(data);
        enginner3(data);
        enginner4(data);
    });
}

function enginner1(data) {
    var data1 = [];
    var data2 = [];
    var data3 = [];
    var name = [];
    var val;
    for (var i = 0, len = data.length; i < len; i++) {
        val = data[i];
        name.push(val.Gz || "其他");
        data1.push(val.Datas.data1 || 0);
        data2.push(val.Datas.data2 || 0);
        data3.push(val.Datas.data3 || 0);
    }
    /*
    var option = {
        title: {
            text: "工种在岗人数统计",
            top: '3%',
            left: '4%',
        },
        tooltip: {
            trigger: 'axis',
            axisPointer: { // 坐标轴指示器，坐标轴触发有效
                type: 'shadow' // 默认为直线，可选为：'line' | 'shadow'
            }
        },
        legend: {
            type: 'scroll',
            orient: 'vertical',
            top: '3%',
            right: '10%',
            bottom: 20,

            itemWidth: 10,
            data: ["总人数", "在职人数", "在岗人数"]
        },
        grid: {
            left: '3%',
            right: '4%',
            bottom: '3%',
            top: '100px',
            containLabel: true
        },
        xAxis: {
            type: 'category',
            data: name
        },
        yAxis: {
            type: 'value'
        },
        series: [{
                name: "在岗人数",
                type: 'bar',
                stack: '总量',
                data: data3
            },
            {
                name: "在职人数",
                type: 'bar',
                stack: '总量',

                data: data2
            },
            {
                name: "总人数",
                type: 'bar',
                stack: '总量',

                data: data1
            }
        ],
        color: ["#6c8ed5", "#92a9df", "#d2dcf5"]
    };
    */
    var legendName = ["在岗人数", "在职人数", "总人数"];

    var option = getThreeBar('工种在岗人数统计', legendName, {
        data1: data3,
        data2: data2,
        data3: data1,
        name: name
    });
	 option.xAxis.axisLabel = {
        interval: 0, //横轴信息全部显示  
        rotate: -10, //-10度角倾斜显示  
    }
    var doc = document.getElementById('enginner_1');
    echarts.init(doc).setOption(option);
    doc.style.background = "white";
}

function enginner2(data) {
    var data1 = [];
    var data4 = [];
    var name = [],
        ary = ["考勤人数", "总人数"];
    var val;
    for (var i = 0, len = data.length; i < len; i++) {
        val = data[i];
        name.push(val.Gz || "其他");
        data1.push(val.Datas.data1 || 0);
        data4.push(val.Datas.data4 || 0);
    }
    /*   var option = {
           title: {
               text: "工种考勤人数统计",
               top: '3%',
               left: '4%',
           },
           tooltip: {
               trigger: 'axis',
               axisPointer: { // 坐标轴指示器，坐标轴触发有效
                   type: 'shadow' // 默认为直线，可选为：'line' | 'shadow'
               }
           },
           legend: {
               type: 'scroll',
               orient: 'vertical',
               top: '3%',
               right: '10%',
               bottom: 20,

               itemWidth: 10,
               data: ary
           },
           grid: {
               left: '3%',
               right: '4%',
               bottom: '3%',
               top: '100px',
               containLabel: true
           },
           xAxis: {
               type: 'category',
               data: name
           },
           yAxis: {
               type: 'value'
           },
           series: [{
               name: ary[1], //"考勤人数",
               type: 'bar',
               stack: '总量',

               data: data4
           }, {
               name: ary[0], //"总人数",
               type: 'bar',
               stack: '总量',

               data: data1
           }],
           color: ["#6c8ed5", "#d2dcf5"]
       };
     */
    var option = getTwoBar('工种考勤人数统计', ary, {
        data1: data4,
        data2: data1,
        name: name
    });
	 option.xAxis.axisLabel = {
        interval: 0, //横轴信息全部显示  
        rotate: -10, //-10度角倾斜显示  
    }
    var doc = document.getElementById('enginner_2');
    echarts.init(doc).setOption(option);
    doc.style.background = "white";
}

function enginner3(data) {
    var data1 = [];
    var data2 = [];
    var name = [];
    var legendName = ["实付统计", "应付工资"];
    var val;
    for (var i = 0, len = data.length; i < len; i++) {
        val = data[i];
        name.push(val.Gz || "其他");
        data2.push(val.Datas.shouldpaynum || 0);
        data1.push(val.Datas.bankpaynum || 0);
    }
    /*
    var option = {
        title: {
            text: "工种工资统计",
            top: '3%',
            left: '4%',
        },
        tooltip: {
            trigger: 'axis',
            axisPointer: { // 坐标轴指示器，坐标轴触发有效
                type: 'shadow' // 默认为直线，可选为：'line' | 'shadow'
            }
        },
        legend: {
            type: 'scroll',
            orient: 'vertical',
            top: '3%',
            right: '10%',
            bottom: 20,

            itemWidth: 10,
            data: legendName
        },
        grid: {
            left: '3%',
            right: '4%',
            bottom: '3%',
            top: '100px',
            containLabel: true
        },
        xAxis: {
            type: 'category',
            data: name
        },
        yAxis: {
            type: 'value'
        },
        series: [{
                name: legendName[1],
                type: 'bar',
                stack: '总量',

                data: data2
            },
            {
                name: legendName[0],
                type: 'bar',
                stack: '总量',
                data: data1
            }
        ],
        color: ["#6c8ed5", "#d2dcf5"]
    };
    */
    var option = getTwoBar('工种工资统计', legendName, {
        data1: data1,
        data2: data2,
        name: name
    });

 option.xAxis.axisLabel = {
        interval: 0, //横轴信息全部显示  
        rotate: -10, //-10度角倾斜显示  
    }
    var doc = document.getElementById('enginner_3');
    echarts.init(doc).setOption(option);
    doc.style.background = "white";
}

function enginner4(data) {
    var data1 = [];
    var data2 = [];
    var name = [];
    var legendName = ['占用床位', '总床位'];
    var val;
    for (var i = 0, len = data.length; i < len; i++) {
        val = data[i];
        name.push(val.Gz || "其他");
        data2.push(val.Datas.allcw || 0);
        data1.push(val.Datas.usecw || 0);
    }
    /*
    var option = {
        title: {
            text: "寝室床位占用统计",
            top: '3%',
            left: '4%',
        },
        tooltip: {
            trigger: 'axis',
            axisPointer: { // 坐标轴指示器，坐标轴触发有效
                type: 'shadow' // 默认为直线，可选为：'line' | 'shadow'
            },
            formatter: function(params) { //数据格式
                console.log(params);
                return params;
            }

        },
        legend: {
            type: 'scroll',
            orient: 'vertical',
            top: '3%',
            right: '10%',
            bottom: 20,

            itemWidth: 10,
            data: legendName
        },
        grid: {
            left: '3%',
            right: '4%',
            bottom: '3%',
            top: '100px',
            containLabel: true
        },
        xAxis: {
            type: 'category',
            data: name
        },
        yAxis: {
            type: 'value'
        },
        series: [{
            name: legendName[1],
            type: 'bar',
            stack: '总量',

            data: data2
        }, {
            name: legendName[0],
            type: 'bar',
            stack: '总量',
            data: data1
        }],
        color: ["#6c8ed5", "#d2dcf5"]
    };*/

    var option = getTwoBar('寝室床位占用统计', legendName, {
        data1: data1,
        data2: data2,
        name: name
    });

option.xAxis.axisLabel = {
        interval: 0, //横轴信息全部显示  
        rotate: -10, //-10度角倾斜显示  
    }
    var doc = document.getElementById('enginner_4');
    echarts.init(doc).setOption(option);
    doc.style.background = "white";
}


function getInfo_GC() {
    ajaxTpl("/wzwgry/getInfo_GC?gcbh="+gcbh, "", function(data) {
        if (data.Datas && data.Datas[0]) {
            var tmp = data.Datas[0];
            $("#smzry").html(tmp.smzry + "人");
            $("#dqry").html(tmp.dqry + "人");
            $("#yjrynum").html(tmp.yjrynum + "人");
            $("#kqycnum").html(tmp.kqycnum + "人");
            $("#sbycnum").html(tmp.sbycnum + "个");
        }
    });
}