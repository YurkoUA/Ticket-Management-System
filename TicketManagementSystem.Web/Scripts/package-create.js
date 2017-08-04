function OnBegin() {
    $("#create-result").html("");
}

function OnSuccess(data) {
    $('form[id = form0]').trigger('reset');
}