/*检测机构收样人员管理*/
function add() {
    try {

        parent.layer.open({
            type: 2,
            title: '添加收样人员',
            shadeClose: true,
            shade: 0.8,
            area: ['800px', '95%'],
            content: "/WebList/EasyUiIndex?FormDm=JCJG_SYRYLY&FormStatus=0",
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}
function del() {
    try {
        var selecteds = pubselects();
        if (selecteds.length == 0) {
            alert("请选择要操作的记录");
            return;
        }
        var rybhs = "";
        $.each(selecteds, function (index, obj) {
            rybhs += obj.RYBH + ",";
        })
        if (!confirm("确定要解除人员的收样人员职务吗？")) {
            return;
        }
        $.ajax({
            type: "POST",
            url: "/qy/removesyry?rybh=" + encodeURIComponent(rybhs),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0") {
                    alert("解除成功！");
                    searchRecord();
                } else {
                    if (data.msg == "")
                        data.msg = "解除失败";
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
function set() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_RY"); 			// 表名
        var tablerecid = encodeURIComponent("RYBH"); 	// 表主键
        var title = encodeURIComponent("人员职务"); 	// 标题
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var jydbh = encodeURIComponent(selected.RYBH)   // 键值
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&jydbh=" + jydbh +
            "&LX=S" +
            "&rownum=1" +
            //"&dllname=" + dllname +
            //"&dllclass=" + dllclass +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '人员职务设置',
            shadeClose: true,
            shade: 0.8,
            area: ['800px', '400px'],
            content: url,
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}