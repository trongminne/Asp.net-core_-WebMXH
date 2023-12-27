
var checkkhoa = document.querySelectorAll("#checkkhoa");

function checkAll(myCheckbox) {
    if (myCheckbox.checked == true) {
        checkkhoa.forEach(function (checkbox) {
            checkbox.checked = true;
        });
    } else {
        checkkhoa.forEach(function (checkbox) {
            checkbox.checked = false;
        });
    }
}
