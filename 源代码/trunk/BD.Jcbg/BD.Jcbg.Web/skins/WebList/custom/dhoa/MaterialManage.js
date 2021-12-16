
//d

//采购管理，耗材管理

//公共功能



//采购管理
//添加采购申请单
function AddPurchaseRequisition() {
    try {
        layer.open({
            type: 2,
            title: '采购申请',
            content: '/dhoa/PurchaseOrderEdit?method=add',
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

function PurchaseRequisitionEdit(recid, status) {
    try {
        if (status != "1") {
            alert("仅可以修改待审核的记录");
            return;
        }
        layer.open({
            type: 2,
            title: '采购申请',
            content: '/dhoa/PurchaseOrderEdit?operType=update&dataType=3&recid=' + recid,
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
function PurchaseOrderDel(recid, status) {
    try {
        if (status != "1") {
            alert("仅可以删除待审核的记录");
            return;
        }
        $.ajax({
            type: "POST",
            url: "/dhoa/PurchaseOrderDelete",
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

//采购管理oper
function FormatOperPurchase(value, row, index) {
    var imgurl = "";
    try {
        imgurl += "<a onclick='PurchaseRequisitionEdit(\"" + value + "\",\"" + row.Status + "\")' style='cursor:pointer;color:#169BD5;' alt='修改'> 修改 </a>"
            + " <a onclick = 'PurchaseOrderDel(\"" + value + "\",\"" + row.Status + "\")' style = 'cursor:pointer;color:#169BD5;' alt = '删除' > 删除 </a > ";

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
