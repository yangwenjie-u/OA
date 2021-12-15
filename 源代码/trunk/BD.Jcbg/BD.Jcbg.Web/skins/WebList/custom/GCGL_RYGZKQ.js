function view()
{
	try{
				
		var selected = pubselect();
        if (selected == undefined) {
            return;
        }
		var gz=selected.GZ;
		var logday=selected.LogDay;
		var url = "/WebList/EasyUiIndex?FormDm=GCGL_RYKQ&FormStatus=1&FormParam=PARAM--" + gz+"|"+logday;
		parent.layer.open({
			type: 2,
			title: '今日工种考勤明细',
			shadeClose: true,
			shade: 0.8,
			area: ['95%', '95%'],
			content: url,
			end: function () {
				
			}
		});
	}
	catch(e)
	{
		alert(e);
	}
}
