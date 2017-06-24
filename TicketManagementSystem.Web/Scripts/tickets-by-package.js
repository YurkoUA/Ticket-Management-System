$(function () {
    $('#tickets-by-package-button').click(function () {
        var id = $('#Id').val();
        $.get("/Package/Tickets", { id: id }, function (data) {
            showModal(data);
        });
    });
});