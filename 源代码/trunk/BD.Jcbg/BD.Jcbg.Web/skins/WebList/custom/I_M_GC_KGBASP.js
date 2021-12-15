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
	var custombutton=encodeURIComponent("审批|SP|check()")
	var js=encodeURIComponent("I_M_GC_KGBASP.js");
    var rdm = Math.random();
    var url = "/datainput/Index?zdzdtable=" + zdzdtable +
        "&t1_tablename=" + tablename +
        "&t1_pri=" + tablerecid +
        "&t1_title=" + title +
        "&rownum=2" +
        "&view=true" +
        "&jydbh=" + jydbh +
		"&custombutton=" + custombutton +
	    "&js=" + js +
        "&_=" + rdm;



    parent.layer.open({
        type: 2,
        title: "企业详情",
        shadeClose: true,
        shade: 0.8,
        area: ['95%', '95%'],
        content: url,
        end: function () {
			searchRecord();
        }
    });
}

function check() {
    var selected = pubselect();
    if (selected == undefined)
        return;

    var layerObj = undefined;

    parent.layer.open({
        type: 2,
        title: '开工备案申请审批',
        shadeClose: true,
        shade: 0.8,
        area: ['350px', '230px'],
        content: "/qy/kgbasp",
        btn: ["保存", "关闭"],
        yes: function (index) {
            var checkvalue = window.parent[layerObj.find('iframe')[0]['name']].getValue();
            var reason = window.parent[layerObj.find('iframe')[0]['name']].getReason();
            $.ajax({
                type: "POST",
                url: "/qy/setkgbasp?jdzch=" +encodeURIComponent(selected.JDZCH) + "&checkoption=" + checkvalue + "&reason=" + encodeURIComponent(reason),
                dataType: "json",
                async: false,
                success: function (data) {
                    if (data.msg != "")
                        alert(data.msg);
                    parent.layer.closeAll();
                },
                complete: function (XMLHttpRequest, textStatus) {
                },
                beforeSend: function (XMLHttpRequest) {
                }
            });
        },
        success: function (layero, index) {
            layerObj = layero;
        },
        btn2: function (index) {
            parent.layer.closeAll();
        },
        end: function () {
            searchRecord();
        }
    });
}