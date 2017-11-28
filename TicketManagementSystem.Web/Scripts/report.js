$(function () {
    $("#save-report-button").click(function () {
        if (confirm("Ви впевнені?") === true) {
            if (confirm("Точно???") === true) {
                $("form#save-report-form").submit();
            }
        }
    });
})();