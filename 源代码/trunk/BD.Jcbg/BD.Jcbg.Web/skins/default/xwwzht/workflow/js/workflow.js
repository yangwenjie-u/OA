/* 工作流转过程中用到的函数 */
// 初始化文件上传控件
function initFileCtrl() {
	try {
		var files = $(":file");
		for (i = 0; i < files.length; i++) {
			var width = 100;
			var fileid = files[i].id;
			$('#' + fileid).uploadify({
				'formData': {
					'ctrlid': fileid
				},
				'swf': '/skins/default/workflow/images/uploadify.swf',
				'uploader': '/workflow/savefile',
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
				'onUploadError': function (file, errorCode, errorMsg, errorString) {
					$.messager.alert('提示', file.name + '上传失败，' + errorString + '，请稍后再试', 'info');
				},
				'buttonText': '请选择文件',
				'width': width,
				'height': '25'
			});
		}
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
	try{
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
		ret = $("#"+ctrlid).combobox("getValues").length>0;
		if (!ret) {
			$.messager.alert('提示', '请选择下一个步骤用户', 'info');
		}
	}
	catch (err) {
		$.messager.alert('校验下一步用户用异常', err, 'info');
	}
	return ret;
}
