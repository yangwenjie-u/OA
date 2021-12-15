function showSend() {
    var users = pubselects();

    if (users.length == 0)
        return;

    var phones = "";
    $.each(users, function (idx, obj) {
        var selected = obj;
        var phone = selected["DH"];
        if (phones.indexOf(phone) == -1) {
            if (phones != "")
                phones += ",";
            phones += phone;
        }
    });
    try {
        parent.layer.open({
            type: 2,
            title: '短信内容',
            shadeClose: false,
            shade: 0.5,
            area: ['800px', '400px'],
            content: "/sms/message?phones="+encodeURIComponent(phones)
        });


    } catch (e) {
        alert(e);
    }
}