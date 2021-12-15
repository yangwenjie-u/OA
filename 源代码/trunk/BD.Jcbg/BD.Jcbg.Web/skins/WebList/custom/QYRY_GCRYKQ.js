
var t_sfzhm="";
$(function()
{
   t_sfzhm = decodeURIComponent(getQueryStr('sfzhm'));
});

function getQueryStr(str){
    var rs = new RegExp("(^|)"+str+"=([^\&]*)(\&|$)","gi").exec(String(window.document.location.href)), tmp;
    if(tmp=rs){
        return tmp[2];
    }
    return "";
}


function view()
{
	 try {
        var tabledesc = "考勤明细";    
        var selected = pubselect();
        if (selected == undefined)
            return;
		var sfzhm=selected.USERID;
		var jdzch=selected.JDZCH;
		var qybh=selected.QYBH;
        var url="/WebList/EasyUiIndex?FormDm=QYRY_KQJL_GL&FormStatus=0&FormParam=PARAM--"+sfzhm+"|"+qybh+"|"+jdzch;
		
		parent.layer.open({
			type: 2,
			title: '人员考勤查询',
			shadeClose: true,
			shade: 0.8,
			area: ['95%', '95%'],
			content: url,
			end: function () {
				//parent.layer.closeAll();
			}
		});
    } catch (e) {
        alert(e);
    }
}
