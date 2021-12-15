var sxtptService = {
    add: function (recid) {
        $.ajax({
            type: "POST",
            url: "/jc/DoAddSxtToPlatform?id=" + encodeURIComponent(recid) ,
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.msg != "") {
                    alert(data.msg);
                } else {
                }
            },
            complete: function (XMLHttpRequest, textStatus) {
            },
            beforeSend: function (XMLHttpRequest) {
            }
        });
    }
}