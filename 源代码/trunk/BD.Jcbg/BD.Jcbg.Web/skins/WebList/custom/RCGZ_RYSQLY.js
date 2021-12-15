function view() {
    try {
		var selecteds = pubselects();
        if (selecteds==undefined) {          
            return;
        }
        var selected = selecteds[0];

        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_RY"); 			// 表名
        var tablerecid = encodeURIComponent("RYBH"); 	// 表主键
        var jydbh = encodeURIComponent(selected.RYBH)   // 键值
        var title = encodeURIComponent("人员信息"); 	// 标题
        var buttons = ""; // 按钮

        var s_tablename = encodeURIComponent("I_S_RY_RYZZ");
        var s_pri = encodeURIComponent("RYBH,RYBH");
        var s_title = encodeURIComponent("人员证书");

        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&jydbh=" + jydbh +
            "&rownum=2" +
            "&t2_tablename=" + s_tablename +
            "&t2_pri=" + s_pri +
            "&t2_title=" + s_title +
            "&LX=SQ" +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '查看人员信息',
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: url,
            end: function () {
               // searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}

function add() {
    try {
        var selectedRows = pubselects();
        if (selectedRows == undefined || selectedRows.length== 0) {
            return;
        }

        var rybhs = "";
        $.each(selectedRows, function (index, selected) {
            rybhs += selected.RYBH + ",";
        })
        $.ajax({
            type: "POST",
            url: "/qy/setrydw?rybh=" + encodeURIComponent(rybhs),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0") {
					 $.ajax({
						type: "POST",
						url: "/dwgxwzlc/setrysqzt?rybh=" + encodeURIComponent(rybhs),
						dataType: "json",
						async: false,
						success: function (data) {
							if (data.code == "0") {							
								alert("录用成功！");								
							} else {
								if (data.msg == "")
									data.msg = "录用失败";
								alert(data.msg);
							}
						},
						complete: function (XMLHttpRequest, textStatus) {
						},
						beforeSend: function (XMLHttpRequest) {
						}
					});
                } else {
                    if (data.msg == "")
                        data.msg = "录用失败";
                    alert(data.msg);
                }
            },
            complete: function (XMLHttpRequest, textStatus) {
				searchRecord();
            },
            beforeSend: function (XMLHttpRequest) {
            }
        });
        
    } catch (e) {
        alert(e);
    }
}