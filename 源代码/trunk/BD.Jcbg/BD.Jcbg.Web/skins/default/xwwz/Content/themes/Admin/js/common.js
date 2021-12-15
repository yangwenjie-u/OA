$(document).ready(function(){	
						   
						   
	//左侧菜单滑动展开
	$(".parent").click(function(){
												   
$(this).next().slideToggle('fast');
/* if($(this).attr('src') == 'images/arrow_d.png'){ 
        $(this).attr('src', 'images/arrow_u.png'); 

    }else{ 

        $(this).attr('src', 'images/arrow_d.png'); 
    }
*/
})







});