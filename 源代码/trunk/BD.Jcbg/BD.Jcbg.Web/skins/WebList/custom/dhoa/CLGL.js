
function add() {
    try {

        layer.open({
            type: 2,
            title: '新增考勤机',
            content: '/dhoa/CarEdit',
            shadeClose: true,
            shade: 0.8,
            area: ['600px', '300px'],

            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}
function edit(id) {
  
    try {

        layer.open({
            type: 2,
            title: '修改考勤机',
            content: '/dhoa/AttendanceMachineEdit?id=' + id,
            shadeClose: true,
            shade: 0.8,
            area: ['600px', '300px'],
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }

}

function Del(id) {
   layer.confirm("是否确认删除考勤机", { icon: 3, title: '提示' }, function (index) {
       ajaxTpl('/dhoa/AttendanceMachineDelete' , {
           id: id
       }, function (data) {

           if (data.msg) {
               layer.msg(data.msg);
           }
             searchRecord();
        });
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


