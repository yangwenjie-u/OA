function add()
{
	try {
            var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
            var tablename = encodeURIComponent("I_M_RY_TS"); 			// 表名
            var tablerecid = encodeURIComponent("Recid"); 	// 表主键
            var title = encodeURIComponent("投诉内容"); 	// 标题
       
            //var custombutton = encodeURIComponent("提交|TJSJ|ryts('')");
            var buttons = encodeURIComponent("提交|TJ| | |添加成功"); // 按钮 
            var js = encodeURIComponent("ryts.js,idcardService.js");
            var callback = encodeURIComponent("ryts('')");

            var rdm = Math.random();
            var url = "/datainput/Index?zdzdtable=" + zdzdtable +
                "&t1_tablename=" + tablename +
                "&t1_pri=" + tablerecid +
                "&t1_title=" + title +
                "&callback=" + callback +
                "&button=" + buttons +
                "&rownum=2" +
                "&js=" + js +
				"&LX=N" +
                "&_=" + rdm;

            parent.layer.open({
                type: 2,
                title: '投诉录入',
                shadeClose: true,
                shade: 0.8,
                area: ['95%', '95%'],
                content: url,
                end: function () {
                    parent.layer.closeAll();
                    searchRecord();               
                }
            });
        } catch (e) {
            alert(e);
        }
}
function edit()
{
	try {
			var selected = pubselect();
			if (selected == undefined)
				return;
            var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
            var tablename = encodeURIComponent("I_M_RY_TS"); 			// 表名
            var tablerecid = encodeURIComponent("Recid"); 	// 表主键
            var title = encodeURIComponent("投诉内容"); 	// 标题
			var jydbh=encodeURIComponent(selected.RECID)

            var rdm = Math.random();
            var url = "/datainput/Index?zdzdtable=" + zdzdtable +
                "&t1_tablename=" + tablename +
                "&t1_pri=" + tablerecid +
                "&t1_title=" + title +
                "&rownum=2" +
				"&jydbh=" + jydbh +
				"&LX=N" +
                "&_=" + rdm;

            parent.layer.open({
                type: 2,
                title: '投诉查看',
                shadeClose: true,
                shade: 0.8,
                area: ['95%', '95%'],
                content: url,
                end: function () {
                    parent.layer.closeAll();
                    searchRecord();               
                }
            });
        } catch (e) {
            alert(e);
        }
}

function view()
{
	try {
			var selected = pubselect();
			if (selected == undefined)
				return;
            var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
            var tablename = encodeURIComponent("I_M_RY_TS"); 			// 表名
            var tablerecid = encodeURIComponent("Recid"); 	// 表主键
            var title = encodeURIComponent("投诉内容"); 	// 标题
			var jydbh=encodeURIComponent(selected.RECID)

            var rdm = Math.random();
            var url = "/datainput/Index?zdzdtable=" + zdzdtable +
                "&t1_tablename=" + tablename +
                "&t1_pri=" + tablerecid +
                "&t1_title=" + title +
                "&rownum=2" +
				"&jydbh=" + jydbh +
				"&LX=A" +
				"&View=true" +
                "&_=" + rdm;

            parent.layer.open({
                type: 2,
                title: '投诉查看',
                shadeClose: true,
                shade: 0.8,
                area: ['95%', '95%'],
                content: url,
                end: function () {
                    parent.layer.closeAll();
                    searchRecord();               
                }
            });
        } catch (e) {
            alert(e);
        }
}


function del() {
    try {
        var tabledesc = "工程";                // 表格描述
		
        var selected = pubselect();
        if (selected == undefined)
            return;
        if (!confirm("确定要删除所选的记录吗？")) {
            return;
        }

        $.ajax({
            type: "POST",
            url: "/delete/deleteryts?recid=" + encodeURIComponent(selected.RECID),
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