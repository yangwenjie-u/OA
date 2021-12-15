/* 工作流转过程中用到的函数 */
// 初始化文件上传控件
function initFileCtrl() {
	try {
	    var files = $(":file");
		for (i = 0; i < files.length; i++) {

			var width = 100;
			var fileid = files[i].id;
			var initType = $(files[i]).attr("displaytype");
			if (initType.toLowerCase() != "image")
			    initFileCtrlOne(fileid);
			else
			    initFileCtrlOneImage(fileid);
		}
	}
	catch (err) {
		$.messager.alert('提示', err, 'info');
	}
}
function initFileCtrlOne(fileid) {
    try {
        var width = 100;
        $('#' + fileid).uploadify({
            'formData': {
                'ctrlid': fileid
            },
            'swf': '/skins/default/workflow/images/uploadify.swf',
            'uploader': '/workflow/savefile',
            'fileTypeExts': '*.*',
            'fileSizeLimit': '409600KB',
            'onUploadSuccess': function (file, data, response) {
                var arrret = data.split("|");
                var code = arrret[0];
                var ctrlid = arrret[1];
                var msg = arrret[2];

                if (code == 0) {
                    // 保存到隐藏域
                    var hideFileName = ctrlid.replace("FILEFIELD_", "");
                    var hideValue = $("#" + hideFileName).val();
                    if (hideValue != "")
                        hideValue += "||";
                    hideValue += file.name + "|" + msg;
                    $("#" + hideFileName).val(hideValue);
                    // 显示文件
                    showUpFile(ctrlid, file.name, msg);
                }
                else
                    $.messager.alert('提示', '上传文件' + file.name + '失败，错误信息：' + msg, 'info');
            },
            'onUploadStart': function (event, queueId, fileObj) {
                //$("body").mask("正在准备上传文件...");
            },
            'onUploadComplete': function (file) {
                //$("body").unmask();
            },
            'onFallback': function () {
                $.messager.alert('提示', '当前未安装flash控件，请从百度搜索flash player下载安装', 'info');
            },

            'onUploadError': function (file, errorCode, errorMsg, errorString) {
                $.messager.alert('提示', file.name + '上传失败，' + errorString + '，请稍后再试', 'info');
            },
            'onUploadProgress': function (file, bytesUploaded, bytesTotal, totalBytesUploaded, totalBytesTotal) {
                /*$("body").unmask();
                var percent = totalBytesTotal / totalBytesUploaded * 100;
                percent = percent - percent % 1;

                $("body").mask("文件上传进度："+ percent+'已上传...' );*/
                //$('#progress').html(totalBytesUploaded + ' bytes uploaded of ' + totalBytesTotal + ' bytes.');
            },

            'buttonText': '请选择文件',
            'width': width,
            'height': '25'
        });
    }
    catch (err) {
        $.messager.alert('提示', err, 'info');
    }
}
function initFileCtrlOneImage(fileid) {
    try {
            var width = 100;
            $('#' + fileid).uploadify({
                'formData': {
                    'ctrlid': fileid
                },
                'swf': '/skins/default/workflow/images/uploadify.swf',
                'uploader': '/workflow/savefile',
                'fileTypeExts': '*.jpg;*.png;*.bmp',
                'fileSizeLimit' : '8192KB',
                'onUploadSuccess': function (file, data, response) {
                    var arrret = data.split("|");
                    var code = arrret[0];
                    var ctrlid = arrret[1];
                    var msg = arrret[2];

                    if (code == 0) {
                        // 保存到隐藏域
                        var hideFileName = ctrlid.replace("FILEFIELD_", "");
                        var hideValue = $("#" + hideFileName).val();
                        if (hideValue != "")
                            hideValue += "||";
                        hideValue += file.name + "|" + msg;
                        $("#" + hideFileName).val(hideValue);
                        // 显示文件
                        showUpImage(ctrlid, file.name, msg);
                    }
                    else
                        $.messager.alert('提示', '上传文件' + file.name + '失败，错误信息：' + msg, 'info');
                },
                'onUploadStart': function (event, queueId, fileObj) {
                    //$("body").mask("正在准备上传文件...");
                },
                'onUploadComplete': function (file) {
                    //$("body").unmask();
                },
                'onFallback': function () {
                    $.messager.alert('提示', '当前未安装flash控件，请从百度搜索flash player下载安装', 'info');
                },

                'onUploadError': function (file, errorCode, errorMsg, errorString) {
                    $.messager.alert('提示', file.name + '上传失败，' + errorString + '，请稍后再试', 'info');
                },
                'onUploadProgress': function (file, bytesUploaded, bytesTotal, totalBytesUploaded, totalBytesTotal) {
                    /*$("body").unmask();
					var percent = totalBytesTotal / totalBytesUploaded * 100;
					percent = percent - percent % 1;

					$("body").mask("文件上传进度："+ percent+'已上传...' );*/
                    //$('#progress').html(totalBytesUploaded + ' bytes uploaded of ' + totalBytesTotal + ' bytes.');
                },

                'buttonText': '请选择文件',
                'width': width,
                'height': '25'
            });

    }
    catch (err) {
        $.messager.alert('提示', err, 'info');
    }
}
// 展示上传文件
function showUpFile(filectrlid, fileorgname, fileid) {
	try {
		var divid = filectrlid.replace('FILEFIELD_', 'FILEFIELD_DIV_');
		var hiddenctrlid = filectrlid.replace('FILEFIELD_', '');

		var tail = (new Date()).getTime();
		var content = $("#" + divid).html();

		var newcontent = "<div class='wfa_frame_div' id='file_div_" + fileid + "'><span class='wfa_text'><a href='/workflow/fileview?id=" + fileid + "' target='_blank' id='FILEFIELD_FILE_" + hiddenctrlid + "_" + tail + "' title='" + fileorgname + "'>" + fileorgname.substr(0, 13) + "</a></span>";
		newcontent += "<div class='wfa_image'><a title='删除文件' id='FILEFIELD_FILE_DEL_" + hiddenctrlid + "_" + tail + "' href='javascript:delFile(\"" + hiddenctrlid + "\",\"" + tail + "\",\"" + fileid + "\",\"" + fileorgname + "\")'><img src='/skins/default/workflow/images/wfa_close1.png' name='" + fileid + "_pic' width='18' height='18' id='" + fileid + "_pic' border='0' onmouseover='wfa_swapImage(\"" + fileid + "_pic\",\"\",\"/skins/default/workflow/images/wfa_close2.png\",1)' onmouseout='wfa_swapImgRestore()'  /></a></div>";
		newcontent += "</div>";

		$("#" + divid).html(content + newcontent);
	}
	catch (err) {
		$.messager.alert('提示', err, 'info');
	}
}
// 展示上传文件
function showUpImage(filectrlid, fileorgname, fileid) {
    try {
        var divid = filectrlid.replace('FILEFIELD_', 'FILEFIELD_DIV_');
        var hiddenctrlid = filectrlid.replace('FILEFIELD_', '');

        var tail = (new Date()).getTime();
        var content = $("#" + divid).html();

        var newcontent = "<div class='wfa_frame_div_img' id='file_div_" + fileid + "'><span class='wfa_frame_img_text'><img src='/workflow/p-s" + fileid + ".jpg' border='0' style='cursor:pointer' onclick='showOrgImageLayer(\"" + fileid + "\")' /></span>";
        newcontent += "<div class='wfa_frame_img_image'><a title='删除文件' id='FILEFIELD_FILE_DEL_" + hiddenctrlid + "_" + tail + "' href='javascript:delFile(\"" + hiddenctrlid + "\",\"" + tail + "\",\"" + fileid + "\",\"" + fileorgname + "\")'><img src='/skins/default/workflow/images/wfa_close1.png' name='" + fileid + "_pic' width='18' height='18' id='" + fileid + "_pic' border='0'/></a></div>";
        newcontent += "</div>";

        $("#" + divid).html(content + newcontent);
    }
    catch (err) {
        $.messager.alert('提示', err, 'info');
    }
}
function showOrgImageLayer(fileid) {
    var url = "/workflow/showimage?url="+encodeURIComponent("/workflow/p-b" + fileid + ".jpg");
    window.open(url);
    /*
    layer.open({
        title:"",
        type: 2,
        shadeClose: true,
        shade: 0.5,
        area: ['800px', '90%'], //宽高
        content: "/workflow/showimage?url="+encodeURIComponent("/workflow/p-b" + fileid + ".jpg")
    });*/

}
// 删除上传文件展示的div
function delUpFileDiv(fileid) {
	try {
		$("#file_div_" + fileid).remove();
	}
	catch (err) {
		$.messager.alert('提示', err, 'info');
	}
}
// 校验必输上传的文件
function hasInvalidFileUpload() {
	var ret = true;
	try {
		var list = $('div[id^=FILEFIELD_MUSTIN_]');
		for (i = 0; i < list.length; i++) {
			if (list[i].innerText == "1") {
				var hideId = list[i].id.replace("FILEFIELD_MUSTIN_", "");
				var fileId = list[i].id.replace("FILEFIELD_MUSTIN_", "FILEFIELD_");
				if ($("#" + hideId).val() == "") {
					$.messager.alert('提示', '上传文件不能为空，请选择上传文件', 'info');
					$("#" + fileId).focus();
					return false;
				}
			}
		}
	}
	catch (err) {
		$.messager.alert('提示', err, 'info');
	}
	return ret;
}
// 删除上传的文件
function delFile(ctrlid, tail, fileid, orgname) {
	try {
		$.messager.confirm("文件删除提示", "确定要删除文件：" + orgname + "吗？删除后请点击[保存]或[转交下一步]按钮，否则将会引起数据不一致！", function (b) {
			if (b) {
				$.ajax({
					type: "POST",
					url: "/workflow/deletefile",
					data: "id=" + fileid,
					dataType: "json",
					success: function (data) {
						if (data.code == 1)
							$.messager.alert('提示', '删除文件失败，错误信息：' + data.msg, 'info');
						else {
							var hideid = ctrlid;
							var hidecontent = $("#" + hideid).val().replace(orgname + "|" + fileid, "");
							$("#" + hideid).val(hidecontent);
							delUpFileDiv(fileid);
						}
					},
					complete: function (XMLHttpRequest, textStatus) {
					},
					beforeSend: function (XMLHttpRequest) {
					}
				});
			}
		});
	}
	catch (err) {
		$.messager.alert('提示', err, 'info');
	}
}

// 数组转换成字符串，split为分隔符
function arrayToStr(arr, split) {
	var ret = "";
	try {
		for (var i = 0; i < arr.length; i++) {
			if (ret != "")
				ret += split;
			ret += arr[i];
		}
	}
	catch (err) {
		$.messager.alert('数组转换成字符串异常', err, 'info');
	}
	return ret;
}

// 校验下一步选择
function checkNextStep(shownextstep, ctrlid) {
	var ret = false;
	try {
		if (!shownextstep)
			return true;
		ret = $("#" + ctrlid).combobox("getValue") != "";
		if (!ret) {
			$.messager.alert('提示', '请选择下一个步骤', 'info');
		}
	}
	catch (err) {
		$.messager.alert('校验下一步选择异常', err, 'info');
	}
	return ret;
}

function checkNextUsers(shownextuser, ctrlid) {
	var ret = false;
	try {
		if (!shownextuser)
			return true;
		ret = $("#" + ctrlid).combobox("getValues").length > 0;
		if (!ret) {
			$.messager.alert('提示', '请选择下一个步骤用户', 'info');
		}
	}
	catch (err) {
		$.messager.alert('校验下一步用户用异常', err, 'info');
	}
	return ret;
}
//是否存在指定函数 
function isExitsFunction(funcName) {
	try {
		if (typeof (eval(funcName)) == "function") {
			return true;
		}
	} catch (e) { }
	return false;
}
//是否存在指定变量 
function isExitsVariable(variableName) {
	try {
		if (typeof (variableName) == "undefined") {
			//alert("value is undefined"); 
			return false;
		} else {
			//alert("value is true"); 
			return true;
		}
	} catch (e) { }
	return false;
}
Date.prototype.Format = function (fmt) { //author: meizz   
	var o = {
		"M+": this.getMonth() + 1,                 //月份   
		"d+": this.getDate(),                    //日   
		"h+": this.getHours(),                   //小时   
		"m+": this.getMinutes(),                 //分   
		"s+": this.getSeconds(),                 //秒   
		"q+": Math.floor((this.getMonth() + 3) / 3), //季度   
		"S": this.getMilliseconds()             //毫秒   
	};
	if (/(y+)/.test(fmt))
		fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
	for (var k in o)
		if (new RegExp("(" + k + ")").test(fmt))
			fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
	return fmt;
}

function selectSelfWords(ctrlid) {
    $.ajax({
        type: "POST",
        url: "/workflow/getselfwords",
        dataType: "json",
        success: function (datas) {
            var table = "<table class='table table-striped' width='100%' style='margin:10px 10px 10px 10px' id='table_self_words'>";
            table += "<tr><td colspan='2'><input type='text' id='tmp_word_search'/><input type='button' value='查找' onclick='searchWords()'/></td></tr>"
            $.each(datas, function (index, node) {
                table += "<tr id='tr_self_words_"+node.recid+"'><td width='40px'><input type='checkbox' id='cb_self_words_" + node.recid + "'/></td><td><span id='sp_self_words_" + node.recid + "'>" + node.yjnr + "</span></td></tr>";
            });
            table += "</table>";
            layer.open({
                type: 1,
                title: '选择常用语句',
                shadeClose: true,
                shade: 0.8,
                area: ['500px', '80%'],
                content: table,
                btn: ["确定", "关闭"],
                yes: function (index) {
                    var ctrlValue = "";
                    $('input[id^="cb_self_words_"]:checked').each(function () {
                        var tmpid = $(this).attr("id").replace("cb_self_words_", "sp_self_words_");

                        var tempval = $("#" + tmpid).html();
                        if (tempval != "") {
                            if (ctrlValue != "")
                                ctrlValue += "\n";
                            ctrlValue += tempval;
                        }


                    });
                    var oldVal = $("#" + ctrlid).val();
                    if (oldVal != "" && ctrlValue != "")
                        ctrlValue = "\n" + ctrlValue;
                    $("#" + ctrlid).val(oldVal + ctrlValue);
                    layer.closeAll();
                },
                btn2: function (index) {
                    layer.closeAll();
                }
            });
        },
        complete: function (XMLHttpRequest, textStatus) {
        },
        beforeSend: function (XMLHttpRequest) {
        }
    });
}
function searchWords() {
    var key = $("#tmp_word_search").val();
    $('tr[id^="tr_self_words_"]').each(function () {
        $(this).css("display", "none");
    });
    $('span[id^="sp_self_words_"]').each(function () {
        if ($(this).html().indexOf(key) > -1) {
            var trid = $(this).attr("id").replace("sp_self_words_", "tr_self_words_");
            $("#"+trid).css("display", "");
        }
    });
}
function addToSelfWords(ctrlid) {
    if ($("#" + ctrlid).val() == "") {
        alert("要添加的内容不能为空！");
        return;
    }
    var contents = $("#" + ctrlid).val().split("\n");
    var table = "<table class='table table-striped' width='100%' style='margin:10px 10px 10px 10px'>";
    $.each(contents, function (index, node) {
        table += "<tr><td width='40px'><input type='checkbox' checked id='tmp_cb_" + index + "'/></td><td><input type='text' id='tmp_content_" + index + "' value='" + node + "' size='40'/></td></tr>";
    });
    table += "</table>";
    layer.open({
        type: 1,
        title: '添加常用语句',
        shadeClose: true,
        shade: 0.8,
        area: ['500px', '80%'],
        content: table,
        btn: ["保存", "关闭"],
        yes: function (index) {
            var datas = "";
            $('input[id^="tmp_cb_"]:checked').each(function () {
                var tmpid = $(this).attr("id").replace("tmp_cb_", "tmp_content_");

                var tempval = $("#" + tmpid).val();
                if (tempval != "") {
                    if (datas != "")
                        datas += "&";
                    datas += tmpid + "=" + encodeURIComponent(tempval);
                }

                
            });
            $.ajax({
                type: "POST",
                url: "/workflow/addselfwords",
                data: datas,
                dataType: "json",
                success: function (data) {
                    if (data.code != "0")
                        alert('添加个人语句失败，错误信息：' + data.msg);
                    else {
                        alert("添加个人语句成功");
                        layer.closeAll();
                    }
                },
                complete: function (XMLHttpRequest, textStatus) {
                },
                beforeSend: function (XMLHttpRequest) {
                }
            });

        },
        btn2: function (index) {
            layer.closeAll();
        }
    });
}

function selectSelfWords2(callback) {
    $.ajax({
        type: "POST",
        url: "/workflow/getselfwords",
        dataType: "json",
        success: function (datas) {
            var table = "<table class='table table-striped' width='100%' style='margin:10px 10px 10px 10px' id='table_self_words'>";
            table += "<tr><td colspan='2'><input type='text' id='tmp_word_search'/><input type='button' value='查找' onclick='searchWords()'/></td></tr>"
            $.each(datas, function (index, node) {
                table += "<tr id='tr_self_words_" + node.recid + "'><td width='40px'><input type='checkbox' id='cb_self_words_" + node.recid + "'/></td><td><span id='sp_self_words_" + node.recid + "'>" + node.yjnr + "</span></td></tr>";
            });
            table += "</table>";
            layer.open({
                type: 1,
                title: '选择常用语句',
                shadeClose: true,
                shade: 0.8,
                area: ['500px', '80%'],
                content: table,
                btn: ["确定", "关闭"],
                yes: function (index) {
                    var ctrlValue = [];
                    $('input[id^="cb_self_words_"]:checked').each(function () {
                        var tmpid = $(this).attr("id").replace("cb_self_words_", "sp_self_words_");

                        var tempval = $("#" + tmpid).html();
                        if (tempval != "") {
                            ctrlValue.push(tempval);
                        }


                    });
                    callback(ctrlValue);
                    layer.closeAll();
                },
                btn2: function (index) {
                    layer.closeAll();
                }
            });
        },
        complete: function (XMLHttpRequest, textStatus) {
        },
        beforeSend: function (XMLHttpRequest) {
        }
    });
}
// 数组去重复 
Array.prototype.unique = function () {
    var newArr = [];
    for (var i = 0; i < this.length; i++) {
        if (newArr.indexOf(this[i]) == -1) {
            newArr.push(this[i]);
        }
    }
    return newArr;
}