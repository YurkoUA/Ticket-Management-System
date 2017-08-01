function packagesByColorModal(colorId) {
    var url = "/Color/GetPackages";

    loadPackages(url, colorId);
}

function packagesBySerialModal(serialId) {
    var url = "/Serial/GetPackages";

    loadPackages(url, serialId);
}

function loadPackages(url, id) {
    $.get(url, { id: id }, function (data) {
        showModal(data);
    });
}