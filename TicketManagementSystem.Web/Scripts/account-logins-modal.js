$(document).ready(function () {
    $('#login-history-button').click(function () {
        $.get("/Account/LoginHistory", function (data) {
            showModal(data);
        });
    });
});