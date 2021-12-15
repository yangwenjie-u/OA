// JavaScript Document
//suntaojuan 20160523
//这个js脚本是给通用版网页的~



function test ()
{
	alert(window.location.href);
	}

<!--加入收藏-->

 function AddFavorite(sURL, sTitle) {
 
if (document.all) 
{ 
window.external.addFavorite(sURL,sTitle); 

} 
else if (window.sidebar) 
{ 
window.sidebar.addPanel(sURL,sTitle, ""); 

}
else
{ 
     
	  alert("加入收藏失败，请使用Ctrl+D进行添加,或手动在浏览器里进行设置.");
} 

 
 /*
            sURL = encodeURI(sURL); 
			
        try{   
 
            window.external.addFavorite(sURL, sTitle);   
 
        }catch(e) {   
 
            try{   
 
                window.sidebar.addPanel(sTitle, sURL, "");   
 
            }catch (e) {   
 
                alert("加入收藏失败，请使用Ctrl+D进行添加,或手动在浏览器里进行设置.");
 
            }   
 
        }
 */
    }
 