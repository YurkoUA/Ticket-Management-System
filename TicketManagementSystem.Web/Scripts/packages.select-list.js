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
        loadPackagesList($('#ColorId').val(), $('#SerialId').val());
    }

    var loadPackagesList = function (colorId, serialId) {
        var params = {
            colorId: colorId,
            serialId: serialId,
            firstNumber: null
        };

        var number = parseInt($('#Number').val()[0]);

        if (!isNaN(number))
            params.firstNumber = number;

        $.get("/api/Package/GetCompatiblePackages", params, function (data) {
            $("#PackageId").html("");

            $("#PackageId").append("<option value>------</option>");

            if (data !== undefined) {
                for (var i in data) {
                    var value = data[i].Id;
                    var name = data[i].Name;

                    if (data[i].TicketsCount > 0) {
                        name += ": " + data[i].TicketsCount + " шт.";
                    }

                    $("#PackageId").append("<option value=\"" + value + "\">" + name + "</option>");
                }
            }

            console.log("Packages loaded successfull.")
        });
    }
})