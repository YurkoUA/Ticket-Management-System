function searchModal() {
    $.get("/Ticket/SearchModal", function (data) {
        showModal(data);
    });
}

function OnBegin() {
    $("#search-result").html("");
    setButtonLoadingState();
}

function OnComplete(request, status) {
    resetButtonLoadingState();
}
