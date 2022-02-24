﻿
//新增人员
function add() {
    try {

        layer.open({
            type: 2,
            title: '添加人员',
            content: '/dhoa/umsedit2',
            shadeClose: true,
            shade: 0.8,
            area: ['1230px', '85%'],

            end: function () {
                searchRecord();
            }
        });
    } catch (e) {
        alert(e);
    }
}

function FormatOper(value, row, index) {

    var imgurl = "";
    try {

       
        if (value.toLowerCase() == "true")
            //imgurl += "   <div id='zhuxiao' style='display: table - cell;'>注销用户</div>";
            imgurl += " <span  style='color:#d85151' value='0' usecode='" + row["USERCODE"] + "'  onclick='AccountStatueSet(this)'>注销用户</span>    <span class='modify' style='width: 80px;' usecode='" + row["USERCODE"] + "' onclick='AccountModify(\"" + row.USERCODE + "\")'>修改</span>";
        else
            imgurl += " <span  style='color:#5180D8' value='1' usecode='" + row["USERCODE"] + "'  onclick='AccountStatueSet(this)'>启用用户</span>    <span class='modify' style='width: 80px;'  usecode='" + row["USERCODE"] + "' onclick='AccountModify(\"" + row.USERCODE + "\")'>修改</span>";

    } catch (e) {
        alert(e);
    }
    return imgurl;
}

function ajaxTpl(url, params, handle, sync) {
    $.ajax({
        url: url,
        type: 'post',
        data: params,
        dataType: 'json',
        async: sync != 'sync',
        success: function (data) {
            if (data.success == true && typeof handle == 'function') {
                handle(data);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log(XMLHttpRequest);
        },
        complete: function (XMLHttpRequest, textStatus) {

        }
    });
}


function AccountStatueSet(doc) {
    var usercode = $(doc).attr("usecode");
    var flag = $(doc).attr('value');
    var str, msg1, msg2;
    if (flag == 1) {
        str = '确认启用用户?';
    } else {
        str = '确认注销用户?';
    }
    layer.confirm(str, { icon: 3, title: '提示' }, function (index) {
        ajaxTpl('/Dhoa/UmsApiService', {
            method: 'User',
            opt: 'ModifyUserStatusByUsercode',
            // username: username,
            usercode: usercode,
            userstatus: flag
        }, function (data) {
            layer.msg(data.msg);
             searchRecord();
        });
        layer.close(index);
    });
}

function AccountModify(usercode) {

    //layer.open({
    //    type: 2,
    //    title: '修改人员',
    //    content: '/dhoa/umsedit2?usercode=' + usercode,
    //    area: ['1230px', '85%'],
    //    end: function () {
    //        searchRecord();
    //    }
    //});

    var dataObj = "{\"UserRecid\":\"f4afa014d93349b2a1e5d4658ba1b22f\",\"ArchivesData\":[{\"Recid\":\"\",\"ArchivesIndex\":\"1\",\"ArchivesType\":\"1\",\"ArchivesName\":\"肯德基档案\",\"AnnexData\":{\"FileName\":\"232\",\"OssUrl\":\"\"},\"Remark\":\"备注\"},{\"ArchivesIndex\":\"1\",\"ArchivesType\":\"1\",\"ArchivesName\":\"肯德基档案\",\"AnnexData\":{\"FileName\":\"\",\"OssUrl\":\"\"},\"Remark\":\"备注\"}]}";

    ajaxTpl('/dhoa/UserArchiveDetailsEdit', dataObj, function (data) {
        if (data.code == '0') {
            layer.alert('保存成功');
            try {
                setTimeout(function () {
                    parent.layer.closeAll();
                }, 1000)
            }
            catch (e) {
            }
        }
        else {

            layer.msg(data.msg, {
                icon: 2,
                time: 2000
            });
        }
    });

  //  $.ajax({
  //      url: '/dhoa/UserArchiveDetailsEdit',
  //      data: data,
  //      type: 'post',
  //      dataType: "json",
  //      contentType: "application/json",
  //      success: function (json) {
  //          layer.msg(data.msg, {
  //              icon: 2,
  //              time: 2000
  //          });
        
  //      }

  //});
}





function FormatJsda(value, row, index) {
    var imgurl = "";
    try {
        imgurl += "<a onclick='showZzDiv(\"" + row.Recid + "\")' style='cursor:pointer;color:#169BD5;' alt='查看技术档案'> 查看技术档案 </a>";
    } catch (e) { }
    return imgurl;
}

//用工类型
function FormatYGXS(value, row, index) {
    var imgurl = "";
    try {
        //1 合约 0在编
        if (value == "1")
            imgurl += "<center>合约</center>";
        else if (value == "0")
            imgurl += "<center>在编</center>";
    } catch (e) {
        imgurl = value;
    }
    return imgurl;
}
function showZzDiv(qybh) {
    parent.layer.open({
        type: 2,
        title: '',
        shadeClose: true,
        shade: 0.8,
        area: ['95%', '95%'],
        content: "/qy/qyzzck?qybh=" + qybh,
        end: function () {
            //searchRecord();
        }
    });
}
