function switchRecord(obj) {
    try {
        var strLocation = decodeURIComponent(window.location.href);
        // 所有
        if (obj.checked) {
            strLocation = strLocation.replace("NOT||CHECKBOX", "ALL||CHECKBOX").replace("所有|我的", "所有|所有");
        }
        else {
            strLocation = strLocation.replace("ALL||CHECKBOX", "NOT||CHECKBOX").replace("所有|所有", "所有|我的");
        }
        window.location = strLocation;
    } catch (ex) {
        alert(ex);
    }
}