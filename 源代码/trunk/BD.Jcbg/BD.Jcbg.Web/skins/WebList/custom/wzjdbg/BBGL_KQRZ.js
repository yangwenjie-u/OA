function FormatYesno(value, row, index) {
    var imgurl = "";
    try {

        if (value == "")
            imgurl += "<center><img src='/skins/default/images/list/set_gray.png' title='否'/></center>";
        else
            imgurl += "<center><img src='/skins/default/images/list/set_green.png' title='是'/></center>";
    } catch (e) {
        alert(e);
    }
    imgurl += "";
    return imgurl;
}
