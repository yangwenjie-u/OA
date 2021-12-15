/**
* jQuery EasyUI 1.4.2
*
* Copyright (c) 2009-2015 www.jeasyui.com. All rights reserved.
*
* Licensed under the GPL license: http://www.gnu.org/licenses/gpl.txt
* To use it on other terms please contact us at info@jeasyui.com
*
*/

(function ($) {

    /**
    * Add:
    */
    function createRoot(target) {

        var state = $.data(target, 'daterangebox');
        var opts = state.options;

        //Add:
        var leftRoot, rightRoot;

        //

        if (!state.topbar) {
            var panel = $(target).combo('panel').css('overflow', 'hidden');
            state.topbar = $('<table cellspacing="0" cellpadding="0" style="width:100%;"><tr></tr></table>').appendTo(panel);
            var tr = state.topbar.find('tr');
            var td1 = $('<td></td>').appendTo(tr).css('width', '60%');
            var td2 = $('<td></td>').appendTo(tr).css('width', '40%');

            //td1 ->
            state.leftInput = $('<input>'), state.rightInput = $('<input>');
            $.each([state.leftInput, state.rightInput], function (i, n) {
                if (i == 1) {
                    $('<span>-</span>').css({ 'margin': '0 5px' }).appendTo(td1);
                }
                n.css('width', 80).appendTo(td1);
                n.textbox();
                n.textbox('readonly', true);
            });

            //td2 ->
            state.zConfirmBtn = $('<a href="javascript:void(0);">ȷ��</a>'),
            state.zCancelBtn = $('<a href="javascript:void(0);">ȡ��</a>');
            $.each([state.zConfirmBtn, state.zCancelBtn], function (i, n) {
                n.css({ 'float': 'right', 'width': '60px' }).appendTo(td2);
                n.linkbutton();
            });

            state.zConfirmBtn.on('click', function () {
                var lv = state.leftInput.textbox('getValue');
                var rv = state.rightInput.textbox('getValue');
                if (lv != '' && rv != '') {
                    var lt = new Date(lv.replace(/-/g, '/')).getTime(),
                        rt = new Date(rv.replace(/-/g, '/')).getTime();
                    if (rt >= lt) {
                        $(target).combo('setValue', lv + ' - ' + rv).combo('setText', lv + ' - ' + rv);
                        //
                        $(target).combo('hidePanel');
                    } else {
                        state.rightInput.textbox('textbox').parent('span').addClass('daterangebox-alert-border');
                    }
                }
            });
            state.zCancelBtn.on('click', function () {
                $(target).combo('hidePanel');
            });
        }


        /**
        * if the calendar isn't created, create it.
        * ��������
        */
        if (!state.calendar) {

            var panel = $(target).combo('panel').css('overflow', 'hidden');

            //Add:
            leftRoot = $('<div class="daterangebox-calendar-left"></div>').css('float', 'left').appendTo(panel);

            //Update:
            var cc = $('<div class="daterangebox-calendar-inner"></div>').prependTo(leftRoot);

            state.calendar = $('<div></div>').appendTo(cc).calendar();

            $.extend(state.calendar.calendar('options'), {
                fit: true,
                //border:false,
                border: true,
                onSelect: function (date) {
                    var target = this.target;
                    var opts = $(target).daterangebox('options');
                    setValueLeft(target, opts.formatter.call(target, date));

                    //���������,�����������.
                    //$(target).combo('hidePanel');

                    opts.onSelect.call(target, date);
                }
            });
        }

        //state.calendar
        if (!state.calendarRight) {

            var panel = $(target).combo('panel').css('overflow', 'hidden');

            //Add:
            rightRoot = $('<div class="daterangebox-calendar-right"></div>').css('float', 'left').appendTo(panel);

            //Update:
            var cc = $('<div class="daterangebox-calendar-inner"></div>').prependTo(rightRoot);

            state.calendarRight = $('<div></div>').appendTo(cc).calendar();

            $.extend(state.calendarRight.calendar('options'), {
                fit: true,
                //border:false,
                border: true,
                onSelect: function (date) {
                    var target = this.target;
                    var opts = $(target).daterangebox('options');
                    setValueRight(target, opts.formatter.call(target, date));

                    //���������,�����������.
                    //$(target).combo('hidePanel');

                    opts.onSelect.call(target, date);
                }
            });
        }

    }

    /**
    * create date box
    */
    function createBox(target) {

        var state = $.data(target, 'daterangebox');
        var opts = state.options;

        //��'onShowPanel'����ʱ��ִ��һϵ�еĺ���.
        $(target).addClass('daterangebox-f').combo($.extend({}, opts, {
            onShowPanel: function () {
                //bindEvents(this);
                //setButtons(this);
                setCalendar(this);
                setValue(this, $(this).daterangebox('getText'), true);
                opts.onShowPanel.call(this);
            },
            required: true
        }));
        //
        $(target).combo('textbox').attr('readonly', true);

        createRoot(target);

        $(target).combo('textbox').parent().addClass('daterangebox');
        //$(target).daterangebox('initValue', opts.value);

        //
        function setCalendar(target) {
            var panel = $(target).combo('panel');

            var leftDiv = panel.children('div.daterangebox-calendar-left');
            var rightDiv = panel.children('div.daterangebox-calendar-right');

            var cc = $(leftDiv).children('div.daterangebox-calendar-inner');
            var ccRight = $(rightDiv).children('div.daterangebox-calendar-inner');

            //_outerWidth ��jquery.parser.js���ж���.
            //panel.children()._outerWidth(panel.width());
            panel.children().not('table')._outerWidth(panel.width() / 2);

            state.calendar.appendTo(cc);
            state.calendarRight.appendTo(ccRight);

            //important
            state.calendar[0].target = target;
            state.calendarRight[0].target = target;


            if (opts.panelHeight != 'auto') {
                var height = panel.height();
                //panel.children().not(cc).each(function(){
                $(leftDiv).children().not(cc).each(function () {
                    height -= $(this).outerHeight();
                });

                cc._outerHeight(height);
                ccRight._outerHeight(height);

            }

            state.calendar.calendar('resize');
            state.calendarRight.calendar('resize');
        }
    }

    function controlRightCalendar(state, leftTime) {
        //�ұߵ����ڱ�����ڻ������ߵ�����.
        state.calendarRight.calendar({
            validator: function (date) {
                if (date.getTime() >= leftTime) {
                    return true;
                }
                return false;
            },
            styler: function (date) {
                if (date.getTime() >= leftTime) {
                    return '';
                }
                return 'color:#cccccc';
            }
        });
        $.parser.parse(state.calendarRight);
    }

    function setValueLeft(target, value, remainText) {
        var state = $.data(target, 'daterangebox');
        var opts = state.options;

        state.calendar.calendar('moveTo', opts.parser.call(target, value));
        state.leftInput.textbox('setValue', value);
        //
        var leftTime = new Date(value.replace(/-/g, '/')).getTime();
        controlRightCalendar(state, leftTime);
    }

    function setValueRight(target, value, remainText) {
        var state = $.data(target, 'daterangebox');
        var opts = state.options;

        state.calendarRight.calendar('moveTo', opts.parser.call(target, value));
        state.rightInput.textbox('setValue', value);
        state.rightInput.textbox('textbox').parent('span').removeClass('daterangebox-alert-border');
    }

    /**
    *
    */
    function setValue(target, value, remainText) {
        var state = $.data(target, 'daterangebox');
        var opts = state.options;
        var leftVal = '', rightVal = '';

        if (value) {
            var valArr = value.split(' - ');
            leftVal = $.trim(valArr[0]);
            rightVal = $.trim(valArr[1]);
        }

        var leftDate = opts.parser.call(target, leftVal), ld = leftDate;
        //
        controlRightCalendar(state, new Date(ld.getFullYear() + '/' + (ld.getMonth() + 1) + '/' + ld.getDate()).getTime());
        state.calendar.calendar('moveTo', leftDate);
        state.calendarRight.calendar('moveTo', opts.parser.call(target, rightVal));

    }

    /**
    * ���췽��.
    */
    $.fn.daterangebox = function (options, param) {

        if (typeof options == 'string') {
            var method = $.fn.daterangebox.methods[options];
            if (method) {
                //�� method ��"daterangebox"����ķ�����,ֱ�ӵ���.
                return method(this, param);
            } else {
                //����,����combo��Ӧ�ķ���.
                return this.combo(options, param);
            }
        }

        options = options || {};

        return this.each(function () {

            var state = $.data(this, 'daterangebox');

            if (state) {
                $.extend(state.options, options);
            } else {
                //��Ԫ���ϴ��(set)����. ����'cloneFrom'������,�����ط����ǻ�ȡ(get).
                $.data(this, 'daterangebox', {
                    // ���� $.fn.daterangebox.defaults ����.
                    options: $.extend({}, $.fn.daterangebox.defaults, $.fn.daterangebox.parseOptions(this), options)
                });
            }
            //
            createBox(this);
        });
    };

    $.fn.daterangebox.methods = {

        //���Է���,'options'�������ڲ����÷ǳ�Ƶ��.
        options: function (jq) {
            var copts = jq.combo('options');
            return $.extend($.data(jq[0], 'daterangebox').options, {
                width: copts.width,
                height: copts.height,
                originalValue: copts.originalValue,
                disabled: copts.disabled,
                readonly: copts.readonly
            });
        },

        //��¶���û�ʹ�õ�API.���calendar����.
        calendar: function (jq) { // get the calendar object
            return $.data(jq[0], 'daterangebox').calendar;
        },

        initValue: function (jq, value) {
            return jq.each(function () {
                var opts = $(this).daterangebox('options');
                var value = opts.value;
                if (value) {
                    //�����ж���'formatter','parser'����.
                    value = opts.formatter.call(this, opts.parser.call(this, value));
                }
                //���ջ��ǻ����'combo'�ķ���, ���һ�����'setText'����
                $(this).combo('initValue', value).combo('setText', value);
            });
        },
        setValue: function (jq, value) {
            return jq.each(function () {
                //���ñ�ģ�鶨���'setValue'����.
                setValue(this, value);
            });
        },
        reset: function (jq) {
            return jq.each(function () {
                var opts = $(this).daterangebox('options');
                // opts.originalValue -> ����combo -> Ϊ���ַ���.
                $(this).daterangebox('setValue', opts.originalValue);
            });
        }
    };

    //�˷������ṩ������ʹ��.Ʃ�������"$.fn.combo.parseOptions(target)".
    $.fn.daterangebox.parseOptions = function (target) {
        return $.extend({}, $.fn.combo.parseOptions(target), $.parser.parseOptions(target));
    };

    $.fn.daterangebox.defaults = $.extend({}, $.fn.combo.defaults, {
        //panelWidth:180,
        panelWidth: 400,
        panelHeight: 'auto',

        /*currentText:'Today',
        closeText:'Close',*/

        formatter: function (date) {
            var y = date.getFullYear();
            var m = date.getMonth() + 1;
            var d = date.getDate();
            return (m < 10 ? ('0' + m) : m) + '/' + (d < 10 ? ('0' + d) : d) + '/' + y;
        },
        parser: function (s) {
            if (!s) return new Date();
            var ss = s.split('/');
            var m = parseInt(ss[0], 10);
            var d = parseInt(ss[1], 10);
            var y = parseInt(ss[2], 10);
            if (!isNaN(y) && !isNaN(m) && !isNaN(d)) {
                return new Date(y, m - 1, d);
            } else {
                return new Date();
            }
        },

        onSelect: function (date) { }
    });
})(jQuery);