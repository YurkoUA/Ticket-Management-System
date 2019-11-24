$(document).ready(function () {
    convertDate();

    $(document).off("container.fluid").on("container.fluid", function () {
        $("div.container").addClass("container-fluid");
        $("div.container").removeClass("container");
    });
});

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
    $('#' + inputName).val(new Date().toLocaleDateString('uk-UA'));
}

function setButtonLoadingState() {
    $(".btn-loading").button('loading');
}

function resetButtonLoadingState() {
    $(".btn-loading").button('reset');
}

function changeUrlParams(newParams) {
    var params = $.deparam.querystring(location.search);

    if (params != undefined) {
        for (var i in newParams) {
            params[newParams[i].name] = newParams[i].value;
        }
        
        history.pushState(null, '', location.pathname + "?" + $.param(params));
    }
}
