function searchModal() {
    $.get("/Ticket/SearchModal", function (data) {
        showModal(data);
    });
}