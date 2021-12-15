//$.extend($.fn.datagrid.defaults.filters, {
//    dateRange: {
//        init: function(container, options) {
//            var c = $('<div style="display:inline-block">&nbsp;&nbsp;&nbsp;<input class="d1">&nbsp;&nbsp;&nbsp;<input class="d2"></div>').appendTo(container);
//            c.find('.d1,.d2').datebox().datebox(options);

//            return c;
//        },
//        destroy: function(target) {
//            $(target).find('.d1,.d2').datebox('destroy');
//        },
//        getValue: function(target) {
//            var d1 = $(target).find('.d1');
//            var d2 = $(target).find('.d2');
//            return d1.datebox('getValue') + ':' + d2.datebox('getValue');
//        },
//        setValue: function(target, value) {
//            var d1 = $(target).find('.d1');
//            var d2 = $(target).find('.d2');
//            var vv = value.split(':');
//            d1.datebox('setValue', vv[0]);
//            d2.datebox('setValue', vv[1]);
//        },
//        resize: function(target, width) {
//            $(target)._outerWidth(width)._outerHeight(22);
//            $(target).find('.d1,.d2').datebox('resize', width);
//        }
//    }
//});

$.extend($.fn.datagrid.defaults.filters, {
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
            $('<div class="c1" style="width:50%;float:left"></div><div class="c2" style="width:50%;float:right"></div>').appendTo(p);
            var c1 = p.find('.c1').calendar();
            var c2 = p.find('.c2').calendar();
            var footer = $('<div>&nbsp;&nbsp;</div>').appendTo(p);
            var btn = $('<a href="javascript:void(0)" style="text-decoration:none">确定</a>').appendTo(footer);
            btn.bind('click', function () {
                var v1 = $.fn.datebox.defaults.formatter(c1.calendar('options').current);
                var v2 = $.fn.datebox.defaults.formatter(c2.calendar('options').current);
                var v = v1 + '~' + v2;
                input.combo('setValue', v).combo('setText', v);
                input.combo('hidePanel');
            })
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
            var dd = value.split(':');
            var d1 = $.fn.datebox.defaults.parser(dd[0]);
            var d2 = $.fn.datebox.defaults.parser(dd[1]);
            var p = $(target).combo('panel');
            p.find('.c1').calendar('moveTo', d1);
            p.find('.c2').calendar('moveTo', d2);
            $(target).combo('setValue', value).combo('setText', value);
        },
        resize: function (target, width) {
            $(target).combo('resize', width);
        }
    }
});

$.extend($.fn.datagrid.defaults.operators, {
    between: {
        text: 'Between',
        isMatch: function (source, value) {
            var dd = value.split('~');
            var d1 = $.fn.datebox.defaults.parser(dd[0]);
            var d2 = $.fn.datebox.defaults.parser(dd[1]);
            var d = $.fn.datebox.defaults.parser(source);
            return d >= d1 && d <= d2;
        }
    }
});