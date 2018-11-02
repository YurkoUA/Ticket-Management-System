function OnBegin() {
    onToolbarSelectionChanged();
    setButtonLoadingState();
}

function onToolbarEventSuccess(data) {
    $("#Date").datepicker({
        orientation: 'bottom'
    });
}

// For delete action.
function onDeletingSuccess(data) {
    $('#ticket-toolbar').html('');
}

function OnComplete(request, status) {
    resetButtonLoadingState();
}

function onToolbarSelectionChanged() {
    $('#ticket-result').html('');
}
