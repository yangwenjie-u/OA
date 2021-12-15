function add() {
    try{
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_LZZGY_ZH"); 			// 表名
        var tablerecid = encodeURIComponent("Recid"); 	// 表主键
        var title = encodeURIComponent("创建劳务员"); 	// 标题
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
     
        var js = encodeURIComponent("userService.js");
        var callback = encodeURIComponent("userService.createUserLWY('u','$$ZH$$','$$GCMC$$')");
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&js=" + js +
			"&preproc=InputCheckLzzgy|$I_M_LZZGY_ZH.ZH" +
            "&callback=" + callback +
            "&rownum=2" +
            "&LX=N"+
            "&_=" + rdm;
      
        parent.layer.open({
            type: 2,
            title: '创建劳务员',
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

function edit() {
   try{
	    var selected = pubselect();
        if (selected == undefined)
            return;
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_LZZGY_ZH"); 			// 表名
        var tablerecid = encodeURIComponent("Recid"); 	// 表主键
        var title = encodeURIComponent("修改劳务员"); 	// 标题
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var jydbh = encodeURIComponent(selected.RECID)   // 键值
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
			"&jydbh=" + jydbh +
            "&rownum=2" +
            "&LX=G"+
            "&_=" + rdm;
      
        parent.layer.open({
            type: 2,
            title: '修改劳务员',
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


function view() {
    try{
	    var selected = pubselect();
        if (selected == undefined)
            return;
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_LZZGY_ZH"); 			// 表名
        var tablerecid = encodeURIComponent("Recid"); 	// 表主键
        var title =""; 	// 标题
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var jydbh = encodeURIComponent(selected.RECID)   // 键值
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
			"&jydbh=" + jydbh +
            "&rownum=2" +
            "&LX=N"+
			"&view=true" +
            "&_=" + rdm;
      
        parent.layer.open({
            type: 2,
            title: '查看劳资专管员',
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: url,
            end: function () {
                
            }
        });
    } catch (e) {
        alert(e);
    }
}
function del(){
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;

        if (!confirm("确定要删除所选的记录吗？")) {
            return;
        }
        $.ajax({
            type: "POST",
            url: "/delete/DeletelZZGY?recid=" + encodeURIComponent(selected.RECID),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0") {
                    alert("删除成功！");
                    searchRecord();
                } else {
                    if (data.msg == "")
                        data.msg = "删除失败";
                    alert(data.msg);
                }
            },
            complete: function (XMLHttpRequest, textStatus) {
            },
            beforeSend: function (XMLHttpRequest) {
            }
        });
    } catch (e) {
        alert(e);
    }
}



function modifyphone() {
    var selected = pubselect();
    if (selected == undefined)
        return;
    var qybh = selected.QYBH;
    var lxrxm = selected.QYFZR;
    var sjhm = selected.LXSJ;
    parent.layer.open({
        type: 2,
        title: '修改企业联系人信息',
        shadeClose: true,
        shade: 0.8,
        area: ['800px', '500px'],
        content: '/user/modifyqyphone?&qybh=' + encodeURIComponent(qybh) + "&lxrxm=" + encodeURIComponent(lxrxm) + "&sjhm=" + encodeURIComponent(sjhm),
        end: function () {
            searchRecord();
        }
    });
}