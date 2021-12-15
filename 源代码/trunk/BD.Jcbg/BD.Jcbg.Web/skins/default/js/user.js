function stripHtml(content) {
	try {
		var ret = $("<p>" + content + "</p>").text();
		return ret;
	} catch (e) {
		alert(e);
	}
}

String.prototype.lTrim = function (s) {

	s = (s ? s : "\\s");                            //没有传入参数的，默认去空格

	s = ("(" + s + ")");

	var reg_lTrim = new RegExp("^" + s + "*", "g");     //拼正则

	return this.replace(reg_lTrim, "");

};



String.prototype.rTrim = function (s) {

	s = (s ? s : "\\s");

	s = ("(" + s + ")");

	var reg_rTrim = new RegExp(s + "*$", "g");

	return this.replace(reg_rTrim, "");

};



String.prototype.trim = function (s) {

	s = (s ? s : "\\s");

	s = ("(" + s + ")");

	var reg_trim = new RegExp("(^" + s + "*)|(" + s + "*$)", "g");

	return this.replace(reg_trim, "");

};

// 数字格式化输出,dg-数字，ws-保留位数
function formatDigital(dg,ws) {
	var ret = dg;
	var dval = 1024 * 1024 * 1024;
	var per = "";

	try {
		if (dg / dval > 1) {
			ret = (dg / dval).toFixed(ws);
			per = "G";
		}
		else {
			dval /= 1024;
			if (dg / dval > 1) {
				ret = (dg / dval).toFixed(ws);
				per = 'M';
			}
			else {
				dval /= 1024;
				if (dg / dval > 1) {
					ret = (dg / dval).toFixed(ws);
					per = 'K';
				}
				else {
					ret = dg;
					per = 'B';
				}
			}
		}
		ret = ret.trim('0');
		if (ret.length > 0 && ret[ret.length-1]=='.')
			ret = ret.substr(0,ret.length-1);
		ret += per;
	}
	catch (e) {
		alert(e);
	}
	
	
	return ret;
}
// 文件下载
jQuery.download = function (url, data, method) {
	 // 获取url和data
	 if (url && data) {
		 // data 是 string 或者 array/object
		 data = typeof data == 'string' ? data : jQuery.param(data);
		 // 把参数组装成 form的  input
		 var inputs = '';
		 jQuery.each(data.split('&'), function () {
			 var pair = this.split('=');
			 inputs += '<input type="hidden" name="' + pair[0] + '" value="' + pair[1] + '" />';
		 });
		 // request发送请求
		jQuery('<form action="' + url + '" method="' + (method || 'post') + '">' + inputs + '</form>').appendTo('body').submit().remove();
	 };
};