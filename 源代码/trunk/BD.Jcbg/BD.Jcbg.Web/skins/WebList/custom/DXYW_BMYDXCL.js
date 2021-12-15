function completedeal() {
   
    try {
        var selecteds = pubselects();
        if (!selecteds) {
            return;
        }

        if (!confirm("确定处理完成所选的记录吗？")) {
            return;
        }
        var recids = "";
        for (i = 0; i < selecteds.length; i++) {
            recids += selecteds[i].RECID + ",";
        }
 
        
        $.ajax({
            type: "POST",
            url: "/sxga/completeMsgPush?recids=" + encodeURIComponent(recids),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0") {
                    layer.alert("处理成功！", { icon: 6 });
                    searchRecord();
                } else {
                    if (data.msg == "")
                        data.msg = "处理失败";
                    layer.alert(data.msg, { icon: 5 });
                }
            },
            complete: function (XMLHttpRequest, textStatus) {
            },
            beforeSend: function (XMLHttpRequest) {
            }
        });
    } catch (e) {
        alert(e);
    }
}