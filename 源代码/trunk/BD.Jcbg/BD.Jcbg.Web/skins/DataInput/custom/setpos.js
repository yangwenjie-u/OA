function setPos() {
    try{
        var gcmc = GetCtrlValue("I_M_GC.GCMC");
		if(gcmc=="") gcmc="工程坐标"
        var pos = GetCtrlValue("I_M_GC.GCZB");
        var layerObj = undefined;
        parent.layer.open({
            type: 2,
            title: '工程标注-' + gcmc,
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: "/jc/map?title=" + encodeURIComponent(gcmc)+"&pos="+pos,
            btn: ["保存", "关闭"],
            yes: function (index) {
                var pos = window.parent[layerObj.find('iframe')[0]['name']].getPos();
                if (pos == "") {
                    return;
                }
                SetCtrlValue("I_M_GC.GCZB",pos);
				parent.layer.close(index);
            },
            success: function (layero, index) {
                layerObj = layero;

            },
            btn2: function (index) {
                layer.close(index);
            },
            end:function(){

            }
        });
    } catch (e) {
        alert(e);
    }
}




