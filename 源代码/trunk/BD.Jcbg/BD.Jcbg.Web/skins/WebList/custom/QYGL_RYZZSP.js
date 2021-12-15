function check() {
    var selected = pubselect();
    if (selected == undefined)
        return;

    var layerObj = undefined;

    parent.layer.open({
        type: 2,
        title: '人员资质申请审批',
        shadeClose: false,
        shade: 0.8,
        area: ['250px', '180px'],
        content: "/qy/qyzzsp",
        btn: ["保存", "关闭"],
        yes: function (index) {
            var checkvalue = window.parent[layerObj.find('iframe')[0]['name']].getValue();

            $.ajax({
                type: "POST",
                url: "/ry/checkryzz?recid=" + selected.RECID + "&checkoption=" + checkvalue,
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data.msg != "")
                        alert(data.msg);
                    parent.layer.closeAll();
                },
                complete: function (XMLHttpRequest, textStatus) {
                },
                beforeSend: function (XMLHttpRequest) {
                }
            });
        }
        ,
        success: function (layero, index) {
            layerObj = layero;
        },
        btn2: function (index) {
            parent.layer.closeAll();
        },
        end: function () {
            searchRecord();
        }
    });

}