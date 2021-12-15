
var jdzch="";
$(function()
{
   jdzch = decodeURIComponent(getQueryStr('jdzch'));
});

function getQueryStr(str){
    var rs = new RegExp("(^|)"+str+"=([^\&]*)(\&|$)","gi").exec(String(window.document.location.href)), tmp;
    if(tmp=rs){
        return tmp[2];
    }
    return "";
}

function add() {
     try {
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("H_RYGZ"); 			// 表名
        var tablerecid = encodeURIComponent("Recid"); 	// 表主键
        var title = encodeURIComponent("工种信息"); 	// 标题
        var buttons = encodeURIComponent("保存|TJ| "); // 按钮
		var fieldparam=encodeURIComponent("H_RYGZ,JDZCH,"+jdzch);
		var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
			"&fieldparam=" + fieldparam +
            "&button=" + buttons +
            "&rownum=2" +
			"&LX=N" +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '录入工种信息',
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
function edit() {
     try {
		var selected = pubselect();
        if (selected == undefined)
            return;
		if (!confirm("如果该工种已被使用，请勿修改！确定要修改所选的记录吗？")) {
            return;
        }
		var jdzch=selected.JDZCH;
		var gzname=selected.GzName;
		$.ajax({
            type: "POST",
            url: "/ZJ_Info/getGcGzUse?jdzch="+encodeURIComponent(jdzch)+"&gzname="+encodeURIComponent(gzname),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0") {
					var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
					var tablename = encodeURIComponent("H_RYGZ"); 			// 表名
					var tablerecid = encodeURIComponent("Recid"); 	// 表主键
					var jydbh = encodeURIComponent(selected.RECID)   // 键值
					var title = encodeURIComponent("工种信息"); 	// 标题
					var buttons = encodeURIComponent("保存|TJ| "); // 按钮		
					var rdm = Math.random();
					var url = "/datainput/Index?zdzdtable=" + zdzdtable +
						"&t1_tablename=" + tablename +
						"&t1_pri=" + tablerecid +
						"&t1_title=" + title +
						"&button=" + buttons +
						"&jydbh=" + jydbh +
						"&rownum=2" +
						"&LX=N" +
						"&_=" + rdm;

					parent.layer.open({
						type: 2,
						title: '修改工种信息',
						shadeClose: false,
						shade: 0.8,
						area: ['95%', '95%'],
						content: url,
						end: function () {
							searchRecord();
						}
					});		   
                } else {
                    if (data.msg == "")
                        data.msg = "无法修改";
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



function add2() {
     try {
		$.ajax({
            type: "POST",
            url: "/ZJ_Info/getgcbh_jdzch",
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0") {
					var gcjdzch=data.jdzch;
					var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
					var tablename = encodeURIComponent("H_RYGZ"); 			// 表名
					var tablerecid = encodeURIComponent("Recid"); 	// 表主键
					var title = encodeURIComponent("工种信息"); 	// 标题
					var buttons = encodeURIComponent("保存|TJ| "); // 按钮
					var fieldparam=encodeURIComponent("H_RYGZ,JDZCH,"+gcjdzch);
					var rdm = Math.random();
					var url = "/datainput/Index?zdzdtable=" + zdzdtable +
						"&t1_tablename=" + tablename +
						"&t1_pri=" + tablerecid +
						"&t1_title=" + title +	
						"&fieldparam=" + fieldparam +						
						"&button=" + buttons +
						"&rownum=2" +
						"&LX=GC" +
						"&_=" + rdm;

					parent.layer.open({
						type: 2,
						title: '录入工种信息',
						shadeClose: false,
						shade: 0.8,
						area: ['95%', '95%'],
						content: url,
						end: function () {
							searchRecord();
						}
					});			   
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

