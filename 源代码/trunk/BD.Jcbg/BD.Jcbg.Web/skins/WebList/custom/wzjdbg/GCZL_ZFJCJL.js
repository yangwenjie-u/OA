function switchRecord(obj) {
    try {
        var strLocation = decodeURIComponent(window.location.href);
        // 所有
        if (obj.checked) {
            strLocation = strLocation.replace("NOT||CHECKBOX", "ALL||CHECKBOX").replace("所有|我的", "所有|所有");
        }
        else {
            strLocation = strLocation.replace("ALL||CHECKBOX", "NOT||CHECKBOX").replace("所有|所有", "所有|我的");
        }
        window.location = strLocation;
    } catch (ex) {
        alert(ex);
    }
}


function switchYesNo(obj) {
    try {
        var strLocation = decodeURIComponent(window.location.href);
        // 已读
        if (obj.checked) {
            strLocation = strLocation.replace("NO||CHECKBOX", "YES||CHECKBOX").replace("已读|未读", "已读|已读");
        }
        else {
            strLocation = strLocation.replace("YES||CHECKBOX", "NO||CHECKBOX").replace("已读|已读", "已读|未读");
        }
        window.location = strLocation;
    } catch (ex) {
        alert(ex);
    }
}

function setRead() {
    var selected = pubselect();
    if (selected == undefined)
        return;
    var apid = selected.APID;
	parent.layer.open({
		type: 2,
		title: '设置阅读状态',
		shadeClose: false,
		shade: 0.5,
		area: ['600px', '300px;'],
		content: "/dwgxwz/YSAPZT?apid="+apid ,
		btn: ["确定", "关闭"],
		yes: function (index) {
			var zt = window.parent[layerObj.find('iframe')[0]['name']].getValues();
			if (zt == null ||  zt=="") {
				alert("请选择阅读状态");
			} else {
				parent.layer.closeAll();

				if (zt != '') {
					$.ajax({
						type: "POST",
						url: "/dwgxwz/updateysapzt",
						data: { zt: zt, apid: apid},
						dataType: "json",
						async: false,
						success: function (data) {
							if (data.code == "0") {
								alert('设置成功');
								searchRecord();
							}
							else {
								alert(data.msg || "设置失败！");
							}

						},
						complete: function (XMLHttpRequest, textStatus) {
						},
						beforeSend: function (XMLHttpRequest) {
						}
					});
				}
			}

		},
		success: function (layero, index) {
			layerObj = layero;
		},
		btn2: function (index) {
			parent.layer.closeAll();
		},
		end: function () {
		}
	});
    
}

function printzfjctzs() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;
        var reportfile = selected.ZFJCGZS;
        var serial = selected.WORKSERIAL;
        var recid = selected.JDJLID;
        if (serial==null ||  serial=='') {
            alert('当前检查记录来自于老系统,无法查看执法检查告知书！');
            return;
        }
        parent.parent.layer.open({
            type: 2,
            title: '报表查看',
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: "/jdbg/flowreport?reportfile=" + encodeURIComponent(reportfile) + "&serial=" + encodeURIComponent(serial) + "&jdjlid=" + recid+"&reporttype=ZFJCGZS",
            end: function () {
            }
        });
    } catch (ex) {
        alert(ex);
    }
}


function printzfjcbab() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;
        var reportfile = selected.ZFJCBAB;
        var serial = selected.WORKSERIAL;
        var recid = selected.JDJLID;
        if (serial==null ||  serial=='') {
            alert('当前检查记录来自于老系统,无法查看执法检查告知书！');
            return;
        }
        parent.parent.layer.open({
            type: 2,
            title: '报表查看',
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: "/jdbg/flowreport?reportfile=" + encodeURIComponent(reportfile) + "&serial=" + encodeURIComponent(serial) + "&jdjlid=" + recid+"&reporttype=ZFJCBAB",
            end: function () {
            }
        });
    } catch (ex) {
        alert(ex);
    }
}