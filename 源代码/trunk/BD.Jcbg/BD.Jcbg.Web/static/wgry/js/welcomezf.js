function statistics4() {

    ajaxTpl("/welcome/GetStatisticsGz", paramsData, function(result) {
        var listdata = result.rows;
        var xAxisdata = [];
        var seriesdata = [];

        $.each(listdata, function(index, value) {
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
                // subtext: '共' + listdata.length + "类工种",
                x: 'left',
                top: '3%',
                left: '4%',
            },
            color: ['#6f89dd'],
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
                splitLine: {
                    show: false
                },
                axisTick: {
                    alignWithLabel: true
                }
            }],
            yAxis: [{
                type: 'value',
                splitLine: {
                    show: false
                },
            }],
            series: [{
                name: '',
                type: 'bar',
                // barWidth: '60%',
                barWidth :'30',
                data: seriesdata
            }]
        };;

        if (option4 && typeof option4 === "object") {
            myChart4.setOption(option4, true);
        }

        //点击图表跳转
        myChart4.on('click', function(param) {
            var name = param.name;
            // alert(name);
        });
    });
}

//工程一览初始化
function initGcTable1(params) {
    // 
    var src = "/WebList/EasyUiIndex?FormDm=GCZS_ZF&FormStatus=0&FormParam=PARAM--" + params;

    if ($("#gcTable").attr("src") != src) {
        $("#gcTable").attr("src", src);
    }

}
//企业一览初始化
function initQyTable1(params) {
    var src = "/WebList/EasyUiIndex?FormDm=QYZS_ZF&FormStatus=0&FormParam=PARAM--" + params;

    if ($("#qyTable").attr("src") != src) {
        $("#qyTable").attr("src", src);
    }
}

//人员一览初始化
function initRyTable1(params) {

    var src = "/WebList/EasyUiIndex?FormDm=WGRYZS_ZF&FormStatus=0&FormParam=PARAM--" + params;

    if ($("#ryTable").attr("src") != src) {
        $("#ryTable").attr("src", src);
    }
}

function fullscreen() {

    //每个图表的宽度
    var w = ($(".panel").width() + 270).toString() + "px";
    //每个图表的高度
    var h = ($(".panel").height() + 50).toString() + "px";


    var index = parent.layer.open({
        type: 2,
        content: '/WzWgry/index',
        area: [w, h],
        closeBtn: 0
    });

    parent.layer.full(index);

    parent.$(".layui-layer-title").css("height", "0");
}

function unfullscreen() {
    parent.layer.closeAll();
}