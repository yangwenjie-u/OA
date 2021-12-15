/* 删除字符串空格 */
String.prototype.trim=function(){
        return this.replace(/(^\s*)|(\s*$)/g, "");
}
String.prototype.ltrim=function(){
        return this.replace(/(^\s*)/g,"");
}
String.prototype.rtrim=function(){
        return this.replace(/(\s*$)/g,"");
}
/* 判断字符串特性 */
/* 是否日期 */
String.prototype.IsDate1=function(obj)
{ 
/*
    var reg1 = /^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$/
	if (reg1.test(this))
		return true;
	var d = new Date();
	var strD = d.getYear() + "-" + (d.getMonth() + 1) + "-" + d.getDate();
	event.srcElement.value = strD;
	return false;*/
	var ret = false;
	
	var strOrg = this;
	var arrDate = null;
	var nYear;
	var nMonth;
	var nDay;
	do
	{
		if (strOrg.length == 0)
			break;
		// yyyy-mm-dd or yy-mm-dd
		if (strOrg.indexOf("-") > -1)
		{
			var arrDate = strOrg.split("-");
			if (arrDate.length != 3)
				break;
			if (arrDate[0].length == 2)
			{
				arrDate[0] = "20" + arrDate[0];
			}
			
		}
		// yyyy/mm/dd or yy/mm/dd
		else if (strOrg.indexOf("/") > -1)
		{
			var arrDate = strOrg.split("/");
			if (arrDate.length != 3)
				break;
			if (arrDate[0].length == 2)
			{
				arrDate[0] = "20" + arrDate[0];
			}
		}
		// yyyy\mm\dd or yy\mm\dd
		else if (strOrg.indexOf("\\") > -1)
		{
			var arrDate = strOrg.split("\\");
			if (arrDate.length != 3)
				break;
			if (arrDate[0].length == 2)
			{
				arrDate[0] = "20" + arrDate[0];
			}
		}
		// yyyymmdd or yymmdd
		else if (strOrg.length == 6 || strOrg.length == 8)
		{
			arrDate = new Array();
			if (strOrg.length == 6)
			{
				strOrg = "20" + strOrg;
			}
			arrDate.push(strOrg.substr(0, 4));
			arrDate.push(strOrg.substr(4, 2));
			arrDate.push(strOrg.substr(6, 2));
		}
		else
			break;
			
		if (arrDate == null)
			break;
			
		nYear = arrDate[0]*1;
		nMonth = arrDate[1]*1;
		nDay = arrDate[2]*1;		
		if (isNaN(nYear) || isNaN(nMonth) || isNaN(nDay))
			break;
			
		var a=new Date(nYear, nMonth-1, nDay);
		var y=a.getFullYear();
		var m=a.getMonth()+1;
		var d=a.getDate();
		
		if (!(y==nYear && m==nMonth && d==nDay))
			break;
			
		strOrg = y+"-"+m+"-"+d;
		
		ret = true;
	} while(false);
	
		
	if (ret)
		obj.value = strOrg;
	else
	{
		var d = new Date();
		var strD = d.getYear() + "-" + (d.getMonth() + 1) + "-" + d.getDate();
		obj.value = strD;
	}
		
	return ret;
	/*
	var reg = /^((20|19)?\d{2}-[0-1]?[1-9]-[0-3]?[1-9])|((20|19)?\d{2}[0-1][1-9][0-3][1-9])$/;
	if (!reg.exec(this))
	{
		var d = new Date();
		var strD = d.getYear() + "-" + (d.getMonth() + 1) + "-" + d.getDate();
		obj.value = strD;
		return false;
	}
	return true;*/
}
String.prototype.IsDate=function()
{ 
/*
    var reg1 = /^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$/
	if (reg1.test(this))
		return true;
	var d = new Date();
	var strD = d.getYear() + "-" + (d.getMonth() + 1) + "-" + d.getDate();
	event.srcElement.value = strD;
	return false;*/
	
	var ret = false;
	
	var strOrg = this;
	var arrDate = null;
	var nYear;
	var nMonth;
	var nDay;
	do
	{
		if (strOrg.length == 0)
			break;
		// yyyy-mm-dd or yy-mm-dd
		if (strOrg.indexOf("-") > -1)
		{
			var arrDate = strOrg.split("-");
			if (arrDate.length != 3)
				break;
			if (arrDate[0].length == 2)
			{
				arrDate[0] = "20" + arrDate[0];
			}
			
		}
		// yyyy/mm/dd or yy/mm/dd
		else if (strOrg.indexOf("/") > -1)
		{
			var arrDate = strOrg.split("/");
			if (arrDate.length != 3)
				break;
			if (arrDate[0].length == 2)
			{
				arrDate[0] = "20" + arrDate[0];
			}
		}
		// yyyy\mm\dd or yy\mm\dd
		else if (strOrg.indexOf("\\") > -1)
		{
			var arrDate = strOrg.split("\\");
			if (arrDate.length != 3)
				break;
			if (arrDate[0].length == 2)
			{
				arrDate[0] = "20" + arrDate[0];
			}
		}
		// yyyymmdd or yymmdd
		else if (strOrg.length == 6 || strOrg.length == 8)
		{
			arrDate = new Array();
			if (strOrg.length == 6)
			{
				strOrg = "20" + strOrg;
			}
			arrDate.push(strOrg.substr(0, 4));
			arrDate.push(strOrg.substr(4, 2));
			arrDate.push(strOrg.substr(6, 2));
		}
		else
			break;
			
		if (arrDate == null)
		    break;
		nYear = arrDate[0] * 1;
		nMonth = arrDate[1] * 1;
		nDay = arrDate[2] * 1;
		if (isNaN(nYear) || isNaN(nMonth) || isNaN(nDay))
			break;
		var a=new Date(nYear, nMonth-1, nDay);
		var y=a.getFullYear();
		var m=a.getMonth()+1;
		var d=a.getDate();
		if (!(y==nYear && m==nMonth && d==nDay))
			break;
		strOrg = y+"-"+m+"-"+d;
		
		ret = true;
	} while(false);
	
		
	if (ret)
		event.srcElement.value = strOrg;
	else
	{
		var d = new Date();
		var strD = d.getYear() + "-" + (d.getMonth() + 1) + "-" + d.getDate();
		event.srcElement.value = strD;
	}
		
	return ret;
	/*
	var reg = /^((20|19)?\d{2}-[0-1]?[1-9]-[0-3]?[1-9])|((20|19)?\d{2}\\[0-1]?[1-9]\\[0-3]?[1-9])|((20|19)?\d{2}/[0-1]?[1-9]/[0-3]?[1-9])|((20|19)?\d{2}[0-1][1-9][0-3][1-9])$/;
	if (!reg.exec(this))
	{
		var d = new Date();
		var strD = d.getYear() + "-" + (d.getMonth() + 1) + "-" + d.getDate();
		event.srcElement.value = strD;
		return false;
	}
	return true;*/
}
/* 是否空 */
String.prototype.IsNotNull1=function(obj)
{/*
	var reg = /[^ \t]/;
	if (!reg.exec(this))
	{
		obj.value = "----";
		return false;
	}
	return true;*/

	if (this != "")
		return true;
	obj.value = "----";
	return false;
}
String.prototype.IsNotNull=function()
{
/*	var reg = /[^ \t]/;
	if (!reg.exec(this))
	{
		event.srcElement.value = "----";
		return false;
	}
	return true;
*/
	if (this != "")
		return true;
	event.srcElement.value = "----";
	return false;
}
/* 是否数字 */
String.prototype.IsNumeric1=function(obj)
{
/*
	var reg = /^-?([1-9]\d*\.\d*|0\.\d*[1-9]\d*|0?\.0+|0)$/
	if (reg.test(this))
		return true;
	obj.value = "0";
	return false;*/
	var reg = /^-?(\d+\.?\d*)$/;
	if (!reg.exec(this))
	{
		obj.value = "0";
		return false;
	}
	return true;
	/*
	var ret = false;
	if (!isNaN(parseFloat(this)))
	{
		ret = true;
	}
	if (!ret)
		obj.value = "0";
	return ret;
	*/
}
String.prototype.IsNumeric=function()
{
/*
	var reg = /^-?([1-9]\d*\.\d*|0\.\d*[1-9]\d*|0?\.0+|0)$/
	if (reg.test(this))
		return true;
	obj.value = "0";
	return false;*/
	var reg = /^-?(\d+\.?\d*)$/;
	if (!reg.exec(this))
	{
		event.srcElement.value = "0";
		return false;
	}
	return true;
	/*
	var ret = false;
	if (!isNaN(parseFloat(this)))
	{
		ret = true;
	}
	if (!ret)
		event.srcElement.value = "0";
	return ret;*/
}
/* 是否整数 */
String.prototype.IsInt1=function(obj)
{
/*
	var reg = /^-?[1-9]\d*$/
	if (reg.test(this))
		return true;*/
	var reg = /^-?(\d+)$/;
	if (!reg.exec(this))
	{
		obj.value = "0";
		return false;
	}
	return true;
		/*
	var ret = false;
	if (!isNaN(parseInt(this)))
	{
		ret = true;
	}
	if (!ret)
		obj.value = "0";
	return ret;*/
}
String.prototype.IsInt=function()
{
/*
	var reg = /^-?[1-9]\d*$/
	if (reg.test(this))
		return true;*/
	var reg = /^-?(\d+)$/;
	if (!reg.exec(this))
	{
		event.srcElement.value = "0";
		return false;
	}
	return true;
	/*var ret = false;
	
	if (!isNaN(parseInt(this)))
	{
		ret = true;
	}
	if (!ret)
		event.srcElement.value = "0";
	return ret;*/
}
// 获取子串，根据英文长度，中文算2个
String.prototype.gbtrim = function(len, s) 
{   
    var str = '';   
    var sp  = s || '';   
    var len2 = 0;   
    for (var i=0; i<this.length; i++) 
    {   
        if (this.charCodeAt(i)>127 || this.charCodeAt(i)==94) 
        {   
            len2 += 2;   
        } 
        else 
        {   
            len2 ++;   
        }   
    }   
    if (len2 <= len) 
    {   
        return this;   
    }   
    len2 = 0;   
    len  = (len > sp.length) ? len-sp.length: len;   
    for (var i=0; i<this.length; i++) 
    {   
        if (this.charCodeAt(i)>127 || this.charCodeAt(i)==94) 
        {   
            len2 += 2;   
        } 
        else 
        {   
            len2 ++;   
        }   
        if (len2 > len) 
        {   
            str += sp;   
            break;   
        }   
        str += this.charAt(i);   
    }   
return str;   
}   
/* 设置控件焦点 */
function SetFocus(controlID)
{
    controlID.focus();
}
/* 关闭窗口 */
function CloseWindow()
{
    window.top.close();
}   
/* 刷新内容frame */
function ContentPageReflush()
{
    window.top.frames["frm_content"].location.reload();
}
/* 设置状态栏文字 */
function SetStatusText(strText)
{
    window.status=strText;
}
/* enter键转换成tab键 */
function ChangeEnterToTab()   
{   
    if (event.keyCode==13)
    {
		var obj = document.activeElement;
		var tagName = obj.tagName.toLowerCase();
		if (tagName == "input" && 
			(obj.getAttribute("type").toLowerCase() == "button" || obj.getAttribute("type").toLowerCase() == "submit")) 
		{
		}
		else
		{
			event.keyCode=9;
		}
	}
    else if (event.keyCode == 37)
	{
		var obj = document.activeElement;
		if (obj != null)
		{
			var eleType = obj.type.toLowerCase();			
			var objFocus, objPrev;
			
			for (i=0; i<document.forms[0].elements.length; i++)
			{
				var tmpEle = document.forms[0].elements[i];
				var tmpEleType = tmpEle.type.toLowerCase();
				if (tmpEleType == "hidden")
				{
					continue;
				}
				objPrev = objFocus;
				objFocus = document.forms[0].elements[i];
				if (obj == objFocus)
				{
					if (eleType == "text" || eleType == "textarea")
					{
						var s = document.selection.createRange();
						var rng = objFocus.createTextRange();
						s.setEndPoint("StartToStart", rng);
						if (s.text.length == 0 && objPrev != null)
						{
							objPrev.focus();
							objPrev.select();
						}
					}
					else
					{
						if (objPrev != null && !((eleType == "radio") && objPrev.name == obj.name))
						{
							objPrev.focus();
							//objPrev.select();
						}
					}
					break;
				}
			}
		}
		
	}
	else if (event.keyCode == 39)
	{
		var obj = document.activeElement;
		if (obj != null)
		{
			var eleType = obj.type.toLowerCase();			
			var objFocus, objNext;
			for (i=0; i<document.forms[0].elements.length; i++)
			{
				var tmpEle = document.forms[0].elements[i];
				var tmpEleType = tmpEle.type.toLowerCase();
				if (tmpEleType == "hidden")
				{
					continue;
				}
				objFocus = document.forms[0].elements[i];
				if (i != document.forms[0].elements.length - 1)
					objNext = document.forms[0].elements[i+1]
				if (objFocus != null && obj == objFocus)
				{
					if (eleType == "text" || eleType == "textarea")
					{
						var s = document.selection.createRange();
						var rng = objFocus.createTextRange();
						s.setEndPoint("StartToStart", rng);
						if (objFocus != null && s.text.length == objFocus.value.length)
						{
							//objFocus.focus();
							//objFocus.select();
							event.keyCode = 9;
						}
					}
					else
					{
						if (!((eleType == "radio") && objNext.name == obj.name))
						{
							//objFocus.focus();
							event.keyCode = 9;
						}
					}
					break;
				}
			}
		}
	}
}
/* 所有页面的enter键转换成tab键 !!!!!!!!!!!!*/
/* document.onkeydown =  ChangeEnterToTab; */
/* tab键转换成enter键 */
function ChangeTabToEnter()   
{   
    if (event.keyCode==9)
    {
		event.keyCode=13;
	}
}
/* 弹出对话框 */
function DialogWindow(url, param, width, height)
{
	return window.showModalDialog(url, param, "dialogHeight:"+height+"px;dialogWidth:"+width+"px;center:1;help:0;resizable:1;status:0;scroll:1");
}
/* 弹出HelpLink对话框 */
var g_HelpLinkChanged = false;
function ShowHelpLink(helplink)
{
	g_HelpLinkChanged = false;
	if (event.keyCode == 113)
	{
		srcObj = event.srcElement;
		srcIndex = 0;
		objCol = eval("oo."+event.srcElement.id);
		try
		{
			for (i=0; i<objCol.length; i++)
			{
				if (objCol[i] == event.srcElement)
				{
					srcIndex = i;
					break;
				}
			}
		}
		catch (e)
		{
			srcIndex = 0;
		}
		var paramArray = new Array();
		paramArray = helplink.split('|');
		if (paramArray.length >= 4)
		{
			var idArray = new Array();
			idArray = paramArray[1].split(',');

			var idValueArray = DialogWindow("../busyness/helplink.aspx?HelpLink=" + encodeURIComponent(helplink), "", 800, 500);
			if (idValueArray != null)
			{
				if (idArray.length == idValueArray.length)
				{
					for (i=0; i<idArray.length; i++)
					{
						if (srcIndex == 0)
						{
							obj = document.getElementById(idArray[i]);
							
							if (obj != null)
							{
								obj.value = idValueArray[i];
								if (obj.type.indexOf("select")==0 && !g_HelpLinkChanged)
								{
									sentSelectEvent(obj.id);
									g_HelpLinkChanged = true;
								}
							}
							else
							{
								var e = document.createElement("input");
								e.type="hidden";
								e.id = idArray[i];
								e.name = idArray[i];
								e.value = idValueArray[i];
								document.forms(0).appendChild(e);
							}
						}
						else
						{
							
							var array = document.getElementsByName(idArray[i]);
							if (array.length > srcIndex)
							{
								array[srcIndex].value=idValueArray[i];
							}
							else
							{
								var e = document.createElement("input");
								e.type="hidden";
								e.id = idArray[i];
								e.name = idArray[i];
								e.value = idValueArray[i];
								document.forms(0).appendChild(e);
							}
								
						}
					}
				}
			}
			
		}
		
	}
}
/* 弹出多选HelpLink对话框 */
function ShowHelpLinkMuti(title, content)
{
	if (event.keyCode == 113)
	{
		srcObj = event.srcElement;
				
		var idValue = DialogWindow("../busyness/helplink_muti.aspx?Title="+title+"&Content="+content, "", 600, 300);
		if (idValue != undefined)
			srcObj.value = idValue;
	}
}
/* 帮助页面tr双击事件 */
function HelpLinkTrDblClick(trid)
{
	var objTr =document.getElementById(trid);	
	if (objTr != null)
	{
		var rt = new Array(objTr.cells.length);  
		var outText = "";
		for (i=0; i<objTr.cells.length; i++)
		{
			rt[i] = objTr.cells[i].innerText;
		}
		window.returnValue = rt;
		window.close();		
	}	
}
/* 多选帮助页面提交 */
function HelpLinkMutiSubmit()
{
	var str = "";
	for(i=0; i<document.all.length; i++) 
	{
		var obj = document.all(i);
		if (obj.type == "checkbox" && obj.checked)
		{
			str += obj.value + "、";
		}
	}
	if (str.length > 0)
		str = str.substr(0, str.length - 1);
	window.returnValue = str;
	window.close();		
}
/* 字符串转化成日期 */
function StrToDate(str)
{
	/*
	var s=str.split("-");   
	y = parseInt(s[0]);
	m = parseInt(s[1])-1;
	d = parseInt(s[2]);*/
    var d = new Date(str.replace(/-/g, "/"));
	return d;
}
/* 日期相加 */
function DateAdd(interval,number,date) 
{ 
	/* 
	*--------------- DateAdd(interval,number,date) ----------------- 
	* DateAdd(interval,number,date) 
	* 功能:实现VBScript的DateAdd功能. 
	* 参数:interval,字符串表达式，表示要添加的时间间隔. 
	* 参数:number,数值表达式，表示要添加的时间间隔的个数. 
	* 参数:date,时间对象. 
	* 返回:新的时间对象. 
	* var now = new Date(); 
	* var newDate = DateAdd("d",5,now); 
	* author:wanghr100(灰豆宝宝.net) 
	* update:2004-5-28 11:46 
	*--------------- DateAdd(interval,number,date) ----------------- 
	*/ 
	
	switch(interval) 
	{ 
		case "y" : { 
		date.setFullYear(date.getFullYear()+number); 
		return date; 
		break; 
		} 
		case "M" : { 
		date.setMonth(date.getMonth()+number); 
		return date; 
		break; 
		}
		case "d" : { 
		date.setDate(date.getDate()+number); 
		return date; 
		break; 
		} 
		case "h" : { 
		date.setHours(date.getHours()+number); 
		return date; 
		break; 
		} 
		case "m" : { 
		date.setMinutes(date.getMinutes()+number); 
		return date; 
		break; 
		} 
		case "s" : { 
		date.setSeconds(date.getSeconds()+number); 
		return date; 
		break; 
		} 
		default : { 
		date.setDate(d.getDate()+number); 
		return date; 
		break; 
		} 
	} 
} 

/* 日期相减 */
function DateDiff(interval,date1,date2)
{
	var long = date2.getTime() - date1.getTime(); //相差毫秒
	switch(interval.toLowerCase())
	{
		case "y": return parseInt(date2.getFullYear() - date1.getFullYear());
		case "m": return parseInt((date2.getFullYear() - date1.getFullYear())*12 + (date2.getMonth()-date1.getMonth()));
		case "d": return parseInt(long/1000/60/60/24);
		case "w": return parseInt(long/1000/60/60/24/7);
		case "h": return parseInt(long/1000/60/60);
		case "n": return parseInt(long/1000/60);
		case "s": return parseInt(long/1000);
		case "l": return parseInt(long);
	}
}

/* 生成从表翻页标签 */
function CreateTabCtrl(srcDocument) {
	try {
		var RecordCount = 0, PageSize = 10, PageIndex = 1, ZH = 0;
		var CreateUrl = "", EditUrl = "";
		var CanModify = "False";
		var ImagePath = "";
		var wtdbh = "";

		var obj = srcDocument.getElementById("RecordCount");
		if (obj != null)
			RecordCount = obj.value * 1;

		obj = srcDocument.getElementById("WTDBH");
		if (obj != null)
			wtdbh = obj.value;

		obj = srcDocument.getElementById("PageSize");
		if (obj != null)
			PageSize = obj.value * 1;

		obj = srcDocument.getElementById("PageIndex");
		if (obj != null)
			PageIndex = obj.value * 1;

		obj = srcDocument.getElementById("ZH");
		if (obj != null)
			ZH = obj.value * 1;

		obj = srcDocument.getElementById("CreateUrl");
		if (obj != null) {
			//CreateUrl = obj.value;
			CreateUrl = "/busyness/forminputs.aspx?CSYLB=" + srcDocument.getElementById("CSYLB").value +
			"&WTDBH=" + srcDocument.getElementById("WTDBH").value +
			"&ZH=" + srcDocument.getElementById("ZH").value +
			"&CanModify=" + srcDocument.getElementById("CanModify").value +
			"&Create=True" +
			"&WriteDB=" + srcDocument.getElementById("WriteDB").value +
			"&Copy=False" + 
			"&CopyOther=False";
		}

		obj = srcDocument.getElementById("EditUrl");
		if (obj != null)
			EditUrl = obj.value;

		obj = srcDocument.getElementById("CanModify");
		if (obj != null)
			CanModify = obj.value;

		obj = srcDocument.getElementById("ImagePath");
		if (obj != null)
			ImagePath = obj.value;

		var startIndex = 0, curSum = 0;
		obj = srcDocument.getElementById("tbTabCtrl");
		if (obj != null) {
			while (obj.rows.length > 0)
				obj.deleteRow(0);
			startIndex = (PageIndex - 1) * PageSize;
			if (startIndex >= RecordCount) {
				startIndex = RecordCount - RecordCount % PageSize;
			}

			curSum = (startIndex + PageSize) > RecordCount ? RecordCount - startIndex : PageSize;
			for (i = 0; i < curSum; i++) {
				var vCurZH = parseInt(i + startIndex + 1);
				var urlUpper = EditUrl.toUpperCase();
				var str1 = "&ZH=", str2 = "&";
				var nIndex1 = urlUpper.indexOf(str1) + str1.length;
				var nIndex2 = urlUpper.indexOf(str2, nIndex1);
				EditUrl = EditUrl.substr(0, nIndex1) + vCurZH + EditUrl.substr(nIndex2);
				str1 = "&WTDBH=", str2 = "&";
				nIndex1 = urlUpper.indexOf(str1) + str1.length;
				nIndex2 = urlUpper.indexOf(str2, nIndex1);
				EditUrl = EditUrl.substr(0, nIndex1) + wtdbh + EditUrl.substr(nIndex2);

				var vRow = obj.insertRow(obj.rows.length);
				var vCell1 = vRow.insertCell();
				var vCell2 = vRow.insertCell();
				if (ZH == vCurZH) {
					vCell1.className = "stripe_cb_tab_select";
					vCell2.className = "stripe_cb_tab_select";
				}
				var strTmp1 = "<td width='20px' valign='middle' align='center'><img src=\"" + ImagePath + "\" border=\"0\" alt=\"\" /></td>";
				vCell1.innerHTML = strTmp1;
				var strTmp2 = "<td valign='middle'><a href=\"" + EditUrl + "\" target=\"frm_input_3\" onclick=\"CBTabClick();\">第" + (vCurZH) + "组</a></td>";
				vCell2.innerHTML = strTmp2;

			}

			if (CanModify.toUpperCase() == "TRUE") {
				var vRow = obj.insertRow(obj.rows.length);
				var vCell1 = vRow.insertCell();
				var vCell2 = vRow.insertCell();
				if (ZH == 0) {
					vCell1.className = "stripe_cb_tab_select";
					vCell2.className = "stripe_cb_tab_select";
				}
				var strTmp1 = "<td width='20px' valign='middle' align='center'><img src=\"" + ImagePath + "\" border=\"0\" alt=\"\" /></td>";
				vCell1.innerHTML = strTmp1;
				var strTmp2 = "<td valign='middle'><a href=\"" + CreateUrl + "\" target=\"frm_input_3\" onclick=\"CBTabClick();\">新建试验组</a></td>";
				vCell2.innerHTML = strTmp2;
			}
		}

		obj = srcDocument.getElementById("spPager");
		if (obj != null) {
			if (RecordCount <= PageSize)
				obj.style.display = "none";
			else {
				obj.style.display = "inline";
			}
		}
	} catch (e) {
		alert(e);
	}
}

/* 生成从表标签翻页事件 */
function CBTabPageEvent(dir)
{
	var RecordCount = 0, PageSize = 10, PageIndex = 1;
	var objtmp = document.getElementById("RecordCount");
	if (objtmp != null)
		RecordCount = objtmp.value * 1;
	objtmp = document.getElementById("PageSize");
	if (objtmp != null)
		PageSize = objtmp.value * 1;
	var MaxPage = ((RecordCount-RecordCount%PageSize) / PageSize) + (RecordCount % PageSize > 0 ? 1 : 0);
	
	var obj = document.getElementById("PageIndex");
	if (obj != null)
	{
		PageIndex = obj.value * 1
		// 首页
		if (dir == 1)
		{
			obj.value = "1";
		}
		// 前一页
		else if (dir == 2)
		{
			PageIndex -= 1;
			if (PageIndex < 1)
				PageIndex = 1;
			obj.value = PageIndex
		}
		// 后一页
		else if (dir == 3)
		{
			PageIndex += 1;
			if (PageIndex > MaxPage)
				PageIndex = MaxPage;
			obj.value = PageIndex;
		}
		// 末页
		else if (dir == 4)
		{
			obj.value = MaxPage;
		}
		
		CreateTabCtrl(document);
		
	}
		
}

/* 从表标签选择函数 */
function CBTabClick()
{
	var obj = event.srcElement;
	var objParentTd = obj.parentNode;
	var objParentTr = objParentTd.parentNode;
	var objParentTb = objParentTr.parentNode;
	for (i=0; i<objParentTb.rows.length; i++)
	{
		for (j=0; j<objParentTb.rows(i).cells.length; j++)
		{
			if (objParentTb.rows(i).cells(j).className == "stripe_cb_tab_select")
			{
				objParentTb.rows(i).cells(j).className = "";
			}
			
		}
	}
	for (i=0; i<objParentTr.cells.length; i++)
	{
		objParentTr.cells(i).className  = "stripe_cb_tab_select";
	}
	var vUrl = event.srcElement.href.toUpperCase();
	var str1 = "&ZH=", str2 = "&";
	var nIndex1 = vUrl.indexOf(str1) + str1.length;
	var nIndex2 = vUrl.indexOf(str2, nIndex1);
	var vZH = vUrl.substr(nIndex1, nIndex2-nIndex1);
	obj = document.getElementById("ZH");
	if (obj != null)
	{
		obj.value = vZH;
	}
	
}

/* 从表标签页面刷新 */
/*
function CBTabLoad(strCSYLB, strWTDBH, strZH, bCanModify, bWriteDB, bCopy, bCopyOther)
{
	obj = window.parent.frames["frm_input_2"].document.getElementById("PageIndex");
	pageIndex = "1";
	if (obj != null)
		pageIndex = obj.value;
	strUrl = "../busyness/forminputspage.aspx?CSYLB="+strCSYLB+"&WTDBH="+strWTDBH+"&CanModify="+bCanModify+"&WriteDB="+bWriteDB+"&ZH="+(bCopy=="False" ? strZH : "0")+"&PageIndex="+pageIndex+"&Copy="+bCopy+"&CopyAll="+bCopyOther;		
	window.parent.frames["frm_input_2"].location = strUrl;
	//window.parent.frames["frm_input_2"].reload();
}
*/
/* 判断提交表单是否有效 */
function IsSubmitValid(objForm)
{
	ret = true;
	for (i=0; i<objForm.elements.length; i++) {
		var ctrl = objForm.elements(i);
		try {

			if (ctrl.outerHTML.toLowerCase().indexOf("onblur") > -1) {
				var var1 = ctrl.outerHTML.toLowerCase();
				var var2 = "onblur";
				var index1 = var1.indexOf(var2) + var2.length + 1;

				var1 = ctrl.outerHTML;
				var index2 = var1.indexOf("\'", index1);
				var index3 = var1.indexOf("\'", index2 + 1);
				var varPar1 = var1.substr(index2 + 1, index3 - index2 - 1);
				index1 = index3 + 1;

				index2 = var1.indexOf("\'", index1);
				index3 = var1.indexOf("\'", index2 + 1);
				var varPar2 = var1.substr(index2 + 1, index3 - index2 - 1);

				if (!MyValidate2(ctrl, varPar1, varPar2)) {
					ret = false;
					break;
				}
			}
		}
		catch (err) {
			alert(err+"\r\n"+ctrl.outerHTML);
		}
	}
	return ret;
}
/* 设置<-和->键 */
function DirKeyDown()
{
	//←   37   ↑   38   →   39   ↓   40 
	if (event.keyCode == 37)
	{
		var obj = document.activeElement;
		if (obj != null)
		{
			var eleType = obj.type.toLowerCase();			
			var objFocus, objPrev;
			for (i=0; i<document.forms[0].elements.length; i++)
			{
				var tmpEle = document.forms[0].elements[i];
				var tmpEleType = tmpEle.type.toLowerCase();
				if (tmpEleType == "hidden")
				{
					continue;
				}
				objPrev = objFocus;
				objFocus = document.forms[0].elements[i];
				if (obj == objFocus)
				{
					if (eleType == "text" || eleType == "textarea")
					{
						var s = document.selection.createRange();
						var rng = objFocus.createTextRange();
						s.setEndPoint("StartToStart", rng);
						if (s.text.length == 0 && objPrev != null)
						{
							objPrev.focus();
							objPrev.select();
						}
					}
					else
					{
						if (objPrev != null)
							objPrev.focus();
					}
					break;
				}
			}
		}
		
	}
	else if (event.keyCode == 39)
	{
		var obj = document.activeElement;
		if (obj != null)
		{
			var eleType = obj.type.toLowerCase();			
			var objFocus, objPrev;
			for (i=0; i<document.forms[0].elements.length; i++)
			{
				var tmpEle = document.forms[0].elements[i];
				var tmpEleType = tmpEle.type.toLowerCase();
				if (tmpEleType == "hidden")
				{
					continue;
				}
				objPrev = objFocus;
				objFocus = document.forms[0].elements[i];
				if (objPrev != null && obj == objPrev)
				{
					if (eleType == "text" || eleType == "textarea")
					{
						var s = document.selection.createRange();
						var rng = objPrev.createTextRange();
						s.setEndPoint("StartToStart", rng);
						if (objPrev != null && s.text.length == objPrev.value.length)
						{
							objFocus.focus();
							objFocus.select();
						}
					}
					else
					{
						if (objFocus != null)
							objFocus.focus();
					}
					break;
				}
			}
		}
	}
}
/* 设置从表标签页 */
function SetCBTab()
{
	
}
/* 当前页面在框架top 显示 */
function SetTopFrame()
{
	if (window.top != window)
	{
		window.top.location = window.location;
	}						
}
/* 设置第一个元素为当前焦点 */
function SetFirstFocus(curForm)
{
	for (i=0; i<curForm.elements.length; i++)
	{
		var ctrl = curForm.elements(i);
		var ctrlType = ctrl.type.toLowerCase(); 
		if (ctrlType != "hidden")
		{
			ctrl.focus();
			break;
		}
	}
}
/* 根据控件名称，获取控件值，§αα§分割 */
function GetCtrlValues(srcDocument, ctrlName)
{
	var objArr = srcDocument.getElementsByName(ctrlName);
	var ret = "";
	for (i=0; i<objArr.length; i++)
	{
		var ctrl = objArr[i];
		var ctrlType = ctrl.type.toLowerCase();
		if (ctrlType == "button" ||
			ctrlType == "file" ||
			ctrlType == "image" ||
			ctrlType == "submit" ||
			ctrlType == "reset")
			continue
		if (ctrlType == "checkbox" ||
				ctrlType == "radio")
			if (!ctrl.checked)
				continue
		else if (ctrlType == "password" ||
				ctrlType == "text" ||
				ctrlType == "hidden" ||
				ctrlType == "textarea")
		{
		}
		else if (ctrlType == "select-multiple" ||
				ctrlType == "select-one")
		{
		}
		ret += ctrl.value+"§αα§";
	}
	return ret;
}
/* 重新创建控件，§αα§分割 */
function SetCtrlValues(srcDocument, ctrlName, arrValues)
{
	var objArr = srcDocument.getElementsByName(ctrlName);	
	if (objArr == null || objArr.length == 0)
		return;
	var strSelected = "、、";
	var objDef = document.getElementById("hd_"+ctrlName);
	if (objDef != null)
		strSelected = "、"+objDef.value.toLowerCase()+"、";
	var isCombox = false;
	var ctrlType = objArr[0].type;
	if (ctrlType == null)
	{	
		ctrlType = objArr[0].innerText.toLowerCase();
		if (ctrlType == "c")
		{
			ctrlType = "checkbox";
		}
		else if (ctrlType = "r")
		{
			ctrlType = "radio";
		}
		else 
			ctrlType = "";
	}
	ctrlType = ctrlType.toLowerCase()
	if (ctrlType == "text")
	{
		isCombox = true;
		objArr = srcDocument.getElementsByName("$"+ctrlName+"$");
		ctrlType = objArr[0].type.toLowerCase();
	}
		
	if (ctrlType == "checkbox")
	{
		var objParent = objArr[0].parentNode;
		// 创建新控件
		var strHtml = "";
		if (arrValues != null)
		{
			for (i=0; i<arrValues.length; i++)
			{
				if (arrValues[i].length == 0)
						continue; 
				var arrValueTmp = arrValues[i].split('、');
				if (arrValueTmp != null)
				{
					for (j=0; j<arrValueTmp.length; j++)
					{
						if (arrValueTmp[j].length == 0)
							continue;
						if (strHtml.indexOf("'"+arrValueTmp[j]+"'") > -1)
							continue;
						if (strSelected.indexOf("、"+arrValueTmp[j].toLowerCase()+"、") > -1)
							strHtml += "<input type='checkbox' id='" + ctrlName + "' name='" + ctrlName + "' value='"+arrValueTmp[j]+"' checked>"+arrValueTmp[j];
						else
							strHtml += "<input type='checkbox' id='" + ctrlName + "' name='" + ctrlName + "' value='"+arrValueTmp[j]+"'>"+arrValueTmp[j];
					}
				}
				
			}
		}
		//alert(objParent.innerHTML);
		//alert(strHtml);
		if (strHtml == "")
			strHtml = "<span id='" + ctrlName + "' name='" + ctrlName + "' style='display:none'>C</span>";
		objParent.innerHTML = strHtml;
		
	}
	else if (ctrlType == "radio") {
		var objParent = objArr[0].parentNode;
		// 创建新控件
		var strHtml = "";
		if (arrValues != null)
		{
			for (i=0; i<arrValues.length; i++)
			{
				if (arrValues[i].length == 0)
						continue; 
				var arrValueTmp = arrValues[i].split('、');
				if (arrValueTmp != null)
				{
					for (j=0; j<arrValueTmp.length; j++)
					{
						if (arrValueTmp[j].length == 0)
							continue;
						if (strHtml.indexOf("、"+arrValueTmp[j]+"、") > -1)
							continue;
						if (strSelected.indexOf(","+arrValueTmp[j].toLowerCase()+",") > -1)
							strHtml += "<input type='radio' id='" + ctrlName + "' name='" + ctrlName + "' value='"+arrValueTmp[j]+"' checked>"+arrValueTmp[j];
						else
							strHtml += "<input type='radio' id='" + ctrlName + "' name='" + ctrlName + "' value='"+arrValueTmp[j]+"'>"+arrValueTmp[j];
					}
				}
			}
		}
		if (strHtml == "")
			strHtml = "<span id='" + ctrlName + "' name='" + ctrlName + "' style='display:none'>R</span>";
		objParent.innerHTML = strHtml;
	}
	else if (ctrlType == "select-multiple" ||
				ctrlType == "select-one")
	{
		// 创建新控件
		for (i=0; i<objArr.length; i++)
		{	
			while (objArr[i].options.length > 0)
			{
				//if (objArr[i].options[0].selected)
				//	strSelected = strSelected + objArr[i].options[0].value + ",";
				objArr[i].options.remove(0);
			}
			if (isCombox)
			{
				var option = document.createElement("option");
				option.text = "";
				option.value = "";
				objArr[i].add(option);
			}
			if (arrValues != null)
			{				
				for (j=0; j<arrValues.length; j++)
				{
					if (arrValues[j].length == 0)
						continue; 
					var option = document.createElement("option");
					option.text = arrValues[j];
					option.value = arrValues[j];
					if (strSelected.indexOf("、"+arrValues[j].toLowerCase()+"、") > -1)
						option.selected = true;
					objArr[i].add(option);
				}
					
			}
			
		}
	}
}

/* 查询报告中添加条件 */
function SearchAddCondition(logicalCtrlName, fieldCtrlID, operationCtrlID, valueCtrlID, displayCtrlID)
{
	var strLogical = "";
	var objs = document.getElementsByName(logicalCtrlName);
	for (i=0; i<objs.length; i++)
	{
		if (objs[i].checked)
		{
			strLogical = objs[i].value;
			break;
		}
	}
	if (strLogical == "")
		return;
	
	var strFieldName = "";
	var strFieldName2 = "";
	var obj = document.getElementById(fieldCtrlID);
	if (obj != null && obj.options.length > 0)
	{
		strFieldName = obj.value;
		strFieldName2 = obj.options[obj.selectedIndex].text;
	}
	if (strFieldName == "")
		return;
	
	var strOperation = "";
	var strOperation2 = "";
	obj = document.getElementById(operationCtrlID);
	if (obj != null && obj.options.length > 0)
	{
		strOperation = obj.value;
		strOperation2 = obj.options[obj.selectedIndex].text;
	}
	if (strOperation == "")
		return;
		
	var strFieldValue = "";
	obj = document.getElementById(valueCtrlID);
	if (obj != null)
	{
		strFieldValue = obj.value;
	}
	if (strFieldValue == "")
		return;
			
	var objDisplaySelect = document.getElementById(displayCtrlID);
	if (objDisplaySelect == null)
		return;
		
	var str1 = " " + strLogical + " ";
	if (strOperation != "like")
	{
		if (strOperation.indexOf("1_") == 0)
		{
			str1 += "convert(decimal,"+strFieldName + ")";
			str1 += strOperation.substr(2) + strFieldValue ;
		}
		else
		{			
			str1 += strFieldName;	
			str1 += strOperation.substr(2) +"'"+ strFieldValue+"'";
		}
	}
	else
	{
		str1 += strFieldName + " "
		str1 += strOperation + " '%%"+strFieldValue+"%%'";
	}
	
	var str2 = "";
	if (strLogical == "and")
		str2 += "并且";
	else
		str2 += "或者";
	str2 += " " + strFieldName2 + " " + strOperation2 + " " + strFieldValue;
	
	var oOption = document.createElement("option");
	oOption.text = str2;
	oOption.value = str1;
	objDisplaySelect.add(oOption);
	
}
/* 查询条件中删除条件 */
function SearchRemoveSelCondition(displayCtrlID)
{
	var obj = document.getElementById(displayCtrlID);
	if (obj != null)
	{
		for (i=obj.options.length-1; i>=0; i--)
		{
			if (obj.options[i].selected)
			{
				obj.remove(i);
			}
				
		}
	}
}
/* 查询条件中删除所有条件 */
function SearchRemoveAllCondition(displayCtrlID)
{
	var obj = document.getElementById(displayCtrlID);
	if (obj != null)
	{
		while (obj.options.length > 0)
		{
			obj.remove(0);
		}
	}
}

/* 提交查询 */
function SearchSubmit()
{
	var departmentid = 0;
	var obj = document.getElementById("ctl00_ContentPlaceHolder1_ddlDepartments");
	if (obj == null)
		return;
	departmentid = obj.value;
	
	var recid = 0;
	obj = document.getElementById("ctl00_ContentPlaceHolder1_ddlSchemes");
	if (obj == null)
		return;
	recid = obj.value;
	
	var strSYRQ1 = "";
	obj = document.getElementById("ctl00_ContentPlaceHolder1_tbSYRQ1");
	if (obj != null)
		strSYRQ1 = obj.value;
	var strSYRQ2 = "";
	obj = document.getElementById("ctl00_ContentPlaceHolder1_tbSYRQ2");
	if (obj != null)
		strSYRQ2 = obj.value;
	var strJYRQ1 = "";
	obj = document.getElementById("ctl00_ContentPlaceHolder1_tbJYRQ1");
	if (obj != null)
		strJYRQ1 = obj.value;
	var strJYRQ2 = "";
	obj = document.getElementById("ctl00_ContentPlaceHolder1_tbJYRQ2");
	if (obj != null)
		strJYRQ2 = obj.value;
	var strStatic = "";
	obj = document.getElementById("cbStatic");
	if (obj != null)
		strStatic = obj.checked ? "True" : "False";
	var strSumField = "";
	var strSumText = "";
	obj = document.getElementById("cbSum");
	if (obj != null)
	{
		if (obj.checked)
		{
			obj = document.getElementById("ctl00_ContentPlaceHolder1_ddlFields2");
			if (obj != null)
			{
				for (i=0; i<obj.options.length; i++)
				{
					if (obj.options[i].selected)
					{
						strSumField = obj.options[i].value;
						strSumText = obj.options[i].text;
						break;
					}
						
				}
			}
		}
	}
	
	var strAdd = "";
	obj = document.getElementById("ctl00_ContentPlaceHolder1_lbConditions");
	if (obj != null)
	{
		for (i=0; i<obj.options.length; i++)
		{
			strAdd += obj.options[i].value;
		}
	}
	
	var strWhere = "";
	if (strSYRQ1 != "")
		strWhere += " and TM.SYRQ >= convert(datetime, '" + strSYRQ1 + " 0:0:0') ";
	if (strSYRQ2 != "")
		strWhere += " and TM.SYRQ <= convert(datetime, '" + strSYRQ2 + " 23:59:59') ";
	if (strJYRQ1 != "")
		strWhere += " and TM.JYRQ >= convert(datetime, '" + strJYRQ1 + " 0:0:0') ";
	if (strJYRQ2 != "")
		strWhere += " and TM.JYRQ <= convert(datetime, '" + strJYRQ2 + " 23:59:59') ";
	strWhere += strAdd;
	strWhere = strWhere.replace(/\&/g, '§αα§');
	strWhere = strWhere.replace(/\%/g, '§ββ§');
	obj = document.getElementById("ifrm1");
	if (obj != null)
	{
		var obj2 = document.getElementById("FrmUrl");
		if (obj2 != null)
			obj.src = obj2.value + "?DepartmentID="+departmentid+"&RECID="+recid+"&Static="+strStatic+"&SumField="+strSumField+"&SumText="+strSumText+"&WHERE="+strWhere;
	}
}

/* 方案组提交查询 */
function GroupSearchSubmit()
{
	var departmentid = 0;
	var obj = document.getElementById("ctl00_ContentPlaceHolder1_ddlDepartments");
	if (obj == null)
		return;
	departmentid = obj.value;
	
	var recid = 0;
	var obj = document.getElementById("ctl00_ContentPlaceHolder1_ddlGroupSchemes");
	if (obj == null)
		return;
	recid = obj.value;
	
	var strSYRQ1 = "";
	obj = document.getElementById("ctl00_ContentPlaceHolder1_tbSYRQ1");
	if (obj != null)
		strSYRQ1 = obj.value;
	var strSYRQ2 = "";
	obj = document.getElementById("ctl00_ContentPlaceHolder1_tbSYRQ2");
	if (obj != null)
		strSYRQ2 = obj.value;
	var strJYRQ1 = "";
	obj = document.getElementById("ctl00_ContentPlaceHolder1_tbJYRQ1");
	if (obj != null)
		strJYRQ1 = obj.value;
	var strJYRQ2 = "";
	obj = document.getElementById("ctl00_ContentPlaceHolder1_tbJYRQ2");
	if (obj != null)
		strJYRQ2 = obj.value;
	
	var strAdd = "";
	obj = document.getElementById("ctl00_ContentPlaceHolder1_lbConditions");
	if (obj != null)
	{
		for (i=0; i<obj.options.length; i++)
		{
			strAdd += obj.options[i].value;
		}
	}
	
	var strWhere = "";
	if (strSYRQ1 != "")
		strWhere += " and TM.SYRQ >= convert(datetime, '" + strSYRQ1 + " 0:0:0') ";
	if (strSYRQ2 != "")
		strWhere += " and TM.SYRQ <= convert(datetime, '" + strSYRQ2 + " 23:59:59') ";
	if (strJYRQ1 != "")
		strWhere += " and TM.JYRQ >= convert(datetime, '" + strJYRQ1 + " 0:0:0') ";
	if (strJYRQ2 != "")
		strWhere += " and TM.JYRQ <= convert(datetime, '" + strJYRQ2 + " 23:59:59') ";
	strWhere += strAdd;
	strWhere = strWhere.replace(/\&/g, '§αα§');
	strWhere = strWhere.replace(/\%/g, '§ββ§');
	obj = document.getElementById("ifrm1");
	if (obj != null)
	{
		var obj2 = document.getElementById("FrmUrl");
		if (obj2 != null)
			obj.src = obj2.value + "?DepartmentID="+departmentid+"&RECID="+recid+"&WHERE="+strWhere;
	}
}
/* 显示年，年季，还是年月 */
function ChangeDateType()
{
	var strType = "";
	var obj = document.getElementById("ctl00_ContentPlaceHolder1_ddlType");
	if (obj != null)
		strType = obj.value;
	var objj1 = document.getElementById("ctl00_ContentPlaceHolder1_ddlJ1");
	var objj2 = document.getElementById("ctl00_ContentPlaceHolder1_ddlJ2");
	var objm1 = document.getElementById("ctl00_ContentPlaceHolder1_ddlM1");
	var objm2 = document.getElementById("ctl00_ContentPlaceHolder1_ddlM2");
	
	if (objj1 == null || 
		objj2 == null ||
		objm1 == null ||
		objm2 == null)
	{
		return;
	}
	if (strType == "1")
	{
		objj1.style.display = "none";
		objj2.style.display = "none";
		objm1.style.display = "none";
		objm2.style.display = "none";
	}
	else if (strType == "2")
	{
		objj1.style.display = "inline";
		objj2.style.display = "inline";
		objm1.style.display = "none";
		objm2.style.display = "none";
	}
	else if (strType == "3")
	{
		objj1.style.display = "none";
		objj2.style.display = "none";
		objm1.style.display = "inline";
		objm2.style.display = "inline";
	}
}
/* 根据onchange事件，设置表单初始值 */
function SetCtrlInitValue(objForm)
{
	for (i=0; i<objForm.elements.length; i++)
	{
		var ctrl = objForm.elements(i);
		var ctrlType = ctrl.type.toLowerCase();
		if (ctrl.outerHTML.toLowerCase().indexOf("onchange") > -1 &&
			(ctrlType == "checkbox" || ctrlType == "radio" || ctrlType == "select-multiple" || ctrlType == "select-one"))
		{				
			var var1 = ctrl.outerHTML.toLowerCase();
			var var2 = "onchange";
			var index1 = var1.indexOf(var2) + var2.length + 2;
			var	index2 = var1.indexOf("\"", index1);
			if (index1 > -1 && index2 > -1 && index1 < index2)
			{
				var vfunc = var1.substr(index1, index2-index1-1);
				alert(vfunc);
				if (vfunc.toLowerCase().indexOf("nextsibling") == -1)
					eval(vfunc);
			}
		}
	}
}
/* 表单某个控件提交 */
function PostCtrl(strCtrlName)
{
	__doPostBack(strCtrlName, "");
}
/* 统计方案提交校验 */
function StatDefSubmit()
{
	var obj = document.getElementById("ctl00_ContentPlaceHolder1_tbSchemeName");
	if (obj != null && obj.value.trim() == "")
	{
		alert("请输入方案名称！");
		obj.focus();
		return false;
	}
	
	obj = document.getElementById("ctl00_ContentPlaceHolder1_tbTitle");
	if (obj != null && obj.value.trim() == "")
	{
		alert("请输入报告标题！");
		obj.focus();
		return false;
	}
	return true;
}
/* 检验某些字段是否为空 */
function CheckNull(objIDs, strErrs)
{
	var arrIds = objIDs.split(",");
	var arrErrs = strErrs.split(",");
	for (i=0; i<arrIds.length && i<arrErrs.length; i++)
	{
		var obj = document.getElementById(arrIds[i]);
		if (obj == null)
			continue
		if (obj.value.trim().length == 0)
		{
			obj.focus();
			alert(arrErrs[i]);
			return true;
		}
	}
	return false;
	
}
/* 检验两个字段是否相等 */
function CheckEqual(objID1, objID2, strErr)
{
	var obj1 = document.getElementById(objID1);
	var obj2 = document.getElementById(objID2);
	if (obj1 == null || obj2 == null)
		return false;
	var val1 = obj1.value;
	var val2 = obj2.value;
	if (val1 == val2)
		return true;
	obj1.focus();
	alert(strErr);
	return false;
}

function printsetup()
{  
　　// 打印页面设置  
　　document.getElementById("wb").execwb(8,1);
}  

function printpreview()
{  

　　// 打印页面预览  
　　document.getElementById("wb").execwb(7,1);  
}  

function printit(destDocument)  
{ 
	if (confirm('确定打印吗？')) 
	{  
		//document.getElementById('wb').execwb(6,6)  
		destDocument.execCommand("Print");
	}  
}

function copy()
{
	document.execCommand("SelectAll");
	document.execCommand("Copy");
	document.execCommand("Unselect");
	window.alert("文档已复制到您的剪贴板");
}
// textarea超过字符不输入
function TxtMaxLength(ctrl,nMax)
{
	if (event.keyCode==13)
		event.returnValue=false;
	else if(ctrl.value.replace(/[^\x00-\xff]/g,"**").length>=nMax && 
	 		(event.keyCode>=48 || event.keyCode==32))   
  event.returnValue=false;  
}
// 超过字符自动截断
function SetSubString(ctrl,nMax)
{
	if (ctrl.value.replace(/[^\x00-\xff]/g,"**").length>=nMax)
ctrl.value = ctrl.value.gbtrim(nMax,'');
}

// 表单添加hidden控件
function addHidden(name, value)
{
	var ctl = document.createElement("input");
	ctl.setAttribute("type","hidden");
	ctl.setAttribute("value",value);
	ctl.setAttribute("id",name);
	document.body.appendChild(ctl);
}
function sentSelectEvent(selid){
				var t=document.getElementById(selid)
				if( t.fireEvent) {
				    //IE
				       t.fireEvent("onchange");  
				  } else { 
				    
				    t.onchange();
				  }
			}
// 控件附加事件
var EventUtil  = new Object;    
EventUtil.addEventHandler = function (oTarget, sEventType, fnHandler) {    
    //firefox情况下    
    if (oTarget.addEventListener ) {    
        oTarget.addEventListener(sEventType, fnHandler, true);    
    }    
    //IE下    
    else if (oTarget.attachEvent ) {    
        oTarget.attachEvent("on" + sEventType, fnHandler);    
    }    
    else {    
        oTarget["on" + sEventType] = fnHandler;    
    }    
};    
EventUtil.removeEventHandler = function (oTarget, sEventType, fnHandler) {    
    if(document.removeEventListener){     
        rootElement.removeEventListener(sEventType, fnHandler, true);     
    } else if(document.detachEvent){     
        rootElement.detachEvent('on' + sEventType, fnHandler);     
    } else {    
        rootElement["on" + sEventType] = null;    
    }    
};         
