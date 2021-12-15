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
        window.location = "/WebList/EasyUiIndex?FormDm=GCZL_WDGC&FormStatus=0&FormParam=PARAM--ALL||CHECKBOX--我的,所有|所有";
    else
        window.location = "/WebList/EasyUiIndex?FormDm=GCZL_WDGC&FormStatus=0&FormParam=PARAM--NOT||CHECKBOX--我的,所有|我的";
}

function addBZ() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_S_GC_BZ"); 			// 表名
        var tablerecid = encodeURIComponent("RECID"); 	// 表主键
        var title = encodeURIComponent("工程备注"); 	// 标题
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var fieldparam = encodeURIComponent("I_S_GC_BZ,GCBH," + selected.GCBH);
        var sufproc = "";
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&fieldparam=" + fieldparam +
            "&sufproc=" + sufproc +
            "&rownum=2" +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '添加工程备注',
            shadeClose: false,
            shade: 0.8,
            area: ['70%', '70%'],
            content: url,
            end: function () {
                
            }
        });
    } catch (ex) {
        alert(ex);
    }
}

function viewBZ()
{
    try {
        var tabledesc = "个人工程备注";
        var selected = pubselect();
        if (selected == undefined)
            return;


        var url = "/WebList/EasyUiIndex?FormDm=I_S_GC_BZ&FormStatus=0&FormParam=PARAM--" + encodeURIComponent(selected.GCBH)
        url += "|" + encodeURIComponent(selected.GCBH);

        parent.layer.open({
            type: 2,
            title: selected.GCMC,
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: url,
            end: function () {
               
            }
        });
    } catch (e) {
        alert(e);
    }
}
