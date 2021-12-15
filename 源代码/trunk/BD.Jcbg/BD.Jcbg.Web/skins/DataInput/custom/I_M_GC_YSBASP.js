function rygzxx() {	
	try {
		var jdzch=GetCtrlValue('I_M_GC_YSBA.JDZCH');
		var url="/WebList/EasyUiIndex?FormDm=GCGL_GCINFO&FormStatus=0&FormParam=PARAM--"+encodeURIComponent(jdzch);
        parent.layer.open({
            type: 2,
            title: "工资详情",
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: url,
            end: function () {
            }
        });
    }
    catch (ex) {
        alert(ex);
    }
}
