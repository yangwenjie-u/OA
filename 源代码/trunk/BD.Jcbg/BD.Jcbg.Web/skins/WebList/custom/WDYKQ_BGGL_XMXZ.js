function showNext() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;

        var gcbh = selected.GCBH;
        var nexturl = "";
        var layerObj = undefined;
        parent.layer.open({
            type: 2,
            title: '查询条件',
            shadeClose: true,
            shade: 0.8,
            area: ['600px', '400px'],
            content: "/cxtj/gcqysjxz?gcbh=" + encodeURIComponent(gcbh) + "&nexturl=" + encodeURIComponent(nexturl),
            btn: ["确定", "取消"],
            yes: function (index) {
                var obj = window.parent[layerObj.find('iframe')[0]['name']].getValues();
                if (!obj.success)
                    return;
            },
            success: function (layero, index) {
                layerObj = layero;
            }
        });
    } catch (ex) {
        alert(ex);
    }
}