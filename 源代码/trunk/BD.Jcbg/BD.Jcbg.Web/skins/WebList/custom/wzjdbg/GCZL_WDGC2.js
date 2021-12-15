function view() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;

        parent.layer.open({
            type: 2,
            title: "工程详情",
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: "/jdbg/gccknb?gcbh=" + encodeURIComponent(selected.GCBH),
            end: function () {
            }
        });
    } catch (ex) {
        alert(ex);
    }
}

function viewSxt() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;

        var url = "/hkws/play?url=" + encodeURIComponent(selected.SXT);
        parent.layer.open({
            type: 2,
            title: "查看摄像头",
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: url,
            end: function () {
            }
        });
    } catch (ex) {
        alert(ex);
    }
}

function FormatSxt(value, row, index) {
    var imgurl = "";
    try {
        if (value !=undefined){
            if (value != "0")
                imgurl += "<center><img src='/skins/default/images/list/set_green.png' title='有'/></center>";
            else
                imgurl += "<center><img src='/skins/default/images/list/set_gray.png' title='无'/></center>";
        }
    } catch (e) {
        alert(e);
    }
    imgurl += "";
    return imgurl;
}

function switchRecord(obj) {
    // 所有
    if (obj.checked)
        window.location = "/WebList/EasyUiIndex?FormDm=GCZL_WDGC&FormStatus=1&FormParam=PARAM--ALL||CHECKBOX--我的,所有|所有";
    else
        window.location = "/WebList/EasyUiIndex?FormDm=GCZL_WDGC&FormStatus=1&FormParam=PARAM--NOT||CHECKBOX--我的,所有|我的";
}

function edit() {
    try {
        var tabledesc = "工程信息";

        var selected = pubselect();
        if (selected == undefined)
            return;

        var jydbh = encodeURIComponent(selected.GCBH);
        var tabledesc = "工程信息";                // 表格描述
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_GC"); 			// 表名
        var tablerecid = encodeURIComponent("GCBH"); 	// 表主键
        var title = encodeURIComponent(tabledesc); 	// 标题
        var formdm = tablename;                             // 列表key名称
        var buttons = encodeURIComponent("保存|TJ| | "); // 按钮;//encodeURIComponent("临时保存|ZC| | ||完成报监|TJ| | "); // 按钮



        var rdm = Math.random();

        var js = "";//encodeURIComponent("i_m_gc.js");
        var callback = "";//encodeURIComponent("jdbgService.jdzcsb('$$GCBH$$')");

        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&js=" + js +
            "&callback=" + callback +
            "&rownum=2" +
            
            "&jydbh=" + jydbh +
            "&LX=N" +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '工程修改',
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
