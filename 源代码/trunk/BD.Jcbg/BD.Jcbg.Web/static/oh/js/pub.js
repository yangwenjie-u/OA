function getBar(title, xAxis, data) {
    var option = {
        title: {
            text: title,
            top: '3%',
            left: '10px',
        },
        tooltip: {
            trigger: 'axis',
            axisPointer: {
                type: 'shadow'
            }
            
            // trigger: 'item',
            // formatter: "{b}: {c}"
        },
        grid: {
            left: '3%',
            right: '4%',
            bottom: '3%',
            containLabel: true
        },
        xAxis: {
            type: 'category',
            data: xAxis,
            splitLine: {
                show: false
            },
            axisTick: {
                alignWithLabel: true
            }
        },
        yAxis: {
            type: 'value',
            splitLine: {
                show: false
            },
        },
        series: [{
            data: data,
            type: 'bar'
        }],
        color: ['#5674d6', '#1ecdbf', '#ea5d67', '#f7b96b', '#91c3f2', '#40c484', '#ff9570', '#84eac0', '#75a7ec']
    };
    return option;
}

function getPie(title, data) {
    if (data && !data.length) {
        data = [{
            value: '1',
            name: '暂无数据'
        }]
    }
    var option = {
        title: {
            text: title,
            top: '3%',
            left: '10px',
        },
        tooltip: {
            trigger: 'item',
            formatter: "{b}: {c} ({d}%)"
        },
        legend: {
            type: 'scroll',
            orient: 'vertical',
            top: '10%',
            right: '10%',
            bottom: 20,
            selectedMode: false,
            itemWidth: 10,
            data: data
        },
        series: [{
            type: 'pie',
            radius: ['20%', '60%'], //内环 外环宽度
            center: ['36%', '56%'], //中点位置
            avoidLabelOverlap: false,
            label: {
                normal: {
                    show: false
                }
            },
            labelLine: {

                normal: {
                    show: false
                }
            },
            data: data
        }],
        color: ['#5674d6', '#ea5d67', '#1ecdbf', '#f7b96b', '#91c3f2', '#40c484', '#ff9570', '#84eac0', '#75a7ec']
    }
    return option;
}


function getLine(title, xAxis, data) {
    if (!data.length) {
        data = [0, 0, 0];
        xAxis = [0, 0, 0]
    }
    var option = {
        title: {
            text: title,
            top: '3%',
            left: '10px',
        },
        tooltip: {
            trigger: 'axis'
        },
        legend: {
            type: 'scroll',
            orient: 'vertical',
            top: '10%',
            right: '10%',
            bottom: 20,
            selectedMode: false,
            // itemWidth: 10,
            data: data
        },
        grid: {
            left: '3%',
            right: '4%',
            bottom: '3%',
            containLabel: true
        },

        xAxis: {
            type: 'category',
            boundaryGap: false,
            data: xAxis
        },
        yAxis: {
            type: 'value'
        },
        series: data,
        color: ["#ea5d67"]
    }
    return option;

}

function getLiquidFill(title, data) {
    if (!data.length) {
        data = [0]
    }

    var option = {
        title: {
            text: title,
            top: '3%',
            left: 'center',
        },
        series: [{
            type: 'liquidFill',
            data: data,
            radius: '60%'
        }]
    };

    return option;
}

function getThreeBar(title, legend, obj) {

    for (var i = 0, len = obj.data1.length; i < len; i++) {

    }

    var option = {
        title: {
            text: title, //属性1
            top: '3%',
            left: '10px',
        },
        tooltip: {
            trigger: 'axis',
            axisPointer: {
                type: 'shadow'
            },
            // formatter: function(params) { //数据格式
            //     var tmp, str = '';
            //     if (params.length) {
            //         str = params[0].axisValue + "<br/>";
            //         var value = 0;
            //         for (var i = 0, len = params.length; i < len; i++) {
            //             tmp = params[i];
            //             // data2 = data1+data2   data3 = data2+data3
            //             value += parseFloat(tmp.value);
            //             str += tmp.marker + tmp.seriesName + ":" + value + "<br/>";
            //         }
            //     }
            //     return str;
            // }
        },
        legend: {
            type: 'scroll',
            orient: 'vertical',
            selectedMode: false,
            top: '3%',
            right: '10%',
            bottom: 20,
            itemWidth: 10,
            data: legend //属性2
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
            data: obj.name
        },
        yAxis: {
            type: 'value'
        },
        series: [{
                name: legend[0],
                type: 'bar',
                stack: 'one',
                barGap: '-100%',
                data: obj.data1 //属性3
            },
            {
                name: legend[1],
                type: 'bar',
                stack: 'one',
                barGap: '-100%',
                data: obj.data2
            },
            {
                name: legend[2],
                type: 'bar',
                stack: 'one',
                barGap: '-100%',
                data: obj.data3
            }
        ],
        color: ["#6c8ed5", "#92a9df", "#d2dcf5"]
    };
    return option;
}