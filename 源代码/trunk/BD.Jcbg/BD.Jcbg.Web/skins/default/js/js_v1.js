// JavaScript Document


  

function left_zhu_ci_tiz(odj) {

    $("#left_yemian").css("display", "none");
    $("#" + odj).css("display", "block");
}
function ziye_fanhui(odj) {


    document.getElementById("left_yemian").style.display = "block";
    document.getElementById(odj).style.display = "none";
}

var g_MaxTabSum = 4;
function tubiao_zhu_nei(itemCode, itemName, itemUrl)
{
    var existsIndex = window.parent.TabControlGetIndex(itemCode);
    // 已存在的
    if (existsIndex != "") {
        window.parent.TabControlRemove(existsIndex);
        var nextIndex = window.parent.TabControlNextIndex();
        window.parent.TabControlAppend(nextIndex, itemName, itemUrl, true, itemCode);
    }
        // 不存在的
    else {
        var curCount = window.parent.TabControlCount();
        if (curCount >= g_MaxTabSum)
            window.parent.TabControlRemove(window.parent.TabControlGetMinIndex());
        var nextIndex = window.parent.TabControlNextIndex();
        window.parent.TabControlAppend(nextIndex + "", itemName, itemUrl, undefined, itemCode);
    }
}
 function right_click(odj){
		 
		 if(odj=="renyuan")
		 {
			
			 window.parent.TabControlAppend('2', '人员', 'temp2.html', undefined, 'index');
			 }
		 else if (odj=="xiaoxi")
		 {
			
			  window.parent.TabControlAppend('4', '代办消息', 'temp1.html', undefined, 'index');
			 }
		 
		 
		 }
 function fanhui(groups) {


     document.getElementById("left_150").style.display = "inline-block";
     document.getElementsByClassName("juti_tubiao").item(0).style.display = "none";
     $.each(groups, function (index, group) {
         $("#" + group.MenuCode).css("display", "none");
     });
 }
		
		
  function zhu_ci_tiz(obj) {

      document.getElementById("left_150").style.display = "none";
      document.getElementsByClassName("juti_tubiao").item(0).style.display = "block";
      document.getElementById(obj).style.display = "inline-block";
  }


	
function right_jiantou_click(){
	
	  $("#page1").css("display","none");
	 $("#page2").css("display","block");
	 
	}


function page2_left_jiantou_click(){
	
	 $("#page2").css("display","none");
	 $("#page1").css("display","block");
}