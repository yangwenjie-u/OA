function FormatSWRYKQ(value, row, index) {
    var name = "<a style='text-decoration:none;' href='#' onclick='ShowList(\"涉外人员考勤月统计列表\",\"/WebList/EasyUiIndex?FormDm=BBGL_SWRYYKQBB&FormStatus=1&FormParam=PARAM--" + row.SFZHM + "|" + row.KQSJ + "\")' >" + value + "</a>";
    return name;
}

function FormatSWXMKQ(value, row, index) {
    var name = "<a style='text-decoration:none;' href='#' onclick='ShowList(\"项目人员考勤月统计列表\",\"/WebList/EasyUiIndex?FormDm=BBGL_RYYKQBB&FormStatus=1&FormParam=PARAM--" + row.SFZHM + "|" + row.KQSJ + "\")' >" + value + "</a>";
    return name;
}

function ShowList(name, url) {
    top.layer.open({
        title: name,
        closeBtn: 1,
        shade: 0.8,
        type: 2,
        content: url,
        area: ['600px', '400px'],
        maxmin: false
    });
}