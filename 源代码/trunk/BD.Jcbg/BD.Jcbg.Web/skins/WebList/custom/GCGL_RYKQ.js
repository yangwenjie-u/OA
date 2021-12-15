function view()
{
	 try {
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_RY"); 			// 表名
        var tablerecid = encodeURIComponent("SFZHM"); 	// 表主键
        var title = encodeURIComponent("人员信息"); 	// 标题
       // var buttons = encodeURIComponent("提交|TJ| | |修改成功"); // 按钮
		var custombutton=encodeURIComponent("提交|TJSJ|SearchGCRYKQ('')");
		var js=encodeURIComponent("SearchRYKQ.js,idcardService.js");
		//var fieldparam = encodeURIComponent("I_M_RY,QYBH," + selected.QYBH+ "|I_M_QY,RYLX,"+selected.RYLX);
		
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
			"&custombutton=" + custombutton +
            "&rownum=2" +	
			"&js="+js+
			"&LX=CX"+
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '人员查询',
            shadeClose: true,
            shade: 0.8,
            area: ['85%', '85%'],
            content: url,
            end: function () {
				parent.layer.closeAll();
               // searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}


function setkq()
{
	 try {
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("KqjUserLog"); 			// 表名
        var tablerecid = encodeURIComponent("RECID"); 	// 表主键
        var title = encodeURIComponent("人员信息"); 	// 标题
        var buttons = encodeURIComponent("提交|TJ| | |修改成功"); // 按钮
		//var custombutton=encodeURIComponent("提交|TJSJ|SearchGCRYKQ('')");
		var js=encodeURIComponent("idcardService.js");
		//var fieldparam = encodeURIComponent("KqjUserLog,HASDEAL,0");
		
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
			"&button=" + buttons +
            "&rownum=2" +	
			"&js="+js+
			"&LX=BL"+
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '人员考勤补录',
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

function viewDateKq()
{
	try{
		var url="/JX_Info/SetKqViewDate";
		parent.layer.open({
			type: 2,
			title: '工种考勤查询',
			shadeClose: false,
			shade: 0.8,
			area: ['30%', '40%'],
			content: url,
			end: function () {
				parent.layer.closeAll();
			}
		});
	}
	catch (e) {
        alert(e);
    }  
}
