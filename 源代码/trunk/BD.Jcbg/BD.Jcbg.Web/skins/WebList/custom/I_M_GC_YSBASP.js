function check() {
    var selected = pubselect();
    if (selected == undefined)
        return;

    var layerObj = undefined;

    parent.layer.open({
        type: 2,
        title: '验收备案申请审批',
        shadeClose: true,
        shade: 0.8,
        area: ['350px', '230px'],
        content: "/qy/ysbasp",
        btn: ["保存", "关闭"],
        yes: function (index) {
            var checkvalue = window.parent[layerObj.find('iframe')[0]['name']].getValue();
            var reason = window.parent[layerObj.find('iframe')[0]['name']].getReason();
            $.ajax({
                type: "POST",
                url: "/qy/setysbasp?jdzch=" +encodeURIComponent(selected.JDZCH) +"&recid=" +encodeURIComponent(selected.RECID) + "&checkoption=" + checkvalue + "&reason=" + encodeURIComponent(reason)+"&ysbalx=" + encodeURIComponent(selected.YSBALX),
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

function view() {
    var selected = pubselect();
    if (selected == undefined)
        return;
    var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
    var tablename = encodeURIComponent("I_M_GC_YSBA"); 			// 表名
    var tablerecid = encodeURIComponent("RECID"); 	// 表主键
    var title = encodeURIComponent("信息查看"); 	// 标题
    var jydbh = encodeURIComponent(selected.RECID);
    var formdm = tablename;                             // 列表key名称
	var js=encodeURIComponent("I_M_GC_YSBASP.js");
    var buttons = "";//encodeURIComponent("提交|TJ| "); // 按钮
	var custombutton = "查看工资详情|ZX1|rygzxx('')";
    var rdm = Math.random();
    var url = "/datainput/Index?zdzdtable=" + zdzdtable +
        "&t1_tablename=" + tablename +
        "&t1_pri=" + tablerecid +
        "&t1_title=" + title +
        //"&button=" + buttons +
		"&custombutton="+custombutton+
		"&js=" + js +
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

function info()
{
	var selected = pubselect();
    if (selected == undefined)
        return;
	var jdzch=selected.JDZCH;
	var url="/WebList/EasyUiIndex?FormDm=GCGL_GCINFO&FormStatus=0&FormParam=PARAM--"+encodeURIComponent(jdzch);
	
	gotoweblist(url,"工程详细信息");
}
function gotoweblist(workurl, title) {
    try {
        var rdm = Math.random();
        var url = workurl;

        parent.layer.open({
            type: 2,
            title: title,
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: url,
            end: function () {
            }
        });
    }
    catch (ex) {
        alert(ex);
    }
}