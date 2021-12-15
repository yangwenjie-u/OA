function check() {
    var selected = pubselect();
    if (selected == undefined)
        return;

    var layerObj = undefined;

    parent.layer.open({
        type: 2,
        title: '企业资质申请审批',
        shadeClose: false,
        shade: 0.8,
        area: ['250px', '180px'],
        content: "/qy/qyzzsp",
        btn: ["保存", "关闭"],
        yes: function (index) {
            var checkvalue = window.parent[layerObj.find('iframe')[0]['name']].getValue();
            
            $.ajax({
                type: "POST",
                url: "/qy/checkqyzz?zzbh=" + selected.ZZBH + "&checkoption=" + checkvalue,
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
        }
        ,
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
    var url = "/datainput/Index?zdzdtable=ZDZD_JC" +
        "&t1_tablename=I_M_QY" +
        "&t1_pri=QYBH" +
        "&t1_title=" + encodeURIComponent("企业信息") +
        "&t2_tablename=I_S_QY_QYZZ" +
        "&t2_pri=QYBH,QYBH" +
        "&t2_title=" + encodeURIComponent("资质信息") +
        "&button=" +
        "&rownum=2" +
        "&view=true" +
        "&jydbh=" + selected.QYBH +
        "&LX=N";
    parent.layer.open({
        type: 2,
        title: "企业详情",
        shadeClose: false,
        shade: 0.8,
        area: ['95%', '95%'],
        content: url,
        end: function () {
        }
    });
}