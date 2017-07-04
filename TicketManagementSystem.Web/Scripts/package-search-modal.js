function searchModal() {
    $.get("/Package/SearchModal", function (data) {
        showModal(data);
    });
}