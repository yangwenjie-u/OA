
//d

//采购管理，耗材管理

//公共功能

var productRecid = '';

//添加消耗的材料数据
function Add(type) {
    try {
        var titleName = "办公耗材";
        //type 1: 办公耗材 2 试验耗材
        if (type == "2") {
            titleName = "试验耗材";
        }
        layer.open({
            type: 2,
            title: titleName + '耗材录入',
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
        var selected = pubselect();
        if (selected == undefined)
            return;

        var titleName = "办公耗材";
        //type 1: 办公耗材 2 试验耗材
        if (type == "2") {
            titleName = "试验耗材";
        }

        var workurl = "/workflow/startwork?processid=44";
        var gcbh = "";
        var bcode = new Base64();
        var title = titleName+"申请";
        var extrainfo1 = bcode.encode("View_OA_MaterialConsume|" + bcode.encode("recid='" + selected.Recid+"'"));
        var extrainfo2 = bcode.encode('[' + title + ']');
        var extrainfo3 = bcode.encode(gcbh);

        gotoStarkWork(workurl, title, extrainfo1, extrainfo2, extrainfo3, "", "");
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
function RecordView(mreicd, name) {
    var url = "/WebList/ElementIndex?FormDm=HCRecord&FormStatus=0&FormParam=PARAM--" + encodeURIComponent(mreicd);
    parent.layer.open({
        type: 2,
        title: name + "申领记录",
        shadeClose: false,
        shade: 0.8,
        area: ['60%', '70%'],
        content: url,
        end: function () {
        }
    })
}


//添加商品
function AddProductInfo() {
    try {
        layer.open({
            type: 2,
            title: '新增',
            content: '/dhoa/ProductEdit',
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

function EditProductInfo() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;

        console.log(selected.Recid);
        layer.open({
            type: 2,
            title: '编辑',
            content: '/dhoa/ProductEdit?recid=' + selected.Recid,
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

//添加产品
function AddProduct() {
    try {
        layer.open({
            type: 2,
            title: "新增产品",
            content: '/dhoa/ProductBaseInfoEdit',
            shadeClose: true,
            shade: 0.8,
            area: ['700px', '200px'],
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }

}

//编辑产品
function UpdateProduct() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;

        layer.open({
            type: 2,
            title: "修改产品",
            content: '/dhoa/ProductBaseInfoEdit?recid=' + selected.Recid,
            shadeClose: true,
            shade: 0.8,
            area: ['990px', '200px'],
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}
//删除产品
function DeleteProduct() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;
        layer.confirm("删除产品时，产品规格也将全部删除，请确认是否删除", { icon: 3, title: '提示' }, function (index) {
            ajaxTpl('/dhoa/ProductBaseInfoDelete', {
                recid: selected.Recid
            }, function (data) {
                if (data.msg) {
                    layer.msg(data.msg);
                } else {
                    layer.msg("删除成功");
                }
                searchRecord();
            });
            layer.close(index);
        });
    } catch (e) {
        alert(e);
    }

}

//查看规格
function SpecView() {
    var selected = pubselect();
    if (selected == undefined)
        return;
    var url = "/WebList/ElementIndex?FormDm=ProductInfo&FormStatus=1&FormParam=PARAM--" + encodeURIComponent(selected.Recid);
    parent.layer.open({
        type: 2,
        title: selected.MaterialName+"规格列表",
        shadeClose: false,
        shade: 0.8,
        area: ['60%', '70%'],
        content: url,
        end: function () {
        }
    })
}

//新增规格
function ProductSpecAdd() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;
        layer.open({
            type: 2,
            title: "新增规格",
            content: '/dhoa/ProductSpecEdit?productRecid=' + selected.Recid,
            shadeClose: true,
            shade: 0.8,
            area: ['700px', '200px'],
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}

function ProductSpecUpdate() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;

        layer.open({
            type: 2,
            title: "编辑规格",
            content: '/dhoa/ProductSpecEdit?specId=' + selected.ID,
            shadeClose: true,
            shade: 0.8,
            area: ['700px', '200px'],
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }

}

function ProductSpecDelete() {
    try {
        var selected = pubselect();
        if (selected == undefined)
            return;
        alert(selected.ID);

        layer.confirm("请确认是否删除", { icon: 3, title: '提示' }, function (index) {
            ajaxTpl('/dhoa/ProductSpecDelete', {
                id: selected.ID
            }, function (data) {
                if (data.msg) {
                    layer.msg(data.msg);
                } else {
                    layer.msg("删除成功");
                }
                searchRecord();
            });
            layer.close(index);
        });
    } catch (e) {
        alert(e);
    }
}

//设置产品状态
function SetProductInfoEnableStatus(dataObj) {
    var dataID = $(dataObj).attr("dataID");
    var flag = $(dataObj).attr('value');
    var str, msg1, msg2;
    if (flag == 1) {
        str = '是否启用?';
    } else {
        str = '是否禁用?';
    }
    layer.confirm(str, { icon: 3, title: '提示' }, function (index) {
        ajaxTpl('/Dhoa/SetProductInfoStatus', {
            recid: dataID,
            status: flag
        }, function (data) {
            layer.msg(data.msg);
            searchRecord();
        });
        layer.close(index);
    });
}
//设置产品状态
function SetProductEnableStatus(dataObj) {
    var dataID = $(dataObj).attr("dataID");
    var flag = $(dataObj).attr('value');
    var str, msg1, msg2;
    if (flag == 1) {
        str = '是否启用?';
    } else {
        str = '是否禁用?';
    }
    layer.confirm(str, { icon: 3, title: '提示' }, function (index) {
        ajaxTpl('/Dhoa/SetProductStatus', {
            recid: dataID,
            status: flag
        }, function (data) {
            layer.msg(data.msg);
            searchRecord();
        });
        layer.close(index);
    });
}
//设置产品状态
function SetProductSpecEnableStatus(dataObj) {
    var dataID = $(dataObj).attr("dataID");
    var flag = $(dataObj).attr('value');
    var str, msg1, msg2;
    if (flag == 1) {
        str = '是否启用?';
    } else {
        str = '是否禁用?';
    }
    layer.confirm(str, { icon: 3, title: '提示' }, function (index) {
        ajaxTpl('/Dhoa/SetProductSpecStatus', {
            id: dataID,
            status: flag
        }, function (data) {
            layer.msg(data.msg);
            searchRecord();
        });
        layer.close(index);
    });
}


//耗材申请记录操作-编辑
function MaterialConsumptionRecordEdit(recid, status) {
    try {
        if (status != "1") {
            alert("仅可以修改待审核的记录");
            return;
        }
        
    } catch (e) {
        alert(e);
    }
}
//耗材申请记录操作-删除
function MaterialConsumptionRecordDel(recid, status) {
    try {
        if (status != "3") {
            alert("仅可以删除已完成的记录");
            return;
        }
      
    } catch (e) {
        alert(e);
    }
}
//耗材申请记录操作-撤销
function ApplyForCancel(recid, status) {
    try {
        //if (status != "1") {
        //    alert("仅待审核的记录可以撤销");
        //    return;
        //}
        layer.confirm("是否撤销此次申请", { icon: 3, title: '提示' }, function (index) {
            ajaxTpl('/Dhoa/MaterialApplyForCancel', {
                recid: recid
            }, function (data) {
                layer.msg(data.msg);
                if (data.code == "0") {
                    searchRecord();
                }
            });
            layer.close(index);
        });
    } catch (e) {
        alert(e);
    }
}


//oper
function FormatOper(value, row, index) {
    var imgurl = "";
    try {
        //imgurl += "<a onclick='MaterialConsumptionEdit(\"" + value + "\",\"" + row.Status + "\")' style='cursor:pointer;color:#169BD5;' alt='修改'> 修改 </a>"
        //    + " <a onclick = 'MaterialConsumptionDel(\"" + value + "\",\"" + row.Status + "\")' style = 'cursor:pointer;color:#169BD5;'  > 删除 </a > "
        imgurl +=  " <a onclick = 'RecordView(\"" + value + "\",\"" + row.MaterialName + "\")' style = 'cursor:pointer;color:#169BD5;' > 申领记录 </a > ";

    } catch (e) {
        alert(e);
    }
    return imgurl;
}

//oper 耗材申请记录操作
function FormatMaterialRecordOper(value, row, index) {
    var imgurl = "";
    try {
        imgurl += "<a onclick='MaterialConsumptionRecordEdit(\"" + value + "\",\"" + row.Status + "\")' style='cursor:pointer;color:#169BD5;' alt='修改'> 修改 </a>"
            + " <a onclick = 'MaterialConsumptionRecordDel(\"" + value + "\",\"" + row.Status + "\")' style = 'cursor:pointer;color:#169BD5;'  > 删除 </a > "
            + " <a onclick = 'ApplyForCancel(\"" + value + "\",\"" + row.Status + "\")' style = 'cursor:pointer;color:#169BD5;' > 撤销申请 </a > ";

    } catch (e) {
        alert(e);
    }
    return imgurl;
}

//设置产品信息状态
function FormatProductInfoStatus(value, row, index) {

    var imgurl = "";
    try {
        if (value == "1")
            imgurl += " <span  style='color:#d85151' value='0' dataID='" + row["Recid"] + "'  onclick='SetProductInfoEnableStatus(this)'>启用</span> ";
        else
            imgurl += " <span  style='color:#5180D8' value='1' dataID='" + row["Recid"] + "'  onclick='SetProductInfoEnableStatus(this)'>禁用</span> ";

    } catch (e) {
        alert(e);
    }
    return imgurl;
}

//设置产品状态
function FormatProductStatus(value, row, index) {

    var imgurl = "";
    try {
        if (value=="1")
            imgurl += " <span  style='color:#d85151' value='0' dataID='" + row["Recid"] + "'  onclick='SetProductEnableStatus(this)'>启用</span> ";
        else
            imgurl += " <span  style='color:#5180D8' value='1' dataID='" + row["Recid"] + "'  onclick='SetProductEnableStatus(this)'>禁用</span> ";

    } catch (e) {
        alert(e);
    }
    return imgurl;
}

//设置产品规格状态
function FormatProductSpecStatus(value, row, index) {
    var imgurl = "";
    try {
        console.log(value);
        if (value == "1")
            imgurl += " <span  style='color:#d85151' value='0' dataID='" + row["ID"] + "'  onclick='SetProductSpecEnableStatus(this)'>启用</span> ";
        else
            imgurl += " <span  style='color:#5180D8' value='1' dataID='" + row["ID"] + "'  onclick='SetProductSpecEnableStatus(this)'>禁用</span> ";

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

function FormatStatusRecord(value, row, index) {
    var imgurl = "";
    try {
        //1: 待审核 2待领取 3已完成  4已撤销 5 驳回
        if (value == "1")
            imgurl += "<center>待审核</center>";
        else if (value == "2")
            imgurl += "<center>待领取</center>";
        else if (value == "3")
            imgurl += "<center>已完成</center>";
        else if (value == "4")
            imgurl += "<center>已撤销</center>";
        else if (value == "5")
            imgurl += "<center>驳回</center>";
    } catch (e) {
        imgurl = value;
    }
    return imgurl;
}

//商品类型
function FormatProductType(value, row, index) {
    var imgurl = "";
    try {
        //1：办公消耗 2：试验消耗
        if (value == "1")
            imgurl += "<center>办公消耗</center>";
        else if (value == "2")
            imgurl += "<center>试验消耗</center>";
    } catch (e) {
        imgurl = value;
    }
    return imgurl;
}
