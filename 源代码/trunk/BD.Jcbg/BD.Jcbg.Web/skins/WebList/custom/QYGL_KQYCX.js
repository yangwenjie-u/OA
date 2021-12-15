function edit() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;

        var zdzdtable = encodeURIComponent("ZDZD_KQGZ"); // zdzd名
        var tablename = encodeURIComponent("KqjUserMonthLog"); 			// 表名
        var tablerecid = encodeURIComponent("RECID"); 	// 表主键
        var jydbh = encodeURIComponent(selected.RECID)   // 键值
        var title = encodeURIComponent("人员工资"); 	// 标题
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var js = "";//encodeURIComponent("idcardService.js,irisService.js");
		var param="KqjUserMonthLog,Workday,"+selected.GZDJ+"|KqjUserMonthLog,Shouldpay,"+selected.SY_ShouldPay;
		var nodbparam="KqjUserMonthLog.Shouldpay";
		var fieldparam = encodeURIComponent(param);
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&jydbh=" + jydbh +
			"&fieldparam=" + fieldparam +
			"&nodbparam=" + nodbparam +
            "&rownum=2" +
            "&js=" + js +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '填写人员薪资',
            shadeClose: true,
            shade: 0.8,
            area: ['95%', '95%'],
            content: url,
            end: function () {
                searchRecord();
            }
        });
    } 
	catch (e) {
        alert(e);
    }
}
function view() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;

        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_RY"); 			// 表名
        var tablerecid = encodeURIComponent("RYBH"); 	// 表主键
        var jydbh = encodeURIComponent(selected.RYBH)   // 键值
        var title = encodeURIComponent("人员信息"); 	// 标题
        var buttons = ""; // 按钮
        var rdm = Math.random();

        var s_tablename = encodeURIComponent("I_S_RY_RYZZ");
        var s_pri = encodeURIComponent("RYBH,RYBH");
        var s_title = encodeURIComponent("人员证书");

        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&jydbh=" + jydbh +
            "&rownum=2" +
            "&LX=N" +
            "&view=true" +
            "&t2_tablename=" + s_tablename +
            "&t2_pri=" + s_pri +
            "&t2_title=" + s_title +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '查看人员信息',
            shadeClose: true,
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