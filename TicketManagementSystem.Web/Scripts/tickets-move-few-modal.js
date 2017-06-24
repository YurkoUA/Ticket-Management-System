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
    $.get("/Package/MoveUnallocatedTickets", { id: id }, function (data) {
        $('#ticket-list').html(data);
        $('#move-result').html('');
    });
}

function OnSuccess(data) {
    loadPartial();
}

function OnBegin() {
    $('#move-result').html('');
}