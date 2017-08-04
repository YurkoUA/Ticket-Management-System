function OnBegin() {
    $('#create-result').html("");
}

function OnSuccess(data) {
    $('#Number').val('');
    $('#SerialNumber').val('');
    $('#Note').val('');
    $('#Date').val('');

    $('#Number').focus();
}
