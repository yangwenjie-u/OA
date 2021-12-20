

//新增制度类型
function GZLXAdd() {
    try {
        layer.open({
            type: 2,
            title: '新增制度类目',
            content: '/dhoa/GZZDLXEdit',
            shadeClose: true,
            shade: 0.8,
            area: ['990px', '350px'],
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}
function GZLXEdit() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;
        console.log(selected);
        layer.open({
            type: 2,
            title: '编辑制度类目',
            content: '/dhoa/GZZDLXEdit?typeid=' + selected.GGZDLXID,
            shadeClose: true,
            shade: 0.8,
            area: ['990px', '350px'],
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}
function GZLXDel() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;
        layer.confirm("删除制度类目后相关规章制度全部删除，请确认是否删除", { icon: 3, title: '提示' }, function (index) {
            ajaxTpl('/dhoa/GZZDLXDelete', {
                typeid: selected.GGZDLXID
            }, function (data) {
                if (data.msg) {
                    layer.msg(data.msg);
                }
                searchRecord();
            });
            layer.close(index);
        });
    } catch (e) {
        alert(e);
    }
}


function GZZDAdd() {
    try {
        layer.open({
            type: 2,
            title: '新增制度类型',
            content: '/dhoa/GZZDEdit' ,
            shadeClose: true,
            shade: 0.8,
            area: ['990px', '350px'],
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}


function GZZDEdit(gzzdid) {
    try {
        console.log(1)
        console.log(gzzdid)
        layer.open({
            type: 2,
            title: '修改规章制度',
            content: '/dhoa/GZZDEdit?gzzdid=' + gzzdid,
            shadeClose: true,
            shade: 0.8,
            area: ['990px', '350px'],
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}
function GZZDDel(gzzdid) {
    try {
        layer.confirm("请确认是否删除", { icon: 3, title: '提示' }, function (index) {
            ajaxTpl('/dhoa/GZZDDelete', {
                recid: gzzdid
            }, function (data) {
                if (data.msg) {
                    layer.msg(data.msg);
                }
                searchRecord();
            });
            layer.close(index);
        });
    } catch (e) {
        alert(e);
    }
}


function FormatOper(value, row, index) {
    var imgurl = "";
    try {
        if (value != "")
            imgurl += "<a onclick='GZZDEdit(\"" + row.GZZDID + "\")' style='cursor:pointer;color:#169BD5;' alt='修改'> 修改 </a><a onclick='GZZDDel(\"" + row.GZZDID + "\")' style='cursor:pointer;color:#169BD5;' alt='删除'> 删除 </a>";

    } catch (e) {
        alert(e);
    }
    return imgurl;
}

function FormatTrueOrFalse(value, row, index) {
    var imgurl = "";
    try {

    } catch (e) {
        alert(e);
    }
    return imgurl;
}


function FormatDownOss(value, row, index) {
    var imgurl = "";
    try {
        if (value != "")
            imgurl += "<a onclick='DownloadOss(\"" + row.GZZDID + "\")' style='cursor:pointer;color:#169BD5;' alt='下载文件'> 下载文件 </a>";
    } catch (e) { }
    return imgurl;
}

//下载oss文件
function DownloadOss() {
    alert("暂不支持");
}

