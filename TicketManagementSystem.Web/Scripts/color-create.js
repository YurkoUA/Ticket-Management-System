function OnBegin() {
    $("#create-result").html("");
    setButtonLoadingState();
}

function OnComplete(request, status) {
    $('form[id = form0]').trigger('reset');
    resetButtonLoadingState();
}
