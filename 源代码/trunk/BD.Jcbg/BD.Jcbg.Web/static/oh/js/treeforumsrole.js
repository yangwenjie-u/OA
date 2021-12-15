;
(function($, window, document) {
    var treeData;
    var _that;
    /*
    indeterminate = true; //   第三种选择
    */
    $.fn.setTree = function(option) {
        _that = $(this);
        var data = option.data;
        var fir, sed, third;
        var left = [];
        var right = [];
        var rightStr, leftStr;

        for (var i = 0, len = data.length; i < len; i++) {
            fir = data[i];
            leftStr = '<div class="fir ' + (i == 0 ? 'active' : '') + '" value="' + fir.menucode +
                '"><span class="open">+</span><span class="slide">-</span><input type="checkbox" class="fir-check" val="' +
                fir.menucode +
                '"><span>' + fir.menuname +
                '</span><ul>';

            for (var j = 0, len2 = fir.child.length; j < len2; j++) {
                sed = fir.child[j];
                leftStr += '<li class="sed ' + ((i == 0 && j == 0) ? 'active' : '') + '" value="' + sed.menucode +
                    '"><input type="checkbox" class="sed-check"  val="' + sed.menucode + '"/><span>' + sed.menuname +
                    '</span></li>';

                rightStr = setSed(sed, i == 0 && j == 0);
                right.push(rightStr);
            }
            leftStr += "</ul></div>";
            left.push(leftStr);
        }
        $(this).html("<div class='left'>" + left.join('') + "</div>" + "<div class='right'>" + right.join('') +
            "</div><div class='bottom'><span class='save'>提交</span></div>");

        layIndex = layer.open({
            type: 1,
            title: '添加权限',
            content: $(this),
            // 43+62 105
            area: ['600px', '505px']
        });

        events(_that, option);
        events = function() {}

        //已经选择的数据
        hasPower(data);
    }

    function events(_that, option) {
        _that.on('click', '.slide', function(event) {
            $(this).parents('.fir').removeClass('active')
            event.stopPropagation();
        })

        //点击1级节点
        _that.on('click', '.fir', function(event) {
            $(this).addClass('active').siblings().removeClass('active');
            $(this).find('.sed').eq(0).click();
            event.stopPropagation();
        });
        //选中/取消勾选 1级节点
        _that.on('click', '.fir-check', function() {
            var check, that = $(this),
                val, tmp;
            if (that[0].indeterminate == true) {
                that[0].indeterminate = false;
                check = true;
            } else {
                check = that.is(":checked");
            }
            // var ipts = that.parent().find(".sed-check");
            var ary = that.parent().find(".sed-check");
            var third = _that.find('.third');
            setChecked(ary, check);
            for (var i = 0, len = ary.length; i < len; i++) {
                val = ary.eq(i).attr('val');
                tmp = third.filter('[value="' + val + '"]').find('.third-check');
                setChecked(tmp, check);
            }
        });


        //点击2级节点
        _that.on('click', '.sed', function(event) {
            var that = $(this);
            var val = that.attr('value');

            that.addClass('active').siblings().removeClass('active');
            var third = _that.find('.right').find('[value="' + val + '"]');
            //显示/隐藏3级节点
            third.addClass('active').siblings().removeClass('active');

            event.stopPropagation();

        });
        //选中/取消勾选 2级节点
        _that.on('click', '.sed-check', function() {
            var that = $(this);
            var val = that.attr('val');
            var third = _that.find('.right').find('[value="' + val + '"]');
            var check;
            if (that[0].indeterminate == true) {
                that[0].indeterminate = false;
                check = true;
            } else {
                check = that.is(":checked");
            }
            setChecked(third.find('.third-check'), check);
            var fir = that.parents('.fir') //_that.find('.fir.active');
            var flag = getChecked(fir.find('.sed-check'));
            var tmp = fir.find('.fir-check')[0];
            setParChecked(tmp, flag);


        });
        //点击3级节点
        _that.on('click', '.third-li', function(event) {
            var third = $(this).find(".third-check");
            var check = third.is(":checked");
            var list = $(this).parent().children();
            var tmp, flag = '';
            // third
            var val = $(this).parents('.third').attr('value');

            var sed = _that.find('.left .sed[value="' + val + '"]');
            // var fir = _that.find('.left .fir.active');
            var ipt = sed.find('.sed-check');
            var fir = sed.parents('.fir');
            // var ipt = fir.find(".sed.active").find('.sed-check'); //2级节点
            if (ipt.length) {
                tmp = ipt[0];
                //选中/取消勾选 2级节点
                if (list.length > 1) {
                    var flag = getChecked(list.find(".third-check"));
                    setParChecked(tmp, flag);
                    if (flag == 2) {
                        ipt = fir.find('.fir-check')[0];
                        ipt.indeterminate = true;
                        event.stopPropagation();
                        return;
                    }
                } else {
                    tmp.checked = check;
                }

                var seds = fir.find('.sed');
                if (seds.length) {
                    var ipts = seds.find(".sed-check");
                    flag = getChecked(ipts);
                    ipt = fir.find('.fir-check')[0];
                    //选中/取消勾选 1级节点
                    setParChecked(ipt, flag);
                }

            }
            event.stopPropagation();

        });
        //点击提交按钮
        _that.on('click', '.save', function(event) {

            var list = _that.find(":input");
            var ary = [];
            for (var i = 0, len = list.length; i < len; i++) {
                if (list.eq(i).is(":checked")) {
                    ary.push(list.eq(i).attr("val"));
                } else if (list[i].indeterminate == true) {
                    ary.push(list.eq(i).attr("val"));
                }
            }
            ary = removeDuplicatedItem(ary);

            function removeDuplicatedItem(arr) {
                for (var i = 0; i < arr.length - 1; i++) {
                    for (var j = i + 1; j < arr.length; j++) {
                        if (arr[i] == arr[j]) {
                            arr.splice(j, 1);
                            j--;
                        }
                    }
                }

                return arr;
            }
            var ary2 = [];
            for (var i = 0, len = ary.length; i < len; i++) {
                ary2.push(JSON.stringify({
                    menucode: ary[i]
                }));
            }
            layer.close(layIndex);
            option.save(ary2);
        });

    }


    function hasPower(data) {
        var fir, sed, third, right = _that.find('.right');
        for (var i = 0, len = data.length; i < len; i++) {
            fir = data[i];
            for (var j = 0, len2 = fir.child.length; j < len2; j++) {
                sed = fir.child[j];
                if (sed.child) {
                    for (var k = 0, len3 = sed.child.length; k < len3; k++) {
                        third = sed.child[k];
                        if (third.hasPower) {
                            right.find('[val="' + third.menucode + '"]').attr("checked", 'true').parents('.third-li').click();
                        }
                    }
                } else {
                    if (sed.hasPower) {
                        right.find('[val="' + sed.menucode + '"]').attr("checked", 'true').parents('.third-li').click();
                    }
                }
            }
        }
    }

    function setSed(sed, flag) {
        var third, rightStr;
        if (!sed.child) {
            rightStr = '<div class="third ' + (flag ? 'active' : '') + '" value="' + sed.menucode + '">';
            rightStr += "<div class='third-li'><label><input type='checkbox' class='third-check' val='" + sed.menucode +
                "'/><span>" + sed.menuname +
                "</span></label></div>";
            rightStr += "</div>";
        } else {
            rightStr = '<div class="third ' + (flag ? 'active' : '') + '" value="' + sed.menucode + '">';
            for (var k = 0, len3 = sed.child.length; k < len3; k++) {
                third = sed.child[k];
                rightStr += "<div class='third-li'><label><input type='checkbox' class='third-check' val='" + third.menucode +
                    "'/><span>" + third
                    .menuname +
                    "</span></label></div>";
            }
            rightStr += "</div>";

        }
        return rightStr;
    }
    //返回子节点列表的选中状态
    function getChecked(ipts) {
        var flag = '',
            tmp, val;
        for (var i = 0, len = ipts.length; i < len; i++) {
            if (ipts[0].indeterminate == true) {
                return 2;
            }
            tmp = ipts.eq(i).is(":checked");
            flag += tmp ? '1' : '0';
        }
        if ((flag.indexOf('1') > -1) && (flag.indexOf('0') > -1)) {
            val = 2;
        } else if (flag.indexOf('1') > -1) { //全选
            val = 1;
        } else { //全未选
            val = 0;
        }
        return val;
    }
    //根据选中状态 设置父checkbox状态
    function setParChecked(ipt, flag) {
        switch (flag) {
            case 0:
                ipt.indeterminate = false;
                ipt.checked = false;
                break;
            case 1:
                ipt.indeterminate = false;
                ipt.checked = true;
                break;
            case 2:
                ipt.indeterminate = true;
                break;
        }
    }

    //选中/取消选中 子节点
    function setChecked(ipts, check) {
        for (var i = 0, len = ipts.length; i < len; i++) {
            ipts[i].indeterminate = false;
            ipts[i].checked = check;
        }
    }
})(jQuery, window, document);