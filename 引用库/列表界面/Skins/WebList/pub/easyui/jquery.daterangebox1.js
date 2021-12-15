$.extend($.fn.datagrid.defaults.filters, {
    dateRange: {
        init: function(container, options) {
            var c = $('<div style="display:inline-block"><input class="d1">至<input class="d2"></div>').appendTo(container);
            c.find('.d1,.d2').datebox();
            return c;
        },
        destroy: function(target) {
            $(target).find('.d1,.d2').datebox('destroy');
        },
        getValue: function(target) {
            var d1 = $(target).find('.d1');
            var d2 = $(target).find('.d2');
            return d1.datebox('getValue') + ':' + d2.datebox('getValue');
        },
        setValue: function(target, value) {
            var d1 = $(target).find('.d1');
            var d2 = $(target).find('.d2');
            var vv = value.split(':');
            d1.datebox('setValue', vv[0]);
            d2.datebox('setValue', vv[1]);
        },
        resize: function(target, width) {
            $(target)._outerWidth(width)._outerHeight(22);
            $(target).find('.d1,.d2').datebox('resize', width / 2);
        }
    }
});