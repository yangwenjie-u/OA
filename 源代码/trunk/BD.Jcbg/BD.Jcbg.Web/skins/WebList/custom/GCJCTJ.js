function viewJcjg(lx) {

    var selecteds = pubselects();

    if (selecteds == null || selecteds.length == 0) {
        return;
    }

    var selected = selecteds[0];

    layer.open({
        type: 2,
        title: '送样检测机构',
        shadeClose: true,
        shade: 0.8,
        area: ['600px', '400px'],
        //content: "/jc/viewwtdyc?wtdwyh=" + encodeURIComponent(recid)+"&yczt="+yczt,
        content: "/WebList/EasyUiIndex?FormDm=GCJCTJ_JCJG&FormStatus=0&FormParam=PARAM--" + selected.GCBH+"|"+lx,
        end: function () {

        }
    });
}
function viewWtxq(lx) {
    var selecteds = pubselects();

    if (selecteds == null || selecteds.length == 0) {
        return;
    }

    var selected = selecteds[0];

    top.layer.open({
        type: 2,
        title: '状态过程',
        shadeClose: true,
        shade: 0.8,
        area: ['98%', '98%'],
        //content: "/jc/viewwtdyc?wtdwyh=" + encodeURIComponent(recid)+"&yczt="+yczt,
        content: "/WebList/EasyUiIndex?FormDm=GCJCTJ_ZTFX&FormStatus=0&FormParam=PARAM--" + selected.GCBH + "|" + lx,
        end: function () {

        }
    });

}
function viewBkfb() {
    window.location = "/WebList/EasyUiIndex?FormDm=GCJCTJ&FormStatus=2";
}
function viewKfb() {
    window.location = "/WebList/EasyUiIndex?FormDm=GCJCTJ&FormStatus=1";

}
function viewAll() {
    window.location = "/WebList/EasyUiIndex?FormDm=GCJCTJ&FormStatus=0";
}