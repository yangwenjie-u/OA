function add(){
    try {
        // 获取目录ID
        var mlid = getMLID();
        var id = "";
        var name = "";
        var zw = "";
        var sjhm = "";

        if (mlid != "") {

            parent.layer.open({
                type: 2,
                title: '新增成员',
                shadeClose: false,
                shade: 0.8,
                area: ['50%', '50%'],
                content: "/jdbg/SmsTXLCYAdd?mlid=" + encodeURIComponent(mlid) + "&id=" + encodeURIComponent(id) + "&name=" + encodeURIComponent(name) + "&zw=" + encodeURIComponent(zw) + "&sjhm=" + encodeURIComponent(sjhm),
                end: function () {
                    searchRecord();
                }
            });
        }
        else {
            layer.alert('目录不能为空！', {
                icon: 0,
                skin: 'layer-ext-moon'
            });
        }
    } catch (e) {
        alert(e);
    }
}

function edit(){
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;

        // 获取目录ID
        var mlid = getMLID();
        var id = selected.ID;
        var name = selected.NAME;
        var zw = selected.ZW;
        var sjhm = selected.SJHM;

        if (mlid != "") {

            parent.layer.open({
                type: 2,
                title: '修改成员',
                shadeClose: false,
                shade: 0.8,
                area: ['50%', '50%'],
                content: "/jdbg/SmsTXLCYAdd?mlid=" + encodeURIComponent(mlid) + "&id=" + encodeURIComponent(id) + "&name=" + encodeURIComponent(name) + "&zw=" + encodeURIComponent(zw) + "&sjhm=" + encodeURIComponent(sjhm),
                end: function () {
                    searchRecord();
                }
            });

        }
        else {
            layer.alert('目录不能为空！', {
                icon: 0,
                skin: 'layer-ext-moon'
            });
        }
    } catch (e) {
        alert(e);
    }
}


function deleteCY(){
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;
        var mlid = getMLID();
        var id = selected.ID;
        $.ajax({
            type: "POST",
            url: "/delete/deletetable?ID=" + encodeURIComponent(id) + "&name=ID&table=OA_SMS_TXLCY",
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0") {
                    parent.layer.alert('删除成功！', {
                        icon: 0,
                        skin: 'layer-ext-moon'
                    });
                    searchRecord();
                } else {
                    if (data.msg == "")
                        data.msg = "删除失败";
                    layer.alert(data.msg, {
                        icon: 0,
                        skin: 'layer-ext-moon'
                    });
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


function exportAll() {
    var frm = $("<form id='exportcyform'></form>");
    $("body").append(frm);
    frm.attr("action", "/jdbg/ExportAllCY");
    frm.submit();
}

function getQueryString(name) { 
	var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i"); 
	var r = window.location.search.substr(1).match(reg); 
	if (r != null) 
		return unescape(r[2]); 
	return null; 
}

function getMLID(){
	var pid = "";
	var param = getQueryString("FormParam");
	if(param != null && param.length > 0){
	    pid = param.replace(/PARAM--/,"");
	}
	return pid;
}

function importCY(iscurrent) {
    var mlid = null;
    if (iscurrent == 0) {
        mlid = getMLID();
    }
    else if (iscurrent == 1) {
        mlid = "";
    }
    if (mlid != null) {
        parent.layer.open({
            type: 2,
            title: '通讯录导入（文件格式：Excel/txt/csv；每行内容为：姓名，职位，号码）',
            shadeClose: false,
            shade: 0.8,
            area: ['50%', '50%'],
            content: "/jdbg/ImportCY?mlid=" + encodeURIComponent(mlid),
            end: function () {
                searchRecord();
            }
        });

    }

}