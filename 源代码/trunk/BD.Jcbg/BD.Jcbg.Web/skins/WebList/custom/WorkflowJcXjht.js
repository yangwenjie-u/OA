function selectQy() {
    try {

        var selected = pubselect();
        if (selected == undefined)
            return;

        parent.$('#khdwbh').val(selected.QYBH);
        parent.$('#khdwzh').val(selected.ZH);
        parent.$('#khdwmc').val(selected.QYMC);
        parent.layer.closeAll()

    } catch (e) {
        alert(e);
    }
}

function selectGc() {
    try {

        var selected = pubselect();
        if (selected == undefined)
            return;

        parent.$('#gcmc').val(selected.GCMC);
        parent.$('#gcbh').val(selected.GCBH);
        parent.$('#zjdjh').val(selected.ZJDJH);
        parent.layer.closeAll()

    } catch (e) {
        alert(e);
    }
}

function selectJcjg(){
    try {

        var selected = pubselect();
        if (selected == undefined)
            return;

        parent.$('#fbjcjgbh').val(selected.QYBH);
        parent.$('#fbjcjgmc').val(selected.QYMC);

        parent.$("#frm_jcxm").attr("src", "/jc/jcxmxz?fbxm=true&limitxmbh=" + parent.$("#syxmbh").val() + "&dwbh=" + selected.QYBH);        
        parent.$("#frm_qyzz").attr("src", "/WebList/EasyUiIndex?FormDm=WorkflowQyzz&FormStatus=0&FormParam=PARAM--" + selected.QYBH);

        parent.layer.closeAll()

    } catch (e) {
        alert(e);
    }
}

function selectSyry() {
    try {

        var selected = pubselect();
        if (selected == undefined)
            return;

        parent.$('#syr').append("<option value='" + selected.RYBH + "'>" + selected.RYXM+ "</option>");

        parent.layer.closeAll()

    } catch (e) {
        alert(e);
    }
}

function selectHt() {
    try {

        var selected = pubselect();
        if (selected == undefined)
            return;

        parent.$('#gcbh').val(selected.GCBH);
        parent.$('#khdwbh').val(selected.KHDWBH);
        parent.$('#syxmbh').val(selected.SYXMBH);
        parent.$('#syxmmc').val(selected.SYXMMC);
        parent.$('#htbh').val(selected.HTBH);
        parent.$('#fbhtbh').val(selected.FBHTBH);

        parent.$('#khdwmc').val(selected.KHDWMC);
        parent.$('#gcmc').val(selected.GCMC);

        parent.$("#frm_jcxm").attr("src", "/jc/jcxmxz?fbxm=false&limitxmbh=" + parent.$("#syxmbh").val() + "&dwbh=" + parent.$('#jcjgbh').val());

        $.ajax({
            type: "POST",
            url: "/jc/getnbhtbh",
            dataType: "json",
            data: "htbh=" + parent.$("#htbh").val(),
            async: false,
            success: function (data) {
                if (data.code != "0") {
                    alert(data.msg);
                } else {
                    parent.$("#jchtbh").val(data.msg);
                }
            },
            complete: function (XMLHttpRequest, textStatus) {
            },
            beforeSend: function (XMLHttpRequest) {
            }
        });

        parent.layer.closeAll()

    } catch (e) {
        alert(e);
    }
}