
function upload() {
    var selected = pubselect();
    if (selected == undefined)
        return;
    var rdm = Math.random();
    var gcbh = selected.GCBH;
    var gcmc = selected.GCMC;
    var gclxbh = selected.GCLXBH;
    var isedit = 1;
    var zllx = 'zj';
    var params = {
        gcbh: gcbh,
        gcmc: gcmc,
        gclxbh: gclxbh,
        isedit: isedit,
        zllx: zllx
    };

    var rdm = Math.random();
    var url = "/jdbg/uploadzl?gcbh=" + gcbh +
        "&gcmc=" + gcmc +
        "&gclxbh=" + gclxbh +
        "&isedit=" + isedit +
        "&zllx=" + zllx +
        "&_=" + rdm;

    parent.layer.open({
        type: 2,
        title: '上传质监资料',
        shadeClose: false,
        shade: 0.8,
        area: ['80%', '80%'],
        content: url,
        end: function () {
            searchRecord();
        }
    });

}

function getdata() {
    var ret = {};
    var bhlist = [];
    var filelist = [];
    $('td[id^=lxbh_]').each(function (index, obj) {
        var td = $(this);
        var id = td.attr('id').replace('lxbh_', '');
        bhlist.push(id);
        var input = td1.next().find("input[type=hidden][id=gczl" + id + "]");
        if (input.length) {
            $.each(input, function (index, ele) {
                filelist.push($(this).val());
                return true;
            });
        }
        else {
            filelist.push('');
        }
    });

    ret = { bhlist: bhlist.join(','), filelist: filelist.join(',') };
    return ret;

    
}



function view() {
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
        var buttons = ""; // 按钮;//encodeURIComponent("临时保存|ZC| | ||完成报监|TJ| | "); // 按钮

        var s_tablename = encodeURIComponent("I_S_GC_JSDW|I_S_GC_KCDW|I_S_GC_SJDW|I_S_GC_JLDW|I_S_GC_SGDW|I_S_GC_TSDW|I_S_GC_FGC");
        //  都是从表中的字段：  主表对应字段,自己主键|……
        var s_pri = encodeURIComponent("GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,GCQYBH|GCBH,RECID");
        var s_title = encodeURIComponent("建设单位|勘察单位|设计单位|监理单位|施工单位|图审单位|单位工程");

        var ss_tablename = encodeURIComponent("I_S_GC_JSDW|I_S_GC_JSRY||I_S_GC_KCDW|I_S_GC_KCRY||I_S_GC_SJDW|I_S_GC_SJRY||I_S_GC_JLDW|I_S_GC_JLRY||I_S_GC_SGDW|I_S_GC_SGRY||I_S_GC_TSDW|I_S_GC_TSRY");
        //都是明细表中的字段：跟主表对应字段,跟从表对应字段,自己主键
        var ss_pri = encodeURIComponent("GCBH,QYBH,RECID|GCBH,QYBH,RECID|GCBH,QYBH,RECID|GCBH,QYBH,RECID|GCBH,QYBH,RECID|GCBH,QYBH,RECID");
        var ss_title = encodeURIComponent("建设人员|勘察人员|设计人员|监理人员|施工人员|图审人员");

        var rdm = Math.random();

        var js = "";//encodeURIComponent("jdbgService.js");
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
            "&t2_tablename=" + s_tablename +
            "&t2_pri=" + s_pri +
            "&t2_title=" + s_title +
            "&t3_tablename=" + ss_tablename +
            "&t3_pri=" + ss_pri +
            "&t3_title=" + ss_title +
            "&view=true" +
            "&jydbh=" + jydbh +
            "&LX=N" +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: selected.GCMC,
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

function FormatZt(value, row, index) {
    var imgurl = "";
    try {

        if (value == "YT") {
            if (row.SPTG == "False")
                imgurl += "<center><img src='/skins/default/images/list/set_gray.png' title='未提交'/></center>";
            else
                imgurl += "<center><img src='/skins/default/images/list/set_red.png' title='未通过'/></center>";
        } else if (row.ZT == "LR") {
            imgurl += "<center><img src='/skins/default/images/list/set_blue.png' title='审批中'/></center>";
        } else {
            imgurl += "<center><img src='/skins/default/images/list/set_green.png' title='已通过'/></center>";
        }
    } catch (e) {
        alert(e);
    }
    imgurl += "";
    return imgurl;
}

