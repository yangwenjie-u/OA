
//d

//采购管理，耗材管理

//公共功能



//添加
function Add(type) {
    try {
        //type 1: 办公耗材 2 试验耗材
        layer.open({
            type: 2,
            title: '耗材录入',
            content: '/dhoa/MaterialConsumptionEdit?type=' + type,
            shadeClose: true,
            shade: 0.8,
            area: ['990px', '350px'],
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}
//库存清点
function StockCheck(type) {
    try {
        alert("功能开发中");
    } catch (e) {
        alert(e);
    }
}
//器材耗材申请
function Apply(type) {
    try {
        alert("功能开发中");

    } catch (e) {
        alert(e);
    }
}

function MaterialConsumptionEdit(recid, status) {
    try {
        //if (status != "1") {
        //    alert("仅可以修改待审核的记录");
        //    return;
        //}
        layer.open({
            type: 2,
            title: '修改',
            content: '/dhoa/MaterialConsumptionEdit?operType=update&dataType=3&recid=' + recid,
            shadeClose: true,
            shade: 0.8,
            area: ['990px', '350px'],

            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}


//删除采购申请记录
function MaterialConsumptionDel(recid, status) {
    try {
        //if (status != "1") {
        //    alert("仅可以删除待审核的记录");
        //    return;
        //}
        $.ajax({
            type: "POST",
            url: "/dhoa/MaterialConsumptionDelete",
            data: {
                recid: recid
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

//查看记录
function RecordView(mreicd,name) {
    var url = "/WebList/ElementIndex?FormDm=HCRecord&FormStatus=0&FormParam=PARAM--" + encodeURIComponent(mreicd);
    parent.layer.open({
        type: 2,
        title: name + "申领记录",
        shadeClose: false,
        shade: 0.8,
        area: ['95%', '95%'],
        content: url,
        end: function () {
        }
    })
}

//oper
function FormatOper(value, row, index) {
    var imgurl = "";
    try {
        imgurl += "<a onclick='MaterialConsumptionEdit(\"" + value + "\",\"" + row.Status + "\")' style='cursor:pointer;color:#169BD5;' alt='修改'> 修改 </a>"
            + " <a onclick = 'MaterialConsumptionDel(\"" + value + "\",\"" + row.Status + "\")' style = 'cursor:pointer;color:#169BD5;'  > 删除 </a > "
            + " <a onclick = 'RecordView(\"" + value + "\",\"" + row.MaterialName + "\")' style = 'cursor:pointer;color:#169BD5;' > 申领记录 </a > ";

    } catch (e) {
        alert(e);
    }
    return imgurl;
}

function FormatStatus(value, row, index) {
    var imgurl = "";
    try {
        //1 待审核 5待入库 9 已完成
        if (value == "1")
            imgurl += "<center>待审核</center>";
        else if (value == "5")
            imgurl += "<center>待入库</center>";
        else if (value == "9")
            imgurl += "<center>已完成</center>";

    } catch (e) {
        imgurl = value;
    }
    return imgurl;
}
