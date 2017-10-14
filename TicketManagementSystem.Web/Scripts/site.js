﻿convertDate();

function showModal(data) {
    $('#modal-window-content').html(data);
    $('#modal-window').modal('show');
    convertDate();
}

function convertDate() {
    $('[data-date]').each(function () {
        var localDate = new Date(parseInt($(this).attr('data-date')));
        $(this).html(localDate.toLocaleString());
    });
}

function putCurrentDate(inputName) {
    $('#' + inputName).val(new Date().toLocaleDateString());
}

function setButtonLoadingState() {
    $(".btn-loading").button('loading');
}

function resetButtonLoadingState() {
    $(".btn-loading").button('reset');
}
