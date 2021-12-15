function add() {
    try {
        var tabledesc = "检测设备";                // 表格描述
        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_SB"); 			// 表名
        var tablerecid = encodeURIComponent("RECID"); 	// 表主键
        var title = encodeURIComponent(tabledesc); 	// 标题
        var formdm = tablename;                             // 列表key名称
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&rownum=2" +
            "&_=" + rdm;

        var content = '<iframe id="frame3d" name="frame3d" frameborder="0" width="100%" scrolling="auto" style="margin-top:-4px;" onload="this.style.height=document.body.clientHeight-84" height="100%" src="' + url + '"><iframe>';
        
        parent.layer.open({
            type: 1,
            title: '录入' + tabledesc,
            shadeClose: false,
            shade: 0.8,
            area: ['95%', '95%'],
            content: content,
            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}
function copy() {
    try {
        var tabledesc = "检测设备";       

        var selected = pubselect();
        if (selected == undefined)
            return;

        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_SB"); 			// 表名
        var tablerecid = encodeURIComponent("RECID"); 	// 表主键
        var copyjydbh = encodeURIComponent(selected.RECID)   // 键值
        var title = encodeURIComponent(tabledesc); 	// 标题
        var formdm = tablename;
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&copyjydbh=" + copyjydbh +
            "&rownum=2" +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '复制' + tabledesc,
            shadeClose: false,
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
function edit() {
    try {
        var tabledesc = "检测设备";    

        var selected = pubselect();
        if (selected == undefined)
            return;

        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_SB"); 			// 表名
        var tablerecid = encodeURIComponent("RECID"); 	// 表主键
        var jydbh = encodeURIComponent(selected.RECID)   // 键值
        var title = encodeURIComponent(tabledesc); 	// 标题
        var formdm = tablename;
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            "&button=" + buttons +
            "&jydbh=" + jydbh +
            "&rownum=2" +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '修改' + tabledesc,
            shadeClose: false,
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
        var tabledesc = "检测设备";     

        var selected = pubselect();
        if (selected == undefined)
            return;

        if (!confirm("确定要删除所选的记录吗？")) {
            return;
        }
        $.ajax({
            type: "POST",
            url: "/delete/deleteimsb?recid=" + encodeURIComponent(selected.RECID),
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
    } catch (e) {
        alert(e);
    }
}

function view() {

    try {
        var tabledesc = "检测设备";

        var selected = pubselect();
        if (selected == undefined)
            return;

        var zdzdtable = encodeURIComponent("ZDZD_JC"); // zdzd名
        var tablename = encodeURIComponent("I_M_SB"); 			// 表名
        var tablerecid = encodeURIComponent("RECID"); 	// 表主键
        var jydbh = encodeURIComponent(selected.RECID)   // 键值
        var title = encodeURIComponent(tabledesc); 	// 标题
        var formdm = tablename;
        var buttons = encodeURIComponent("提交|TJ| "); // 按钮
        var rdm = Math.random();
        var url = "/datainput/Index?zdzdtable=" + zdzdtable +
            "&t1_tablename=" + tablename +
            "&t1_pri=" + tablerecid +
            "&t1_title=" + title +
            //"&button=" + buttons +
            "&jydbh=" + jydbh +
            "&rownum=2" +
            "&_=" + rdm;

        parent.layer.open({
            type: 2,
            title: '查看' + tabledesc,
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
function viewBdxx() {
    try {
        var selected = pubselect();//dataGrid.jqxGrid('getrowdata', rowindex);
        if (!selected) {
            return;
        }
        realViewBdxx(selected.RECID);
    } catch (e) {
        alert(e);
    }
}

function realViewBdxx(recid) {
    try{
        var url = "/WebList/EasyUiIndex?FormDm=I_S_SB_BD&FormStatus=1&FormParam=PARAM--" + encodeURIComponent(recid);

        parent.layer.open({
            type: 2,
            title: '设备检定记录',
            shadeClose: true,
            shade: 0.8,
            area: ['95%', '95%'],
            content: url
        });
    } catch (e) {
        alert(e);
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

function formatFj(value, row, index) {
    var imgurl = "";
    try {
        if (value == "有")
            imgurl += "<center><img src='/skins/default/images/list/set_green.png' title='有' style='cursor:pointer' onclick='realViewBdxx(\""+row.RECID+"\")'/></center>";
        else
            imgurl += "<center><img src='/skins/default/images/list/set_gray.png' title='无' style='cursor:pointer' onclick='realViewBdxx(\"" + row.RECID + "\")'/></center>";

        /*
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


        });*/
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