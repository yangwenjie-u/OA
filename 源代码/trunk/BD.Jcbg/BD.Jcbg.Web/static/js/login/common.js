function ajaxTpl(url, params, handle) {
    $.ajax({
        url: url,
        type: 'post',
        data: params,
        // dataType: 'json',
        success: function(data) {
            console.log(data);
            if (typeof handle == 'function') {
                handle(data);
            }
        },
        error: function(XMLHttpRequest, textStatus, errorThrown) {
            console.log(XMLHttpRequest);
        },
        complete: function(XMLHttpRequest, textStatus) {

        }
    });
}

function getCookie(key) {
    var arr, reg = new RegExp("(^| )" + key + "=([^;]*)(;|$)");
    if (arr = document.cookie.match(reg))
        return unescape(arr[2]);
    return "";
}

function delCookie(key) {
    var exp = new Date();
    exp.setTime(exp.getTime() - 1);
    var cval = getCookie(key);
    if (cval != null)
        document.cookie = key + "=" + cval + ";expires=" + exp.toGMTString();
}

function setCookie(key, value, time) {
    var date = new Date();
    date.setDate(date.getDate() + time);
    document.cookie = key + "=" + escape(value) +
        ((time == null) ? "" : ";expires=" + date.toGMTString());
}