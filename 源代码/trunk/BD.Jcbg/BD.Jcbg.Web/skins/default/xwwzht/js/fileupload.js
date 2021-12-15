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
				'swf': '/skins/default/images/uploadify.swf',
				'uploader': '/oa/savefile',
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

		var newcontent = "<div class='wfa_frame_div' id='file_div_" + fileid + "'><span class='wfa_text'><a href='/oa/fileview?id=" + fileid + "' target='_blank' id='FILEFIELD_FILE_" + hiddenctrlid + "_" + tail + "' title='" + fileorgname + "'>" + fileorgname.substr(0, 13) + "</a></span>";
		newcontent += "<div class='wfa_image'><a title='删除文件' id='FILEFIELD_FILE_DEL_" + hiddenctrlid + "_" + tail + "' href='javascript:delFile(\"" + hiddenctrlid + "\",\"" + tail + "\",\"" + fileid + "\",\"" + fileorgname + "\")'><img src='/skins/default/images/wfa_close1.png' name='" + fileid + "_pic' width='18' height='18' id='" + fileid + "_pic' border='0' onmouseover='wfa_swapImage(\"" + fileid + "_pic\",\"\",\"/skins/default/images/wfa_close2.png\",1)' onmouseout='wfa_swapImgRestore()'  /></a></div>";
		newcontent += "</div>";

		$("#" + divid).html(content + newcontent);
	}
	catch (err) {
		$.messager.alert('提示', err, 'info');
	}
}
// 展示多个上传文件
function showUpFiles(filectrlid, filestr) {
	try {
		if (filestr != "") {
			var arr = filestr.split("||");
			for (var i = 0; i < arr.length; i++) {
				var arritem = arr[i].split('|');
				showUpFile(filectrlid, arritem[0], arritem[1]);
			}
		}
	}
	catch (err) {
		$.messager.alert('提示', err, 'info');
	}
}
// 显示文件
function dispUpFiles(divid, filestr) {
	try {
		if (filestr != "") {
			var ret = "";
			var arr = filestr.split("||");
			for (var i = 0; i < arr.length; i++) {
				var arritem = arr[i].split('|');
				ret += "<a href='/oa/fileview?id=" + arritem[1] + "'>" + arritem[0] + "</a><br>";
			}
			$("#" + divid).html(ret);
		}
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
					url: "/oa/deletefile",
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
function wfa_swapImgRestore() { //v3.0
	var i, x, a = document.MM_sr; for (i = 0; a && i < a.length && (x = a[i]) && x.oSrc; i++) x.src = x.oSrc;
}

function wfa_findObj(n, d) { //v4.01
	var p, i, x; if (!d) d = document; if ((p = n.indexOf("?")) > 0 && parent.frames.length) {
		d = parent.frames[n.substring(p + 1)].document; n = n.substring(0, p);
	}
	if (!(x = d[n]) && d.all) x = d.all[n]; for (i = 0; !x && i < d.forms.length; i++) x = d.forms[i][n];
	for (i = 0; !x && d.layers && i < d.layers.length; i++) x = wfa_findObj(n, d.layers[i].document);
	if (!x && d.getElementById) x = d.getElementById(n); return x;
}

function wfa_swapImage() { //v3.0
	var i, j = 0, x, a = wfa_swapImage.arguments; document.MM_sr = new Array; for (i = 0; i < (a.length - 2); i += 3)
		if ((x = wfa_findObj(a[i])) != null) { document.MM_sr[j++] = x; if (!x.oSrc) x.oSrc = x.src; x.src = a[i + 2]; }
}