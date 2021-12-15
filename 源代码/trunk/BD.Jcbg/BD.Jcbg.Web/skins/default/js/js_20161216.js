
$(document).ready(function () {

   /* 
    if (window.screen.width > 1400) {
        //$(this).addClass("center");login
        $(".s_container").addClass("width_1400");
        $(".width_auto").addClass("width_1400");

    }
    else {
        $(".s_container").addClass("width_1240");
        $(".width_auto").addClass("width_1240");
    }  */
  
    $(".s_container").addClass("width_1240");
    $(".width_auto").addClass("width_1240");
 



})



function top_tiaozhun(odj) {
   // alert(odj);
    if (odj == "1") {
        window.location.href = "/user/login"
    
    }
    else if (odj == "2") {
       
        window.location.href = "ruanjianchaoshi.html"
    
    }
    else if (odj == "3") {
        //about_vs
        window.location.href = "about_vs.html"
    }
    else if (odj == "4") {
        window.location.href = "hezuohuoban.html"
    }
    else if (odj == "5") {
        window.location.href = "/user/downs"
    }


}