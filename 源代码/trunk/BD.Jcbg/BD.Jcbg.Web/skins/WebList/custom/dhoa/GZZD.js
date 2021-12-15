


function FormatOper(value, row, index) {
    var imgurl = "";
    try {
        imgurl += "<a onclick='UpdateGZDD(\"" + row.GZDDID + "\")' style='cursor:pointer;color:#169BD5;' alt='修改'> 修改 </a><a onclick='DelGZDD(\"" + row.GZDDID + "\")' style='cursor:pointer;color:#169BD5;' alt='删除'> 删除 </a>";

    } catch (e) {
        alert(e);
    }
    return imgurl;
}

function FormatTrueOrFalse(value, row, index) {
    var imgurl = "";
    try {
        imgurl += "<a onclick='UpdateGZDD(\"" + row.GZDDID + "\")' style='cursor:pointer;color:#169BD5;' alt='修改'> 修改 </a><a onclick='DelGZDD(\"" + row.GZDDID + "\")' style='cursor:pointer;color:#169BD5;' alt='删除'> 删除 </a>";

    } catch (e) {
        alert(e);
    }
    return imgurl;
}


function FormatDownOss(value, row, index) {
    var imgurl = "";
    try {
        imgurl += "<a onclick='DownloadOss(\"" + row.GZDDID + "\")' style='cursor:pointer;color:#169BD5;' alt='下载文件'> 下载文件 </a>";
    } catch (e) { }
    return imgurl;
}

//下载oss文件
function DownloadOss() {

}

///删除规章制度
function UpdateGZDD(recid) {



}
///删除规章制度
function DelGZDD(recid) {

    alert("暂不支持");


}