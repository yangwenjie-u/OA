function GetNewscUrl1(articleid, fatherid, newsmenuid) {
    var returl;

    returl = '"/xwwzUser/newsc",{id:"' + articleid + '",fid:"' + fatherid + '",rid:"' + newsmenuid + '"},"_blank"';
    
    return returl;
}

//item为新闻对象
function GetNewscUrl(item, fatherid, newsmenuid) {
    var returl;

    if (item["isfile"].toLowerCase() == "true") {
        returl = '"/xwwzUser/getAttachFile",{id:"' + item["aid"] + '",name:"' + item["name"] + '"}';
    } else if (item["islink"].toLowerCase() == "true") {
        returl = '"' + item["articlelink"] + '",{},"_blank"';
    } else {
        returl = '"/xwwzUser/newsc",{id:"' + item["aid"] + '",fid:"' + fatherid + '",rid:"' + newsmenuid + '"},"_blank"';
    }
    

    return returl;
}

function GetNewscUrlMore(categoryid, fatherid, newsmenuid) {
    var returl;

    returl = '"/xwwzUser/newlb",{cid:"' + categoryid + '",fid:"' + fatherid + '",rid:"' + newsmenuid + '"},"_blank"';

    return returl;
}