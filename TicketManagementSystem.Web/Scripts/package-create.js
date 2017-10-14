function OnBegin() {
    $("#create-result").html("");
    setButtonLoadingState();
}

function OnSuccess(data) {
    $('form[id = form0]').trigger('reset');
}

function OnComplete(request, status) {
    resetButtonLoadingState();
}
