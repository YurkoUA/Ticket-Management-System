function OnPaginationSuccess(data) {
    window.scroll(0, 0);

    var currentPageNumber = $("#PageNumber").val();

    if (currentPageNumber != undefined) {
        changeUrlParams([
            { name: "page", value: currentPageNumber }
        ]);
    }
}

function OnPaginationBegin() {
    $("#tickets-list-table").html("");
}