;
(function($, window, document) {
    var that;
    var filterDelay = 1000;

    var tabData = {
        table: null, //table
        tab: null, // return table
        data: null,
        that: null,
        tabFilter: '',
        done: function(data) {
            tabData.data = data;

            var thead = tabData.that.find(".layui-table-header table thead").eq(0).find('tr');

            var getWidth = thead.eq(0).find('th:visible');
            var setWidth = thead.eq(1).find('th:visible');

            var filter, wid, setfilter, getfilter;
            for (var i = 0, len = setWidth.length; i < len; i++) {
                setfilter = setWidth.eq(i).find('.table-filter');
                getfilter = getWidth.eq(i).find('.layui-table-cell');
                if (setfilter.length) {
                    //获取当前宽度并在style中赋值
                    wid = getfilter[0].getClientRects()[0].width;
                    setfilter.width(wid + 'px');
                }
            }
        },
        idx: -1, //当前选中的行

        //返回固定框
        fixed: function() {
            var fix = tabData.that.find(".layui-table-fixed .layui-table-body");
            if (!fix.length) {
                fix = tabData.that.find(".layui-table-body");
                if (fix.length) {
                    fix = fix.eq(fix.length - 1);
                }
            }
            return fix;
        },
        //返回数据内容
        mainBody: function() {
            return tabData.that.find(".layui-table-box .layui-table-body");
        },
        //返回多选 选中的编号 [0,1,2]
        checked: function() {
            var ary = [],
                checked = this.fixed().find(":checked");
            for (var i = 0, len = checked.length; i < len; i++) {
                ary.push(checked.eq(i).parents('tr').index());
            }
            return ary;
        }
    };
    //返回筛选条件
    $.fn.tableFilter = function() {
        var thead = tabData.that.find(".layui-table-header table thead").eq(0).find('tr');
        var filter = thead.eq(1).find('th:visible').find(":input");
        var ary = [],
            tmp;
        for (var i = 0, len = filter.length; i < len; i++) {
            tmp = filter.eq(i);
            if (tmp.val()) {
                ary.push({
                    field: tmp.attr('name'),
                    value: tmp.val(),
                    "op": "contains"
                });
            }

        }
        return ary;
    }


    $.fn.pubselect = function() {
        var idx;
        if (tabData.idx != -1) {
            idx = tabData.idx;
            tabData.idx = -1;
        } else {
            idx = tabData.checked()[0];

        }
        return tabData.data[idx];

    }
    $.fn.pubselects = function() {

        var checked = tabData.checked();
        var ary = [];
        for (var i = 0, len = checked.length; i < len; i++) {
            ary.push(tabData.data[checked[i]]);
        }
        return ary;
    }

    $.fn.setWebList = function(options, val) {
        that = $(this);
        tabData.that = that;


        var settings = $.extend({}, tabData, options);
        //**** 初始化界面 ****
        var tab;
        try {
            //按钮
            if (!val.success) {
                alert(val.msg);
                return;
            }
            //加载自定义js
            loadLink(val.data.form);
            //初始化隐藏字段
            // initHiddenField(val.data);
            //初始化工具栏
            initToolbar(val.data.buttonList);
            //初始化查询栏
            initCondition(val.data.conditionList, val.data.conditionCols || 3);
            //列表
            initTableForm(val.data, [{
                type: 'numbers'
            }, {
                type: 'checkbox'
            }]);

            //初始化字段选择
            // if (hiddenZdzdField.length > 0) {
            //     initZdzdChoose();
            // }
            //初始化其他内容
            // initCheckbox1();
        } catch (e) {
            console.log("JS执行出错，原因：" + e.Message)
            // alert("JS执行出错，原因：" + e.Message);
        }

        return;
    };
    //查询
    $.fn.searchRecord = function() {

        var key, val, ipt, obj = {};
        var filter = [];

        var ary = that.find('.top-filter .filter-ipt')
        for (var i = 0, len = ary.length; i < len; i++) {
            ipt = ary.eq(i).find(":input");
            //区间查询
            if (ipt.length > 1) {
                val = ipt.eq(0).val();
                if (val) {
                    key = ipt.eq(0).attr('name');
                    obj[key] = val;
                }

                val = ipt.eq(1).val();
                if (val) {
                    key = ipt.eq(1).attr('name');
                    obj[key] = val;
                }

            } else if (ipt.length == 1) {
                val = ipt.val();
                if (val) {
                    key = ipt.attr('name');
                    obj[key] = val;
                }
            }
        }
        tabParams(obj);
    }
    //加载链接脚本
    function loadLink(data) {
        if (data.js && data.js.length) {
            var ary = data.js.split(',');
            for (var i = 0, len = ary.length; i < len; i++) {
                $("body").append('<script type="text/javascript" src="/skins/WebList/custom/' + ary[i] + '"></script>')
            }
        }
    }

    //顶部按钮
    function initToolbar(buttonList) {
        var str = '<div class="top-btn"><div class="buttonList" >';
        var tmp;


        for (var i = 0, len = buttonList.length; i < len; i++) {
            tmp = buttonList[i];

            switch (tmp.type) {
                case 'checkbox':
                    var ary = tmp.mc.split(",");
                    str += '<div class="switch"><span>' + ary[0] + '</span><div onclick="layuiFormSwitch(this);' + tmp.funname + '" checked="false" class="layui-unselect layui-form-switch" lay-skin="_switch"><em></em><i></i></div>';
                    str += '<span>' + ary[1] + '</span></div>';

                    break;
                case 'radio':
                    var ary = tmp.mc.split(",");
                    str += '<div class="btn">';
                    for (var j = 0, len2 = ary.length; j < len; j++) {
                        str += '<input type="radio" name="radio-' + i + '" ';
                        if (ary[j] == tmp.def)
                            str += "checked='checked' ";
                        str += "onclick='" + tmp.funname + "' ";
                        str += "/>" + ary[j];
                    }
                    str += '</div>';
                    break;
                case 'checkbox1':
                    var ary = tmp.mc.split(",");
                    str += "<p class='field switch'>";
                    if (ary[0] == tmp.def) {
                        checkbox1value = ary[0];
                        str += "<label class='cb-enable selected'><span>" + ary[0] + "</span></label>";
                    } else
                        str += "<label class='cb-enable'><span>" + ary[0] + "</span></label>";
                    if (ary[1] == tmp.def) {
                        checkbox1value = ary[1];
                        str += "<label class='cb-disable selected'><span>" + ary[1] + "</span></label>";
                    } else
                        str += "<label class='cb-disable'><span>" + ary[1] + "</span></label>";
                    break;
                default:
                    str += '<div class="btn" onclick="' + tmp.funname + '"><span><i class="' + tmp.icon + '"></i> ' + tmp.mc +
                        '</span></div>';
                    break;
            }
        }
        str += "</div></div>";
        that.append(str);
    }
    //头部表单布局
    function initCondition(data, cols) {
        // cols  自定义的列数
        var str = '<div class="top-filter dis-table">';
        var colsLen = 0;
        str += '<div class="dis-rows">';
        for (var i = 0, len = data.length; i < len; i++) {
            str += '<div class="dis-cell">' + conditionType(data[i]) + '</div>';

            colsLen++;
            if (colsLen % cols == 0) {
                if (i < len - 1) {
                    str += '</div><div class="dis-rows">';
                } else {
                    str += '</div>';
                }
            }
        }
        //如果最后没有闭合
        if (colsLen % cols != 0) {
            str += '</div>';
        }
        str += "</div>";
        that.append(str);
    }

    function initTableForm(data, leftGd) {
        //界面参数
        var listform = data.form;
        listform.limitsize = eval(listform.limitsize);
        //初始化字段
        var fieldzdzd = data.zdzdList;
        var str1 = '',
            str2 = '';

        var sfgd = 0;
        var cols = [];
        //如果有筛选搜索条
        if (data.zdzdList) {
            var gd = [],
                ary = [],
                gdFilter = [],
                aryFilter = [];
            //添加左侧固定栏的类型
            for (var i = 0, len = leftGd.length; i < len; i++) {
                gd.push({
                    type: leftGd[i].type,
                    fixed: 'left'
                });
                leftGd[i].type == 'numbers' ? gdFilter.push("<th><div class='table-filter'>检索</div></th>") : gdFilter.push(
                    "<th></th>");
            }

            for (var i = 0, len = data.zdzdList.length; i < len; i++) {
                if (data.zdzdList[i].sfgd) {
                    if (!data.zdzdList[i].sfhidden) {
                        gd.push(tableForm(data.zdzdList[i]));
                    }
                } else {
                    if (!data.zdzdList[i].sfhidden) {
                        ary.push(tableForm(data.zdzdList[i]));
                    }
                }
            }
        }

        cols.push(gd.concat(ary));

        that.append('<div class="table-warp"><table class="tableData" id="table"></table></div>');

        layui.use(['table', 'layer'], function() {
            var table = layui.table;
            var layer = layui.layer;

            tabData.table = table;

            var loadWinIndex = layer.load(2);
            setTimeout(function() {
                tabData.tab = table.render({
                    url: '/WebList/SearchEasyUiFormData',
                    method: 'post',
                    where: {
                        param: $("#param").val()
                    },
                    request: {
                        pageName: 'page', //页码的参数名称，默认：page
                        limitName: 'rows' //每页数据量的参数名，默认：limit
                    },
                    response: {
                        countName: 'total', //数据总数的字段名称，默认：count
                        dataName: 'rows'
                    },
                    page: {
                        //分页条数
                        limit: listform.pagesize ? listform.pagesize : 10,
                        //每页条数的选择项
                        limits: listform.limitsize.length ? listform.limitsize : [5, 10, 20, 50],
                    },
                    loading: true,
                    done: function(res, curr, count) {
                        //为了防止 页面没加载出来
                        // var interval = setInterval(function() {
                        var h = window.innerHeight || document.documentElement.clientHeight || document.body.clientHeight;
                        if (h > 0) {
                            h -= that.find('.top-btn').height();
                            h -= that.find('.top-filter').height();
                            h -= that.find('.layui-table-header').height();
                            h -= 55;
                            var tab1 = $('.layui-table-box').children('.layui-table-main').height(h).find('table');
                            var tab2 = tabData.fixed().height(h).find('table');

                            tab2.height(tab1.height());

                            // clearTimeout(interval);
                            if (res.rows && res.rows.length) {
                                tabData.tab.scrollPatch();
                            }


                            tabData.done(res.rows);
                        } else {
                            //页面暂时隐藏的时候
                            $('.table-filter').width('100%');
                        }

                        // }, 1000);

                        layer.close(loadWinIndex);
                    },
                    elem: that.find('.tableData')[0],
                    cols: cols,
                    //自定义排序
                    curtomSort: function(type, field) {
                        if (type) {
                            tabParams({
                                sort: field,
                                order: type
                            });
                        } else {
                            tabParams();
                        }
                    }
                });

                that.on('dblclick', 'td', function(event) {
                    var idx = $(this).parent().index();
                    tabData.idx = idx;

                    var warp = tabData.fixed().find('tr').eq(idx);
                    var ipt = warp.find(':input')[0];
                    //点击checkbox
                    var kid = $(this).find(':input')[0];
                    if (kid != ipt) {
                        ipt.checked = !ipt.checked;
                        warp.find('.layui-form-checkbox').toggleClass('layui-form-checked');
                    }

                    if (listform.dbclickfunname) {
                        //eval(listform.dbclickfunname + "()");
                        eval(listform.dbclickfunname);
                        event.stopPropagation();
                    }
                    return false;
                });



                that.on('click', 'td', function() {
                    var idx = $(this).parent().index();

                    var warp = tabData.fixed().find("tr").eq(idx);
                    var ipt = warp.find(':input')[0];
                    //点击checkbox
                    var kid = $(this).find(':input')[0];
                    if (kid == ipt) {
                        return;
                    }
                    ipt.checked = !ipt.checked;
                    warp.find('.layui-form-checkbox').toggleClass('layui-form-checked');
                });

                if (data.filter) { //是否需要过滤条件
                    var th, tmp, wid;
                    var thead = that.find(".layui-table-header table thead").eq(0).find('tr');
                    for (var i = 0, len = data.zdzdList.length; i < len; i++) {
                        th = data.zdzdList[i].sfhidden ? '<th style="display:none;">' : '<th>';
                        tmp = "<div class='table-filter' style='width:1px'>";
                        try {
                            // data-field="RYXM"
                            wid = thead.find('[data-field="' + data.zdzdList[i].zdname + '"]').find('.layui-table-cell')[0].getClientRects()[
                                0].width;

                            // wid = thead.eq(i).find('.layui-table-cell')[0].getClientRects()[0].width;
                            tmp = "<div class='table-filter' style='width:" + wid + "px'>";
                        } catch (e) {

                        }


                        if (data.zdzdList[i].sfgd) {
                            gdFilter.push(th + tmp + tableFilter(data.zdzdList[i]) + "</div></th>");
                        } else {
                            aryFilter.push(th + tmp + tableFilter(data.zdzdList[i]) + "</div></th>");
                        }
                    }
                    //添加左侧固定栏的空标签
                    tmp = '';
                    for (var j = 0, len2 = gd.length; j < len2; j++) {
                        tmp += "<td></td>";
                    }
                    var str = "<tr>" + gdFilter.join('') + aryFilter.join('') + "</tr>";

                    var str2 = "<tr>" + gdFilter.join('') + "</tr>";

                    that.find(".layui-table-header table thead").eq(0).append(str);
                    that.find(".layui-table-fixed  table thead").eq(0).append(str2);

                    filterRules();
                }
            }, 10)
        });

    }

    function filterRules() {
        var time;

        that.on("keydown change", ".table-filter :input", function(e) {
            if (time) {
                clearTimeout(time);
            }
            if (e.keyCode == 13) {
                tabParams();
            } else {
                time = setTimeout(tabParams, filterDelay);
            }
        });
    }



    function tabParams(obj) {
        obj = obj || {};
        var filter = [];
        var ary = that.find(".table-filter :input");
        var tmp, val, name, data;
        for (var i = 0, len = ary.length; i < len; i++) {
            tmp = ary.eq(i);
            val = tmp.val();
            if (val != '' && tmp.attr("name") && tmp.attr("name").length) {
                data = {
                    field: tmp.attr("name"),
                    op: "contains",
                    value: tmp.val()
                    // value:'a:b'区间类型的table-filter
                };

                filter.push(data);
            }
        }

        obj.param = $("#param").val()


        if (filter.length) {
            obj.filterRules = JSON.stringify(filter);
        }
        tabAjax(obj, function(res) {
            tabData.done(res.rows);
        });
    }

    function tabAjax(params, handle) {
        tabData.tab.pullData(params, handle);
    }

    function tableForm(val) {
        var obj = {
            field: val.zdname,
            title: val.zdsy,
            align: val.align,
            event: "",
        }
        if (val.sfgd) { //是否固定
            obj.fixed = 'left';
        }
        if (val.sfpx) { //是否排序
            obj.sort = true;
        }
        if (val.zdwidth != 50) { //自定义宽度
            obj.width = val.zdwidth;
        }
        if (val.formatevent) {
            var num = 0;
            obj.templet = function(row) { //自定义js td
                var idx = (function() {
                    return num++;
                })();
                return eval(val.formatevent + "(row[this.field],row,idx)");
            }
        }

        return obj;
    }

    function tableFilter(val) {
        var str = '';
        var obj = {
            id: val.zdname,
            name: val.zdname,
            DefVal: val.filterDef || ''
        };
        switch (val.filtertype) {
            case 'list': //下拉框
                str = makeSelect({
                    id: '',
                    name: val.zdname,
                    CtrlString: val.datatype
                });
                break;
            case 'dateRange': //日期范围
                onFocus = 'WdatePicker({isShowClear:false,readOnly:false,dateFmt:"yyyy-MM-dd"})';
                str = makeIpt({
                    id: val.zdname + '1',
                    name: val.zdname + '1',
                    DefVal: val.filterDef || '',
                    onFocus: onFocus
                });

                str += '-' + makeIpt({
                    id: val.zdname + '2',
                    name: val.zdname + '2',
                    DefVal: val.filterDef || '',
                    onFocus: onFocus
                });

                break;
            case 'dateRange1': //日期范围
                onFocus = 'WdatePicker({isShowClear:false,readOnly:false,dateFmt:"yyyy-MM-dd"})';
                str = makeIpt({
                    id: val.zdname + '1',
                    name: val.zdname + '1',
                    DefVal: val.filterDef || '',
                    onFocus: onFocus
                });

                str += '-' + makeIpt({
                    id: val.zdname + '2',
                    name: val.zdname + '2',
                    DefVal: val.filterDef || '',
                    onFocus: onFocus
                });

                break;
            case 'dateRange2': //日期范围
                onFocus = 'WdatePicker({isShowClear:false,readOnly:false,dateFmt:"yyyy-MM-dd"})';
                str = makeIpt({
                    id: val.zdname + '1',
                    name: val.zdname + '1',
                    DefVal: val.filterDef || '',
                    onFocus: onFocus
                });

                str += '-' + makeIpt({
                    id: val.zdname + '2',
                    name: val.zdname + '2',
                    DefVal: val.filterDef || '',
                    onFocus: onFocus
                });
                break;

            case "yyyy": //日期年
                onFocus = 'WdatePicker({isShowClear:false,readOnly:false,dateFmt:"yyyy"})';
                obj.onFocus = onFocus;
                str = makeIpt(obj);

                break;
            case "yyyymm": //日期年月
                onFocus = 'WdatePicker({isShowClear:false,readOnly:false,dateFmt:"yyyy-MM"})';
                obj.onFocus = onFocus;
                str = makeIpt(obj);
                break;
            case "date": //日期年月日
                onFocus = 'WdatePicker({isShowClear:false,readOnly:false,dateFmt:"yyyy-MM-dd"})';
                obj.onFocus = onFocus;
                str = makeIpt(obj);
                break;
            case 'number': //数值
                break;
            case 'bdgrid': //帮助列表
                break;
            default:
                str = '<input class="ipt" name="' + val.zdname + '" value="' + val.filterDef + '" />'
                break;
        }
        return str;
    }


    // 查询栏类型
    function conditionType(data) {

        var str = '<div class="filter-ipt">';
        var onFocus;
        var obj = {
            id: data.FieldName,
            name: data.FieldName,
            DefVal: data.DefVal || ''
        };
        switch (data.FieldType) {
            case "SELECT":
                obj.CtrlString = data.CtrlString;
                str += '<label>' + data.FieldSy + '</label>' + makeSelect(obj);
                break;
            case "DATEYYYY": //日期年
                onFocus = 'WdatePicker({isShowClear:false,readOnly:false,dateFmt:"yyyy"})';
                obj.onFocus = onFocus;
                str += '<label>' + data.FieldSy + '</label>' + makeIpt(obj);

                break;
            case "DATEYYYYMM": //日期年月
                onFocus = 'WdatePicker({isShowClear:false,readOnly:false,dateFmt:"yyyy-MM"})';
                obj.onFocus = onFocus;
                str += '<label>' + data.FieldSy + '</label>' + makeIpt(obj);

                break;
            case "DATEMM": //日期月
                onFocus = 'WdatePicker({isShowClear:false,readOnly:false,dateFmt:"MM"})';
                obj.onFocus = onFocus;
                str += '<label>' + data.FieldSy + '</label>' + makeIpt(obj);
                break;
            case "DATEM": //日期月
                onFocus = 'WdatePicker({isShowClear:false,readOnly:false,dateFmt:"MM"})';
                obj.onFocus = onFocus;
                str += '<label>' + data.FieldSy + '</label>' + makeIpt(obj);
                break;
            case "DATEDD": //日期日
                onFocus = 'WdatePicker({isShowClear:false,readOnly:false,dateFmt:"dd"})';
                obj.onFocus = onFocus;
                str += '<label>' + data.FieldSy + '</label>' + makeIpt(obj);
                break;
            case "DATES": //日期范围
                break;
            case "NUMBERS": //数值范围
                var obj1 = {
                    id: data.FieldName + '1',
                    name: data.FieldName + '1',
                    DefVal: data.DefVal || ''
                };
                var obj2 = {
                    id: data.FieldName + '2',
                    name: data.FieldName + '2',
                    DefVal: data.DefVal || ''
                };
                str += '<label>' + data.FieldSy + '</label>' + makeIpt(obj1) + '<span class="filter-space">-</span>' + makeIpt(
                    obj2);
                break;
            default: //TEXT || ""
                str += '<label>' + data.FieldSy + '</label>' + makeIpt(obj);
                break;
        }
        str += '</div>'
        return str;
    }

    function makeIpt(data) {
        /*
                var obj = {
                    id: data.FieldName,
                    name: data.FieldName,
                    DefVal: data.DefVal || ''
                };
        */

        var str = "<input type='text' name='" + data.name + "' id='" + data.id + "' ";
        if (data.onFocus) {
            str += "class='Wdate layui-input' onFocus=" + data.onFocus;
        } else {
            str += "class='layui-input' ";
        }
        str += " value='" + data.DefVal + "' />";

        return str;
    }

    function makeSelect(data) {
        /*
            data = {
                id:id,
                name:name,
                CtrlString:CtrlString,
                hasClass:''
            }
        */
        var obj = makeCtrlString(data.CtrlString);
        var ary = obj.aryData;

        var str = '<select name="' + data.name + '" id="' + data.id + '" ';
        if (data.hasClass) {
            str += 'class="' + data.hasClass + '"';
        }
        if (obj.change) {
            str += ' onchange=filterOnChange(event,"' + obj.change + '")';
        }
        str += ' >';

        for (var i = 0, len = ary.length; i < len; i++) {

            if (ary[i].flag == 1) {
                str += '<option selected="selected" value="' + ary[i].value + '">' + ary[i].key + '</option>';
            } else {
                str += '<option value="' + ary[i].value + '">' + ary[i].key + '</option>';
            }
        }
        str += '</select>'
        return str;
    }

    //筛选框有事件 改变值影响另一个
    window.filterOnChange = function(event, change) {
        var val = event.target.value;

        ajaxTpl('/WebList/CtrlStringDataEasyUI', {
            ctrlString: change.replace(/wherectrl-.*?\|/, 'wherectrl-' + val + '|')
        }, function(data) {
            var str = '';
            if (data.data && data.data.rows && data.data.rows.length) {
                var rows = data.data.rows;
                for (var i = 0, len = rows.length; i < len; i++) {
                    str += "<option value='" + rows[i].id + "'>" + rows[i].text + "</option>";
                }
            }
            var match = change.match(/targetctrl-.*?\|/);
            if (match.length) {
                var szxq = match[0].replace('targetctrl-', '').replace('|', '');
                var th = $("#tableList").find('.layui-table-header table thead').eq(0).find('tr').eq(1).find('[name="' + szxq +
                    '"]');
                if (th.length) {
                    th.html(str);
                }
            }
        });
    }

    function makeCtrlString(str) {

        var ctrl = str.split("||");
        var tmp, aryData = [],
            change;

        for (var j = 0, len2 = ctrl.length; j < len2; j++) {
            if (ctrl[j].indexOf('value--') > -1) {
                var strs = ctrl[j].replace("value--", '');
                var ary = strs.split("|");
                for (var i = 0, len = ary.length; i < len; i++) {
                    tmp = ary[i].split(",");
                    aryData.push({
                        key: tmp[0] || "",
                        value: tmp[1] || "",
                        flag: tmp[2] || ""
                    });
                }
            } else if (ctrl[j].indexOf('ctrlChange--') > -1) {
                change = ctrl[j].replace("ctrlChange--", '');
            }
        }
        var obj = {
            aryData: aryData
        }
        if (change) {
            obj.change = change;
        }
        return obj;
    }
})(jQuery, window, document);