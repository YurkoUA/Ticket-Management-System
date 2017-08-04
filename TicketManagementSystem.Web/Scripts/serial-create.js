function OnBegin() {
    $("#create-result").html("");
}

function OnComplete(request, status) {
    $('form[id = form0]').trigger('reset');
}
