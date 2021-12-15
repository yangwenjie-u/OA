
function edit()
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
            //var custombutton = encodeURIComponent("提交|TJSJ|ryts('')");
            var buttons = encodeURIComponent("提交|TJ| | |添加成功"); // 按钮 
            var js = encodeURIComponent("ryts.js");
            var callback = encodeURIComponent("ryts('')");
			var param="I_M_RY_TS,TJSP,1|I_M_RY_TS,SFSP,0|I_M_RY_TS,TSZT,已反馈|I_M_RY_TS,REMARK,"; 
			var fieldparam=encodeURIComponent(param);
			
            var rdm = Math.random();
            var url = "/datainput/Index?zdzdtable=" + zdzdtable +
                "&t1_tablename=" + tablename +
                "&t1_pri=" + tablerecid +
                "&t1_title=" + title +   
				"&jydbh=" + jydbh +
				"&button=" + buttons +
				"&fieldparam=" + fieldparam +
                "&rownum=2" +
				"&LX=A" +
                "&_=" + rdm;

            parent.layer.open({
                type: 2,
                title: '投诉反馈',
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