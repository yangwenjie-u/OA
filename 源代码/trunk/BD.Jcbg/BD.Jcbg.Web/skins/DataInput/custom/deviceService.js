var deviceService = {
    setBdxx: function (recid) {
        
        $.ajax({
            type: "POST",
            url: "/jc/setBdxx" ,
            data:"recid="+ encodeURIComponent(recid),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code != "0") {
                    //alert("提交成功")
                    if (data.msg == "")
                        data.msg = "提交失败";
                    alert(data.msg);
                }
            },
            complete: function (XMLHttpRequest, textStatus) {
            },
            beforeSend: function (XMLHttpRequest) {
            }
        });
    }
};