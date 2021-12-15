function statistics4(qybh) {
    if (qybh == undefined)
        qybh = "";
    ajaxTpl("/wzwgry/GetQYGzTj?qybh="+qybh, paramsData, function(result) {
        var data1 = [];
        var data2 = [];
        var data3 = [];
        var name = [];
        var legendName = ['在岗人数', '在职人数', '总人数'];
        var val;
        for (var i = 0, len = result.length; i < len; i++) {
            val = result[i];
            name.push(val.Gz || "其他");
            data1.push(val.Datas.data1 || 0);
            data2.push(val.Datas.data2 || 0);
            data3.push(val.Datas.data3 || 0);
        }

        var option = getThreeBar('务工人员工种分布', legendName, {
            data1: data3,
            data2: data2,
            data3: data1,
            name: name
        });

        var doc = document.getElementById('enginner_4');
        echarts.init(doc).setOption(option);
        doc.style.background = "white";
    });
}




//工程一览初始化
function initGcTable1(params, qybh) {
    var src = "/WebList/EasyUiIndex?FormDm=GCZS_QY_ZF&FormStatus=0&FormParam=PARAM--" + params+"|"+qybh;

    if ($("#gcTable").attr("src") != src) {
        $("#gcTable").attr("src", src);
    }
}
//企业一览初始化
function initQyTable1(params,qybh) {
    var src = "/WebList/EasyUiIndex?FormDm=QYZS_QY_ZF&FormStatus=0&FormParam=PARAM--" + qybh;

    if ($("#qyTable").attr("src") != src) {
        $("#qyTable").attr("src", src);
    }
}

//人员一览初始化
function initRyTable1(params, qybh) {
    var src = "/WebList/EasyUiIndex?FormDm=WGRYZS_QY_ZF&FormStatus=0&FormParam=PARAM--" + params + "|" + qybh;

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
        content: '/WzWgry/qyIndex',
        area: [w, h],
        closeBtn: 0
    });

    parent.layer.full(index);

    parent.$(".layui-layer-title").css("height", "0");
}

function unfullscreen() {
    parent.layer.closeAll();
}
//选择默认的省市区
function GetProvince(qybh) {

    ajaxTpl("/WZWGRY/GetQYGcXQ?qybh="+qybh, "", function(val) {

        if (val.Datas && val.Datas.length) {
            var province = val.Datas[0].szsf;

            ajaxGet("/welcome/GetProvinceList", "", function(data) {
                var str = "";

                for (var i = 0, len = data.length; i < len; i++) {

                    str += "<option value='" + data[i].szsf + "'>" + data[i].szsf + "</option>";

                }
                $("#province10").append(str);
                $("#province10").val(val.Datas[0].szsf);


                ajaxGet("/welcome/GetCityList?province=" + province, "", function(data) {
                    var str = "";

                    for (var i = 0, len = data.length; i < len; i++) {

                        str += "<option value='" + data[i].szcs + "'>" + data[i].szcs + "</option>";
                    }
                    $("#city10").html(str);
                    $("#city10").val(val.Datas[0].szcs);

                    ajaxGet("/welcome/GetXQList", {
                        province: province,
                        city: val.Datas[0].szcs
                    }, function(data) {
                        var str = "";

                        for (var i = 0, len = data.length; i < len; i++) {
                            str += "<option value='" + data[i].szxq + "'>" + data[i].szxq + "</option>";
                        }
						if (i > 1) {
                            str = "<option value=''>请选择</option>" + str;
                        }
                        $("#district10").html(str);
                        $("#district10").val(val.Datas[0].szxq).change();
                    });
                });
            });
        }
    });
}