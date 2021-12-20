var store = {
    save: function (key, value) {
        localStorage.setItem(key, JSON.stringify(value));
    },
    fetch: function (key) {
        return JSON.parse(localStorage.getItem(key)) || [];
    },
    remove: function (key) {
        localStorage.removeItem(key);
    },
    clear: function () {
        localStorage.clear()
    }
}
var g_roles = store.fetch('roleButton')
function showRoleButton(){
    $('.roleButton').hide();
    console.log(111)

    g_roles.forEach(function (e) {
        $('.roleButton').each(function () {
            var btncode=$(this).attr('data-btncode')
            if (e.btncode == btncode&&e.btntype=='1') {
                console.log(e,btncode);
                $(this).show()
            }
        })
    })
}
$(function(){
    showRoleButton()
    // console.log(g_roles);
})