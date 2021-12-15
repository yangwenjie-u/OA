function inputIris() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;
        var iris = CallCSharpMethodByResult('Iris', '1.0.0.16');
        var ret = eval('(' + iris + ')');
        if (ret.success) {
            var iristext = ret.sIrisLeftBig + ret.sIrisRightBig;

            $.ajax({
                type: "POST",
                url: "/user/saveinneruseriris",
                data: "usercode=" + encodeURIComponent(selected.usercode) + "&iris=" + iristext,
                dataType: "json",
                async: false,
                success: function (data) {
                    try {
                        if (data.code == "0") {
                            searchRecord();
                            alert("注册成功！");
                        }

                        else {
                            if (data.msg != "")
                                alert(data.msg);
                        }
                    } catch (e) {
                        alert(e);
                    }
                },
                complete: function (XMLHttpRequest, textStatus) {
                },
                beforeSend: function (XMLHttpRequest) {
                }
            });
        }
    } catch (ex) {
        alert(ex);
    }

    console.log(iris);
}