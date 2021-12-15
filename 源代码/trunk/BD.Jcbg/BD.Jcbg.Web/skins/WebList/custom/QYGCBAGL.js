function view() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;

        parent.layer.open({
            type: 2,
<<<<<<< .mine
            title: "工程详情",
            shadeClose: false,
||||||| .r98
            title: selected.GCMC,
            shadeClose: true,
=======
            title: "工程详情",
            shadeClose: true,
>>>>>>> .r220
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