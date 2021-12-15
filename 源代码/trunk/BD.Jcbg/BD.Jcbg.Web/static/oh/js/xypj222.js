function isImg(str) {
    str = str.toLocaleLowerCase();
    if (str.indexOf(".jpg") > -1 || str.indexOf(".jpeg") > -1 || str.indexOf(".png") > -1 || str.indexOf(".bmp") > -1 || str.indexOf(".tif") > -1 || str.indexOf(".gif") > -1) {
        return true;
    }
    return false;
}

function isPdf(str) {
    str = str.toLocaleLowerCase();
    if (str.indexOf(".pdf") > -1) {
        return true;
    }
    return false;
}

function showUpFile(doc, fileid, filename) {
    // try {
    // var isImg = isImage(filename.toLowerCase());
    var str = "";
    var par = doc.parents(".line-warp").find(".upload-warp");
    // str += "<div class='down-warp' filename='" + filename + "' fileid='" + fileid + "'><a class='downLoad-link' download='" + filename + "' title='" + filename + "' href='/DataInput/FileService?method=DownloadFile&fileid=" + fileid + "'>" + filename + "</a>";
    // str += "<span class='del-upload' >X</span></div>";
    if (isImg(filename)) {
        str += "<div class='down-warp'  filename='" + filename + "' fileid='" + fileid + "'><span class='downLoad-link'  value='" + fileid + "' filename='" + filename + "' title='" + filename + "' >" + filename + "</span>";
        str += "<span class='del-upload' >X</span></div>";
    } else {
        str += "<div class='down-warp' filename='" + filename + "' fileid='" + fileid + "'><a class='downLoad-link-file' download='" + filename + "' title='" + filename + "' href='/DataInput/FileService?method=DownloadFile&fileid=" + fileid + "'>" + filename + "</a>";
        str += "<span class='del-upload' >X</span></div>";
    }
    par.append(str);

    var fileValue = par.find(".file-value");
    if (fileValue.length) {
        var value = fileValue.val();
        value += "|" + fileid + "," + filename;
        fileValue.val(value);
    } else {
        var value = fileid + "," + filename;
        // par.append(str);
        str = $("<input type='text' style='display:none' class='file-value' name='" + par.attr("value") + "' />").appendTo(par);
        str.val(value);
    }
}



function getImgList(str) {
    var ary = []
    var items = str.split("|");
    var tmp;
    for (var i = 0; i < items.length; i++) {
        if (typeof items[i] == "string" && items[i].length) {
            tmp = items[i].split(",");
            if (tmp.length == 2) {
                ary.push({
                    id: tmp[0],
                    name: tmp[1]
                })
            }
        }
    }
    return ary;
}

function setValue(doc, obj) {
    var ary = doc.find("[name]");
    var tmp, key;
    for (var i = 0; i < ary.length; i++) {
        tmp = ary.eq(i);
        key = tmp.attr("name");
        tmp.val(obj[key]);
    }
}

function setcjGcList(doc, obj) {
    var str = "";
    if (obj.recid) {
        if (obj.zt == "0") {
            str = "<tr class='red-color'>"
        } else {
            str = "<tr>"
        }
        str += "<td>" + (obj.gcbh || "") + "</td><td title='" + (obj.gcmc || "") + "'>" + (obj.gcmc || "") + "</td><td title='" + (obj.sgxkzh || "") + "'>" + (obj.sgxkzh || "") + "</td><td>" + (obj.kgrq || "") + "</td><td>" + (obj.yxrq || "") + "</td><td>" + (obj.xyyxrq || "") + "</td>";
        if (obj.fj) {
            var fjList = getImgList(obj.fj);
            str += "<td class='files'>";
            for (var idx = 0; idx < fjList.length; idx++) {
                if (isImg(fjList[idx].name)) {
                    str += "<span name='" + fjList[idx].name + "' value='" + fjList[idx].id + "'>" + fjList[idx].name + "</span><br/>";
                } else if (isPdf(fjList[idx].name)) {
                    str += "<div class='pdf' name='" + fjList[idx].name + "' value='" + fjList[idx].id + "'>" + fjList[idx].name + "</div><br/>";
                } else {
                    str += "<a class='downLoad-link-file' download='" + fjList[idx].name + "' title='" + fjList[idx].name + "' href='/DataInput/FileService?method=DownloadFile&fileid=" + fjList[idx].id + "'>" + fjList[idx].name + "</a><br/>";
                }
            }
            str += "</td>";
        } else {
            str += "<td>无</td>";
        }
        str += "</tr>";
    } else {
        str += "<tr><td colspan='7'>暂无数据</td></tr>";
    }
    $("#cjgcList").html(str)
}
$("#cjgcList").on("click", ".files span", function() {
    var imgSrc = $(this).attr("value");
    var imgWarp = $("#imgWarp").find("[src*='" + imgSrc + "']");
    var doc;
    if (imgWarp.length) {
        doc = imgWarp.parent();
    } else {
        doc = $("<div><img src='/DataInput/FileService?method=DownloadFile&fileid=" + imgSrc + "'></div>").appendTo($("#imgWarp"));
    }
    doc.show().siblings('div').hide();

    $("#imgWarp").css({ display: 'flex' })

});

$("#tbody").on("click", ".add", function() {
    tt = $(this).attr("tt");

    editInit("", tt);
    editIdx = layer.open({
        type: 1,
        content: $("#edit"),
        title: "新增信用评价",
        area: ["600px", "80%"]
    });
    var doc = $("#pjbz").find("[value='" + tt + "']")
    if (doc.length) {
        doc.show().siblings().hide();
    } else {
        $("#pjbz").children().hide();
        loadPjbz(tt);
    }

    pjbzIdx = layer.open({
        type: 1,
        content: $("#pjbz"),
        title: "评价标准",
        area: ["600px", "80%"]
    });
}).on("click", ".edit", function() {
    // tt = $(this).
    var par = $(this).parents("td");
    tt = par.attr("tt");
    var i = par.attr("data-i");
    var j = par.attr("data-j");
    var obj = geResDataDtl(i, j);
    editInit(obj, obj.tt);


    editIdx = layer.open({
        type: 1,
        content: $("#edit"),
        title: "修改信用评价",
        area: ["600px", "80%"]
    });

}).on("click", ".del", function() {
    var par = $(this).parents("td");
    if (resData.info.zt == "0" || getParams().type) {
        layer.confirm("请确认是否删除？", {
            icon: 3,
            title: "删除确认"
        }, function(idx) {
            del(par);
            layer.close(idx);
        });
    } else {
        layer.confirm("请确认是否删除？删除后需要重新审核！！！", {
            icon: 3,
            title: "删除确认"
        }, function(idx) {
            del(par);
            layer.close(idx);
        });
    }
});

function del(doc) {
    var i = doc.attr("data-i");
    var j = doc.attr("data-j");
    var obj = geResDataDtl(i, j);
    var type = getParams().type;
    ajaxTpl("/tz/getXYPJSHDel", {
        pjid: obj.pjid,
        id: obj.recid,
        table: obj.tt,
        type: type
    }, function(res) {
        if (res.code == 0) {
            layer.msg("删除成功!", {
                icon: 1,
                time: 1500
            });
            loadData(getParams().key);
        } else {
            layer.msg(res.msg || "删除失败!", {
                icon: 2,
                time: 1500
            });
        }
    });
}

function geResDataDtl(i, j) {
    if (i >= 0 && j >= 0) {
        if (resData.list[i] && typeof resData.list[i].rows == "string") {
            return JSON.parse(resData.list[i].rows)[j]
        }
    }
    return {};
}

function upModify(zjzbh, pjid) {
    ajaxTpl("/tz/getXYPJSHModify", {
        pjid: pjid,
        zjzbh: zjzbh
    }, function(res) {
        if (res.code == 0) {
            layer.msg("保存成功!", {
                icon: 1,
                time: 1500
            });
        } else {
            layer.msg(res.msg || "保存失败!", {
                icon: 2,
                time: 2000
            });
        }
        if (resData.info.zt != "0") {
            loadData(getParams().key);
        }
    });
}

function editInit(obj, tt) {
    var doc = $("#edit").find(":input[name]")
    doc.val("");
    $("#fileBtn").empty();
    $("#fileBtn").parents(".line-warp").find(".down-warp").remove();
    // $("#fileBtn").parents(".line-warp").find(".upload-warp").remove();
    yxsjVal = -1;

    uploadify($("#fileBtn"));
    $("[name^=fileselect]").removeAttr("name");
    if (obj != "") {
        var tmp, key;
        for (var i = 0; i < doc.length; i++) {
            tmp = doc.eq(i);
            key = tmp.attr("name")
            if (key == "fwsj") {
                if (obj[key].length > 4 && obj[key].substr(0, 4) > 1991) {
                    obj[key] = obj[key].replace(/\//g, "-").replace(/ .*/, "");
                } else {
                    obj[key] = "";
                }
            }
            if (key == "yxsj") {
                obj[key] = obj[key].replace(/\//g, "-").replace(/ .*/, "");
            }
            if (key == "fj") {
                continue;
            }
            tmp.val(obj[key] || "");
        }
        $("#edit").find(":input[name=id]").val(obj.recid);

        if (obj.fj) {
            var list = getImgList(obj.fj);
            for (var j = 0; j < list.length; j++) {
                tmp = list[j];
                showUpFile($("#fileBtn"), tmp.id, tmp.name);
            }
        }

    }
    if (tt == "I_M_XYPJ_JLGZZL" || tt == "I_M_XYPJ_HTLY") {
        $("#edit [name=df]").removeAttr("readonly");
    } else {
        $("#edit [name=df]").attr("readonly", "true");
    }

}
$("#edit").on("click", ".del-upload", function() {
    var par = $(this).parents(".line-warp")

    var idx = $(this).attr("num");
    $(this).parents(".down-warp").remove();

    var downList = par.find(".down-warp");
    var tmp, ary = [];
    for (var i = 0; i < downList.length; i++) {
        tmp = downList.eq(i);
        ary.push(tmp.attr("fileid") + "," + tmp.attr("filename"))
    }
    par.find(".file-value").val(ary.join("|"));

})

$("#tbody").on("click", ".files span", function() {
    var imgSrc = $(this).attr("value");
    var imgWarp = $("#imgWarp").find("[src*='" + imgSrc + "']");
    var doc;
    if (imgWarp.length) {
        doc = imgWarp.parent();
    } else {
        doc = $("<div><img src='/DataInput/FileService?method=DownloadFile&fileid=" + imgSrc + "'></div>").appendTo($("#imgWarp"));
    }
    doc.show().siblings('div').hide();

    $("#imgWarp").css({ display: 'flex' })

});
$(".close").click(function() {
    $("#imgWarp").hide();
})

function loadPjbz(key) {
    // string lxdm = Request["lxdm"].GetSafeString();
    // string table = Request["table"].GetSafeString();
    ajaxTpl("/tz/getXYPJSHHelp", {
        lxdm: resData.info.lxdm,
        table: key
    }, function(res) {
        if (res.code == 1) {

            pjbzData[key] = res.rows;
            var tmp;
            var str = "<table class='table table-border  layui-table' value='" + key + "'>";
            str += "<thead><tr><th style='width:70%;'>评价标准</th><th style='width:15%;'>计分</th><th style='width:15%;'>操作</th></tr><thead>";
            str += "<tbody>";
            for (var i = 0; i < res.rows.length; i++) {
                tmp = res.rows[i];
                str += "<tr><td>" + tmp.lxmc + "</td><td>" + tmp.fz + "</td><td class='sure' tt='" + key + "'>选择</td></tr>";
            }
            str += "</tbody></table>";
            $("#pjbz").append(str);
        }
    })

}

function uploadify(doc) {
    doc.Huploadify({
        auto: true,
        // fileTypeExts: '*',
        fileTypeExts:'*.jpg;*.jpeg;*.png;*.bmp;*.tif;*.gif;*.pdf',
        multi: false,
        fileObjName: 'Filedata',
        fileSizeLimit: 20480,
        buttonText: '请选择文件',
        'uploader': '/DataInput/FileService?method=UploadFile&type=',
        'onSelect': function(file) {
            // this.addPostParam("file_name", encodeURIComponent(file.name));
        },
        onUploadSuccess: function(file, data) {
            var val = JSON.parse(data);
            if (!val.success) {
                alert(val.msg);
                return;
            }
            var filevalue = doc.val();
            filevalue += val.fileid + "," + val.filename + "|";
            doc.val(filevalue);
            //控件名，当前选择的文件名及上传后返回的文件ID
            showUpFile(doc, val.fileid, val.filename);
        },

        'onUploadError': function(file, errorCode, errorMsg, errorString) {
            alert(file.name + '上传失败，' + errorString + '，请稍后再试');
        }
    });
}
$("#openPjbz").click(function() {
    var doc = $("#pjbz").find("[value='" + tt + "']")
    if (doc.length) {
        doc.show().siblings().hide();
    } else {
        $("#pjbz").children().hide();
        loadPjbz(tt);
    }
    pjbzIdx = layer.open({
        type: 1,
        content: $("#pjbz"),
        title: "评价标准",
        area: ["600px", "80%"]
    });
});
$("#pjbz").on("click", ".sure", function() {
    var tt = $(this).parents(".layui-table").attr("value");
    var idx = $(this).parents("tr").index();
    var obj = pjbzData[tt][idx];
    var doc = $("#edit");

    doc.find("[name=lxtype]").val(obj.lxtype);
    doc.find("[name=lx]").val(obj.lxmc);
    doc.find("[name=df]").val(obj.fz);
    yxsjVal = parseInt(obj.yxsj) || 0;

    doc.find("[name=fwsj]").val("");
    doc.find("[name=yxsj]").val("");

    layer.close(pjbzIdx);
}).on("dblclick", "td", function() {
    $(this).parents("tr").find(".sure").click();
});

$("#edit").on("click", ".downLoad-link", function() {

    // });
    // $("#tbody").on("click", ".files span", function() {
    var imgSrc = $(this).attr("value");
    var imgWarp = $("#imgWarp").find("[src*='" + imgSrc + "']");
    var doc;
    if (imgWarp.length) {
        doc = imgWarp.parent();
    } else {
        doc = $("<div><img src='/DataInput/FileService?method=DownloadFile&fileid=" + imgSrc + "'></div>").appendTo($("#imgWarp"));
    }
    doc.show().siblings('div').hide();

    $("#imgWarp").css({ display: 'flex' })

});

var yxsjVal = -1;

function kgrqChange(value) {
    var day = typeof value == "string" ? value : this.value;

    if (yxsjVal > 0 && day) {
        var date = new Date(day);
        var month = date.getMonth();
        month += yxsjVal + 1;
        date.setMonth(month, 0);
        $(".wdate[name=yxsj]").val(getDate(date))
    }
}

function getDate(date) {
    var month = date.getMonth() + 1;
    month = month < 10 ? "0" + month : month;
    var day = date.getDate();
    day = day < 10 ? "0" + day : day;
    return date.getFullYear() + "-" + month + "-" + day;
}