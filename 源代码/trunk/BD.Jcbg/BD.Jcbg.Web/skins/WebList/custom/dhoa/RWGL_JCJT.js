function viewSyxm(){
	
	
	
	
}

function CancelRw(ids){
	try{
		var selecteds = ids;
        if (selecteds == null || selecteds.length == 0) {
            return;
        }
		//if (!confirm("确定要退回" + selecteds.length + "条任务记录吗？")) {
  //          return;
  //      }
		var rwrecid="";
		$.each(selecteds, function (index, e) {
            var curitem = e;
			rwrecid += curitem.rwzbid+',';
        });
        layer.prompt({
            title: "任务退回-退回原因"
        }, function (reason, index) {
            var url = "/jcjt/CancelRw?rwzbrecid=" + encodeURIComponent(rwrecid) + "&reason=" + encodeURIComponent(reason);
            $.ajax({
                type: "POST",
                url: url,
                dataType: "json",
                async: false,
                success: function (data) {
                    layer.close(index);
                    if (data.code == "0") {
                        alert("退回成功。");
                        parent.layer.closeAll();
                        searchRecord();
                    } else {
                        if (data.msg == "")
                            data.msg = "退回失败";
                        alert(data.msg);
                    }
                },
                complete: function (XMLHttpRequest, textStatus) {
                },
                beforeSend: function (XMLHttpRequest) { }
            });
        });
		
	}
	catch(e)
	{
		
	}
}

function viewWtsXq(ids) {
        try {
        var tabledesc = "委托单";
        // var selecteds = pubselects();
        var selecteds = ids;
        if (selecteds == null || selecteds.length == 0) {
            return;
        }
        if (selecteds.length > 1) {
            alert("选择了多份，请选一份");
        }
        var selected = selecteds[0];	
        console.log(selected);					 
        parent.layer.open({
            type: 2,
            title: '查看委托单',
            shadeClose: true,
            shade: 0.8,
            area: ['100%', '100%'],
            content: "/jcjt/viewwts?syxmbh=" + encodeURIComponent(selected.syxmbh) + "&recid=" + encodeURIComponent(selected.byzbrecid) + "&wtsmb=" + encodeURIComponent(selected.syxmbh),
            end: function () {

            }
        });
    } catch (e) {
        alert(e);
    }
}
function viewbgxq(ids){
	    try {
        var tabledesc = "报告单";
        var selecteds = ids;
        if (selecteds == null || selecteds.length == 0) {
            return;
        }
        if (selecteds.length > 1) {
            alert("选择了多份，请选一份");
        }
        var selected = selecteds[0];
		var syxmbh = selected.syxmbh;
		var sy_sjbg =selected.sy_sjbg;
		if(sy_sjbg=="否"){
			alert("未生成报告不能查看报告单");
            return;			
		}
		$.ajax({
			type: "POST",
            url: "/jcjt/GetBgWyh?recid=" + encodeURIComponent(selected.byzbrecid),
			dataType: "json",
			async: false,
			success: function (data) {
				 if (data.code == "0") {
					parent.layer.open({
					type: 2,
					title: '查看报告',
					shadeClose: true,
					shade: 0.8,
					area: ['100%', '100%'],
					content: "/jcjt/Viewbgs?syxmbh=" + encodeURIComponent(syxmbh) + "&recid=" + encodeURIComponent(data.datas[0].recid) + "&bgmb=" + encodeURIComponent(data.datas[0].bgmb),
					end: function () {
					}
					});
				 }
			},
			complete: function (XMLHttpRequest, textStatus) {
				searchRecord();
			},
			beforeSend: function (XMLHttpRequest) {
			}
		});
    } catch (e) {
        alert(e);
    }
	
}



function Scbg(ids)
{
	try{
		var selecteds = ids;
        if (selecteds == null || selecteds.length == 0) {
            return;
        }
		if (!confirm("确定要合成" + selecteds.length + "条任务记录为一份报告吗？")) {
            return;
        }
		var rwrecid="";
		var syxmbh="";
		$.each(selecteds, function (index, e) {
            var curitem = e;
			rwrecid += curitem.rwzbid+',';
			syxmbh=curitem.syxmbh;
        });
		$.ajax({
			type: "POST",
			url: "/jcjt/RWToBG?rwzbrecid=" + encodeURIComponent(rwrecid)+"&syxmbh="+encodeURIComponent(syxmbh),
			dataType: "json",
			async: false,
			success: function (data) {
				if (data.code != "0") {
					alert(data.msg);
				} else {
					if(data.msg!="")
						layer.alert(data.msg);
					else
						layer.alert("成功生成报告");
				}
			},
			complete: function (XMLHttpRequest, textStatus) {
                //  searchRecord();
                app.getAllTabList();
			},
			beforeSend: function (XMLHttpRequest) {
			}
		});
	}
	catch(e)
	{
		alert(e);
	}
}
function searchRecord()
{
	try{
		app.getAllTabList();
	}
	catch(e)
	{}
}

function Scddbg(ids)
{
	try{
		var selecteds = ids;
        if (selecteds == null || selecteds.length == 0) {
            return;
        }
	    if (!confirm("确定要为" + selecteds.length + "条任务记录为生成"+selecteds.length+"报告吗？")) {
            return;
        }
		var rwrecid="";
		var syxmbh="";
		$.each(selecteds, function (index, e) {
            var curitem = e;
			rwrecid += curitem.rwzbid+',';
			syxmbh +=curitem.syxmbh+',';
        });

		$.ajax({
			type: "POST",
			url: "/jcjt/RWToDDBG?rwzbrecid=" + encodeURIComponent(rwrecid)+"&syxmbh="+encodeURIComponent(syxmbh),
			dataType: "json",
			async: false,
			success: function (data) {
				if (data.code != "0") {
					alert(data.msg);
				} else {
					if(data.msg!="")
						layer.alert(data.msg);
					else
						layer.alert("成功生成报告");
				}
			},
			complete: function (XMLHttpRequest, textStatus) {
				 app.getAllTabList();
			},
			beforeSend: function (XMLHttpRequest) {
			}
		});
	}
	catch(e)
	{
		alert(e);
	}
}

function SysjCalculate_BG()
{
	try{
		var selecteds = pubselects();
        if (selecteds == null || selecteds.length == 0) {
            return;
        }
		if (!confirm("确定要合成" + selecteds.length + "条任务记录为一份报告吗？")) {
            return;
        }
		var rwrecid="";
		var syxmbh="";
		$.each(selecteds, function (index, e) {
            var curitem = e;
			rwrecid += curitem.rwzbid+',';
			syxmbh=curitem.SYXMBH;
        });

		$.ajax({
			type: "POST",
			url: "/jcjt/SysjCalculate_BG?rwzbrecids=" + encodeURIComponent(rwrecid)+"&syxmbh="+encodeURIComponent(syxmbh)+"&bgtype=bg",
			dataType: "json",
			async: false,
			success: function (data) {
				if (data.code != "0") {
					alert(data.msg);
				} else {
					if(data.msg!="")
						layer.alert(data.msg);
					else
						layer.alert("成功生成报告");
				}
			},
			complete: function (XMLHttpRequest, textStatus) {
				 searchRecord();
			},
			beforeSend: function (XMLHttpRequest) {
			}
		});
	}
	catch(e)
	{
		alert(e);
	}
}

function FormatZt(value, row, index) {
    var imgurl = "";
    try {
		
		// var td = $(this).closest("td[field]");
		// if (td.length) {
			// var field = td.attr("field");
		// }
		
        if (value == "是")
            imgurl += "<center><img src='/skins/default/images/list/set_green.png' title='是'/></center>";
        else
            imgurl += "<center><img src='/skins/default/images/list/set_gray.png' title='否'/></center>";
    } catch (e) {
        alert(e);
    }
    imgurl += "";
    return imgurl;
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