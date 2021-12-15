function docount() {
    try {
        layer.load();


        $.ajax({
            type: "POST",
            url: "/dwgxwz/dokqcount",
            dataType: "json",
            success: function (data) {
                if (data.code == "0") {
                    alert("统计成功！");
                    searchRecord();
                } else {
                    if (data.msg == "")
                        data.msg = "统计失败";
                    alert(data.msg);
                }
            },
            complete: function (XMLHttpRequest, textStatus) {
                layer.closeAll('loading');
            },
            beforeSend: function (XMLHttpRequest) {
                
            }
        });
    } catch (e) {
        alert(e);
        layer.closeAll('loading');
    }
}

function exportMonth()
{
    parent.layer.open({
        type: 2,
        title: "考勤月报表",
        shadeClose: false,
        shade: 0.8,
        area: ['50%', '50%'],
        content: "/kqj/kqexport",
        end: function () {
        }
    });
}

function modifykq() {
    var selected = pubselect();
    if (selected == undefined)
        return;
    var recid = selected.Recid;
    var usercode = selected.Usercode;
    var s1 = selected.S1;
    var s4 = selected.S4;
    parent.layer.open({
        type: 2,
        title: "修改考勤时间",
        shadeClose: false,
        shade: 0.8,
        area: ['50%', '50%'],
        content: "/kqj/modifykq?recid=" + encodeURIComponent(recid) + "&usercode=" + encodeURIComponent(usercode) + "&s1=" + encodeURIComponent(s1) + "&s4="+encodeURIComponent(s4),
        end: function () {
            searchRecord();
        }
    });
}
