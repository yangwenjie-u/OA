function view(){
    var selected = pubselect();
    if (selected == undefined)
        return;
    var guid = selected.RGUID;
	var gcbh=selected.JDZCH;
	$.ajax({
		type: "POST",
		url: "/ZJ_Info/GetGZffRecid",
		data: {
			guid:guid                 
		},
		dataType : 'json',            
		cache: false,
		success: function (data) {
			if (data.code == 0) {
			   var recids=data.msg;
			   layer.open({
				type: 2,
				skin: 'layui-layer-lan',
				title: '标题',
				fix: false,
				shadeClose: true,
				maxmin: true,
				area: ['70%', '70%'],
				content: '/WebList/EasyUiIndex?FormDm=CXTJ_GZFF_DETAIL&FormStatus=1&FormParam=PARAM--'+recids+"|"+gcbh
				//end: function () {           //关闭弹出层触发
				//    location.reload();       //刷新父界面，可改为其他
				//}
				});
			}
		 
		},
		complete: function (XMLHttpRequest, textStatus) {
		 
		},
		beforeSend: function (XMLHttpRequest) {	   
		}
	});
	
    // var url = "/WebList/EasyUiIndex?FormDm=CXTJ_GZFF_DETAIL&FormStatus=0&FormParam=PARAM--" + gcbh + "|" + year + "|" + month;
    // parent.layer.open({
        // type: 2,
        // title: '选择月份',
        // shadeClose: false,
        // shade: 0.8,
        // area: ['500px', '300px'],
        // btn: ["确定", "关闭"],
        // content: url,
        // success: function (layero, index) {       
        // }       
    // });
}

function upload()
{
	var selected = pubselect();
    if (selected == undefined)
        return;
	var ffcg=selected.FFCG;
	if(ffcg=="True")
	{
		alert("不能重复上传");
		return ;
	}
	var rguid=selected.RGUID;
	try{
		var rdm = Math.random();
		var url = "/doexcel/uploadFileGZFF?rguid="+rguid;
		url+="&_="+rdm;	  
		parent.layer.open({
			type: 2,
			title: '上传人员实发工资',
			shadeClose: true,
			shade: 0.8,
			area: ['40%', '50%'],
			content: url,
			end: function () {
				searchRecord();
			}
		});
    } catch (e) {
        alert(e);
    }
}


function viewffjl() //查看发放记录
{
	var selected = pubselect();
    if (selected == undefined)
        return;
	var LSH=selected.LSH;
	var ffcg=selected.FFCG;
	if(ffcg!="True")
	{
		alert("还未上传发放记录，无法查看");
		return ;
	}

	 try{
		var rdm = Math.random();
		var url = "/doexcel/uploadFileGZFF?rguid="+rguid;
		url+="&_="+rdm;	  
		parent.layer.open({
			type: 2,
			title: '上传人员实发工资',
			shadeClose: true,
			shade: 0.8,
			area: ['40%', '50%'],
			content: url,
			end: function () {
				searchRecord();
			}
		});
    } catch (e) {
        alert(e);
    }
}

function exportTXT()
{
	var selected = pubselect();
    if (selected == undefined)
        return;
	var rguid=selected.RGUID;
	try{
		var rdm = Math.random();
		var url = "/doexcel/exportTXTGZFF?rguid="+rguid;
		url+="&_="+rdm;	  
		parent.layer.open({
			type: 2,
			title: '上传人员实发工资',
			shadeClose: true,
			shade: 0.8,
			area: ['40%', '50%'],
			content: url,
			end: function () {
				searchRecord();
			}
		});
    } catch (e) {
        alert(e);
    }
}

function view1(){
    var selected = pubselect();
    if (selected == undefined)
        return;
    var gcbh = selected.GCBH;
    var url = "/kqj/bankmonthpay?gcbh=" + gcbh;
    parent.layer.open({
        type: 2,
        title: '选择月份',
        shadeClose: false,
        shade: 0.8,
        area: ['500px', '300px'],
        btn: ["确定", "关闭"],
        content: url,
        success: function (layero, index) {
            layerObj = layero;
        },
        yes: function (index) {
            var p = window.parent[layerObj.find('iframe')[0]['name']].getParams();
            var year = p.year;
            var month = p.month;
            var url = "/WebList/EasyUiIndex?FormDm=CXTJ_GZFF_DETAIL&FormStatus=0&FormParam=PARAM--" + gcbh + "|" + year + "|" + month;
            parent.layer.closeAll();
            parent.layer.open({
                type: 2,
                title: '工资发放表',
                shadeClose: false,
                shade: 0.8,
                area: ['80%', '80%'],
                content: url,
                end: function () {
                }
            });
        },
        btn2: function (index) {
            parent.layer.closeAll();
        },
        end: function () {
            searchRecord();
        }
    });
}

function FormatSPTG(value, row, index) {
    var imgurl = "<div class=\"jqx-grid-cell-middle-align\" style=\"margin-top: 6px;\">";
    try {
        
        if (row.SPTG == "True")
            imgurl += "<img src='/skins/default/images/list/set_green.png' title='已审批'/>";
        else
            imgurl += "<img src='/skins/default/images/list/set_gray.png' title='未审批'/>";
    } catch (e) {
        alert(e);
    }
    imgurl += "</div>";
    return imgurl;
}

function FormatFFCG(value, row, index) {
    var imgurl = "<div class=\"jqx-grid-cell-middle-align\" style=\"margin-top: 6px;\">";
    try {
        
        if (row.FFCG == "True")
            imgurl += "<img src='/skins/default/images/list/set_green.png' title='已审批'/>";
        else
            imgurl += "<img src='/skins/default/images/list/set_gray.png' title='未审批'/>";
    } catch (e) {
        alert(e);
    }
    imgurl += "</div>";
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