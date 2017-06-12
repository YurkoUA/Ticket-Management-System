$(function () {
    $(function () {
        load();
    })

    $('#ColorId').change(function () {
        load();
    })

    $('#SerialId').change(function () {
        load();
    })

    $('#Number').change(function () {
        load();
    })

    var load = function () {
        loadPackagesList($('#ColorId').val(), $('#SerialId').val(), "PackageId", "PackageId");
    }

    var loadPackagesList = function (colorId, serialId, selectId, selectName) {
        var params = {
            colorId: colorId,
            serialId: serialId,
            selectId: selectId,
            selectName: selectName
        };

        var number = parseInt($('#Number').val()[0]);

        if (!isNaN(number))
            params.number = number;

        $.get("/Ticket/GetPackageSelectPartial", params, function (data) {
            $('#PackageId').replaceWith(data);
        });
    }
})