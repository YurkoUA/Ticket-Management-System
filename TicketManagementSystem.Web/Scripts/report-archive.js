$(document).ready(function () {
    $("div.collapse").on("show.bs.collapse", function () {
        var key = $(this).attr("data-reports-key");
        $("button[data-reports-key=" + key + "]").addClass("list-group-item-info");
    });

    $("div.collapse").on("hidden.bs.collapse", function () {
        var key = $(this).attr("data-reports-key");
        $("button[data-reports-key=" + key + "]").removeClass("list-group-item-info");
    });
});