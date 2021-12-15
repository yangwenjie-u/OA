
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
            content: '/dhoa/MaterialEdit?type=add&dataType=3',
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

function PurchaseRequisitionEdit(obj) {
    try {
        var recid = $(obj).attr("recid");
        layer.open({
            type: 2,
            title: '采购申请',
            content: '/dhoa/MaterialEdit?operType=update&dataType=3&recid=' + recid,
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
