function getThreeBar(title, legend, obj) {
    for (var i = 0, len = obj.data1.length; i < len; i++) {
        obj.data3[i] = obj.data3[i] - obj.data2[i]; //总人数
        obj.data2[i] = obj.data2[i] - obj.data1[i];
    }

    var option = {
        title: {
            text: title, //属性1
            top: '3%',
            left: '4%',
        },
        tooltip: {
            trigger: 'axis',
            axisPointer: {
                type: 'shadow'
            },
            formatter: function(params) { //数据格式
                var tmp, str = '';
                if (params.length) {
                    str = params[0].axisValue + "<br/>";

                    var value = 0;
                    for (var i = 0, len = params.length; i < len; i++) {
                        tmp = params[i];
                        // data2 = data1+data2   data3 = data2+data3
                        value += parseFloat(tmp.value);
                        str += tmp.marker + tmp.seriesName + ":" + value + "<br/>";
                    }
                }
                return str;
            }
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
                barWidth: 32,
                data: obj.data1 //属性3
            },
            {
                name: legend[1],
                type: 'bar',
                stack: 'one',
                barWidth: 32,
                data: obj.data2
            },
            {
                name: legend[2],
                type: 'bar',
                stack: 'one',
                barWidth: 32,
                data: obj.data3
            }
        ],
        color: ["#6c8ed5", "#92a9df", "#d2dcf5"]
    };
    return option;
}

function getTwoBar(title, legend, obj) {
    for (var i = 0, len = obj.data1.length; i < len; i++) {
        obj.data2[i] = obj.data2[i] - obj.data1[i];
    }

    var option = {
        title: {
            text: title,
            top: '3%',
            left: '4%',
        },
        tooltip: {
            trigger: 'axis',
            axisPointer: { // 坐标轴指示器，坐标轴触发有效
                type: 'shadow' // 默认为直线，可选为：'line' | 'shadow'
            },
            formatter: function(params) { //数据格式
                var tmp, str = '';
                if (params.length) {
                    str = params[0].axisValue + "<br/>";

                    var value = 0;
                    for (var i = 0, len = params.length; i < len; i++) {
                        tmp = params[i];
                        // data2 = data1+data2
                        value += parseFloat(tmp.value);
                        str += tmp.marker + tmp.seriesName + ":" + value + "<br/>";
                    }
                }
                return str;
            }
        },
        legend: {
            type: 'scroll',
            orient: 'vertical',
            top: '3%',
            right: '10%',
            bottom: 20,
            selectedMode: false,
            itemWidth: 10,
            data: legend
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
            barWidth: 32,
            data: obj.data1
        }, {
            name: legend[1],
            type: 'bar',
            stack: 'one',
            barWidth: 32,
            data: obj.data2
        }],
        color: ["#6c8ed5", "#d2dcf5"]
    };
    return option;
}