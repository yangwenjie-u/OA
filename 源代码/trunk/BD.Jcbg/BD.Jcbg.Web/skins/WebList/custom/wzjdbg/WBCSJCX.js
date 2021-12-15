function viewXq() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;

        parent.layer.open({
            type: 2,
            title: '查看试验数据',
            shadeClose: false,
            shade: 0.8,
            area: ['100%', '100%'],
            content: "/jc/viewsysj?sywyh=" + encodeURIComponent(selected.SYWYH),
            end: function () {

            }
        });
    } catch (e) {
        alert(e);
    }
}

function FormatSjc(value, row, index) {
    var imgurl = "";
    try {
        value = value * 1;
        if (value != 0) {
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