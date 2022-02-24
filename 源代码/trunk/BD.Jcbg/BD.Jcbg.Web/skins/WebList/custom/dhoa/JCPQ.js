
function DispatchAdd() {
    try {
        var workurl = "/workflow/startwork?processid=39";
        var gcbh = "";
        var bcode = new Base64();
        var title = "检测派遣";
        var extrainfo1 = bcode.encode("view_i_m_gc|" + bcode.encode("gcbh=''"));
        var extrainfo2 = bcode.encode('[' + "检测派遣" + ']');
        var extrainfo3 = bcode.encode(gcbh);
       
        gotoStarkWork(workurl, title, extrainfo1, extrainfo2, extrainfo3, "", "");
    } catch (e) {
        alert(e);
    }

}

function ViewDispatchRecord() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;
        var url = "/workflow/processtest?serial=" + selected.SerialNo + "&id=39";
        parent.layer.open({
            type: 2,
            title: "查看检测派遣记录",
            shadeClose: true,
            shade: 0.8,
            area: ['90%', '90%'],
            content: url,
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}


function DispatchClose() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;
        //alert("暂无实现");
        var url = "/workflow/processtest?serial=" + selected.SerialNo + "&id=39";
        parent.layer.open({
            type: 2,
            title: "查看检测派遣记录",
            shadeClose: true,
            shade: 0.8,
            area: ['90%', '90%'],
            content: url,
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}

function FormatOper(value, row, index) {
    var imgurl = "";
    try {
        imgurl += "<a onclick='Edit(" + value + ")' style='cursor:pointer;color:#169BD5;' alt='修改'> 修改 </a>"
            + "<a onclick='UseRecord((\"" + value + "\",\"" + row.SignatureCode + "\",\"" + row.SignatureName + "\")' style='cursor:pointer;color:#169BD5;'> 查看使用记录 </a>";

    } catch (e) {
        alert(e);
    }
    return imgurl;
}


function FormatStatus(value, row, index) {
    var imgurl = "";
    try {
        //-1 删除 1待审批 2 已审批 3已驳回 4已完成

        if (value == "1")
            imgurl += "<center>待审批</center>";
        else if (value == "2")
            imgurl += "<center>已审批</center>";
        else if (value == "3")
            imgurl += "<center>已驳回</center>";
        else if (value == "4")
            imgurl += "<center>已完成</center>";

    } catch (e) {
        imgurl = value;
    }
    return imgurl;
}

