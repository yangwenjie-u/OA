/* 流程表单中的基础函数 */
// 获取当前日期
function getCurDate() {
	var ret = "";
	try {
		var dt = new Date();
		ret = dt.getFullYear() + '-' + (dt.getMonth() + 1) + '-' + dt.getDate();
	} catch (e) {
		alert("formbase.js/getCurDate/" + e);
	}
	return ret;
}
// 获取当前时间
function getCurTime() {
	var ret = "";
	try {
		var dt = new Date();
		ret = dt.getHours() + ':' + dt.getMinutes() + ':' + dt.getSeconds();
	} catch (e) {
		alert("formbase.js/getCurTime/" + e);
	}
	return ret;
}