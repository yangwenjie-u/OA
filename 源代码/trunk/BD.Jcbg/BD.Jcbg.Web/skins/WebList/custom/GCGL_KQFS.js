function edit()
{
	 try {
		 
		var selected = pubselect();
        if (selected == undefined) {
            return;
        }	
		var gcbh=selected.GCBH;
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_GC"); 			// 表名
        var tablerecid = encodeURIComponent("GCBH"); 	// 表主键
        var title = encodeURIComponent("人员信息"); 	// 标题
		var buttons = encodeURIComponent("提交|TJ| | |修改成功"); // 按钮
        var jydbh = encodeURIComponent(selected.GCBH);   // 键值
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
			"&jydbh=" + jydbh +
			"&button=" + buttons +
            "&rownum=2" +	
			"&LX=FS"+
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '考勤方式设置',
            shadeClose: true,
            shade: 0.8,
            area: ['85%', '85%'],
            content: url,
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}


