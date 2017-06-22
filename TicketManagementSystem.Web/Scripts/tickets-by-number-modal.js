$(function () {
    $('#tickets-with-number').click(function () {
        id = $('#Id').val();

        $.get("/Ticket/ClonesWith", { id: id }, function (data) {
            showModal(data);
        })
    })
})