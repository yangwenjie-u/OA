function add() {
    try {
        var selectedRows = pubselects();
        if (selectedRows == undefined || selectedRows.length== 0) {
            return;
        }

        var rybhs = "";
        $.each(selectedRows, function (index, selected) {
            rybhs += selected.RYBH + ",";
        })
        $.ajax({
            type: "POST",
            url: "/qy/setrydw?rybh=" + encodeURIComponent(rybhs),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0") {
                    alert("录用成功！");
                    parent.layer.closeAll();
                } else {
                    if (data.msg == "")
                        data.msg = "录用失败";
                    alert(data.msg);
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