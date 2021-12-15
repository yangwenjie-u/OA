function FormatImage(value, row, index) {
    try {
        var imgurl = "";
        try {
            var arr = value.split("|");
            $.each(arr, function (i, item) {
                var itemarr = item.split(",");
                if (itemarr.length < 2)
                    return true;
                imgurl += "<img src='/DataInput/FileService?method=DownloadFile&type=small&fileid=" + itemarr[0] + "' title='" + itemarr[1] + "' style='cursor:pointer;margin-right:2px' onclick='showBigImage(\""+itemarr[0]+"\",\""+itemarr[1]+"\")'/>";

            });
        } catch (e) {
            alert(e);
        }
        return imgurl;

    } catch (e) {
        alert(e);
    }
}
function showBigImage(imgid, imgdesc) {
    top.layer.open({
        type: 1,
        title: "资质详情",
        shadeClose: true,
        shade: 0.8,
        area: ['95%', '95%'],
        content: "<img src='/DataInput/FileService?method=DownloadFile&fileid=" + imgid + "' title='"+imgdesc+"'/>",
        end: function () {
        }
    });
}