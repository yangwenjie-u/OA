
function fullscreen() {

    //每个图表的宽度
    var w = ($(".panel").width() + 270).toString() + "px";
    //每个图表的高度
    var h = ($(".panel").height() + 50).toString() + "px";


    var index = parent.layer.open({
        type: 2,
        content: '/user/welcome',
        area: [w, h],
        closeBtn: 0
    });

    parent.layer.full(index);

    parent.$(".layui-layer-title").css("height", "0");
}

function unfullscreen() {
    parent.layer.closeAll();
}

function setStatistics() {
    var kgnf = $("#bjnf").val();
    var jgnf = $("#jgnf").val();
    var gczt = $("#gczt").val();
    var gclx = $("#gclx").val();

    ajaxPost("/jdbg/GetGcStatistic1", "kgnf=" + kgnf + "&jgnf=" + jgnf + "&gczt=" + gczt + "&gclx=" + gclx, function (data) {
        try{
            if (data.code == "0") {
                var record = data.record;
                var gcs = 0;
                var zmj = 0;
                var zzj = 0;
                if (record.hasOwnProperty("gcs"))
                    gcs = record.gcs;
                if (record.hasOwnProperty("zmj"))
                    zmj = record.zmj*1;
                if (record.hasOwnProperty("gcs"))
                    zzj = record.zzj*1;
                $("#statistic_gcs").html(gcs);
                $("#statistic_zmj").html(zmj.toLocaleString()+"米<sup>2</sup>");
                $("#statistic_zzj").html(zzj.toLocaleString() + "元");
            }
        } catch (ex) {
            alert(ex);
        }
    });
}
function setMapUrl() {
    var kgnf = $("#bjnf").val();
    var jgnf = $("#jgnf").val();
    var gczt = $("#gczt").val();
    var gclx = $("#gclx").val();
    var url= "/jdbg/mapgc?kgnf=" + kgnf + "&jgnf=" + jgnf + "&gczt=" + gczt + "&gclx=" + gclx;
    $("#xcjcTable").attr("src", url);
}
function setAnnounce() {
    try {
        $.ajax({
            type: "POST",
            url: "/oa/getannounce?page=1&rows=100&hasread=0&key=",
            dataType: "json",
            async: true,
            success: function (data) {
                $.each(data.rows, function (index, row) {
                    $("#table_announce").append("<tr><td onclick='showAnnounce(\"" + row.recid + "\")' style='cursor:pointer'>" + row.title + "</td></tr>");
                });
            },
            complete: function (XMLHttpRequest, textStatus) {
            },
            beforeSend: function (XMLHttpRequest) {
            }
        });
    } catch (ex) {
        alert(ex);
    }
}
function showAnnounce(recid) {
    parent.layer.open({
        type: 2,
        title: '公告详情',
        shadeClose: true,
        shade: 0.8,
        area: ['98%', '98%'],
        content: "/oa/announceview?read=true&id=" + recid,
        //content: "/jdbg/player",
        end: function () {
        }
    });
}
function showWorkList() {
    $.ajax({
        type: "POST",
        url: "/workflow/getworktodolist?page=1&rows=1000&key=",
        dataType: "json",
        async: true,
        success: function (data) {
            $("#table_task").html("");
            if (data.total > 0) {
                $.each(data.rows, function (i, item) {
                    var extrainfo = "";
                    if (item.ExtraInfo2 != "")
                        extrainfo = "[" + item.ExtraInfo2 + "]";

                    var dt = eval("new " + item.DateAccepted.substr(1, item.DateAccepted.length - 2));
                    var acceptDate = dt.pattern("yyyy-MM-dd HH:mm");

                    $("#table_task").append("<tr><td><a href='javascript:showCheckWorkDialog(" + item.Taskid + ")'>[" + acceptDate + "][来自：" + item.PreUserRealName + "]" + item.ActivityName + extrainfo + "</a></td></tr>");
                });
            }
        },
        complete: function (XMLHttpRequest, textStatus) {
        },
        beforeSend: function (XMLHttpRequest) {
        }
    });
}

function showCheckWorkDialog(taskid) {
    parent.layer.open({
        type: 2,
        title: '',
        shadeClose: true,
        shade: 0.5,
        area: ['95%', '95%'],
        content: "/workflow/checkwork?taskid=" + taskid + "&DlgId=1&_=" + Math.random(),
        end: function () {
        }
    });

}