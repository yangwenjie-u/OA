;
(function($, window, document) {
    var treeData = {};
    var _that;

    $.fn.setTree = function(option) {
        _that = $(this);
        var data = option.data;
        var edit = option.edit;

        var left = makeLeft(data);
        var str = edit == 0 ? '':"<div class='bottom'><span class='save'>提交</span><span class='close'>取消</span></div>";

        $(this).html("<div class='left'>" + left.join('') + "</div>" + str);
        //选中父节点
        checkedData();

        events(_that, option);
        events = function() {}

    }

    function makeLeft(data) {
        var left = [];
        var fir, sed, third, child, leftStr, checked;
        for (var i = 0, len = data.length; i < len; i++) {
            fir = data[i];
            console.log('fir:',fir);
            leftStr = '<div class="fir" value="' + fir.menucode +
                '"><span class="open">+</span><span class="slide">-</span><input type="checkbox" class="fir-check" id="' + fir.menucode + '" /><label for="' + fir.menucode + '">' + fir.menuname +
                '</label><ul>';
            if (fir.child) {
                
                for (var j = 0, len2 = fir.child.length; j < len2; j++) {
                    sed = fir.child[j];
                    leftStr += '<li class="sed" value="' + sed.menucode + '">';
                    child = '';
                    if (sed.child) {

                        leftStr += "<span class='open'>+</span><span class='slide'>-</span><input type='checkbox' class='sed-check' id='" + sed.menucode + "'><span class='user-list' num='" + sed.usernum + "'></span><label for='" + sed.menucode + "'>" + sed.menuname + "</label><span class='power-list' num='" + sed.rolenum + "'></span>";
                        child = "<ul class='third'>";
                        for (var k = 0, len3 = sed.child.length; k < len3; k++) {
                            third = sed.child[k];
                            checked = third.hasPower ? 'checked=true' : '';
                            child += "<li value='" + third.menucode + "'><input type='checkbox' class='third-check' " + checked + " id='" + third.menucode + "' /><span class='user-list' num='" + third.usernum + "'></span><label for='" + third.menucode + "'>" + third.menuname + "</label><span class='power-list' num='" + third.rolenum + "'></span></li>";
                        }
                        child += "</ul>";
                        leftStr += child;
                    } else {
                        checked = sed.hasPower ? 'checked=true' : '';
                        leftStr += "<input type='checkbox' class='sed-check' " + checked + " id='" + sed.menucode + "' /><span class='user-list' num='" + sed.usernum + "'></span><label for='" + sed.menucode + "'>" + sed.menuname + "</label><span class='power-list' num='" + sed.rolenum + "'></span>";
                    }
                    leftStr += '</li>';
                }
            }
            leftStr += "</ul></div>";
            left.push(leftStr);
        }
        return left;
    }
    /*
        function makeRight(data) {
            var right = [];
            var fir, sed, third, child, rightStr;
            for (var i = 0, len = data.length; i < len; i++) {
                fir = data[i];

                rightStr = '<div class="fir ' + (i == 0 ? 'active' : '') + '" value="' + fir.menucode +
                    '"><span class="open">+</span><span class="slide">-</span><span>' + fir.menuname +
                    '</span><ul>';
                if (fir.child) {

                    for (var j = 0, len2 = fir.child.length; j < len2; j++) {
                        sed = fir.child[j];
                        rightStr += '<li class="sed ' + ((i == 0 && j == 0) ? 'active' : '') + '" value="' + sed.menucode + '">';
                        child = '';
                        if (sed.child) {
                            rightStr += "<span class='open'>+</span><span class='slide'>-</span><span>" + sed.menuname + "</span>";
                            child = "<ul class='third'>";
                            for (var k = 0, len3 = sed.child.length; k < len3; k++) {
                                third = sed.child[k];
                                child += "<li value='" + third.menucode + "'><span>" + third.menuname + "</span></li>";
                            }
                            child += "</ul>";
                            rightStr += child;
                        } else {
                            rightStr += "<span>" + sed.menuname + "</span>";
                        }
                        rightStr += '</li>';
                    }
                }
                rightStr += "</ul></div>";
                right.push(rightStr);
            }
            return right;

        }
    */
    function checkedData() {
        var third = _that.find('.left .third');
        var tmp, check;
        for (var i = 0, len = third.length; i < len; i++) {
            tmp = third.eq(i);
            check = tmp.find(":checked");
            if (check.length == tmp.children().length) {
                tmp.siblings('.sed-check')[0].checked = true;
            } else if (check.length > 0) {
                tmp.siblings('.sed-check')[0].indeterminate = true;
                tmp.parents('.fir').find('.fir-check')[0].indeterminate = true;
            }
        }
        var fir = _that.find('.left .fir');
        for (var i = 0, len = fir.length; i < len; i++) {
            tmp = fir.eq(i);
            check = tmp.find(".sed-check:checked");
            if (check.length == tmp.find('.sed-check').length) {
                tmp.find('.fir-check')[0].checked = true;
            } else if (check.length > 0) {
                tmp.find('.fir-check')[0].indeterminate = true;
            }
        }
    }


    function events(_that, option) {

        //点击1级节点
        _that.on('click', '.fir', function(event) {
            $(this).toggleClass('active').siblings().removeClass('active');
            $(this).find('.sed').eq(0).click();
            event.stopPropagation();
        });
        //选中/取消 1级节点
        _that.on('click', '.fir-check', function(event) {
            var val = $(this).is(":checked");
            var ipt = $(this).siblings('ul').find(":input");
            for (var i = 0, len = ipt.length; i < len; i++) {
                ipt[i].indeterminate = false;
                ipt[i].checked = val;
            }
            event.stopPropagation();
        });

        //点击2级节点
        _that.on('click', '.sed', function(event) {
            var ul = $(this).find('ul');
            $(this).addClass('active').siblings().removeClass('active');
            event.stopPropagation();
        });
        _that.on('click', '.sed .slide', function(event) {
            $(this).parents('.sed').removeClass('active');
            event.stopPropagation();
        });
        _that.on('click', '.sed .open', function(event) {
            $(this).parents('.sed').addClass('active').siblings().removeClass('active');
            event.stopPropagation();
        });
        //选中 取消2级节点
        _that.on('click', '.sed-check', function(event) {
            var that = $(this);
            var check;
            if (that[0].indeterminate == true) {
                that[0].indeterminate = false;
                check = true;
            } else {
                check = that.is(":checked");
            }
            var third = that.siblings('ul').find('.third-check');
            setChecked(third, check);
            var fir = that.parents('.fir');
            var flag = getChecked(fir.find('.sed-check'));
            var tmp = fir.find('.fir-check')[0];
            setParChecked(tmp, flag);
            event.stopPropagation();
        })


        //点击3级节点
        _that.on('click', '.third-check', function(event) {
            var check = $(this).is(":checked");
            var tmp, flag = '';
            var val = $(this).parents('.third').attr('value');
            var sed = $(this).parents('.sed');
            var list = sed.find('.third').find(":input");

            var ipt = sed.find('.sed-check');
            var fir = sed.parents('.fir');

            if (ipt.length) {
                tmp = ipt[0];
                //选中/取消勾选 2级节点
                if (list.length > 1) {
                    var flag = getChecked(list);
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
                    ary.push(list.eq(i).attr("id"));
                } else if (list[i].indeterminate == true) {
                    ary.push(list.eq(i).attr("id"));
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
            layer.closeAll();
            option.save(ary2);
        });

         _that.on('click', '.close', function(event) {
            layer.closeAll();
        });
    }
    /*
        function setRight(sed, flag) {
            var third, rightStr;
            if (!sed.child) {
                rightStr = '<div class="third " value="' + sed.menucode + '">';
                rightStr += "<div class='third-li'><span>查看所属角色</span></div>";
                rightStr += "<div class='third-li'><span>查看拥有人员</span></div>";
                rightStr += "</div>";
            } else {
                rightStr = '<div class="third ' + (flag ? 'active' : '') + '" value="' + sed.menucode + '">';
                for (var k = 0, len3 = sed.child.length; k < len3; k++) {
                    third = sed.child[k];
                    rightStr += "<div class='third-li'><span>" + third.menuname +
                        "</span></div>";
                }
                rightStr += "</div>";
            }
            return rightStr;
        }
        //获取已有权限
        function getSelectData(ary) {
            var fir, sed, third, tmp;
            var data = [];
            for (var i = 0, len = ary.length; i < len; i++) {
                fir = ary[i];
                if (fir.child) {
                    for (var j = 0, len2 = fir.child.length; j < len2; j++) {
                        sed = fir.child[j];
                        if (sed.hasPower) {
                            data.push($.extend(true, {}, ary[i]));
                            break;
                        }
                        if (sed.child) {
                            tmp = 0;
                            for (var k = 0, len3 = sed.child.length; k < len3; k++) {
                                third = sed.child[k];
                                if (third.hasPower) {
                                    data.push($.extend(true, {}, ary[i]));
                                    tmp = 1;
                                    break;
                                }
                            }
                            if (tmp) {
                                break;
                            }
                        }
                    }
                }
            }

            for (var i = 0, len = data.length; i < len; i++) {
                fir = ary[i];
                if (fir.child) {
                    for (var j = 0, len2 = fir.child.length; j < len2; j++) {
                        sed = fir.child[j];
                        if (sed.child) {
                            for (var k = 0, len3 = sed.child.length; k < len3; k++) {
                                third = sed.child[k];
                                if (!third.hasPower) {
                                    sed.child.splice(k, 1);
                                    k--;
                                    len3--;
                                }
                            }
                        } else {
                            if (!sed.hasPower) {
                                fir.child.splice(j, 1);
                                j--;
                                len2--;
                            }
                        }
                    }
                }
            }

            return data;
        }
    */
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