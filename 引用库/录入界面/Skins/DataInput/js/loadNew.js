//加载返回的数据
var _data = null;
var dataReadonly = false;


//记录添加的html字符串
//2级表要添加的html  addHtmlStr[i].sed
//3级表要添加的html addHtmlStr[i].child
var addHtmlStr = [];

//记录添加的操作
//2级表长度 addHtmlNum.length  
//2级表tag的长度 addHtmlNum[i]
var addHtmlNum = [];

//给单选框和多选框加后缀  
var check = [];
var checkLength = 0;


function ajaxTpl(url, params, handle, sync) {
    $.ajax({
        url: url,
        type: 'post',
        data: params,
        dataType: 'json',
        async: sync != 'sync',
        success: function(data) {
            console.log(data);
            if (data.success == true && typeof handle == 'function') {
                handle(data);
            }
        },
        error: function(XMLHttpRequest, textStatus, errorThrown) {
            console.log(XMLHttpRequest);
        },
        complete: function(XMLHttpRequest, textStatus) {

        }
    });
}



ajaxTpl('/DataInput/SearchData', { param: $("#param").val() }, function(data) {
    _data = data.data;

    //加载结构
    load(_data);
    //赋值
    try {
        loadData(_data);
    } catch (e) {}
    $('.table').find('.active').removeClass('active')

    for (var i = 0, len = check.length; i < len; i++) {
        $('#main').find('[name= "' + check[i] + '"]').attr('name', check[i] + '--' + checkLength++);
    }

    $('#main').find('[kjlx="BOOTSTRAPSELECT"]').multiselect({
        nonSelectedText: "请选择",
        checkAllText: "全选",
        uncheckAllText: '全不选',
        buttonWidth: "210px",
        includeSelectAllOption: true,
        enableFiltering: true,

    });

    $('#main').find('[kjlx="BOOTSTRAPMULSELECT"]').multiselect({
        nonSelectedText: "请选择",
        checkAllText: "全选",
        uncheckAllText: '全不选',
        buttonWidth: "210px",
        includeSelectAllOption: true,
        enableFiltering: true
    });
});

function load(data) {
    var str = '';
    //1级主表
    for (var i = 0, len = data.t1field.length; i < len; i++) {
        str += comHeadType(data.t1field[i]);
    }
    $('#info').append(str);
    str = ''
    var titleList = '',
        sed = '',
        child = '',
        tmp = '',
        j, len2, kjlx;

    for (i = 0, len = data.t2fields.length; i < len; i++) {
        addHtmlNum.push(1);
        titleList += i == 0 ? '<li class="active">' : '<li>';
        titleList += data.t2fields[i].t2sy + "</li>";

        //2级从表
        str += '<div class="field-sed" t2table="' + data.t2fields[i].t2table + '">';
        str += '<h5>' + data.t2fields[i].t2sy + '<span class="delSed">删除</span><span>复制</span><span class="addSed">添加</span></h5>';
        str += '<div class="tags"><span class="active">' + data.t2fields[i].t2sy + "1" +
            '</span></div><div class="clickMore">更多</div>';
        sed = '<div class="u-sed-info" >';
        for (j = 0, len2 = data.t2fields[i].t2field.length; j < len2; j++) {
            sed += comHeadType(data.t2fields[i].t2field[j]);
            //给单选框和多选框加后缀
            kjlx = data.t2fields[i].t2field[j].kjlx || '';
            if (kjlx == 'RADIO' || kjlx == 'CHECKBOX') {
                check.push(data.t2fields[i].t2field[j].fieldname);
            }
        }

        child = '';
        if (data.t2fields[i].t3field && data.t2fields[i].t3field.length) {

            len2 = data.t2fields[i].t3field.length;
            var width = '';

            if (len2 > 4) {
                width = "width:" + len2 * 150 + "px";
            }

            //3级明细表
            sed += '<div class="field-child" num="' + i + '">';
            sed += '<div class="table-edit"><span class="del">删除 </span><span class="edit">编辑</span><span class="addChild">添加</span></div>';
            child = '<div class="table-warp"><table class="table table-border table-hover" style="' + width + '"><thead><tr><th style="width: 50px;"></th>'
            tmp = "<tr class=''><td style='width: 50px;'><input type='checkbox' class='findSelect' /></td>";
            for (j = 0; j < len2; j++) {
                child += '<th name="' + data.t2fields[i].t3field[j].fieldname + '">' + data.t2fields[i].t3field[
                    j].sy + "</th>";
                tmp += '<td><span>' + '' + '</span>' + chooseType2(data.t2fields[i].t3field[j]) + '</td>';
            }
            child += '</tr></thead>'
            tmp += '</tr>';

            child += '<tbody></tboby></table></div>';

            sed += child;
            sed += '</div>';
        }


        str += sed;
        str += '</div></div>';

        addHtmlStr.push({
            sedsy: data.t2fields[i].t2sy,
            sed: sed,
            child: tmp
        });
    }
    $('#titleList').append(titleList);
    $('#content').append(str);
}

function changeVal(content, change, str) {

    var m, doc, pat, val;

    //获取 targetctrl-M1.SZSF|
    var ary = /targetctrl.*?\|/.exec(change)
    if (ary && ary[0] && ary[0].length) {
        ary[0] = ary[0].substr(0, ary[0].length - 1, 1);
        m = ary[0].split('-')[1];
        doc = content.find(':input[name = "' + m + '"]');

        pat = new RegExp(m + '.*?}');
        val = pat.exec(str);
        val = val[0].split(":")[1];
        val = val.substr(0, val.length - 1, 1);

        doc.val(val);
    }

}

function loadData(data) {

    var tmp, doc, ipt, change, str, val, main, i, j, len, len2, t2datas, k, len3, tab, tbody, ary;
    //加载主菜单的数据
    if (data.t1datas && data.t1datas.t1data) {
        main = $('#main');
        str = JSON.stringify(data.t1datas.t1data).replace(/\"/g, '');
        for (i = 0, len = data.t1datas.t1data.length; i < len; i++) {
            tmp = data.t1datas.t1data[i];
            doc = main.find(':input[name = "' + tmp.zdmc + '"]');

            ipt = doc.eq(0);
            ipt.val(tmp.zdval);

            change = ipt.attr('onchange');
            //有值改变事件
            if (change && change.length) {
                ipt.change(); //执行chang事件
                changeVal(main, change, str);
            }
        }
    }
    //加载2级菜单的数据
    var tableName = []; //记录重复的2级节点的表名及jquery
    if (data.t2datas) {
        content = $('#content');
        for (i = 0, len = data.t2datas.length; i < len; i++) {
            t2datas = data.t2datas[i];
            for (j = 0, len2 = tableName.length; j < len2; j++) {
                //重复的2级节点
                if (t2datas.t2table == tableName[j].name) {
                    doc = tableName[j].content;
                    doc.parent('.field-sed').find('h5 .addSed').click();

                    main = doc.parent('.field-sed').children().last();

                    break;
                }
            }
            if (j == len2) { //不重复的2级节点
                tmp = t2datas.t2data[0];
                if (tmp && tmp.zdmc) {
                    doc = content.find(':visible:input[name = "' + tmp.zdmc + '"]').eq(0);
                    main = doc.parents('.u-sed-info');

                    tableName.push({
                        name: t2datas.t2table,
                        content: main
                    });
                }
                // main.parents('.field-sed').attr('t2table', t2datas.t2table);
            }
            main.attr('t2pri', t2datas.t2pri);


            str = JSON.stringify(t2datas.t2data).replace(/\"/g, '');
            //添加2级从表的数据
            for (j = 0, len2 = t2datas.t2data.length; j < len2; j++) {

                tmp = t2datas.t2data[j];

                doc = main.find(':input[name = "' + tmp.zdmc + '"]')
                ipt = doc.eq(0);
                ipt.val(tmp.zdval);

                change = ipt.attr('onchange');
                //有值改变事件
                if (change && change.length) {
                    ipt.change(); //执行chang事件
                    changeVal(main, change, str);
                }
            }

            //  添加3级从表的数据
            for (j = 0, len2 = t2datas.t3datas.length; j < len2; j++) {
                tmp = t2datas.t3datas[j];

                main.find('.addChild').click();
                tab = main.find('.table tbody').children().last();
                doc = tab.find('td');

                tab.attr('t3pri', tmp.t3pri);

                main.find('.table').attr({
                    't3table': tmp.t3table,
                    't3syfield': tmp.t3syfield
                });

                ary = tmp.t3data;
                if (ary && ary.length) {
                    for (k = 0, len3 = ary.length; k < len3; k++) {
                        ipt = tab.find('[name="' + ary[k].zdmc + '"]');
                        //赋值
                        ipt.val(ary[k].zdval);
                        //对span赋值
                        doc.eq(k + 1).find('span').eq(0).text(ary[k].zdval).attr('title', ary[k].zdval);

                        change = ipt.attr('onchange');
                        //有值改变事件
                        if (change && change.length) {
                            ipt.change(); //执行chang事件
                            changeVal(main, change, str);
                        }
                    }
                }
            }
        }
    }
}

//删除2级从表标签页面
$('#content').on('click', '.delSed', function() {
    var pat = $(this).parents('.field-sed');
    var tags = pat.find('.tags');


    var num = tags.find('.active').index();
    tags.find('.active').remove();

    var children = tags.children();

    pat.find('.u-sed-info').eq(num).remove();
    // --addHtmlNum[pat.index()];

    if (children.length > num) {
        children.eq(num).click();
    } else if (children.length > 0) {
        children.last().click();
    } else {
        pat.find('.addSed').click();
    }
});


function save() {
    var saveData = {};
    var fir = $('#info').find(':input');
    var firData = {},
        obj,
        key, val, i, j, k, len, len2, len3;

    firData = getSaveData(fir);
    if (typeof firData == 'object') {
        saveData.fir = firData;
    } else {
        var text = fir.eq(firData).parents('.input-type').find('label').text() || '必填数据';

        layer.msg('请填写' + text, {
            icon: 2,
            time: 2000
        });
        return;
    }


    var sed = $('#content').find('.field-sed');
    var sedData = [];
    var tmp, x, y, child, ary, t2pri;

    for (i = 0, len = sed.length; i < len; i++) {

        tmp = sed.eq(i).find('.u-sed-info'); //2级从表

        t2table = sed.eq(i).attr('t2table');

        obj = {
            "t2table": t2table,
            "t2data": []
        };


        for (j = 0, len2 = tmp.length; j < len2; j++) {
            x = tmp.eq(j).children('.input-type').find(':input');
            val = getSaveData(x);

            if (typeof val == 'object') {
                t2pri = tmp.eq(j).attr('t2pri')
                if (t2pri && t2pri.length) {
                    val.t2pri = t2pri;
                }
                child = tmp.eq(j).find('.table tbody');
                // child = tmp.eq(j).find('.field-child');

                //有3级节点
                if (child.children().length) {
                    ary = getT3fieldData(child);
                    if (ary == false) {
                        return;
                    } else {
                        val.child = ary;
                    }
                }

                obj.t2data.push(val);

            } else {

                try { //2级节点未填写 必填项目
                    var input = x.eq(val).parents('.input-type');
                    var field = input.parents('.field-sed');

                    var num = field.find('.u-sed-info').index(input.parents('.u-sed-info'));
                    var tx = field.find('.tags').children().eq(num).text();
                    var text = x.eq(val).parents('.input-type').find('label').text() || '必填数据';
                    layer.msg('请填写' + tx + '下的' + text, {
                        icon: 2,
                        time: 2000
                    });
                } catch (e) {
                    console.log('wrong...2级')
                }
                return;
            }
        }
        sedData.push(obj);
    }
    saveData.sedData = sedData;

    console.log(saveData);

}

//保存3级节点的数据
function getT3fieldData(child) {
    var ary = [],
        pat, obj = {},
        orz, tmp, obj, t3syfield, t3table, t3pri;

    var val = child.find('tr');

    pat = child.parents('.table');
    t3syfield = pat.attr('t3syfield');
    t3table = pat.attr('.t3table');
    if (t3syfield && t3syfield.length) {
        obj.t3syfield = t3syfield;
    }
    if (t3table && t3table.length) {
        obj.t3table = t3table;
    }
    for (var i = 0, len = val.length; i < len; i++) {
        tmp = val.eq(i).find(":input")
        orz = getSaveData(tmp);
        if (typeof orz == 'object') {
            t3pri = val.eq(i).attr('t3pri');
            if (t3pri && t3pri.length) {
                orz.t3pri = t3pri;
            }
            ary.push(orz);
        } else {
            try { //3级节点未填写 必填项目

                var input = tmp.eq(orz).parents('td');
                var table = input.parents('.table');
                var field = table.parents('.field-sed');
                var num = field.find('.u-sed-info').index(input.parents('.u-sed-info'));
                var tx = field.find('.tags').children().eq(num).text();

                var text = table.find('th').eq(input.index()).text() || '必填数据';
                layer.msg('请填写' + tx + '下的' + text, {
                    icon: 2,
                    time: 2000
                });
            } catch (e) {
                console.log('wrong...3级')
            }
            return false;
        }
    }
    obj.t3data = ary;
    return obj;
}

function getSaveData(jq) {
    var key, tmp,kjlx, obj = {};
    for (var i = 0, len = jq.length; i < len; i++) {
        tmp = jq.eq(i);
        if (tmp.attr('mustin') == 'mustin' && tmp.val() == '') {
            // alert('必填');
            return i;
        }
        key = tmp.attr('name');
        if (key) {
            kjlx  = tmp.attr('kjlx')
            if (kjlx == 'RADIO' || kjlx == 'CHECKBOX') {
                key = key.split('--')[0];
            }
            obj[key] = tmp.val();
        }
    }
    return obj;
}






//combox
$('#content').on('click input', '.combox input', function(event) {
    $(this).next().show();

    var newVal = $(this).val();
    var li = $(this).next().children();
    var tmp, eq;

    for (var i = 0, len = li.length; i < len; i++) {
        eq = li.eq(i)
        tmp = eq.attr('value');
        if (tmp.indexOf(newVal) == -1) {
            eq.hide();
        } else {
            eq.show();
        }
    }
    //阻止事件继续传递
    event.stopPropagation();
});
//选择组合框的下拉列表
$('#content').on('click', '.combox li', function() {
    $(this).parent().hide();
    $(this).parent().prev().val($(this).attr('value'));
});

//3级表格的下拉框
$('#content').on('click', '.active .select-type input', function(event) {
    console.log('ssssss');
    $(this).next().show();
    //阻止事件继续传递
    event.stopPropagation();
});
//选择组合框的下拉列表
$('#content').on('click', '.select-type li', function() {
    $(this).parent().hide();
    $(this).parent().prev().val($(this).attr('value'));
});



//隐藏组合框的下拉列表
$('body').click(function(event) {
    $('.combox .box-ul').hide();
    $('.select-type .select-ul').hide();
});



//点击最左侧
$('#titleList').on('click', 'li', function() {
    var i = $(this).index();
    var scroll = $('#content').children().eq(i).offset().top;
    $('html').animate({
        scrollTop: scroll
    }, 500);
    $(this).addClass('active').siblings().removeClass('active');
});
//点击标签 切换2级从表
$('#content').on('click', '.tags span', function() {
    var that = $(this);
    var par = that.parent();
    var num = par.find('.active').index();
    var index = that.index();

    var sedInfo = par.parent().find('.u-sed-info');

    sedInfo.eq(num).hide();
    sedInfo.eq(index).show();

    that.addClass('active').siblings().removeClass('active');
    var left = parseInt(par.css('margin-left'));

    var width = 100; // that.innerWidth();
    var parWidth = par.width() / width;

    if ((index + 1) * width + left < parWidth || (index - 2) * width + left > 0) {
        par.css('margin-left', left - width + 'px');
    } else if (left < 0) {
        par.css('margin-left', left + width + 'px');
    }
});



//添加3级明细表  
$('#content').on('click', '.addChild', function() {

    var tbody = $(this).parents('.field-child').find('tbody');
    var num = $(this).parents('.field-child').attr('num');
    var str = addHtmlStr[num].child;
    // tbody.find('.active').removeClass('active').find(':input').attr('readOnly', 'readOnly')
    var cli = $(str).appendTo(tbody);
    // tbody.append(str);
    cli.find('td').eq(0).dblclick();

    try {
        //滚动到最后
        tbody.scrollTop(tbody[0].scrollHeight || '');
    } catch (e) {

    }



}).on('click', '.del', function() {

    var tbody = $(this).parents('.field-child').find('.table tbody');

    var checked = tbody.find('.findSelect:checked');

    var tmp;
    var index = [];
    var del = [];

    for (var i = 0, len = checked.length; i < len; i++) {

        tmp = checked.eq(i).parents('tr');
        del.push(tmp);
        index.push(tmp.index() + 1);
    }

    if (index.length) {
        layer.confirm('确认删除第' + index.join(",") + '条记录', function() {
            for (var i = 0, len = del.length; i < len; i++) {
                del[i].remove();
            }

            layer.msg('删除成功!', {
                icon: 1,
                time: 1500
            });
        });
    } else {
        layer.msg('请选择要删除的数据!', {
            icon: 2,
            time: 2000
        });
    }
}).on('dblclick', '.table td', function() {

    if ($(this).parent().hasClass('active')) {
        return;
    } else {
        var val, tmp;
        var ipt = $(this).parent().addClass('active').siblings('.active').removeClass('active').find('td');

        //可编辑输入
        for (var i = 0, len = ipt.length; i < len; i++) {
            tmp = ipt.eq(i).find(':input').eq(0);
            val = tmp.val() == 'undefined' ? '' : tmp.val();
            tmp.parents('td').children('span').text(val).attr('title', val);
            //.show(); //显示input的值
            // tmp.parents('td').children('div').hide(); //隐藏div
        }


        // ipt = $(this).parent().addClass('active').find(':input');
        //现可编辑框
        // for (i = 0, len = ipt.length; i < len; i++) {
        // tmp = ipt.eq(i);
        // tmp.parents('td').children('span').hide();
        // tmp.parents('td').children('div').show();
        // }
    }
}).on('click', '.edit', function() {

    var tbody = $(this).parents('.field-child').find('.table tbody');
    var checked = tbody.find('.findSelect:checked');

    var tmp;

    if (checked.length) {
        checked.eq(0).dblclick();
    } else {
        layer.msg('请选择要编辑的数据!', {
            icon: 2,
            time: 2000
        });

    }
});


// $('#main').on('keydown', '.table', function (event) {
//     if (event.keyCode == '113') { //按下f2
//     }
// });
// document.addEventListener('keydown', function(event){
//     console.log(event.keyCode=='13');//回车
// });
// $('#main').on('keydown' , '' ,function(event){
//     console.log(event.keyCode);
//      if (event.keyCode == '113') { //按下f2
//     }else if(event.keyCode == ''){
//     }
// });







//添加2级节点
$('#content').on('click', '.addSed', function() {
    var sed = $(this).parents('.field-sed');
    var i = sed.index();

    ++addHtmlNum[i];
    var tag = sed.children('.tags');
    var str = ''
    var j = tag.children().length;
    if (j == 6) {
        sed.find('.clickMore').show();
    }
    tag.find('.active').removeClass('active');
    tag.append('<span class="active">' + addHtmlStr[i].sedsy + addHtmlNum[i] + '</span>');
    sed.children('.u-sed-info').hide();

    //tags标签滑动
    var width = tag.find('span').eq(0).innerWidth();
    if (j > 5) {
        tag.css('margin-left', -(j - 4) * width + 'px');
    }
    sed.append(addHtmlStr[i].sed);

    for (var i = 0, len = check.length; i < len; i++) {
        sed.find(':visible[name= "' + check[i] + '"]').attr('name', check[i] + '--' + checkLength++);
    }
});


function comHeadType(obj) {

    var str = '<div class="input-type">';
    var ary = [];

    str += '<label>' + obj.sy + '</label>';
    str += chooseType(obj);
    str += '</div>';

    return str;
}

function chooseType2(obj) {
    var str;
    //判断类型
    switch (obj.kjlx) {
        case "LABEL":
            str = CreateHtmlLabel(obj);
            break;
        case "TEXT":
            str = CreateHtmlText(obj);
            break;
        case "TEXTAREA":
            str = CreateHtmlTextarea(obj);
            break;
        case "TEXTBUTTON":
            str = CreateHtmlTextButton(obj);
            break;
        case "TEXTBUTTON2":
            str = CreateHtmlTextButton2(obj);
            break;
        case "TEXTIMAGE":
            str = CreateHtmlTextImage(obj);
            break;
        case "CHECKBOX":
            str = CreateHtmlCheckBox(obj);
            break;
        case "RADIO":
            str = CreateHtmlRadio(obj);
            break;
        case "SELECT":
            str = CreateHtmlSelect(obj);
            break;
        case "COMBOBOX":
            str = CreateHtmlCombobox(obj);
            break;
        case "FILE":
        case "FILELABEL":
            str = CreateHtmlFile(obj);
            break;
        case "IMAGEFILE":
            str = CreateHtmlImageFile(obj);
            break;
        case "DATE": //日期
            str = CreateHtmlDate(obj);
            break;
        case "DATETIME": //日期时间
            str = CreateHtmlDateTime(obj);
            break;
        case "BOOTSTRAPSELECT": //bootstrap插件
            str = CreateBootstrapSelect(obj);
            break;
        case "BOOTSTRAPMULSELECT": //bootstrap multiselect 
            str = CreateBootstrapMulSelect(obj);
            break;
        case "BOOTSTRAPSINGSELECT":
            str = CreateBootstrapSingleSelect(obj);
            break;
        default:
            str = CreateHtmlText(obj);
            break;
    }
    return '<div>' + str + '</div>';
    // var str = '';
    // switch (obj.kjlx) {
    //     case 'SELECT':
    //         str = '<div class="select-type" style="display:none;">' + CreateHtmlSelect2(obj) + '</div>';
    //         break;
    //     default:

    //         str = '<div class="input-type" style="display:none;">' + CreateHtmlText2(obj) + '</div>';
    //         break;
    // }
    // return str;
}

function chooseType(obj) {
    var str;
    //判断类型
    switch (obj.kjlx) {
        case "LABEL":
            str = CreateHtmlLabel(obj);
            break;
        case "TEXT":
            str = CreateHtmlText(obj);
            break;
        case "TEXTAREA":
            str = CreateHtmlTextarea(obj);
            break;
        case "TEXTBUTTON":
            str = CreateHtmlTextButton(obj);
            break;
        case "TEXTBUTTON2":
            str = CreateHtmlTextButton2(obj);
            break;
        case "TEXTIMAGE":
            str = CreateHtmlTextImage(obj);
            break;
        case "CHECKBOX":
            str = CreateHtmlCheckBox(obj);
            break;
        case "RADIO":
            str = CreateHtmlRadio(obj);
            break;
        case "SELECT":
            str = CreateHtmlSelect(obj);
            break;
        case "COMBOBOX":
            str = CreateHtmlCombobox(obj);
            break;
        case "FILE":
        case "FILELABEL":
            str = CreateHtmlFile(obj);
            break;
        case "IMAGEFILE":
            str = CreateHtmlImageFile(obj);
            break;
        case "DATE": //日期
            str = CreateHtmlDate(obj);
            break;
        case "DATETIME": //日期时间
            str = CreateHtmlDateTime(obj);
            break;
        case "BOOTSTRAPSELECT": //bootstrap插件
            str = CreateBootstrapSelect(obj);
            break;
        case "BOOTSTRAPMULSELECT": //bootstrap multiselect 
            str = CreateBootstrapMulSelect(obj);
            break;
        case "BOOTSTRAPSINGSELECT":
            str = CreateBootstrapSingleSelect(obj);
            break;
        default:
            str = CreateHtmlText(obj);
            break;
    }
    return str;
}