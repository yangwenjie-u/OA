
function CarAdd() {
    try {

        layer.open({
            type: 2,
            title: '新增车辆',
            content: '/dhoa/CarEdit',
            shadeClose: true,
            shade: 0.8,
            area: ['1000px', '350px'],

            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}
function CarEdit(id) {
    try {
        layer.open({
            type: 2,
            title: '修改车辆',
            content: '/dhoa/CarEdit?id=' + id,
            shadeClose: true,
            shade: 0.8,
            area: ['1000px', '350px'],
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}

function CarDelete(id) {
    try {
        alert("暂不支持");
        //$.ajax({
        //    type: "POST",
        //    url: "/dhoa/CarRecordDelete",
        //    data: {
        //        id: id
        //    },
        //    dataType: 'json',
        //    cache: false,
        //    success: function (data) {
        //        if (data.code == 0) {
        //            alert("删除成功");
        //            searchRecord();
        //        }
        //    },
        //    complete: function (XMLHttpRequest, textStatus) {
        //    },
        //    beforeSend: function (XMLHttpRequest) {
        //    }
        //});

    } catch (e) {
        alert(e);
    }
}

//用车记录申请
function CarRecordAdd() {
    var selected = pubselect();
    if (selected == undefined)
        return;
    try {
        if (selected.isUsing == "1") {
            alert("选择的车辆已经被申请，请查看用车记录");
            return;
        }
       
        var workurl = "/workflow/startwork?processid=46";
        var bcode = new Base64();
        var title = "用车申请";
        var extrainfo1 = bcode.encode("OA_CarInfomation|" + bcode.encode("id='" + selected.id+"'"));
        var extrainfo2 = bcode.encode('[' + title + ']');
        var extrainfo3 = bcode.encode(selected.id);
        gotoStarkWork(workurl, title, extrainfo1, extrainfo2, extrainfo3, "", "");

        searchRecord();


        //function gotoStarkWork(workurl, title, extrainfo1, extrainfo2, extrainfo3, extrainfo4, fgcmc) {
        //    try {
        //        var rdm = Math.random();
        //        var url = workurl +
        //            // 加密的条件信息（表单中用到）
        //            "&extrainfo=" + encodeURIComponent(extrainfo1) +
        //            // 流程中显示的主体信息
        //            "&extrainfo2=" + encodeURIComponent(extrainfo2) +
        //            // 流程中用到的跟工程关联的主键
        //            "&extrainfo3=" + encodeURIComponent(extrainfo3) +
        //            // 流程中用到的分工程主键
        //            "&extrainfo4=" + encodeURIComponent(extrainfo4) +
        //            "&fgcmc=" + encodeURIComponent(fgcmc) +
        //            "&callbackjs=" + encodeURIComponent("parent.layer.closeAll();") +
        //            "&_=" + rdm;

        //        parent.layer.open({
        //            type: 2,
        //            title: title,
        //            shadeClose: true,
        //            shade: 0.8,
        //            area: ['95%', '95%'],
        //            content: url,
        //            end: function () {
        //                searchRecord();
        //            }
        //        });

        //    }
        //    catch (ex) {
        //        alert(ex);
        //    }
        //}

        //if (selected.isUsing == "1") {
        //    alert("选择的车辆已经被申请，请查看用车记录");
        //    return;
        //}
        //layer.open({
        //    type: 2,
        //    title: '用车申请',
        //    content: '/dhoa/CarRecordEdit?method=applyfor&mid=' + selected.id,
        //    shadeClose: true,
        //    shade: 0.8,
        //    area: ['1000px', '75%'],

        //    end: function () {
        //        searchRecord();
        //    }
        //});
    } catch (e) {
        alert(e);
    }
}


//查看用车记录
function CarRecordView(mid, carName) {
    console.log(carName);
    var url = "/WebList/ElementIndex?FormDm=CLGL_CLGL&FormStatus=1&FormParam=PARAM--" + encodeURIComponent(mid);
    parent.layer.open({
        type: 2,
        title: carName + "汽车用车记录",
        shadeClose: false,
        shade: 0.8,
        area: ['95%', '95%'],
        content: url,
        end: function () {
        }
    })
}

//用车记录修改
function CarRecordDetail(id, status) {
    try {
        if (status != "1") {
            alert("仅可以修改待审核的记录");
            return;
        }
        layer.open({
            type: 2,
            title: '修改用车申请',
            content: '/dhoa/CarRecordEdit?method=updateRecord&id=' + id,
            shadeClose: true,
            shade: 0.8,
            area: ['1000px', '75%'],
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}

//删除用车记录
function CarRecordDel(id, status) {
    try {
        if (status != "1") {
            alert("仅可以删除待审核的记录");
            return;
        }
        $.ajax({
            type: "POST",
            url: "/dhoa/CarRecordDelete",
            data: {
                id: id
            },
            dataType: 'json',
            cache: false,
            success: function (data) {
                if (data.code == 0) {
                    alert("删除成功");
                    searchRecord();
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

//用车信息完善
function CarRecordEdit(id) {
    //var selected = pubselect();
    //if (selected == undefined)
    //    return;
    try {


        layer.open({
            type: 2,
            title: '用车信息完善',
            content: '/dhoa/CarRecordEdit?method=improveRecord&id=' + id,
            shadeClose: true,
            shade: 0.8,
            area: ['1000px', '75%'],
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}


//用车记录审核
function CarRecordReview(id, status) {
    try {
        alert("功能尚未开通");

    } catch (e) {
        alert(e);
    }
}
//用车记录oper
function FormatOperCarUserRecord(value, row, index) {

    var imgurl = "";
    try {
        imgurl += "<a onclick='CarRecordDetail(" + value + "," + row.Status + ")' style='cursor:pointer;color:red;' alt='修改'> 修改 </a>"
            + "<a onclick='CarRecordDel(" + value + "," + row.Status + ")' style='cursor:pointer;color:red;' alt='删除'> 删除 </a>"
            + "<a onclick='CarRecordEdit(" + value + ")' style='cursor:pointer;color:red;' alt='记录'> 记录 </a>"
            + "<a onclick='CarRecordReview(" + value + ")' style='cursor:pointer;color:red;' alt='审核'> 审核 </a>";

    } catch (e) {
        alert(e);
    }
    return imgurl;
}


//formate用车区域
function FormatCarUserArea(value, row, index) {
    var imgurl = "";
    try {
        if (value == "1")
            imgurl += "<center>室内</center>";
        else
            imgurl += "<center>室外</center>";
    } catch (e) {
        imgurl = value;
    }
    return imgurl;
}

//formate状态
function FormatCarUserStatus(value, row, index) {
    var imgurl = "";
    try {
        //状态（-1：无效数据 ，1：待审核 2 未出发 3外出中 4完成 30 已驳回 40不通过）
        if (value == "1")
            imgurl += "<center>待审核</center>";
        else if (value == "2")
            imgurl += "<center>未出发</center>";
        else if (value == "3")
            imgurl += "<center>外出中</center>";

        else if (value == "4")
            imgurl += "<center>完成</center>";
        else if (value == "30")
            imgurl += "<center>已驳回</center>";
        else if (value == "40")
            imgurl += "<center>不通过</center>";
    } catch (e) {
        imgurl = value;
    }
    return imgurl;
}





//维保申请
function CarMaintenanceAdd() {
    var selected = pubselect();
    if (selected == undefined)
        return;
    try {

        layer.open({
            type: 2,
            title: '维保申请',
            content: '/dhoa/CarMaintenanceEdit?method=add&mid=' + selected.id + "&carid=" + selected.CarID,
            shadeClose: true,
            shade: 0.8,
            area: ['1000px', '350px'],

            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}

//查看维保记录
function CarMaintenanceView(mid, carid) {
    console.log(carid);
    var url = "/WebList/ElementIndex?FormDm=CLGL_CLGL&FormStatus=2&FormParam=PARAM--" + encodeURIComponent(mid);
    parent.layer.open({
        type: 2,
        title: carid + "维保记录",
        shadeClose: false,
        shade: 0.8,
        area: ['95%', '95%'],
        content: url,
        end: function () {
        }
    })
}

//维保状态 OA_CarMaintenance
function FormatCarMaintenanceStatus(value, row, index) {
    var imgurl = "";
    try {
        /*//1待审核 2待维保 3已完成*/
        if (value == "1")
            imgurl += "<center>待审核</center>";
        else if (value == "2")
            imgurl += "<center>待维保</center>";
        else if (value == "3")
            imgurl += "<center>已完成</center>";

    } catch (e) {
        imgurl = value;
    }
    return imgurl;
}

function CarMaintenanceEdit(id) {
    try {
        layer.open({
            type: 2,
            title: '维保申请',
            content: '/dhoa/CarMaintenanceEdit?method=update&id=' + id ,
            shadeClose: true,
            shade: 0.8,
            area: ['1000px', '350px'],

            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }


}

//删除维保记录
function CarMaintenanceDel(id, status) {
    try {
        if (status != "1") {
            alert("仅可以删除待审核的记录");
            return;
        }
        $.ajax({
            type: "POST",
            url: "/dhoa/CarMaintenanceDelete",
            data: {
                id: id
            },
            dataType: 'json',
            cache: false,
            success: function (data) {
                if (data.code == 0) {
                    alert("删除成功");
                    searchRecord();
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

//维保记录 审核
function CarMaintenanceReview(id) {
    try {
            alert("暂不支持");

    } catch (e) {
        alert(e);
    }
}



//维保记录oper
function FormatOperCarMaintenance(value, row, index) {

    var imgurl = "";
    try {
        imgurl += "<a onclick='CarMaintenanceEdit(" + value + "," + row.Status + ")' style='cursor:pointer;color:#169BD5;' alt='修改'> 修改 </a>"
            + "<a onclick='CarMaintenanceDel(" + value + "," + row.Status + ")' style='cursor:pointer;color:#169BD5;' alt='删除'> 删除 </a>"
            + "<a onclick='CarMaintenanceReview(" + value + "," + row.Status + ")' style='cursor:pointer;color:#169BD5;' alt='审核'> 审核 </a>";

    } catch (e) {
        alert(e);
    }
    return imgurl;
}

function FormatOper(value, row, index) {
    var imgurl = "";
    try {
        imgurl += "<a onclick='CarEdit(" + value + ")' style='cursor:pointer;color:#169BD5;' alt='修改'> 修改 </a> "
            + " <a onclick = 'CarDelete(" + value + ")' style = 'cursor:pointer;color:#169BD5;' alt = '删除' > 删除 </a > "
            + " <a onclick = 'CarRecordView(" + value + ",\"" + row.Brand + "\")' style = 'cursor:pointer;color:#169BD5;' alt = '用车记录' > 用车记录 </a > "
            + " <a onclick = 'CarMaintenanceView(" + value + ",\"" + row.CarID + "\")' style = 'cursor:pointer;color:#169BD5;' alt = '维保记录' > 维保记录 </a > ";

    } catch (e) {
        alert(e);
    }
    return imgurl;
}


