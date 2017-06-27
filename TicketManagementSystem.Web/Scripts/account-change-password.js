$(document).ready(function () {
    $('#change-password-button').click(function () {
        $.get("/Account/ChangePassword", function (data) {
            showModal(data);
        });
    });
});

function OnBegin() {
    $('#change-result').html('');
}