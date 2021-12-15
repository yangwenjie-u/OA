function read()
{
	//cardIdr.setCardInfo;
}

function view()
{
	 try {
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_WGRY"); 			// 表名
        var tablerecid = encodeURIComponent("SFZHM"); 	// 表主键
        var title = encodeURIComponent("人员信息"); 	// 标题
       // var buttons = encodeURIComponent("提交|TJ| | |修改成功"); // 按钮
		var custombutton=encodeURIComponent("提交|TJSJ|SearchRYKQ('')");
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
            area: ['95%', '95%'],
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



