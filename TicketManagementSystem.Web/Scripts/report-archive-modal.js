$(document).ready(function () {
    $('#archive-button').click(function () {
        $.get("/Report/Archive", function (data) {
            showModal(data);
        });
    });
});