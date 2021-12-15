function show() {

    try {
        

        //var rowindex = dataGrid.jqxGrid('getselectedrowindex');

        //if (rowindex == -1) {
        //    alert("请选择要操作的" + tabledesc);
        //    return;
        //}
        //var selected = dataGrid.jqxGrid('getrowdata', rowindex);
        var selected = pubselect();//dataGrid.jqxGrid('getrowdata', rowindex);
        if (!selected) {
            layer.alert('请先选择一条数据', { icon: 5 });
            return;
        }


        var pt = encodeURIComponent(selected.POLICETYPE)   // 警种
        var ptc = encodeURIComponent(selected.POLICETYPECODE); 	// 警种code
        var ny = encodeURIComponent(selected.NY); 	//年月
        var ac = encodeURIComponent(selected.AREACODE); 	// 区域代码
        var an = encodeURIComponent(selected.AREANAME); 	// 区域
        var bmy = encodeURIComponent(selected.BMY); 	// 不满意数
        if (bmy <= 0) {
            layer.alert('该区域警种没有不满意的短信', { icon: 6 });
            return;
        }
        var tabledesc = selected.NY + selected.AREANAME + selected.POLICETYPE + "不满意短信查看";
        
        var rdm = Math.random();
        var url = "/WebList/EasyUiIndex?FormDm=MR_BMY_QYJZ_NY&FormStatus=0&MenuCode=MR_BMY_QYJZ_NY&FormFilter=0"
                + "&FormParam=PARAM--" + ac + "|" + ptc + "|" + ny + ""
                + "&_=" + rdm;


        parent.layer.open({
            type: 2,
            title:  tabledesc,
            shadeClose: true,
            shade: 0.8,
            area: ['95%', '95%'],
            content: url
        });





    } catch (e) {
        alert(e);
    }
}