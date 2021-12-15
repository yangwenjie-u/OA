function edit() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;

        var url = "";

        parent.layer.open({
            type: 2,
            title: '查看任务详情',
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: url,
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}