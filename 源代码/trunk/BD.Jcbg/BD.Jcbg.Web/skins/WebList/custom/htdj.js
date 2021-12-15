function dbclick(row) {
    alert(row.SZSF);
}

function addRecord() {
    var selected = dataGrid.bootstrapTable('getSelections');

    if (selected.length == 0)
        return;

    alert(selected[0].SZSF + ' ' + selected[0].SZCS + ' ' + selected[0].SZXQ);
}

function modifyRecord(rowindex) {
    //判断是否传递了行号
    if (rowindex == undefined) {
        rowindex = dataGrid.jqxGrid('getselectedrowindexes');         //单选
    }
    //获取多选择的 IDgetselectedrowindexes 
    alert(rowindex);
    var data = dataGrid.jqxGrid('getrowdata', rowindex);
}

//格式化内容
function FormatEventTest(row, datafield, value) {
    var imgurl = "";
    if(value == "111")
        imgurl = "<img src='/skins/WebList/custom/img/set1.png'/>";
    else if(value == "222")
        imgurl = "<img src='/skins/WebList/custom/img/set2.png'/>";
    else if(value == "333")
        imgurl = "<img src='/skins/WebList/custom/img/set3.png'/>";
    return imgurl;
}
