function view() {
    var selected = pubselect();
    if (selected == undefined)
        return;
    var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
    var tablename = encodeURIComponent("I_M_GC_KGBA"); 			// 表名
    var tablerecid = encodeURIComponent("JDZCH"); 	// 表主键
    var title = encodeURIComponent("信息查看"); 	// 标题
    var jydbh = encodeURIComponent(selected.JDZCH);
    var formdm = tablename;                             // 列表key名称
    var buttons ="";// encodeURIComponent("提交|TJ| "); // 按钮

    var rdm = Math.random();
    var url = "/datainput/Index?zdzdtable=" + zdzdtable +
        "&t1_tablename=" + tablename +
        "&t1_pri=" + tablerecid +
        "&t1_title=" + title +
        "&button=" + buttons +
        "&rownum=2" +
        "&view=true" +
        "&jydbh=" + jydbh +
        "&_=" + rdm;



    parent.layer.open({
        type: 2,
        title: "企业详情",
        shadeClose: true,
        shade: 0.8,
        area: ['95%', '95%'],
        content: url,
        end: function () {
        }
    });
}


function setGCKQJ()
{
	try{
		var selected = pubselect();
		if (selected == undefined)
			return;
		var qybh=selected.QYBH;
		var jdzch=selected.JDZCH;
		var kqlx=selected.KQLX;
		$.ajax({
            type: "POST",
            url: "/JX_Info/setgckqtype?qybh=" + encodeURIComponent(qybh)+"&jdzch="+encodeURIComponent(jdzch)+"&kqlx="+encodeURIComponent(kqlx),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0") {
                    alert("设置成功！");
                    searchRecord();
                } else {
                    if (data.msg == "")
                        data.msg = "设置失败";
                    alert(data.msg);
                }
            },
            complete: function (XMLHttpRequest, textStatus) {
            },
            beforeSend: function (XMLHttpRequest) {
            }
        });
			
	}
	catch(e)
	{
		alert(e);
	}
}
