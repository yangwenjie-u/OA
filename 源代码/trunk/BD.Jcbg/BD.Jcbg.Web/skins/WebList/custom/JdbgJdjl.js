function view()
{
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;

        var reportfile = selected.ReportFile;
        var serial = selected.WorkSerial;
        var recid = selected.RECID;
		var reporttype = selected.LX;
        reportfile = GetAvailableReportFile(reportfile, serial);
        parent.parent.layer.open({
            type: 2,
            title: '报表查看',
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: "/jdbg/flowreport?reportfile="+encodeURIComponent(reportfile)+"&serial="+encodeURIComponent(serial)+"&jdjlid="+recid + "&reporttype=" + reporttype,
            end: function () {
            }
        });
    }catch(ex){
        alert(ex);
    }
}
<<<<<<< .mine
function viewZgd() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;
        var canprint = '0';
        if (selected.LRRXM != "")
            canprint = "1";
        var reportfile = selected.ReportFile;
        var serial = selected.WorkSerial;
        var recid = selected.RECID;
        reportfile = GetAvailableReportFile(reportfile, serial);
        // 校验用户是否需要输入手机号码验证
        $.ajax({
            type: "POST",
            url: "/dwgxwz/CheckReportUser?reportfile=" + encodeURIComponent(reportfile) + "&serial=" + encodeURIComponent(serial) + "&jdjlid=" + recid + "&print=" + canprint + "&reporttype=ZGD",
            dataType: "json",
            async: false,
            success: function (ret) {
                // 校验成功
                if (ret.code == "0") {
                    var data = ret.data;
                    // 需要用户进行验证
                    if (data.result == "0") {
                        var p = data.jsonparams;
                        parent.parent.layer.open({
                            type: 2,
                            title: '电子签收验证',
                            shadeClose: false,
                            shade: 0.8,
                            area: ['95%', '95%'],
                            content: "/dwgxwz/ValidateReportUser?p=" + p,
                            end: function () {
                            }
                        });
                    }
                    //不需要验证
                    else { 
                        parent.parent.layer.open({
                            type: 2,
                            title: '报表查看',
                            shadeClose: false,
                            shade: 0.8,
                            area: ['95%', '95%'],
                            content: "/jdbg/flowreport?reportfile=" + encodeURIComponent(reportfile) + "&serial=" + encodeURIComponent(serial) + "&jdjlid=" + recid + "&print=" + canprint + "&reporttype=ZGD",
                            end: function () {
                            }
                        });
                    }
                }
                // 校验失败
                else {
                    var msg = ret.msg;
                    if (msg == "") {
                        msg = "用户校验失败！";
                    }
                    layer.alert(msg);  
                }
                    
            },
            complete: function (XMLHttpRequest, textStatus) {
            },
            beforeSend: function (XMLHttpRequest) {
            }
        });
        
    } catch (ex) {
        alert(ex);
    }
}
||||||| .r98
=======
function viewZgd() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;
        var canprint = '0';
        if (selected.LRRXM != "")
            canprint = "1";
        var reportfile = selected.ReportFile;
        var serial = selected.WorkSerial;
        var recid = selected.RECID;
        reportfile = GetAvailableReportFile(reportfile, serial);
        parent.parent.layer.open({
            type: 2,
            title: '报表查看',
            shadeClose: true,
            shade: 0.8,
            area: ['95%', '95%'],
            content: "/jdbg/flowreport?reportfile=" + encodeURIComponent(reportfile) + "&serial=" + encodeURIComponent(serial) + "&jdjlid=" + recid+"&print="+canprint,
            end: function () {
            }
        });
    } catch (ex) {
        alert(ex);
    }
}
>>>>>>> .r220
function viewZghf() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;
        if (selected.ExtraInfo3 == "全部未回复") {
            alert("整改单全部未回复，无法查看回复情况");
            return;
        }

        var reportfile = selected.ExtraInfo15;
        var serial = selected.WorkSerial;
        var recid = selected.RECID;

        parent.parent.layer.open({
            type: 2,
            title: '查看整改回复',
            shadeClose: true,
            shade: 0.8,
            area: ['95%', '95%'],
            content: "/jdbg/flowreport?reportfile=" + encodeURIComponent(reportfile) + "&serial=" + encodeURIComponent(serial) + "&jdjlid=" + recid+ "&reporttype=ZGHF",
            end: function () {
            }
        });
    } catch (ex) {
        alert(ex);
    }
}
// 获取实际可用的reportfile
// 主要考虑到，手机填写的监督记录，在后台没有生成模板文件，也就是reportfile为空
// 因此，如果在pc端查看手机填写的监督记录，需要到后台先生成模板文件
function GetAvailableReportFile(reportfile, serial) {
    var realReportFile = reportfile;

    if (realReportFile == null || realReportFile == '') {
        $.ajax({
            type: "POST",
            url: "/jdbg/GeneratePhoneJDJLTemplate",
            data: "template=" + encodeURIComponent("监督记录v1.docx") + "&serial=" + serial,
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0")
                    realReportFile = data.msg;
            },
            complete: function (XMLHttpRequest, textStatus) {
            },
            beforeSend: function (XMLHttpRequest) {
            }
        }); 
    }

    return realReportFile;

}

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

<<<<<<< .mine
function setBXKF() {
||||||| .r98
function setHFZT(lx) {
=======
function setBXKF() {
    var selected = pubselect();
    if (selected == undefined)
        return;
    var zgdbh = selected.ExtraInfo4;
    if (zgdbh && zgdbh != "") {
        var rdm = Math.random();
        var bcode = new Base64();
        var extrainfo1 = bcode.encode("view_jdbg_jdjl|" + bcode.encode("lx='zgd' and extrainfo4='" + zgdbh + "'"));
        var url = "/workflow/startwork?processid=42" +
            // 加密的条件信息（表单中用到）
            "&extrainfo=" + encodeURIComponent(extrainfo1) +
            //// 流程中显示的主体信息
            //"&extrainfo2=" + encodeURIComponent(extrainfo2) +
            //// 流程中用到的跟工程关联的主键
            //"&extrainfo3=" + encodeURIComponent(extrainfo3) +
            //// 流程中用到的分工程主键
            //"&extrainfo4=" + encodeURIComponent(extrainfo4) +
            "&callbackjs=" + encodeURIComponent("parent.layer.closeAll();") +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '整改单不需扣分申请',
            shadeClose: true,
            shade: 0.8,
            area: ['95%', '95%'],
            content: url,
            end: function () {
            }
        });
    }
}

function setHFZT(lx) {
>>>>>>> .r220
    var selected = pubselect();
    if (selected == undefined)
        return;
    var jbr = selected.CJRY;
    if (jbr == null || jbr == '') {
        layer.alert('整改单经办人为空!');
    }
    else {
        // 校验整改单经办人是否为当前用户
        $.ajax({
            type: "POST",
            url: "/jdbg/checkzgdjbr?jbr=" + encodeURIComponent(jbr),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0") {
                    if (data.msg == "0") {
                        layer.alert('您不是当前整改单经办人，不能设置整改单扣分!');
                    }
                    else if (data.msg=="1") {
                        var zgdbh = selected.ExtraInfo4;
                        if (zgdbh && zgdbh != "") {
                            var rdm = Math.random();
                            var bcode = new Base64();
                            var extrainfo1 = bcode.encode("view_jdbg_jdjl|" + bcode.encode("lx='zgd' and extrainfo4='" + zgdbh + "'"));
                            var url = "/workflow/startwork?processid=42" +
                                // 加密的条件信息（表单中用到）
                                "&extrainfo=" + encodeURIComponent(extrainfo1) +
                                //// 流程中显示的主体信息
                                //"&extrainfo2=" + encodeURIComponent(extrainfo2) +
                                //// 流程中用到的跟工程关联的主键
                                //"&extrainfo3=" + encodeURIComponent(extrainfo3) +
                                //// 流程中用到的分工程主键
                                //"&extrainfo4=" + encodeURIComponent(extrainfo4) +
                                "&callbackjs=" + encodeURIComponent("parent.layer.closeAll();") +
                                "&_=" + rdm;

                            parent.layer.open({
                                type: 2,
                                title: '整改单扣分申请',
                                shadeClose: false,
                                shade: 0.8,
                                area: ['95%', '95%'],
                                content: url,
                                end: function () {
                                }
                            });
                        }
                    }

                } else {
                    if (data.msg == "")
                        data.msg = "校验整改单经办人出错！";
                    layer.alert(data.msg);
                }
            },
            complete: function (XMLHttpRequest, textStatus) {
            },
            beforeSend: function (XMLHttpRequest) {
            }
        });
    }
    
}


function setHFZT() {
    var selected = pubselect();
    if (selected == undefined)
        return;
    if (selected.ExtraInfo1 == '已回复') {
        layer.alert('已回复的整改记录不能设置!');
        return;
    }
    else if (selected.ExtraInfo1 == '未回复') {
        var serial = selected.WorkSerial;
        var zgdbh = selected.ExtraInfo4;
        parent.layer.open({
            type: 2,
            title: '设置整改状态',
            shadeClose: false,
            shade: 0.5,
            area: ['600px', '300px;'],
            content: "/jdbg/ZGDHFZT?serial="+serial + "&zgdbh="+zgdbh,
            btn: ["确定", "关闭"],
            yes: function (index) {
                var hfzt = window.parent[layerObj.find('iframe')[0]['name']].getValues();
                if (hfzt == null ||  hfzt=="") {
                    alert("请选择整改状态");
                } else {
                    parent.layer.closeAll();

                    if (hfzt != '') {
                        $.ajax({
                            type: "POST",
                            url: "/jdbg/updatezgdhfzt",
                            data: { hfzt: hfzt, serial: serial},
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
    
}



function Modify() {
    var selected = pubselect();
    if (selected == undefined)
        return;
    try {
        var serialno = selected.WorkSerial;
        var lx = selected.LX;
        $.ajax({
            type: "POST",
            url: "/dwgxwz/getjdbgjdjlmodifyinfo",
            data: { serialno: serialno, lx: lx, createuser: selected.LRRZH },
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.processid != "0") {
                    var workurl = '/workflow/startwork?processid=' + data.processid;
                    var bcode = new Base64();
                    var title = "记录修改";
                    var extrainfo1 = bcode.encode(data.extrainfo1);
                    var extrainfo2 = bcode.encode(data.extrainfo2);
                    var extrainfo3 = bcode.encode(data.extrainfo3);
                    var extrainfo4 = bcode.encode(data.extrainfo4);
                    var extrainfo5 = bcode.encode(data.extrainfo5);
                    var extrainfo6 = bcode.encode(data.extrainfo6);
                    var parentserial = serialno;

                    gotoStarkWork(workurl, title, extrainfo1, extrainfo2, extrainfo3, extrainfo4, parentserial, extrainfo5, extrainfo6);
                }
                else {
                    if (data.msg != "")
                        alert(data.msg);
                    else
                        alert("当前记录不允许修改，请联系系统管理人员！");
                }

            },
            complete: function (XMLHttpRequest, textStatus) {
            },
            beforeSend: function (XMLHttpRequest) {
            }
        });
    }
    catch (ex) {
        alert(ex);
    }

}


function gotoStarkWork(workurl, title, extrainfo1, extrainfo2, extrainfo3, extrainfo4, parentserial, extrainfo5, extrainfo6) {
    try {
        var rdm = Math.random();
        var url = workurl +
            // 加密的条件信息（表单中用到）
            "&extrainfo=" + encodeURIComponent(extrainfo1) +
            // 流程中显示的主体信息
            "&extrainfo2=" + encodeURIComponent(extrainfo2) +
            // 流程中用到的跟工程关联的主键
            "&extrainfo3=" + encodeURIComponent(extrainfo3) +
            // 流程中用到的分工程主键
            "&extrainfo4=" + encodeURIComponent(extrainfo4) +
            "&extrainfo5=" + encodeURIComponent(extrainfo5) +
            "&extrainfo6=" + encodeURIComponent(extrainfo6) +
            "&parentserial=" + encodeURIComponent(parentserial) +
            "&callbackjs=" + encodeURIComponent("parent.layer.closeAll();") +
            "&preurldone=true&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: title,
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: url,
            end: function () {
                searchRecord();
            }
        });

    }
    catch (ex) {
        alert(ex);
    }
}




// 监督报告是否已审批，格式化显示
function FormatJdbgsp(value, row, index) {
    var imgurl = "";
    try {
        var ivalue = value * 1;

        if (value == 1)
            imgurl += "<center><img src='/skins/default/images/list/set_green.png' title='已审批'/></center>";
        else
            imgurl += "<center><img src='/skins/default/images/list/set_gray.png' title='未审批'/></center>";
    } catch (e) {
        alert(e);
    }
    imgurl += "";
    return imgurl;
}


// 格式化整改期限
function FormatZGQX(value, row, index) {
    var ret = value;
    try {
		if(value!="" && row.ExtraInfo1=='未回复'){
			var zgqx=new Date(value);
			var now = new Date();
			var today = new Date(now.getFullYear(), now.getMonth(), now.getDate());
			if(today > zgqx){
				ret = "<span style=\"background-color: #B2DFEE; color: #FF0000\">" + value + "</span>";
			}
			else {
				var deadline = new Date(zgqx.setDate(zgqx.getDate()-6));
				if(today >= deadline){
					ret = "<span style=\"background-color: #B2DFEE; color: #FF6EB4\">" + value + "</span>";
				}
				else{
					ret = value;
				}
			}
		}
				
    } catch (e) {
        alert(e);
    }
    return ret;
}


// 格式化扣分情况
function FormatKF(value, row, index) {
    var ret = "";
    try {
		if(value=="0")
			ret="不扣分";
		else if(value=="1")
			ret="扣分"; 
    } catch (e) {
        alert(e);
    }
    return ret;
}


// 查看监督报告
function viewJdbg() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;
        var canprint = '0';
        if (selected.ExtraInfo8 == "1")
            canprint = "1";
        var reportfile = selected.ReportFile;
        var serial = selected.WorkSerial;
        var recid = selected.RECID;
		var reporttype = selected.LX;
        reportfile = GetAvailableReportFile(reportfile, serial);
        parent.parent.layer.open({
            type: 2,
            title: '查看监督报告',
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: "/jdbg/flowreport?reportfile=" + encodeURIComponent(reportfile) + "&serial=" + encodeURIComponent(serial) + "&jdjlid=" + recid+"&print="+canprint+ "&reporttype=" + reporttype,
            end: function () {
            }
        });
    } catch (ex) {
        alert(ex);
    }
}

function del() {
    try {
        var tabledesc = "记录删除";                // 表格描述

        //var rowindex = dataGrid.jqxGrid('getselectedrowindex');

        //if (rowindex == -1) {
        //    alert("请选择要操作的" + tabledesc);
        //    return;
        //}

        //var selected = dataGrid.jqxGrid('getrowdata', rowindex);
        var selected = pubselect();

        if (!selected) {
            return;
        }


        if (!confirm("确定要删除所选的记录吗？")) {
            return;
        }
        $.ajax({
            type: "POST",
            url: "/delete/DeleteJDJL?RECID=" + encodeURIComponent(selected.RECID),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0") {
                    alert("删除成功！");
                    searchRecord();
                } else {
                    if (data.msg == "")
                        data.msg = "删除失败";
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



function del2() {
    try {
        var tabledesc = "记录删除";                // 表格描述

        //var rowindex = dataGrid.jqxGrid('getselectedrowindex');

        //if (rowindex == -1) {
        //    alert("请选择要操作的" + tabledesc);
        //    return;
        //}

        //var selected = dataGrid.jqxGrid('getrowdata', rowindex);
        var selected = pubselect();

        if (!selected) {
            return;
        }


        if (!confirm("确定要删除所选的记录吗？")) {
            return;
        }
        $.ajax({
            type: "POST",
            url: "/delete/DeleteJDBG?RECID=" + encodeURIComponent(selected.RECID),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0") {
                    alert("删除成功！");
                    searchRecord();
                } else {
                    if (data.msg == "")
                        data.msg = "删除失败";
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

function showyqfj() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;
        var serial = selected.WorkSerial;
        var zgdbh = selected.ExtraInfo4;
        parent.parent.layer.open({
            type: 2,
            title: '查看申请内容',
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: "/jdbg/showyqfj?serial=" + encodeURIComponent(serial) + "&zgdbh=" + encodeURIComponent(zgdbh),
            end: function () {
            }
        });

    } catch (e) {
        alert(e);
    }

}



function Base64() {

    // private property
    _keyStr = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";

    // public method for encoding
    this.encode = function (input) {
        var output = "";
        var chr1, chr2, chr3, enc1, enc2, enc3, enc4;
        var i = 0;
        input = _utf8_encode(input);
        while (i < input.length) {
            chr1 = input.charCodeAt(i++);
            chr2 = input.charCodeAt(i++);
            chr3 = input.charCodeAt(i++);
            enc1 = chr1 >> 2;
            enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
            enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
            enc4 = chr3 & 63;
            if (isNaN(chr2)) {
                enc3 = enc4 = 64;
            } else if (isNaN(chr3)) {
                enc4 = 64;
            }
            output = output +
            _keyStr.charAt(enc1) + _keyStr.charAt(enc2) +
            _keyStr.charAt(enc3) + _keyStr.charAt(enc4);
        }
        return output;
    }

    // public method for decoding
    this.decode = function (input) {
        var output = "";
        var chr1, chr2, chr3;
        var enc1, enc2, enc3, enc4;
        var i = 0;
        input = input.replace(/[^A-Za-z0-9\+\/\=]/g, "");
        while (i < input.length) {
            enc1 = _keyStr.indexOf(input.charAt(i++));
            enc2 = _keyStr.indexOf(input.charAt(i++));
            enc3 = _keyStr.indexOf(input.charAt(i++));
            enc4 = _keyStr.indexOf(input.charAt(i++));
            chr1 = (enc1 << 2) | (enc2 >> 4);
            chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
            chr3 = ((enc3 & 3) << 6) | enc4;
            output = output + String.fromCharCode(chr1);
            if (enc3 != 64) {
                output = output + String.fromCharCode(chr2);
            }
            if (enc4 != 64) {
                output = output + String.fromCharCode(chr3);
            }
        }
        output = _utf8_decode(output);
        return output;
    }

    // private method for UTF-8 encoding
    _utf8_encode = function (string) {
        string = string.replace(/\r\n/g, "\n");
        var utftext = "";
        for (var n = 0; n < string.length; n++) {
            var c = string.charCodeAt(n);
            if (c < 128) {
                utftext += String.fromCharCode(c);
            } else if ((c > 127) && (c < 2048)) {
                utftext += String.fromCharCode((c >> 6) | 192);
                utftext += String.fromCharCode((c & 63) | 128);
            } else {
                utftext += String.fromCharCode((c >> 12) | 224);
                utftext += String.fromCharCode(((c >> 6) & 63) | 128);
                utftext += String.fromCharCode((c & 63) | 128);
            }

        }
        return utftext;
    }

    // private method for UTF-8 decoding
    _utf8_decode = function (utftext) {
        var string = "";
        var i = 0;
        var c = c1 = c2 = 0;
        while (i < utftext.length) {
            c = utftext.charCodeAt(i);
            if (c < 128) {
                string += String.fromCharCode(c);
                i++;
            } else if ((c > 191) && (c < 224)) {
                c2 = utftext.charCodeAt(i + 1);
                string += String.fromCharCode(((c & 31) << 6) | (c2 & 63));
                i += 2;
            } else {
                c2 = utftext.charCodeAt(i + 1);
                c3 = utftext.charCodeAt(i + 2);
                string += String.fromCharCode(((c & 15) << 12) | ((c2 & 63) << 6) | (c3 & 63));
                i += 3;
            }
        }
        return string;
    }
}
// 监督报告是否已审批，格式化显示
function FormatJdbgsp(value, row, index) {
    var imgurl = "";
    try {
        var ivalue = value * 1;

        if (value == 1)
            imgurl += "<center><img src='/skins/default/images/list/set_green.png' title='已审批'/></center>";
        else
            imgurl += "<center><img src='/skins/default/images/list/set_gray.png' title='未审批'/></center>";
    } catch (e) {
        alert(e);
    }
    imgurl += "";
    return imgurl;
}
// 查看监督报告
function viewJdbg() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;
        var canprint = '0';
        if (selected.ExtraInfo8 == "1")
            canprint = "1";
        var reportfile = selected.ReportFile;
        var serial = selected.WorkSerial;
        var recid = selected.RECID;
        reportfile = GetAvailableReportFile(reportfile, serial);
        parent.parent.layer.open({
            type: 2,
            title: '查看监督报告',
            shadeClose: true,
            shade: 0.8,
            area: ['95%', '95%'],
            content: "/jdbg/flowreport?reportfile=" + encodeURIComponent(reportfile) + "&serial=" + encodeURIComponent(serial) + "&jdjlid=" + recid+"&print="+canprint,
            end: function () {
            }
        });
    } catch (ex) {
        alert(ex);
    }
}

function del() {
    try {
        var tabledesc = "记录删除";                // 表格描述

        //var rowindex = dataGrid.jqxGrid('getselectedrowindex');

        //if (rowindex == -1) {
        //    alert("请选择要操作的" + tabledesc);
        //    return;
        //}

        //var selected = dataGrid.jqxGrid('getrowdata', rowindex);
        var selected = pubselect();

        if (!selected) {
            return;
        }


        if (!confirm("确定要删除所选的记录吗？")) {
            return;
        }
        $.ajax({
            type: "POST",
            url: "/delete/DeleteJDJL?RECID=" + encodeURIComponent(selected.RECID),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0") {
                    alert("删除成功！");
                    searchRecord();
                } else {
                    if (data.msg == "")
                        data.msg = "删除失败";
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


function Base64() {

    // private property
    _keyStr = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";

    // public method for encoding
    this.encode = function (input) {
        var output = "";
        var chr1, chr2, chr3, enc1, enc2, enc3, enc4;
        var i = 0;
        input = _utf8_encode(input);
        while (i < input.length) {
            chr1 = input.charCodeAt(i++);
            chr2 = input.charCodeAt(i++);
            chr3 = input.charCodeAt(i++);
            enc1 = chr1 >> 2;
            enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
            enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
            enc4 = chr3 & 63;
            if (isNaN(chr2)) {
                enc3 = enc4 = 64;
            } else if (isNaN(chr3)) {
                enc4 = 64;
            }
            output = output +
            _keyStr.charAt(enc1) + _keyStr.charAt(enc2) +
            _keyStr.charAt(enc3) + _keyStr.charAt(enc4);
        }
        return output;
    }

    // public method for decoding
    this.decode = function (input) {
        var output = "";
        var chr1, chr2, chr3;
        var enc1, enc2, enc3, enc4;
        var i = 0;
        input = input.replace(/[^A-Za-z0-9\+\/\=]/g, "");
        while (i < input.length) {
            enc1 = _keyStr.indexOf(input.charAt(i++));
            enc2 = _keyStr.indexOf(input.charAt(i++));
            enc3 = _keyStr.indexOf(input.charAt(i++));
            enc4 = _keyStr.indexOf(input.charAt(i++));
            chr1 = (enc1 << 2) | (enc2 >> 4);
            chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
            chr3 = ((enc3 & 3) << 6) | enc4;
            output = output + String.fromCharCode(chr1);
            if (enc3 != 64) {
                output = output + String.fromCharCode(chr2);
            }
            if (enc4 != 64) {
                output = output + String.fromCharCode(chr3);
            }
        }
        output = _utf8_decode(output);
        return output;
    }

    // private method for UTF-8 encoding
    _utf8_encode = function (string) {
        string = string.replace(/\r\n/g, "\n");
        var utftext = "";
        for (var n = 0; n < string.length; n++) {
            var c = string.charCodeAt(n);
            if (c < 128) {
                utftext += String.fromCharCode(c);
            } else if ((c > 127) && (c < 2048)) {
                utftext += String.fromCharCode((c >> 6) | 192);
                utftext += String.fromCharCode((c & 63) | 128);
            } else {
                utftext += String.fromCharCode((c >> 12) | 224);
                utftext += String.fromCharCode(((c >> 6) & 63) | 128);
                utftext += String.fromCharCode((c & 63) | 128);
            }

        }
        return utftext;
    }

    // private method for UTF-8 decoding
    _utf8_decode = function (utftext) {
        var string = "";
        var i = 0;
        var c = c1 = c2 = 0;
        while (i < utftext.length) {
            c = utftext.charCodeAt(i);
            if (c < 128) {
                string += String.fromCharCode(c);
                i++;
            } else if ((c > 191) && (c < 224)) {
                c2 = utftext.charCodeAt(i + 1);
                string += String.fromCharCode(((c & 31) << 6) | (c2 & 63));
                i += 2;
            } else {
                c2 = utftext.charCodeAt(i + 1);
                c3 = utftext.charCodeAt(i + 2);
                string += String.fromCharCode(((c & 15) << 12) | ((c2 & 63) << 6) | (c3 & 63));
                i += 3;
            }
        }
        return string;
    }
}