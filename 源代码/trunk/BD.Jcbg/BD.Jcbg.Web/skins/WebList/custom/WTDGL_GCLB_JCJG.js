
function addWtdJcjg() {
    try {
        var tabledesc = "工程";
        var selected = pubselect();
        if (selected == undefined)
            return;
        var title = "试验项目选择—" + "[" + selected.GCBH + "]" + selected.GCMC;
        parent.layer.open({
            type: 2,
            title: title,
            shadeClose: true,
            shade: 0.5,
            area: ['98%', '98%'],
            content: "/jc/syxmxz?gcbh=" + encodeURIComponent(selected.GCBH) + "&gcmc=" + encodeURIComponent(selected.GCMC) + "&dwbh=" + encodeURIComponent(selected.JCJGBH)+"&jchtrecid="+selected.RECID,
        });
        
    } catch (e) {
        alert(e);
    }
}
function viewWtd() {
    try {
        var arrselected =  pubselects2();
        var selected = undefined;
        if (arrselected != null && arrselected.length > 0)
            selected = arrselected[0];
        var title = "";
        var gcbh = "";
        if (selected != undefined) {
            gcbh = selected.GCBH;
            title = "[" + selected.GCBH + "]["+selected.GCMC+"]" ;
        }
        title += "委托清单";
        
        var url = "/WebList/EasyUiIndex?FormDm=WTDGL&FormStatus=0&FormParam=PARAM--" + gcbh + "|ALL|";

        parent.layer.open({
            type: 2,
            title: title,
            shadeClose: true,
            shade: 0.8,
            area: ['100%', '100%'],
            content: url,
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}