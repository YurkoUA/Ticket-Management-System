function OnBegin() {
    onToolbarSelectionChanged();
    setButtonLoadingState();
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
