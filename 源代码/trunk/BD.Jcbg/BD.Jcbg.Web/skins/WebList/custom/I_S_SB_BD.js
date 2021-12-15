function edit() {
    try {
        var selected = pubselect();//dataGrid.jqxGrid('getrowdata', rowindex);
        if (!selected) {
            return;
        }
        if (selected.SFYX != "True") {
            alert("不能修改失效的检定信息");
            return;
        }
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_S_SB_BD"); 			// 表名
        var tablerecid = encodeURIComponent("RECID"); 	// 表主键
        var title = encodeURIComponent("设备检定信息"); 	// 标题
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var jydbh = encodeURIComponent(selected.RECID)   // 键值
        var rdm = Math.random();
        var fieldparam = "";

        var js = encodeURIComponent("deviceService.js");
        var callback = encodeURIComponent("deviceService.setBdxx('$$RECID$$')");

        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&jydbh=" + jydbh +
            "&js=" + js +
            "&rownum=2" +
            "&callback=" + callback+
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '修改设备检定信息',
            shadeClose: true,
            shade: 0.8,
            area: ['95%', '95%'],
            content: url,
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}


function del() {
    try {
        var selected = pubselect();
        if (!selected)
            return;
        if (selected.SFYX != "True") {
            alert("不能修改失效的检定信息");
            return;
        }
        if (!confirm("确定要删除所选的记录吗？")) {
            return;
        }
        $.ajax({
            type: "POST",
            url: "/delete/deletebdxx?id=" + encodeURIComponent(selected.RECID),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.code == "0") {
                    alert("删除成功！");
                    searchRecord();
                } else {
                    if (data.msg == "")
                        data.msg = "删除失败";
                    alert(data.msg);
                }
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


function getDateString(value, row, index) {
    var ret = "";
    try {
        ttDate = value.match(/\d{4}.\d{1,2}.\d{1,2}/mg).toString();
        ttDate = ttDate.replace(/[^0-9]/mg, '-');
        if (ttDate.indexOf("1900") == -1)
            ret = ttDate;
    } catch (e) {
        ret = value;
    }
    return ret;
}
function formatYesNo(value, row, index) {
    var imgurl = "";
    try {
        if (value == "是")
            imgurl += "<center><img src='/skins/default/images/list/set_green.png' title='是'/></center>";
        else
            imgurl += "<center><img src='/skins/default/images/list/set_gray.png' title='否'/></center>";
    } catch (e) {
        imgurl = value;
    }
    return imgurl;
}

function formatFj(value,row, index) {
    var imgurl = "";
    try {
        var imgArr = value.split("|");
        var allimages = "";
        $.each(imgArr, function (j, subItem) {
            if (subItem == "")
                return true;
            var arr = subItem.split(",");
            if (arr.length < 2)
                return true;
            if (allimages != "")
                allimages += "||";
            allimages += "/DataInput/FileService?method=DownloadFile&fileid=" + arr[0];


        });
        $.each(imgArr, function (j, subItem) {
            if (subItem == "")
                return true;
            var arr = subItem.split(",");
            if (arr.length < 2)
                return true;
            var bigUrl = "/DataInput/FileService?method=DownloadFile&fileid=" + arr[0];
            var smallUrl = "/DataInput/FileService?method=DownloadFile&type=small&fileid=" + arr[0];
            
            imgurl += "<img src=\"" + smallUrl + "\" alt=\"" + arr[1] + "\" id='" + arr[0] + "' style='cursor:pointer' onclick='viewFj(\"" + allimages + "\")'/>";
            

        });
    } catch (e) {
        imgurl = value;
    }
    return imgurl;
}

function viewFj(images) {
    parent.layer.open({
        type: 2,
        title: '',
        shadeClose: true,
        shade: 0.8,
        area: ['95%', '95%'],
        content: "/jc/viewimages?images=" + encodeURIComponent(images),
        end: function () {
            //searchRecord();
        }
    });
}