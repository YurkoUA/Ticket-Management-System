function OnPaginationSuccess(data) {
    window.scroll(0, 0);

    var currentPageNumber = $("#PageNumber").val();

    if (currentPageNumber != undefined) {
        var params = $.deparam.querystring(location.search);

        if (params != undefined) {
            params.page = currentPageNumber;
            history.pushState(null, '', location.pathname + "?" + $.param(params));
        }
    }
}

function OnPaginationBegin() {
    $("#tickets-list-table").html("");
}