function check() {
	 var jdzch=GetCtrlValue('I_M_GC_KGBA.JDZCH');
     var layerObj = undefined;

     parent.layer.open({
         type: 2,
         title: '开工备案申请审批',
         shadeClose: true,
         shade: 0.8,
         area: ['350px', '230px'],
         content: "/qy/kgbasp",
         btn: ["保存", "关闭"],
         yes: function (index) {
             var checkvalue = window.parent[layerObj.find('iframe')[0]['name']].getValue();
             var reason = window.parent[layerObj.find('iframe')[0]['name']].getReason();
             $.ajax({
                 type: "POST",
                 url: "/qy/setkgbasp?jdzch=" +encodeURIComponent(jdzch) + "&checkoption=" + checkvalue + "&reason=" + encodeURIComponent(reason),
                 dataType: "json",
                 async: false,
                 success: function (data) {
                     if (data.msg != "")
                         alert(data.msg);
                     parent.layer.closeAll();
                 },
                 complete: function (XMLHttpRequest, textStatus) {
                 },
                 beforeSend: function (XMLHttpRequest) {
                 }
             });
         },
         success: function (layero, index) {
             layerObj = layero;
         },
         btn2: function (index) {
             // parent.layer.closeAll();
             layer.close(index);
         },
         end: function () {
         }
     });
}
