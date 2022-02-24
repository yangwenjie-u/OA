
function Add() {
    try {

        layer.open({
            type: 2,
            title: '印章录入',
            content: '/dhoa/SignatureEdit',
            shadeClose: true,
            shade: 0.8,
            area: ['1000px', '300px'],
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}
function Give() {
    try {
        alert("功能正在开发");
        //layer.open({
        //    type: 2,
        //    title: '印章录入',
        //    content: '/dhoa/SignatureEdit',
        //    shadeClose: true,
        //    shade: 0.8,
        //    area: ['600px', '300px'],

        //    end: function () {
        //        searchRecord();
        //    }
        //}); dhoa/QZGL.js
    } catch (e) {
        alert(e);
    }
}

function Edit(id) {
    try {

        layer.open({
            type: 2,
            title: '修改印章',
            content: '/dhoa/SignatureEdit?id=' + id,
            shadeClose: true,
            shade: 0.8,
            area: ['1000px', '300px'],
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }

}

function Del() {
    var selected = pubselect();
    if (selected == undefined)
        return;

    layer.confirm("是否销毁印章", { icon: 3, title: '提示' }, function (index) {
        ajaxTpl('/dhoa/SignatureDelete', {
            recid: selected.Recid
        }, function (data) {

            if (data.msg) {
                layer.msg(data.msg);
            }
            searchRecord();
        });
        layer.close(index);
    });
}


//查看使用记录
function UseRecord(id, code, name) {
    var url = "/WebList/ElementIndex?FormDm=YZGL_YZGL&FormStatus=1&FormParam=PARAM--" + encodeURIComponent(id);
    parent.layer.open({
        type: 2,
        title: code + "-" + name + "用章记录",
        shadeClose: false,
        shade: 0.8,
        area: ['95%', '95%'],
        content: url,
        end: function () {
        }
    })
}

function FormatOper(value, row, index) {
    var imgurl = "";
    try {
        imgurl += "<a onclick='Edit(" + value + ")' style='cursor:pointer;color:#169BD5;' alt='修改'> 修改 </a>"
            + "<a onclick='UseRecord(\"" + value + "\",\"" + row.SignatureCode + "\",\"" + row.SignatureName + "\")' style='cursor:pointer;color:#169BD5;'> 查看使用记录 </a>";

    } catch (e) {
        alert(e);
    }
    return imgurl;
}


