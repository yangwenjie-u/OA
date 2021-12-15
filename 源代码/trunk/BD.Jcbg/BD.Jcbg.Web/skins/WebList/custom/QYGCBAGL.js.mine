function view() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;

        parent.layer.open({
            type: 2,
            title: "工程详情",
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: "/jdbg/gcckwb?gcbh=" + encodeURIComponent(selected.GCBH),
            end: function () {
            }
        });
    } catch (ex) {
        alert(ex);
    }
}