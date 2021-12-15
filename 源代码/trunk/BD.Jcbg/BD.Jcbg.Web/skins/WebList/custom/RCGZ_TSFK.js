
function view()
{
   try {
	        var selected = pubselect();
			if (selected == undefined)
				return;
			var recid = selected.RECID;
			
            var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
            var tablename = encodeURIComponent("I_M_RY_TS"); 			// 表名
            var tablerecid = encodeURIComponent("Recid"); 	// 表主键
            var title = encodeURIComponent("投诉内容"); 	// 标题
            var jydbh = encodeURIComponent(recid)   // 键值         
           // var buttons = encodeURIComponent("提交|TJ| | |添加成功"); // 按钮 
            var js = encodeURIComponent("ryts.js");
            var callback = encodeURIComponent("ryts('')");
			
			
            var rdm = Math.random();
            var url = "/datainput/Index?zdzdtable=" + zdzdtable +
                "&t1_tablename=" + tablename +
                "&t1_pri=" + tablerecid +
                "&t1_title=" + title +   
				"&jydbh=" + jydbh +					
                "&rownum=2" +
				"&LX=A" +
				"&view=true" +
                "&_=" + rdm;

            parent.layer.open({
                type: 2,
                title: '投诉反馈查看',
                shadeClose: true,
                shade: 0.8,
                area: ['95%', '95%'],
                content: url,
                end: function () {
                    parent.layer.closeAll();
                    searchRecord();               
                }
            });
        } catch (e) {
            alert(e);
        }
}
function check() {
	
    var selected = pubselect();
    if (selected == undefined)
        return;
    var layerObj = undefined;
	var jdzch=selected.JDZCH;
    parent.layer.open({
        type: 2,
        title: '人员投诉反馈审批',
        shadeClose: true,
        shade: 0.8,
        area: ['380px', '230px'],
        content: "/jdbg/rytsfksp",
        btn: ["保存", "关闭"],
        yes: function (index) {
            var checkvalue = window.parent[layerObj.find('iframe')[0]['name']].getValue();
            var reason=window.parent[layerObj.find('iframe')[0]['name']].getReason();
            $.ajax({
                type: "POST",
                url: "/jdbg/checkrytsfk",
				data:{
					recid:selected.RECID ,
					checkoption:checkvalue,
					reason:reason,
					jdzch:jdzch
				},
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