
function edit() {
    try {
        var tabledesc = "人员账号创建设置";        
        var selected = pubselect();
        if (selected == undefined)
            return;

        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("H_RYLX"); 			// 表名
        var tablerecid = encodeURIComponent("LXBH"); 	// 表主键
        var jydbh = encodeURIComponent(selected.LXBH)   // 键值
        var title = encodeURIComponent(tabledesc); 	// 标题
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&jydbh=" + jydbh +
            "&rownum=2" +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '修改' + tabledesc,
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

function FormatYesno(value, row, index) {
    var imgurl = "";
    try {

        if (value == "是")
            imgurl += "<center><img src='/skins/default/images/list/set_green.png' title='" + value + "'/></center>";
        else
            imgurl += "<center><img src='/skins/default/images/list/set_gray.png' title='" + value + "'/></center>";
    } catch (e) {
        alert(e);
    }
    imgurl += "";
    return imgurl;
}
