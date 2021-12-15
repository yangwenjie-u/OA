// 对Date的扩展，将 Date 转化为指定格式的String 
// 月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符， 
// 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字) 
// 例子： 
// (new Date()).Format("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423 
// (new Date()).Format("yyyy-M-d h:m:s.S")      ==> 2006-7-2 8:9:4.18 
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

function DateAddStr(interval, number, dateStr) {
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
    number = number * 1;
    var date = strToDate(dateStr);
    switch (interval) {
        case "y":
            {
                date.setFullYear(date.getFullYear() + number);
                return date.Format("yyyy-MM-dd");
                break;
            }
        case "M": 
            {
                date.setMonth(date.getMonth() + number);
                return date.Format("yyyy-MM-dd");
                break;
            }
        case "d":
            {
                date.setDate(date.getDate() + number); 
                return date.Format("yyyy-MM-dd");
                break;
            }
        case "h": 
            {
                date.setHours(date.getHours() + number);
                return date.Format("yyyy-MM-dd");
                break;
            }
        case "m": 
            {
                date.setMinutes(date.getMinutes() + number);
                return date.Format("yyyy-MM-dd");
                break;
            }
        case "s": 
            {
                date.setSeconds(date.getSeconds() + number);
                return date.Format("yyyy-MM-dd");
                break;
            }
        default: 
            {
                date.setDate(d.getDate() + number);
                return date.Format("yyyy-MM-dd");
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



/* 日期相减 */
function DateDiffStr(interval, dateStr1, dateStr2) {
    var date1 = strToDate(dateStr1);
    var date2 = strToDate(dateStr2);
    var long = date2.getTime() - date1.getTime(); //相差毫秒
    switch (interval.toLowerCase()) {
        case "y": return parseInt(date2.getFullYear() - date1.getFullYear());
        case "m": return parseInt((date2.getFullYear() - date1.getFullYear()) * 12 + (date2.getMonth() - date1.getMonth()));
        case "d": return parseInt(long / 1000 / 60 / 60 / 24);
        case "w": return parseInt(long / 1000 / 60 / 60 / 24 / 7);
        case "h": return parseInt(long / 1000 / 60 / 60);
        case "n": return parseInt(long / 1000 / 60);
        case "s": return parseInt(long / 1000);
        case "l": return parseInt(long);
    }
}

//function strToDate(str) {
//    var val = Date.parse(str);
//    var newDate = new Date(val);
//    return newDate;
//}

/* 字符串转化成日期 */
function strToDate(str) {
    /*
    var s=str.split("-");   
    y = parseInt(s[0]);
    m = parseInt(s[1])-1;
    d = parseInt(s[2]);*/
    var d = new Date(str.replace(/-/g, "/"));
    return d;
}