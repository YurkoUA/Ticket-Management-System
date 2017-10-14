$(function () {
    $('#unallocated-tickets').click(function () {
        $.get("/Package/MoveUnallocatedModal", {}, function (data) {
            showModal(data);
            loadPartial();
        });
    });
});

function loadPartial() {
    var id = $("#Id").val();

    $('#move-result').html('');

    $("#ticket-list").hide();
    $("#loading-message").show();

    $.get("/Package/MoveUnallocatedTickets", { id: id }, function (data) {
        $('#ticket-list').html(data);

        $("#ticket-list").show();
        $("#loading-message").hide();
    });
}

function onMoveUnallocatedStarted() {
    setButtonLoadingState();
}

function onMoveUnallocatedComplete(request, status) {
    resetButtonLoadingState();

    $("#loading-message").show();
    setTimeout(loadPartial, 3000);
}
