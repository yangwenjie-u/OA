function add() {
    try {
		var selected = pubselect();
        if (selected == undefined)
            return;
		var jdzch=selected.JDZCH;
        parent.layer.open({
            type: 2,
            title: '工种录用',
            shadeClose: false,
            shade: 0.8,
            area: ['1200px', '95%'],
            content: "/WebList/EasyUiIndex?FormDm=QYGL_GCGZLR&FormStatus=0&FormParam=PARAM--"+encodeURIComponent(jdzch)+"&jdzch="+jdzch,
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}
