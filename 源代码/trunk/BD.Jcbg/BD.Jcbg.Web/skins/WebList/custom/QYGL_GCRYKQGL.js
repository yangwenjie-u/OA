function setKQ1() {
    try {
		
		var selecteds = pubselects();
        if (selecteds == undefined) {
            return;
        }
		var rybhs="";
		var sfzhms="";
		$.each(selecteds, function (index, obj) {
			rybhs += obj.RYBH + ",";
			sfzhms+= obj.SFZHM+ ",";
        })
		
		$.ajax({
            type: "POST",
            url: "/JX_Info/getgckqtype",
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0") {
                    $.ajax({
						type: "POST",
						url: "/jx_info/SetGcryKq",//?recids=" + encodeURIComponent(recids)+"&jdzchs="+encodeURIComponent(jdzchs),
						data:{
							sfzhms:sfzhms
						},
						dataType: "json",
						async: false,
						success: function (data) {
							if (data.code == "0") {
								alert("设置成功！");
								searchRecord();
							} else {
								if (data.msg == "")
									data.msg = "设置失败";
								alert(data.msg);
							}
						},
						complete: function (XMLHttpRequest, textStatus) {
						},
						beforeSend: function (XMLHttpRequest) {
						}
					});
                } else {
                    if (data.msg == "")
                        data.msg = "设置失败,该工程无法手动设置";
                    alert(data.msg);
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

function setKQ() {
    try {
		
		var selecteds = pubselects();
        if (selecteds == undefined) {
            return;
        }
		var rybhs="";
		var sfzhms="";
		$.each(selecteds, function (index, obj) {
			rybhs += obj.RYBH + ",";
			sfzhms+= obj.SFZHM+ ",";
        })
		$.ajax({
            type: "POST",
            url: "/JX_Info/getgckqtype",
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0") {
					 $.ajax({
						type: 'POST',
						url: '/JX_Info/SetKqView',//发送请求
						data: { sfzhms: sfzhms},
						dataType: "html",
						success: function (result) {
							var htmlCont = result;//返回的结果页面
							parent.layer.open({
								type: 1,//弹出框类型
								title: '设置考勤日期',
								shadeClose: false, //点击遮罩关闭层
								area: ['30%', '40%'],
								shift: 1,
								content: htmlCont,//将结果页面放入layer弹出层中
								success: function (layero, index) {

								}
							});
						}
					});
					                  
                } else {
                    if (data.msg == "")
                        data.msg = "设置失败,该工程无法手动设置";
                    alert(data.msg);
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

