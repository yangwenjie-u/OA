function selectQY() {
    try {

        var selected = pubselect();
        if (selected == undefined)
            return;

        parent.$('#qybh').val(selected.QYBH);
        parent.$('#qymc').val(selected.QYMC);
        parent.$('#qyzz').val(selected.ZZDJ);
        parent.$('#zzzh').val(selected.ZZZSBH);
        parent.$('#qyfr').val(selected.QYFR);
        parent.$('#lxdh').val(selected.LXDH);
        parent.layer.closeAll()

    } catch (e) {
        alert(e);
    }
}
