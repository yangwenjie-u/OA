function FormatSj(value, row, index) {
    var imgurl = "";
    try {
        if (value == "有")
            imgurl += "<center><img src='/skins/default/images/list/set_green.png' title='有' ondblclick='showSj(\""+row.RECID+"\")'/></center>";
        else
            imgurl += "<center><img src='/skins/default/images/list/set_gray.png' title='无'/></center>";
    } catch (e) {
        alert(e);
    }
    return imgurl;
}
function showSj(recid) {
    parent.layer.open({
        type: 2,
<<<<<<< .mine
        title: '试验数据详情',
        shadeClose: false,
||||||| .r98
        title: '查看试验数据',
        shadeClose: true,
=======
        title: '试验数据详情',
        shadeClose: true,
>>>>>>> .r220
        shade: 0.8,
        area: ['98%', '98%'],
        content: "/jc/viewwtdsysjs?wtdwyh=" + encodeURIComponent(recid),
        end: function () {

        }
    });
}
function FormatQx(value, row, index) {
    var imgurl = "";
    try {
        if (value == "有")
            imgurl += "<center><img src='/skins/default/images/list/set_green.png' title='有'  ondblclick='showSj(\"" + row.RECID + "\")'/></center>";
        else
            imgurl += "<center><img src='/skins/default/images/list/set_gray.png' title='无'/></center>";
    } catch (e) {
        alert(e);
    }
    return imgurl;
}
function FormatSp(value, row, index) {
    var imgurl = "";
    try {
        if (value == "已上传")
            imgurl += "<center><img src='/skins/default/images/list/set_green.png' title='已上传' ondblclick='showSp(\"" + row.RECID + "\")'/></center>";
        else if (value == "未上传")
            imgurl += "<center><img src='/skins/default/images/list/set_blue.png' title='未上传' ondblclick='showSp(\"" + row.RECID + "\")'/></center>";
        else
            imgurl += "<center><img src='/skins/default/images/list/set_gray.png' title='无'/></center>";
    } catch (e) {
        alert(e);
    }
    return imgurl;
}

function FormatBg(value, row, index) {
    var imgurl = "";
    try {
        if (value == "是")
            imgurl += "<center><img src='/skins/default/images/list/set_green.png' title='是' ondblclick='showBg(\"" + row.RECID + "\")'/></center>";
        else
            imgurl += "<center><img src='/skins/default/images/list/set_gray.png' title='否'/></center>";
    } catch (e) {
        alert(e);
    }
    return imgurl;
}
function showBg(recid) {
    parent.layer.open({
        type: 2,
<<<<<<< .mine
        title: '报告详情',
        shadeClose: false,
||||||| .r98
        title: '查看试验报告',
        shadeClose: true,
=======
        title: '报告详情',
        shadeClose: true,
>>>>>>> .r220
        shade: 0.8,
        area: ['98%', '98%'],
        content: "/jc/viewwtdreport?wtdwyh=" + encodeURIComponent(recid),
        end: function () {

        }
    });
}
function FormatSxt(value, row, index) {
    var imgurl = "";
    try {
        if (value == "是")
            imgurl += "<center><img src='/skins/default/images/list/set_green.png' title='是'/></center>";
        else
            imgurl += "<center><img src='/skins/default/images/list/set_gray.png' title='否'/></center>";
    } catch (e) {
        alert(e);
    }
    imgurl += "";
    return imgurl;
}
function FormatJd(value, row, index) {
    var imgurl = "";
    try {
        if (value == "已委托")
            imgurl += "<center><img src='/skins/default/images/list/process_25.png' title='"+value+"'/></center>";
        else if (value == "已收样")
            imgurl += "<center><img src='/skins/default/images/list/process_50.png' title='" + value + "'/></center>";
        else if (value == "已试验")
            imgurl += "<center><img src='/skins/default/images/list/process_75.png' title='" + value + "'/></center>";
        else if (value == "已出报告")
            imgurl += "<center><img src='/skins/default/images/list/process_100.png' title='" + value + "'/></center>";
        else if (value == "已归档")
            imgurl += "<center><img src='/skins/default/images/list/process_orange-100.gif' title='" + value + "'/></center>";
    } catch (e) {
        alert(e);
    }
    imgurl += "";
    return imgurl;
}
function FormatYc(value, row, index) {
    var imgurl = "";
    try {
        var desc = getYcDesc(row.YCZT);
        if (value == "是")
            imgurl += "<center><img src='/skins/default/images/list/set_red.png' title='" + desc + "'  ondblclick='showYc(\"" + row.RECID + "\"," + row.YCZT + ")'/></center>";
        else
            imgurl += "<center><img src='/skins/default/images/list/set_gray.png' title='否'/></center>";
    } catch (e) {
        alert(e);
    }
    return imgurl;
}
function FormatSjc(value, row, index) {
    var imgurl = "";
    try {
        value = value * 1;
        if (value != 0)
        {
            var d = parseInt(value / (24 * 60));
            var h = parseInt((value % (24 * 60)) / 60);
            var m = value % 60;
            if (d > 0)
                imgurl += d + "天";
            if (h > 0)
                imgurl += h + "小时";
            if (m > 0)
                imgurl += m + "分钟";
        }
        
    } catch (e) {
        alert(e);
    }
    return imgurl;
}
function FormatSjcBg(value, row, index) {
    var imgurl = "";
    try {
        value = value * 1;
        if (value != 0) {
            var d = parseInt(value / (24 * 60));
            if (d > 0)
                imgurl += d + "天";
        }

    } catch (e) {
        alert(e);
    }
    return imgurl;
}
function viewWtd() {
    try {
        var tabledesc = "委托单";
        var selected = pubselect();
        if (selected == undefined)
            return;
        
        parent.layer.open({
            type: 2,
            title: '查看委托单',
            shadeClose: false,
            shade: 0.8,
            area: ['100%', '100%'],
            content: "/jc/viewwts?syxmbh=" + encodeURIComponent(selected.SYXMBH) + "&recid=" + encodeURIComponent(selected.RECID),
            end: function () {

            }
        });
<<<<<<< .mine
        /*
        parent.layer.open({
            type: 2,
            title: '委托单数据详情',
            shadeClose: false,
            shade: 0.8,
            area: ['98%', '98%'],
            content: "/jc/viewwtdalldatas?wtdwyh=" + encodeURIComponent(selected.RECID),
            end: function () {

            }
        });*/
||||||| .r98
=======
        /*
        parent.layer.open({
            type: 2,
            title: '委托单数据详情',
            shadeClose: true,
            shade: 0.8,
            area: ['98%', '98%'],
            content: "/jc/viewwtdalldatas?wtdwyh=" + encodeURIComponent(selected.RECID),
            end: function () {

            }
        });*/
>>>>>>> .r220
    } catch (e) {
        alert(e);
    }
}
/*按钮用到的函数*/
function viewSysj() {
    try {
        var tabledesc = "委托单";
        var selected = pubselect();
        if (selected == undefined)
            return;

        if (selected.SY_SJ == "无") {
            alert("请选择有试验数据的" + tabledesc);
            return;
        }
        var id = selected.RECID;

        showSj(id);

    } catch (e) {
        alert(e);
    }
}
/*按钮用到的函数*/
function viewBg() {
    try {
        var tabledesc = "委托单";
        var selected = pubselect();
        if (selected == undefined)
            return;

        if (selected.SY_BGBS == "否") {
            alert("请选择有报告的" + tabledesc);
            return;
        }
        var id = selected.RECID;

        showBg(id);
    } catch (e) {
        alert(e);
    }
}

function viewYc() {
    try {
        var tabledesc = "异常详情";
        var selected = pubselect();
        if (selected == undefined)
            return;
        
        showYc(selected.RECID, selected.YCZT);

    } catch (e) {
        alert(e);
    }
}


function getYcDesc(yczt) {
    var desc = "";
    var zt = yczt * 1;
    if ((zt & 1) > 0)
        desc += "委托单有修改,"
    if ((zt & 2) > 0)
        desc += "委托书字段未全部上传,"
    if ((zt & 4) > 0)
        desc += "自动采集数据有修改,"
    if ((zt & 8) > 0)
        desc += "自动采集有未保存数据,"
    if ((zt & 16) > 0)
        desc += "自动采集有重做数据,"
    if ((zt & 32) > 0)
        desc += "自动采集有重复试验,"
    if ((zt & 64) > 0)
        desc += "有重复报告,"
    if ((zt & 128) > 0)
        desc += "试验员未登记,"
    if ((zt & 256) > 0)
        desc += "试验员未到场,"
    if ((zt & 512) > 0)
        desc += "数据上传时间超差,"
    if ((zt & 1024) > 0)
        desc += "报告上传超差,"
    if (desc[desc.length - 1] == ',')
        desc = desc.substring(0, desc.length-1);
    return desc;
<<<<<<< .mine
}

function showYc(recid, yczt) {
    parent.layer.open({
        type: 2,
        title: '异常详情',
        shadeClose: false,
        shade: 0.8,
        area: ['98%', '98%'],
        content: "/jc/viewwtdyc?wtdwyh=" + encodeURIComponent(recid)+"&yczt="+yczt,
        end: function () {

        }
    });
}

||||||| .r98
}=======
}

function showYc(recid, yczt) {
    if (yczt == 0) {
        alert("请选择有异常的委托单");
        return;
    }

    parent.layer.open({
        type: 2,
        title: '异常详情',
        shadeClose: true,
        shade: 0.8,
        area: ['98%', '98%'],
        //content: "/jc/viewwtdyc?wtdwyh=" + encodeURIComponent(recid)+"&yczt="+yczt,
        content: "/jc/viewwtdxq?wtdwyh=" + encodeURIComponent(recid) + "&yczt=" + yczt,
        end: function () {

        }
    });
}

function FormatYw(value, row, index) {
    var imgurl = "";
    try {
        if (value == "是")
            imgurl += "<center><img src='/skins/default/images/list/set_red.png' title='是'/></center>";
        else
            imgurl += "<center><img src='/skins/default/images/list/set_gray.png' title='否'/></center>";
    } catch (e) {
        alert(e);
    }
    return imgurl;
}

function FormatXcsj(value, row, index) {
    var imgurl = "";
    try {
        if (value == "有")
            imgurl += "<center><img src='/skins/default/images/list/set_green.png' title='有' ondblclick='viewXcsjDlg(\"" + row.RECID + "\")'/></center>";
        else
            imgurl += "<center><img src='/skins/default/images/list/set_gray.png' title='无'/></center>";
    } catch (e) {
        alert(e);
    }
    imgurl += "";
    return imgurl;
}
function viewXcsj() {
    var selected = pubselect();
    if (selected == undefined)
        return;
    viewXcsjDlg(selected.RECID);
}
function viewXcsjDlg(recid) {
    if (recid == "00072229")
        parent.layer.open({
            type: 2,
            title: '静载数据',
            shadeClose: true,
            shade: 0.8,
            area: ['98%', '98%'],
            content: "http://wz.jzyglxt.com:20019/xcjc/viewdetail?id=WHYH_WL_JZ_1",
            end: function () {

            }
        });
    else
        parent.layer.open({
            type: 2,
            title: '动测数据',
            shadeClose: true,
            shade: 0.8,
            area: ['98%', '98%'],
            content: "http://wz.jzyglxt.com:20019/xcjc/viewdetail?id=WHYH_WL_DC_1",
            end: function () {

            }
        });

}
function FormatJztp(value, row, index) {
    var imgurl = "";
    try {
        if (value == "有")
            imgurl += "<center><img src='/skins/default/images/list/set_green.png' title='有' ondblclick='showJztp(\"" + row.RECID + "\")'/></center>";
        else
            imgurl += "<center><img src='/skins/default/images/list/set_gray.png' title='无'/></center>";
    } catch (e) {
        alert(e);
    }
    return imgurl;
}

function viewJztp() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;

        if (selected.SY_JZTP == "无") {
            alert("请选择有见证取样图片的委托单");
            return;
        }

        showJztp(selected.RECID);

    } catch (e) {
        alert(e);
    }
}

function showJztp(recid) {
    parent.layer.open({
        type: 2,
        title: '见证取样图片',
        shadeClose: true,
        shade: 0.8,
        area: ['98%', '98%'],
        content: "/jc/viewjzqytp?wtdwyh=" + encodeURIComponent(recid),
        end: function () {

        }
    });
}

function FormatXctp(value, row, index) {
    var imgurl = "";
    try {
        if (value == "有")
            imgurl += "<center><img src='/skins/default/images/list/set_green.png' title='有' ondblclick='showXctp(\"" + row.RECID + "\")'/></center>";
        else
            imgurl += "<center><img src='/skins/default/images/list/set_gray.png' title='无'/></center>";
    } catch (e) {
        alert(e);
    }
    return imgurl;
}


function viewXctp() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;

        if (selected.SY_XCTP == "无") {
            alert("请选择有现场图片的委托单");
            return;
        }

        showXctp(selected.RECID);

    } catch (e) {
        alert(e);
    }
}

function showXctp(recid) {
    parent.layer.open({
        type: 2,
        title: '现场检测图片',
        shadeClose: true,
        shade: 0.8,
        area: ['98%', '98%'],
        content: "/jc/viewxcjctp?wtdwyh=" + encodeURIComponent(recid),
        end: function () {

        }
    });
}

function FormatXcsp(value, row, index) {
    var imgurl = "";
    try {
        if (value == "已上传")
            imgurl += "<center><img src='/skins/default/images/list/set_green.png' title='已上传' ondblclick='showXcsp(\"" + row.RECID + "\")'/></center>";
        else if (value == "未上传")
            imgurl += "<center><img src='/skins/default/images/list/set_blue.png' title='未上传' ondblclick='showXcsp(\"" + row.RECID + "\")'/></center>";
        else
            imgurl += "<center><img src='/skins/default/images/list/set_gray.png' title='无'/></center>";
    } catch (e) {
        alert(e);
    }
    return imgurl;
}



function viewXcsp() {
    try {
        var tabledesc = "委托单";
        var selected = pubselect();
        if (selected == undefined)
            return;

        if (selected.SY_XCSP == "无") {
            alert("请选择有现场视频的" + tabledesc);
            return;
        }
        var recid = selected.RECID;
        showXcsp(recid);

    } catch (e) {
        alert(e);
    }
}

function showXcsp(recid) {
    parent.layer.open({
        type: 2,
        title: '现场视频',
        shadeClose: true,
        shade: 0.8,
        area: ['98%', '98%'],
        content: "/jc/viewwtdsp?wtdwyh=" + encodeURIComponent(recid)+"&xc=1",
        end: function () {

        }
    });
}

function FormatXcjk(value, row, index) {
    var imgurl = "";
    try {
        if (value == "有")
            imgurl += "<center><img src='/skins/default/images/list/set_green.png' title='有' ondblclick='showXcjk(\"" + row.RECID + "\")'/></center>";
        else
            imgurl += "<center><img src='/skins/default/images/list/set_gray.png' title='无'/></center>";
    } catch (e) {
        alert(e);
    }
    return imgurl;
}

function viewXcjk() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;

        if (selected.SY_XCSP == "无") {
            alert("请选择正在试验的现场委托单");
            return;
        }

        showXcsp(selected.RECID);

    } catch (e) {
        alert(e);
    }
}

function showXcjk(recid) {
    parent.layer.open({
        type: 2,
        title: '现场检测监控',
        shadeClose: true,
        shade: 0.8,
        area: ['98%', '98%'],
        content: "/jc/viewxcjcsp?wtdwyh=" + encodeURIComponent(recid),
        end: function () {

        }
    });
}

/*按钮用到的函数*/
function viewSp() {
    try {
        var tabledesc = "委托单";
        var selected = pubselect();
        if (selected == undefined)
            return;

        if (selected.SY_SP == "无") {
            alert("请选择有视频的" + tabledesc);
            return;
        }
        var recid = selected.RECID;
        showSp(recid);

        
    } catch (e) {
        alert(e);
    }
}

function showSp(recid) {

    parent.layer.open({
        type: 2,
        title: '采集视频',
        shadeClose: true,
        shade: 0.8,
        area: ['98%', '98%'],
        content: "/jc/viewwtdsp?wtdwyh=" + encodeURIComponent(recid),
        end: function () {

        }
    });

}>>>>>>> .r220
