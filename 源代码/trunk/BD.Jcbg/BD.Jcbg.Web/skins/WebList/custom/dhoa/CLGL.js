
function add() {
    try {

        layer.open({
            type: 2,
            title: '新增车辆',
            content: '/dhoa/CarEdit',
            shadeClose: true,
            shade: 0.8,
            area: ['1000px', '350px'],

            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}
function edit() {
    var selected = pubselects();
    if (selected == undefined)
        return;
    try {

        layer.open({
            type: 2,
            title: '修改车辆',
            content: '/dhoa/CarEdit?id=' + selected.id,
            shadeClose: true,
            shade: 0.8,
            area: ['1000px','75%'],
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }

}

function Del(id) {
   layer.confirm("是否确认删除", { icon: 3, title: '提示' }, function (index) {
       //ajaxTpl('/dhoa/AttendanceMachineDelete' , {
       //    id: id
       //}, function (data) {

       //    if (data.msg) {
       //        layer.msg(data.msg);
       //    }
       //      searchRecord();
       // });
        layer.close(index);
    });
}



function FormatOper(value, row, index) {
    var imgurl = "";
    try {
        imgurl += "<a onclick='edit(" + value + ")' style='cursor:pointer;color:#169BD5;' alt='修改'> 修改 </a><a onclick='Del(" + value +")' style='cursor:pointer;color:#169BD5;' alt='删除'> 删除 </a>";

    } catch (e) {
        alert(e);
    }
    return imgurl;
}


