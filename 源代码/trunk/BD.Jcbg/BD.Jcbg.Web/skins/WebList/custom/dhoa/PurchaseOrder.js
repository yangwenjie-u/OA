

//采购管理
//添加采购申请单
function AddPurchaseRequisition() {
    try {
        var workurl = "/workflow/startwork?processid=43";
        var gcbh = "";
        var bcode = new Base64();
        var title = "采购申请";
        var extrainfo1 = bcode.encode("OA_PurchaseOrder|" + bcode.encode("recid=''"));
        var extrainfo2 = bcode.encode('[' + "商品采购" + ']');
        var extrainfo3 = bcode.encode(gcbh);

        gotoStarkWork(workurl, title, extrainfo1, extrainfo2, extrainfo3, "", "");
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
//添加商品
function ProductAdd() {
    try {
        layer.open({
            type: 2,
            title: '添加商品',
            content: '/dhoa/ProductEdit' ,
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


//入库
function InsertToStorage() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return; 

        if (selected.Status != "5") {
            alert("请选择待入库的记录");
            return;
        }
        layer.open({
            type: 2,
            title: '入库',
            content: '/dhoa/PurchaseOrderEdit?method=storage&recid=' + selected.Recid,
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
        //-1 删除 1 待审核 2 主管驳回 3财务审核 4财务驳回  5待入库 6主管终止 7财务终止 9 已完成
        //valueFixed--,,1|待审核,1,0|主管驳回,2,0|财务审核,3,0|财务驳回,4,0|待入库,5,0|主管终止,6,0|财务终止,7,0|已完成,9,0
        if (value == "1")
            imgurl += "<center>待审核</center>";
        else if (value == "2" || value == "4" )
            imgurl += "<center>驳回</center>";
        else if (value == "3")
            imgurl += "<center>财务审核</center>";
        else if (value == "5")
            imgurl += "<center>待入库</center>";
        else if (value == "6" || value == "7")
            imgurl += "<center>已终止</center>";
        else if (value == "9")
            imgurl += "<center>已完成</center>";

    } catch (e) {
        imgurl = value;
    }
    return imgurl;
}
