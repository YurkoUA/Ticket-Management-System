function OnPaginationBegin() {
    $("#packages-list-table").html("");
}

function OnPaginationSuccess(data) {
    window.scroll(0, 0);

    var currentPageNumber = $("#PageNumber").val();

    if (currentPageNumber != undefined) {
        changeUrlParams([
            { name: "page", value: currentPageNumber }
        ]);
    }
}

function OnTabPaginationSuccess(data) {
    var tabParameter = $("#SelectedTab").val();
    
    if (tabParameter != undefined) {
        changeUrlParams([
            { name: "tab", value: tabParameter }
        ]);

        $("li[role=presentation]").removeClass("active");
        $("li[role=presentation][data-tab-parameter=" + tabParameter + "]").addClass("active");
    }

    OnPaginationSuccess(data);
}
