function postcall(url, params, target) {


    var tempform = document.createElement("form");
    tempform.action = url;
    tempform.method = "post";
    tempform.style.display = "none"
    if (target) {
        tempform.target = target;
    }

    for (var x in params) {
        var opt = document.createElement("input");
        opt.name = x;
        opt.value = params[x];
        tempform.appendChild(opt);
    }

    var opt = document.createElement("input");
    opt.type = "submit";
    tempform.appendChild(opt);
    document.body.appendChild(tempform);
    tempform.submit();
    document.body.removeChild(tempform);
}


//JS打开链接接
function linkJs(url, params, target) {


    if (url.indexOf("getAttachFile") >= 0) {
        postcall(url, params, target)
    } else {

        var url_ = url + "?";

        for (var x in params) {
            url_ += x + "=" + params[x] + "&";
        }
        /*
        if (params.fid != null) {
            url_ += "fid=" + params.fid + "&";
        }
        if (params.id != null) {
            url_ += "id=" + params.id + "&";
        }
        if (params.rid != null) {
            url_ += "rid=" + params.rid + "&";
        }
        if (params.cid != null) {
            url_ += "cid=" + params.cid + "&";
        }
        if (params.name != null) {
            url_ += "name=" + params.name + "&";
        }
        if (params.zncontent != null) {
            url_ += "zncontent=" + params.zncontent + "&";
        }
        if (params.page != null) {
            url_ += "page=" + params.page + "&";
        }
        if (params.size != null) {
            url_ += "size=" + params.size + "&";
        }
        */
        window.open(url_, target);
    }
     
}

//取文件后缀名
function GetFileExt(filepath) {
    if (filepath != "") {
        var pos = "." + filepath.replace(/.+\./, "");
        return pos;
    }
}
//取文件名不带后缀
function GetFileNameNoExt(filepath) {
    var pos = strturn(GetFileExt(filepath));
    var file = strturn(filepath);
    var pos1 = strturn(file.replace(pos, ""));
    var pos2 = GetFileName(pos1);
    return pos2;

}
//取文件全名名称
function GetFileName(filepath) {
    if (filepath != "") {
        var names = filepath.split("\\");
        return names[names.length - 1];
    }
}

//字符串逆转
function strturn(str) {
    if (str != "") {
        var str1 = "";
        for (var i = str.length - 1; i >= 0; i--) {
            str1 += str.charAt(i);
        }
        return (str1);
    }
}

 //截取固定字符串长度为18
function cutstr(str) {
    var len = 18;
    var str_length = 0;
    var str_len = 0;
    str_cut = new String();
    str_len = str.length;
    for (var i = 0; i < str_len; i++) {
        a = str.charAt(i);
        str_length++;
        if (escape(a).length > 4) {
            //中文字符的长度经编码之后大于4
            str_length++;
        }
        str_cut = str_cut.concat(a);
        if (str_length >= len) {
            str_cut = str_cut.concat("...");
            return str_cut;
        }
    }
    //如果给定字符串小于指定长度，则返回源字符串；
    if (str_length < len) {
        return str;
    }
}
