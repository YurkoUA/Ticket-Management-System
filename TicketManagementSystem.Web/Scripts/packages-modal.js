$(function () {
    $('#packages-by-serial').click(function () {
        var id = $('#Id').val();
        var url = "/Serial/GetPackages";

        loadPackages(url, id);
    });

    $('#packages-by-color').click(function () {
        var id = $('#Id').val();
        var url = "/Color/GetPackages";

        loadPackages(url, id);
    });

    var loadPackages = function (url, id) {
        $.get(url, { id: id }, function (data) {
            showModal(data);
        });
    }
})