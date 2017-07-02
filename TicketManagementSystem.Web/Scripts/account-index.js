$(document).ready(function () {
    $('#change-password-button').click(function () {
        $.get("/Account/ChangePassword", function (data) {
            showModal(data);
        });
    });

    $('#login-history-button').click(function () {
        $.get("/Account/LoginHistory", function (data) {
            showModal(data);
        });
    });
});

function OnBegin() {
    $('#change-result').html('');
}