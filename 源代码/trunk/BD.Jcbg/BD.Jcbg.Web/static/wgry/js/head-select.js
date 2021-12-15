//省
function GetProvince(flag) {
    ajaxGet("/welcome/GetProvinceList", "", function(data) {
        var str = "";

        for (var i = 0, len = data.length; i < len; i++) {
            if (i == 0) {
                str += "<option selected='selected' value='" + data[i].szsf + "'>" + data[i].szsf + "</option>";
            } else {
                str += "<option value='" + data[i].szsf + "'>" + data[i].szsf + "</option>";
            }
        }
        var val = $("#province10").removeAttr("disabled").append(str);
        if (flag) {
            CityList(1);
        }else{
            $("#province10").change();
        }
    });
}

//市
function CityList(flag) {
    var province = $("#province10").val();
    if (province != 0) {

        ajaxGet("/welcome/GetCityList?province=" + province, "", function(data) {
            var str = "";

            for (var i = 0, len = data.length; i < len; i++) {
                if (i == 0) {
                    str += "<option selected='selected' value='" + data[i].szcs + "'>" + data[i].szcs + "</option>";

                } else {
                    str += "<option value='" + data[i].szcs + "'>" + data[i].szcs + "</option>";

                }
            }

            var val = $("#city10").removeAttr("disabled").html(str);
            if (flag) {
                XQList(1);
            }else{
                $("#city10").change();
            }
        });

    }
}
//区
function XQList(flag) {
    var province = $("#province10").val();
    var city = $("#city10").val();
    if (province != 0 && city != 0) {

        ajaxGet("/welcome/GetXQList", {
            province: province,
            city: city
        }, function(data) {
            var str = "";
            var flag2 = false;
            if (data.length > 1) {
                str = "<option value='' selected='selected' >请选择</option>";
                flag2 = true;
            }
            for (var i = 0, len = data.length; i < len; i++) {
                if (i == 0) {
                    if(data.length == 1 )
                        str += "<option selected='selected' value='" + data[i].szxq + "'>" + data[i].szxq + "</option>";
                    else
                        str += "<option value='" + data[i].szxq + "'>" + data[i].szxq + "</option>";
                } else {
                    str += "<option value='" + data[i].szxq + "'>" + data[i].szxq + "</option>";
                }
            }


            var val = $("#district10").html(str);
            $("#district10").attr("val", "")
            
            if (flag) {
                if (flag2) {
                    cityChange();
                } else {
                    jDList();
                }
               
            }
            else {
                $("#district10").change();
            }
        });
    } else {
        $("#district10").val($("#district10 option:first").val());
        $("#street10").val($("#street10 option:first").val());
    }
}
//街道
function jDList() {
    var province = $("#province10").val();
    var city = $("#city10").val();
    var xq = $("#district10").val();
    if (province != 0 && city != 0 && xq != 0) {

        ajaxGet("/welcome/GetJDList?province=" + province + "&city=" + city + "&xq=" + xq, "", function(data) {
            var str = "<option value='' selected='selected'>全部</option>";

            for (var i = 0, len = data.length; i < len; i++) {
                if (i == 0) {
                    str += "<option value='" + data[i].szjd + "'>" + data[i].szjd + "</option>";
                } else {
                    str += "<option value='" + data[i].szjd + "'>" + data[i].szjd + "</option>";
                }
            }
            $("#street10").html(str).change();
        });

    } else {
        $("#street10").val($("#street10 option:first").val());
    }
}

$("#province10").change(function() {
    CityList();
});

$("#city10").change(function() {
    XQList();
    var currentCity = $("#province10").val() + $(this).val();
    if (currentCity == tempCity) {
        return;
    }
    tempCity = currentCity;
    cityChange();
});
$("#district10").change(function() {
    jDList();

    var currentCity = $("#province10").val() + $("#city10").val() + $(this).val();
    if (currentCity == tempCity) {
        return;
    }
    tempCity = currentCity;
    cityChange();
});
$("#street10").change(function() {
    // 街道用区的 行政区
    var currentCity = $("#province10").val() + $("#city10").val() + $("#district10").val();
    tempCity = currentCity;
    cityChange();
});

//省变更 市变更 区变更
function cityChange() {
    //地图定位
    getBoundary(tempCity);
    //全局初始化
    initvariable();
    //页面刷新
    pageLoad();
}

function bindChange() {
    //全部类型   建筑   交通
    $("#gcxz").change(function() {
        initvariable();
        pageLoad();
    });
    //所有企业 施工  监理   建设
    $("#selectQylx").change(function() {
        initvariable();
        pageLoad();
    });


    //所有工程  在建  竣工
    $("#selectGczt").change(function() {
        initvariable();
        Refresh()
    });

    //筛选工程名称/公司名称
    $("#keyVal").blur(function(event) {
        initvariable();
        Refresh()
    });

    //企业改变  改变企业下的工程 
    $("#selectQy").change(function(event) {
        var gcbh = $(this).val();
        if (gcbh != "") {
            ajaxTpl("/wzwgry/GetGcList_ByQybh", {
                gcbh: gcbh
            }, function(data) {
                var str = "<option value=''>所有工程</option>";
                for (var i = 0, len = data.Datas.length; i < len; i++) {
                    str += "<option value='" + data.Datas[i].gcbh + "'>" + data.Datas[i].gcmc + "</option>";
                }
                $("#selectGczt").html(str);
            });
        }
        initvariable();
        Refresh()
    });
}


//获取企业一览,成功后内容初始化
function pageLoad() {
    var province = $("#province10").val();
    var city = $("#city10").val();
    var district = $("#district10").val();
    var jd = $("#street10").val();

    var qylx = $("#selectQylx").val();
    var gcxz = $("#gcxz").val();

    if (type == 1) {
        //获取企业一览
        var data = {
            province: province,
            jd: jd,
            city: city,
            district: district,

            qylx: qylx,
            gclx: gcxz
        };

        $.ajax({
            type: "POST",
            url: "/wzwgry/GetSelectQyList",
            data: data,
            dataType: "json",
            success: function(result) {

                var str = "<option value=''  selected='selected'>请选择企业</option>"
                var d = result.rows;
                for (var i = 0; i < d.length; i++) {
                    str += "<option value='" + d[i].qybh + "'>" + d[i].qymc + "</option>";
                }
                $("#selectQy").html(str);
                $('#selectQy').selectpicker('refresh');
                //刷新
                Refresh();
            }
        });
    } else {
        //刷新
        Refresh();
    }
}


var paramsData = {};

function formChange(type) {
    var province = $("#province10").val();
    var city = $("#city10").val();
    var district = $("#district10").val();
    var jd = $("#street10").val();


    if (type) {
        //省 市 区 街道
        return {
            province: province,
            city: city,
            district: district,
            jd: jd
        };
    }

    var qylx = $("#selectQylx").val();
    var qybh = $("#selectQy").val();
    var gczt = $("#selectGczt").val();
    var gcxz = $("#gcxz").val();
    var key = $("#keyVal").val();

    paramsData = {
        province: province,
        city: city,
        district: district,
        jd: jd,
        qylx: qylx,
        qybh: qybh,
        gczt: gczt,
        gclx: gcxz,
        key: key
    };
}