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
<<<<<<< .mine
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
||||||| .r98
}

function viewjc() {
    var selected = pubselect();
    if (selected == undefined)
        return;
    var gcbh = selected.GCBH;
    var gcmc = selected.GCMC;
    var gclxbh = selected.GCLXBH;
    var isedit = 0;
    var zllx = 'jcys';
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
        title: '查看基础资料',
        shadeClose: true,
        shade: 0.8,
        area: ['80%', '80%'],
        content: url,
        end: function () {
            searchRecord();
        }
    });
}

function viewzt() {
    var selected = pubselect();
    if (selected == undefined)
        return;
    var gcbh = selected.GCBH;
    var gcmc = selected.GCMC;
    var gclxbh = selected.GCLXBH;
    var isedit = 0;
    var zllx = 'ztys';
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
        title: '查看主体验收资料',
        shadeClose: true,
        shade: 0.8,
        area: ['80%', '80%'],
        content: url,
        end: function () {
            searchRecord();
        }
    });
}

function viewzj() {
    var selected = pubselect();
    if (selected == undefined)
        return;
    var gcbh = selected.GCBH;
    var gcmc = selected.GCMC;
    var gclxbh = selected.GCLXBH;
    var isedit = 0;
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
        title: '查看质监资料',
        shadeClose: true,
        shade: 0.8,
        area: ['80%', '80%'],
        content: url,
        end: function () {
            searchRecord();
        }
    });
}


function setPos() {
    try{
        var selected = pubselect();
        if (selected == undefined)
            return;

        var gcbh = encodeURIComponent(selected.GCBH)   // 键值
        var gcmc = selected.GCMC   // 键值
        var pos = encodeURIComponent(selected.GCZB);
        var layerObj = undefined;
        parent.layer.open({
            type: 2,
            title: '工程标注-' + gcmc,
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: "/jc/map?title=" + encodeURIComponent(gcmc)+"&pos="+pos,
            btn: ["保存", "关闭"],
            yes: function (index) {
                var pos = window.parent[layerObj.find('iframe')[0]['name']].getPos();
                if (pos == "") {
                    return;
                }
                $.ajax({
                    type: "POST",
                    url: "/jc/setgcbz?gcbh=" + gcbh + "&pos=" + encodeURIComponent(pos),
                    dataType: "json",
                    async: false,
                    success: function (data) {
                        if (data.code != 0)
                            alert("标注失败，详细信息：" + data.msg);
                        else {
                            alert("标注成功");
                            parent.layer.closeAll();
                        }

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
            end:function(){
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}
=======
}>>>>>>> .r220
