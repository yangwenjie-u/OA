
(
function($){

$.getUrlParam = function(name)

{
var reg = new RegExp("(^|&)"+ name +"=([^&]*)(&|$)");

var r = window.location.search.substr(1).match(reg);

if (r!=null) return unescape(r[2]); return null;

}
})(jQuery);

function getQueryStr(str){
    var rs = new RegExp("(^|)"+str+"=([^\&]*)(\&|$)","gi").exec(String(window.document.location.href)), tmp;
    if(tmp=rs){
        return tmp[2];
    }
    return "";
}


function view(){
	//var param = $.getUrlParam("FormParam");
	var param = decodeURIComponent(getQueryStr("FormParam"));
	var arr = param.replace("PARAM--","").split('|');
	if (arr.length<5){
		alert("参数不能小于5个");
		return;
	}
	var gcbh = arr[0];
	var year = arr[1];
	var month = arr[2];
	var gzname = arr[3];
	var bzzuserid = arr[4];
	var gw = arr[5];
	var template = "WGRYGL_KQYBB";
	if (gw != "")
		template = "WGRYGL_KQYBB_GW";
	var tablename = encodeURIComponent("I_M_GC|KqjUserMonthPay");
	var where = encodeURIComponent("gcbh='" + gcbh + "'|jdzch='" + gcbh + "' and logyear=" + year + " and logmonth=" + month + " and (gzname='" + gzname + "' or '" + gzname + "'='') and (BzzUserId='" + bzzuserid + "' or '" + bzzuserid + "'='')  and (GW='" + gw + "' or '" + gw + "'='') order by gzname,ryxm");
	var url = "/jdbg/WGRYReportDown?reportfile=" + template + "&tablename=" + tablename + "&type=1&where=" + where;
	parent.layer.open({
	    type: 2,
	    title: '月考勤统计',
	    shadeClose: true,
	    shade: 0.8,
	    area: ['95%', '95%'],
	    content: url,
	    end: function () {
	    }
	});
}

function down(){
	//var param = $.getUrlParam("FormParam");
	var param = decodeURIComponent(getQueryStr("FormParam"));
	var arr = param.replace("PARAM--","").split('|');
	if (arr.length<5){
		alert("参数不能小于5个");
		return;
	}
	var gcbh = arr[0];
	var year = arr[1];
	var month = arr[2];
	var gzname = arr[3];
	var bzzuserid = arr[4];
	var gw = arr[5];
	var template = "WGRYGL_KQYBB";
	if (gw != "")
		template = "WGRYGL_KQYBB_GW";
	var tablename = encodeURIComponent("I_M_GC|KqjUserMonthPay");
	var where = encodeURIComponent("gcbh='" + gcbh + "'|jdzch='" + gcbh + "' and logyear=" + year + " and logmonth=" + month + " and (gzname='" + gzname + "' or '" + gzname + "'='') and (BzzUserId='" + bzzuserid + "' or '" + bzzuserid + "'='') and (GW='" + gw + "' or '" + gw + "'='') order by gzname,ryxm ");
	var url = "/jdbg/WGRYReportDown?reportfile=" + template + "&tablename=" + tablename + "&type=1&where=" + where + "&opentype=filedown";

	downFile(url);
}

function downFile(url) {
    var index = layer.load(1, {
        shade: [0.9, '#fff'] //0.1透明度的白色背景
    });
        
    var xhr = new XMLHttpRequest();
    xhr.open('POST', url, true);        // 也可以使用POST方式，根据接口
    xhr.responseType = "blob";    // 返回类型blob
    // 定义请求完成的处理函数，请求前也可以增加加载框/禁用下载按钮逻辑
    xhr.onload = function () {
        layer.close(index);
        // 请求完成
        if (this.status === 200) {
            // 返回200
            var blob = this.response;
            var reader = new FileReader();
            reader.readAsDataURL(blob);    // 转换为base64，可以直接放入a表情href
            reader.onload = function (e) {
                // 转换完成，创建一个a标签用于下载
                var a = document.createElement('a');
                a.download = '月考勤详情.xlsx';
                a.href = e.target.result;
                $("body").append(a);    // 修复firefox中无法触发click
                a.click();
                $(a).remove();
            }
        }
    };
    // 发送ajax请求
    xhr.send();
}