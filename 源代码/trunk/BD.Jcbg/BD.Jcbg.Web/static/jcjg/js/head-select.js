
var g_JgProv = []; //省
var g_JgCity = []; // 市
var g_JgCounty = []; // 区
var g_JgStreet = [];// 街道
function GetProvince() {
    ajaxPost("/jc/GetJgAreaList", "", function (data) {
        g_JgCity = [];
        g_JgCounty = [];
        g_JgProv = [];
        g_JgStreet = [];
        $.each(data.records, function (i, item) {            
            // 添加省
            var finds = g_JgProv.filter(function (pitem) {
                return pitem.sfid == item.sfid;
            });
            if (finds.length == 0) {
                g_JgProv.push({ "sfid": item.sfid, "szsf": item.szsf });
            }
            // 添加市
            finds = g_JgCity.filter(function (citem) {
                return citem.csid == item.csid;
            });
            if (finds.length == 0) {
                g_JgCity.push({ "sfid": item.sfid, "szsf": item.szsf, "csid": item.csid, "szcs": item.szcs });
            }
            // 添加区
            finds = g_JgCounty.filter(function (citem) {
                return citem.xqid == item.xqid;
            });
            if (finds.length == 0) {
                g_JgCounty.push({ "sfid": item.sfid, "szsf": item.szsf, "csid": item.csid, "szcs": item.szcs, "xqid": item.xqid, "szxq": item.szxq });
            }
            // 添加街道
            finds = g_JgStreet.filter(function (citem) {
                return citem.jdid == item.jdid;
            });
            if (finds.length == 0) {
                g_JgStreet.push({ "sfid": item.sfid, "szsf": item.szsf, "csid": item.csid, "szcs": item.szcs, "xqid": item.xqid, "szxq": item.szxq, "jdid": item.jdid, "szjd": item.szjd });
            }
        });
        var str = "";
        if (g_JgProv.length > 1 || g_JgProv.length == 0)
            str += "<option value=''>=不限=</option>";
        $.each(g_JgProv, function (i, item) {
            str += "<option value='" + item.sfid + "'>" + item.szsf + "</option>";
        });

        $("#province10").html(str);

        $("#province10").change();
    });
}



//市
function CityList() {
    var province = $("#province10").val();
    if (province == "") {
        $("#city10").html("<option value=''>=不限=</option>");
    } else {
        var finds = finds = g_JgCity.filter(function (citem) {
            return citem.sfid == province;
        });
        var str = "";
        if (finds.length > 1 || finds.length == 0)
            str += "<option value=''>=不限=</option>";
        $.each(finds, function (i, item) {
            str += "<option value='" + item.csid + "'>" + item.szcs + "</option>";
        });
        $("#city10").html(str);
    }
    $("#city10").change();
}
//区
function XQList() {
    var city = $("#city10").val();

    if (city == "") {
        $("#district10").html("<option value=''>=不限=</option>");
    } else {
        var finds = g_JgCounty.filter(function (citem) {
            return citem.csid == city;
        });
        var str = "";
        if (finds.length > 1 || finds.length == 0)
            str += "<option value=''>=不限=</option>";
        $.each(finds, function (i, item) {
            str += "<option value='" + item.xqid + "'>" + item.szxq + "</option>";
        });
        
        $("#district10").html(str);
    }
    $("#district10").change();
}
//街道
function jDList() {
    
    var xq = $("#district10").val();
    if (xq == "") {
        $("#street10").html("<option value=''>=不限=</option>");
    } else {
        var finds = g_JgStreet.filter(function (citem) {
            return citem.xqid == xq;
        });
        var str = "";
        if (finds.length > 1 || finds.length == 0)
            str += "<option value=''>=不限=</option>";
        $.each(finds, function (i, item) {
            str += "<option value='" + item.jdid + "'>" + item.szjd + "</option>";
        });
        $("#street10").html(str);
    }
    $("#street10").change();

}


//省变更 市变更 区变更
function cityChange() {
    //地图定位
    //getBoundary(tempCity);
    //全局初始化
    initvariable();
    //页面刷新
    pageLoad();
    // 数字统计
    statistics0();
    statistics1();
    statistics2();
    statistics3();
    statistics4();
}

function bindChange() {
    $("#province10").change(function () {
        CityList();
    });
    $("#city10").change(function () {
        XQList();
        var currentCity = $("#province10").val() + $(this).val();
        if (currentCity == tempCity) {
            return;
        }
        tempCity = currentCity;
        //cityChange();
    });
    $("#district10").change(function () {
        jDList();

        var currentCity = $("#province10").val() + $("#city10").val() + $(this).val();
        if (currentCity == tempCity) {
            return;
        }
        tempCity = currentCity;
        //cityChange();
    });
    $("#street10").change(function () {
        // 街道用区的 行政区
        var currentCity = $("#province10").val() + $("#city10").val() + $("#district10").val();
        tempCity = currentCity;
        cityChange();
    });


    // 检测机构变更
    $("#jcjg10").change(function () {
        statistics0();
    });
    // 工程变更
    $("#gc10").change(function () {
        statistics0();
    });

}


//获取企业一览,成功后内容初始化
function pageLoad() {
    var province = $("#province10").val();
    var city = $("#city10").val();
    var district = $("#district10").val();
    var jd = $("#street10").val();

    GetJggc(province, city, district, jd);
    
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
        gcxz: gcxz,
        key: key
    };
}


var g_JgJcjg = [];
function GetJcjg() {
    ajaxPost("/jc/GetJgJcjgList", "", function (data) {
        g_JgJcjg = [];
        g_JgJcjg = data.records;

        var str = "";
        if (g_JgJcjg.length > 1 || g_JgJcjg.length == 0)
            str += "<option value=''>=不限=</option>";
        $.each(g_JgJcjg, function (i, item) {
            str += "<option value='" + item.qybh + "'>" + item.qymc + "</option>";
        });

        $("#jcjg10").html(str);
        $("#enginner_jcjgzs").html(g_JgJcjg.length + "个");

    });
}

function GetJggc(sfid, csid, xqid, jdid) {
    ajaxPost("/jc/GetJgGcList", "sfid="+sfid+"&csid="+csid+"&xqid="+xqid+"&jdid="+jdid, function (data) {
        var gcs = data.records;

        var str = "";
        if (gcs.length > 1 || gcs.length == 0)
            str += "<option value=''>=不限=</option>";
        $.each(gcs, function (i, item) {
            str += "<option value='" + item.gcbh + "'>" + item.gcmc + "</option>";
        });

        $("#gc10").html(str);

    });
}