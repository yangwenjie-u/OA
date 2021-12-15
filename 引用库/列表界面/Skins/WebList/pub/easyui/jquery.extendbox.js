$.extend($.fn.datagrid.defaults.filters, {
    bdgrid: {
        init: function (container, options) {
            var input = $('<input>').appendTo(container);
            input.combo({
                panelWidth: 300,
                onShowPanel: function () {
                    var dd = input.combo('getText').split('~');
                    var d1 = $.fn.datebox.defaults.parser(dd[0]);
                    var d2 = $.fn.datebox.defaults.parser(dd[1]);
                    var p = input.combo('panel');
                    p.find('.c1').calendar('moveTo', d1);
                    p.find('.c2').calendar('moveTo', d2);
                }
            });
            var p = input.combo('panel');
            $('<div class="dbgrid" style="width:50%;float:left"></div>').appendTo(p);
            var grid = p.find('.dbgrid').datagrid({
                queryParams: {
                    param: $("#param").val()
                },
                fit: true,
                pagination: true,
                rownumbers: true,
                remoteFilter: true,
                columns: [[
                    { field: 'a', title: 'a1', width: 100 },
                    { field: 'b', title: 'b1', width: 100 },
                    { field: 'c', title: 'c1', width: 100 }
                ]]
            });
            alert(JSON.stringify(options));
            grid.datagrid('options').url = "/WebList/GetHelplinkDataEasyUI";
            grid.datagrid('load');
            return input;
        },
        destroy: function (target) {
            $(target).find('.bddatagrid').datagrid('destroy');
        },
        getValue: function (target) {

            return "1";
        },
        setValue: function (target, value) {

        },
        resize: function (target, width) {
            $(target).combo('resize', width);
        }
    },
    dateRange: {
        init: function (container, options) {
            var c = $('<div style="display:inline-block">&nbsp;&nbsp;&nbsp;<input class="d1">&nbsp;&nbsp;&nbsp;<input class="d2"></div>').appendTo(container);
            c.find('.d1,.d2').datebox();
            return c;
        },
        destroy: function (target) {
            $(target).find('.d1,.d2').datebox('destroy');
        },
        getValue: function (target) {
            var d1 = $(target).find('.d1');
            var d2 = $(target).find('.d2');
            return d1.datebox('getValue') + '~' + d2.datebox('getValue');
        },
        setValue: function (target, value) {
            var d1 = $(target).find('.d1');
            var d2 = $(target).find('.d2');
            var vv = value.split('~');
            d1.datebox('setValue', vv[0]);
            d2.datebox('setValue', vv[1]);
        },
        resize: function (target, width) {
            $(target)._outerWidth(width)._outerHeight(18);
            $(target).find('.d1,.d2').datebox('resize', width - 30);
        }
    },
    dateRange1: {
        init: function (container, options) {
            var c = $('<div style="display:inline-block">&nbsp;&nbsp;&nbsp;<input class="d1">&nbsp;&nbsp;~&nbsp;&nbsp;<input class="d2"></div>').appendTo(container);
            c.find('.d1,.d2').datebox();
            return c;
        },
        destroy: function (target) {
            $(target).find('.d1,.d2').datebox('destroy');
        },
        getValue: function (target) {
            var d1 = $(target).find('.d1');
            var d2 = $(target).find('.d2');
            return d1.datebox('getValue') + '~' + d2.datebox('getValue');
        },
        setValue: function (target, value) {
            var d1 = $(target).find('.d1');
            var d2 = $(target).find('.d2');
            var vv = value.split('~');
            d1.datebox('setValue', vv[0]);
            d2.datebox('setValue', vv[1]);
        },
        resize: function (target, width) {
            $(target)._outerWidth(width)._outerHeight(22);
            $(target).find('.d1,.d2').datebox('resize', width / 2 - 20);
        }
    },
    dateRange2: {
        init: function (container, options) {
            var input = $('<input>').appendTo(container);
            input.combo({
                panelWidth: 350,
                panelHeight: 220,
				editable: options.editable || false,
                onShowPanel: function () {
                    var dd = input.combo('getText').split('~');
                    var d1 = $.fn.datebox.defaults.parser(dd[0]);
                    var d2 = $.fn.datebox.defaults.parser(dd[1]);
                    var p = input.combo('panel');
                    p.find('.c1').calendar('moveTo', d1);
                    p.find('.c2').calendar('moveTo', d2);
                }
            });
            input.combo('textbox').parent().addClass('datebox');
            var p = input.combo('panel');
            //$('<div class="c1" style="width:50%;float:left"></div><div class="c2" style="width:50%;float:right"></div>').appendTo(p);
            //var c1 = p.find('.c1').calendar();
            //var c2 = p.find('.c2').calendar();
            //var footer = $('<div>&nbsp;&nbsp;</div>').appendTo(p);
            //var btn = $('<a href="javascript:void(0)" style="text-decoration:none">确定</a>').appendTo(footer);
            //btn.bind('click', function () {
            //    var v1 = $.fn.datebox.defaults.formatter(c1.calendar('options').current);
            //    var v2 = $.fn.datebox.defaults.formatter(c2.calendar('options').current);
            //    var v = v1 + '~' + v2;
            //    input.combo('setValue', v).combo('setText', v);
            //    input.combo('hidePanel');
            //});
            $('<div class="c1" style="width:50%;float:left;"></div><div class="c2" style="width:50%;float:right;"></div>').appendTo(p);
            var c1 = p.find('.c1').calendar();
            var c2 = p.find('.c2').calendar();
            var footer = $('<div style="padding:5px 10px;clear:both;font-size:14px;text-align:center;"><a class="b-ok" href="javascript:void(0);" style="text-decoration:none;margin-left:5px;">确定</a><a class="b-cancel" href="javascript:void(0);" style="text-decoration:none;margin-left:20px;margin-top:10px;">取消</a></div>').appendTo(p);
            footer.find('.b-ok,.b-cancel').bind('click', function () {
                if ($(this).hasClass('b-ok')) {
                    var v1 = $.fn.datebox.defaults.formatter(c1.calendar('options').current);
                    var v2 = $.fn.datebox.defaults.formatter(c2.calendar('options').current);
                    var v = v1 + '~' + v2;
                    input.combo('setValue', v).combo('setText', v);
                    input.combo('hidePanel');
                    if (options.buttons) {
                        var okbtn = options.buttons[0];
                        okbtn.handler.call(input,input[0]);
                    }
                    
                }
                else {
                    input.combo('setValue', '');
                    input.combo('hidePanel');
                    if (options.buttons) {
                        var cancelbtn = options.buttons[1];
                        cancelbtn.handler.call(input, input[0]);
                    }                    
                }
                
            });
            
            return input;
        },
        destroy: function (target) {
            $(target).combo('destroy');
        },
        getValue: function (target) {
            var p = $(target).combo('panel');
            var v1 = $.fn.datebox.defaults.formatter(p.find('.c1').calendar('options').current);
            var v2 = $.fn.datebox.defaults.formatter(p.find('.c2').calendar('options').current);
            return v1 + '~' + v2;
        },
        setValue: function (target, value) {
            var dd = value.split('~');
            if (dd.length == 2) {
                var d1 = $.fn.datebox.defaults.parser(dd[0]);
                var d2 = $.fn.datebox.defaults.parser(dd[1]);
                var p = $(target).combo('panel');
                p.find('.c1').calendar('moveTo', d1);
                p.find('.c2').calendar('moveTo', d2);
            }
            $(target).combo('setValue', value).combo('setText', value);
        },
        resize: function (target, width) {
            $(target).combo('resize', width);
        }
    },
    bdgrid2: {
        init: function (container, options) {
            var input = $('<input>').appendTo(container);
            var config = options.config || {};
            
            input.combo({
                panelWidth: config.panelWidth || 500,
                panelHeight: config.panelHeight || 400,
                editable: options.editable || false,
                onShowPanel: function () {
                    var v = input.combo('getText');
                    
                    var url = config.url || "";
                    var field = config.field || "";
                    var ifrWidth = config.iframeWidth || 500;
                    var ifrHeight = config.iframeHeight || 400;                
                    var p = input.combo('panel');    
                    if (p.html() == "") {
                        var ifrid = field + "datagridifr";
                        var iframe = $("<iframe id=\"" + ifrid + "\" name=\"" + ifrid + "\" width=\"" + ifrWidth + "\" height=\"" + ifrHeight + "\" src=\"" + url + "\"></iframe>").appendTo(p);
                        var footer = $('<div style="padding:5px 10px;clear:both;font-size:14px;text-align:center;"><a class="b-cancel" href="javascript:void(0);" style="text-decoration:none;margin-left:20px;margin-top:10px;">取消</a></div>').appendTo(p);
                        iframe.on('load', function () {
                            var w = window.frames[ifrid];
                            var postfunc = function () {
                                if (w["dataGrid"]) {                                    
                                    clearInterval(itv);
                                    var dg = w["dataGrid"];
                                    var dgoptions = dg.datagrid('options');
                                    dgoptions["onSelect"] = function (index, row) {
                                        var valueField = config.valueField || "";
                                        if (valueField != "") {
                                            var v = row[valueField];
                                            if (v) {
                                                input.combo('setValue', v).combo('setText', v);
                                                if (config.onselect) {
                                                    config.onselect.call(input, input[0]);
                                                }
                                            }
                                        }
                                        input.combo('hidePanel');
                                    };                                    
                                    footer.find('.b-cancel').bind('click', function () {
                                        dg.datagrid('clearSelections');
                                        input.combo('setValue', '');
                                        input.combo('hidePanel');
                                        if (config.oncancel) {
                                            config.oncancel.call(input, input[0]);
                                        }
                                        

                                    });

                                }                                
                            }
                            var itv = setInterval(postfunc, 500);                            
                                                        
                        });                     
                        
                        
                        
                    }
                    
                }
            });          

            return input;
        },
        destroy: function (target) {
            $(target).combo('destroy');
        },
        getValue: function (target) {
            var v = "";
            var p = $(target).combo('pannel');
            
            return v;
        },
        setValue: function (target, value) {
            $(target).combo('setValue', value).combo('setText', value);
        },
        resize: function (target, width) {
            $(target).combo('resize', width);
        }
    }
});