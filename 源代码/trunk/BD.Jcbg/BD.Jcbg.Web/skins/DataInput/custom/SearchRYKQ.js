function SearchRYKQ(a) { //管理
	if (!IsSubmitFormValid())
		return;
	var ret = false;
    var sfzhm=GetCtrlValue("I_M_RY.SFZHM");
	var url="/WebList/EasyUiIndex?FormDm=QYRY_GCRYKQ&FormStatus=0&FormParam=PARAM--"+sfzhm+"&sfzhm="+sfzhm;
	//var url="/WebList/EasyUiIndex?FormDm=QYRY_KQJL&FormStatus=0&MenuCode=QYRY_KQJL&FormParam=PARAM--"+sfzhm;
	parent.layer.open({
		type: 2,
		title: '人员考勤查询',
		shadeClose: true,
		shade: 0.8,
		area: ['95%', '95%'],
		content: url,
		end: function () {
			parent.layer.closeAll();
		}
	});
}


function SearchGCRYKQ(a) {
	var ret = false;
    var sfzhm=GetCtrlValue("I_M_RY.SFZHM");
	var url="/WebList/EasyUiIndex?FormDm=GCGL_RYKQ&FormStatus=0&FormParam=PARAM--"+sfzhm;
	parent.layer.open({
		type: 2,
		title: '人员考勤查询',
		shadeClose: true,
		shade: 0.8,
		area: ['95%', '95%'],
		content: url,
		end: function () {
			parent.layer.closeAll();
		}
	});
}


function SearchRYKQ_QY() {  //企业
	if (!IsSubmitFormValid())
		return;
	var ret = false;
    var sfzhm=GetCtrlValue("I_M_RY.SFZHM");
	var url="/WebList/EasyUiIndex?FormDm=QYRY_KQJL_QY&FormStatus=0&FormParam=PARAM--"+sfzhm;
	parent.layer.open({
		type: 2,
		title: '人员考勤查询',
		shadeClose: true,
		shade: 0.8,
		area: ['95%', '95%'],
		content: url,
		end: function () {
			parent.layer.closeAll();
		}
	});
}