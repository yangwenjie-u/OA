function realknow()
{
	var selected = pubselect();
	if (selected == undefined)
		return;
	
	if(selected.SY_ZT!="未提交")
	{		
		alert("该工程备案信息已录入，请勿重复录入");
		return;
	}
		
	
    var url = "/jx_info/agreement";
    parent.layer.open({
        type: 2,
        title: '阅读须知',
        shadeClose: false,
        shade: 0.8,
        area: ['600px', '400px'],
        content: url,
		btn: ["我已阅读并会严格遵守执行", "取消"],
		yes: function(){
		
				parent.layer.closeAll();
				add(selected);
		  },
		btn2: function (index) {

		  }
    });
}



function add(selected) {
    try {
        if (selected == undefined)
            return;

		var gcmc=selected.GCMC;
		
        var tabledesc = "开工备案";                // 表格描述
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_GC_KGBA"); 			// 表名
        var tablerecid = encodeURIComponent("RECID"); 	// 表主键
        var title = encodeURIComponent(tabledesc); 	// 标题
        var formdm = tablename;                             // 列表key名称
        var buttons = encodeURIComponent("保存|TJ| "); // 按钮
        var para = "I_M_GC_KGBA,GCMC," + selected.GCMC + "|I_M_GC_KGBA,GCDD," + selected.GCDD + "|I_M_GC_KGBA,SGDWMC," + selected.SY_SGDWMC +
            "|I_M_GC_KGBA,SGFRDB," + selected.FRDB + "|I_M_GC_KGBA,FRDH," + selected.FRDBDH +
            "|I_M_GC_KGBA,SGXMFZR," + selected.SGXMFZR + "|I_M_GC_KGBA,FZRDH," + selected.SGXMFZRDH +
             "|I_M_GC_KGBA,JDZCH," + selected.JDZCH+"|I_M_GC_KGBA,QYBH,"+selected.QYBH;
			 
        var fieldparam = encodeURIComponent(para)
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&fieldparam=" + fieldparam+
            "&rownum=2" +
			"&LX=N"+
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '录入' + tabledesc,
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: url,
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}
function view() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;
	
        var gcmc = selected.GCMC;

        var tabledesc = "开工备案";                // 表格描述
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_GC_KGBA"); 			// 表名
        var tablerecid = encodeURIComponent("JDZCH"); 	// 表主键
        var title = encodeURIComponent(tabledesc); 	// 标题
        var formdm = tablename;                             // 列表key名称
        var buttons = "";// encodeURIComponent("保存|TJ| | ");
        var jydbh = encodeURIComponent(selected.JDZCH)   // 键值
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&jydbh="+jydbh+
            "&rownum=2" +
			"&view=true" +
			"&LX=N"+
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '录入' + tabledesc,
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: url,
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}
function edit() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;
		if(selected.SY_ZT=="通过")
		{		
             alert("该工程已经通过");
            return;
		}
		
        var gcmc = selected.GCMC;

        var tabledesc = "开工备案";                // 表格描述
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_GC_KGBA"); 			// 表名
        var tablerecid = encodeURIComponent("JDZCH"); 	// 表主键
        var title = encodeURIComponent(tabledesc); 	// 标题
        var formdm = tablename;                             // 列表key名称
        var buttons = encodeURIComponent("保存|TJ| | ");
        var jydbh = encodeURIComponent(selected.JDZCH)   // 键值
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&jydbh="+jydbh+
            "&rownum=2" +
			"&LX=N"+			
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '录入' + tabledesc,
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: url,
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}

function edit2() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;
		if(selected.SY_ZT=="未提交")
		{		
             alert("该工程未录入备案信息");
            return;
		}
        var gcmc = selected.GCMC;

        var tabledesc = "开工备案";                // 表格描述
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_GC_KGBA"); 			// 表名
        var tablerecid = encodeURIComponent("JDZCH"); 	// 表主键
        var title = encodeURIComponent(tabledesc); 	// 标题
        var formdm = tablename;                             // 列表key名称
        var buttons = encodeURIComponent("保存|TJ| | ");
        var jydbh = encodeURIComponent(selected.JDZCH)   // 键值
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&jydbh="+jydbh+
            "&rownum=2" +
			"&LX=J" +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '录入' + tabledesc,
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: url,
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}

function apply() {
    try {
        var tabledesc = "工程";                // 表格描述
        var selected = pubselect();// dataGrid.jqxGrid('getrowdata', rowindex);
        if (selected == undefined)
            return;
        if (selected.SY_ZT == "通过") {
            alert("该工程已经通过");
            return;
        }
        if (!confirm("确定要提交吗？")) {
            return;
        }
        var gcmc = selected.GCMC;
        $.ajax({
            type: "POST",
            url: "/qy/submitgckgba?jdzch=" + encodeURIComponent(selected.JDZCH),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0") {
                    alert("提交成功！");         
                    $.ajax({
                       type: "POST",
                       url: "/jdbg/SetInfoMail?mailcon=" + encodeURIComponent(gcmc + "进行工程开工备案") + "&maintitle=" + encodeURIComponent("开工备案"),
                       dataType: "json",
                       async: false,
                       success: function (data) {
                           // if (data.code != 0)
                           //alert("发送邮件失败，详细信息：" + data.msg);
                           // else {
                           // alert("发送邮件成功");
                           // }

                       },
                       complete: function (XMLHttpRequest, textStatus) {
						      searchRecord();
                       },
                       beforeSend: function (XMLHttpRequest) {
                       }
                    });
                } else {
                    if (data.msg == "")
                        data.msg = "提交失败";
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
function printGc()
{
	    var selected = pubselect();// dataGrid.jqxGrid('getrowdata', rowindex);
        if (selected == undefined)
            return;
		selectGc(selected);
}


function selectGc(selected) {
    try {

        if (selected == undefined)
            return;
        var table = "View_I_M_GC_KGBA";
        var jdzch = selected.JDZCH;
        var bcode = new Base64();
        var tablename = bcode.encode(table);

        var reportname = encodeURIComponent("工程开工备案v1");;

        var workurl = "/ReportPrint/Index?type=word&opentype=pdf&table=" + tablename + "&where=jdzch%3d%27" + jdzch + "%27&filename=" + reportname;       

        var title = selected.GCMC;
        gotoStarkWork(workurl, title);


    } catch (e) {
        alert(e);
    }
}
function gotoStarkWork(workurl, title) {
    try {
        var rdm = Math.random();
        var url = workurl;

        parent.layer.open({
            type: 2,
            title: title,
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: url,
            end: function () {
            }
        });
    }
    catch (ex) {
        alert(ex);
    }
}


function setPos() {
    try{
        var selected = pubselect();
        if (selected == undefined)
            return;

        var jdzch = encodeURIComponent(selected.JDZCH)   // 键值
        var gcmc = selected.GCMC   // 键值
        var pos = encodeURIComponent(selected.GCZB);
        var layerObj = undefined;
        parent.layer.open({
            type: 2,
            title: '工程标注-' + gcmc,
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: "/jc/map?title=" + encodeURIComponent(gcmc)+"&pos="+pos,
            btn: ["保存", "关闭"],
            yes: function (index) {
                var pos = window.parent[layerObj.find('iframe')[0]['name']].getPos();
                if (pos == "") {
                    return;
                }
                $.ajax({
                    type: "POST",
                    url: "/jc/setgcbz?jdzch=" + jdzch + "&pos=" + encodeURIComponent(pos),
                    dataType: "json",
                    async: false,
                    success: function (data) {
                        if (data.code != 0)
                            alert("标注失败，详细信息：" + data.msg);
                        else {
                            alert("标注成功");
                            parent.layer.closeAll();
                        }

                    },
                    complete: function (XMLHttpRequest, textStatus) {
                    },
                    beforeSend: function (XMLHttpRequest) {
                    }
                });
            },
            success: function (layero, index) {
                layerObj = layero;
            },
            btn2: function (index) {
                parent.layer.closeAll();
            },
            end:function(){
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}

function FormatWtl(value, row, index) {
    var imgurl = "";
    try {
        var ivalue = value * 1;

        if (value > 0)
            imgurl += "<center><img src='/skins/default/images/list/set_green.png' title='已设置'/></center>";
        else
            imgurl += "<center><img src='/skins/default/images/list/set_gray.png' title='未设置'/></center>";
    } catch (e) {
        alert(e);
    }
    imgurl += "";
    return imgurl;
}

function FormatWcwtl(value, row, index) {
    var imgurl = "";
    try {
        var ivalue = value * 1;
        var szsl = row.WTL * 1;

        if (value == 0) {
            if (szsl > 0)
                imgurl += "<center><img src='/skins/default/images/list/set_green.png' title='已完成'/></center>";
            else
                imgurl += "<center><img src='/skins/default/images/list/set_gray.png' title='未设置'/></center>";
        }
        else
            imgurl += "<center><img src='/skins/default/images/list/set_red.png' title='未完成'/></center>";
    } catch (e) {
        alert(e);
    }
    imgurl += "";
    return imgurl;
}

function IsBjwc(zt) {
    return zt != "" && zt != "LR" && zt != 'YT';
}
function FormatBjwc(value, row, index) {
    var imgurl = "";
    try {
        var bjwc = IsBjwc(value);

        if (bjwc)
            imgurl += "<center><img src='/skins/default/images/list/set_green.png' title='已完成'/></center>";
        else
            imgurl += "<center><img src='/skins/default/images/list/set_gray.png' title='未完成'/></center>";
    } catch (e) {
        alert(e);
    }
    imgurl += "";
    return imgurl;
}
function FormatGczb(value, row, index) {
    var imgurl = "";
    try {


        if (value != "")
            imgurl += "<center><img src='/skins/default/images/list/set_green.png' title='已标注'/></center>";
        else
            imgurl += "<center><img src='/skins/default/images/list/set_gray.png' title='未标注'/></center>";
    } catch (e) {
        alert(e);
    }
    imgurl += "";
    return imgurl;
}
/**
*
*  Base64 encode / decode
*
*  @author haitao.tu
*  @date   2010-04-26
*  @email  tuhaitao@foxmail.com
*
*/

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