
//$(window).load(function () {
//    alert($('button[name=AGREEMENT]').length);
//    $('button[name=AGREEMENT]').css({ "margin-left": "10px" });
//});

function showAgreement() {
    var url = "/user/agreement";
    layer.open({
        type: 2,
        title: '用户注册协议',
        shadeClose: true,
        shade: 0.8,
        area: ['400px', '300px'],
        content: url
    });
}