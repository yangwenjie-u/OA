function add() {
    try {
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_WGRY"); 			// 表名
        var tablerecid = encodeURIComponent("Recid"); 	// 表主键
        var title = encodeURIComponent("人员信息"); 	// 标题
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var js = encodeURIComponent("userService.js,idcardService2.js,irisService2.js,ryly.js"); //idcardService2.js 有判断人员库中有没有该人员信息

        var callback = encodeURIComponent("downWgryIris('$$RYBH$$','$$HM$$','$$SFZHM$$','$$JDZCH$$')");
		var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&js=" + js +
            "&rownum=2" +
            "&preproc=InputCheckImgcry|$i_m_wgry.SFZHM|$i_m_wgry.JDZCH" +
			"&sufproc=InputCheckWGRYBzfzr|$i_m_wgry.SFZHM|$i_m_wgry.JDZCH" +
			"&callback=" + callback +
			"&LX=GC" +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '录入人员信息',
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
function view() {
    try {
        var selecteds = pubselects();
        if (selecteds == undefined) {
            return;
        }
        var selected = selecteds[0];

        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_WGRY"); 			// 表名
        var tablerecid = encodeURIComponent("Recid"); 	// 表主键
        var jydbh = encodeURIComponent(selected.RECID)   // 键值
        var title = encodeURIComponent("人员信息"); 	// 标题
        var buttons = ""; // 按钮

        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&jydbh=" + jydbh +
            "&rownum=2" +
            "&LX=XX" +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '查看人员信息',
            shadeClose: true,
            shade: 0.8,
            area: ['95%', '95%'],
            content: url,
            end: function () {
                //searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}
function del() {
    try {
        var selecteds = pubselects();
        if (selecteds == undefined) {

            return;
        }
        var recids = "";
		var jdzch="";
		var sfzhms="";
		var delfalse="";

        $.each(selecteds, function (index, obj) {
            recids += obj.RECID + ",";
			sfzhms += obj.SFZHM + ",";
			if(jdzch!=""&&jdzch != obj.JDZCH)
			{
				alert("只能批量辞退相同工程的人员！");
				delfalse="1";
				return false;
			}
			if(jdzch=="")
				jdzch = obj.JDZCH;
        })
		if(delfalse=="1")
			return;
        if (!confirm("确定要辞退所选的人员吗？")) {
            return;
        }
        $.ajax({
            type: "POST",
            url: "/qy/SetWgryLeave",//?recids=" + encodeURIComponent(recids)+"&jdzchs="+encodeURIComponent(jdzchs),
			data:{
				recids:recids,
				jdzch:jdzch,
				sfzhms:sfzhms
			},
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0") {
                    alert("辞退成功！");
                    searchRecord();
                } else {
                    if (data.msg == "")
                        data.msg = "辞退失败";
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
function edit() {
    try {
        var selecteds = pubselects();
        if (selecteds == undefined) {
            return;
        }
        var selected = selecteds[0];

        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_WGRY"); 			// 表名
        var tablerecid = encodeURIComponent("Recid"); 	// 表主键
        var jydbh = encodeURIComponent(selected.RECID)   // 键值
        var title = encodeURIComponent("人员信息"); 	// 标题
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮

        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&jydbh=" + jydbh +
            "&rownum=2" +
			"&LX=GC" +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '修改人员信息',
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

