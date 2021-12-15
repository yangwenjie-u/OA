function add(){
    try {
        // 获取父目录ID
        var pid = getPID();
        var id = "";
        var name = "";

        if (pid != "") {

            parent.layer.open({
                type: 2,
                title: '新增目录',
                shadeClose: false,
                shade: 0.8,
                area: ['50%', '50%'],
                content: "/jdbg/SmsTXLAdd?pid=" + encodeURIComponent(pid) + "&id=" + encodeURIComponent(id) + "&name=" + encodeURIComponent(name),
                end: function () {
                    parent.loadTree(pid);
                    //searchRecord();
                }
            });
        }
        else {
            layer.alert('父目录不能为空！', {
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

        // 获取父目录ID
        var pid = getPID();
        var id = selected.ID;
        var name = selected.NAME;

        if (pid != "") {

            parent.layer.open({
                type: 2,
                title: '修改目录',
                shadeClose: false,
                shade: 0.8,
                area: ['50%', '50%'],
                content: "/jdbg/SmsTXLAdd?pid=" + encodeURIComponent(pid) + "&id=" + encodeURIComponent(id) + "&name=" + encodeURIComponent(name),
                end: function () {
                    searchRecord();
                }
            });

        }
        else {
            layer.alert('父目录不能为空！', {
                icon: 0,
                skin: 'layer-ext-moon'
            });
        }
    } catch (e) {
        alert(e);
    }
}


function deleteML(){
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;
        var pid = getPID();
        var id = selected.ID;
        layer.open({
            content: '删除目录时，会同时删除该目录下的子目录与成员， 确定要删除吗？',
            btn: ['确定', '取消'],
            yes: function (index, layero) {
                layer.close(index);
                $.ajax({
                    type: "POST",
                    url: "/jdbg/deleteSmsML?id=" + encodeURIComponent(id),
                    dataType: "json",
                    async: false,
                    success: function (data) {
                        if (data.code == "0") {
                            parent.layer.alert('删除成功！', {
                                icon: 0,
                                skin: 'layer-ext-moon'
                            });
                            parent.loadTree(pid);
                            //searchRecord();
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
            },
            btn2: function (index, layero) {

            }
        });
        
       
    } catch (e) {
        alert(e);
    }
}


function getQueryString(name) { 
	var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i"); 
	var r = window.location.search.substr(1).match(reg); 
	if (r != null) 
		return unescape(r[2]); 
	return null; 
}

function getPID(){
	var pid = "";
	var param = getQueryString("FormParam");
	if(param != null && param.length > 0){
	    pid = param.replace(/PARAM--/,"");
	}
	return pid;
} 